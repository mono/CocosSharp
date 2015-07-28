#region License
/*
Microsoft Public License (Ms-PL)
MonoGame - Copyright © 2009 The MonoGame Team

All rights reserved.

This license governs use of the accompanying software. If you use the software, you accept this license. If you do not
accept the license, do not use the software.

1. Definitions
The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under 
U.S. copyright law.

A "contribution" is the original software, or any additions or changes to the software.
A "contributor" is any person that distributes its contribution under this license.
"Licensed patents" are a contributor's patent claims that read directly on its contribution.

2. Grant of Rights
(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.

3. Conditions and Limitations
(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, 
your patent license from such contributor to the software ends automatically.
(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution 
notices that are present in the software.
(D) If you distribute any portion of the software in source code form, you may do so only under this license by including 
a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object 
code form, you may only do so under a license that complies with this license.
(E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees
or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent
permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular
purpose and non-infringement.
*/
#endregion License

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

    public class IMEKeyboardImpl : ICCIMEDelegate
    {
        private bool isVisible;
        private string contentText;

        private static IMEKeyboardImpl instance;

        public static IMEKeyboardImpl SharedInstance
        {
            get
            {
                if (instance == null)
                    instance = new IMEKeyboardImpl();

                return instance;
            }
        }

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

        //////////////////////////////////////////////////////////////////////////
        // keyboard show/hide notification
        //////////////////////////////////////////////////////////////////////////

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
    }
    #endregion

}