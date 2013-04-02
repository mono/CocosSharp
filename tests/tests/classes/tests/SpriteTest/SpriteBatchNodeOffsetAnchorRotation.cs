using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteBatchNodeOffsetAnchorRotation : SpriteTestDemo
    {
        public SpriteBatchNodeOffsetAnchorRotation()
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
                CCSprite sprite = CCSprite.Create("grossini_dance_01.png");
                sprite.Position = (new CCPoint(s.Width / 4 * (i + 1), s.Height / 2));

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

                var animFrames = new List<CCSpriteFrame>(14);
                string str = "";
                for (int k = 0; k < 14; k++)
                {
                    string temp = "";
                    if (k+1<10)
                    {
                        temp ="0"+(k+1);
                    }
                    else
                    {
                        temp = k + 1 + "";
                    }
                    str = string.Format("grossini_dance_{0}.png", temp);
                    CCSpriteFrame frame = cache.SpriteFrameByName(str);
                    animFrames.Add(frame);
                }

                CCAnimation animation = CCAnimation.Create(animFrames, 0.3f);
                sprite.RunAction(CCRepeatForever.Create(new CCAnimate (animation)));
                sprite.RunAction(CCRepeatForever.Create(new CCRotateBy (10, 360)));

                spritebatch.AddChild(sprite, i);

                //animFrames.release();    // win32 : memory leak    2010-0415
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
            return "SpriteBatchNode offset + anchor + rot";
        }
    }
}
