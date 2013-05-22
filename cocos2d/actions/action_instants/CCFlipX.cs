namespace Cocos2D
{
    public class CCFlipX : CCActionInstant
    {
        private bool m_bFlipX;

        protected CCFlipX()
        {
        }
        public CCFlipX(bool x)
        {
            InitWithFlipX(x);
        }

        protected virtual bool InitWithFlipX(bool x)
        {
            m_bFlipX = x;
            return true;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            ((CCSprite) (target)).FlipX = m_bFlipX;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCFlipX(!m_bFlipX);
        }

        public override object Copy(ICopyable pZone)
        {
            CCFlipX pRet;

            if (pZone != null)
            {
                pRet = (CCFlipX) (pZone);
            }
            else
            {
                pRet = new CCFlipX();
                pZone =  (pRet);
            }

            base.Copy(pZone);
            pRet.InitWithFlipX(m_bFlipX);
            return pRet;
        }
    }
}