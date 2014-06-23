using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class AnimationCache : SpriteTestDemo
    {
        CCSprite grossini;
        CCFiniteTimeAction seqAnimation;

        #region Properties

        public override string Title
        {
            get { return "AnimationCache"; }
        }

        public override string Subtitle
        {
            get { return "Sprite should be animated"; }
        }

        #endregion Properties


        #region Constructors

        public AnimationCache()
        {
            var frameCache = CCApplication.SharedApplication.SpriteFrameCache;
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
                CCSpriteFrame frame = CCApplication.SharedApplication.SpriteFrameCache[str];
                animFrames.Add(frame);
            }

            CCAnimation animation = new CCAnimation(animFrames, 0.2f);

            // Add an animation to the Cache
            CCApplication.SharedApplication.AnimationCache.AddAnimation(animation, "dance");

            //
            // create animation "dance gray"
            //
            animFrames.Clear();

            for (int i = 1; i < 15; i++)
            {
                str = String.Format("grossini_dance_gray_{0:00}.png", i);
                CCSpriteFrame frame = CCApplication.SharedApplication.SpriteFrameCache[str];
                animFrames.Add(frame);
            }

            animation = new CCAnimation(animFrames, 0.2f);

            // Add an animation to the Cache
            CCApplication.SharedApplication.AnimationCache.AddAnimation(animation, "dance_gray");

            //
            // create animation "dance blue"
            //
            animFrames.Clear();

            for (int i = 1; i < 4; i++)
            {
                str = String.Format("grossini_blue_{0:00}.png", i);
                CCSpriteFrame frame = CCApplication.SharedApplication.SpriteFrameCache[str];
                animFrames.Add(frame);
            }

            animation = new CCAnimation(animFrames, 0.2f);

            // Add an animation to the Cache
            CCApplication.SharedApplication.AnimationCache.AddAnimation(animation, "dance_blue");


            CCAnimationCache animCache = CCApplication.SharedApplication.AnimationCache;

            CCAnimation normal = animCache["dance"];
            normal.RestoreOriginalFrame = true;
            CCAnimation dance_grey = animCache["dance_gray"];
            dance_grey.RestoreOriginalFrame = true;
            CCAnimation dance_blue = animCache["dance_blue"];
            dance_blue.RestoreOriginalFrame = true;

            CCAnimate animN = new CCAnimate (normal);
            CCAnimate animG = new CCAnimate (dance_grey);
            CCAnimate animB = new CCAnimate (dance_blue);

            seqAnimation = new CCSequence(animN, animG, animB);

            grossini = new CCSprite();
            AddChild(grossini);
        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow(windowSize);

            var frameCache = CCApplication.SharedApplication.SpriteFrameCache;

            grossini.SpriteFrame = frameCache["grossini_dance_01.png"];
            grossini.Position = (new CCPoint(windowSize.Width / 2, windowSize.Height / 2));

            // run the animation
            grossini.RunAction(seqAnimation);
        }

        #endregion Setup content
    }
}
