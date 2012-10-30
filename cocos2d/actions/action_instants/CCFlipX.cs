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

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCFlipX pRet;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pRet = (CCFlipX) (pZone.m_pCopyObject);
            }
            else
            {
                pRet = new CCFlipX();
                pZone = new CCZone(pRet);
            }

            base.CopyWithZone(pZone);
            pRet.InitWithFlipX(m_bFlipX);
            return pRet;
        }
    }
}