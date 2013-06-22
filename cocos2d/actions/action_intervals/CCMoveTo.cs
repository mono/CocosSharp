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

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_startPosition = target.Position;
            m_positionDelta = m_endPosition - target.Position;
        }
    }
}