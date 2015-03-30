using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelSFOldNew : AtlasDemoNew
    {
		CCLabel label1;
        CCLabelTtf label2;
        CCDrawNode drawNode;

        public LabelSFOldNew()
        {
            // CCLabel SpriteFont
            label1 = new CCLabel("SpriteFont Label Test", "arial", 48, CCLabelFormat.SpriteFont);

            AddChild(label1);

            label2 = new CCLabelTtf("SpriteFont Label Test", "arial", 48);
            label2.Color = CCColor3B.Red;
            label2.AnchorPoint = CCPoint.AnchorMiddle;

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
                return "New / Old Sprite Font";
            }
        }

        public override string Subtitle
        {
            get {
                return "Comparison between old(red) and\nnew(white) SpriteFont label";
            }
        }
    }
}
