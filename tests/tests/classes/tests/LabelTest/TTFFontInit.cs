using Cocos2D;

namespace tests
{
    internal class TTFFontInit : AtlasDemo
    {
        public TTFFontInit()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            var font = new CCLabelTTF();
            font.Init();
            font.FontName = "Marker Felt";
            font.FontSize = 38;
            font.Text = ("It is working!");
            AddChild(font);
            font.Position = new CCPoint(s.Width / 2, s.Height / 4 * 2);
        }

        public override string title()
        {
            return "CCLabelTTF init";
        }

        public override string subtitle()
        {
            return "Test for support of init method without parameters.";
        }
    }
}