using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelAtlasColorTest : AtlasDemo
    {
        //ccTime m_time;
        float m_time;
        CCColor3B ccRED = new CCColor3B
        {
            R = 255,
            G = 0,
            B = 0
        };

        public LabelAtlasColorTest()
        {
            CCLabelAtlas label1 = new CCLabelAtlas("123 Test", "fonts/tuffy_bold_italic-charmap", 48, 64, ' ');
            AddChild(label1, 0, (int)TagSprite.kTagSprite1);
            label1.Position = new CCPoint(10, 100);
            label1.Opacity = 200;

            CCLabelAtlas label2 = new CCLabelAtlas("0123456789", "fonts/tuffy_bold_italic-charmap", 48, 64, ' ');
            AddChild(label2, 0, (int)TagSprite.kTagSprite2);
            label2.Position = new CCPoint(10, 200);
            label2.Color = ccRED;

            CCActionInterval fade = new CCFadeOut  (1.0f);
            CCFiniteTimeAction fade_in = fade.Reverse();
            CCFiniteTimeAction seq = new CCSequence(fade, fade_in);
            CCAction repeat = new CCRepeatForever ((CCActionInterval)seq);
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
            label1.Text = (stepstring);

            CCLabelAtlas label2 = (CCLabelAtlas)GetChildByTag((int)TagSprite.kTagSprite2);
            //sprintf(string, "%d", (int)m_time);
            stepstring = string.Format("{0:D1}", (int)m_time);
            label2.Text = (stepstring);
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
