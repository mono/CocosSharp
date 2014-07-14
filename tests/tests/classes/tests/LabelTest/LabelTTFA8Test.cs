using CocosSharp;

namespace tests
{
    internal class LabelTTFA8Test : AtlasDemo
    {
		CCLabelTtf label1;

        public LabelTTFA8Test()
        {
			var layer = new CCLayerColor(new CCColor4B(128, 128, 128, 255));
            AddChild(layer, -10);

            // CCLabelBMFont
			label1 = new CCLabelTtf("Testing A8 Format", "MarkerFelt", 38);
            AddChild(label1);
            label1.Color = CCColor3B.Red;


			var fadeOut = new CCFadeOut  (2);
			var fadeIn = new CCFadeIn  (2);
			label1.RepeatForever(fadeIn, fadeOut);
        }

        protected override void AddedToNewScene()
        {
            base.AddedToNewScene();

            var s = Scene.VisibleBoundsWorldspace.Size;

			label1.Position = s.Center;
		}

        public override string title()
        {
            return "Testing A8 Format";
        }

        public override string subtitle()
        {
            return "RED label, fading In and Out in the center of the screen";
        }
    }
}