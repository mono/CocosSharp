
#region Using clause
using System;
using System.Threading.Tasks;

using Windows.UI.Core;
using Windows.Foundation;
using Windows.Phone.UI.Input;
using Windows.System;

#endregion Using clause


namespace CocosSharp
{
    /// <summary>
    /// IME keyboard implementaion for Windows.  
    /// 
    /// https://msdn.microsoft.com/en-us/library/windows/apps/jj247546(v=vs.105).aspx
    /// https://code.msdn.microsoft.com/windowsapps/Touch-keyboard-sample-43532fda#content
    /// </summary>
    internal class IMEKeyboardImpl : ICCIMEDelegate
    {
        private bool isVisible;
        private string contentText;

        private static IMEKeyboardImpl instance;

        public CCTextField TextFieldInFocus { get; set; }

        private static CoreDispatcher dispatcher;

        static Windows.UI.Xaml.Controls.TextBox hiddenKeyInput;

        static Windows.UI.ViewManagement.InputPane keyboard;

        bool AutoRepeat { get; set; }

        /// <summary>
        /// Returns a shared instance of the platform keyboard implemenation
        /// </summary>
        /// <value>The shared instance.</value>
        public static IMEKeyboardImpl SharedInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IMEKeyboardImpl();

                    // Obtain the CoreWindow of the current thread because we are going to need
                    // to execute all of this on the UI thread
                    var coreWindow = CoreWindow.GetForCurrentThread();

                    // Dispatcher needed to run on UI Thread
                    dispatcher = coreWindow.Dispatcher;

                    // Obtain the input pane for our current view
                    keyboard = Windows.UI.ViewManagement.InputPane.GetForCurrentView();
                }

