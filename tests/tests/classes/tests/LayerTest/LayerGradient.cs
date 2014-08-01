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
		CCMenu menu;
		CCLayerGradient gradientLayer;

        public LayerGradient()
        {
            gradientLayer = new CCLayerGradient(new CCColor4B(255, 0, 0, 255), new CCColor4B(0, 255, 0, 255), new CCPoint(0.9f, 0.9f));

            AddChild(gradientLayer, 0, kTagLayer);

			// Register Touch Event
			var touchListener = new CCEventListenerTouchAllAtOnce();
			touchListener.OnTouchesMoved = onTouchesMoved;
			AddEventListener(touchListener);

            CCLabelTtf label1 = new CCLabelTtf("Compressed Interpolation: Enabled", "arial", 26);
            CCLabelTtf label2 = new CCLabelTtf("Compressed Interpolation: Disabled", "arial", 26);
            CCMenuItemLabelTTF item1 = new CCMenuItemLabelTTF(label1);
            CCMenuItemLabelTTF item2 = new CCMenuItemLabelTTF(label2);
            CCMenuItemToggle item = new CCMenuItemToggle((toggleItem), item1, item2);

            menu = new CCMenu(item);
            AddChild(menu);
        }

        protected override void AddedToScene()
		{
            base.AddedToScene();

            CCSize s = Layer.VisibleBoundsWorldspace.Size;
			menu.Position = (new CCPoint(s.Width / 2, 100));
		}

		void onTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            CCSize s = Layer.VisibleBoundsWorldspace.Size;

            var it = touches.FirstOrDefault();
            CCTouch touch = (CCTouch)(it);
            var start = touch.LocationOnScreen;

            CCPoint diff = new CCPoint(s.Width / 2 - start.X, s.Height / 2 - start.Y);
            diff = CCPoint.Normalize(diff);

            gradientLayer.Vector = diff;
        }

		public override string Title
		{
			get
			{
				return "LayerGradient";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Touch the screen and move your finger";
			}
		}

        public void toggleItem(object sender)
        {
            
            gradientLayer.IsCompressedInterpolation = !gradientLayer.IsCompressedInterpolation;
        }
    }
}
