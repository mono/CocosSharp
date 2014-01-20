namespace CocosSharp
{
    public class CCMoveBy : CCActionInterval
    {
        protected CCPoint m_positionDelta;
        protected CCPoint m_endPosition;
        protected CCPoint m_startPosition;
        protected CCPoint m_previousPosition;


        #region Constructors

        public CCMoveBy(float duration, CCPoint position) : base(duration)
        {
            InitWithDuration(duration, position);
        }

        // Perform deep copy of CCMoveBy
        protected CCMoveBy(CCMoveBy moveBy) : base(moveBy)
        {
            InitWithDuration(moveBy.m_fDuration, moveBy.m_positionDelta);
        }

        private void InitWithDuration(float duration, CCPoint position)
        {
            m_positionDelta = position;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCMoveBy(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_previousPosition = m_startPosition = target.Position;
        }

        public override void Update(float time)
        {
            if (m_pTarget != null)
            {
                CCPoint currentPos = m_pTarget.Position;
                CCPoint diff = currentPos - m_previousPosition;
                m_startPosition = m_startPosition + diff;
                CCPoint newPos = m_startPosition + m_positionDelta * time;
                m_pTarget.Position = newPos;
                m_previousPosition = newPos;
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCMoveBy(m_fDuration, new CCPoint(-m_positionDelta.X, -m_positionDelta.Y));
        }
    }
}