using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelTTFChinese : AtlasDemo
    {

		CCLabelTtf pLable;

        public LabelTTFChinese()
        {
			pLable = new CCLabelTtf("中国", "Marker Felt", 30);
            AddChild(pLable);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var size = Layer.VisibleBoundsWorldspace.Size;


            pLable.Position = size.Center;

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
		CCLabelBMFont pLable;

        public LabelBMFontChinese()
        {
            pLable = new CCLabelBMFont("中国", "fonts/bitmapFontChinese.fnt");
            AddChild(pLable);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var size = Layer.VisibleBoundsWorldspace.Size;

            pLable.Position = size.Center;

		}

		public override string title()
        {
            return "Testing CCLabelBMFont with Chinese character";
        }

    }
}
