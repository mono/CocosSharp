using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{

    public class Background : CCDrawNode
    {
        
        public Background(CCColor4B color)
        {
            Color = new CCColor3B(color);
            Opacity = color.A;
            AnchorPoint = CCPoint.AnchorMiddle;
        }

        public Background(CCSize size, CCColor4B color)
        {
            Color = new CCColor3B(color);
            Opacity = color.A;
            AnchorPoint = CCPoint.AnchorMiddle;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            DrawRect(VisibleBoundsWorldspace);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            if (ContentSize == CCSize.Zero)
                ContentSize = VisibleBoundsWorldspace.Size;
        }
    }

    public class RotateWorldMainLayer : CCLayer
    {

		CCAction rot = new CCRotateBy (8, 720);

		public RotateWorldMainLayer()
		{}

        public override void OnEnter()
        {
            base.OnEnter();

            float x, y;

            CCSize size = Layer.VisibleBoundsWorldspace.Size;
            x = size.Width;
            y = size.Height;

            var offset = (CCPoint)size / 4.0f;

            var blue = new Background(new CCColor4B(0, 0, 255, 255));
            var red = new Background(new CCColor4B(255, 0, 0, 255));
            var green = new Background(new CCColor4B(0, 255, 0, 255));
            var white = new Background(new CCColor4B(255, 255, 255, 255));

            blue.Scale = 0.5f;
            blue.Position = offset;
            blue.AddChild(new SpriteLayer());

            red.Scale = 0.5f;
            red.Position = offset;
            red.PositionX += x / 2.0f;

            green.Scale = 0.5f;
            green.Position = size.Center;
            green.PositionX -= offset.X;
            green.PositionY += offset.Y;
            green.AddChild(new TestLayer());

            white.Scale = 0.5f;
            white.Position = size.Center + offset;
            //white.PositionX += x / 2.0f;
            //white.PositionY += y / 2.0f;

            AddChild(blue, -1);
            AddChild(white);
            AddChild(green);
            AddChild(red);

            blue.RunAction(rot);
            red.RunAction(rot);
            green.RunAction(rot);
            white.RunAction(rot);
        }

    }
}
