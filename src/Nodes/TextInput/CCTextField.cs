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
        string placeHolderText = string.Empty;

        CCColor4B defaultTextColor = CCColor4B.White;
        CCColor4B placeholderTextColor = CCColor4B.Gray;

        public event CCTextFieldDelegate BeginEditing;
        public event CCTextFieldDelegate EndEditing;

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CocosSharp.CCTextField"/> read only.
        /// </summary>
        /// <value><c>true</c> if read only; otherwise, <c>false</c>.</value>
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

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CocosSharp.CCTextField"/> auto edits.
        /// </summary>
        /// <value><c>true</c> if auto edit; otherwise, <c>false</c>.</value>
        public bool AutoEdit
        {
            get { return autoEdit; }
            set
            {
                autoEdit = value;
                CheckTouchState();
            }
        }

        /// <summary>
        /// Gets or sets the place holder text when the Text is empty.
        /// </summary>
        /// <value>The place holder text.</value>
        public string PlaceHolderText 
        {
            get { return placeHolderText; }
            set 
            {
                if (placeHolderText == value)
                    return;
                
                placeHolderText = value;

                if (string.IsNullOrEmpty(Text))
                    Text = placeHolderText;
            }
        }

        /// <summary>
        /// Gets or sets the color of the place holder text.
        /// </summary>
        /// <value>The color of the place holder text.</value>
        public CCColor4B PlaceHolderTextColor
        {
            get { return placeholderTextColor; }
            set {
                placeholderTextColor = value;
                updateColors();
            }
        }

        void updateColors()
        {
            if (Text == placeHolderText)
            {
                if (Color.R != PlaceHolderTextColor.R ||
                    Color.G != PlaceHolderTextColor.G ||
                    Color.B != PlaceHolderTextColor.B ||
                    Opacity != PlaceHolderTextColor.A)
                {
                    Color = new CCColor3B(placeholderTextColor);
                    Opacity = placeholderTextColor.A;
                }
            }
            else
            {
                if (Color.R != defaultTextColor.R ||
                    Color.G != defaultTextColor.G ||
                    Color.B != defaultTextColor.B ||
                    Opacity != defaultTextColor.A)
                {
                    Color = new CCColor3B(defaultTextColor);
                    Opacity = defaultTextColor.A;
                }
            }
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    base.Text = placeHolderText;
                    updateColors();
                }
                else
                {
                    base.Text = value;
                    updateColors();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CocosSharp.CCTextField"/> auto repeats.
        /// </summary>
        /// <value><c>true</c> if auto repeat; otherwise, <c>false</c>.</value>
        public bool AutoRepeat { get; set; }

        /// <summary>
        /// Gets the character count.
        /// </summary>
        /// <value>The character count.  Returns 0 if current text is equal to PlaceHolderText</value>
        public int CharacterCount 
        {
            get 
            { 
                var currentText = Text;
                return currentText == placeHolderText ? 0 : currentText.Length; 
            }
        }

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
            placeHolderText = text;
            updateColors();
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
                    TextFieldIMEImplementation.KeyboardDidHide += OnKeyboardDidHide;

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

        /// <summary>
        /// Deletes a character from the end of TextField.
        /// </summary>
        protected virtual void DeleteBackwards()
        {
            var text = Text;
            if (text == placeHolderText)
                return;
            
            if (string.IsNullOrEmpty(text) || text.Length == 1)
            {
                Text = string.Empty;
            }
            else
            {
                Text = text.Remove(text.Length - 1);
            }
        }

        /// <summary>
        /// Inserts the text at the end of the TextField
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="len">Length.</param>
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
                if (Text == placeHolderText)
                    Text = text;
                else
                    Text += insert.ToString();
                return;
            }

            EndEdit();

        }

        /// <summary>
        /// Replaces the current text of the field
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="len">Length.</param>
        protected virtual void ReplaceText(string text, int len)
        {
            var insert = new System.Text.StringBuilder(text, len);

            if (len == 0)
            {
                Text = placeHolderText;
                return;
            }

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

        void OnKeyboardDidHide(object sender, CCIMEKeyboardNotificationInfo e)
        {
            if (TextFieldIMEImplementation != null)
            {
                TextFieldIMEImplementation.DeleteBackward -= TextFieldIMEImplementation_DeleteBackward;
                TextFieldIMEImplementation.InsertText -= TextFieldIMEImplementation_InsertText;
                TextFieldIMEImplementation.ReplaceText -= TextFieldIMEImplementation_ReplaceText;
                TextFieldIMEImplementation.KeyboardDidHide -= OnKeyboardDidHide;

                var newText = Text;

                //if (newText != null && Text != newText)
                //{

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
//                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            CheckTouchState();
            if (TextFieldIMEImplementation != null)
            {
                TextFieldIMEImplementation.KeyboardDidHide -= OnKeyboardDidHide;
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