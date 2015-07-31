
#region Using clause
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

#endregion Using clause


namespace CocosSharp
{
    /// <summary>
    /// IME keyboard implementaion for Windows.  
    /// </summary>
    internal class IMEKeyboardImpl : ICCIMEDelegate
    {
        private bool isVisible;
        private string contentText;

        private static IMEKeyboardImpl instance;

        public CCTextField TextFieldInFocus { get; set; }

        CCEventListenerKeyboard keyboardListener;
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
                    instance = new IMEKeyboardImpl();

                return instance;
            }
        }

        public string ShowKeyboardInput(
            string defaultText)
        {

            OnKeyboardWillShow();

            IsVisible = true;

            OnKeyboardDidShow();

            return contentText;
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
            ShowKeyboardInput(ContentText);
            if (true)
            {
                if (TextFieldInFocus != null)
                {
                    AutoRepeat = TextFieldInFocus.AutoRepeat;
                    keyboardListener = new CCEventListenerKeyboard();
                    keyboardListener.OnKeyPressed = OnKeyPressed;
                    keyboardListener.OnKeyReleased = OnKeyReleased;

                    TextFieldInFocus.AddEventListener(keyboardListener);
                }
            }
            return true;
        }

        public bool DetachWithIME()
        {
            if (CanDetachWithIME())
            {
                if (IsVisible)
                {
                    IsVisible = false;
                    OnKeyboardWillHide();
                    TextFieldInFocus.RemoveEventListener(keyboardListener);
                    OnKeyboardDidHide();

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


        CCKeys lastPressedKey = CCKeys.None;
        void OnKeyPressed(CCEventKeyboard eventKeyboard)
        {

            lastPressedKey = CCKeys.None;

            if (eventKeyboard.Keys == CCKeys.Back)
            {
                lastPressedKey = eventKeyboard.Keys;
            }
            else
            {
                keyState = eventKeyboard.KeyboardState;
                var charKey = ConvertKey(eventKeyboard.Keys, ShiftDown, false, false);
                if (charKey != (int)CCKeys.None)
                {
                    lastPressedKey = eventKeyboard.Keys;
                }
            }

            CCLog.Log("On Pressed " + lastPressedKey);
            if (AutoRepeat && lastPressedKey != CCKeys.None)
            {
                StartAutorepeat();
            }
        }

        void OnKeyReleased(CCEventKeyboard eventKeyboard)
        {
            CCLog.Log("On Released " + eventKeyboard.Keys);
            if (eventKeyboard.Keys == CCKeys.Back)
                OnDeleteBackward();
            else
            {
                keyState = eventKeyboard.KeyboardState;
                var charKey = ConvertKey(eventKeyboard.Keys, ShiftDown, false, false);
                if (charKey != (int)CCKeys.None)
                    OnInsertText(new CCIMEKeybardEventArgs(charKey.ToString(), 1));
            }

            lastPressedKey = CCKeys.None;
            if (AutoRepeat)
                StopAutorepeat();
        }

        const float AutorepeatDeltaTime = 0.15f;
        const int AutorepeatIncreaseTimeIncrement = 12;
        int autorepeatCount;

        protected void StartAutorepeat()
        {
            autorepeatCount--;

            TextFieldInFocus.Schedule(Repeater, AutorepeatDeltaTime, CCSchedulePriority.RepeatForever, AutorepeatDeltaTime * 3);
        }

        protected void StopAutorepeat()
        {
            TextFieldInFocus.Unschedule(Repeater);
        }

        public void Repeater(float dt)
        {
            autorepeatCount++;

            if ((autorepeatCount < AutorepeatIncreaseTimeIncrement) && (autorepeatCount % 3) != 0)
                return;

            if (lastPressedKey == CCKeys.Back)
                OnDeleteBackward();
            else
            {
                var charKey = ConvertKey(lastPressedKey, ShiftDown, false, false);
                if (charKey != (int)CCKeys.None)
                    OnInsertText(new CCIMEKeybardEventArgs(charKey.ToString(), 1));
            }
        }

        #region Keyboard event conversion helpers.

        // The following code is something that was in an old MonoGame test project I had lying around 
        // and originated from the following discussion:  http://www.gamedev.net/topic/457783-xna-getting-text-from-keyboard/
        // it was modified slightly then and just slapped it in here with small modifications to make
        // it work with CocosSharp.

        // This will be better to served to break it out into it's own routine so it can be easily modified
        // by users.

        CCKeyboardState keyState;
        bool capsLock;
        bool numLock;
        bool scrollLock;

        public bool AltDown
        {
            get { return keyState.IsKeyDown(CCKeys.LeftAlt) || keyState.IsKeyDown(CCKeys.RightAlt); }
        }

        public bool CtrlDown
        {
            get { return keyState.IsKeyDown(CCKeys.LeftControl) || keyState.IsKeyDown(CCKeys.RightControl); }
        }

        public bool ShiftDown
        {
            get { return keyState.IsKeyDown(CCKeys.LeftShift) || keyState.IsKeyDown(CCKeys.RightShift); }
        }

        public bool CapsLock { get { return capsLock; } }
        public bool NumLock { get { return numLock; } }
        public bool ScrollLock { get { return scrollLock; } }

        public static char ConvertKey(CCKeys key, bool shift, bool capsLock, bool numLock)
        {

            switch (key)
            {

                case CCKeys.A: return ConvertToChar('a', shift, capsLock);
                case CCKeys.B: return ConvertToChar('b', shift, capsLock);
                case CCKeys.C: return ConvertToChar('c', shift, capsLock);
                case CCKeys.D: return ConvertToChar('d', shift, capsLock);
                case CCKeys.E: return ConvertToChar('e', shift, capsLock);
                case CCKeys.F: return ConvertToChar('f', shift, capsLock);
                case CCKeys.G: return ConvertToChar('g', shift, capsLock);
                case CCKeys.H: return ConvertToChar('h', shift, capsLock);
                case CCKeys.I: return ConvertToChar('i', shift, capsLock);
                case CCKeys.J: return ConvertToChar('j', shift, capsLock);
                case CCKeys.K: return ConvertToChar('k', shift, capsLock);
                case CCKeys.L: return ConvertToChar('l', shift, capsLock);
                case CCKeys.M: return ConvertToChar('m', shift, capsLock);
                case CCKeys.N: return ConvertToChar('n', shift, capsLock);
                case CCKeys.O: return ConvertToChar('o', shift, capsLock);
                case CCKeys.P: return ConvertToChar('p', shift, capsLock);
                case CCKeys.Q: return ConvertToChar('q', shift, capsLock);
                case CCKeys.R: return ConvertToChar('r', shift, capsLock);
                case CCKeys.S: return ConvertToChar('s', shift, capsLock);
                case CCKeys.T: return ConvertToChar('t', shift, capsLock);
                case CCKeys.U: return ConvertToChar('u', shift, capsLock);
                case CCKeys.V: return ConvertToChar('v', shift, capsLock);
                case CCKeys.W: return ConvertToChar('w', shift, capsLock);
                case CCKeys.X: return ConvertToChar('x', shift, capsLock);
                case CCKeys.Y: return ConvertToChar('y', shift, capsLock);
                case CCKeys.Z: return ConvertToChar('z', shift, capsLock);

                case CCKeys.D0: return (shift) ? ')' : '0';
                case CCKeys.D1: return (shift) ? '!' : '1';
                case CCKeys.D2: return (shift) ? '@' : '2';
                case CCKeys.D3: return (shift) ? '#' : '3';
                case CCKeys.D4: return (shift) ? '$' : '4';
                case CCKeys.D5: return (shift) ? '%' : '5';
                case CCKeys.D6: return (shift) ? '^' : '6';
                case CCKeys.D7: return (shift) ? '&' : '7';
                case CCKeys.D8: return (shift) ? '*' : '8';
                case CCKeys.D9: return (shift) ? '(' : '9';

                case CCKeys.Add: return '+';
                case CCKeys.Divide: return '/';
                case CCKeys.Multiply: return '*';
                case CCKeys.Subtract: return '-';

                case CCKeys.Space: return ' ';
                case CCKeys.Enter: return '\n';

                case CCKeys.Decimal: if (numLock && !shift) return '.'; break;
                case CCKeys.NumPad0: if (numLock && !shift) return '0'; break;
                case CCKeys.NumPad1: if (numLock && !shift) return '1'; break;
                case CCKeys.NumPad2: if (numLock && !shift) return '2'; break;
                case CCKeys.NumPad3: if (numLock && !shift) return '3'; break;
                case CCKeys.NumPad4: if (numLock && !shift) return '4'; break;
                case CCKeys.NumPad5: if (numLock && !shift) return '5'; break;
                case CCKeys.NumPad6: if (numLock && !shift) return '6'; break;
                case CCKeys.NumPad7: if (numLock && !shift) return '7'; break;
                case CCKeys.NumPad8: if (numLock && !shift) return '8'; break;
                case CCKeys.NumPad9: if (numLock && !shift) return '9'; break;

                case CCKeys.OemBackslash: return shift ? '|' : '\\';
                case CCKeys.OemCloseBrackets: return shift ? '}' : ']';
                case CCKeys.OemComma: return shift ? '<' : ',';
                case CCKeys.OemMinus: return shift ? '_' : '-';
                case CCKeys.OemOpenBrackets: return shift ? '{' : '[';
                case CCKeys.OemPeriod: return shift ? '>' : '.';
                case CCKeys.OemPipe: return shift ? '|' : '\\';
                case CCKeys.OemPlus: return shift ? '+' : '=';
                case CCKeys.OemQuestion: return shift ? '?' : '/';
                case CCKeys.OemQuotes: return shift ? '"' : '\'';
                case CCKeys.OemSemicolon: return shift ? ':' : ';';
                case CCKeys.OemTilde: return shift ? '~' : '`';
            }
            return (char)0;
        }

        public static char ConvertToChar(char baseChar, bool shift, bool capsLock)
        {
            return (capsLock ^ shift) ? char.ToUpper(baseChar) : baseChar;
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