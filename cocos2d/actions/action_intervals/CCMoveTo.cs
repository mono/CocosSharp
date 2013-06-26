namespace Cocos2D
{
    public class CCMoveTo : CCMoveBy
    {
        public CCMoveTo(float duration, CCPoint position): base(duration, position)
        {
        }

        protected CCMoveTo(CCMoveTo moveTo) : base(moveTo)
        {
            InitWithDuration(moveTo.m_fDuration, moveTo.m_endPosition);
        }

        protected override bool InitWithDuration(float duration, CCPoint position)
        {
            if (base.InitWithDuration(duration))
            {
                m_endPosition = position;
                return true;
            }
            return false;
        }

        public override object Copy(ICCCopyable zone)
        {
            if (zone != null)
            {
                var ret = (CCMoveTo) zone;
                base.Copy(zone);
                ret.InitWithDuration(m_fDuration, m_endPosition);

                return ret;
            }
            else
            {
                return new CCMoveTo(this);
            }
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