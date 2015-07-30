using System;
using Microsoft.Xna.Framework.Input;

namespace CocosSharp
{
    public delegate void CCTextFieldDelegate(object sender, ref string text, ref bool canceled);

    public class CCTextField : CCLabel
    {
        bool readOnly = false;
        bool autoEdit;
        bool touchHandled;



        public ICCIMEDelegate TextFieldIMEImplementation { get; set; }

        public event CCTextFieldDelegate BeginEditing;
        public event CCTextFieldDelegate EndEditing;

        #region Properties

        public bool ReadOnly
        {
            get { return readOnly; }
            set
            {
                readOnly = value;

                if (!value)
                {
                    EndEdit();
                }

                CheckTouchState();
            }
        }

        public bool AutoEdit
        {
            get { return autoEdit; }
            set
            {
                autoEdit = value;
                CheckTouchState();
            }
        }

        public bool AutoRepeat { get; set; }

        #endregion Properties


        #region Constructors

        public CCTextField(string text, string fontName, float fontSize)
            : this(text, fontName, fontSize, CCSize.Zero, CCTextAlignment.Center, CCVerticalTextAlignment.Top)
        {
        }

        public CCTextField(string text, string fontName, float fontSize, CCLabelFormat labelFormat)
            : this(text, fontName, fontSize, CCSize.Zero, CCTextAlignment.Center, CCVerticalTextAlignment.Top, labelFormat)
        {
        }


        public CCTextField(string text, string fontName, float fontSize, CCSize dimensions, CCTextAlignment hAlignment)
            : this(text, fontName, fontSize, dimensions, hAlignment, CCVerticalTextAlignment.Top)
        {
        }

        public CCTextField(string text, string fontName, float fontSize, CCSize dimensions, CCTextAlignment hAlignment, CCLabelFormat labelFormat)
            : this(text, fontName, fontSize, dimensions, hAlignment, CCVerticalTextAlignment.Top, labelFormat)
        {
        }

        public CCTextField(string text, string fontName, float fontSize, CCSize dimensions, CCTextAlignment hAlignment, CCVerticalTextAlignment vAlignment)
            : this(text, fontName, fontSize, dimensions, hAlignment, vAlignment, new CCLabelFormat(CCLabelFormatFlags.Unknown))
        { }

        public CCTextField(string text, string fontName, float fontSize, CCSize dimensions, CCTextAlignment hAlignment, CCVerticalTextAlignment vAlignment, CCLabelFormat labelFormat)
            : base(text, fontName, fontSize, dimensions, labelFormat)
        {
            this.HorizontalAlignment = hAlignment;
            this.VerticalAlignment = vAlignment;
            AutoRepeat = true;
            TextFieldIMEImplementation = IMEKeyboardImpl.SharedInstance;
        }

        #endregion Constructors

        public void Edit()
        {
#if !WINDOWS_PHONE

            if (!readOnly && TextFieldIMEImplementation != null && TextFieldIMEImplementation.CanAttachWithIME())
            {
                var canceled = false;
                var text = Text;

                DoBeginEditing(ref text, ref canceled);

                if (!canceled)
                {
                    TextFieldIMEImplementation.TextFieldInFocus = this;
                    TextFieldIMEImplementation.ContentText = Text;
                    var attached = TextFieldIMEImplementation.AttachWithIME();
                    if (attached)
                    {
                        TextFieldIMEImplementation.DeleteBackward += TextFieldIMEImplementation_DeleteBackward;
                        TextFieldIMEImplementation.InsertText += TextFieldIMEImplementation_InsertText;
                        TextFieldIMEImplementation.ReplaceText += TextFieldIMEImplementation_ReplaceText;
                    }

                }
            }
#endif
        }

        private void TextFieldIMEImplementation_ReplaceText(object sender, CCIMEKeybardEventArgs e)
        {
            ReplaceText(e.Text, e.Length);
        }

        private void TextFieldIMEImplementation_InsertText(object sender, CCIMEKeybardEventArgs e)
        {
            InsertText(e.Text, e.Length);
        }

