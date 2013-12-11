using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SceneTestLayer2 : CCLayer
    {
        string s_pPathGrossini = "Images/grossini";
        float m_timeCounter;
        private CCMenuItemFont _PopMenuItem;
        private CCMenu _TheMenu;

        public SceneTestLayer2()
        {
            m_timeCounter = 0;

            CCMenuItemFont item1 = new CCMenuItemFont("(2) replaceScene", onReplaceScene);
            CCMenuItemFont item2 = new CCMenuItemFont("(2) replaceScene w/transition", onReplaceSceneTran);
            CCMenuItemFont item3 = new CCMenuItemFont("(2) Go Back", onGoBack);
            _PopMenuItem = new CCMenuItemFont("(2) Test popScene w/transition", onPopSceneTran);

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

            Schedule(testDealloc);
        }

        public void testDealloc(float dt)
        {
            //m_timeCounter += dt;
            //if( m_timeCounter > 10 )
            //	onReplaceScene(this);
        }

        public void onGoBack(object pSender)
        {
            CCDirector.SharedDirector.PopScene();
        }

        public override void OnEnter()
        {
            CCLog.Log("SceneTestLayer2#onEnter");
            base.OnEnter();
            _PopMenuItem.Visible = CCDirector.SharedDirector.CanPopScene;
            _TheMenu.AlignItemsVerticallyWithPadding(12f);
        }

        public void onReplaceScene(object pSender)
        {
            CCScene pScene = new SceneTestScene();
            CCLayer pLayer = new SceneTestLayer3();
            pScene.AddChild(pLayer, 0);
            CCDirector.SharedDirector.ReplaceScene(pScene);
        }

        public void onReplaceSceneTran(object pSender)
        {
            CCScene pScene = new SceneTestScene();
            CCLayer pLayer = new SceneTestLayer3();
            pScene.AddChild(pLayer, 0);
            CCDirector.SharedDirector.ReplaceScene(new CCTransitionFlipX(2, pScene, CCTransitionOrientation.UpOver));
        }

        public void onPopSceneTran(object pSender)
        {
            CCScene scene = new SceneTestScene();
            CCLayer pLayer = new SceneTestLayer1();
            scene.AddChild(pLayer, 0);

            CCDirector.SharedDirector.PopScene(1f, new CCTransitionSlideInB(1f, scene));
        }

        //CREATE_NODE(SceneTestLayer2);
    }
}
