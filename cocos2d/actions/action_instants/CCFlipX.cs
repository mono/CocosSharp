namespace cocos2d
{
    public class CCFlipX : CCActionInstant
    {
        private bool m_bFlipX;

        public static CCFlipX Create(bool x)
        {
            var pRet = new CCFlipX();
            pRet.InitWithFlipX(x);
            return pRet;
        }

        public bool InitWithFlipX(bool x)
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
            return Create(!m_bFlipX);
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