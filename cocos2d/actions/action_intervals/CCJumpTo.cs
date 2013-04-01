namespace cocos2d
{
    public class CCJumpTo : CCJumpBy
    {
        public CCJumpTo (float duration, CCPoint position, float height, uint jumps) :
            base (duration, position, height, jumps)
        {
            InitWithDuration(duration, position, height, jumps);
        }

        protected CCJumpTo (CCJumpTo jumpTo) : base (jumpTo)
        {
            InitWithDuration(jumpTo.m_fDuration, jumpTo.m_delta, jumpTo.m_height, jumpTo.m_nJumps);
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_delta = CCPointExtension.CreatePoint(m_delta.X - m_startPosition.X, m_delta.Y - m_startPosition.Y);
        }

        public override object Copy(ICopyable zone)
        {
            if (zone != null)
            {
                var ret = zone as CCJumpTo;
                if (ret == null)
                {
                    return null;
                }
                base.Copy(zone);
                
                ret.InitWithDuration(m_fDuration, m_delta, m_height, m_nJumps);
                
                return ret;
            }
            else
            {
                return new CCJumpTo(this);
            }


        }
    }
}