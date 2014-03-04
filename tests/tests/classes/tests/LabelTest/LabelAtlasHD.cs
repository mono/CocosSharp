using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelAtlasHD : AtlasDemo
    {
        public LabelAtlasHD()
        {
			var s = CCDirector.SharedDirector.WinSize;

            // CCLabelBMFont
			var label1 = new CCLabelAtlas("TESTING RETINA DISPLAY", "fonts/larabie-16", 10, 20, 'A');
			label1.AnchorPoint = CCPoint.AnchorMiddle;

            AddChild(label1);
			label1.Position = s.Center;
        }

        public override string title()
        {
            return "LabelAtlas with Retina Display";
        }

        public override string subtitle()
        {
            return "loading larabie-16 / larabie-16-hd";
        }
    }
}
