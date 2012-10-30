using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LayerTest1 : LayerTest
    {
        int kTagLayer = 1;
        int kCCMenuTouchPriority = -128;

        public override void OnEnter()
        {
            base.OnEnter();

            this.TouchEnabled = true;

            CCSize s = CCDirector.SharedDirector.WinSize;
            CCLayerColor layer = CCLayerColor.Create(new ccColor4B(0xFF, 0x00, 0x00, 0x80), 200, 200);

            layer.IgnoreAnchorPointForPosition = false;
            layer.Position = (new CCPoint(s.width / 2, s.height / 2));
            AddChild(layer, 1, kTagLayer);
        }

        public override string title()
        {
            return "ColorLayer resize (tap & move)";
        }

        public override void RegisterWithTouchDispatcher()
        {
            CCDirector.SharedDirector.TouchDispatcher.AddTargetedDelegate(this, kCCMenuTouchPriority + 1, true);
        }

        public void updateSize(CCPoint touchLocation)
        {
            CCSize s = CCDirector.SharedDirector.WinSize;
            CCSize newSize = new CCSize(Math.Abs(touchLocation.x - s.width / 2) * 2, Math.Abs(touchLocation.y - s.height / 2) * 2);
            CCLayerColor l = (CCLayerColor)GetChildByTag(kTagLayer);
            l.ContentSize = newSize;
        }

        public override bool TouchBegan(CCTouch touche, CCEvent events)
        {
            updateSize(touche.Location);
            return true;
        }

        public override void TouchMoved(CCTouch touche, CCEvent events)
        {
            updateSize(touche.Location);
        }

        public override void TouchEnded(CCTouch touche, CCEvent events)
        {
            updateSize(touche.Location);
        }
    }
}
