using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;
using System.Diagnostics;

namespace tests
{
    public class SceneTestLayer1 : CCLayer
    {
        string s_pPathGrossini = "Images/grossini";
        private CCMenuItemFont _PopMenuItem;
        private CCMenu _TheMenu;

        public SceneTestLayer1()
        {
            CCMenuItemFont item1 = new CCMenuItemFont("(1) Test pushScene", onPushScene);
            CCMenuItemFont item2 = new CCMenuItemFont("(1) Test pushScene w/transition", onPushSceneTran);
            CCMenuItemFont item3 = new CCMenuItemFont("(1) Quit", onQuit);
            _PopMenuItem = new CCMenuItemFont("(1) Test popScene w/transition", onPopSceneTran);

            _TheMenu = new CCMenu(item1, item2, item3, _PopMenuItem);
            _TheMenu.AlignItemsVertically();

            AddChild(_TheMenu);

            CCSize s = CCDirector.SharedDirector.WinSize;
            CCSprite sprite = new CCSprite(s_pPathGrossini);
            AddChild(sprite);
            sprite.Position = new CCPoint(s.Width - 40, s.Height / 2);
            CCActionInterval rotate = new CCRotateBy (2, 360);
            CCAction repeat = new CCRepeatForever (rotate);
            sprite.RunAction(repeat);
        }

        public override void OnEnter()
        {
            CCLog.Log("SceneTestLayer1#onEnter");
            base.OnEnter();
            _PopMenuItem.Visible = CCDirector.SharedDirector.CanPopScene;
            _TheMenu.AlignItemsVerticallyWithPadding(12f);
        }

        public override void OnEnterTransitionDidFinish()
        {
            CCLog.Log("SceneTestLayer1#onEnterTransitionDidFinish");
            base.OnEnterTransitionDidFinish();
        }

        public void onPushScene(object pSender)
        {
            CCScene scene = new SceneTestScene();
            CCLayer pLayer = new SceneTestLayer2();
            scene.AddChild(pLayer, 0);
            CCDirector.SharedDirector.PushScene(scene);
        }

        public void onPushSceneTran(object pSender)
        {
            CCScene scene = new SceneTestScene();
            CCLayer pLayer = new SceneTestLayer2();
            scene.AddChild(pLayer, 0);

            CCDirector.SharedDirector.PushScene(new CCTransitionSlideInT(1f, scene));
        }

        public void onPopSceneTran(object pSender)
        {
            CCScene scene = new SceneTestScene();
            CCLayer pLayer = new SceneTestLayer2();
            scene.AddChild(pLayer, 0);

            CCDirector.SharedDirector.PopScene(1f, new CCTransitionSlideInB(1f, scene));
        }

        public void onQuit(object pSender) 
        {
            CCDirector.SharedDirector.PopToRootScene();
        }
    }
}
