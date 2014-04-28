using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class AnimationCache : SpriteTestDemo
    {
        public AnimationCache()
        {
            var frameCache = CCSpriteFrameCache.Instance;
            frameCache.AddSpriteFrames("animations/grossini.plist");
            frameCache.AddSpriteFrames("animations/grossini_gray.plist");
            frameCache.AddSpriteFrames("animations/grossini_blue.plist");

            //
            // create animation "dance"
            //
            var animFrames = new List<CCSpriteFrame>(15);
            string str = "";
            for (int i = 1; i < 15; i++)
            {
                str = string.Format("grossini_dance_{0:00}.png", i);
                CCSpriteFrame frame = CCSpriteFrameCache.Instance[str];
                animFrames.Add(frame);
            }

            CCAnimation animation = new CCAnimation(animFrames, 0.2f);

            // Add an animation to the Cache
            CCAnimationCache.Instance.AddAnimation(animation, "dance");

            //
            // create animation "dance gray"
            //
            animFrames.Clear();

            for (int i = 1; i < 15; i++)
            {
                str = String.Format("grossini_dance_gray_{0:00}.png", i);
                CCSpriteFrame frame = CCSpriteFrameCache.Instance[str];
                animFrames.Add(frame);
            }

            animation = new CCAnimation(animFrames, 0.2f);

            // Add an animation to the Cache
            CCAnimationCache.Instance.AddAnimation(animation, "dance_gray");

            //
            // create animation "dance blue"
            //
            animFrames.Clear();

            for (int i = 1; i < 4; i++)
            {
                str = String.Format("grossini_blue_{0:00}.png", i);
                CCSpriteFrame frame = CCSpriteFrameCache.Instance[str];
                animFrames.Add(frame);
            }

            animation = new CCAnimation(animFrames, 0.2f);

            // Add an animation to the Cache
            CCAnimationCache.Instance.AddAnimation(animation, "dance_blue");


            CCAnimationCache animCache = CCAnimationCache.Instance;

            CCAnimation normal = animCache["dance"];
            normal.RestoreOriginalFrame = true;
            CCAnimation dance_grey = animCache["dance_gray"];
            dance_grey.RestoreOriginalFrame = true;
            CCAnimation dance_blue = animCache["dance_blue"];
            dance_blue.RestoreOriginalFrame = true;

            CCAnimate animN = new CCAnimate (normal);
            CCAnimate animG = new CCAnimate (dance_grey);
            CCAnimate animB = new CCAnimate (dance_blue);

            CCFiniteTimeAction seq = new CCSequence(animN, animG, animB);

            // create an sprite without texture
            CCSprite grossini = new CCSprite();
            grossini.DisplayFrame = frameCache["grossini_dance_01.png"];

            CCSize winSize = CCDirector.SharedDirector.WinSize;
            grossini.Position = (new CCPoint(winSize.Width / 2, winSize.Height / 2));
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
