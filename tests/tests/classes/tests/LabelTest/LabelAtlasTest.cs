using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class LabelAtlasTest : AtlasDemo
    {
        //ccTime m_time;
        float m_time;

        public LabelAtlasTest()
        {
            m_time = 0;

            CCLabelAtlas label1 = new CCLabelAtlas("123 Test", "fonts/tuffy_bold_italic-charmap", 48, 64, ' ');
            AddChild(label1, 0, (int)TagSprite.kTagSprite1);
            label1.Position = new CCPoint(10, 100);
            label1.Opacity = 200;

            CCLabelAtlas label2 = new CCLabelAtlas("0123456789", "fonts/tuffy_bold_italic-charmap", 48, 64, ' ');
            AddChild(label2, 0, (int)TagSprite.kTagSprite2);
            label2.Position = new CCPoint(10, 200);
            label2.Opacity = 32;

            Schedule(step);
        }

        public virtual void step(float dt)
        {
            m_time += dt;
            //char string[12] = {0};
            string Stepstring;

            //sprintf(Stepstring, "%2.2f Test", m_time);
            Stepstring = string.Format("{0,2:f2} Test", m_time);
            //Stepstring.format("%2.2f Test", m_time);

            CCLabelAtlas label1 = (CCLabelAtlas)GetChildByTag((int)TagSprite.kTagSprite1);
            label1.Label = (Stepstring);

            CCLabelAtlas label2 = (CCLabelAtlas)GetChildByTag((int)TagSprite.kTagSprite2);
            //sprintf(Stepstring, "%d", (int)m_time);
            Stepstring = m_time.ToString();
            Stepstring = string.Format("{0:d}", (int)m_time);
            label2.Label = (Stepstring);
        }

        public override string title()
        {
            return "LabelAtlas";
        }

        public override string subtitle()
        {
            return "Updating label should be fast";
        }

    }
}
