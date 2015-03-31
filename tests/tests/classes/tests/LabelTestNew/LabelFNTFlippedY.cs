using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelFNTFlippedY : AtlasDemoNew
    {

        CCLabel label1;

        public LabelFNTFlippedY()
        {
            // CCLabel Bitmap Font
            label1 = new CCLabel("123\n456", "fonts/bitmapFontTest3.fnt");
            label1.AnchorPoint = CCPoint.AnchorLowerLeft;
            AddChild(label1);

            base.Schedule((dt) => 
                {
                    int len = label1.Text.Length;
                    for (int i = 0; i < len; ++i)
                    {
                        var sprite = (CCSprite)label1[i];
                        if (sprite != null)
                        {
                            sprite.FlipY = !sprite.FlipY;
                        }
                    }
                }
                , 2.0f);

        }


        protected override void AddedToScene()
        {
            base.AddedToScene();

            var s = Layer.VisibleBoundsWorldspace.Size;

            label1.Position = s.Center;
        }

        public override string Title
        {
            get {
                return "New Label .FNT file";
            }
        }

        public override string Subtitle
        {
            get {
                return "Label flips vertically every 2 seconds.";
            }
        }
    }
}
