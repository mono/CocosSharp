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
            CCLabelBMFont label = new CCLabelBMFont("abcdefg", "fonts/bitmapFontTest4.fnt");
            AddChild(label);

            CCSize s = CCDirector.SharedDirector.WinSize;

            label.Position = new CCPoint(s.Width / 2, s.Height / 2);
            label.AnchorPoint = new CCPoint(0.5f, 0.5f);
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
