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
            CCSize s = CCDirector.SharedDirector.WinSize;

            // CCLabelBMFont
            CCLabelAtlas label1 = new CCLabelAtlas("TESTING RETINA DISPLAY", "fonts/larabie-16", 10, 20, 'A');
            label1.AnchorPoint = new CCPoint(0.5f, 0.5f);

            AddChild(label1);
            label1.Position = new CCPoint(s.Width / 2, s.Height / 2);
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
