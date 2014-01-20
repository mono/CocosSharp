namespace CocosSharp
{
    public class CCFlipY : CCActionInstant
    {
        private bool m_bFlipY;


        #region Constructors

        public CCFlipY()
        {
        }

        public CCFlipY(bool y)
        {
            InitWithFlipY(y);
        }

        protected virtual bool InitWithFlipY(bool y)
        {
            m_bFlipY = y;
            return true;
        }

        protected CCFlipY(CCFlipY flipY) : base(flipY)
        {
            InitWithFlipY(m_bFlipY);
        }

        #endregion Constructors


        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            ((CCSprite) (target)).FlipY = m_bFlipY;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCFlipY(!m_bFlipY);
        }

        public override object Copy(ICCCopyable pZone)
        {
            return new CCFlipY(this);
        }
    }
}