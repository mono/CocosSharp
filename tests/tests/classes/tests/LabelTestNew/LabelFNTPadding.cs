using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Atlas5New : AtlasDemoNew
    {
		CCLabel label;

        public Atlas5New()
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
            return "CCLabel BMFont";
        }

        public override string subtitle()
        {
            return "Testing padding";
        }
    }
}
