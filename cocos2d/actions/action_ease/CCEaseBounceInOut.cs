
namespace Cocos2D
{
    public class CCEaseBounceInOut : CCEaseBounce
    {

        public CCEaseBounceInOut (CCActionInterval pAction) : base (pAction)
        { }

        public CCEaseBounceInOut (CCEaseBounceInOut easeBounceInOut) : base (easeBounceInOut)
        { }

        public override void Update(float time)
        {
            float newT;

            if (time < 0.5f)
            {
                time = time * 2;
                newT = (1 - BounceTime(1 - time)) * 0.5f;
            }
            else
            {
                newT = BounceTime(time * 2 - 1) * 0.5f + 0.5f;
            }

            m_pOther.Update(newT);
        }

        public override object Copy(ICCCopyable pZone)
        {

            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseBounceInOut;
                pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));
                
                return pCopy;
            }
            else
            {
                return new CCEaseBounceInOut(this);
            }

        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseBounceInOut((CCActionInterval)m_pOther.Reverse());
        }


    }
}