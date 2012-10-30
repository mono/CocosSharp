using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LabelAtlasColorTest : AtlasDemo
    {
        //ccTime m_time;
        float m_time;
        ccColor3B ccRED = new ccColor3B
        {
            r = 255,
            g = 0,
            b = 0
        };

        public LabelAtlasColorTest()
        {
            CCLabelAtlas label1 = CCLabelAtlas.Create("123 Test", "fonts/tuffy_bold_italic-charmap", 48, 64, ' ');
            AddChild(label1, 0, (int)TagSprite.kTagSprite1);
            label1.Position = new CCPoint(10, 100);
            label1.Opacity = 200;

            CCLabelAtlas label2 = CCLabelAtlas.Create("0123456789", "fonts/tuffy_bold_italic-charmap", 48, 64, ' ');
            AddChild(label2, 0, (int)TagSprite.kTagSprite2);
            label2.Position = new CCPoint(10, 200);
            label2.Color = ccRED;

            CCActionInterval fade = CCFadeOut.Create(1.0f);
            CCFiniteTimeAction fade_in = fade.Reverse();
            CCFiniteTimeAction seq = CCSequence.Create(fade, fade_in);
            CCAction repeat = CCRepeatForever.Create((CCActionInterval)seq);
            label2.RunAction(repeat);

            m_time = 0;

            Schedule(step); //:@selector(step:)];
        }

        public virtual void step(float dt)
        {
            m_time += dt;
            //char string[12] = {0};
            string stepstring;

            //sprintf(string, "%2.2f Test", m_time);
            stepstring = string.Format("{0,2:f2} Test", m_time);
            //std::string string = std::string::stringWithFormat("%2.2f Test", m_time);
            CCLabelAtlas label1 = (CCLabelAtlas)GetChildByTag((int)TagSprite.kTagSprite1);
            label1.SetString(stepstring);

            CCLabelAtlas label2 = (CCLabelAtlas)GetChildByTag((int)TagSprite.kTagSprite2);
            //sprintf(string, "%d", (int)m_time);
            stepstring = string.Format("{0:D1}", (int)m_time);
            label2.SetString(stepstring);
        }

        public override string title()
        {
            return "CCLabelAtlas";
        }

        public override string subtitle()
        {
            return "Opacity + Color should work at the same time";
        }
    }
}
