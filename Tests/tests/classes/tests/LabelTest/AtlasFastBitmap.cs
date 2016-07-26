using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class AtlasFastBitmap : AtlasDemo
    {
        public AtlasFastBitmap()
        {
            // Upper Label
            for (int i = 0; i < 100; i++)
            {
                //char str[6] = {0};
                string str;
                //sprintf(str, "-%d-", i);
                str = string.Format("-{0}-", i);
				var label = new CCLabelBMFont(str, "fonts/bitmapFontTest.fnt") { Tag = i + 100};
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
        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Creating several CCLabelBMFont with the same .fnt file should be fast";
        }
    }
}
