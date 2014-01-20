namespace CocosSharp
{
    public class CCBezierTo : CCBezierBy
    {
        #region Constructors

        public CCBezierTo(float t, CCBezierConfig c) : base(t, c)
        {
        }

        protected CCBezierTo(CCBezierTo bezierTo) : base(bezierTo)
        {
        }

        #endregion Constructors


        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_sConfig.ControlPoint1 = (m_sConfig.ControlPoint1 - m_startPosition);
            m_sConfig.ControlPoint2 = (m_sConfig.ControlPoint2 - m_startPosition);
            m_sConfig.EndPosition = (m_sConfig.EndPosition - m_startPosition);
        }

        public override object Copy(ICCCopyable zone)
        {
            return new CCBezierTo(this);
        }
    }
}