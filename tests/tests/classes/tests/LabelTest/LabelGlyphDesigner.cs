using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelGlyphDesigner : AtlasDemo
    {

		CCLabelBMFont label1;

        public LabelGlyphDesigner()
        {
			var layer = new CCLayerColor(new CCColor4B(128, 128, 128, 255));
            AddChild(layer, -10);

            // CCLabelBMFont
			label1 = new CCLabelBMFont("Testing Glyph Designer", "fonts/futura-48.fnt");
            AddChild(label1);
        }

		protected override void RunningOnNewWindow(CCSize windowSize)
		{
			base.RunningOnNewWindow(windowSize);

			label1.Position = windowSize.Center;

		}
        public override string title()
        {
            return "Testing Glyph Designer";
        }

        public override string subtitle()
        {
            return "You should see a font with shawdows and outline";
        }
    }
}
