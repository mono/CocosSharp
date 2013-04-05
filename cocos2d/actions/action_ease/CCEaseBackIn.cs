namespace cocos2d
{
    public class CCEaseBackIn : CCActionEase
    {

		public CCEaseBackIn(CCActionInterval pAction) : base (pAction)
		{}

		protected CCEaseBackIn (CCEaseBackIn easeBackIn) : base (easeBackIn)
		{}

		public override void Update(float time)
        {
            const float overshoot = 1.70158f;
            m_pOther.Update(time * time * ((overshoot + 1) * time - overshoot));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseBackOut.Create((CCActionInterval) m_pOther.Reverse());
        }

        public override object Copy(ICopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseBackIn;
				pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));
				
				return pCopy;
			}
            else
            {
                return new CCEaseBackIn(this);
            }

        }


    }
}