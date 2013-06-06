

namespace Cocos2D
{
    public class CCAnimationFrame : ICCCopyable
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

		public CCAnimationFrame Copy()
		{
			return (CCAnimationFrame)Copy(null);
		}

        public object Copy(ICCCopyable pZone)
        {
            CCAnimationFrame pCopy;
            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = (CCAnimationFrame) (pZone);
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