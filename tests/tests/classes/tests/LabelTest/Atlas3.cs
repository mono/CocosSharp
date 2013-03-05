using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Atlas3 : AtlasDemo
    {
        //ccTime	m_time;
        float m_time;
        ccColor3B ccRED = new ccColor3B
        {
            r = 255,
            g = 0,
            b = 0
        };

        public Atlas3()
        {
            m_time = 0;

            CCLayerColor col = CCLayerColor.Create(new ccColor4B(128, 128, 128, 255));
            AddChild(col, -10);

            CCLabelBMFont label1 = CCLabelBMFont.Create("Test", "fonts/bitmapFontTest2.fnt");

            // testing anchors
            label1.AnchorPoint = new CCPoint(0, 0);
            AddChild(label1, 0, (int)TagSprite.kTagBitmapAtlas1);
            CCActionInterval fade = CCFadeOut.Create(1.0f);
            CCFiniteTimeAction fade_in = fade.Reverse();
            CCFiniteTimeAction seq = CCSequence.Create(fade, fade_in);
            CCAction repeat = CCRepeatForever.Create((CCActionInterval)seq);
            label1.RunAction(repeat);


            // VERY IMPORTANT
            // color and opacity work OK because bitmapFontAltas2 loads a BMP image (not a PNG image)
            // If you want to use both opacity and color, it is recommended to use NON premultiplied images like BMP images
            // Of course, you can also tell XCode not to compress PNG images, but I think it doesn't work as expected
            CCLabelBMFont label2 = CCLabelBMFont.Create("Test", "fonts/bitmapFontTest2.fnt");
            // testing anchors
            label2.AnchorPoint = new CCPoint(0.5f, 0.5f);
            label2.Color = ccRED;
            AddChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);
            label2.RunAction((CCAction)(repeat.Copy()));

            CCLabelBMFont label3 = CCLabelBMFont.Create("Test", "fonts/bitmapFontTest2.fnt");
            // testing anchors
            label3.AnchorPoint = new CCPoint(1, 1);
            AddChild(label3, 0, (int)TagSprite.kTagBitmapAtlas3);


            CCSize s = CCDirector.SharedDirector.WinSize;
            label1.Position = new CCPoint();
            label2.Position = new CCPoint(s.Width / 2, s.Height / 2);
            label3.Position = new CCPoint(s.Width, s.Height);

            base.Schedule(step);//:@selector(step:)];
        }

        public virtual void step(float dt)
        {
            m_time += dt;
            string stepString;
            stepString = string.Format("{0,2:f2} Test j", m_time);

            CCLabelBMFont label1 = (CCLabelBMFont)GetChildByTag((int)TagSprite.kTagBitmapAtlas1);
            label1.SetString(stepString);

            CCLabelBMFont label2 = (CCLabelBMFont)GetChildByTag((int)TagSprite.kTagBitmapAtlas2);
            label2.SetString(stepString);

            CCLabelBMFont label3 = (CCLabelBMFont)GetChildByTag((int)TagSprite.kTagBitmapAtlas3);
            label3.SetString(stepString);
        }

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Testing alignment. Testing opacity + tint";
        }

    }
}
