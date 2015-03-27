using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelTTFUnicodeJapanese : AtlasDemoNew
    {
		CCLabel label;

        public LabelTTFUnicodeJapanese()
        {
            label = new CCLabel("良い一日を", "Hiragino Kaku Gothic Pro", 28, CCLabelFormat.SystemFont);
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
                return "New Label + System Font Japanese";
            }
        }

        public override string Subtitle
        {
            get {
                return "Testing new Label + SystemFont with Japanese character";
            }
        }
    }
}
