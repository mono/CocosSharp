using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelBMFontHD : AtlasDemo
    {
		CCLabelBMFont label1;

        public LabelBMFontHD()
        {
            // CCLabelBMFont
			label1 = new CCLabelBMFont("TESTING RETINA DISPLAY", "fonts/konqa32.fnt");
            AddChild(label1);
        }

		protected override void RunningOnNewWindow(CCSize windowSize)
		{
			base.RunningOnNewWindow(windowSize);

			label1.Position = windowSize.Center;
		}
        public override string title()
        {
            return "Testing Retina Display BMFont";
        }

        public override string subtitle()
        {
            return "loading arista16 or arista16-hd";
        }
    }
}
