using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelBMFontHD : AtlasDemo
    {
        public LabelBMFontHD()
        {
			var s = Scene.VisibleBoundsWorldspace.Size;

            // CCLabelBMFont
			var label1 = new CCLabelBMFont("TESTING RETINA DISPLAY", "fonts/konqa32.fnt");
            AddChild(label1);
			label1.Position = s.Center;
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
