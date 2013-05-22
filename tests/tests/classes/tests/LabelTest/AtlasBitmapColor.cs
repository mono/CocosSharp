using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class AtlasBitmapColor : AtlasDemo
    {
        CCColor3B ccBLUE = new CCColor3B
        {
            R = 0,
            G = 0,
            B = 255
        };

        CCColor3B ccRED = new CCColor3B
        {
            R = 255,
            G = 0,
            B = 0
        };

        CCColor3B ccGREEN = new CCColor3B
        {
            R = 0,
            G = 255,
            B = 0
        };
        public AtlasBitmapColor()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelBMFont label = null;
            label = new CCLabelBMFont("Blue", "fonts/bitmapFontTest5.fnt");
            label.Color = ccBLUE;
            AddChild(label);
            label.Position = new CCPoint(s.Width / 2, s.Height / 4);
            label.AnchorPoint = new CCPoint(0.5f, 0.5f);

            label = new CCLabelBMFont("Red", "fonts/bitmapFontTest5.fnt");
            AddChild(label);
            label.Position = new CCPoint(s.Width / 2, 2 * s.Height / 4);
            label.AnchorPoint = new CCPoint(0.5f, 0.5f);
            label.Color = ccRED;

            label = new CCLabelBMFont("G", "fonts/bitmapFontTest5.fnt");
            AddChild(label);
            label.Position = new CCPoint(s.Width / 2, 3 * s.Height / 4);
            label.AnchorPoint = new CCPoint(0.5f, 0.5f);
            label.Color = ccGREEN;
            label.Label = ("Green");
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
