
namespace cocos2d
{
    public class CCAnimationFrame : CCObject
    {
        private float m_fDelayUnits;
        private CCSpriteFrame m_pSpriteFrame;
        private PlistDictionary m_pUserInfo;

        public CCSpriteFrame SpriteFrame
        {
            get { return m_pSpriteFrame; }
        }

        public float DelayUnits
        {
            get { return m_fDelayUnits; }
        }

        public PlistDictionary UserInfo
        {
            get { return m_pUserInfo; }
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCAnimationFrame pCopy;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCAnimationFrame) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCAnimationFrame();
            }

            pCopy.InitWithSpriteFrame((CCSpriteFrame) m_pSpriteFrame.Copy(), m_fDelayUnits, m_pUserInfo);

            return pCopy;
        }

        public bool InitWithSpriteFrame(CCSpriteFrame spriteFrame, float delayUnits, PlistDictionary userInfo)
        {
            m_pSpriteFrame = spriteFrame;
            m_fDelayUnits = delayUnits;
            m_pUserInfo = userInfo;
            return true;
        }
    }
}