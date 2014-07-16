using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
namespace tests
{
    public class Atlas6 : AtlasDemo
    {

		CCLabelBMFont label1, label2, label3;

        public Atlas6()
        {
            label1 = new CCLabelBMFont("FaFeFiFoFu", "fonts/bitmapFontTest5.fnt");
            AddChild(label1);
            label1.AnchorPoint = CCPoint.AnchorMiddle;

            label2 = new CCLabelBMFont("fafefifofu", "fonts/bitmapFontTest5.fnt");
            AddChild(label2);
            label2.AnchorPoint = CCPoint.AnchorMiddle;

            label3 = new CCLabelBMFont("aeiou", "fonts/bitmapFontTest5.fnt");
            AddChild(label3);
            label3.AnchorPoint = CCPoint.AnchorMiddle;
        }

        protected override void AddedToNewScene()
        {
            base.AddedToNewScene();

            var s = Layer.VisibleBoundsWorldspace.Size;

			label1.Position = new CCPoint(s.Width / 2, s.Height / 2 + 50);
			label2.Position = s.Center;
			label3.Position = new CCPoint(s.Width / 2, s.Height / 2 - 50);

		}

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Rendering should be OK. Testing offset";
        }
    }
}
