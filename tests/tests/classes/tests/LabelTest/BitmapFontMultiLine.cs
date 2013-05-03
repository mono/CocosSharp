using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class BitmapFontMultiLine : AtlasDemo
    {
        public BitmapFontMultiLine()
        {
            CCSize s;

            // Left
            CCLabelBMFont label1 = new CCLabelBMFont("Multi line\nLeft", "fonts/bitmapFontTest3.fnt");
            label1.AnchorPoint = new CCPoint(0, 0);
            AddChild(label1, 0, (int)TagSprite.kTagBitmapAtlas1);

            s = label1.ContentSize;

            //CCLOG("content size: %.2fx%.2f", s.width, s.height);
            CCLog.Log("content size: {0,0:2f}x{1,0:2f}", s.Width, s.Height);


            // Center
            CCLabelBMFont label2 = new CCLabelBMFont("Multi line\nCenter", "fonts/bitmapFontTest3.fnt");
            label2.AnchorPoint = new CCPoint(0.5f, 0.5f);
            AddChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);

            s = label2.ContentSize;
            //CCLOG("content size: %.2fx%.2f", s.width, s.height);
            CCLog.Log("content size: {0,0:2f}x{1,0:2f}", s.Width, s.Height);

            // right
            CCLabelBMFont label3 = new CCLabelBMFont("Multi line\nRight\nThree lines Three", "fonts/bitmapFontTest3.fnt");
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
}
