using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelTTFUnicodeChinese : AtlasDemoNew
    {
		CCLabel label;

        public LabelTTFUnicodeChinese()
        {
            label = new CCLabel("美好的一天啊", "fonts/HKYuanMini.ttf", 28, CCLabelFormat.SystemFont);
            label.LabelFormat.Alignment = CCTextAlignment.Center;
            AddChild(label);

			label.AnchorPoint = CCPoint.AnchorMiddle;
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            var s = Layer.VisibleBoundsWorldspace.Size;

			label.Position = s.Center;		
		}

        public override string Title
		{
            get {
                return "New Label + .TTF file Chinese";
            }
        }

        public override string Subtitle
        {
            get {
                return "Testing new Label + TTF with Chinese character";
            }
        }
    }
}
