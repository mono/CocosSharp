namespace Cocos2D
{
    public class CCFlipY : CCActionInstant
    {
        private bool m_bFlipY;

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

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            ((CCSprite) (target)).FlipY = m_bFlipY;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCFlipY(!m_bFlipY);
        }

        public override object Copy(ICopyable pZone)
        {
            CCFlipY pRet;

            if (pZone != null)
            {
                pRet = (CCFlipY) (pZone);
            }
            else
            {
                pRet = new CCFlipY();
                pZone =  (pRet);
            }

            base.Copy(pZone);
            pRet.InitWithFlipY(m_bFlipY);
            return pRet;
        }
    }
}