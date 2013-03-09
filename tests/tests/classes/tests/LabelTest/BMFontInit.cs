using cocos2d;

namespace tests
{
    internal class BMFontInit : AtlasDemo
    {
        public BMFontInit()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            var bmFont = new CCLabelBMFont();
            bmFont.Init();
            //CCLabelBMFont* bmFont = [CCLabelBMFont create:@"Foo" fntFile:@"arial-unicode-26"];
            bmFont.FntFile = "fonts/helvetica-32.fnt";
            bmFont.SetString("It is working!");
            AddChild(bmFont);
            bmFont.Position = new CCPoint(s.Width / 2, s.Height / 4 * 2);
        }

        public override string title()
        {
            return "CCLabelBMFont init";
        }

        public override string subtitle()
        {
            return "Test for support of init method without parameters.";
        }
    }
}