using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Atlas5 : AtlasDemo
    {
        public Atlas5()
        {
			var label = new CCLabelBMFont("abcdefg", "fonts/bitmapFontTest4.fnt");
            AddChild(label);

			var s = CCApplication.SharedApplication.MainWindowDirector.WinSize;

			label.Position = s.Center;
			label.AnchorPoint = CCPoint.AnchorMiddle;
        }
        public override string title()
		{
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Testing padding";
        }
    }
}
