namespace Cocos2D
{
    public class CCEaseBackOut : CCActionEase
    {

		public CCEaseBackOut (CCActionInterval pAction) : base (pAction)
		{ }

		protected CCEaseBackOut (CCEaseBackOut easeBackOut) : base (easeBackOut)
		{}

		public override void Update(float time)
        {
            const float overshoot = 1.70158f;

            time = time - 1;
            m_pOther.Update(time * time * ((overshoot + 1) * time + overshoot) + 1);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseBackIn ((CCActionInterval) m_pOther.Reverse());
        }

        public override object Copy(ICopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseBackOut;
				pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));
				
				return pCopy;
			}
            else
            {
                return new CCEaseBackOut(this);
            }

        }
    }
}