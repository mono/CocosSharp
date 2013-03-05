using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using tests;
using System.Diagnostics;

namespace cocos2d
{
    public enum TagSprite
    {
        kTagTileMap = 1,
        kTagSpriteManager = 1,
        kTagAnimation1 = 1,
        kTagBitmapAtlas1 = 1,
        kTagBitmapAtlas2 = 2,
        kTagBitmapAtlas3 = 3,

        kTagSprite1,
        kTagSprite2,
        kTagSprite3,
        kTagSprite4,
        kTagSprite5,
        kTagSprite6,
        kTagSprite7,
        kTagSprite8
    }

    public class AtlasDemo : CCLayer
    {
        //protected:

        public AtlasDemo()
        {

        }

        public enum LabelTestConstant
        {
            IDC_NEXT = 100,
            IDC_BACK,
            IDC_RESTART
        }

        public virtual string title()
        {
            return "No title";
        }

        public virtual string subtitle()
        {
            return "";
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelTTF label = CCLabelTTF.Create(title(), "arial", 28);
            AddChild(label, 1);
            label.Position = new CCPoint(s.Width / 2, s.Height - 50);

            string strSubtitle = subtitle();
            if (strSubtitle != null)
            {
                CCLabelTTF l = CCLabelTTF.Create(strSubtitle, "arial", 16);
                AddChild(l, 1);
                l.Position = new CCPoint(s.Width / 2, s.Height - 80);
            }

            CCMenuItemImage item1 = CCMenuItemImage.Create(TestResource.s_pPathB1, TestResource.s_pPathB2, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.Create(TestResource.s_pPathR1, TestResource.s_pPathR2, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.Create(TestResource.s_pPathF1, TestResource.s_pPathF2, nextCallback);

            CCMenu menu = new CCMenu(item1, item2, item3);

            menu.Position = new CCPoint();
            item1.Position = new CCPoint(s.Width / 2 - 100, 30);
            item2.Position = new CCPoint(s.Width / 2, 30);
            item3.Position = new CCPoint(s.Width / 2 + 100, 30);

            AddChild(menu, 1);

        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new AtlasTestScene();
            s.AddChild(AtlasTestScene.restartAtlasAction());

            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(CCObject pSender)
        {

            CCScene s = new AtlasTestScene();

            s.AddChild(AtlasTestScene.nextAtlasAction());

            CCDirector.SharedDirector.ReplaceScene(s);

        }

        public void backCallback(CCObject pSender)
        {

            CCScene s = new AtlasTestScene();

            s.AddChild(AtlasTestScene.backAtlasAction());

            CCDirector.SharedDirector.ReplaceScene(s);

        }

    }

    public class Atlas1 : AtlasDemo
    {
        CCTextureAtlas m_textureAtlas;

        public Atlas1()
        {
            m_textureAtlas = CCTextureAtlas.Create(TestResource.s_AtlasTest, 3);
            //m_textureAtlas.retain();

            CCSize s = CCDirector.SharedDirector.WinSize;

            //
            // Notice: u,v tex coordinates are inverted
            //
            //ccV3F_C4B_T2F_Quad quads[] = 
            //{
            //    {
            //        {{0,0,0},new ccColor4B(0,0,255,255),{0.0f,1.0f},},				// bottom left
            //        {{s.width,0,0},new ccColor4B(0,0,255,0),{1.0f,1.0f},},			// bottom right
            //        {{0,s.height,0},new ccColor4B(0,0,255,0),{0.0f,0.0f},},	    // top left
            //        {{s.width,s.height,0},{0,0,255,255},{1.0f,0.0f},},	                // top right
            //    },		
            //    {
            //        {{40,40,0}, new ccColor4B(255,255,255,255),{0.0f,0.2f},},			// bottom left
            //        {{120,80,0},new ccColor4B(255,0,0,255),{0.5f,0.2f},},			        // bottom right
            //        {{40,160,0},new ccColor4B(255,255,255,255),{0.0f,0.0f},},		    // top left
            //        {{160,160,0},new ccColor4B(0,255,0,255),{0.5f,0.0f},},			    // top right
            //    },

            //    {
            //        {{s.width/2,40,0},new ccColor4B(255,0,0,255),{0.0f,1.0f},},		         // bottom left
            //        {{s.width,40,0},new ccColor4B(0,255,0,255),{1.0f,1.0f},},		        // bottom right
            //        {{s.width/2-50,200,0},new ccColor4B(0,0,255,255),{0.0f,0.0f},},		// top left
            //        {{s.width,100,0},new ccColor4B(255,255,0,255),{1.0f,0.0f},},		    // top right
            //    },		
            //};

            //for( int i=0;i<3;i++) 
            //{
            //    m_textureAtlas.updateQuad(&quads[i], i);
            //}
        }

        public override string title()
        {
            return "CCTextureAtlas";
        }

        public override string subtitle()
        {
            return "Manual creation of CCTextureAtlas";
        }

        public override void Draw()
        {
            // GL_VERTEX_ARRAY, GL_COLOR_ARRAY, GL_TEXTURE_COORD_ARRAY
            // GL_TEXTURE_2D

            m_textureAtlas.DrawQuads();

            //	[textureAtlas drawNumberOfQuads:3];
        }

    }

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
            CCLabelAtlas label1 = CCLabelAtlas.Create("123 Test", "fonts/tuffy_bold_italic-charmap.png", 48, 64, ' ');
            AddChild(label1, 0, (int)TagSprite.kTagSprite1);
            label1.Position = new CCPoint(10, 100);
            label1.Opacity = 200;

            CCLabelAtlas label2 = CCLabelAtlas.Create("0123456789", "fonts/tuffy_bold_italic-charmap.png", 48, 64, ' ');
            AddChild(label2, 0, (int)TagSprite.kTagSprite2);
            label2.Position = new CCPoint(10, 200);
            //label2.setColor( ccRED );
            label2.Color = ccRED;

            CCActionInterval fade = CCFadeOut.Create(1.0f);
            //CCActionInterval fade_in = fade.reverse();
            CCActionInterval fade_in = null;
            CCFiniteTimeAction seq = CCSequence.Create(fade, fade_in, null);
            CCAction repeat = CCRepeatForever.Create((CCActionInterval)seq);
            label2.RunAction(repeat);

            m_time = 0;

            //schedule( schedule_selector(LabelAtlasColorTest.step) ); //:@selector(step:)];
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
            stepstring = m_time.ToString();
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


            CCSprite BChar = (CCSprite)label.GetChildByTag(1);
            CCSprite FChar = (CCSprite)label.GetChildByTag(7);
            CCSprite AChar = (CCSprite)label.GetChildByTag(12);


            CCActionInterval rotate = CCRotateBy.Create(2, 360);
            CCAction rot_4ever = CCRepeatForever.Create(rotate);

            CCActionInterval scale = CCScaleBy.Create(2, 1.5f);
            //CCActionInterval scale_back = scale.reverse();
            CCActionInterval scale_back = null;
            CCFiniteTimeAction scale_seq = CCSequence.Create(scale, scale_back, null);
            CCAction scale_4ever = CCRepeatForever.Create((CCActionInterval)scale_seq);

            CCActionInterval jump = CCJumpBy.Create(0.5f, new CCPoint(), 60, 1);
            CCAction jump_4ever = CCRepeatForever.Create(jump);

            CCActionInterval fade_out = CCFadeOut.Create(1);
            CCActionInterval fade_in = CCFadeIn.Create(1);
            CCFiniteTimeAction seq = CCSequence.Create(fade_out, fade_in, null);
            CCAction fade_4ever = CCRepeatForever.Create((CCActionInterval)seq);

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
            Stepstring = string.Format("{0,4:1f}", m_time);
            // 	std::string string;
            // 	string.format("%04.1f", m_time);

            CCLabelBMFont label1 = (CCLabelBMFont)GetChildByTag((int)TagSprite.kTagBitmapAtlas2);
            label1.SetString(Stepstring);
        }

        public override void Draw()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;
            //ccDrawLine(new CCPoint(0, s.height / 2), new CCPoint(s.width, s.height / 2));
            //ccDrawLine(new CCPoint(s.width / 2, 0), new CCPoint(s.width / 2, s.height));
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

    public class Atlas5 : AtlasDemo
    {

        public Atlas5()
        {
            CCLabelBMFont label = CCLabelBMFont.Create("abcdefg", "fonts/bitmapFontTest4.fnt");
            AddChild(label);

            CCSize s = CCDirector.SharedDirector.WinSize;

            label.Position = new CCPoint(s.Width / 2, s.Height / 2);
            label.AnchorPoint = new CCPoint(0.5f, 0.5f);
        }
        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Testing padding";
        }

    }

