using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LabelGlyphDesigner : AtlasDemo
    {
        public LabelGlyphDesigner()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLayerColor layer = CCLayerColor.Create(new ccColor4B(128, 128, 128, 255));
            AddChild(layer, -10);

            // CCLabelBMFont
            CCLabelBMFont label1 = CCLabelBMFont.Create("Testing Glyph Designer", "fonts/futura-48.fnt");
            AddChild(label1);
            label1.Position = new CCPoint(s.width / 2, s.height / 2);
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
