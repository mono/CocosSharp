using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelSFContentSizeRatios : AtlasDemoNew
    {
		CCLabel label1;
        CCSize defaultTexelToContentSizeRatios;

        public LabelSFContentSizeRatios()
        {
            // CCLabel Sprite Font
            label1 = new CCLabel("Testing Content\nSize", "MarkerFelt", 22);
            label1.LabelFormat.Alignment = CCTextAlignment.Center;

            AddChild(label1);

        }

        public override void OnEnter()
        {
            base.OnEnter();

            defaultTexelToContentSizeRatios = CCLabel.DefaultTexelToContentSizeRatios;

            CCLabel.DefaultTexelToContentSizeRatio = defaultTexelToContentSizeRatios.Width * 0.6f;;

            var s = Layer.VisibleBoundsWorldspace.Size;
            label1.Position = s.Center;
        }

        public override void OnExit()
        {
            base.OnExit();

            CCLabel.DefaultTexelToContentSizeRatios = defaultTexelToContentSizeRatios;
        }

        public override string Title
        {
            get {
                return "New Label + SpriteFont file";
            }
        }

        public override string Subtitle
        {
            get {
                return "Testing Content Scale Ratios";
            }
        }
    }
}
