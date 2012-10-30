using cocos2d;

namespace tests
{
    public class BMFontOneAtlas : AtlasDemo
    {
        public BMFontOneAtlas()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelBMFont label1 = CCLabelBMFont.Create("This is Helvetica", "fonts/helvetica-32.fnt", CCLabelBMFont.kCCLabelAutomaticWidth,
                                                        CCTextAlignment.CCTextAlignmentLeft, CCPoint.Zero);
            AddChild(label1);
            label1.Position = new CCPoint(s.width / 2, s.height / 3 * 2);

            CCLabelBMFont label2 = CCLabelBMFont.Create("And this is Geneva", "fonts/geneva-32.fnt", CCLabelBMFont.kCCLabelAutomaticWidth,
                                                        CCTextAlignment.CCTextAlignmentLeft, new CCPoint(0, 128));
            AddChild(label2);
            label2.Position = new CCPoint(s.width / 2, s.height / 3 * 1);
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