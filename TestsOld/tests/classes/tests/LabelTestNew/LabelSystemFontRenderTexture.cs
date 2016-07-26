using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelSystemFontRenderTexture : AtlasDemoNew
    {
        CCLabel label1;
        CCSize size = new CCSize(300, 300);
        CCRenderTexture txtLabel;

        public LabelSystemFontRenderTexture()
        {

            Color = new CCColor3B(200, 191, 231);
            Opacity = 255;

            label1 = new CCLabel("Visit Rendering", "fonts/MorrisRoman-Black.ttf", 30, CCLabelFormat.SystemFont)
                {
                    Color = CCColor3B.Orange,
                    AnchorPoint = CCPoint.AnchorMiddleLeft,
                    Dimensions = size,
                };
            label1.LabelFormat.Alignment = CCTextAlignment.Center;

            txtLabel = new CCRenderTexture (size, size);


            txtLabel.BeginWithClear (CCColor4B.AliceBlue);
            label1.Visit ();
            txtLabel.End ();

            AddChild(txtLabel.Sprite);

        }


        protected override void AddedToScene()
        {
            base.AddedToScene();

            txtLabel.Sprite.Position = VisibleBoundsWorldspace.Center;

        }

        public override string Title
		{
            get {
                return "New Label";
            }
        }

        public override string Subtitle
        {
            get {
                return "Render to CCRenderTexture\nShould not crash - Issue 200";
            }
        }
    }
}
