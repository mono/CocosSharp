namespace CocosSharp
{
    public class CCToggleVisibility : CCActionInstant
    {
        #region Constructors

        public CCToggleVisibility()
        {
        }

        protected CCToggleVisibility(CCToggleVisibility toggleVisibility) : base(toggleVisibility)
        {
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCToggleVisibility(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            target.Visible = !target.Visible;
        }
    }
}