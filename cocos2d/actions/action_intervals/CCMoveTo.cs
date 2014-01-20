namespace CocosSharp
{
    public class CCMoveTo : CCMoveBy
    {
        #region Constructors

        public CCMoveTo(float duration, CCPoint position): base(duration, position)
        {
            InitCCMoveTo(position);
        }

        protected CCMoveTo(CCMoveTo moveTo) : base(moveTo)
        {
            InitCCMoveTo(moveTo.m_endPosition);
        }

        private void InitCCMoveTo(CCPoint position)
        {
            m_endPosition = position;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCMoveTo(this);
        }

        public override void Update(float time)
        {
            if (m_pTarget != null)
            {
                CCPoint currentPos = m_pTarget.Position;
                CCPoint diff = currentPos - m_previousPosition;
                //m_startPosition = m_startPosition + diff;
                CCPoint newPos = m_startPosition + m_positionDelta * time;
                m_pTarget.Position = newPos;
                m_previousPosition = newPos;
            }
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_startPosition = target.Position;
            m_positionDelta = m_endPosition - target.Position;
        }
    }
}