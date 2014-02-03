using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteAnchorPoint : SpriteTestDemo
    {
        public SpriteAnchorPoint()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;


            CCRotateBy rotate = new CCRotateBy(10, 360);
            CCRepeatForever action = new CCRepeatForever(rotate);

            for (int i = 0; i < 3; i++)
            {
                CCSprite sprite = new CCSprite("Images/grossini_dance_atlas", new CCRect(85, 121 * 1, 85, 121));
                sprite.Position = (new CCPoint(s.Width / 4 * (i + 1), s.Height / 2));

                CCSprite point = new CCSprite("Images/r1");
                point.Scale = 0.25f;
                point.Position = (sprite.Position);
                AddChild(point, 10);

                switch (i)
                {
                    case 0:
                        sprite.AnchorPoint = (new CCPoint(0, 0));
                        break;
                    case 1:
                        sprite.AnchorPoint = (new CCPoint(0.5f, 0.5f));
                        break;
                    case 2:
                        sprite.AnchorPoint = (new CCPoint(1, 1));
                        break;
                }

                point.Position = sprite.Position;

                CCRepeatForever copy = new CCRepeatForever(action);
                sprite.RunAction(copy);
                AddChild(sprite, i);
            }
        }

        public override string title()
        {
            return "Sprite: anchor point";
        }

    }
}
