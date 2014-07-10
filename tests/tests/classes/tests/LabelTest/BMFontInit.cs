using CocosSharp;

namespace tests
{
    internal class BMFontInit : AtlasDemo
    {
        public BMFontInit()
        {
			var s = Scene.VisibleBoundsWorldspace.Size;

            var bmFont = new CCLabelBMFont();
            //CCLabelBMFont* bmFont = [CCLabelBMFont create:@"Foo" fntFile:@"arial-unicode-26"];
            bmFont.FntFile = "fonts/helvetica-32.fnt";
            bmFont.Text = ("It is working!");
            AddChild(bmFont);
            bmFont.Position = new CCPoint(s.Width / 2, s.Height / 4 * 2);
        }

        public override string title()
        {
            return "CCLabelBMFont init";
        }

        public override string subtitle()
        {
			return "Test for support of CCLabelBMFont constructor without parameters.";
        }
    }
}