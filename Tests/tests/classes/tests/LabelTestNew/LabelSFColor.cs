using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelSFColor : AtlasDemoNew
    {
		CCLabel label1, label2, label3;

        public LabelSFColor()
        {
            var labelFormat = CCLabelFormat.SpriteFont;
            labelFormat.Alignment = CCTextAlignment.Center;

			label1 = new CCLabel("Green", "fonts/arial", 18, labelFormat);
            label1.Color = CCColor3B.Green;
            AddChild(label1);

            label2 = new CCLabel("Red", "fonts/arial", 18, labelFormat);
            label2.Color = CCColor3B.Red;
            AddChild(label2);

            label3 = new CCLabel("Blue", "fonts/arial", 18, labelFormat);
            label3.Color = CCColor3B.Blue;
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
                return "New Label + SpriteFont";
            }
        }

        public override string Subtitle
        {
            get {
                return "Uses the new Label with SpriteFont\nTesting Color";
            }
        }
    }
}
