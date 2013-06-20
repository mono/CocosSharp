namespace Cocos2D
{
    public class CCShow : CCActionInstant
    {
        public CCShow()
        {
        }

        protected CCShow(CCShow show) : base(show)
        {
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            target.Visible = true;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return (new CCHide());
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                var pRet = (CCShow) (pZone);
                base.Copy(pZone);
                return pRet;
            }
            return new CCShow(this);
        }
    }
}