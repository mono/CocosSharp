namespace CocosSharp
{
    public class CCHide : CCActionInstant
    {
        #region Constructors

        public CCHide()
        {
        }

        protected CCHide(CCHide hide) : base(hide)
        {
        }

        #endregion Constructors


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
            return new CCHide(this);
        }
    }
}