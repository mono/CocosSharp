using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelFNTGlyphDesigner : AtlasDemoNew
    {

		CCLabel label1;

        public LabelFNTGlyphDesigner()
        {
            Color = new CCColor3B(128, 128, 128);
            Opacity = 255;

            // CCLabelBMFont
			label1 = new CCLabel ("Testing Glyph Designer", "fonts/futura-48.fnt");
            AddChild(label1);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var s = Layer.VisibleBoundsWorldspace.Size;

            label1.Position = s.Center;

		}

        public override string Title
        {
            get {
                return "New Label + .FNT file";
            }
        }

        public override string Subtitle
        {
            get {
                return "Testing Glyph Designer: you should see a font with shawdows and outline";
            }
        }
    }
}
