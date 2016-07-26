using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelFNTandSFEmpty : AtlasDemoNew
    {

        CCLabel label1;
        CCLabel label2;
        CCLabel label3;

        public LabelFNTandSFEmpty()
        {
            // CCLabel Bitmap Font
            label1 = new CCLabel("", "fonts/bitmapFontTest3.fnt");
            AddChild(label1, 0, (int)TagSprite.kTagBitmapAtlas1);

            // CCLabel Sprite Font
            label2 = new CCLabel("", "arial", 24, CCLabelFormat.SpriteFont);
            AddChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);

            // CCLabelAtlas
            //          label3 = new CCLabelAtlas("", "fonts/tuffy_bold_italic-charmap", 48, 64, ' ');
            //            AddChild(label3, 0, (int)TagSprite.kTagBitmapAtlas3);

            base.Schedule(updateStrings, 1.0f);

            setEmpty = false;
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var s = Layer.VisibleBoundsWorldspace.Size;
            var rowSplit = s.Height / 4;

            label1.Position = new CCPoint(s.Width / 2, s.Height - rowSplit);
            label1.Dimensions = new CCSize(s.Width, 0);
            label1.LabelFormat.Alignment = CCTextAlignment.Center;
            label2.Position = s.Center;
            //          label3.Position = new CCPoint(s.Width / 2, rowSplit);
        }


        public void updateStrings(float dt)
        {
            var label1 = (CCLabel) this [(int)TagSprite.kTagBitmapAtlas1];
            var label2 = (CCLabel) this [(int)TagSprite.kTagBitmapAtlas2];
            //var label3 = (CCLabelAtlas)this[(int)TagSprite.kTagBitmapAtlas3];

            if (!setEmpty)
            {
                label1.Text = ("not empty");
                label2.Text = ("not empty");
                //label3.Text = ("hi");

                setEmpty = true;
            }
            else
            {
                label1.Text = string.Empty;
                label2.Text = string.Empty;
                //label3.Text = string.Empty;

                setEmpty = false;
            }
        }

        public override string Title
        {
            get {
                return "New Label : .FNT file & SpriteFont file";
            }
        }

        public override string Subtitle
        {
            get {
                return "3 empty labels: new Label + FNT/SpriteFont/CharMap";
            }
        }

        private bool setEmpty;
    }
}
