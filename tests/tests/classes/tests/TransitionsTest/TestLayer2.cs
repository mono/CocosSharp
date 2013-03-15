using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TestLayer2 : CCLayer
    {
        public TestLayer2()
        {
            float x, y;

            CCSize size = CCDirector.SharedDirector.WinSize;
            x = size.Width;
            y = size.Height;

            CCSprite bg2 = CCSprite.Create(TransitionsTestScene.s_back2);
            bg2.Position = new CCPoint(size.Width / 2, size.Height / 2);
			bg2.ScaleX = size.Width / bg2.ContentSize.Width;
			bg2.ScaleY = size.Height / bg2.ContentSize.Height;
            AddChild(bg2, -1);

            CCLabelTTF title = CCLabelTTF.Create((TransitionsTestScene.transitions[TransitionsTestScene.s_nSceneIdx]), "arial", 32);
            AddChild(title);
            title.Color = new CCColor3B(255, 32, 32);
            title.Position = new CCPoint(x / 2, y - 100);

            CCLabelTTF label = CCLabelTTF.Create("SCENE 2", "markerFelt", 38);
            label.Color = new CCColor3B(16, 16, 255);
            label.Position = new CCPoint(x / 2, y / 2);
            AddChild(label);

            // menu
            CCMenuItemImage item1 = CCMenuItemImage.Create(TransitionsTestScene.s_pPathB1, TransitionsTestScene.s_pPathB2, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.Create(TransitionsTestScene.s_pPathR1, TransitionsTestScene.s_pPathR2, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.Create(TransitionsTestScene.s_pPathF1, TransitionsTestScene.s_pPathF2, nextCallback);

            CCMenu menu = CCMenu.Create(item1, item2, item3);

            menu.Position = CCPoint.Zero;
            item1.Position = new CCPoint(x / 2 - item2.ContentSize.Width * 2, item2.ContentSize.Height / 2);
            item2.Position = new CCPoint(x / 2, item2.ContentSize.Height / 2);
			item3.Position = new CCPoint(x / 2 + item2.ContentSize.Width * 2, item2.ContentSize.Height / 2);

            AddChild(menu, 1);
            Schedule(step, 1.0f);
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new TransitionsTestScene();

            CCLayer pLayer = new TestLayer1();
            s.AddChild(pLayer);

            CCScene pScene = TransitionsTestScene.createTransition(TransitionsTestScene.s_nSceneIdx, TransitionsTestScene.TRANSITION_DURATION, s);

            if (pScene != null)
            {
                CCDirector.SharedDirector.ReplaceScene(pScene);
            }
        }

        public void nextCallback(CCObject pSender)
        {
            TransitionsTestScene.s_nSceneIdx++;
            TransitionsTestScene.s_nSceneIdx = TransitionsTestScene.s_nSceneIdx % TransitionsTestScene.MAX_LAYER;

            CCScene s = new TransitionsTestScene();

            CCLayer pLayer = new TestLayer1();
            s.AddChild(pLayer);
            CCScene pScene = TransitionsTestScene.createTransition(TransitionsTestScene.s_nSceneIdx, TransitionsTestScene.TRANSITION_DURATION, s);

            if (pScene != null)
            {
                CCDirector.SharedDirector.ReplaceScene(pScene);
            }
        }

        public void backCallback(CCObject pSender)
        {
            TransitionsTestScene.s_nSceneIdx--;
            int total = TransitionsTestScene.MAX_LAYER;
            if (TransitionsTestScene.s_nSceneIdx < 0)
                TransitionsTestScene.s_nSceneIdx += total;

            CCScene s = new TransitionsTestScene();

            CCLayer pLayer = new TestLayer1();
            s.AddChild(pLayer);

            CCScene pScene = TransitionsTestScene.createTransition(TransitionsTestScene.s_nSceneIdx, TransitionsTestScene.TRANSITION_DURATION, s);
            
            if (pScene != null)
            {
                CCDirector.SharedDirector.ReplaceScene(pScene);
            }
        }

        public void step(float dt) { }
    }
}
