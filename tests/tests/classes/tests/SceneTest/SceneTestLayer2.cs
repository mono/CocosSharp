using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using cocos2d.menu_nodes;

namespace tests
{
    public class SceneTestLayer2 : CCLayer
    {
        string s_pPathGrossini = "Images/grossini";
        float m_timeCounter;

        public SceneTestLayer2()
        {
            m_timeCounter = 0;

            CCMenuItemFont item1 = CCMenuItemFont.Create("replaceScene", onReplaceScene);
            CCMenuItemFont item2 = CCMenuItemFont.Create("replaceScene w/transition", onReplaceSceneTran);
            CCMenuItemFont item3 = CCMenuItemFont.Create("Go Back", onGoBack);

            CCMenu menu = CCMenu.Create(item1, item2, item3);
            menu.AlignItemsVertically();

            AddChild(menu);

            CCSize s = CCDirector.SharedDirector.WinSize;
            CCSprite sprite = CCSprite.Create(s_pPathGrossini);
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
            CCDirector.SharedDirector.ReplaceScene(CCTransitionFlipX.Create(2, pScene, tOrientation.kOrientationUpOver));
        }

        //CREATE_NODE(SceneTestLayer2);
    }
}
