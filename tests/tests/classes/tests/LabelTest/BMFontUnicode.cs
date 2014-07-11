using CocosSharp;

namespace tests
{
    internal class BMFontUnicode : AtlasDemo
    {

		CCLabelBMFont label1, label2, label3;

        public BMFontUnicode()
        {
            var data = CCFileUtils.GetFileData("fonts/strings.plist");
            PlistDocument doc = new PlistDocument(data);
            var strings = doc.Root as PlistDictionary;

            string chinese = strings["chinese1"].AsString;
            string japanese = strings["japanese"].AsString;
            string spanish = strings["spanish"].AsString;

            CCSize s = CCApplication.SharedApplication.MainWindowDirector.WindowSizeInPoints;

            label1 = new CCLabelBMFont(spanish, "fonts/arial-unicode-26.fnt", 200, CCTextAlignment.Left);
            AddChild(label1);

            label2 = new CCLabelBMFont(chinese, "fonts/arial-unicode-26.fnt");
            AddChild(label2);

            label3 = new CCLabelBMFont(japanese, "fonts/arial-unicode-26.fnt");
            AddChild(label3);
        }

		protected override void RunningOnNewWindow(CCSize windowSize)
		{
			base.RunningOnNewWindow(windowSize);

			var s = windowSize;

			label1.Position = new CCPoint(s.Width / 2, s.Height / 4 * 3);
			label2.Position = new CCPoint(s.Width / 2, s.Height / 4 * 2);
			label3.Position = new CCPoint(s.Width / 2, s.Height / 4 * 1);

		}

        public override string title()
        {
            return "CCLabelBMFont with Unicode support";
        }

        public override string subtitle()
        {
            return "You should see 3 differnt labels: In Spanish, Chinese and Korean";
        }
    }
}