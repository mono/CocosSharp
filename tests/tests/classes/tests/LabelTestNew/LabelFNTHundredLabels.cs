using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelFNTHundredLabels : AtlasDemoNew
    {
        public LabelFNTHundredLabels()
        {
            // Upper Label
            for (int i = 0; i < 100; i++)
            {
                string str;
                str = string.Format("-{0}-", i);
				var label = new CCLabel(str, "fonts/bitmapFontTest.fnt") { Tag = i + 100};
                AddChild(label);

                label.AnchorPoint = CCPoint.AnchorMiddle;
            }
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var s = Layer.VisibleBoundsWorldspace.Size;

			for (int i = 0; i < 100; i++)
			{
				var p = new CCPoint(CCMacros.CCRandomBetween0And1() * s.Width, CCMacros.CCRandomBetween0And1() * s.Height);
				this[i+100].Position = p;
			}

		}
        public override string Title
        {
            get {
                return "New Label + .FNT file";
            }
        }

        public override string Subtitle
        {
            get {
                return "Creating several Labels using the same FNT file; should be fast";
            }
        }
    }
}
