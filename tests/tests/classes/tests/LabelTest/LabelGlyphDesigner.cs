using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelGlyphDesigner : AtlasDemo
    {
        public LabelGlyphDesigner()
        {
			var s = CCDirector.SharedDirector.WinSize;

			var layer = new CCLayerColor(new CCColor4B(128, 128, 128, 255));
            AddChild(layer, -10);

            // CCLabelBMFont
			var label1 = new CCLabelBMFont("Testing Glyph Designer", "fonts/futura-48.fnt");
            AddChild(label1);
			label1.Position = s.Center;
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
