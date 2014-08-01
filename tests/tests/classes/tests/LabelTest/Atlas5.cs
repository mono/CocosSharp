using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Atlas5 : AtlasDemo
    {
		CCLabelBMFont label;

        public Atlas5()
        {
			label = new CCLabelBMFont("abcdefg", "fonts/bitmapFontTest4.fnt");
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
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Testing padding";
        }
    }
}
