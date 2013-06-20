namespace Cocos2D
{
    public class CCTintTo : CCActionInterval
    {
        protected CCColor3B m_from;
        protected CCColor3B m_to;

        public CCTintTo(float duration, byte red, byte green, byte blue)
        {
            InitWithDuration(duration, red, green, blue);
        }

        protected CCTintTo(CCTintTo tintTo) : base(tintTo)
        {
            InitWithDuration(tintTo.m_fDuration, tintTo.m_to.R, tintTo.m_to.G, tintTo.m_to.B);
        }

        public bool InitWithDuration(float duration, byte red, byte green, byte blue)
        {
            if (base.InitWithDuration(duration))
            {
                m_to = new CCColor3B(red, green, blue);
                return true;
            }

            return false;
        }

        public override object Copy(ICCCopyable zone)
        {
            if (zone != null && zone != null)
            {
                var ret = zone as CCTintTo;
                if (ret == null)
                {
                    return null;
                }

                base.Copy(zone);

                ret.InitWithDuration(m_fDuration, m_to.R, m_to.G, m_to.B);

                return ret;
            }
            else
            {
                return new CCTintTo(this);
            }
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            var protocol = m_pTarget as ICCRGBAProtocol;
            if (protocol != null)
            {
                m_from = protocol.Color;
            }
        }

        public override void Update(float time)
        {
            var protocol = m_pTarget as ICCRGBAProtocol;
            if (protocol != null)
            {
                protocol.Color = new CCColor3B((byte) (m_from.R + (m_to.R - m_from.R) * time),
                                               (byte) (m_from.G + (m_to.G - m_from.G) * time),
                                               (byte) (m_from.B + (m_to.B - m_from.B) * time));
            }
        }
    }
}