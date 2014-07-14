using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Atlas3 : AtlasDemo
    {
        float m_time;

		CCLabelBMFont label1, label2, label3;

        public Atlas3()
        {
            m_time = 0;

			var col = new CCLayerColor(new CCColor4B(128, 128, 128, 255));
			//var col = new CCLayerColor(new CCColor4B(Microsoft.Xna.Framework.Color.Black));
            AddChild(col, -10);

			label1 = new CCLabelBMFont("Test", "fonts/bitmapFontTest2.fnt");

            // testing anchors
			label1.AnchorPoint = CCPoint.AnchorLowerLeft;
            AddChild(label1, 0, (int)TagSprite.kTagBitmapAtlas1);

			var fade = new CCFadeOut  (1.0f);
			var fade_in = fade.Reverse();
			label1.RepeatForever ( fade, fade_in);


            // VERY IMPORTANT
            // color and opacity work OK because bitmapFontAltas2 loads a BMP image (not a PNG image)
            // If you want to use both opacity and color, it is recommended to use NON premultiplied images like BMP images
            // Of course, you can also tell XCode not to compress PNG images, but I think it doesn't work as expected
			label2 = new CCLabelBMFont("Test", "fonts/bitmapFontTest2.fnt");
            // testing anchors
			label2.AnchorPoint = CCPoint.AnchorMiddle;
            label2.Color = CCColor3B.Red;
            AddChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);

			label2.RepeatForever( new CCTintTo (1, 255, 0, 0), new CCTintTo (1, 0, 255, 0), new CCTintTo (1, 0, 0, 255));

			label3 = new CCLabelBMFont("Test", "fonts/bitmapFontTest2.fnt");
            // testing anchors
			label3.AnchorPoint = CCPoint.AnchorUpperRight;
            AddChild(label3, 0, (int)TagSprite.kTagBitmapAtlas3);

            base.Schedule(step);
        }

        protected override void AddedToNewScene()
        {
            base.AddedToNewScene();

            var s = Scene.VisibleBoundsWorldspace.Size;;

			label1.Position = CCVisibleRect.LeftBottom;
			label2.Position = CCVisibleRect.Center;
			label3.Position = CCVisibleRect.RightTop;
		}


        public virtual void step(float dt)
        {
            m_time += dt;
            string stepString;
            stepString = string.Format("{0,2:f2} Test j", m_time);

			var label1 = (CCLabelBMFont)GetChildByTag((int)TagSprite.kTagBitmapAtlas1);
            label1.Text = stepString;

			var label2 = (CCLabelBMFont)GetChildByTag((int)TagSprite.kTagBitmapAtlas2);
            label2.Text = stepString;

			var label3 = (CCLabelBMFont)GetChildByTag((int)TagSprite.kTagBitmapAtlas3);
            label3.Text = stepString;
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
