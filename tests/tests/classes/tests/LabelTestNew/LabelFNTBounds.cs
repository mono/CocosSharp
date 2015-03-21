using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelFNTBounds : AtlasDemoNew
    {
		CCLabel label;
        CCDrawNode drawNode;

        public LabelFNTBounds ()
        {

            Color = new CCColor3B(128, 128, 128);
            Opacity = 255;

            label = new CCLabel("Testing Glyph Designer", "fonts/boundsTestFont.fnt");
            label.LabelFormat.Alignment = CCTextAlignment.Center;
            AddChild(label);

            drawNode = new CCDrawNode();
            AddChild(drawNode);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            var origin = Layer.VisibleBoundsWorldspace.Size;
			
            label.Position = origin.Center;
            label.Dimensions = new CCSize(origin.Width, 0);

            var labelSize = label.ContentSize;

            origin.Width = origin.Width   / 2 - (labelSize.Width / 2);
            origin.Height = origin.Height / 2 - (labelSize.Height / 2);

            CCPoint[] vertices =
                {
                    new CCPoint(origin.Width, origin.Height),
                    new CCPoint(labelSize.Width + origin.Width, origin.Height),
                    new CCPoint(labelSize.Width + origin.Width, labelSize.Height + origin.Height),
                    new CCPoint(origin.Width, labelSize.Height + origin.Height)
                };

            drawNode.DrawPolygon(vertices, 4, CCColor4B.Transparent, 1, CCColor4B.White);
		}

        public override string title()
		{
            return "New Label + .FNT + Bounds";
        }

        public override string subtitle()
        {
            return "You should see string enclosed by a box";
        }
    }
}
