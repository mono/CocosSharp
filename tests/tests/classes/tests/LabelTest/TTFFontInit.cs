using CocosSharp;

namespace tests
{
    internal class TTFFontInit : AtlasDemo
    {
        public TTFFontInit()
        {
			var s = CCDirector.SharedDirector.WinSize;

            var font = new CCLabelTtf();
            font.FontName = "MarkerFelt";
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
			return "Test for support of CCLabelTTF() constructor without parameters.";
        }
    }
}