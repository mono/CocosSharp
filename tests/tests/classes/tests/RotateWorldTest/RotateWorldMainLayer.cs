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
            ContentSize = size;
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

            var offset = new CCPoint(x/4, y/4);

            var blue = new Background(size, new CCColor4B(0, 0, 255, 255));
            var red = new Background(size, new CCColor4B(255, 0, 0, 255));
            var green = new Background(size, new CCColor4B(0, 255, 0, 255));
            var white = new Background(size, new CCColor4B(255, 255, 255, 255));

            blue.Scale = 0.5f;
            blue.Position = CCPoint.Zero + offset;
            blue.AddChild(new SpriteLayer());

            red.Scale = 0.5f;
            red.Position = new CCPoint(x / 2,0) + offset;

            green.Scale = 0.5f;
            green.Position = new CCPoint(0, y / 2) + offset;
            green.AddChild(new TestLayer());

            white.Scale = 0.5f;
            white.Position = new CCPoint(x / 2, y / 2) + offset;

            AddChild(blue);
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
