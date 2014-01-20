namespace CocosSharp
{
    public class CCTintBy : CCActionInterval
    {
        protected short m_deltaB;
        protected short m_deltaG;
        protected short m_deltaR;
        protected short m_fromB;
        protected short m_fromG;
        protected short m_fromR;


        #region Constructors

        public CCTintBy(float duration, short deltaRed, short deltaGreen, short deltaBlue) : base(duration)
        {
            InitCCTintBy(deltaRed, deltaGreen, deltaBlue);
        }

        // Perform deep copy of CCTintBy
        protected CCTintBy(CCTintBy tintBy) : base(tintBy)
        {
            InitCCTintBy(tintBy.m_deltaR, tintBy.m_deltaG, tintBy.m_deltaB);
        }

        private void InitCCTintBy(short deltaRed, short deltaGreen, short deltaBlue)
        {
            m_deltaR = deltaRed;
            m_deltaG = deltaGreen;
            m_deltaB = deltaBlue;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCTintBy(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);

            var protocol = target as ICCColor;
            if (protocol != null)
            {
                CCColor3B color = protocol.Color;
                m_fromR = color.R;
                m_fromG = color.G;
                m_fromB = color.B;
            }
        }

        public override void Update(float time)
        {
            var protocol = m_pTarget as ICCColor;
            if (protocol != null)
            {
                protocol.Color = new CCColor3B((byte) (m_fromR + m_deltaR * time),
                                               (byte) (m_fromG + m_deltaG * time),
                                               (byte) (m_fromB + m_deltaB * time));
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCTintBy(m_fDuration, (short) -m_deltaR, (short) -m_deltaG, (short) -m_deltaB);
        }
    }
}