

namespace CocosSharp
{
    public class CCAnimationFrame : ICCCopyable<CCAnimationFrame>
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


        #region Constructors

        public CCAnimationFrame(CCSpriteFrame spriteFrame, float delayUnits, PlistDictionary userInfo)
        {
            InitCCAnimationFrame(spriteFrame, delayUnits, userInfo);
        }

        protected CCAnimationFrame(CCAnimationFrame animFrame)
        {
            InitCCAnimationFrame(animFrame.m_pSpriteFrame.Copy(), animFrame.m_fDelayUnits, animFrame.m_pUserInfo);
        }

        private void InitCCAnimationFrame(CCSpriteFrame spriteFrame, float delayUnits, PlistDictionary userInfo)
        {
            m_pSpriteFrame = spriteFrame;
            m_fDelayUnits = delayUnits;
            m_pUserInfo = userInfo;
        }

        #endregion Constructors


        public CCAnimationFrame Copy()
        {
            return new CCAnimationFrame(this);
        }

    }
}