using CocosSharp;

namespace tests
{
    public class BMFontOneAtlas : AtlasDemo
    {
        public BMFontOneAtlas()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelBMFont label1 = new CCLabelBMFont("This is Helvetica", "fonts/helvetica-32.fnt", CCLabelBMFont.kCCLabelAutomaticWidth,
                                                        CCTextAlignment.Left, CCPoint.Zero);
            AddChild(label1);
            label1.Position = new CCPoint(s.Width / 2, s.Height / 3 * 2);

            CCLabelBMFont label2 = new CCLabelBMFont("And this is Geneva", "fonts/geneva-32.fnt", CCLabelBMFont.kCCLabelAutomaticWidth,
                                                        CCTextAlignment.Left, new CCPoint(0, 128));
            AddChild(label2);
            label2.Position = new CCPoint(s.Width / 2, s.Height / 3 * 1);
        }

        public override string title()
        {
            return "CCLabelBMFont with one texture";
        }

        public override string subtitle()
        {
            return "Using 2 .fnt definitions that share the same texture atlas.";
        }
    }
}