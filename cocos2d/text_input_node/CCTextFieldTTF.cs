using System;
#if !WINDOWS_PHONE && !XBOX
using Microsoft.Xna.Framework.GamerServices;
#endif
namespace Cocos2D
{
	public delegate void CCTextFieldTTFDelegate(object sender, ref string text, ref bool canceled);

    public class CCTextFieldTTF : CCLabelTTF, ICCTargetedTouchDelegate
    {
        private IAsyncResult m_pGuideShowHandle;
        private string m_sEditTitle = "Input";
        private string m_sEditDescription = "Please provide input";
        private bool m_bReadOnly = false;
        private bool m_bAutoEdit;
        private bool m_bTouchHandled;

        public event CCTextFieldTTFDelegate BeginEditing;
        public event CCTextFieldTTFDelegate EndEditing;

        public bool ReadOnly
        {
            get { return m_bReadOnly; }
            set
            {
                m_bReadOnly = value;
                
                if (!value)
                {
                    EndEdit();
                }
                
                CheckTouchState();
            }
        }

        public string EditTitle
        {
            get { return m_sEditTitle; }
            set { m_sEditTitle = value; }
        }

        public string EditDescription
        {
            get { return m_sEditDescription; }
            set { m_sEditDescription = value; }
        }

        public bool AutoEdit
        {
            get { return m_bAutoEdit; }
            set
            {
                m_bAutoEdit = value;
                CheckTouchState();
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
            Edit(m_sEditTitle, m_sEditDescription);
        }

        public void Edit(string title, string defaultText)
        {
#if !WINDOWS_PHONE && !XBOX
            if (!m_bReadOnly && !Guide.IsVisible)
            {
                var canceled = false;
                var text = Text;

                DoBeginEditing(ref text, ref canceled);

                if (!canceled)
                {
                    m_pGuideShowHandle = Guide.BeginShowKeyboardInput(
                        Microsoft.Xna.Framework.PlayerIndex.One, title, defaultText, Text, InputHandler, null
                        );
                }
            }
#endif
        }

        private void InputHandler(IAsyncResult result)
        {
#if !WINDOWS_PHONE && !XBOX
            var newText = Guide.EndShowKeyboardInput(result);

            m_pGuideShowHandle = null;

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
#endif
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
            if (m_pGuideShowHandle != null)
			{
#if !WINDOWS_PHONE && !XBOX
				Guide.EndShowKeyboardInput(m_pGuideShowHandle);
#endif
                m_pGuideShowHandle = null;
            }
        }

        private void CheckTouchState()
        {
            if (m_bRunning)
            {
                if (!m_bTouchHandled && !m_bReadOnly && m_bAutoEdit)
                {
                    CCDirector.SharedDirector.TouchDispatcher.AddTargetedDelegate(this, 0, true);
                    m_bTouchHandled = true;
                }
                else if (m_bTouchHandled && (m_bReadOnly || !m_bAutoEdit))
                {
                    CCDirector.SharedDirector.TouchDispatcher.RemoveDelegate(this);
                    m_bTouchHandled = true;
                }
            }
            else
            {
                if (!m_bRunning && m_bTouchHandled)
                {
                    CCDirector.SharedDirector.TouchDispatcher.RemoveDelegate(this);
                    m_bTouchHandled = false;
                }
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            CheckTouchState();
        }

        public override void OnExit()
        {
            base.OnExit();
            CheckTouchState();
        }

        public override bool TouchBegan(CCTouch pTouch)
        {
            var pos = ConvertTouchToNodeSpace(pTouch);
            if (pos.X >= 0 && pos.X < ContentSize.Width && pos.Y >= 0 && pos.Y <= ContentSize.Height)
            {
                return true;
            }
            return false;
        }

        public override void TouchMoved(CCTouch pTouch)
        {
            //nothing
        }

        public override void TouchEnded(CCTouch pTouch)
        {
            var pos = ConvertTouchToNodeSpace(pTouch);
            if (pos.X >= 0 && pos.X < ContentSize.Width && pos.Y >= 0 && pos.Y <= ContentSize.Height)
            {
                Edit();
            }
        }

        public override void TouchCancelled(CCTouch pTouch)
        {
            //nothing
        }
    }
}