namespace CocosSharp
{
    public class CCFadeTo : CCActionInterval
    {
        protected byte m_fromOpacity;
        protected byte m_toOpacity;


        #region Constructors

        public CCFadeTo(float duration, byte opacity) : base(duration)
        {
            InitCCFaceTo(opacity);
        }

        protected CCFadeTo(CCFadeTo fadeTo) : base(fadeTo)
        {
            InitCCFaceTo(fadeTo.m_toOpacity);
        }

        private void InitCCFaceTo(byte opacity)
        {
                m_toOpacity = opacity;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable pZone)
        {
            return new CCFadeTo(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);

            var pRGBAProtocol = target as ICCColor;
            if (pRGBAProtocol != null)
            {
                m_fromOpacity = pRGBAProtocol.Opacity;
            }
        }

        public override void Update(float time)
        {
            var pRGBAProtocol = m_pTarget as ICCColor;
            if (pRGBAProtocol != null)
            {
                pRGBAProtocol.Opacity = (byte) (m_fromOpacity + (m_toOpacity - m_fromOpacity) * time);
            }
        }
    }
}