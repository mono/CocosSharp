namespace cocos2d
{
    public class CCFadeIn : CCActionInterval
    {
        public CCFadeIn (float d)
        {
            InitWithDuration(d);
        }

        protected CCFadeIn (CCFadeIn fadeIn) : base (fadeIn) 
        { }

        public override object Copy(ICopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = (CCFadeIn) (pZone);
                base.Copy(pZone);
                return pCopy;
            }
            else
            {
                return new CCFadeIn(this);
            }

        }

        public override void Update(float time)
        {
            var pRGBAProtocol = m_pTarget as ICCRGBAProtocol;
            if (pRGBAProtocol != null)
            {
                pRGBAProtocol.Opacity = (byte) (255 * time);
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCFadeOut (m_fDuration);
        }
    }
}