using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelSystemFontColorAndOpacity : AtlasDemoNew
    {
        float m_time;

		CCLabel label1, label2, label3;

        public LabelSystemFontColorAndOpacity()
        {
            m_time = 0;

            Color = new CCColor3B(128, 128, 128);
            Opacity = 255;

            label1 = new CCLabel("Label FadeOut/FadeIn", "fonts/Marker Felt.ttf", 40, CCLabelFormat.SystemFont)
                { 
                    Color = CCColor3B.Orange,
                    // testing anchors
                    AnchorPoint = CCPoint.AnchorLowerLeft
                };

            AddChild(label1, 0, (int)TagSprite.kTagBitmapAtlas1);

			var fade = new CCFadeOut  (1.0f);
			var fade_in = fade.Reverse();
			label1.RepeatForever ( fade, fade_in);

            label2 = new CCLabel("Label TintTo", "fonts/MorrisRoman-Black.ttf", 40, CCLabelFormat.SystemFont)                
                { 
                    Color = CCColor3B.Red,
                    // testing anchors
                    AnchorPoint = CCPoint.AnchorMiddle
                };

            AddChild(label2, 0, (int)TagSprite.kTagBitmapAtlas2);

			label2.RepeatForever( new CCTintTo (1, 255, 0, 0), new CCTintTo (1, 0, 255, 0), new CCTintTo (1, 0, 0, 255));

            label3 = new CCLabel("Label\nPlain\nBlue", "Helvetica", 40, CCLabelFormat.SystemFont)
                { 
                    Color = CCColor3B.Blue,
                    Opacity = 128,
                    // testing anchors
                    AnchorPoint = CCPoint.AnchorUpperRight
                };
            
             AddChild(label3, 0, (int)TagSprite.kTagBitmapAtlas3);

            //base.Schedule(step);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            var visibleRect = VisibleBoundsWorldspace;

            label1.Position = visibleRect.LeftBottom();
            label2.Position = visibleRect.Center();
            label3.Position = visibleRect.RightTop();
		}


//        public virtual void step(float dt)
//        {
//            m_time += dt;
//            string stepString;
//            stepString = string.Format("{0,2:f2} Test j", m_time);
//
//            label1.Text = stepString;
//
//            label2.Text = stepString;
//
//            label3.Text = stepString;
//        }

        public override string Title
        {
            get {
                return "New Label + System Font";
            }
        }

        public override string Subtitle
        {
            get {
                return "Testing opacity + tint";
            }
        }

    }
}
