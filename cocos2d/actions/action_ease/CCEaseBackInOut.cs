namespace cocos2d
{
    public class CCEaseBackInOut : CCActionEase
    {

		public CCEaseBackInOut (CCActionInterval pAction) : base (pAction)
		{ }

		protected CCEaseBackInOut (CCEaseBackInOut easeBackInOut) : base (easeBackInOut)
		{ }

        public override void Update(float time)
        {
            const float overshoot = 1.70158f * 1.525f;

            time = time * 2;
            if (time < 1)
            {
                m_pOther.Update((time * time * ((overshoot + 1) * time - overshoot)) / 2);
            }
            else
            {
                time = time - 2;
                m_pOther.Update((time * time * ((overshoot + 1) * time + overshoot)) / 2 + 1);
            }
        }

        public override object Copy(ICopyable pZone)
        {

            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseBackInOut;
				pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));
				
				return pCopy;
			}
            else
            {
                return new CCEaseBackInOut(this);
            }

        }

        public override CCFiniteTimeAction Reverse()
        {
			return new CCEaseBackInOut((CCActionInterval) m_pOther.Reverse());
        }

    }
}