using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using System.Diagnostics;

namespace tests
{
    public class Bug914Layer : BugsTestBaseLayer
    {
        public static CCScene scene()
        {
            // 'scene' is an autorelease object.
            CCScene pScene = CCScene.Create();
            // 'layer' is an autorelease object.
            //Bug914Layer layer = Bug914Layer.node();

            // add layer as a child to scene
            //pScene.addChild(layer);

            // return the scene
            return pScene;
        }

        public override bool Init()
        {
            // always call "super" init
            // Apple recommends to re-assign "self" with the "super" return value
            if (base.Init())
            {
                TouchEnabled = true;
                // ask director the the window size
                CCSize size = CCDirector.SharedDirector.WinSize;
                CCLayerColor layer;
                for (int i = 0; i < 5; i++)
                {
                    layer = CCLayerColor.Create(new ccColor4B((byte)(i*20), (byte)(i*20), (byte)(i*20),255));
                    layer.ContentSize = new CCSize(i * 100, i * 100);
                    layer.Position = new CCPoint(size.Width / 2, size.Height / 2);
                    layer.AnchorPoint = new CCPoint(0.5f, 0.5f);
                    layer.IgnoreAnchorPointForPosition = true;
                    AddChild(layer, -1 - i);
                    
                }

                // create and initialize a Label
                CCLabelTTF label = CCLabelTTF.Create("Hello World", "Marker Felt", 64);
                CCMenuItem item1 = CCMenuItemFont.Create("restart", restart);

                CCMenu menu = CCMenu.Create(item1);
                menu.AlignItemsVertically();
                menu.Position = new CCPoint(size.Width / 2, 100);
                AddChild(menu);

                // position the label on the center of the screen
                label.Position = new CCPoint(size.Width / 2, size.Height / 2);

                // add the label as a child to this Layer
                AddChild(label);
                return true;
            }
            return false;
        }

        public void ccTouchesMoved(List<CCTouch> touches, CCEvent eventn)
        {
            CCLog.Log("Number of touches: %d", touches.Count);
        }

        public void ccTouchesBegan(List<CCTouch> touches, CCEvent eventn)
        {
            ccTouchesMoved(touches, eventn);
        }

        public void restart(CCObject sender)
        {
            CCDirector.SharedDirector.ReplaceScene(Bug914Layer.scene());
        }

        //LAYER_NODE_FUNC(Bug914Layer);
    }
}
