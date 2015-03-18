using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelFNTSpriteActions : AtlasDemoNew
    {
        //ccTime m_time;
        float m_time;

		CCLabel label, label2;

        public LabelFNTSpriteActions()
        {
            m_time = 0;

            // Upper Label
			label = new CCLabel("Bitmap Font Atlas", "fonts/bitmapFontTest.fnt");
            AddChild(label);

			label.AnchorPoint = CCPoint.AnchorMiddle;


			var BChar = label[0];
			var FChar = label[7];
			var AChar = label[12];


			var rotate = new CCRotateBy (2, 360);
			var rot_4ever = new CCRepeatForever (rotate);

			var scale = new CCScaleBy(2, 1.5f);
			var scale_back = scale.Reverse();
			var scale_seq = new CCSequence(scale, scale_back);
			var scale_4ever = new CCRepeatForever (scale_seq);

            var jump = new CCJumpBy (0.5f, new CCPoint(), 60, 1);
            var jump_4ever = new CCRepeatForever (jump);

			var fade_out = new CCFadeOut  (1);
			var fade_in = new CCFadeIn  (1);
			var seq = new CCSequence(fade_out, fade_in);
			var fade_4ever = new CCRepeatForever (seq);

            BChar.RunAction(rot_4ever);
            BChar.RunAction(scale_4ever);
            FChar.RunAction(jump_4ever);
			AChar.RunAction(fade_4ever);


            // Bottom Label
			label2 = new CCLabel("00.0", "fonts/bitmapFontTest.fnt");
            AddChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);

			var lastChar = label2[3];
            lastChar.RunAction(rot_4ever);

            //schedule( schedule_selector(Atlas4::step), 0.1f);
            base.Schedule(step, 0.1f);
        }


        protected override void AddedToScene()
        {
            base.AddedToScene();

            var s = Layer.VisibleBoundsWorldspace.Size;

			label.Position = s.Center;
			label2.Position = new CCPoint(s.Width / 2.0f, 80);
		}
        public virtual void step(float dt)
        {
            m_time += dt;

			var Stepstring = string.Format("{0,4:f1}", m_time);

			var label1 = (CCLabel)GetChildByTag((int)TagSprite.kTagBitmapAtlas2);
            label1.Text = (Stepstring);
        }

        protected override void Draw()
        {
            CCSize s = Layer.VisibleBoundsWorldspace.Size;

            CCDrawingPrimitives.Begin();
            CCDrawingPrimitives.DrawLine(new CCPoint(0, s.Height / 2), new CCPoint(s.Width, s.Height / 2));
            CCDrawingPrimitives.DrawLine(new CCPoint(s.Width / 2, 0), new CCPoint(s.Width / 2, s.Height));
            CCDrawingPrimitives.End();
        }

        public override string title()
        {
            return "New Label + .FNT file";
        }

        public override string subtitle()
        {
            return "Using fonts as Sprite objects. Some characters should rotate.";
        }
    }
}
