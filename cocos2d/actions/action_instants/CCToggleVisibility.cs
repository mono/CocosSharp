namespace Cocos2D
{
    public class CCToggleVisibility : CCActionInstant
    {
        public CCToggleVisibility()
        {
        }

        protected CCToggleVisibility(CCToggleVisibility toggleVisibility) : base(toggleVisibility)
        {
        }

        public override object Copy(ICCCopyable zone)
        {
            if (zone != null)
            {
                var ret = (CCToggleVisibility) zone;
                base.Copy(zone);
                return ret;
            }
            return new CCToggleVisibility(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            target.Visible = !target.Visible;
        }
    }
}