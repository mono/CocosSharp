using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class RotateWorldMainLayer : CCLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            float x, y;

            CCSize size = CCDirector.SharedDirector.WinSize;
            x = size.width;
            y = size.height;

            CCNode blue = CCLayerColor.Create(new ccColor4B(0, 0, 255, 255));
            CCNode red = CCLayerColor.Create(new ccColor4B(255, 0, 0, 255));
            CCNode green = CCLayerColor.Create(new ccColor4B(0, 255, 0, 255));
            CCNode white = CCLayerColor.Create(new ccColor4B(255, 255, 255, 255));

            blue.Scale = (0.5f);
            blue.Position = (new CCPoint(-x / 4, -y / 4));
            blue.AddChild(SpriteLayer.node());

            red.Scale = (0.5f);
            red.Position = (new CCPoint(x / 4, -y / 4));

            green.Scale = (0.5f);
            green.Position = (new CCPoint(-x / 4, y / 4));
            green.AddChild(TestLayer.node());

            white.Scale = (0.5f);
            white.Position = (new CCPoint(x / 4, y / 4));

            AddChild(blue, -1);
            AddChild(white);
            AddChild(green);
            AddChild(red);

            CCAction rot = CCRotateBy.Create(8, 720);

            blue.RunAction(rot);
            red.RunAction((CCAction)(rot.Copy()));
            green.RunAction((CCAction)(rot.Copy()));
            white.RunAction((CCAction)(rot.Copy()));
        }

        public static new RotateWorldMainLayer node()
        {
            RotateWorldMainLayer node = new RotateWorldMainLayer();
            return node;
        }
    }
}
