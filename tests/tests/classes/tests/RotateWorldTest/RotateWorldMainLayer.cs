using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class RotateWorldMainLayer : CCLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            float x, y;

            CCSize size = CCApplication.SharedApplication.MainWindowDirector.WindowSizeInPoints;
            x = size.Width;
            y = size.Height;

            CCNode blue = new CCLayerColor(new CCColor4B(0, 0, 255, 255));
            CCNode red = new CCLayerColor(new CCColor4B(255, 0, 0, 255));
            CCNode green = new CCLayerColor(new CCColor4B(0, 255, 0, 255));
            CCNode white = new CCLayerColor(new CCColor4B(255, 255, 255, 255));

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

            CCAction rot = new CCRotateBy (8, 720);

            blue.RunAction(rot);
            red.RunAction(rot);
            green.RunAction(rot);
            white.RunAction(rot);
        }

        public static new RotateWorldMainLayer node()
        {
            RotateWorldMainLayer node = new RotateWorldMainLayer();
            return node;
        }
    }
}
