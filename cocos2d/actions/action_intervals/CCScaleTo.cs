namespace Cocos2D
{
    public class CCScaleTo : CCActionInterval
    {
        protected float m_fDeltaX;
        protected float m_fDeltaY;
        protected float m_fEndScaleX;
        protected float m_fEndScaleY;
        protected float m_fScaleX;
        protected float m_fScaleY;
        protected float m_fStartScaleX;
        protected float m_fStartScaleY;

        protected CCScaleTo(CCScaleTo copy)
            : base(copy)
        {
            m_fEndScaleX = copy.m_fEndScaleX;
            m_fEndScaleY = copy.m_fEndScaleY;
        }

        public CCScaleTo(float duration, float s) : base(duration)
        {
            m_fEndScaleX = s;
            m_fEndScaleY = s;
        }

        public CCScaleTo(float duration, float sx, float sy) : base(duration)
        {
            m_fEndScaleX = sx;
            m_fEndScaleY = sy;
        }

        public override object Copy(ICCCopyable zone)
        {
            if (zone != null)
            {
                var ret = zone as CCScaleTo;
                base.Copy(zone);
                m_fEndScaleX = ret.m_fEndScaleX;
                m_fEndScaleY = ret.m_fEndScaleY;
                return ret;
            }
            else
            {
                return new CCScaleTo(this);
            }
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_fStartScaleX = target.ScaleX;
            m_fStartScaleY = target.ScaleY;
            m_fDeltaX = m_fEndScaleX - m_fStartScaleX;
            m_fDeltaY = m_fEndScaleY - m_fStartScaleY;
        }

        public override void Update(float time)
        {
            if (m_pTarget != null)
            {
                m_pTarget.ScaleX = m_fStartScaleX + m_fDeltaX * time;
                m_pTarget.ScaleY = m_fStartScaleY + m_fDeltaY * time;
            }
        }
    }
}