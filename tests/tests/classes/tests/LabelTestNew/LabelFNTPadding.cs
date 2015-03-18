using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelFNTPadding : AtlasDemoNew
    {
		CCLabel label;

        public LabelFNTPadding()
        {
			label = new CCLabel("abcdefg", "fonts/bitmapFontTest4.fnt");
            AddChild(label);

			label.AnchorPoint = CCPoint.AnchorMiddle;
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            var s = Layer.VisibleBoundsWorldspace.Size;

			label.Position = s.Center;		
		}

        public override string title()
		{
            return "New Label + .FNT file";
        }

        public override string subtitle()
        {
            return "Testing padding";
        }
    }
}
