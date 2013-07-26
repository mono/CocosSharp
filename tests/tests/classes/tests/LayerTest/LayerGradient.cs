using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class LayerGradient : LayerTest
    {
        int kTagLayer = 1;
        public LayerGradient()
        {
            CCLayerGradient layer1 = new CCLayerGradient(new CCColor4B(255, 0, 0, 255), new CCColor4B(0, 255, 0, 255), new CCPoint(0.9f, 0.9f));

            AddChild(layer1, 0, kTagLayer);

            this.TouchEnabled = true;

            CCLabelTTF label1 = new CCLabelTTF("Compressed Interpolation: Enabled", "arial", 26);
            CCLabelTTF label2 = new CCLabelTTF("Compressed Interpolation: Disabled", "arial", 26);
            CCMenuItemLabel item1 = new CCMenuItemLabel(label1);
            CCMenuItemLabel item2 = new CCMenuItemLabel(label2);
            CCMenuItemToggle item = new CCMenuItemToggle((toggleItem), item1, item2);

            CCMenu menu = new CCMenu(item);
            AddChild(menu);
            CCSize s = CCDirector.SharedDirector.WinSize;
            menu.Position = (new CCPoint(s.Width / 2, 100));
        }

        public override void TouchesMoved(List<CCTouch> touches)
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            var it = touches.FirstOrDefault();
            CCTouch touch = (CCTouch)(it);
            var start = touch.Location;

            CCPoint diff = new CCPoint(s.Width / 2 - start.X, s.Height / 2 - start.Y);
            diff = CCPoint.Normalize(diff);

            CCLayerGradient gradient = (CCLayerGradient)GetChildByTag(1);
            gradient.Vector = diff;
        }


        public override string title()
        {
            return "LayerGradient";
        }

        public override string subtitle()
        {
            return "Touch the screen and move your finger";
        }

        public void toggleItem(object sender)
        {
            CCLayerGradient gradient = (CCLayerGradient)GetChildByTag(kTagLayer);
            gradient.IsCompressedInterpolation = !gradient.IsCompressedInterpolation;
        }
    }
}
