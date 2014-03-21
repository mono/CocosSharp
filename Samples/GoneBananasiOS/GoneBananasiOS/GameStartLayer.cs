using System;
using System.Collections.Generic;
using CocosSharp;

namespace GoneBananas
{
    public class GameStartLayer : CCLayerColor
    {
        public GameStartLayer ()
        {
            var touchListener = new CCEventListenerTouchAllAtOnce ();
			touchListener.OnTouchesEnded = (touches, ccevent) => CCDirector.SharedDirector.ReplaceScene (GameLayer.Scene);
       
            EventDispatcher.AddEventListener (touchListener, this);

            var label = new CCLabelTtf ("Tap Screen to Go Bananas!", "MarkerFelt", 22) {
                Position = CCDirector.SharedDirector.WinSize.Center,
                Color = new CCColor3B (CCColor4B.Blue),
                HorizontalAlignment = CCTextAlignment.Center,
                VerticalAlignment = CCVerticalTextAlignment.Center,
                Dimensions = ContentSize
            };

            AddChild (label);

            Color = new CCColor3B (CCColor4B.AliceBlue);
            Opacity = 255;
        }

        public static CCScene Scene {
            get {
                var scene = new CCScene ();
                var layer = new GameStartLayer ();

                scene.AddChild (layer);

                return scene;
            }
        }
    }
}

