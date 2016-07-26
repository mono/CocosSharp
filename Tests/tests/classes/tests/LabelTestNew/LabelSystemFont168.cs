using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LabelSystemFont168 : AtlasDemoNew
    {
        CCNode label1;
        CCFadeOutBLTiles fadeOut;
        int count = 1;

        public LabelSystemFont168()
        {

            Color = new CCColor3B(200, 191, 231);
            Opacity = 255;

            fadeOut = new CCFadeOutBLTiles(1f, new CCGridSize(10, 10));

            Schedule(StressIt, 2.0f);
        }

        public static CCNode CreateLabel(string text, float size, CCColor3B color, CCColor3B shadowColor)
        {
            var node = new CCNode () {
                AnchorPoint = CCPoint.AnchorMiddle,
            };


            size *= 1.2f;

            var lbl = new CCLabel(text, "fonts/MorrisRoman-Black.ttf", size, CCLabelFormat.SystemFont)
                {
                    Color = color,
                    AnchorPoint = CCPoint.AnchorMiddle,
                };
            lbl.LabelFormat.Alignment = CCTextAlignment.Center;

            node.ContentSize = lbl.ScaledContentSize;
            lbl.Position = node.ContentSize.Center;

            if (shadowColor != CCColor3B.Magenta)
            {
                node.ContentSize = lbl.ContentSize + 2;
                var shadowLbl = new CCLabel (text, "fonts/MorrisRoman-Black.ttf", size, CCLabelFormat.SystemFont) {
                    Color = shadowColor,
                    AnchorPoint = CCPoint.AnchorMiddle,
                    Position = new CCPoint(node.ContentSize.Center.X + 2, node.ContentSize.Center.Y - 2)
                };
                shadowLbl.LabelFormat.Alignment = CCTextAlignment.Center;
                node.AddChild (shadowLbl);
            }
            node.AddChild (lbl);
            return node;
        }

        async void StressIt (float dt)
        {

            if (this.NumberOfRunningActions > 0)
                return;
            
            var targetNode = new CCNodeGrid();
            AddChild(targetNode);

            //lbl = new CCSprite("images/bat2.png");
            label1 = CreateLabel (string.Format("Score: {0:n0} - Level: {1}", 123456, count), 30, CCColor3B.Yellow, CCColor3B.Blue);
            label1.Position = this.ContentSize.Center;
            targetNode.AddChild(label1);
            count++;

            await targetNode.RunActionAsync(fadeOut);

            // Make sure we unset our grid that was attached.
            targetNode.Grid = null;

            label1.RemoveFromParent();
            label1 = null; 

            targetNode.RemoveFromParent();

//            GC.Collect ();
//            GC.WaitForPendingFinalizers ();
//            GC.Collect ();
        }

        public override string Title
		{
            get {
                return "New Label + SystemFont";
            }
        }

        public override string Subtitle
        {
            get {
                return "Should not crash - Issue 168";
            }
        }
    }
}
