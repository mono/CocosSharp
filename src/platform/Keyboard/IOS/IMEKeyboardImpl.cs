
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
    /// IME keyboard implementaion for iOS.  Creates an instance of a custom class HiddenInput which extends UITextField
    /// </summary>
    internal class IMEKeyboardImpl : ICCIMEDelegate
    {
        private bool isVisible;
        private string contentText;

        private static IMEKeyboardImpl instance;

        public CCTextField TextFieldInFocus { get; set; }

        private static UIViewController gameViewController;
        private static UIWindow keyWindow;
        static NSObject keyboardWillShowObserver, keyboardWillHideObserver, keyboardDidShowObserver, keyboardDidHideObserver;

        static HiddenInput hiddenKeyInput;

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

        private string ShowKeyboardInput()
        {

            UIApplication.SharedApplication.InvokeOnMainThread (delegate {

                AddObservers();

                // Create an instance of our custom UITextField that will be added to our view but hidden
                hiddenKeyInput = new HiddenInput(new CGRect(0,0,1,1));
                hiddenKeyInput.Hidden = true;

                hiddenKeyInput.AutocorrectionType = UITextAutocorrectionType.No;
                hiddenKeyInput.SpellCheckingType = UITextSpellCheckingType.No;


                if (TextFieldInFocus.CharacterCount > 0)
                    hiddenKeyInput.Text = TextFieldInFocus.Text;

                //hiddenKeyInput.Delegate = this;

                gameViewController.Add(hiddenKeyInput);

                hiddenKeyInput.BecomeFirstResponder();

            }
            );
            return contentText;
        }

        void AddObservers()
        {
            keyboardWillShowObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, 
                (notification) => 
                {

                    // In iOS when there is a change of keyboard the WillShow and DidShow events are fired again without
                    // the HideXXXX events.
                    // If we are already visible then this is probably a change of keyboard so we will
                    // not attach our event handlers again or we will get multiple events on key input.
                    if (!IsVisible)
                    {
                        hiddenKeyInput.DeleteBackwardEvent += OnDeleteBackwardEvent;
                        hiddenKeyInput.InsertTextEvent += OnInsertTextEvent;
                    }

                    OnKeyboardWillShow();


                }
            );

            keyboardDidShowObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, 
                (notification) => 
                {
                    IsVisible = true;
                    OnKeyboardDidShow();
                }
            );

            keyboardWillHideObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, 
                (notification) => 
                {
                    UIApplication.EnsureUIThread();
                    OnKeyboardWillHide();
                }
            );

            keyboardDidHideObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, 
                (notification) => 
                {
                    UIApplication.EnsureUIThread();

                    // Remove our hiddenKeyInput event listeners.
                    hiddenKeyInput.DeleteBackwardEvent -= OnDeleteBackwardEvent;
                    hiddenKeyInput.InsertTextEvent -= OnInsertTextEvent;

                    // Remove it from it's view
                    hiddenKeyInput.RemoveFromSuperview();

                    hiddenKeyInput = null;

                    // Remove the notifications
                    NSNotificationCenter.DefaultCenter.RemoveObserver(keyboardWillShowObserver);
                    NSNotificationCenter.DefaultCenter.RemoveObserver(keyboardDidShowObserver);
                    NSNotificationCenter.DefaultCenter.RemoveObserver(keyboardWillHideObserver);
                    NSNotificationCenter.DefaultCenter.RemoveObserver(keyboardDidHideObserver);


                    IsVisible = false;
                    OnKeyboardDidHide();
                }
            );
        }
        void OnDeleteBackwardEvent (object sender, EventArgs e)
        {
            OnDeleteBackward ();
        }

        void OnInsertTextEvent (object sender, string text)
        {
            OnInsertText (new CCIMEKeybardEventArgs (text, 1));
        }

        public async Task ShowKeyboardAsync()
        {

            ShowKeyboardInput ();

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

        public bool AttachWithIME()
        {
            ShowKeyboardAsync ();
            return true;
        }

        public bool DetachWithIME()
        {
            if (CanDetachWithIME())
            {
                if (hiddenKeyInput != null)
                {
                    hiddenKeyInput.ResignFirstResponder();
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
            if (IsVisible && hiddenKeyInput != null)
                return true;

            return false;
        }

        public bool DidDetachWithIME()
        {
            if (!IsVisible && hiddenKeyInput == null)
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

        #region TextField Delegate


        private class HiddenInput : UITextField
        {

            public event EventHandler<string> InsertTextEvent;

            void OnInsertText(string text)
            {
                var handler = InsertTextEvent;
                if (handler != null)
                {
                    handler (this, text);
                }
            }

            public event EventHandler DeleteBackwardEvent;

            void OnDeleteBackward()
            {
                var handler = DeleteBackwardEvent;
                if (handler != null)
                {
                    handler (this, EventArgs.Empty);
                }

            }

            public HiddenInput (CGRect frame) : base(frame)
            {}

            public override void InsertText (string text)
            {
                base.InsertText (text);
                OnInsertText (text);
            }

            public override void DeleteBackward ()
            {
                base.DeleteBackward ();
                OnDeleteBackward ();
            }
        }

        #endregion

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