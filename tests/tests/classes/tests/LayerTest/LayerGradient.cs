using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using cocos2d.menu_nodes;

namespace tests
{
    public class LayerGradient : LayerTest
    {
        int kTagLayer = 1;
        public LayerGradient()
        {
            CCLayerGradient layer1 = CCLayerGradient.Create(new CCColor4B(255, 0, 0, 255), new CCColor4B(0, 255, 0, 255), new CCPoint(0.9f, 0.9f));
            AddChild(layer1, 0, kTagLayer);

            this.TouchEnabled = true;

            CCLabelTTF label1 = new CCLabelTTF("Compressed Interpolation: Enabled", "arial", 26);
            CCLabelTTF label2 = new CCLabelTTF("Compressed Interpolation: Disabled", "arial", 26);
            CCMenuItemLabel item1 = CCMenuItemLabel.Create(label1);
            CCMenuItemLabel item2 = CCMenuItemLabel.Create(label2);
            CCMenuItemToggle item = CCMenuItemToggle.Create((toggleItem), item1, item2);

            CCMenu menu = CCMenu.Create(item);
            AddChild(menu);
            CCSize s = CCDirector.SharedDirector.WinSize;
            menu.Position = (new CCPoint(s.Width / 2, 100));
        }

        public override void TouchesMoved(List<cocos2d.CCTouch> touches, cocos2d.CCEvent event_)
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            var it = touches.FirstOrDefault();
            CCTouch touch = (CCTouch)(it);
            CCPoint start = touch.LocationInView;
            start = CCDirector.SharedDirector.ConvertToGl(start);

            CCPoint diff = new CCPoint(s.Width / 2 - start.X, s.Height / 2 - start.Y);
            diff = CCPointExtension.Normalize(diff);

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
