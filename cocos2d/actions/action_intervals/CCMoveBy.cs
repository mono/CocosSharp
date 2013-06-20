namespace Cocos2D
{
    public class CCMoveBy : CCMoveTo
    {
        public CCMoveBy(float duration, CCPoint position) : base(duration, position)
        {
            InitWithDuration(duration, position);
        }

        protected CCMoveBy(CCMoveBy moveBy) : base(moveBy)
        {
            InitWithDuration(moveBy.m_fDuration, moveBy.m_delta);
        }

        protected new bool InitWithDuration(float duration, CCPoint position)
        {
            if (base.InitWithDuration(duration))
            {
                m_delta = position;
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

                ret.InitWithDuration(m_fDuration, m_delta);

                return ret;
            }
            else
            {
                return new CCMoveBy(this);
            }
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            CCPoint dTmp = m_delta;
            base.StartWithTarget(target);
            m_delta = dTmp;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCMoveBy(m_fDuration, new CCPoint(-m_delta.X, -m_delta.Y));
        }
    }
}