    public class Atlas6 : AtlasDemo
    {

        public Atlas6()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelBMFont label = null;
            label = CCLabelBMFont.Create("FaFeFiFoFu", "fonts/bitmapFontTest5.fnt");
            AddChild(label);
            label.Position = new CCPoint(s.Width / 2, s.Height / 2 + 50);
            label.AnchorPoint = new CCPoint(0.5f, 0.5f);

            label = CCLabelBMFont.Create("fafefifofu", "fonts/bitmapFontTest5.fnt");
            AddChild(label);
            label.Position = new CCPoint(s.Width / 2, s.Height / 2);
            label.AnchorPoint = new CCPoint(0.5f, 0.5f);

            label = CCLabelBMFont.Create("aeiou", "fonts/bitmapFontTest5.fnt");
            AddChild(label);
            label.Position = new CCPoint(s.Width / 2, s.Height / 2 - 50);
            label.AnchorPoint = new CCPoint(0.5f, 0.5f);
        }

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Rendering should be OK. Testing offset";
        }

    }

    public class AtlasBitmapColor : AtlasDemo
    {
        ccColor3B ccBLUE = new ccColor3B
      {
          r = 0,
          g = 0,
          b = 255
      };

        ccColor3B ccRED = new ccColor3B
       {
           r = 255,
           g = 0,
           b = 0
       };

        ccColor3B ccGREEN = new ccColor3B
       {
           r = 0,
           g = 255,
           b = 0
       };
        public AtlasBitmapColor()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelBMFont label = null;
            label = CCLabelBMFont.Create("Blue", "fonts/bitmapFontTest5.fnt");
            label.Color = ccBLUE;
            AddChild(label);
            label.Position = new CCPoint(s.Width / 2, s.Height / 4);
            label.AnchorPoint = new CCPoint(0.5f, 0.5f);

            label = CCLabelBMFont.Create("Red", "fonts/bitmapFontTest5.fnt");
            AddChild(label);
            label.Position = new CCPoint(s.Width / 2, 2 * s.Height / 4);
            label.AnchorPoint = new CCPoint(0.5f, 0.5f);
            label.Color = ccRED;

            label = CCLabelBMFont.Create("G", "fonts/bitmapFontTest5.fnt");
            AddChild(label);
            label.Position = new CCPoint(s.Width / 2, 3 * s.Height / 4);
            label.AnchorPoint = new CCPoint(0.5f, 0.5f);
            label.Color = ccGREEN;
            label.SetString("Green");
        }

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Testing color";
        }

    }

    public class AtlasFastBitmap : AtlasDemo
    {

        public AtlasFastBitmap()
        {
            // Upper Label
            for (int i = 0; i < 100; i++)
            {
                //char str[6] = {0};
                string str;
                //sprintf(str, "-%d-", i);
                str = string.Format("-{0,d}-", i);
                CCLabelBMFont label = CCLabelBMFont.Create(str, "fonts/bitmapFontTest.fnt");
                AddChild(label);

                CCSize s = CCDirector.SharedDirector.WinSize;

                CCPoint p = new CCPoint(ccMacros.CCRANDOM_0_1() * s.Width, ccMacros.CCRANDOM_0_1() * s.Height);
                label.Position = p;
                label.AnchorPoint = new CCPoint(0.5f, 0.5f);
            }
        }

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Creating several CCLabelBMFont with the same .fnt file should be fast";
        }

    }

    public class BitmapFontMultiLine : AtlasDemo
    {
        public BitmapFontMultiLine()
        {
            CCSize s;

            // Left
            CCLabelBMFont label1 = CCLabelBMFont.Create("Multi line\nLeft", "fonts/bitmapFontTest3.fnt");
            label1.AnchorPoint = new CCPoint(0, 0);
            AddChild(label1, 0, (int)TagSprite.kTagBitmapAtlas1);

            s = label1.ContentSize;

            //CCLOG("content size: %.2fx%.2f", s.width, s.height);
            CCLog.Log("content size: {0,0:2f}x{1,0:2f}", s.Width, s.Height);


            // Center
            CCLabelBMFont label2 = CCLabelBMFont.Create("Multi line\nCenter", "fonts/bitmapFontTest3.fnt");
            label2.AnchorPoint = new CCPoint(0.5f, 0.5f);
            AddChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);

            s = label2.ContentSize;
            //CCLOG("content size: %.2fx%.2f", s.width, s.height);
            CCLog.Log("content size: {0,0:2f}x{1,0:2f}", s.Width, s.Height);

            // right
            CCLabelBMFont label3 = CCLabelBMFont.Create("Multi line\nRight\nThree lines Three", "fonts/bitmapFontTest3.fnt");
            label3.AnchorPoint = new CCPoint(1, 1);
            AddChild(label3, 0, (int)TagSprite.kTagBitmapAtlas3);

            s = label3.ContentSize;
            //CCLOG("content size: %.2fx%.2f", s.width, s.height);

            s = CCDirector.SharedDirector.WinSize;
            label1.Position = new CCPoint();
            label2.Position = new CCPoint(s.Width / 2, s.Height / 2);
            label3.Position = new CCPoint(s.Width, s.Height);
        }

        public override string title()
        {
            return "CCLabelBMFont";
        }

        public override string subtitle()
        {
            return "Multiline + anchor point";
        }
    }

    public class LabelsEmpty : AtlasDemo
    {

        public LabelsEmpty()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            // CCLabelBMFont
            CCLabelBMFont label1 = CCLabelBMFont.Create("", "fonts/bitmapFontTest3.fnt");
            AddChild(label1, 0, (int)TagSprite.kTagBitmapAtlas1);
            label1.Position = new CCPoint(s.Width / 2, s.Height - 100);

            // CCLabelTTF
            CCLabelTTF label2 = CCLabelTTF.Create("", "arial", 24);
            AddChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);
            label2.Position = new CCPoint(s.Width / 2, s.Height / 2);

            // CCLabelAtlas
            CCLabelAtlas label3 = CCLabelAtlas.Create("", "fonts/tuffy_bold_italic-charmap.png", 48, 64, ' ');
            AddChild(label3, 0, (int)TagSprite.kTagBitmapAtlas3);
            label3.Position = new CCPoint(s.Width / 2, 0 + 100);

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

    public class LabelBMFontHD : AtlasDemo
    {

        public LabelBMFontHD()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            // CCLabelBMFont
            CCLabelBMFont label1 = CCLabelBMFont.Create("TESTING RETINA DISPLAY", "fonts/konqa32.fnt");
            AddChild(label1);
            label1.Position = new CCPoint(s.Width / 2, s.Height / 2);
        }

        public override string title()
        {
            return "Testing Retina Display BMFont";
        }

        public override string subtitle()
        {
            return "loading arista16 or arista16-hd";
        }

    }

    public class LabelAtlasHD : AtlasDemo
    {
        public LabelAtlasHD()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            // CCLabelBMFont
            CCLabelAtlas label1 = CCLabelAtlas.Create("TESTING RETINA DISPLAY", "fonts/larabie-16.png", 10, 20, 'A');
            label1.AnchorPoint = new CCPoint(0.5f, 0.5f);

            AddChild(label1);
            label1.Position = new CCPoint(s.Width / 2, s.Height / 2);
        }

        public override string title()
        {
            return "LabelAtlas with Retina Display";
        }

        public override string subtitle()
        {
            return "loading larabie-16 / larabie-16-hd";
        }
    }

    public class LabelGlyphDesigner : AtlasDemo
    {

        public LabelGlyphDesigner()
        {
            //CCSize s = CCDirector.sharedDirector().getWinSize();

            //CCLayerColor layer = CCLayerColor.layerWithColor(new ccColor4B(128, 128, 128, 255));
            //addChild(layer, -10);

            //// CCLabelBMFont
            //CCLabelBMFont label1 = CCLabelBMFont.labelWithString("Testing Glyph Designer", "fonts/futura-48.fnt");
            //addChild(label1);
            //label1.position = new CCPoint(s.width / 2, s.height / 2);
        }

        public override string title()
        {
            return "Testing Glyph Designer";
        }

        public override string subtitle()
        {
            return "You should see a font with shawdows and outline";
        }

        //  void AtlasTestScene::runThisTest()
        //{
        //    CCLayer pLayer = nextAtlasAction();
        //    addChild(pLayer);

        //    CCDirector.sharedDirector().replaceScene(this);
        //}

    }



    //    public class LabelTTFTest : AtlasDemo
    //    {
    //        public LabelTTFTest()
    //        {
    //            CCSize s = CCDirector.sharedDirector().getWinSize();

    //            // CCLabelBMFont
    //            CCLabelTTF left = CCLabelTTF.labelWithString("align left", new CCSize(s.width, 50), CCTextAlignment.CCTextAlignmentLeft, "arial", 32);
    //            CCLabelTTF center = CCLabelTTF.labelWithString("align center", new CCSize(s.width, 50), CCTextAlignment.CCTextAlignmentCenter, "arial", 32);
    //            CCLabelTTF right = CCLabelTTF.labelWithString("align right", new CCSize(s.width, 50), CCTextAlignment.CCTextAlignmentRight, "arial", 32);

    //            left.position = new CCPoint(s.width / 2, 200);
    //            center.position = new CCPoint(s.width / 2, 150);
    //            right.position = new CCPoint(s.width / 2, 100);

    //            addChild(left);
    //            addChild(center);
    //            addChild(right);
    //        }

    //        public override string title()
    //        {
    //            return "Testing CCLabelTTF";
    //        }
    //        public override string subtitle()
    //        {
    //            return "You should see 3 labels aligned left, center and right";
    //        }

    //    }

    //    public class LabelTTFMultiline : AtlasDemo
    //    {

    //        public LabelTTFMultiline()
    //        {
    //            CCSize s = CCDirector.sharedDirector().getWinSize();

    //            // CCLabelBMFont
    //            CCLabelTTF center = CCLabelTTF.labelWithString("word wrap \"testing\" (bla0) bla1 'bla2' [bla3] (bla4) {bla5} {bla6} [bla7] (bla8) [bla9] 'bla0' \"bla1\"",
    //                new CCSize(s.width / 2, 200), CCTextAlignment.CCTextAlignmentCenter, "MarkerFelt.ttc", 32);
    //            center.position = new CCPoint(s.width / 2, 150);

    //            addChild(center);
    //        }

    //        public override string title()
    //        {
    //            return "Testing CCLabelTTF Word Wrap";
    //        }

    //        public override string subtitle()
    //        {
    //            return "Word wrap using CCLabelTTF";
    //        }

    //    }

    //    public class LabelTTFChinese : AtlasDemo
    //    {

    //        public LabelTTFChinese()
    //        {
    //            CCSize size = CCDirector.sharedDirector().getWinSize();
    //            CCLabelTTF pLable = CCLabelTTF.labelWithString("Chinaese", "arial", 30);
    //            pLable.position = new CCPoint(size.width / 2, size.height / 2);
    //            addChild(pLable);
    //        }

    //        public override string title()
    //        {
    //            return "Testing CCLabelTTF with Chinese character";
    //        }

    //        public override string subtitle()
    //        {
    //            return "You should see Chinese font";
    //        }

    //    }
}
