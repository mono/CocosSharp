
namespace Cocos2D
{
    public class CCEaseBounceOut : CCEaseBounce
    {

        public CCEaseBounceOut (CCActionInterval pAction) : base (pAction)
        { }

        public CCEaseBounceOut (CCEaseBounceOut easeBounceOut) : base (easeBounceOut)
        { }

        public override void Update(float time)
        {
            float newT = BounceTime(time);
            m_pOther.Update(newT);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseBounceIn((CCActionInterval) m_pOther.Reverse());
        }

        public override object Copy(ICCCopyable pZone)
        {

            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseBounceOut;
                pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));
                
                return pCopy;
            }
            else
            {
                return new CCEaseBounceOut(this);
            }

        }

    }
}