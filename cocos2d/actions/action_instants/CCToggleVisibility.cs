namespace cocos2d
{
    public class CCToggleVisibility : CCActionInstant
    {
        public new static CCToggleVisibility Create()
        {
            var pRet = new CCToggleVisibility();
            return pRet;
        }

        public override object CopyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCActionInstant ret;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = (CCToggleVisibility) tmpZone.m_pCopyObject;
            }
            else
            {
                ret = new CCToggleVisibility();
                tmpZone = new CCZone(ret);
            }

            base.CopyWithZone(tmpZone);
            return ret;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            target.Visible = !target.Visible;
        }
    }
}