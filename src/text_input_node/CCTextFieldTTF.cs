using System;
#if !WINDOWS_PHONE
using Microsoft.Xna.Framework.GamerServices;
#endif
namespace CocosSharp
{
	public delegate void CCTextFieldTTFDelegate(object sender, ref string text, ref bool canceled);

    public class CCTextFieldTTF : CCLabelTtf
    {
		bool readOnly = false;
		bool autoEdit;
		bool touchHandled;

		IAsyncResult guideShowHandle;

        public event CCTextFieldTTFDelegate BeginEditing;
        public event CCTextFieldTTFDelegate EndEditing;


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

		public string EditTitle { get; set; }
		public string EditDescription { get; set; }

        public bool AutoEdit
        {
            get { return autoEdit; }
            set
            {
                autoEdit = value;
                CheckTouchState();
            }
        }

		#endregion Properties


        #region Constructors

        public CCTextFieldTTF(string text, string fontName, float fontSize) 
            : this(text, fontName, fontSize, CCSize.Zero, CCTextAlignment.Center, CCVerticalTextAlignment.Top)
        {
        }

        public CCTextFieldTTF(string text, string fontName, float fontSize, CCSize dimensions, CCTextAlignment hAlignment) 
            : this(text, fontName, fontSize, dimensions, hAlignment, CCVerticalTextAlignment.Top)
        {
        }

        public CCTextFieldTTF(string text, string fontName, float fontSize, CCSize dimensions, CCTextAlignment hAlignment, CCVerticalTextAlignment vAlignment)
            : base (text, fontName, fontSize, dimensions, hAlignment, vAlignment)
        {
			EditTitle = "Input";
			EditDescription = "Please provide input";
        }

        #endregion Constructors


        public void Edit()
        {
			Edit(EditTitle, EditDescription);
        }

        public void Edit(string title, string defaultText)
        {
#if !WINDOWS_PHONE
            if (!readOnly && !Guide.IsVisible)
            {
                var canceled = false;
                var text = Text;

                DoBeginEditing(ref text, ref canceled);

                if (!canceled)
                {
                    guideShowHandle = Guide.BeginShowKeyboardInput(
                        Microsoft.Xna.Framework.PlayerIndex.One, title, defaultText, Text, InputHandler, null
                        );
                }
            }
#endif
        }

        void InputHandler(IAsyncResult result)
        {
#if !WINDOWS_PHONE
			var newText = Guide.EndShowKeyboardInput(result);

            guideShowHandle = null;

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
            if (guideShowHandle != null)
			{
#if !WINDOWS_PHONE
				Guide.EndShowKeyboardInput(guideShowHandle);
#endif
                guideShowHandle = null;
            }
        }

        void CheckTouchState()
        {
			if (IsRunning)
            {
                if (!touchHandled && !readOnly && autoEdit)
                {
					//CCDirector.SharedDirector.TouchDispatcher.AddTargetedDelegate(this, 0, true);
                    touchHandled = true;
                }
                else if (touchHandled && (readOnly || !autoEdit))
                {
					//CCDirector.SharedDirector.TouchDispatcher.RemoveDelegate(this);
                    touchHandled = true;
                }
            }
            else
            {
				if (!IsRunning && touchHandled)
                {
					//CCDirector.SharedDirector.TouchDispatcher.RemoveDelegate(this);
                    touchHandled = false;
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

        public bool TouchBegan(CCTouch pTouch)
        {
            var pos = ConvertTouchToNodeSpace(pTouch);
            if (pos.X >= 0 && pos.X < ContentSize.Width && pos.Y >= 0 && pos.Y <= ContentSize.Height)
            {
                return true;
            }
            return false;
        }

        public void TouchMoved(CCTouch pTouch)
        {
            //nothing
        }

        public void TouchEnded(CCTouch pTouch)
        {
            var pos = ConvertTouchToNodeSpace(pTouch);
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