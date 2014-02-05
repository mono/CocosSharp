using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CocosSharp
{
    public class CCAnimation : ICCCopyable<CCAnimation>
    {
        protected bool m_bRestoreOriginalFrame;
        protected float m_fDelayPerUnit;
        protected float m_fTotalDelayUnits;
        protected List<CCAnimationFrame> m_pFrames;
        protected uint m_uLoops;


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


        #region Constructors

        public CCAnimation() : this (new List<CCSpriteFrame>(), 0)
        { }

        public CCAnimation(CCSpriteSheet cs, string[] frames, float delay)
        {
            List<CCSpriteFrame> l = new List<CCSpriteFrame>();
            foreach(string f in frames) 
            {
                CCSpriteFrame cf = cs[f];
                if (cf != null)
                {
                    l.Add(cs[f]);
                }
            }
            InitWithSpriteFrames(l, delay);
        }

        public CCAnimation(CCSpriteSheet cs, float delay)
        {
            InitWithSpriteFrames(cs.Frames, delay);
        }

        // Perform deep copy of CCAnimation
        protected CCAnimation(CCAnimation animation) : this(animation.m_pFrames, animation.m_fDelayPerUnit, animation.m_uLoops)
        {
            RestoreOriginalFrame = animation.m_bRestoreOriginalFrame;
        }

        public CCAnimation (List<CCSpriteFrame> frames, float delay)
        {
            InitWithSpriteFrames(frames, delay);
        }

        public CCAnimation (List<CCAnimationFrame> arrayOfAnimationFrameNames, float delayPerUnit, uint loops)
        {
            InitWithAnimationFrames(arrayOfAnimationFrameNames, delayPerUnit, loops);
        }

        private void InitWithSpriteFrames(List<CCSpriteFrame> pFrames, float delay)
        {
            if (pFrames != null)
            {/*
                foreach (object frame in pFrames)
                {
                    Debug.Assert(frame is CCSpriteFrame, "element type is wrong!");
                }
              */
            }

            m_uLoops = 1;
            m_fDelayPerUnit = delay;

            if (pFrames != null)
            {
                m_pFrames = pFrames.Select(frame => new CCAnimationFrame(frame, 1, null)).ToList();
                m_fTotalDelayUnits = m_pFrames.Count;
            }
        }

        private void InitWithAnimationFrames(List<CCAnimationFrame> arrayOfAnimationFrames, float delayPerUnit, uint loops)
        {
            m_fDelayPerUnit = delayPerUnit;
            m_uLoops = loops;

            m_pFrames = new List<CCAnimationFrame>(arrayOfAnimationFrames);

            m_fTotalDelayUnits = m_pFrames.Sum(animFrame => animFrame.DelayUnits);
        }

        #endregion Constructors

        public CCAnimation DeepCopy()
        {
            return new CCAnimation(this);
        }

        public void AddSprite(CCSprite sprite)
        {
            CCSpriteFrame f = new CCSpriteFrame(sprite.Texture, new CCRect(0, 0, sprite.ContentSize.Width, sprite.ContentSize.Height));
            AddSpriteFrame(f);
        }

        public void AddSpriteFrame(CCSpriteFrame pFrame)
        {
            var animFrame = new CCAnimationFrame(pFrame, 1.0f, null);
            m_pFrames.Add(animFrame);

            // update duration
            m_fTotalDelayUnits++;
        }

        public void AddSpriteFrameWithFileName(string pszFileName)
        {
            CCTexture2D pTexture = CCTextureCache.SharedTextureCache.AddImage(pszFileName);
            CCRect rect = CCRect.Zero;
            rect.Size = pTexture.ContentSize;
            CCSpriteFrame pFrame = new CCSpriteFrame(pTexture, rect);
            AddSpriteFrame(pFrame);
        }

        public void AddSpriteFrameWithTexture(CCTexture2D pobTexture, CCRect rect)
        {
            CCSpriteFrame pFrame = new CCSpriteFrame(pobTexture, rect);
            AddSpriteFrame(pFrame);
        }
    }
}