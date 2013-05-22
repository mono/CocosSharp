using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class LabelTTFMultiline : AtlasDemo
    {
        public LabelTTFMultiline()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            // CCLabelBMFont
            CCLabelTTF center = new CCLabelTTF("word wrap \"testing\" (bla0) bla1 'bla2' [bla3] (bla4) {bla5} {bla6} [bla7] (bla8) [bla9] 'bla0' \"bla1\"",
                "Paint Boy", 32, 
                new CCSize(s.Width / 2, 200), 
                CCTextAlignment.CCTextAlignmentCenter);
            center.Position = new CCPoint(s.Width / 2, 150);

            AddChild(center);
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
