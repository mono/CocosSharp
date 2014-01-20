namespace CocosSharp
{
    public class CCFadeOut : CCActionInterval
    {
        #region Constructors

        public CCFadeOut(float d) : base(d)
        {
        }

        protected CCFadeOut(CCFadeOut fadeOut) : base(fadeOut)
        {
        }

        #endregion Constructors


        public override object Copy(ICCCopyable pZone)
        {
            return new CCFadeOut(this);
        }

        public override void Update(float time)
        {
            var pRGBAProtocol = m_pTarget as ICCColor;
            if (pRGBAProtocol != null)
            {
                pRGBAProtocol.Opacity = (byte) (255 * (1 - time));
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCFadeIn(m_fDuration);
        }
    }
}