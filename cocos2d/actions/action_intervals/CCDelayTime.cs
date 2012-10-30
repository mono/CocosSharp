namespace cocos2d
{
    public class CCDelayTime : CCActionInterval
    {
        public new static CCDelayTime Create(float d)
        {
            var pAction = new CCDelayTime();
            pAction.InitWithDuration(d);
            return pAction;
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCDelayTime pCopy;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCDelayTime) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCDelayTime();
                pZone = new CCZone(pCopy);
            }


            base.CopyWithZone(pZone);

            return pCopy;
        }

        public override void Update(float time)
        {
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create(m_fDuration);
        }
    }
}