        private void TextFieldIMEImplementation_DeleteBackward(object sender, CCIMEKeybardEventArgs e)
        {
            DeleteBackwards();
        }

        protected virtual void DoBeginEditing(ref string newText, ref bool canceled)
        {
            if (BeginEditing != null)
            {
                BeginEditing(this, ref newText, ref canceled);
            }
        }

        protected virtual void DoEndEditing(ref string newText, ref bool canceled)
        {
            if (EndEditing != null)
            {
                EndEditing(this, ref newText, ref canceled);
            }
        }

        public void EndEdit()
        {
#if !WINDOWS_PHONE
            if (TextFieldIMEImplementation != null && TextFieldIMEImplementation.CanDetachWithIME())
            {
                TextFieldIMEImplementation.DetachWithIME();
                
            }

#endif
        }

        protected virtual void DeleteBackwards()
        {
            var text = Text;
            if (string.IsNullOrEmpty(text) || text.Length == 1)
            {
                Text = string.Empty;
            }
            else
            {
                Text = text.Remove(text.Length - 1);
            }
        }

        protected virtual void InsertText(string text, int len)
        {

            var insert = new System.Text.StringBuilder(text, len);

            // if we have a new line then we end editing
            int pos = text.IndexOf('\n');
            if (pos >= 0 && insert.Length != pos)
            {
                len = pos;
                insert.Length = pos;
            }

            if (len > 0)
            {
                Text += insert.ToString();
                return;
            }

            EndEdit();
        }

        protected virtual void ReplaceText(string text, int len)
        {
            var insert = new System.Text.StringBuilder(text, len);

            // if we have a new line then we end editing
            int pos = text.IndexOf('\n');
            if (pos >= 0 && insert.Length != pos)
            {
                len = pos;
                insert.Length = pos;
            }

            if (len > 0)
            {
                Text = insert.ToString();
            }

            EndEdit();

        }

        void CheckTouchState()
        {
            if (IsRunning)
            {
                if (!touchHandled && !readOnly && autoEdit)
                {
                    touchHandled = true;
                }
                else if (touchHandled && (readOnly || !autoEdit))
                {
                    touchHandled = true;
                }
            }
            else
            {
                if (!IsRunning && touchHandled)
                {
                    touchHandled = false;
                }
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            CheckTouchState();
        }

        private void IMEImplementation_KeyboardDidHide(object sender, CCIMEKeyboardNotificationInfo e)
        {
            if (TextFieldIMEImplementation != null)
            {
                TextFieldIMEImplementation.DeleteBackward -= TextFieldIMEImplementation_DeleteBackward;
                TextFieldIMEImplementation.InsertText -= TextFieldIMEImplementation_InsertText;
                var newText = TextFieldIMEImplementation.ContentText;
                if (newText != null && Text != newText)
                {
                    bool canceled = false;

                    ScheduleOnce(
                        time =>
                        {
                            DoEndEditing(ref newText, ref canceled);

                            if (!canceled)
                            {
                                Text = newText;
                            }
                        },
                        0
                    );
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            CheckTouchState();
            if (TextFieldIMEImplementation != null)
            {
                TextFieldIMEImplementation.KeyboardDidHide -= IMEImplementation_KeyboardDidHide;
            }
        }

        public bool TouchBegan(CCTouch touch)
        {
            var pos = Layer.ScreenToWorldspace(touch.LocationOnScreen);
            if (pos.X >= 0 && pos.X < ContentSize.Width && pos.Y >= 0 && pos.Y <= ContentSize.Height)
            {
                return true;
            }
            return false;
        }

        public void TouchMoved(CCTouch touch)
        {
            //nothing
        }

        public void TouchEnded(CCTouch touch)
        {
            var pos = Layer.ScreenToWorldspace(touch.LocationOnScreen);
            if (pos.X >= 0 && pos.X < ContentSize.Width && pos.Y >= 0 && pos.Y <= ContentSize.Height)
            {
                Edit();
            }
        }

        public void TouchCancelled(CCTouch pTouch)
        {
            //nothing
        }

    }
}