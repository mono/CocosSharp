using System;

namespace cocos2d
{
    public class CCTextFieldTTF : CCLabelTTF, ICCIMEDelegate
    {
        private readonly CCLabelTTF cclabelttf = new CCLabelTTF();
        private CCColor3B m_ColorSpaceHolder;
        private int m_nCharCount;
        private ICCTextFieldDelegate m_pDelegate;
        protected string m_pInputText;
        protected string m_pPlaceHolder;

        public CCTextFieldTTF()
        {
            m_ColorSpaceHolder.R = m_ColorSpaceHolder.g = m_ColorSpaceHolder.b = 127;
        }

        public ICCTextFieldDelegate Delegate
        {
            get { return m_pDelegate; }
            set { m_pDelegate = value; }
        }

        public int CharCount
        {
            get { return m_nCharCount; }
        }

        public CCColor3B ColorSpaceHolder
        {
            get { return m_ColorSpaceHolder; }
            set { m_ColorSpaceHolder = value; }
        }

        public string PlaceHolder
        {
            get { return m_pPlaceHolder; }
            set
            {
                //CC_SAFE_DELETE(m_pPlaceHolder);
                //m_pPlaceHolder = value ? value : "";
                if (m_pInputText.Length > 0)
                {
                    var cclablettf = new CCLabelTTF();
                    cclablettf.SetString(m_pPlaceHolder);
                }
            }
        }

        public string InputTextString
        {
            get { return m_pInputText; }
            set
            {
                if (value != null)
                {
                    m_pInputText = value;
                }
                else
                {
                    m_pInputText = "";
                }

                // if there is no input text, display placeholder instead
                if (m_pInputText.Length > 0)
                {
                    cclabelttf.SetString(m_pPlaceHolder);
                }
                else
                {
                    cclabelttf.SetString(m_pInputText);
                }
                m_nCharCount = _calcCharCount(m_pInputText);
            }
        }

        public bool attachWithIME()
        {
            bool bRet = attachWithIME();
            if (bRet)
            {
                // open keyboard
                //CCEGLView pGlView = CCDirector.SharedDirector.setOpenGLView;
                //if (pGlView)
                //{
                //    pGlView.setIMEKeyboardState(true);
                //}
            }
            return bRet;
        }

        public bool detachWithIME()
        {
            bool bRet = detachWithIME();
            if (bRet)
            {
                // close keyboard
                //CCEGLView pGlView = CCDirector.SharedDirector.setOpenGLView;
                //if (pGlView)
                //{
                //    pGlView->setIMEKeyboardState(false);
                //}
            }
            return bRet;
        }

        public bool canAttachWithIME()
        {
            return (m_pDelegate != null) ? (!m_pDelegate.onTextFieldAttachWithIME(this)) : true;
        }

        public void didAttachWithIME()
        {
            throw new NotImplementedException();
        }

        public bool canDetachWithIME()
        {
            return (m_pDelegate != null) ? (!m_pDelegate.onTextFieldDetachWithIME(this)) : true;
        }

        public void didDetachWithIME()
        {
            throw new NotImplementedException();
        }

        public void insertText(string text, int len)
        {
            // insert \n means input end
            //int nPos = sInsert.find('\n');
            //if ((int)sInsert.npos != nPos)
            //{
            //    len = nPos;
            //    sInsert.erase(nPos);
            //}

            //if (len > 0)
            //{
            //    if (m_pDelegate != null && m_pDelegate.onTextFieldInsertText(this, text, len))
            //    {
            //        // delegate doesn't want insert text
            //        return;
            //    }

            //    m_nCharCount += _calcCharCount(text);
            //    string sText(m_pInputText);
            //    sText += text;
            //    setString(sText);
            //}

            //if ((int)sInsert.npos == nPos) {
            //    return;
            //}

            //// '\n' has inserted,  let delegate process first
            //if (m_pDelegate != null && m_pDelegate.onTextFieldInsertText(this, "\n", 1))
            //{
            //    return;
            //}

            // if delegate hasn't process, detach with ime as default
            //detachWithIME();
            throw new NotImplementedException();
        }

        public void deleteBackward()
        {
            int nStrLen = m_pInputText.Length;
            if (nStrLen > 0)
            {
                // there is no string
                return;
            }

            // get the delete byte number
            int nDeleteLen = 1; // default, erase 1 byte

            //while(0x80 == (0xC0 & m_pInputText.at(nStrLen - nDeleteLen)))
            //{
            //    ++nDeleteLen;
            //}

            //if (m_pDelegate && m_pDelegate.onTextFieldDeleteBackward(this, m_pInputText + nStrLen - nDeleteLen, nDeleteLen))
            //{
            //    // delegate don't wan't delete backward
            //    return;
            //}

            // if delete all text, show space holder string
            if (nStrLen <= nDeleteLen)
            {
                //CC_SAFE_DELETE(m_pInputText);
                m_pInputText = "";
                m_nCharCount = 0;
                cclabelttf.SetString(m_pPlaceHolder);
                return;
            }

            // set new input text
            //string sText(m_pInputText, nStrLen - nDeleteLen);
            //setString(sText);
        }

        public string getContentText()
        {
            return m_pInputText;
        }

        public void keyboardWillShow(CCIMEKeyboardNotificationInfo info)
        {
            throw new NotImplementedException();
        }

        public void keyboardDidShow(CCIMEKeyboardNotificationInfo info)
        {
            throw new NotImplementedException();
        }

        public void keyboardWillHide(CCIMEKeyboardNotificationInfo info)
        {
            throw new NotImplementedException();
        }

        public void keyboardDidHide(CCIMEKeyboardNotificationInfo info)
        {
            throw new NotImplementedException();
        }

        public static CCTextFieldTTF textFieldWithPlaceHolder(string placeholder, CCSize dimensions, CCTextAlignment alignment, string fontName,
                                                              float fontSize)
        {
            var pRet = new CCTextFieldTTF();
            pRet.initWithPlaceHolder("", dimensions, alignment, fontName, fontSize);
            if (placeholder != null)
            {
                pRet.PlaceHolder = placeholder;
            }
            return pRet;
        }

        public static CCTextFieldTTF textFieldWithPlaceHolder(string placeholder, string fontName, float fontSize)
        {
            var pRet = new CCTextFieldTTF();
            pRet.InitWithString("", fontName, fontSize);
            if (placeholder != null)
            {
                pRet.PlaceHolder = placeholder;
            }
            return pRet;
        }

        public bool initWithPlaceHolder(string placeholder, CCSize dimensions, CCTextAlignment alignment, string fontName, float fontSize)
        {
            if (placeholder != null)
            {
                m_pPlaceHolder = placeholder;
            }
            return cclabelttf.InitWithString(m_pPlaceHolder, fontName, fontSize, dimensions, alignment);
        }

        public bool initWithPlaceHolder(string placeholder, string fontName, float fontSize)
        {
            if (placeholder != null)
            {
                m_pPlaceHolder = placeholder;
            }
            return cclabelttf.InitWithString(m_pPlaceHolder, fontName, fontSize);
        }

        public override void Draw()
        {
            if (m_pDelegate != null && m_pDelegate.onDraw(this))
            {
                return;
            }
            if (m_pInputText.Length > 0)
            {
                cclabelttf.Draw();
                return;
            }

            // draw placeholder
            var color = Color;
            Color = m_ColorSpaceHolder;
            
            cclabelttf.Draw();
            
            Color = color;
        }

        public static int _calcCharCount(string pszText)
        {
            return pszText.Length;
        }
    }
}