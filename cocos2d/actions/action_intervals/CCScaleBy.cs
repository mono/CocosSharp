namespace CocosSharp
{
    public class CCScaleBy : CCScaleTo
    {
        #region Constructors

        protected CCScaleBy(CCScaleBy copy) : base(copy)
        {
            // Handled by the base class.
        }

        public CCScaleBy(float duration, float s) : base(duration, s)
        {
        }

        public CCScaleBy(float duration, float sx, float sy) : base(duration, sx, sy)
        {
        }

        #endregion Constructors


        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_fDeltaX = m_fStartScaleX * m_fEndScaleX - m_fStartScaleX;
            m_fDeltaY = m_fStartScaleY * m_fEndScaleY - m_fStartScaleY;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCScaleBy(m_fDuration, 1 / m_fEndScaleX, 1 / m_fEndScaleY);
        }

        public override object Copy(ICCCopyable zone)
        {
                return new CCScaleBy(this);
        }
    }
}