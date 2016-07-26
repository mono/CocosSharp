using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelFNTUNICODELanguages : AtlasDemoNew
    {
        CCLabel label1, label2, label3, label4;

        public LabelFNTUNICODELanguages()
        {
            var data = CCFileUtils.GetFileData("fonts/strings.plist");
            PlistDocument doc = new PlistDocument(data);
            var strings = doc.Root as PlistDictionary;

            var chinese = strings["chinese1"].AsString;
            var japanese = strings["japanese"].AsString;
            var spanish = strings["spanish"].AsString;
            var russian = strings["russian"].AsString;

            label1 = new CCLabel(spanish, "fonts/arial-unicode-26.fnt", new CCSize(200,0), CCLabelFormat.BitMapFont);
            label1.LabelFormat.Alignment = CCTextAlignment.Center;
            AddChild(label1);

            label2 = new CCLabel(chinese, "fonts/arial-unicode-26.fnt");
            AddChild(label2);

            label3 = new CCLabel(russian, "fonts/arial-26-en-ru.fnt");
            AddChild(label3);

            label4 = new CCLabel(japanese, "fonts/arial-unicode-26.fnt");
            AddChild(label4);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            var s = Layer.VisibleBoundsWorldspace.Size;

            label1.Position = new CCPoint(s.Width / 2, s.Height / 5 * 4);
            label2.Position = new CCPoint(s.Width / 2, s.Height / 5 * 3);
            label3.Position = new CCPoint(s.Width / 2, s.Height / 5 * 2);
            label4.Position = new CCPoint(s.Width / 2, s.Height / 5);

        }

        public override string Title
        {
            get {
                return "New Label + .FNT + UNICODE";
            }
        }

        public override string Subtitle
        {
            get {
                return "You should see 4 differnt labels:\nIn Spanish, Chinese, Russian and Korean";
            }
        }
    }
}
