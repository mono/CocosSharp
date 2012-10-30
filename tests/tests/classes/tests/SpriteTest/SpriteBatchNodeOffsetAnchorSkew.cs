using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeOffsetAnchorSkew : SpriteTestDemo
    {
        public SpriteBatchNodeOffsetAnchorSkew()
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
                CCSprite sprite = CCSprite.Create("grossini_dance_01.png");
                sprite.Position = (new CCPoint(s.width / 4 * (i + 1), s.height / 2));

                CCSprite point = CCSprite.Create("Images/r1");
                point.Scale = 0.25f;
                point.Position = sprite.Position;
                AddChild(point, 200);

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

                CCSpriteBatchNode spritebatch = CCSpriteBatchNode.Create("animations/grossini");
                AddChild(spritebatch);

               var animFrames = new List<CCObject>();
                string tmp = "";
                for (int j = 0; j < 14; j++)
                {
                    string temp = "";
                    if (j+1<10)
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

                CCAnimation animation = CCAnimation.Create(animFrames, 0.3f);
                sprite.RunAction(CCRepeatForever.Create(CCAnimate.Create(animation)));

                animFrames = null;

                CCSkewBy skewX = CCSkewBy.Create(2, 45, 0);
                CCActionInterval skewX_back = (CCActionInterval)skewX.Reverse();
                CCSkewBy skewY = CCSkewBy.Create(2, 0, 45);
                CCActionInterval skewY_back = (CCActionInterval)skewY.Reverse();

                CCFiniteTimeAction seq_skew = CCSequence.Create(skewX, skewX_back, skewY, skewY_back);
                sprite.RunAction(CCRepeatForever.Create((CCActionInterval)seq_skew));

                spritebatch.AddChild(sprite, i);
            }
        }
        public override string title()
        {
            return "SpriteBatchNode offset + anchor + skew";
        }
    }
}
