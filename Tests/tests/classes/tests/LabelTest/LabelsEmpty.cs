using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelsEmpty : AtlasDemo
    {

		CCLabelBMFont label1;
		CCLabelTtf label2;
		CCLabelAtlas label3;

        public LabelsEmpty()
        {
            // CCLabelBMFont
			label1 = new CCLabelBMFont("", "fonts/bitmapFontTest3.fnt");
            AddChild(label1, 0, (int)TagSprite.kTagBitmapAtlas1);
            
            // CCLabelTTF
			label2 = new CCLabelTtf("", "arial", 24);
            AddChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);

            // CCLabelAtlas
			label3 = new CCLabelAtlas("", "fonts/tuffy_bold_italic-charmap", 48, 64, ' ');
            AddChild(label3, 0, (int)TagSprite.kTagBitmapAtlas3);

            base.Schedule(updateStrings, 1.0f);

            setEmpty = false;
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var s = Layer.VisibleBoundsWorldspace.Size;
            var rowSplit = s.Height / 4;

			label1.Position = new CCPoint(s.Width / 2, s.Height - rowSplit);
			label2.Position = s.Center;
			label3.Position = new CCPoint(s.Width / 2, rowSplit);
		}


        public void updateStrings(float dt)
        {
			var label1 = (CCLabelBMFont)this[(int)TagSprite.kTagBitmapAtlas1];
			var label2 = (CCLabelTtf)this[(int)TagSprite.kTagBitmapAtlas2];
			var label3 = (CCLabelAtlas)this[(int)TagSprite.kTagBitmapAtlas3];

            if (!setEmpty)
            {
                label1.Text = ("not empty");
                label2.Text = ("not empty");
                label3.Text = ("hi");

                setEmpty = true;
            }
            else
            {
				label1.Text = string.Empty;
				label2.Text = string.Empty;
				label3.Text = string.Empty;

                setEmpty = false;
            }
        }

        public override string title()
        {
            return "Testing empty labels";
        }

        public override string subtitle()
        {
            return "3 empty labels: LabelAtlas, LabelTTF and LabelBMFont";
        }

        private bool setEmpty;
    }
}
