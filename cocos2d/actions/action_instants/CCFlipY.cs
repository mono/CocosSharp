namespace cocos2d
{
    public class CCFlipY : CCActionInstant
    {
        private bool m_bFlipY;

        public static CCFlipY Create(bool y)
        {
            var pRet = new CCFlipY();
            pRet.InitWithFlipY(y);
            return pRet;
        }

        public bool InitWithFlipY(bool y)
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
            return Create(!m_bFlipY);
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCFlipY pRet;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pRet = (CCFlipY) (pZone.m_pCopyObject);
            }
            else
            {
                pRet = new CCFlipY();
                pZone = new CCZone(pRet);
            }

            base.CopyWithZone(pZone);
            pRet.InitWithFlipY(m_bFlipY);
            return pRet;
        }
    }
}