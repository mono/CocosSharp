using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelFNTRetina : AtlasDemoNew
    {
		CCLabel label1;

        public LabelFNTRetina()
        {
            // CCLabel Bitmap Font
            label1 = new CCLabel("TESTING RETINA DISPLAY", "fonts/konqa32.fnt");

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
                return "loading arista16 or arista16-hd";
            }
        }
    }
}
