
namespace cocos2d
{
    public class CCEaseBounceIn : CCEaseBounce
    {
        public override void Update(float time)
        {
            float newT = 1 - BounceTime(1 - time);
            m_pOther.Update(newT);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseBounceOut.Create((CCActionInterval) m_pOther.Reverse());
        }

        public override object Copy(ICopyable pZone)
        {
            CCEaseBounceIn pCopy;

            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = pZone as CCEaseBounceIn;
            }
            else
            {
                pCopy = new CCEaseBounceIn();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));

            return pCopy;
        }

        public new static CCEaseBounceIn Create(CCActionInterval pAction)
        {
            var pRet = new CCEaseBounceIn();
            pRet.InitWithAction(pAction);
            return pRet;
        }
    }
}