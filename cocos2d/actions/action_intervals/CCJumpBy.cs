namespace Cocos2D
{
    public class CCJumpBy : CCActionInterval
    {
        protected CCPoint m_delta;
        protected float m_height;
        protected uint m_nJumps;
        protected CCPoint m_startPosition;
        protected CCPoint m_previousPos;


        public CCJumpBy(float duration, CCPoint position, float height, uint jumps)
        {
            InitWithDuration(duration, position, height, jumps);
        }

        protected CCJumpBy(CCJumpBy jumpBy) : base(jumpBy)
        {
            InitWithDuration(jumpBy.m_fDuration, jumpBy.m_delta, jumpBy.m_height, jumpBy.m_nJumps);
        }

        protected bool InitWithDuration(float duration, CCPoint position, float height, uint jumps)
        {
            if (base.InitWithDuration(duration))
            {
                m_delta = position;
                m_height = height;
                m_nJumps = jumps;

                return true;
            }

            return false;
        }

        public override object Copy(ICCCopyable zone)
        {
            if (zone != null)
            {
                var ret = zone as CCJumpBy;
                if (ret == null)
                {
                    return null;
                }
                base.Copy(zone);

                ret.InitWithDuration(m_fDuration, m_delta, m_height, m_nJumps);

                return ret;
            }
            return new CCJumpBy(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_previousPos = m_startPosition = target.Position;
        }

        public override void Update(float time)
        {
            if (m_pTarget != null)
            {
                // Is % equal to fmodf()???
                float frac = (time * m_nJumps) % 1f;
                float y = m_height * 4f * frac * (1f - frac);
                y += m_delta.Y * time;
                float x = m_delta.X * time;

                CCPoint currentPos = m_pTarget.Position;

                CCPoint diff = currentPos - m_previousPos;
                m_startPosition = diff + m_startPosition;

                CCPoint newPos = m_startPosition + new CCPoint(x,y);
                m_pTarget.Position = newPos;

                m_previousPos = newPos;
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCJumpBy(m_fDuration, new CCPoint(-m_delta.X, -m_delta.Y), m_height, m_nJumps);
        }
    }
}