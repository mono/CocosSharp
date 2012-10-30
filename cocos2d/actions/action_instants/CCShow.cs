namespace cocos2d
{
    public class CCShow : CCActionInstant
    {
        public new static CCShow Create()
        {
            var pRet = new CCShow();
            return pRet;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            target.Visible = true;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return (CCHide.Create());
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCShow pRet;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pRet = (CCShow) (pZone.m_pCopyObject);
            }
            else
            {
                pRet = new CCShow();
                pZone = new CCZone(pRet);
            }

            base.CopyWithZone(pZone);
            return pRet;
        }
    }
}