namespace Cocos2D
{
    public class CCHide : CCActionInstant
    {
        public CCHide()
        {
        }

        protected CCHide(CCHide hide) : base(hide)
        {
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            target.Visible = false;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return (new CCShow());
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                var pRet = (CCHide) (pZone);
                base.Copy(pZone);
                return pRet;
            }
            return new CCHide(this);
        }
    }
}