namespace Cocos2D
{
    public class CCFadeOut : CCActionInterval
    {
        public CCFadeOut (float d)
        {
            InitWithDuration(d);
        }

        protected CCFadeOut (CCFadeOut fadeOut) : base (fadeOut)
        {}

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = (CCFadeOut) (pZone);
                base.Copy(pZone);
                return pCopy;
            }
            else
            {
                return new CCFadeOut(this);
            }
        }

        public override void Update(float time)
        {
            var pRGBAProtocol = m_pTarget as ICCRGBAProtocol;
            if (pRGBAProtocol != null)
            {
                pRGBAProtocol.Opacity = (byte) (255 * (1 - time));
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCFadeIn (m_fDuration);
        }
    }
}