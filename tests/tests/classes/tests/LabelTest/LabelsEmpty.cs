using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LabelsEmpty : AtlasDemo
    {
        public LabelsEmpty()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            // CCLabelBMFont
            CCLabelBMFont label1 = CCLabelBMFont.Create("", "fonts/bitmapFontTest3.fnt");
            AddChild(label1, 0, (int)TagSprite.kTagBitmapAtlas1);
            label1.Position = new CCPoint(s.width / 2, s.height - 100);

            // CCLabelTTF
            CCLabelTTF label2 = CCLabelTTF.Create("", "arial", 24);
            AddChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);
            label2.Position = new CCPoint(s.width / 2, s.height / 2);

            // CCLabelAtlas
            CCLabelAtlas label3 = CCLabelAtlas.Create("", "fonts/tuffy_bold_italic-charmap", 48, 64, ' ');
            AddChild(label3, 0, (int)TagSprite.kTagBitmapAtlas3);
            label3.Position = new CCPoint(s.width / 2, 0 + 100);

            base.Schedule(updateStrings, 1.0f);

            setEmpty = false;
        }

        public void updateStrings(float dt)
        {
            CCLabelBMFont label1 = (CCLabelBMFont)GetChildByTag((int)TagSprite.kTagBitmapAtlas1);
            CCLabelTTF label2 = (CCLabelTTF)GetChildByTag((int)TagSprite.kTagBitmapAtlas2);
            CCLabelAtlas label3 = (CCLabelAtlas)GetChildByTag((int)TagSprite.kTagBitmapAtlas3);

            if (!setEmpty)
            {
                label1.SetString("not empty");
                label2.SetString("not empty");
                label3.SetString("hi");

                setEmpty = true;
            }
            else
            {
                label1.SetString("");
                label2.SetString("");
                label3.SetString("");

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
