using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class TestLayer1 : CCLayer
    {
        public TestLayer1()
        {
            float x, y;

            CCSize size = CCApplication.SharedApplication.MainWindowDirector.WinSize;
            x = size.Width;
            y = size.Height;

            CCSprite bg1 = new CCSprite(TransitionsTestScene.s_back1);
            bg1.Position = (new CCPoint(size.Width / 2, size.Height / 2));
			bg1.ScaleX = size.Width / bg1.ContentSize.Width;
			bg1.ScaleY = size.Height / bg1.ContentSize.Height;
            AddChild(bg1, -1);

            CCLabelTtf title = new CCLabelTtf((TransitionsTestScene.transitions[TransitionsTestScene.s_nSceneIdx]), "arial", 32);
            AddChild(title);
            title.Color = new CCColor3B(255, 32, 32);
            title.Position = new CCPoint(x / 2, y - 100);

			CCLabelTtf label = new CCLabelTtf("SCENE 1", "arial", 26);
            label.Color = (new CCColor3B(16, 16, 255));
            label.Position = (new CCPoint(x / 2, y / 2));
            AddChild(label);

            // menu
            CCMenuItemImage item1 = new CCMenuItemImage(TransitionsTestScene.s_pPathB1, TransitionsTestScene.s_pPathB2, backCallback);
            CCMenuItemImage item2 = new CCMenuItemImage(TransitionsTestScene.s_pPathR1, TransitionsTestScene.s_pPathR2, restartCallback);
            CCMenuItemImage item3 = new CCMenuItemImage(TransitionsTestScene.s_pPathF1, TransitionsTestScene.s_pPathF2, nextCallback);

            CCMenu menu = new CCMenu(item1, item2, item3);

            menu.Position = CCPoint.Zero;
			item1.Position = new CCPoint(x / 2 - item2.ContentSize.Width * 2, item2.ContentSize.Height / 2);
			item2.Position = new CCPoint(x / 2, item2.ContentSize.Height / 2);
			item3.Position = new CCPoint(x / 2 + item2.ContentSize.Width * 2, item2.ContentSize.Height / 2);

            AddChild(menu, 1);
            Schedule(step, 1.0f);
        }

        public void restartCallback(object pSender)
        {
            CCScene s = new TransitionsTestScene();
            CCLayer pLayer = new TestLayer2();
            s.AddChild(pLayer);

            CCScene pScene = TransitionsTestScene.createTransition(TransitionsTestScene.s_nSceneIdx, TransitionsTestScene.TRANSITION_DURATION, s);
            if (pScene != null)
            {
                CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(pScene);
            }
        }

        public void nextCallback(object pSender)
        {
            TransitionsTestScene.s_nSceneIdx++;
            TransitionsTestScene.s_nSceneIdx = TransitionsTestScene.s_nSceneIdx % TransitionsTestScene.MAX_LAYER;

            CCScene s = new TransitionsTestScene();
            CCLayer pLayer = new TestLayer2();
            s.AddChild(pLayer);

            CCScene pScene = TransitionsTestScene.createTransition(TransitionsTestScene.s_nSceneIdx, TransitionsTestScene.TRANSITION_DURATION, s);
            if (pScene != null)
            {
                CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(pScene);
            }
        }

        public void backCallback(object pSender)
        {
            TransitionsTestScene.s_nSceneIdx--;
            int total = TransitionsTestScene.MAX_LAYER;
            if (TransitionsTestScene.s_nSceneIdx < 0)
                TransitionsTestScene.s_nSceneIdx += total;

            CCScene s = new TransitionsTestScene();
            CCLayer pLayer = new TestLayer2();
            s.AddChild(pLayer);

            CCScene pScene = TransitionsTestScene.createTransition(TransitionsTestScene.s_nSceneIdx, TransitionsTestScene.TRANSITION_DURATION, s);
            if (pScene != null)
            {
                CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(pScene);
            }
        }

        public void step(float dt)
        {

        }
    }
}
