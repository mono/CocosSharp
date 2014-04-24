using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CocosSharp
{
    public class CCAnimation
    {
		#region Properties

		public bool RestoreOriginalFrame { get; set; }
		public float DelayPerUnit { get; set; }
		public float TotalDelayUnits { get; private set; }
		public List<CCAnimationFrame> Frames { get; private set; }
		public uint Loops { get; set; }

        public float Duration
        {
			get { return TotalDelayUnits * DelayPerUnit; }
        }

		#endregion Properties


        #region Constructors

        public CCAnimation() : this(new List<CCSpriteFrame>(), 0)
        { 
		}

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

		public CCAnimation(CCSpriteSheet cs, float delay) : this(cs.Frames, delay)
        {
        }

        // Perform deep copy of CCAnimation
        protected CCAnimation(CCAnimation animation) : this(animation.Frames, animation.DelayPerUnit, animation.Loops)
        {
            RestoreOriginalFrame = animation.RestoreOriginalFrame;
        }

        public CCAnimation (List<CCSpriteFrame> frames, float delay)
        {
            InitWithSpriteFrames(frames, delay);
        }

        public CCAnimation (List<CCAnimationFrame> arrayOfAnimationFrameNames, float delayPerUnit, uint loops)
        {
			DelayPerUnit = delayPerUnit;
			Loops = loops;

			Frames = new List<CCAnimationFrame>(arrayOfAnimationFrameNames);

			TotalDelayUnits = Frames.Sum(animFrame => animFrame.DelayUnits);
        }

        private void InitWithSpriteFrames(List<CCSpriteFrame> pFrames, float delay)
        {
            Loops = 1;
            DelayPerUnit = delay;

            if (pFrames != null)
            {
                Frames = pFrames.Select(frame => new CCAnimationFrame(frame, 1, null)).ToList();
                TotalDelayUnits = Frames.Count;
            }
        }

		public CCAnimation Copy()
		{
			return new CCAnimation(this);
		}

        #endregion Constructors


		public void AddSpriteFrame(CCSprite sprite)
        {
            CCSpriteFrame f = new CCSpriteFrame(sprite.Texture, new CCRect(0, 0, sprite.ContentSize.Width, sprite.ContentSize.Height));
            AddSpriteFrame(f);
        }

        public void AddSpriteFrame(CCSpriteFrame pFrame)
        {
            var animFrame = new CCAnimationFrame(pFrame, 1.0f, null);
            Frames.Add(animFrame);

            // update duration
            TotalDelayUnits++;
        }

        public void AddSpriteFrame(string pszFileName)
        {
            CCTexture2D pTexture = CCTextureCache.SharedTextureCache.AddImage(pszFileName);
            CCRect rect = CCRect.Zero;
            rect.Size = pTexture.ContentSize;
            CCSpriteFrame pFrame = new CCSpriteFrame(pTexture, rect);
            AddSpriteFrame(pFrame);
        }

        public void AddSpriteFrame(CCTexture2D pobTexture, CCRect rect)
        {
            CCSpriteFrame pFrame = new CCSpriteFrame(pobTexture, rect);
            AddSpriteFrame(pFrame);
        }
    }
}