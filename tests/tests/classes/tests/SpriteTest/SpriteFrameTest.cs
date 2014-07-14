using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteFrameTest : SpriteTestDemo
    {
        CCSprite sprite1;
        CCSprite sprite2;
        int counter;

        CCAnimation animation;
        CCAnimation animMixed;

        #region Properties

        public override string Title
        {
            get { return "Sprite vs. SpriteBatchNode animation"; }
        }

        public override string Subtitle
        {
            get { return "Testing issue #792"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteFrameTest()
        {
            CCSpriteFrameCache cache = CCSpriteFrameCache.SharedSpriteFrameCache;
            cache.AddSpriteFrames("animations/grossini.plist");
            cache.AddSpriteFrames("animations/grossini_gray.plist", "animations/grossini_gray");
            cache.AddSpriteFrames("animations/grossini_blue.plist", "animations/grossini_blue");

            var animFrames = new List<CCSpriteFrame>(15);

            string str = "";
            for (int i = 1; i < 15; i++)
            {
                str = string.Format("grossini_dance_{0:00}.png", i);
                CCSpriteFrame frame = cache[str];
                animFrames.Add(frame);
            }

            animation = new CCAnimation(animFrames, 0.3f);

            var moreFrames = new List<CCSpriteFrame>(20);
            for (int i = 1; i < 15; i++)
            {
                string temp;
                str = string.Format("grossini_dance_gray_{0:00}.png", i);
                CCSpriteFrame frame = cache[str];
                moreFrames.Add(frame);
            }


            for (int i = 1; i < 5; i++)
            {
                str = string.Format("grossini_blue_{0:00}.png", i);
                CCSpriteFrame frame = cache[str];
                moreFrames.Add(frame);
            }

            // append frames from another batch
            moreFrames.AddRange(animFrames);

            animMixed = new CCAnimation(moreFrames, 0.3f);


            CCSpriteBatchNode spritebatch = new CCSpriteBatchNode("animations/grossini");
            AddChild(spritebatch);

            sprite1 = new CCSprite("grossini_dance_01.png");
            spritebatch.AddChild(sprite1);

            // to test issue #732, uncomment the following line
            sprite1.FlipX = false;
            sprite1.FlipY = false;

            // Animation using standard Sprite
            sprite2 = new CCSprite("grossini_dance_01.png");
            AddChild(sprite2);

            // to test issue #732, uncomment the following line
            sprite2.FlipX = false;
            sprite2.FlipY = false;
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Scene.VisibleBoundsWorldspace.Size;

            sprite1.Position = (new CCPoint(windowSize.Width / 2 -80, windowSize.Height / 2));
            sprite2.Position = (new CCPoint(windowSize.Width / 2 + 80, windowSize.Height / 2));

            sprite1.RunAction(new CCRepeatForever (new CCAnimate (animation)));
            sprite2.RunAction(new CCRepeatForever (new CCAnimate (animMixed)));

            Schedule(StartIn05Secs, 0.5f);
            counter = 0;
        }

        #endregion Setup content


        public override void OnExit()
        {
            base.OnExit();
            CCSpriteFrameCache cache = CCSpriteFrameCache.SharedSpriteFrameCache;
            cache.RemoveSpriteFrames("animations/grossini.plist");
            cache.RemoveSpriteFrames("animations/grossini_gray.plist");
            cache.RemoveSpriteFrames("animations/grossini_blue.plist");
        }

        void StartIn05Secs(float dt)
        {
            Unschedule(StartIn05Secs);
            Schedule(FlipSprites, 1.0f);
        }

        void FlipSprites(float dt)
        {
            counter++;

            bool fx = false;
            bool fy = false;
            int i = counter % 4;

            switch (i)
            {
                case 0:
                    fx = false;
                    fy = false;
                    break;
                case 1:
                    fx = true;
                    fy = false;
                    break;
                case 2:
                    fx = false;
                    fy = true;
                    break;
                case 3:
                    fx = true;
                    fy = true;
                    break;
            }

            sprite1.FlipX = (fx);
            sprite1.FlipY = (fy);
            sprite2.FlipX = (fx);
            sprite2.FlipY = (fy);
        }
    }
}
