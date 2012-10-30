using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class AnimationCache : SpriteTestDemo
    {
        public AnimationCache()
        {
            var frameCache = CCSpriteFrameCache.SharedSpriteFrameCache;
            frameCache.AddSpriteFramesWithFile("animations/grossini.plist");
            frameCache.AddSpriteFramesWithFile("animations/grossini_gray.plist");
            frameCache.AddSpriteFramesWithFile("animations/grossini_blue.plist");

            //
            // create animation "dance"
            //
            var animFrames = new List<CCSpriteFrame>(15);
            string str = "";
            for (int i = 1; i < 15; i++)
            {
                str = string.Format("grossini_dance_{0:00}.png", i);
                CCSpriteFrame frame = CCSpriteFrameCache.SharedSpriteFrameCache.SpriteFrameByName(str);
                animFrames.Add(frame);
            }

            CCAnimation animation = CCAnimation.Create(animFrames, 0.2f);

            // Add an animation to the Cache
            CCAnimationCache.SharedAnimationCache.AddAnimation(animation, "dance");

            //
            // create animation "dance gray"
            //
            animFrames.Clear();

            for (int i = 1; i < 15; i++)
            {
                str = String.Format("grossini_dance_gray_{0:00}.png", i);
                CCSpriteFrame frame = CCSpriteFrameCache.SharedSpriteFrameCache.SpriteFrameByName(str);
                animFrames.Add(frame);
            }

            animation = CCAnimation.Create(animFrames, 0.2f);

            // Add an animation to the Cache
            CCAnimationCache.SharedAnimationCache.AddAnimation(animation, "dance_gray");

            //
            // create animation "dance blue"
            //
            animFrames.Clear();

            for (int i = 1; i < 4; i++)
            {
                str = String.Format("grossini_blue_{0:00}.png", i);
                CCSpriteFrame frame = CCSpriteFrameCache.SharedSpriteFrameCache.SpriteFrameByName(str);
                animFrames.Add(frame);
            }

            animation = CCAnimation.Create(animFrames, 0.2f);

            // Add an animation to the Cache
            CCAnimationCache.SharedAnimationCache.AddAnimation(animation, "dance_blue");


            CCAnimationCache animCache = CCAnimationCache.SharedAnimationCache;

            CCAnimation normal = animCache.AnimationByName("dance");
            normal.RestoreOriginalFrame = true;
            CCAnimation dance_grey = animCache.AnimationByName("dance_gray");
            dance_grey.RestoreOriginalFrame = true;
            CCAnimation dance_blue = animCache.AnimationByName("dance_blue");
            dance_blue.RestoreOriginalFrame = true;

            CCAnimate animN = CCAnimate.Create(normal);
            CCAnimate animG = CCAnimate.Create(dance_grey);
            CCAnimate animB = CCAnimate.Create(dance_blue);

            CCFiniteTimeAction seq = CCSequence.Create(animN, animG, animB);

            // create an sprite without texture
            CCSprite grossini = CCSprite.Create();
            grossini.DisplayFrame = frameCache.SpriteFrameByName("grossini_dance_01.png");

            CCSize winSize = CCDirector.SharedDirector.WinSize;
            grossini.Position = (new CCPoint(winSize.width / 2, winSize.height / 2));
            AddChild(grossini);

            // run the animation
            grossini.RunAction(seq);
        }

        public override string title()
        {
            return "AnimationCache";
        }

        public override string subtitle()
        {
            return "Sprite should be animated";
        }
    }
}
