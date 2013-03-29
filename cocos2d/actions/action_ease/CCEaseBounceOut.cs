
namespace cocos2d
{
    public class CCEaseBounceOut : CCEaseBounce
    {
        public override void Update(float time)
        {
            float newT = BounceTime(time);
            m_pOther.Update(newT);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseBounceIn.Create((CCActionInterval) m_pOther.Reverse());
        }

        public override object Copy(ICopyable pZone)
        {
            CCEaseBounceOut pCopy;

            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = pZone as CCEaseBounceOut;
            }
            else
            {
                pCopy = new CCEaseBounceOut();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));

            return pCopy;
        }

        public new static CCEaseBounceOut Create(CCActionInterval pAction)
        {
            var pRet = new CCEaseBounceOut();
            pRet.InitWithAction(pAction);
            return pRet;
        }
    }
}