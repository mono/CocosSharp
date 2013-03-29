namespace cocos2d
{
    public class CCToggleVisibility : CCActionInstant
    {
        public new static CCToggleVisibility Create()
        {
            var pRet = new CCToggleVisibility();
            return pRet;
        }

        public override object Copy(ICopyable zone)
        {
            ICopyable tmpZone = zone;
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