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
				var label = new CCLabelBMFont(str, "fonts/bitmapFontTest.fnt");
                AddChild(label);

				var s = CCDirector.SharedDirector.WinSize;

				var p = new CCPoint(CCMacros.CCRandomBetween0And1() * s.Width, CCMacros.CCRandomBetween0And1() * s.Height);
                label.Position = p;
                label.AnchorPoint = CCPoint.AnchorMiddle;
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
