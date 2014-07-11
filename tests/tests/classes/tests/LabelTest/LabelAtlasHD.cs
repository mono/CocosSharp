using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelAtlasHD : AtlasDemo
    {
		CCLabelAtlas label1;

        public LabelAtlasHD()
        {
            // CCLabelBMFont
			label1 = new CCLabelAtlas("TESTING RETINA DISPLAY", "fonts/larabie-16", 10, 20, 'A');
			label1.AnchorPoint = CCPoint.AnchorMiddle;

            AddChild(label1);

        }

		protected override void RunningOnNewWindow(CCSize windowSize)
		{
			base.RunningOnNewWindow(windowSize);
			label1.Position = windowSize.Center;
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
