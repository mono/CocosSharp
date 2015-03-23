using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelFNTColor : AtlasDemoNew
    {
		CCLabel label1, label2, label3;

        public LabelFNTColor()
        {
			label1 = new CCLabel("Blue", "fonts/bitmapFontTest5.fnt");
            label1.Color = CCColor3B.Blue;
            AddChild(label1);

            label2 = new CCLabel("Red", "fonts/bitmapFontTest5.fnt");
            label2.Color = CCColor3B.Red;
            AddChild(label2);

            label3 = new CCLabel("Green", "fonts/bitmapFontTest5.fnt");
            label3.Color = CCColor3B.Green;
            AddChild(label3);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            var s = Layer.VisibleBoundsWorldspace.Size;

			label1.Position = s.Center;		
            label1.PositionY = s.Height / 4;

            label2.Position = s.Center;     
            label2.PositionY = 2 * s.Height / 4;

            label3.Position = s.Center;     
            label3.PositionY = 3 * s.Height / 4;
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
                return "Testing color";
            }
        }
    }
}
