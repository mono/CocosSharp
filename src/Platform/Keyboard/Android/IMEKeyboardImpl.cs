
#region Using clause
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;
using System.Threading.Tasks;

#endregion Using clause


namespace CocosSharp
{
    /// <summary>
    /// IME keyboard implementaion for Android.  This class uses an AlerDialog.Builder to create an AlertDialog to be 
    /// presented for input.
    /// </summary>
    public class IMEKeyboardImpl : ICCIMEDelegate
    {
        private bool isVisible;
        private string contentText;

        private static IMEKeyboardImpl instance;

        public CCTextField TextFieldInFocus { get; set; }

        /// <summary>
        /// Returns a shared instance of the platform keyboard implemenation
        /// </summary>
        /// <value>The shared instance.</value>
        public static IMEKeyboardImpl SharedInstance
        {
            get
            {
                if (instance == null)
                    instance = new IMEKeyboardImpl();

                return instance;
            }
        }

        // Based on MonoGame's Guide implementation for Android
        AlertDialog alertDialog = null;
        public string ShowKeyboardInput(string defaultText)
        {            
            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

            var kbi = new CCIMEKeyboardNotificationInfo();

            OnKeyboardWillShow();

            IsVisible = true;

            var context = Android.App.Application.Context;

            var alert = new AlertDialog.Builder(context);

            var input = new EditText(context) { Text = defaultText };
            if (defaultText != null)
            {
                input.SetSelection(defaultText.Length);
            }
            alert.SetView(input);

            alert.SetPositiveButton("Ok", (dialog, whichButton) =>
            {
                ContentText = input.Text;
                waitHandle.Set();
                IsVisible = false;
                OnKeyboardWillHide();
            });

            alert.SetNegativeButton("Cancel", (dialog, whichButton) =>
            {
                ContentText = null;
                waitHandle.Set();
                IsVisible = false;
                OnKeyboardWillHide();
            });
            alert.SetCancelable(false);

            alertDialog = alert.Create();
            alertDialog.Show();
            OnKeyboardDidShow();

            waitHandle.WaitOne();
            
            if (alertDialog != null)
            {
                alertDialog.Dismiss();
                alertDialog.Dispose();
                alertDialog = null;
            }

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

        public async Task AttachWithIMEAsync(string title, string defaultText, string text)
        {
            contentText = await ShowKeyboardAsync(ContentText);
        }

        public bool AttachWithIME()
        {
            AttachWithIMEAsync(string.Empty, string.Empty, string.Empty);
            return true;
        }

        public bool DetachWithIME()
        {
            if (CanDetachWithIME())
            {
                if (alertDialog != null && alertDialog.IsShowing)
                {
                    alertDialog.Dismiss();
                    alertDialog.Dispose();
                    alertDialog = null;
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
            if (IsVisible && alertDialog != null && alertDialog.IsShowing)
                return true;

            return false;
        }

        public bool DidDetachWithIME()
        {
            if (!IsVisible && alertDialog == null)
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

}