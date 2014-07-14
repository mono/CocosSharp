using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteOffsetAnchorRotation : SpriteTestDemo
    {
        const int numOfSprites = 3;

        CCSprite[] sprites;
        CCSprite[] pointSprites;

        CCAnimation animation;


        #region Properties

        public override string Title
        {
            get { return "Sprite offset + anchor + rot"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteOffsetAnchorRotation()
        {
            sprites = new CCSprite[numOfSprites];
            pointSprites = new CCSprite[numOfSprites];

            // Create actions

            CCSpriteFrameCache cache = CCSpriteFrameCache.SharedSpriteFrameCache;
            cache.AddSpriteFrames("animations/grossini.plist");
            cache.AddSpriteFrames("animations/grossini_gray.plist", "animations/grossini_gray");
            var animFrames = new List<CCSpriteFrame>();
            string tmp = "";
            for (int j = 0; j < 14; j++)
            {
                tmp = string.Format("grossini_dance_{0:00}.png", j + 1);
                CCSpriteFrame frame = cache[tmp];
                animFrames.Add(frame);
            }

            animation = new CCAnimation(animFrames, 0.3f);

            for (int i = 0; i < numOfSprites; i++)
            {
                // Animation using Sprite batch
                sprites[i] = new CCSprite("grossini_dance_01.png");
                AddChild(sprites[i], 0);

                pointSprites[i] = new CCSprite("Images/r1");
                AddChild(pointSprites[i], 1);
            }
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Scene.VisibleBoundsWorldspace.Size;

            for(int i = 0; i < numOfSprites; i++) 
            {
                sprites[i].Position = (new CCPoint(windowSize.Width / 4 * (i + 1), windowSize.Height / 2));
                pointSprites[i].Scale = 0.25f;
                pointSprites[i].Position = sprites[i].Position;

                switch(i)
                {
                    case 0:
                        sprites[i].AnchorPoint = new CCPoint(0, 0);
                        break;
                    case 1:
                        sprites[i].AnchorPoint = (new CCPoint(0.5f, 0.5f));
                        break;
                    case 2:
                        sprites[i].AnchorPoint = (new CCPoint(1, 1));
                        break;
                }

                sprites[i].RunAction(new CCRepeatForever(new CCAnimate(animation)));
                sprites[i].RunAction(new CCRepeatForever (new CCRotateBy (10, 360)));
            }
        }

        #endregion Setup content


        public override void OnExit()
        {
            base.OnExit();
            CCSpriteFrameCache cache = CCSpriteFrameCache.SharedSpriteFrameCache;
            cache.RemoveSpriteFrames("animations/grossini.plist");
            cache.RemoveSpriteFrames("animations/grossini_gray.plist");
        }
    }
}
