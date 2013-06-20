namespace Cocos2D
{
    public class CCScaleBy : CCScaleTo
    {
        protected CCScaleBy(CCScaleBy copy)
            : base(copy)
        {
            // Handled by the base class.
        }

        public CCScaleBy(float duration, float s)
            : base(duration, s)
        {
        }

        public CCScaleBy(float duration, float sx, float sy)
            : base(duration, sx, sy)
        {
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_fDeltaX = m_fStartScaleX * m_fEndScaleX - m_fStartScaleX;
            m_fDeltaY = m_fStartScaleY * m_fEndScaleY - m_fStartScaleY;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCScaleBy(m_fDuration, 1 / m_fEndScaleX, 1 / m_fEndScaleY);
        }

        public override object Copy(ICCCopyable zone)
        {
            if (zone != null)
            {
                var ret = zone as CCScaleBy;
                base.Copy(zone); // Handles all data copying.
                return ret;
            }
            else
            {
                return new CCScaleBy(this);
            }
        }
    }
}