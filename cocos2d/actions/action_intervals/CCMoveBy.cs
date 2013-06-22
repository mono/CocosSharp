namespace Cocos2D
{
    public class CCMoveBy : CCActionInterval
    {
        protected CCPoint m_positionDelta;
        protected CCPoint m_endPosition;
        protected CCPoint m_startPosition;
        protected CCPoint m_previousPosition;

        public CCMoveBy(float duration, CCPoint position) 
        {
            InitWithDuration(duration, position);
        }

        protected CCMoveBy(CCMoveBy moveBy) : base(moveBy)
        {
            InitWithDuration(moveBy.m_fDuration, moveBy.m_positionDelta);
        }

        protected virtual bool InitWithDuration(float duration, CCPoint position)
        {
            if (base.InitWithDuration(duration))
            {
                m_positionDelta = position;
                return true;
            }
            return false;
        }

        public override object Copy(ICCCopyable zone)
        {
            if (zone != null)
            {
                var ret = zone as CCMoveBy;

                if (ret == null)
                {
                    return null;
                }

                base.Copy(zone);

                ret.InitWithDuration(m_fDuration, m_positionDelta);

                return ret;
            }
            else
            {
                return new CCMoveBy(this);
            }
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