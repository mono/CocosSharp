using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class SpriteAnimationSplit : SpriteTestDemo
    {
        public SpriteAnimationSplit()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCTexture2D texture = CCTextureCache.SharedTextureCache.AddImage("animations/dragon_animation");

            // manually add frames to the frame cache
            CCSpriteFrame frame0 = new CCSpriteFrame(texture, new CCRect(132 * 0, 132 * 0, 132, 132));
            CCSpriteFrame frame1 = new CCSpriteFrame(texture, new CCRect(132 * 1, 132 * 0, 132, 132));
            CCSpriteFrame frame2 = new CCSpriteFrame(texture, new CCRect(132 * 2, 132 * 0, 132, 132));
            CCSpriteFrame frame3 = new CCSpriteFrame(texture, new CCRect(132 * 3, 132 * 0, 132, 132));
            CCSpriteFrame frame4 = new CCSpriteFrame(texture, new CCRect(132 * 0, 132 * 1, 132, 132));
            CCSpriteFrame frame5 = new CCSpriteFrame(texture, new CCRect(132 * 1, 132 * 1, 132, 132));

            //
            // Animation using Sprite BatchNode
            //
            CCSprite sprite = new CCSprite(frame0);
            sprite.Position = (new CCPoint(s.Width / 2 - 80, s.Height / 2));
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
            CCActionInterval seq = (CCActionInterval)(CCSequence.FromActions(animate,
                               new CCFlipX(true),
                              (CCFiniteTimeAction)animate.Copy(),
                               new CCFlipX(false)
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
