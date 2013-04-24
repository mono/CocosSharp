
namespace cocos2d
{
    public class CCEaseBounceIn : CCEaseBounce
    {

        public CCEaseBounceIn (CCActionInterval pAction) : base (pAction)
        {  }

        public CCEaseBounceIn (CCEaseBounceIn easeBounceIn) : base (easeBounceIn)
        {  }

        public override void Update(float time)
        {
            float newT = 1 - BounceTime(1 - time);
            m_pOther.Update(newT);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseBounceOut((CCActionInterval) m_pOther.Reverse());
        }

        public override object Copy(ICopyable pZone)
        {

            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseBounceIn;
                pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));
                
                return pCopy;
            }
            else
            {
                return new CCEaseBounceIn(this);
            }

        }


    }
}