namespace CocosSharp
{
    public class CCJumpTo : CCJumpBy
    {
        #region Constructors

        public CCJumpTo(float duration, CCPoint position, float height, uint jumps) : base(duration, position, height, jumps)
        {
        }

        protected CCJumpTo(CCJumpTo jumpTo) : base(jumpTo)
        {
        }

        #endregion Constructors


        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_delta = new CCPoint(m_delta.X - m_startPosition.X, m_delta.Y - m_startPosition.Y);
        }

        public override object Copy(ICCCopyable zone)
        {
            return new CCJumpTo(this);
        }
    }
}