using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class SpriteOffsetAnchorFlip : SpriteTestDemo
    {
        public SpriteOffsetAnchorFlip()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            for (int i = 0; i < 3; i++)
            {
                CCSpriteFrameCache cache = CCSpriteFrameCache.SharedSpriteFrameCache;
                cache.AddSpriteFramesWithFile("animations/grossini.plist");
                cache.AddSpriteFramesWithFile("animations/grossini_gray.plist", "animations/grossini_gray");

                //
                // Animation using Sprite batch
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
                        sprite.AnchorPoint = (new CCPoint(0.5f, 0.5f));
                        break;
                    case 2:
                        sprite.AnchorPoint = new CCPoint(1, 1);
                        break;
                }

                point.Position = sprite.Position;

                var animFrames = new List<CCSpriteFrame>();
                string tmp = "";
                for (int j = 0; j < 14; j++)
                {
                    string temp = "";
                    if (j +1< 10)
                    {
                        temp = "0" + (j + 1);
                    }
                    else
                    {
                        temp = (j + 1).ToString();
                    }
                    tmp = string.Format("grossini_dance_{0}.png", temp);
                    CCSpriteFrame frame = cache.SpriteFrameByName(tmp);
                    animFrames.Add(frame);
                }

                CCAnimation animation = new CCAnimation(animFrames, 0.3f);
                sprite.RunAction(new CCRepeatForever (new CCAnimate (animation)));

                animFrames = null;

                CCFlipY flip = new CCFlipY(true);
                CCFlipY flip_back = new CCFlipY(false);
                CCDelayTime delay = new CCDelayTime (1);
                CCFiniteTimeAction seq = new CCSequence((CCFiniteTimeAction)delay, (CCFiniteTimeAction)flip, (CCFiniteTimeAction)delay.Copy(null), flip_back);
                sprite.RunAction(new CCRepeatForever ((CCActionInterval)seq));

                AddChild(sprite, 0);
            }
        }

        public override string title()
        {
            return "Sprite offset + anchor + flip";
        }

        public override string subtitle()
        {
            return "issue #1078";
        }
    }
}
