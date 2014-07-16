using CocosSharp;

namespace tests
{
    internal class BMFontInit : AtlasDemo
    {

		CCLabelBMFont bmFont;

        public BMFontInit()
        {
            bmFont = new CCLabelBMFont();
            //CCLabelBMFont* bmFont = [CCLabelBMFont create:@"Foo" fntFile:@"arial-unicode-26"];
            bmFont.FntFile = "fonts/helvetica-32.fnt";
            bmFont.Text = ("It is working!");
            AddChild(bmFont);
        }

        protected override void AddedToNewScene()
        {
            base.AddedToNewScene();

            var s = Layer.VisibleBoundsWorldspace.Size;
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