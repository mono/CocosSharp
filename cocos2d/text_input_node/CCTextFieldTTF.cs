using System;
using Microsoft.Xna.Framework.GamerServices;

namespace Cocos2D
{
    public delegate void CCTextFieldTTFChangeTextDelegate(ref string text, ref bool canceled);

    public class CCTextFieldTTF : CCLabelTTF
    {
        private bool _readOnly = false;
        private IAsyncResult _GuideShowHandle;

        public event CCTextFieldTTFChangeTextDelegate TextChanged;

        public bool ReadOnly
        {
            get { return _readOnly; }
            set
            {
                _readOnly = value;
                
                if (!value)
                {
                    EndEdit();
                }
            }
        }

        public CCTextFieldTTF(string text, string fontName, float fontSize) :
            this(text, fontName, fontSize, CCSize.Zero, CCTextAlignment.Center,
                 CCVerticalTextAlignment.Top)
        {
        }

        public CCTextFieldTTF(string text, string fontName, float fontSize, CCSize dimensions,
                              CCTextAlignment hAlignment) :
                                  this(text, fontName, fontSize, dimensions, hAlignment, CCVerticalTextAlignment.Top)
        {
        }

        public CCTextFieldTTF(string text, string fontName, float fontSize, CCSize dimensions,
                              CCTextAlignment hAlignment,
                              CCVerticalTextAlignment vAlignment)
        {
            InitWithString(text, fontName, fontSize, dimensions, hAlignment, vAlignment);
        }

        public void Edit()
        {
            Edit("Input", "Please provide input");
        }

        public void Edit(string title, string defaultText)
        {
            if (!_readOnly && !Guide.IsVisible)
            {
                _GuideShowHandle = Guide.BeginShowKeyboardInput(
                    Microsoft.Xna.Framework.PlayerIndex.One, title, defaultText, Text, InputHandler, null
                    );
            }
        }

        private void InputHandler(IAsyncResult result)
        {
            var newText = Guide.EndShowKeyboardInput(result);

            if (newText != null && Text != newText)
            {
                bool canceled = false;

                DoTextChanged(ref newText, ref canceled);

                if (!canceled)
                {
                    Text = newText;
                }
            }

            _GuideShowHandle = null;
        }

        protected virtual void DoTextChanged(ref string newText, ref bool canceled)
        {
            if (TextChanged != null)
            {
                TextChanged(ref newText, ref canceled);
            }
        }

        public void EndEdit()
        {
            if (_GuideShowHandle != null)
            {
                Guide.EndShowKeyboardInput(_GuideShowHandle);
                _GuideShowHandle = null;
            }
        }
    }
}