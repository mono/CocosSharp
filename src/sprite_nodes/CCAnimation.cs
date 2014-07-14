using System.Collections.Generic;
using System.Diagnostics;

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

            float totalDelatUnits = 0.0f;
            foreach (CCAnimationFrame frame in Frames) { totalDelatUnits += frame.DelayUnits; }

            TotalDelayUnits = totalDelatUnits;
        }

        void InitWithSpriteFrames(List<CCSpriteFrame> frames, float delay)
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
            CCRect textureRect = sprite.TextureRectInPixels;
            CCSpriteFrame f = new CCSpriteFrame(sprite.ContentSize, sprite.Texture, textureRect);
            AddSpriteFrame(f);
        }

        public void AddSpriteFrame(CCSpriteFrame frame)
        {
            var animFrame = new CCAnimationFrame(frame, 1.0f, null);
            Frames.Add(animFrame);

            // update duration
            TotalDelayUnits++;
        }

        public void AddSpriteFrame(string filename, CCSize? contentSize=null)
        {
            var texture = CCTextureCache.SharedTextureCache.AddImage(filename);
            CCRect rect = CCRect.Zero;
            rect.Size = texture.ContentSizeInPixels;

            if(contentSize == null)
                contentSize = rect.Size;

            AddSpriteFrame(new CCSpriteFrame ((CCSize)contentSize, texture, rect));
        }

        public void AddSpriteFrame(CCTexture2D texture, CCRect textureRect, CCSize? contentSize=null)
        {
            if(contentSize == null)
                contentSize = textureRect.Size;

            AddSpriteFrame(new CCSpriteFrame((CCSize)contentSize, texture, textureRect));
        }
    }
}