namespace cocos2d
{
    public class CCHide : CCActionInstant
    {
        public new static CCHide Create()
        {
            var pRet = new CCHide();
            return pRet;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            target.Visible = false;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return (CCShow.Create());
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCHide pRet;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pRet = (CCHide) (pZone.m_pCopyObject);
            }
            else
            {
                pRet = new CCHide();
                pZone = new CCZone(pRet);
            }

            base.CopyWithZone(pZone);
            return pRet;
        }
    }
}