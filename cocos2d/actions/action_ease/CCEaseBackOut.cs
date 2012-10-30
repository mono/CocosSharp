namespace cocos2d
{
    public class CCEaseBackOut : CCActionEase
    {
        public override void Update(float time)
        {
            const float overshoot = 1.70158f;

            time = time - 1;
            m_pOther.Update(time * time * ((overshoot + 1) * time + overshoot) + 1);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseBackIn.Create((CCActionInterval) m_pOther.Reverse());
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCEaseBackOut pCopy;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseBackOut;
            }
            else
            {
                pCopy = new CCEaseBackOut();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));

            return pCopy;
        }

        public new static CCEaseBackOut Create(CCActionInterval pAction)
        {
            var pRet = new CCEaseBackOut();
            pRet.InitWithAction(pAction);
            return pRet;
        }
    }
}