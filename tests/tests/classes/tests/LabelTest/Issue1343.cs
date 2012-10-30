using cocos2d;

namespace tests
{
    internal class Issue1343 : AtlasDemo
    {
        public Issue1343()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            var bmFont = new CCLabelBMFont();
            bmFont.Init();
            bmFont.FntFile = "fonts/font-issue1343.fnt";
            bmFont.SetString("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz.,'");
            AddChild(bmFont);
            bmFont.Scale = 0.3f;

            bmFont.Position = new CCPoint(s.width / 2, s.height / 4 * 2);
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