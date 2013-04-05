using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteOffsetAnchorSkew : SpriteTestDemo
    {
        public SpriteOffsetAnchorSkew()
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
                        sprite.AnchorPoint = new CCPoint(0.5f, 0.5f);
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
                    tmp = string.Format("grossini_dance_{0:00}.png", j + 1);
                    CCSpriteFrame frame = cache.SpriteFrameByName(tmp);
                    animFrames.Add(frame);
                }

                CCAnimation animation = CCAnimation.Create(animFrames, 0.3f);
                sprite.RunAction(new CCRepeatForever (new CCAnimate (animation)));

                animFrames = null;

                CCSkewBy skewX = new CCSkewBy (2, 45, 0);
                CCActionInterval skewX_back = (CCActionInterval)skewX.Reverse();
                CCSkewBy skewY = new CCSkewBy (2, 0, 45);
                CCActionInterval skewY_back = (CCActionInterval)skewY.Reverse();

                CCFiniteTimeAction seq_skew = CCSequence.FromActions(skewX, skewX_back, skewY, skewY_back);
                sprite.RunAction(new CCRepeatForever ((CCActionInterval)seq_skew));

                AddChild(sprite, 0);
            }
        }

        public override string title()
        {
            return "Sprite offset + anchor + scale";
        }
    }
}
