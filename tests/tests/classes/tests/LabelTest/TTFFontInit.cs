using CocosSharp;

namespace tests
{
    internal class TTFFontInit : AtlasDemo
    {

		CCLabelTtf font;

        public TTFFontInit()
        {
            font = new CCLabelTtf();
            font.FontName = "MarkerFelt";
            font.FontSize = 38;
            font.Text = ("It is working!");
            AddChild(font);
        }

        protected override void AddedToNewScene()
        {
            base.AddedToNewScene();

            var s = Layer.VisibleBoundsWorldspace.Size;

            font.Position = new CCPoint(s.Width / 2, s.Height / 4 * 2);
		}

        public override string title()
        {
            return "CCLabelTTF init";
        }

        public override string subtitle()
        {
			return "Test for support of CCLabelTTF() constructor without parameters.";
        }
    }
}