using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using System.Diagnostics;

namespace tests
{
    public class Bug914Layer : BugsTestBaseLayer
    {
        public static CCScene scene()
        {
            // 'scene' is an autorelease object.
            CCScene pScene = new CCScene();
            // 'layer' is an autorelease object.
            //Bug914Layer layer = Bug914Layer.node();

            // add layer as a child to scene
            //pScene.addChild(layer);

            // return the scene
            return pScene;
        }

        public Bug914Layer()
        {
            // always call "super" init
            // Apple recommends to re-assign "self" with the "super" return value
			// Register Touch Event
			var touchListener = new CCEventListenerTouchAllAtOnce();

			touchListener.OnTouchesBegan = onTouchesBegan;
			touchListener.OnTouchesMoved = onTouchesMoved;

            EventDispatcher.AddEventListener(touchListener, this);

            // ask director the the window size
            CCSize size = CCApplication.SharedApplication.MainWindowDirector.WindowSizeInPoints;
            CCLayerColor layer;
            for (int i = 0; i < 5; i++)
            {
                layer = new CCLayerColor(new CCColor4B((byte)(i*20), (byte)(i*20), (byte)(i*20),255));
                layer.ContentSize = new CCSize(i * 100, i * 100);
                layer.Position = new CCPoint(size.Width / 2, size.Height / 2);
                layer.AnchorPoint = new CCPoint(0.5f, 0.5f);
                layer.IgnoreAnchorPointForPosition = true;
                AddChild(layer, -1 - i);
                    
            }

            // create and initialize a Label
            CCLabelTtf label = new CCLabelTtf("Hello World", "Marker Felt", 64);
            CCMenuItem item1 = new CCMenuItemFont("restart", restart);

            CCMenu menu = new CCMenu(item1);
            menu.AlignItemsVertically();
            menu.Position = new CCPoint(size.Width / 2, 100);
            AddChild(menu);

            // position the label on the center of the screen
            label.Position = new CCPoint(size.Width / 2, size.Height / 2);

            // add the label as a child to this Layer
            AddChild(label);
        }

		void onTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            CCLog.Log("Number of touches: %d", touches.Count);
        }

		void onTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
			onTouchesMoved(touches, touchEvent);
        }

        public void restart(object sender)
        {
            CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(Bug914Layer.scene());
        }

        //LAYER_NODE_FUNC(Bug914Layer);
    }
}
