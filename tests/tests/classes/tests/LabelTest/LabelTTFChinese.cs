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
			var size = CCDirector.SharedDirector.WinSize;
			var pLable = new CCLabelTTF("中国", "Marker Felt", 30);
			pLable.Position = size.Center;
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
			var size = CCDirector.SharedDirector.WinSize;
            var pLable = new CCLabelBMFont("中国", "fonts/bitmapFontChinese.fnt");
			pLable.Position = size.Center;
            AddChild(pLable);
        }

        public override string title()
        {
            return "Testing CCLabelBMFont with Chinese character";
        }

    }
}
