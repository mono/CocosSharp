namespace CocosSharp
{
    public class CCShow : CCActionInstant
    {
        #region Constructors

        public CCShow()
        {
        }

        protected CCShow(CCShow show) : base(show)
        {
        }

        #endregion Constructors


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
            return new CCShow(this);
        }
    }
}