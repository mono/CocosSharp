using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class SpriteOffsetAnchorScale : SpriteTestDemo
    {
        public SpriteOffsetAnchorScale()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            for (int i = 0; i < 3; i++)
            {
                CCSpriteFrameCache cache = CCSpriteFrameCache.SharedSpriteFrameCache;
                cache.AddSpriteFramesWithFile("animations/grossini.plist");
                cache.AddSpriteFramesWithFile("animations/grossini_gray.plist", "animations/grossini_gray");

                //
                // Animation using Sprite BatchNode
                //
                CCSprite sprite = new CCSprite("grossini_dance_01.png");
                sprite.Position = (new CCPoint(s.Width / 4 * (i + 1), s.Height / 2));

                CCSprite point = new CCSprite("Images/r1");
                point.Scale = 0.25f;
                point.Position = sprite.Position;
                AddChild(point, 1);

                switch (i)
                {
                    case 0:
                        sprite.AnchorPoint = new CCPoint(0, 0);
                        break;
                    case 1:
                        sprite.AnchorPoint = new CCPoint(0.5f, 0.5f);
                        break;
                    case 2:
                        sprite.AnchorPoint = new CCPoint(1, 1);
                        break;
                }

                point.Position = sprite.Position;

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
                    CCSpriteFrame frame = cache.SpriteFrameByName(str);
                    animFrames.Add(frame);
                }

                CCAnimation animation = new CCAnimation(animFrames, 0.3f);
                sprite.RunAction(new CCRepeatForever (new CCAnimate (animation)));

                CCActionInterval scale = new CCScaleBy(2, 2);
                CCActionInterval scale_back = (CCActionInterval)scale.Reverse();
                CCActionInterval seq_scale = (CCActionInterval)(new CCSequence(scale, scale_back));
                sprite.RunAction(new CCRepeatForever (seq_scale));

                AddChild(sprite, 0);

                //animFrames->release();    // win32 : memory leak    2010-0415
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            CCSpriteFrameCache cache = CCSpriteFrameCache.SharedSpriteFrameCache;
            cache.RemoveSpriteFramesFromFile("animations/grossini.plist");
            cache.RemoveSpriteFramesFromFile("animations/grossini_gray.plist");
        }

        public override string title() 
        {
            return "Sprite offset + anchor + scale";
        }
    }
}
