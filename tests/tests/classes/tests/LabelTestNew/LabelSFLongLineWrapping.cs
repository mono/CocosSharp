using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
namespace tests
{
    public class LabelSFLongLineWrapping : AtlasDemoNew
    {
        CCLabel label1;

        public LabelSFLongLineWrapping()
        {
            // Long sentence
            label1 = new CCLabel(LabelFNTMultiLineAlignment.LongSentencesExample, "fonts/arial", 14, CCLabelFormat.TrueTypeFont);
            label1.LabelFormat.Alignment = CCTextAlignment.Center;
            label1.AnchorPoint = CCPoint.AnchorMiddleTop;
            AddChild(label1);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var s = Layer.VisibleBoundsWorldspace.Size;
            label1.Dimensions = new CCSize(s.Width, 0);
            label1.Position = s.Center;

		}

        public override string title()
        {
            return "New Label + SpriteFont";
        }

        public override string subtitle()
        {
            return "Uses the new Label with SpriteFont. Testing auto-wrapping";
        }
    }
}
