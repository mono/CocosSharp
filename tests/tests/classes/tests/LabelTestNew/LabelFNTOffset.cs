using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
namespace tests
{
    public class LabelFNTOffset : AtlasDemoNew
    {

		CCLabel label1, label2, label3;

        public LabelFNTOffset()
        {
            label1 = new CCLabel("FaFeFiFoFu", "fonts/bitmapFontTest5.fnt");
            AddChild(label1);
            label1.AnchorPoint = CCPoint.AnchorMiddle;

            label2 = new CCLabel("fafefifofu", "fonts/bitmapFontTest5.fnt");
            AddChild(label2);
            label2.AnchorPoint = CCPoint.AnchorMiddle;

            label3 = new CCLabel("aeiou", "fonts/bitmapFontTest5.fnt");
            AddChild(label3);
            label3.AnchorPoint = CCPoint.AnchorMiddle;
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var s = Layer.VisibleBoundsWorldspace.Size;

			label1.Position = new CCPoint(s.Width / 2, s.Height / 2 + 50);
			label2.Position = s.Center;
			label3.Position = new CCPoint(s.Width / 2, s.Height / 2 - 50);

		}

        public override string title()
        {
            return "New Label + .FNT file";
        }

        public override string subtitle()
        {
            return "Rendering should be OK. Testing offset";
        }
    }
}
