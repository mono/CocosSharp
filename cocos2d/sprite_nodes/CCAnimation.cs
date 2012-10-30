using System.Collections.Generic;
using System.Diagnostics;

namespace cocos2d
{
    public class CCAnimation : CCObject
    {
        protected bool m_bRestoreOriginalFrame;
        protected float m_fDelayPerUnit;
        protected float m_fTotalDelayUnits;
        protected List<CCAnimationFrame> m_pFrames;
        protected uint m_uLoops;

        public CCAnimation()
        {
            m_pFrames = new List<CCAnimationFrame>();
        }

        public float Duration
        {
            get { return m_fTotalDelayUnits * m_fDelayPerUnit; }
        }

        public float DelayPerUnit
        {
            get { return m_fDelayPerUnit; }
            set { m_fDelayPerUnit = value; }
        }

        public List<CCAnimationFrame> Frames
        {
            get { return m_pFrames; }
        }

        public bool RestoreOriginalFrame
        {
            get { return m_bRestoreOriginalFrame; }
            set { m_bRestoreOriginalFrame = value; }
        }

        public uint Loops
        {
            get { return m_uLoops; }
            set { m_uLoops = value; }
        }

        public float TotalDelayUnits
        {
            get { return m_fTotalDelayUnits; }
        }

        public static CCAnimation Create()
        {
            var pAnimation = new CCAnimation();
            pAnimation.Init();

            return pAnimation;
        }

        public static CCAnimation Create(List<CCSpriteFrame> frames, float delay)
        {
            var pAnimation = new CCAnimation();
            pAnimation.InitWithSpriteFrames(frames, delay);

            return pAnimation;
        }

        public static CCAnimation Create(List<CCAnimationFrame> arrayOfAnimationFrameNames, float delayPerUnit, uint loops)
        {
            var pAnimation = new CCAnimation();
            pAnimation.InitWithAnimationFrames(arrayOfAnimationFrameNames, delayPerUnit, loops);
            return pAnimation;
        }

        public bool Init()
        {
            return InitWithSpriteFrames(new List<CCSpriteFrame>(), 0);
        }

        public bool InitWithSpriteFrames(List<CCSpriteFrame> pFrames, float delay)
        {
            if (pFrames != null)
            {/*
                foreach (CCObject frame in pFrames)
                {
                    Debug.Assert(frame is CCSpriteFrame, "element type is wrong!");
                }
              */
            }

            m_uLoops = 1;
            m_fDelayPerUnit = delay;
            m_pFrames = new List<CCAnimationFrame>();

            if (pFrames != null)
            {
                foreach (CCSpriteFrame pObj in pFrames)
                {
                    var frame = (CCSpriteFrame) pObj;
                    var animFrame = new CCAnimationFrame();
                    animFrame.InitWithSpriteFrame(frame, 1, null);
                    m_pFrames.Add(animFrame);

                    m_fTotalDelayUnits++;
                }
            }

            return true;
        }

        public bool InitWithAnimationFrames(List<CCAnimationFrame> arrayOfAnimationFrames, float delayPerUnit, uint loops)
        {
            if (arrayOfAnimationFrames != null)
            {/*
                foreach (CCObject frame in arrayOfAnimationFrames)
                {
                    Debug.Assert(frame is CCAnimationFrame, "element type is wrong!");
                }
              */
            }

            m_fDelayPerUnit = delayPerUnit;
            m_uLoops = loops;

            m_pFrames = new List<CCAnimationFrame>(arrayOfAnimationFrames);

            foreach (CCAnimationFrame pObj in m_pFrames)
            {
                var animFrame = (CCAnimationFrame) pObj;
                m_fTotalDelayUnits += animFrame.DelayUnits;
            }

            return true;
        }

        public void AddSpriteFrame(CCSpriteFrame pFrame)
        {
            var animFrame = new CCAnimationFrame();
            animFrame.InitWithSpriteFrame(pFrame, 1.0f, null);
            m_pFrames.Add(animFrame);

            // update duration
            m_fTotalDelayUnits++;
        }

        public void AddSpriteFrameWithFileName(string pszFileName)
        {
            CCTexture2D pTexture = CCTextureCache.SharedTextureCache.AddImage(pszFileName);
            CCRect rect = CCRect.Zero;
            rect.size = pTexture.ContentSize;
            CCSpriteFrame pFrame = CCSpriteFrame.Create(pTexture, rect);
            AddSpriteFrame(pFrame);
        }

        public void AddSpriteFrameWithTexture(CCTexture2D pobTexture, CCRect rect)
        {
            CCSpriteFrame pFrame = CCSpriteFrame.Create(pobTexture, rect);
            AddSpriteFrame(pFrame);
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCAnimation pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCAnimation) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCAnimation();
            }

            pCopy.InitWithAnimationFrames(m_pFrames, m_fDelayPerUnit, m_uLoops);
            pCopy.RestoreOriginalFrame = m_bRestoreOriginalFrame;

            return pCopy;
        }
    }
}