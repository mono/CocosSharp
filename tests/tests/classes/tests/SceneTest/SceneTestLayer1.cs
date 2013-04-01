using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using cocos2d.menu_nodes;
using System.Diagnostics;

namespace tests
{
    public class SceneTestLayer1 : CCLayer
    {
        string s_pPathGrossini = "Images/grossini";

        public SceneTestLayer1()
        {
            CCMenuItemFont item1 = CCMenuItemFont.Create("Test pushScene", onPushScene);
            CCMenuItemFont item2 = CCMenuItemFont.Create("Test pushScene w/transition", onPushSceneTran);
            CCMenuItemFont item3 = CCMenuItemFont.Create("Quit", onQuit);

            CCMenu menu = CCMenu.Create(item1, item2, item3);
            menu.AlignItemsVertically();

            AddChild(menu);

            CCSize s = CCDirector.SharedDirector.WinSize;
            CCSprite sprite = CCSprite.Create(s_pPathGrossini);
            AddChild(sprite);
            sprite.Position = new CCPoint(s.Width - 40, s.Height / 2);
            CCActionInterval rotate = new CCRotateBy (2, 360);
            CCAction repeat = CCRepeatForever.Create(rotate);
            sprite.RunAction(repeat);

            Schedule(testDealloc);
        }

        public override void OnEnter()
        {
            CCLog.Log("SceneTestLayer1#onEnter");
            base.OnEnter();
        }

        public override void OnEnterTransitionDidFinish()
        {
            CCLog.Log("SceneTestLayer1#onEnterTransitionDidFinish");
            base.OnEnterTransitionDidFinish();
        }

        public void testDealloc(float dt)
        {
            //UXLOG("SceneTestLayer1:testDealloc");    
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

            CCDirector.SharedDirector.PushScene(CCTransitionSlideInT.Create(1f, scene));
        }

        public void onQuit(object pSender) 
        {
            //getCocosApp()->exit();
            //CCDirector::sharedDirector()->popScene();

            //// HA HA... no more terminate on sdk v3.0
            //// http://developer.apple.com/iphone/library/qa/qa2008/qa1561.html
            //if( [[UIApplication sharedApplication] respondsToSelector:@selector(terminate)] )
            //	[[UIApplication sharedApplication] performSelector:@selector(terminate)];
        }
    }
}
