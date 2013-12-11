namespace CocosSharp
{
    public class CCFadeTo : CCActionInterval
    {
        protected byte m_fromOpacity;
        protected byte m_toOpacity;

        public CCFadeTo(float duration, byte opacity)
        {
            InitWithDuration(duration, opacity);
        }

        protected CCFadeTo(CCFadeTo fadeTo) : base(fadeTo)
        {
            InitWithDuration(fadeTo.m_fDuration, fadeTo.m_toOpacity);
        }

        protected bool InitWithDuration(float duration, byte opacity)
        {
            if (base.InitWithDuration(duration))
            {
                m_toOpacity = opacity;
                return true;
            }

            return false;
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = (CCFadeTo) (pZone);
                base.Copy(pZone);

                pCopy.InitWithDuration(m_fDuration, m_toOpacity);

                return pCopy;
            }
            else
            {
                return new CCFadeTo(this);
            }
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);

            var pRGBAProtocol = target as ICCRGBAProtocol;
            if (pRGBAProtocol != null)
            {
                m_fromOpacity = pRGBAProtocol.Opacity;
            }
        }

        public override void Update(float time)
        {
            var pRGBAProtocol = m_pTarget as ICCRGBAProtocol;
            if (pRGBAProtocol != null)
            {
                pRGBAProtocol.Opacity = (byte) (m_fromOpacity + (m_toOpacity - m_fromOpacity) * time);
            }
        }
    }
}