using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
namespace tests
{
    public class Atlas6 : AtlasDemo
    {
        public Atlas6()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelBMFont label = null;
            label = CCLabelBMFont.Create("FaFeFiFoFu", "fonts/bitmapFontTest5.fnt");
            AddChild(label);
            label.Position = new CCPoint(s.Width / 2, s.Height / 2 + 50);
            label.AnchorPoint = new CCPoint(0.5f, 0.5f);

            label = CCLabelBMFont.Create("fafefifofu", "fonts/bitmapFontTest5.fnt");
            AddChild(label);
            label.Position = new CCPoint(s.Width / 2, s.Height / 2);
            label.AnchorPoint = new CCPoint(0.5f, 0.5f);

            label = CCLabelBMFont.Create("aeiou", "fonts/bitmapFontTest5.fnt");
            AddChild(label);
            label.Position = new CCPoint(s.Width / 2, s.Height / 2 - 50);
            label.AnchorPoint = new CCPoint(0.5f, 0.5f);
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
