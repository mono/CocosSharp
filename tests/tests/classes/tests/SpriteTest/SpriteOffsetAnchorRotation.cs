using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteOffsetAnchorRotation : SpriteTestDemo
    {
        public SpriteOffsetAnchorRotation()
        {
            CCSize s = CCApplication.SharedApplication.MainWindowDirector.WinSize;
            CCSpriteFrameCache cache = CCSpriteFrameCache.Instance;
            cache.AddSpriteFrames("animations/grossini.plist");
            cache.AddSpriteFrames("animations/grossini_gray.plist", "animations/grossini_gray");

            for (int i = 0; i < 3; i++)
            {
                //
                // Animation using Sprite BatchNode
                //
                CCSprite sprite = new CCSprite("grossini_dance_01.png");
                sprite.Position = (new CCPoint(s.Width / 4 * (i + 1), s.Height / 2));

                CCSprite point = new CCSprite("Images/r1");
                point.Scale = 0.25f;
                point.Position = (sprite.Position);
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
                    str = string.Format("grossini_dance_{0:00}.png", j + 1);
                    CCSpriteFrame frame = cache[str];
                    animFrames.Add(frame);
                }

                CCAnimation animation = new CCAnimation(animFrames, 0.3f);
                sprite.RunAction(new CCRepeatForever (new CCAnimate (animation)));
                sprite.RunAction(new CCRepeatForever (new CCRotateBy (10, 360)));

                AddChild(sprite, 0);

                //animFrames.release();    // win32 : memory leak    2010-0415
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            CCSpriteFrameCache cache = CCSpriteFrameCache.Instance;
            cache.RemoveSpriteFrames("animations/grossini.plist");
            cache.RemoveSpriteFrames("animations/grossini_gray.plist");
        }

        public override string title()
        {
            return "Sprite offset + anchor + rot";
        }
    }
}
