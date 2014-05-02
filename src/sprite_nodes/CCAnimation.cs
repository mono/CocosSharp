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
                    l.Add(cs[f]);
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

		private void InitWithSpriteFrames(List<CCSpriteFrame> frames, float delay)
        {
            Loops = 1;
            DelayPerUnit = delay;

			if (frames == null)
				return;
            
			Frames = new List<CCAnimationFrame> (frames.Count);
			foreach (var frame in frames)
				Frames.Add (new CCAnimationFrame (frame, 1, null));
            TotalDelayUnits = Frames.Count;
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

        public void AddSpriteFrame(CCSpriteFrame frame)
        {
            var animFrame = new CCAnimationFrame(frame, 1.0f, null);
            Frames.Add(animFrame);

            // update duration
            TotalDelayUnits++;
        }

        public void AddSpriteFrame(string filename)
        {
			var texture = CCTextureCache.Instance.AddImage(filename);
            CCRect rect = CCRect.Zero;
			rect.Size = texture.ContentSize;
			AddSpriteFrame (new CCSpriteFrame (texture, rect));
        }

		public void AddSpriteFrame(CCTexture2D texture, CCRect rect)
        {
			AddSpriteFrame(new CCSpriteFrame(texture, rect));
        }
    }
}