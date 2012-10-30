
namespace cocos2d
{
    public class CCEaseBounce : CCActionEase
    {
        public float BounceTime(float time)
        {
            if (time < 1 / 2.75)
            {
                return 7.5625f * time * time;
            }
            
            if (time < 2 / 2.75)
            {
                time -= 1.5f / 2.75f;
                return 7.5625f * time * time + 0.75f;
            }
            
            if (time < 2.5 / 2.75)
            {
                time -= 2.25f / 2.75f;
                return 7.5625f * time * time + 0.9375f;
            }

            time -= 2.625f / 2.75f;
            return 7.5625f * time * time + 0.984375f;
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCEaseBounce pCopy;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseBounce;
            }
            else
            {
                pCopy = new CCEaseBounce();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));

            return pCopy;
        }

        public new static CCEaseBounce Create(CCActionInterval pAction)
        {
            var pRet = new CCEaseBounce();
            pRet.InitWithAction(pAction);
            return pRet;
        }
    }
}