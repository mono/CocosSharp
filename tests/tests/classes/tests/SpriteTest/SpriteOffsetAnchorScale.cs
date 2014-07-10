using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteOffsetAnchorScale : SpriteTestDemo
    {
        const int numOfSprites = 3;

        CCSprite[] sprites;
        CCSprite[] pointSprites;

        CCAnimation animation;
        CCFiniteTimeAction seq_scale;


        #region Properties

        public override string Title 
        {
            get { return "Sprite offset + anchor + scale"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteOffsetAnchorScale()
        {
            CCSpriteFrameCache cache = CCApplication.SharedApplication.SpriteFrameCache;
            cache.AddSpriteFrames("animations/grossini.plist");
            cache.AddSpriteFrames("animations/grossini_gray.plist", "animations/grossini_gray");

            sprites = new CCSprite[numOfSprites];
            pointSprites = new CCSprite[numOfSprites];

            var animFrames = new List<CCSpriteFrame>(14);
            string str = "";
            for (int j = 0; j < 14; j++)
            {
                string temp = "";
                if (j + 1 < 10)
                {
                    temp = "0" + (j + 1);
                }
                else
                {
                    temp = (j + 1).ToString();
                }
                str = string.Format("grossini_dance_{0}.png", temp);
                CCSpriteFrame frame = cache[str];
                animFrames.Add(frame);
            }

            animation = new CCAnimation(animFrames, 0.3f);

            for (int i = 0; i < numOfSprites; i++)
            {
                // Animation using Sprite BatchNode
                sprites[i] = new CCSprite("grossini_dance_01.png");
                AddChild(sprites[i], 0);

                pointSprites[i] = new CCSprite("Images/r1");
                AddChild(pointSprites[i], 1);


                CCActionInterval scale = new CCScaleBy(2, 2);
                CCActionInterval scale_back = (CCActionInterval)scale.Reverse();
                seq_scale = (CCActionInterval)(new CCSequence(scale, scale_back));
            }
        }

        #endregion Constructors


        #region Setup content

        public void OnEnter()
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
                sprites[i].RunAction(new CCRepeatForever ((CCActionInterval)seq_scale));
            }
        }

        #endregion Setup content


        public override void OnExit()
        {
            base.OnExit();
            CCSpriteFrameCache cache = CCApplication.SharedApplication.SpriteFrameCache;
            cache.RemoveSpriteFrames("animations/grossini.plist");
            cache.RemoveSpriteFrames("animations/grossini_gray.plist");
        }
    }
}
