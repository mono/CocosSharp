using CocosSharp;

namespace tests
{
    internal class Issue1343 : AtlasDemo
    {
		CCLabelBMFont bmFont;

        public Issue1343()
        {
            bmFont = new CCLabelBMFont();
            bmFont.FntFile = "fonts/font-issue1343.fnt";
            bmFont.Text = ("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz.,'");
            AddChild(bmFont);
            bmFont.Scale = 0.3f;

        }

        protected override void AddedToNewScene()
        {
            base.AddedToNewScene();

            CCSize s = Scene.VisibleBoundsWorldspace.Size;

			bmFont.Position = new CCPoint(s.Width / 2, s.Height / 4 * 2);

		}
        public override string title()
        {
            return "Issue 1343";
        }

        public override string subtitle()
        {
            return "You should see: ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz.,'";
        }
    }
}