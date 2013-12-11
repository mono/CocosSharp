using CocosSharp;

namespace tests
{
    internal class Issue1343 : AtlasDemo
    {
        public Issue1343()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            var bmFont = new CCLabelBMFont();
            bmFont.FntFile = "fonts/font-issue1343.fnt";
            bmFont.Text = ("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz.,'");
            AddChild(bmFont);
            bmFont.Scale = 0.3f;

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