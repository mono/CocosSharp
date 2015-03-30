using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelFNTOldNew : AtlasDemoNew
    {
		CCLabel label1;
        CCLabelBMFont label2;
        CCDrawNode drawNode;

        public LabelFNTOldNew()
        {
            // CCLabel Bitmap Font
            label1 = new CCLabel("Bitmap Font Label Test", "fonts/arial-unicode-26.fnt");
            label1.Scale = 2;
            label1.Color = CCColor3B.White;

            AddChild(label1);

            label2 = new CCLabelBMFont("Bitmap Font Label Test", "fonts/arial-unicode-26.fnt");
            label2.Scale = 2;
            label2.Color = CCColor3B.Red;

            AddChild(label2);

            drawNode = new CCDrawNode();
            AddChild(drawNode);

            var touchListener = new CCEventListenerTouchAllAtOnce();
            touchListener.OnTouchesEnded = (touches, touchEvent) =>
            {
                var location = touches[0].Location;

                if (label1.BoundingBoxTransformedToWorld.ContainsPoint(location))
                    Console.WriteLine("Hit");
            };
            AddEventListener(touchListener);

        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            var origin = Layer.VisibleBoundsWorldspace.Size;

            float delta = origin.Height/4;

            label1.Position = origin.Center;
            label1.PositionY = delta * 2;

            label2.Position = label1.Position;

            // We use the ScaledContentSize here instead of ContentSize
            var labelSize = label1.ScaledContentSize;

            origin.Width = origin.Width   / 2 - (labelSize.Width / 2);
            origin.Height = origin.Height / 2 - (labelSize.Height / 2);

            CCPoint[] vertices1 =
                {
                    new CCPoint(origin.Width, origin.Height),
                    new CCPoint(labelSize.Width + origin.Width, origin.Height),
                    new CCPoint(labelSize.Width + origin.Width, labelSize.Height + origin.Height),
                    new CCPoint(origin.Width, labelSize.Height + origin.Height)
                };

            drawNode.DrawPolygon(vertices1, 4, CCColor4B.Transparent, 1, CCColor4B.White);

            // We use the ScaledContentSize here instead of ContentSize
            labelSize = label2.ScaledContentSize;
            origin = Layer.VisibleBoundsWorldspace.Size;

            origin.Width = origin.Width   / 2 - (labelSize.Width / 2);
            origin.Height = origin.Height / 2 - (labelSize.Height / 2);

            CCPoint[] vertices2 =
                {
                    new CCPoint(origin.Width, origin.Height),
                    new CCPoint(labelSize.Width + origin.Width, origin.Height),
                    new CCPoint(labelSize.Width + origin.Width, labelSize.Height + origin.Height),
                    new CCPoint(origin.Width, labelSize.Height + origin.Height)
                };

            drawNode.DrawPolygon(vertices2, 4, CCColor4B.Transparent, 1, CCColor4B.Red);
		}

        public override string Title
        {
            get {
                return "New / Old Bitmap Font";
            }
        }

        public override string Subtitle
        {
            get {
                return "Comparison between old(red) and\nnew(white) bitmap font label";
            }
        }
    }
}
