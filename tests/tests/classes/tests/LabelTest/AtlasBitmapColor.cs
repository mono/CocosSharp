using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class AtlasBitmapColor : AtlasDemo
    {

		CCLabelBMFont label1, label2, label3;

        public AtlasBitmapColor()
        {
			label1 = new CCLabelBMFont("Blue", "fonts/bitmapFontTest5.fnt");
			label1.Color = CCColor3B.Blue;
            AddChild(label1);
			label1.AnchorPoint = CCPoint.AnchorMiddle;

            label2 = new CCLabelBMFont("Red", "fonts/bitmapFontTest5.fnt");
            AddChild(label2);
			label2.AnchorPoint = CCPoint.AnchorMiddle;
			label2.Color = CCColor3B.Red;

            label3 = new CCLabelBMFont("G", "fonts/bitmapFontTest5.fnt");
            AddChild(label3);
			label3.AnchorPoint = CCPoint.AnchorMiddle;
			label3.Color = CCColor3B.Green;
            label3.Text = "Green";
        }

        protected override void AddedToNewScene()
        {
            base.AddedToNewScene();

            var s = Layer.VisibleBoundsWorldspace.Size;

			label1.Position = new CCPoint(s.Width / 2, s.Height / 4);
			label2.Position = new CCPoint(s.Width / 2, 2 * s.Height / 4);
			label3.Position = new CCPoint(s.Width / 2, 3 * s.Height / 4);

		}

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Testing color";
        }
    }
}
