using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteAnimationSplit : SpriteTestDemo
    {
        public SpriteAnimationSplit()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCTexture2D texture = CCTextureCache.SharedTextureCache.AddImage("animations/dragon_animation");

            // manually add frames to the frame cache
            CCSpriteFrame frame0 = CCSpriteFrame.Create(texture, new CCRect(132 * 0, 132 * 0, 132, 132));
            CCSpriteFrame frame1 = CCSpriteFrame.Create(texture, new CCRect(132 * 1, 132 * 0, 132, 132));
            CCSpriteFrame frame2 = CCSpriteFrame.Create(texture, new CCRect(132 * 2, 132 * 0, 132, 132));
            CCSpriteFrame frame3 = CCSpriteFrame.Create(texture, new CCRect(132 * 3, 132 * 0, 132, 132));
            CCSpriteFrame frame4 = CCSpriteFrame.Create(texture, new CCRect(132 * 0, 132 * 1, 132, 132));
            CCSpriteFrame frame5 = CCSpriteFrame.Create(texture, new CCRect(132 * 1, 132 * 1, 132, 132));

            //
            // Animation using Sprite BatchNode
            //
            CCSprite sprite = CCSprite.Create(frame0);
            sprite.Position = (new CCPoint(s.Width / 2 - 80, s.Height / 2));
            AddChild(sprite);

            var animFrames = new List<CCSpriteFrame>(6);
            animFrames.Add(frame0);
            animFrames.Add(frame1);
            animFrames.Add(frame2);
            animFrames.Add(frame3);
            animFrames.Add(frame4);
            animFrames.Add(frame5);

            CCAnimation animation = CCAnimation.Create(animFrames, 0.2f);
            CCAnimate animate = new CCAnimate (animation);
            CCActionInterval seq = (CCActionInterval)(CCSequence.FromActions(animate,
                               CCFlipX.Create(true),
                              (CCFiniteTimeAction)animate.Copy(),
                               CCFlipX.Create(false)
                               ));

            sprite.RunAction(new CCRepeatForever (seq));
            //animFrames->release();    // win32 : memory leak    2010-0415
        }

        public override void OnExit()
        {
            base.OnExit();
            CCSpriteFrameCache.SharedSpriteFrameCache.RemoveUnusedSpriteFrames();
        }

        public override string title()
        {
            return "Sprite: Animation + flip";
        }
    }
}
