
#region Using clause
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;
using System.Drawing;

using Foundation;
using GameKit;
using UIKit;
using CoreGraphics;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input.Touch;

using Microsoft.Xna.Framework;

#endregion Using clause


namespace CocosSharp
{
    /// <summary>
    /// IME keyboard implementaion for iOS.  This class uses a custom layout built by KeyboardInputViewController with a toolbar
    /// presenting Cancel and Done buttons.
    /// </summary>
    internal class IMEKeyboardImpl : ICCIMEDelegate
    {
        private bool isVisible;
        private string contentText;

        private static IMEKeyboardImpl instance;

        public CCTextField TextFieldInFocus { get; set; }

        private static KeyboardInputViewController keyboardViewController;
        private static UIViewController gameViewController;
        private static UIWindow keyWindow;

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
                    instance = new IMEKeyboardImpl ();
                    keyWindow = UIApplication.SharedApplication.KeyWindow;
                    if (keyWindow == null)
                        throw new InvalidOperationException(
                            "iOSGamePlatform must add the main UIWindow to Game.Services");

                    gameViewController = keyWindow.RootViewController;
                    if (gameViewController == null)
                        throw new InvalidOperationException(
                            "iOSGamePlatform must add the game UIViewController to Game.Services");
                }

                return instance;
            }
        }

        // Based on MonoGame's Guide implementation for iOS
        private string ShowKeyboardInput(
            string defaultText)
        {

            OnKeyboardWillShow();

            IsVisible = true;
            EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

            keyboardViewController = new KeyboardInputViewController(
                defaultText, gameViewController);

            UIApplication.SharedApplication.InvokeOnMainThread(delegate
                {
                    gameViewController.PresentViewController(keyboardViewController, true, null);

                    keyboardViewController.View.InputAccepted += (sender, e) =>
                    {
                        gameViewController.DismissViewController(true, null);
                        ContentText = keyboardViewController.View.Text;
                        waitHandle.Set();
                        OnKeyboardWillHide();
                    };

                    keyboardViewController.View.InputCanceled += (sender, e) =>
                    {
                        ContentText = null;
                        gameViewController.DismissViewController(true, null);
                        waitHandle.Set();
                        OnKeyboardWillHide();
                    };
                    OnKeyboardDidShow();
                });
            waitHandle.WaitOne();

            OnReplaceText(new CCIMEKeybardEventArgs(contentText, contentText.Length));
            IsVisible = false;
            return contentText;
        }

        public async Task<string> ShowKeyboardAsync(
            string defaultText)
        {

            var tcs = new TaskCompletionSource<string>(defaultText);
            var task = Task.Run<string>(
                () => ShowKeyboardInput(defaultText)
            ); 
            await task.ContinueWith(t =>
                {
                    // Copy the task result into the returned task.
                    if (t.IsFaulted)
                        tcs.TrySetException(t.Exception.InnerExceptions);
                    else if (t.IsCanceled)
                        tcs.TrySetCanceled();
                    else
                        tcs.TrySetResult(t.Result);
                });

            IsVisible = false;
            OnKeyboardDidHide();

            return task.Result;

        }

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is visible.
        /// </summary>
        /// <value><c>true</c> if this instance is visible; otherwise, <c>false</c>.</value>
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

        public async Task AttachWithIMEAsync()
        {
            contentText = await ShowKeyboardAsync(ContentText);
        }

        public bool AttachWithIME()
        {
            AttachWithIMEAsync();
            return true;
        }

        public bool DetachWithIME()
        {
            if (CanDetachWithIME())
            {
                if (keyboardViewController != null)
                {
                    keyboardViewController = null;
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
            if (IsVisible && keyboardViewController != null)
                return true;

            return false;
        }

        public bool DidDetachWithIME()
        {
            if (!IsVisible && keyboardViewController == null)
                return true;

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
                return ProcessCancelableEvent(handler, new CCIMEKeybardEventArgs(string.Empty, 0));
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

    class KeyboardInputView : UIScrollView {

        private static readonly PaddingF TextFieldMargin = new PaddingF (10, 5, 10, 5);

        private readonly UIToolbar _toolbar;
        private readonly UITextField _textField;
        private readonly UIScrollView _textFieldContainer;

        public KeyboardInputView (RectangleF frame)
            : base(frame)
        {
            _toolbar = new UIToolbar (frame);

            var toolbarItems = new UIBarButtonItem[] {
                new UIBarButtonItem (UIBarButtonSystemItem.Cancel, CancelButton_Tapped),
                new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace, null),
                new UIBarButtonItem (UIBarButtonSystemItem.Done, DoneButton_Tapped)
            };

            _toolbar.SetItems (toolbarItems, false);
            _toolbar.SizeToFit ();

            _textFieldContainer = new UIScrollView(new RectangleF(0, 0, 100, 100));

            _textField = new UITextField (_textFieldContainer.Bounds);
            _textField.AutoresizingMask =
                UIViewAutoresizing.FlexibleWidth |
                UIViewAutoresizing.FlexibleHeight;
            _textField.BorderStyle = UITextBorderStyle.RoundedRect;
            _textField.Delegate = new TextFieldDelegate (this);

            _textFieldContainer.Add (_textField);

            Add (_toolbar);
            Add (_textFieldContainer);

            AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            AutosizesSubviews = false;
            Opaque = true;
            BackgroundColor = UIColor.FromRGB (0xC5, 0xCC, 0xD4);

            SetNeedsLayout ();
        }

        #region Properties

        public string Text {
            get { return _textField.Text; }
            set {
                if (_textField.Text != value) {
                    _textField.Text = value;
                }
            }
        }

        #endregion Properties

        #region Events

        public event EventHandler<EventArgs> InputAccepted;
        public event EventHandler<EventArgs> InputCanceled;

        #endregion Events

        public void ActivateFirstField ()
        {
            _textField.BecomeFirstResponder ();
        }

        public void ScrollActiveFieldToVisible ()
        {
            if (!_textField.IsFirstResponder)
                return;

            var bounds = Bounds;
            bounds.X += ContentInset.Left;
            bounds.Width -= (ContentInset.Left + ContentInset.Right);
            bounds.Y += ContentInset.Top;
            bounds.Height -= (ContentInset.Top + ContentInset.Bottom);

            if (!bounds.Contains(_textFieldContainer.Frame)) {
                ScrollRectToVisible (_textFieldContainer.Frame, true);
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            _textField.ResignFirstResponder ();
        }

        public override void LayoutSubviews ()
        {
            _toolbar.SizeToFit ();

            var textFieldSize = _textField.SizeThatFits (
                new CGSize(Bounds.Width - TextFieldMargin.Horizontal, Bounds.Height));
            _textFieldContainer.Frame = new CGRect (
                TextFieldMargin.Left, _toolbar.Bounds.Bottom + TextFieldMargin.Top,
                Bounds.Width - TextFieldMargin.Horizontal, textFieldSize.Height);

            ContentSize = new CGSize(Bounds.Width, _textFieldContainer.Frame.Bottom + TextFieldMargin.Bottom);
        }

        private static CGSize SizeThatFitsWidth(UILabel label, nfloat width)
        {
            var font = label.Font;
            return label.SizeThatFits(new CGSize(width, font.LineHeight * label.Lines));
        }

        private void DoneButton_Tapped (object sender, EventArgs e)
        {
            OnInputAccepted (e);
        }

        private void CancelButton_Tapped (object sender, EventArgs e)
        {
            OnInputCanceled (e);
        }

        private void OnInputAccepted(EventArgs e)
        {
            var handler = InputAccepted;
            if (handler != null)
                handler (this, e);
        }

        private void OnInputCanceled(EventArgs e)
        {
            var handler = InputCanceled;
            if (handler != null)
                handler (this, e);
        }

        private class TextFieldDelegate : UITextFieldDelegate
        {
            private readonly KeyboardInputView _owner;
            public TextFieldDelegate (KeyboardInputView owner)
            {
                if (owner == null)
                    throw new ArgumentNullException ("owner");
                _owner = owner;
            }

            public override bool ShouldReturn(UITextField textField)
            {
                _owner.OnInputAccepted (EventArgs.Empty);
                return true;
            }
        }
    }

    class KeyboardInputViewController : UIViewController {
        private readonly string _defaultText;
        private UIViewController _gameViewController;

        public KeyboardInputViewController (
            string defaultText, UIViewController gameViewController)
        {
            _defaultText = defaultText;
            _gameViewController = gameViewController;
        }

        private readonly List<NSObject> _keyboardObservers = new List<NSObject> ();
        public override void LoadView ()
        {
            var view = new KeyboardInputView (new RectangleF (0, 0, 240, 320));
            view.Text = _defaultText;

            view.ActivateFirstField ();

            base.View = view;

            _keyboardObservers.Add (
                NSNotificationCenter.DefaultCenter.AddObserver(
                    UIKeyboard.DidShowNotification, Keyboard_DidShow));
            _keyboardObservers.Add (
                NSNotificationCenter.DefaultCenter.AddObserver(
                    UIKeyboard.WillHideNotification, Keyboard_WillHide));
        }

        public new KeyboardInputView View {
            get { return (KeyboardInputView) base.View; }
        }

        [Obsolete]
        public override void ViewDidUnload ()
        {
            base.ViewDidUnload ();

            NSNotificationCenter.DefaultCenter.RemoveObservers (_keyboardObservers);
            _keyboardObservers.Clear ();

            _gameViewController = null;
        }

        private void Keyboard_DidShow(NSNotification notification)
        {
            var keyboardSize = UIKeyboard.FrameBeginFromNotification (notification).Size;

            if (InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft ||
                InterfaceOrientation == UIInterfaceOrientation.LandscapeRight)
            {
                var tmpkeyboardSize = keyboardSize;
                keyboardSize.Width = (nfloat)Math.Max(tmpkeyboardSize.Height, tmpkeyboardSize.Width);
                keyboardSize.Height = (nfloat)Math.Min(tmpkeyboardSize.Height, tmpkeyboardSize.Width);
            }

            var view = (KeyboardInputView)View;
            var contentInsets = new UIEdgeInsets(0f, 0f, keyboardSize.Height, 0f);
            view.ContentInset = contentInsets;
            view.ScrollIndicatorInsets = contentInsets;

            view.ScrollActiveFieldToVisible ();
        }

        private void Keyboard_WillHide(NSNotification notification)
        {
            var view = (KeyboardInputView)View;
            view.ContentInset = UIEdgeInsets.Zero;
            view.ScrollIndicatorInsets = UIEdgeInsets.Zero;
        }

        #region Autorotation for iOS 5 or older
        [Obsolete]
        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
//            var requestedOrientation = OrientationConverter.ToDisplayOrientation(toInterfaceOrientation);
//            var supportedOrientations = _gameViewController.GetSupportedInterfaceOrientations ();
//            return (supportedOrientations & requestedOrientation) != 0;
            return true;
        }
        #endregion

        #region Autorotation for iOS 6 or newer
        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
        {
            return _gameViewController.GetSupportedInterfaceOrientations ();
        }

        public override bool ShouldAutorotate ()
        {
            return true;
        }

        public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation ()
        {
            return _gameViewController.PreferredInterfaceOrientationForPresentation();
        }
        #endregion

        public override void WillRotate(UIInterfaceOrientation toInterfaceOrientation, double duration)
        {
            base.WillRotate(toInterfaceOrientation, duration);
            View.LayoutSubviews ();
        }
    }

    struct PaddingF {
        public float Left;
        public float Top;
        public float Right;
        public float Bottom;

        public float Horizontal {
            get { return Left + Right; }
        }

        public float Vertical {
            get { return Top + Bottom; }
        }

        public PaddingF (float all)
        {
            Left = Top = Right = Bottom = all;
        }

        public PaddingF (float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }

}