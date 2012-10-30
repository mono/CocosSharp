using cocos2d;

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
            font.SetString("It is working!");
            AddChild(font);
            font.Position = new CCPoint(s.width / 2, s.height / 4 * 2);
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