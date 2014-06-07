using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
namespace tests
{
    public class Atlas6 : AtlasDemo
    {
        public Atlas6()
        {
            CCSize s = CCApplication.SharedApplication.MainWindowDirector.WindowSizeInPoints;

            CCLabelBMFont label = null;
            label = new CCLabelBMFont("FaFeFiFoFu", "fonts/bitmapFontTest5.fnt");
            AddChild(label);
            label.Position = new CCPoint(s.Width / 2, s.Height / 2 + 50);
            label.AnchorPoint = CCPoint.AnchorMiddle;

            label = new CCLabelBMFont("fafefifofu", "fonts/bitmapFontTest5.fnt");
            AddChild(label);
            label.Position = new CCPoint(s.Width / 2, s.Height / 2);
            label.AnchorPoint = CCPoint.AnchorMiddle;

            label = new CCLabelBMFont("aeiou", "fonts/bitmapFontTest5.fnt");
            AddChild(label);
            label.Position = new CCPoint(s.Width / 2, s.Height / 2 - 50);
            label.AnchorPoint = CCPoint.AnchorMiddle;
        }

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Rendering should be OK. Testing offset";
        }
    }
}
