using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteAnimationSplit : SpriteTestDemo
    {
        CCSprite sprite;

        CCActionInterval seq;


        #region Properties

        public override string Title
        {
            get { return "Sprite: Animation + flip"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteAnimationSplit()
        {
            var texture = CCApplication.SharedApplication.TextureCache.AddImage("animations/dragon_animation");
            CCSize contentSizeInPixels = texture.ContentSizeInPixels;
            float height = contentSizeInPixels.Height / 4.0f;
            float heightOffset = height / 2.0f;
            float width = contentSizeInPixels.Width / 5.0f;

            // Manually add frames to the frame cache
            // The rects in pixels of each frame are determined from the textureatlas 
            var frame0 = new CCSpriteFrame(texture, new CCRect(width * 0, heightOffset + height * 0, width, height));
            var frame1 = new CCSpriteFrame(texture, new CCRect(width * 1, heightOffset + height * 0, width, height));
            var frame2 = new CCSpriteFrame(texture, new CCRect(width * 2, heightOffset + height * 0, width, height));
            var frame3 = new CCSpriteFrame(texture, new CCRect(width * 3, heightOffset + height * 0, width, height));

            // Note: The height positioning below is a bit of voodoo because the sprite atlas isn't currently packed tightly
            // See the dragon_animation.png file
            var frame4 = new CCSpriteFrame(texture, new CCRect(width * 0, heightOffset * 1.6f + height * 1, width, height));
            var frame5 = new CCSpriteFrame(texture, new CCRect(width * 1, heightOffset * 1.6f + height * 1, width, height));

            // Animation using Sprite BatchNode
            sprite = new CCSprite(frame0);
            AddChild(sprite);

            var animFrames = new List<CCSpriteFrame>(6);
            animFrames.Add(frame0);
            animFrames.Add(frame1);
            animFrames.Add(frame2);
            animFrames.Add(frame3);
            animFrames.Add(frame4);
            animFrames.Add(frame5);

            CCAnimation animation = new CCAnimation(animFrames, 0.2f);
            CCAnimate animate = new CCAnimate (animation);
            seq = new CCSequence(animate, new CCFlipX(true), animate, new CCFlipX(false));
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Scene.VisibleBoundsWorldspace.Size;

            sprite.Position = new CCPoint(windowSize.Width / 2, windowSize.Height / 2);
            sprite.RepeatForever(seq);
        }

        #endregion Setup content


        public override void OnExit()
        {
            base.OnExit();
            CCApplication.SharedApplication.SpriteFrameCache.RemoveUnusedSpriteFrames();
        }

    }
}
