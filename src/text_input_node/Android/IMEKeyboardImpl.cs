
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
    internal class IMEKeyboardImpl : ICCIMEDelegate
    {
        private bool isVisible;
        private string contentText;

        private static IMEKeyboardImpl instance;

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
        public string ShowKeyboardInput(
         string defaultText)
        {
            
            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

            var kbi = new CCIMEKeyboardNotificationInfo();

            OnKeyboardWillShow();

            IsVisible = true;

            

            CCGame.Activity.RunOnUiThread(() =>
            {
                var alert = new AlertDialog.Builder(Game.Activity);

                var input = new EditText(Game.Activity) { Text = defaultText };
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

            });
            waitHandle.WaitOne();

            if (alertDialog != null)
            {
                alertDialog.Dismiss();
                alertDialog.Dispose();
                alertDialog = null;
            }

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

        [CLSCompliant(false)]
        public AndroidGameWindow Window
        {
            get;
            set;
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

        //friend class CCIMEDispatcher;

        /**
        @brief	Decide the delegate instance is ready for receive ime message or not.

        Called by CCIMEDispatcher.
        */
        public bool CanAttachWithIME()
        {
            if (!IsVisible)
                return true;

            return false;
        }
        /**
        @brief	When the delegate detach with IME, this method call by CCIMEDispatcher.
        */
        public bool DidAttachWithIME()
        {
            return IsVisible;
        }

        /**
        @brief	Decide the delegate instance can stop receive ime message or not.
        */
        public bool CanDetachWithIME()
        {
            if (IsVisible && alertDialog != null && alertDialog.IsShowing)
                return true;

            return false;
        }

        /**
        @brief	When the delegate detach with IME, this method call by CCIMEDispatcher.
        */
        public bool DidDetachWithIME()
        {
            if (!IsVisible && alertDialog == null)
                return true;

            return false;
        }

        /**
        @brief	Called by CCIMEDispatcher when some text input from IME.
        */
        public void InsertText(string text, int len)
        {

        }

        /**
        @brief	Called by CCIMEDispatcher when user clicked the backward key.
        */
        public void DeleteBackward()
        {

        }

        /**
        @brief	Called by CCIMEDispatcher for get text which delegate already has.
        */
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