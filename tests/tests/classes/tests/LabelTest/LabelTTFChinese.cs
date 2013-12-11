using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelTTFChinese : AtlasDemo
    {
        public LabelTTFChinese()
        {
            CCSize size = CCDirector.SharedDirector.WinSize;
            CCLabelTTF pLable = new CCLabelTTF("中国", "Marker Felt", 30);
            pLable.Position = new CCPoint(size.Width / 2, size.Height / 2);
            AddChild(pLable);
        }

        public override string title()
        {
            return "Testing CCLabelTTF with Chinese character";
        }

        public override string subtitle()
        {
            return "You should see Chinese font";
        }
    }

    public class LabelBMFontChinese : AtlasDemo
    {
        public LabelBMFontChinese()
        {
            CCSize size = CCDirector.SharedDirector.WinSize;
            var pLable = new CCLabelBMFont("中国", "fonts/bitmapFontChinese.fnt");
            pLable.Position = new CCPoint(size.Width / 2, size.Height / 2);
            AddChild(pLable);
        }

        public override string title()
        {
            return "Testing CCLabelBMFont with Chinese character";
        }

    }
}
