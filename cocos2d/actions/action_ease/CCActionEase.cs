
namespace cocos2d
{
    public class CCActionEase : CCActionInterval
    {
        protected CCActionInterval m_pOther;

        public bool InitWithAction(CCActionInterval pAction)
        {
            if (base.InitWithDuration(pAction.Duration))
            {
                m_pOther = pAction;
                return true;
            }
            return false;
        }

        public override object CopyWithZone(CCZone pZone)
        {
            CCActionEase pCopy;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCActionEase;
            }
            else
            {
                pCopy = new CCActionEase();
            }

            base.CopyWithZone(pZone);

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));

            return pCopy;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_pOther.StartWithTarget(m_pTarget);
        }

        public override void Stop()
        {
            m_pOther.Stop();
            base.Stop();
        }

        public override void Update(float time)
        {
            m_pOther.Update(time);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create((CCActionInterval) m_pOther.Reverse());
        }

        public static CCActionEase Create(CCActionInterval pAction)
        {
            var pRet = new CCActionEase();
            pRet.InitWithAction(pAction);
            return pRet;
        }
    }
}