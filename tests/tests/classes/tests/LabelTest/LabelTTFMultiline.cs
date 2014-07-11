using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelTTFMultiline : AtlasDemo
    {
		CCLabelTtf center;

        public LabelTTFMultiline()
        {

            // CCLabelBMFont
			center = new CCLabelTtf("word wrap \"testing\" (bla0) bla1 'bla2' [bla3] (bla4) {bla5} {bla6} [bla7] (bla8) [bla9] 'bla0' \"bla1\"",
                "Paint Boy", 32, 
                CCSize.Zero, 
                CCTextAlignment.Center);
            

            AddChild(center);
        }

		protected override void RunningOnNewWindow(CCSize windowSize)
		{
			base.RunningOnNewWindow(windowSize);

			center.Position = new CCPoint(windowSize.Width / 2, 150);
			center.Dimensions = new CCSize(windowSize.Width / 2, 200);
		}
        public override string title()
        {
            return "Testing CCLabelTTF Word Wrap";
        }

        public override string subtitle()
        {
            return "Word wrap using CCLabelTTF";
        }
    }
}
