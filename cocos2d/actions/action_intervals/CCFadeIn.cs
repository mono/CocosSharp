namespace CocosSharp
{
    public class CCFadeIn : CCActionInterval
    {
        #region Constructors

        public CCFadeIn(float d) : base(d)
        {
        }

        protected CCFadeIn(CCFadeIn fadeIn) : base(fadeIn)
        {
        }

        #endregion Constructors


        public override object Copy(ICCCopyable pZone)
        {
            return new CCFadeIn(this);
        }

        public override void Update(float time)
        {
            var pRGBAProtocol = m_pTarget as ICCColor;
            if (pRGBAProtocol != null)
            {
                pRGBAProtocol.Opacity = (byte) (255 * time);
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCFadeOut(m_fDuration);
        }
    }
}