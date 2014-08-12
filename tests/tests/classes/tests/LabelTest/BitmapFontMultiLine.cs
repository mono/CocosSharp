using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using System.Diagnostics;

namespace tests
{
    public class BitmapFontMultiLine : AtlasDemo
    {

		CCLabelBMFont label1, label2, label3;

        public BitmapFontMultiLine()
        {
            CCSize s;

            // Left
            label1 = new CCLabelBMFont("Multi line\nLeft", "fonts/bitmapFontTest3.fnt");
            label1.AnchorPoint = new CCPoint(0, 0);
            AddChild(label1, 0, (int)TagSprite.kTagBitmapAtlas1);

            s = label1.ContentSize;

            //CCLOG("content size: %.2fx%.2f", s.width, s.height);
            CCLog.Log("content size: {0,0:2f}x{1,0:2f}", s.Width, s.Height);


            // Center
            label2 = new CCLabelBMFont("Multi line\nCenter", "fonts/bitmapFontTest3.fnt");
            label2.AnchorPoint = new CCPoint(0.5f, 0.5f);
            AddChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);

            s = label2.ContentSize;
            //CCLOG("content size: %.2fx%.2f", s.width, s.height);
            CCLog.Log("content size: {0,0:2f}x{1,0:2f}", s.Width, s.Height);

            // right
            label3 = new CCLabelBMFont("Multi line\nRight\nThree lines Three", "fonts/bitmapFontTest3.fnt");
            label3.AnchorPoint = new CCPoint(1, 1);
            AddChild(label3, 0, (int)TagSprite.kTagBitmapAtlas3);

            s = label3.ContentSize;
            //CCLOG("content size: %.2fx%.2f", s.width, s.height);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var visibleRect = VisibleBoundsWorldspace;

            label1.Position = visibleRect.LeftBottom();
            label2.Position = visibleRect.Center();
            label3.Position = visibleRect.RightTop();

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
