using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Atlas4 : AtlasDemo
    {
        //ccTime m_time;
        float m_time;

        public Atlas4()
        {
            m_time = 0;

            // Upper Label
            CCLabelBMFont label = CCLabelBMFont.Create("Bitmap Font Atlas", "fonts/bitmapFontTest.fnt");
            AddChild(label);

            CCSize s = CCDirector.SharedDirector.WinSize;

            label.Position = new CCPoint(s.Width / 2, s.Height / 2);
            label.AnchorPoint = new CCPoint(0.5f, 0.5f);


            CCSprite BChar = (CCSprite)label.GetChildByTag(0);
            CCSprite FChar = (CCSprite)label.GetChildByTag(7);
            CCSprite AChar = (CCSprite)label.GetChildByTag(12);


            CCActionInterval rotate = new CCRotateBy (2, 360);
            CCAction rot_4ever = new CCRepeatForever (rotate);

            CCActionInterval scale = new CCScaleBy(2, 1.5f);
            CCFiniteTimeAction scale_back = scale.Reverse();
            CCFiniteTimeAction scale_seq = CCSequence.FromActions(scale, scale_back);
            CCAction scale_4ever = new CCRepeatForever ((CCActionInterval)scale_seq);

            CCActionInterval jump = new CCJumpBy (0.5f, new CCPoint(), 60, 1);
            CCAction jump_4ever = new CCRepeatForever (jump);

            CCActionInterval fade_out = new CCFadeOut  (1);
            CCActionInterval fade_in = new CCFadeIn  (1);
            CCFiniteTimeAction seq = CCSequence.FromActions(fade_out, fade_in);
            CCAction fade_4ever = new CCRepeatForever ((CCActionInterval)seq);

            BChar.RunAction(rot_4ever);
            BChar.RunAction(scale_4ever);
            FChar.RunAction(jump_4ever);
            AChar.RunAction(fade_4ever);


            // Bottom Label
            CCLabelBMFont label2 = CCLabelBMFont.Create("00.0", "fonts/bitmapFontTest.fnt");
            AddChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);
            label2.Position = new CCPoint(s.Width / 2.0f, 80);

            CCSprite lastChar = (CCSprite)label2.GetChildByTag(3);
            lastChar.RunAction((CCAction)(rot_4ever.Copy()));

            //schedule( schedule_selector(Atlas4::step), 0.1f);
            base.Schedule(step, 0.1f);
        }

        public virtual void step(float dt)
        {
            m_time += dt;
            //char string[10] = {0};
            string Stepstring;
            //sprintf(string, "%04.1f", m_time);
            Stepstring = string.Format("{0,4:f1}", m_time);
            // 	std::string string;
            // 	string.format("%04.1f", m_time);

            CCLabelBMFont label1 = (CCLabelBMFont)GetChildByTag((int)TagSprite.kTagBitmapAtlas2);
            label1.Label = (Stepstring);
        }

        public override void Draw()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCDrawingPrimitives.Begin();
            CCDrawingPrimitives.DrawLine(new CCPoint(0, s.Height / 2), new CCPoint(s.Width, s.Height / 2), new CCColor4B(255, 0, 0, 255));
            CCDrawingPrimitives.DrawLine(new CCPoint(s.Width / 2, 0), new CCPoint(s.Width / 2, s.Height), new CCColor4B(255, 0, 0, 255));
            CCDrawingPrimitives.End();
        }

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Using fonts as CCSprite objects. Some characters should rotate.";
        }
    }
}
