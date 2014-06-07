using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteAnimationSplit : SpriteTestDemo
    {
        public SpriteAnimationSplit()
        {
			var s = CCApplication.SharedApplication.MainWindowDirector.WindowSizeInPoints;

			var texture = CCTextureCache.Instance.AddImage("animations/dragon_animation");

            // manually add frames to the frame cache
			var frame0 = new CCSpriteFrame(texture, new CCRect(132 * 0, 132 * 0, 132, 132));
			var frame1 = new CCSpriteFrame(texture, new CCRect(132 * 1, 132 * 0, 132, 132));
			var frame2 = new CCSpriteFrame(texture, new CCRect(132 * 2, 132 * 0, 132, 132));
			var frame3 = new CCSpriteFrame(texture, new CCRect(132 * 3, 132 * 0, 132, 132));
			var frame4 = new CCSpriteFrame(texture, new CCRect(132 * 0, 132 * 1, 132, 132));
			var frame5 = new CCSpriteFrame(texture, new CCRect(132 * 1, 132 * 1, 132, 132));

            //
            // Animation using Sprite BatchNode
            //
			var sprite = new CCSprite(frame0);
            sprite.Position = new CCPoint(s.Width / 2 - 80, s.Height / 2);
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
            CCActionInterval seq = new CCSequence(animate,
                               new CCFlipX(true),
                              animate,
                               new CCFlipX(false)
                               );

			sprite.RepeatForever(seq);
            //animFrames->release();    // win32 : memory leak    2010-0415
        }

        public override void OnExit()
        {
            base.OnExit();
            CCSpriteFrameCache.Instance.RemoveUnusedSpriteFrames();
        }

        public override string title()
        {
            return "Sprite: Animation + flip";
        }
    }
}
