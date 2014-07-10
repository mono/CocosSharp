using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LayerGradient : LayerTest
    {
        int kTagLayer = 1;
        public LayerGradient()
        {
            CCLayerGradient layer1 = new CCLayerGradient(new CCColor4B(255, 0, 0, 255), new CCColor4B(0, 255, 0, 255), new CCPoint(0.9f, 0.9f));

            AddChild(layer1, 0, kTagLayer);

			// Register Touch Event
			var touchListener = new CCEventListenerTouchAllAtOnce();
			touchListener.OnTouchesMoved = onTouchesMoved;
			EventDispatcher.AddEventListener(touchListener, this);

            CCLabelTtf label1 = new CCLabelTtf("Compressed Interpolation: Enabled", "arial", 26);
            CCLabelTtf label2 = new CCLabelTtf("Compressed Interpolation: Disabled", "arial", 26);
            CCMenuItemLabelTTF item1 = new CCMenuItemLabelTTF(label1);
            CCMenuItemLabelTTF item2 = new CCMenuItemLabelTTF(label2);
            CCMenuItemToggle item = new CCMenuItemToggle((toggleItem), item1, item2);

            CCMenu menu = new CCMenu(item);
            AddChild(menu);
            CCSize s = Scene.VisibleBoundsWorldspace.Size;
            menu.Position = (new CCPoint(s.Width / 2, 100));
        }

		void onTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            CCSize s = Scene.VisibleBoundsWorldspace.Size;

            var it = touches.FirstOrDefault();
            CCTouch touch = (CCTouch)(it);
            var start = touch.LocationOnScreen;

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
