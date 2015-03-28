using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelSystemFontColor : AtlasDemoNew
    {
		CCLabel label1, label2, label3;

        public LabelSystemFontColor()
        {
            var labelFormat = CCLabelFormat.SystemFont;
            labelFormat.Alignment = CCTextAlignment.Center;

			label1 = new CCLabel("Green", "arial", 18, labelFormat);
            label1.Color = CCColor3B.Green;
            label1.Scale = 2;
            AddChild(label1);

            label2 = new CCLabel("Red", "arial", 18, labelFormat);
            label2.Color = CCColor3B.Red;
            label2.Scale = 3;
            AddChild(label2);

            label3 = new CCLabel("Blue", "arial", 18, labelFormat);
            label3.Color = CCColor3B.Blue;
            label1.Scale = 4;
            AddChild(label3);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            var s = Layer.VisibleBoundsWorldspace.Size;

			label1.Position = s.Center;		
            label1.PositionY = s.Height * 0.3f;

            label2.Position = s.Center;     
            label2.PositionY = s.Height * 0.4f;

            label3.Position = s.Center;     
            label3.PositionY = s.Height  * 0.5f;
		}

        public override string Title
		{
            get {
                return "New Label + SystemFont";
            }
        }

        public override string Subtitle
        {
            get {
                return "Uses the new Label with SystemFont\nTesting Scale and Color";
            }
        }
    }
}
