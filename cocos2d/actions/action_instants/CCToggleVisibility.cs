namespace Cocos2D
{
    public class CCToggleVisibility : CCActionInstant
    {
        public CCToggleVisibility()
        {
        }

        public override object Copy(ICCCopyable zone)
        {
            ICCCopyable tmpZone = zone;
            CCActionInstant ret;

            if (tmpZone != null && tmpZone != null)
            {
                ret = (CCToggleVisibility) tmpZone;
            }
            else
            {
                ret = new CCToggleVisibility();
                tmpZone =  (ret);
            }

            base.Copy(tmpZone);
            return ret;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            target.Visible = !target.Visible;
        }
    }
}