                return instance;
            }
        }

        public bool ShowKeyboardInput()
        {

            keyboard.Hiding += OnSofKeyboardHiding;
            keyboard.Showing += OnSofKeyboardShowing;

            // RunAsync all of the UI info.
            dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {

                    // Create a TextBox that will be added to our view but hidden
                    hiddenKeyInput = new Windows.UI.Xaml.Controls.TextBox();
                    hiddenKeyInput.Opacity = 0;

                    // We will only be generating one character at a time and does not
                    // matter about the font.  All we need is the text.
                    hiddenKeyInput.Width = 1;
                    hiddenKeyInput.Height = 1;
                    hiddenKeyInput.MaxWidth = 1;

                    // Create an input scope for us.  Probably not needed
                    var scope = new Windows.UI.Xaml.Input.InputScope();
                    var name = new Windows.UI.Xaml.Input.InputScopeName();

                    name.NameValue = Windows.UI.Xaml.Input.InputScopeNameValue.Default;
                    scope.Names.Add(name);

                    hiddenKeyInput.InputScope = scope;

                    // Set spell checking off.
                    hiddenKeyInput.IsSpellCheckEnabled = false;

                    // Get our Current SwapChainPanel.  This is for our WP8.1 implementation
                    var content = Windows.UI.Xaml.Window.Current.Content as CCGameView;
                    if (content == null)
                    {
                        // Do our detach stuff
                        DetachWithIME();
                        return;
                    }

                    // Add our hidden TextBox to our panel
                    content.Children.Add(hiddenKeyInput);
                    
                    // Hook up our key delegates
                    hiddenKeyInput.KeyDown += OnKeyDown;
                    hiddenKeyInput.KeyUp += OnKeyUp;

                    // enable us and set focus
                    hiddenKeyInput.IsEnabled = true;
                    hiddenKeyInput.Focus(Windows.UI.Xaml.FocusState.Programmatic);

                    // Show the soft keyboard if possible
                    var didShow = keyboard.TryShow();
                    if (!didShow)
                    { 
                        DetachWithIME();
                    }

                    //CCLog.Log("we be in background");
            }
                );

            return true;
        }

        private void OnSofKeyboardShowing(Windows.UI.ViewManagement.InputPane sender, Windows.UI.ViewManagement.InputPaneVisibilityEventArgs args)
        {
            OnKeyboardWillShow();
            IsVisible = true;
            OnKeyboardDidShow();
        }

        private void OnSofKeyboardHiding(Windows.UI.ViewManagement.InputPane sender, Windows.UI.ViewManagement.InputPaneVisibilityEventArgs args)
        {
            OnKeyboardWillHide();

            IsVisible = false;

            // Get our Current SwapChainPanel
            var content = Windows.UI.Xaml.Window.Current.Content as Windows.UI.Xaml.Controls.SwapChainBackgroundPanel;
            if (content == null)
            {
                // Do our detach stuff
                return;
            }

            hiddenKeyInput.KeyDown -= OnKeyDown;
            hiddenKeyInput.KeyUp -= OnKeyUp;

            // enable us
            hiddenKeyInput.IsEnabled = false;
            hiddenKeyInput.Focus(Windows.UI.Xaml.FocusState.Pointer);

            content.Children.Remove(hiddenKeyInput);

            hiddenKeyInput = null;

            keyboard.Hiding -= OnSofKeyboardHiding;

            OnKeyboardDidHide();
        }

        private void OnKeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            //CCLog.Log("Key Up" + e.Key.ToString());
            var textBox = sender as Windows.UI.Xaml.Controls.TextBox;
            switch (e.Key)
            {
                // We can not seem to capture the escape key so we rely on Keyboard Hide
                //case VirtualKey.Escape:
                //    e.Handled = true;
                //    IsVisible = false;
                //    OnKeyboardWillHide();
                //    OnKeyboardDidHide();
                //    break;
                case VirtualKey.Back:
                    OnDeleteBackward();
                    break;
                case VirtualKey.Enter:
                    OnInsertText(new CCIMEKeybardEventArgs("\n", 1));
                    break;
                default:
                    var text = textBox.Text;
                    if (!string.IsNullOrEmpty(text))
                    {
                        var charToSend = text.Substring(text.Length - 1);
                        OnInsertText(new CCIMEKeybardEventArgs(charToSend, 1));
                    }
                    break;

            }
            textBox.Text = string.Empty;
        }

        private void OnKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            //CCLog.Log("Key Down" + e.Key.ToString());
            //var textBox = sender as Windows.UI.Xaml.Controls.TextBox;
            //switch (e.Key)
            //{
            //    case VirtualKey.Escape:
            //        e.Handled = true;
            //        IsVisible = false;
            //        OnKeyboardWillHide();
            //        OnKeyboardDidHide();
            //        break;
            //}
            //textBox.Text = string.Empty;
        }
        #region Properties

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            internal set
            {
                isVisible = value;
            }
        }

        #endregion

        #region IMEDelegate implementation

        public bool AttachWithIME()
        {
            return ShowKeyboardInput();
        }

        public bool DetachWithIME()
        {
            if (CanDetachWithIME())
            {
                if (IsVisible)
                {

                    // Show the soft keyboard
                    var didHide = keyboard.TryHide();

                    keyboard.Showing -= OnSofKeyboardShowing;

                }
            }
            return true;
        }

        public bool CanAttachWithIME()
        {
            if (!IsVisible)
                return true;

            return false;
        }

        public bool DidAttachWithIME()
        {
 
            return IsVisible;
        }

        public bool CanDetachWithIME()
        {
            if (IsVisible)
                return true;

            return false;
        }

        public bool DidDetachWithIME()
        {
            if (!IsVisible)
            {
                TextFieldInFocus = null;
                return true;
            }

            return false;
        }

        public event EventHandler<CCIMEKeybardEventArgs> InsertText;

        bool OnInsertText(CCIMEKeybardEventArgs eventArgs)
        {
            var handler = InsertText;
            if (handler != null)
            {
                return ProcessCancelableEvent(handler, eventArgs);
            }

            return false;
        }

        public event EventHandler<CCIMEKeybardEventArgs> ReplaceText;

        bool OnReplaceText(CCIMEKeybardEventArgs eventArgs)
        {
            var handler = ReplaceText;
            if (handler != null)
            {
                return ProcessCancelableEvent(handler, eventArgs);
            }

            return false;
        }


        public event EventHandler<CCIMEKeybardEventArgs> DeleteBackward;

        bool OnDeleteBackward()
        {
            var handler = DeleteBackward;
            if (handler != null)
            {
                return ProcessCancelableEvent (handler, new CCIMEKeybardEventArgs(string.Empty, 0));
            }

            return false;
        }

        private bool ProcessCancelableEvent(EventHandler<CCIMEKeybardEventArgs> handler, CCIMEKeybardEventArgs eventArgs)
        {
            var canceled = false;
            Delegate inFocusDelegate = null;
            var sender = TextFieldInFocus;
            foreach (var instantHandler in handler.GetInvocationList())
            {
                if (eventArgs.Cancel)
                {
                    break;
                }

                // Make sure we process all event handlers except for our focused text field
                // We need to process it at the end to give the other event handlers a chance 
                // to cancel the event from propogating to our focused text field.
                if (instantHandler.Target == sender)
                    inFocusDelegate = instantHandler;
                else
                    instantHandler.DynamicInvoke(sender, eventArgs);
            }

            canceled = eventArgs.Cancel;

            if (inFocusDelegate != null && !canceled)
                inFocusDelegate.DynamicInvoke(sender, eventArgs);

            return canceled;
        }

        public string ContentText
        {
            get
            {
                return contentText;
            }
            set
            {
                contentText = value;
            }
        }

        #region keyboard show/hide notification

        public event EventHandler<CCIMEKeyboardNotificationInfo> KeyboardWillShow;

        void OnKeyboardWillShow()
        {
            var handler = KeyboardWillShow;
            if (handler != null)
                handler(this, new CCIMEKeyboardNotificationInfo());
        }

        public event EventHandler<CCIMEKeyboardNotificationInfo> KeyboardDidShow;

        void OnKeyboardDidShow()
        {
            var handler = KeyboardDidShow;
            if (handler != null)
                handler(this, new CCIMEKeyboardNotificationInfo());
        }

        public event EventHandler<CCIMEKeyboardNotificationInfo> KeyboardWillHide;

        void OnKeyboardWillHide()
        {
            var handler = KeyboardWillHide;
            if (handler != null)
                handler(this, new CCIMEKeyboardNotificationInfo());
        }

        public event EventHandler<CCIMEKeyboardNotificationInfo> KeyboardDidHide;

        void OnKeyboardDidHide()
        {
            var handler = KeyboardDidHide;
            if (handler != null)
                handler(this, new CCIMEKeyboardNotificationInfo());
        }
        #endregion
    }
    #endregion

}