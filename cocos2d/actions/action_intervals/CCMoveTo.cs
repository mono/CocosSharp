namespace cocos2d
{
    public class CCMoveTo : CCActionInterval
    {
        protected CCPoint m_delta;
        protected CCPoint m_endPosition;
        protected CCPoint m_startPosition;

        public CCMoveTo (float duration, CCPoint position)
        {
            InitWithDuration(duration, position);
        }

        protected CCMoveTo (CCMoveTo moveTo) : base (moveTo)
        {
            InitWithDuration(moveTo.m_fDuration, moveTo.m_endPosition);

        }

        protected bool InitWithDuration(float duration, CCPoint position)
        {
            if (base.InitWithDuration(duration))
            {
                m_endPosition = position;
                return true;
            }

            return false;
        }

        public override object Copy(ICopyable zone)
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

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_startPosition = target.Position;
            m_delta = CCPointExtension.Subtract(m_endPosition, m_startPosition);
        }

        public override void Update(float time)
        {
            if (m_pTarget != null)
            {
                m_pTarget.Position = new CCPoint(m_startPosition.X + m_delta.X * time,
                                                          m_startPosition.Y + m_delta.Y * time);
            }
        }

    }
}