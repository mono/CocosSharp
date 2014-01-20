namespace CocosSharp
{
    public class CCTintTo : CCActionInterval
    {
        protected CCColor3B m_from;
        protected CCColor3B m_to;


        #region Constructors

        public CCTintTo(float duration, byte red, byte green, byte blue) : base(duration)
        {
            InitCCTintTo(red, green, blue);
        }

        // Perform deep copy of CCTintTo
        protected CCTintTo(CCTintTo tintTo) : base(tintTo)
        {
            InitCCTintTo(tintTo.m_to.R, tintTo.m_to.G, tintTo.m_to.B);
        }

        private void InitCCTintTo(byte red, byte green, byte blue)
        {
            m_to = new CCColor3B(red, green, blue);
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCTintTo(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            var protocol = m_pTarget as ICCColor;
            if (protocol != null)
            {
                m_from = protocol.Color;
            }
        }

        public override void Update(float time)
        {
            var protocol = m_pTarget as ICCColor;
            if (protocol != null)
            {
                protocol.Color = new CCColor3B((byte) (m_from.R + (m_to.R - m_from.R) * time),
                                               (byte) (m_from.G + (m_to.G - m_from.G) * time),
                                               (byte) (m_from.B + (m_to.B - m_from.B) * time));
            }
        }
    }
}