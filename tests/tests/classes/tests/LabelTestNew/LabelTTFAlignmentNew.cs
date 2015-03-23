using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelTTFAlignmentNew : AtlasDemoNew
    {
        CCLabel ttf0, ttf1, ttf2;

        public LabelTTFAlignmentNew()
        {
            ttf0 = new CCLabel("Alignment 0\nnew line", "fonts/tahoma.ttf", 16, CCLabelFormat.SystemFont);
            ttf0.LabelFormat.Alignment = CCTextAlignment.Left;
            AddChild(ttf0);

            ttf1 = new CCLabel("Alignment 1\nnew line", "fonts/tahoma.ttf", 16, CCLabelFormat.SystemFont);
            ttf1.LabelFormat.Alignment = CCTextAlignment.Center;
            AddChild(ttf1);

            ttf2 = new CCLabel("Alignment 2\nnew line", "fonts/tahoma.ttf", 16, CCLabelFormat.SystemFont);
            ttf2.LabelFormat.Alignment = CCTextAlignment.Right;
            AddChild(ttf2);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            var s = Layer.VisibleBoundsWorldspace.Size;

			ttf0.Position = s.Center;
            ttf0.PositionY = (s.Height / 6) * 2 - 30;

            ttf1.Position = s.Center;
            ttf1.PositionY = (s.Height / 6) * 3 - 30;

            ttf2.Position = s.Center;
            ttf2.PositionY = (s.Height / 6) * 4 - 30;


        }

        public override string Title
		{
            get {
                return "New Label + TTF";
            }
        }

        public override string Subtitle
        {
            get {
                return "Tests alignment values";
            }
        }
    }
}
