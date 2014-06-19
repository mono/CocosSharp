using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using System.Diagnostics;

namespace tests
{
    public class SceneTestLayer1 : CCLayer
    {
        
		private CCMenuItemFont popMenuItem;
        private CCMenu theMenu;

        public SceneTestLayer1()
        {
            CCMenuItemFont item1 = new CCMenuItemFont("(1) Test pushScene", onPushScene);
            CCMenuItemFont item2 = new CCMenuItemFont("(1) Test pushScene w/transition", onPushSceneTran);
            CCMenuItemFont item3 = new CCMenuItemFont("(1) Quit", onQuit);
            popMenuItem = new CCMenuItemFont("(1) Test popScene w/transition", onPopSceneTran);

            theMenu = new CCMenu(item1, item2, item3, popMenuItem);
            theMenu.AlignItemsVertically();

            AddChild(theMenu);

			CCSprite sprite = new CCSprite(SceneTestScene.grossini) { Tag = SceneTestScene.GROSSINI_TAG };
            AddChild(sprite);
            
        }

		protected override void RunningOnNewWindow(CCSize windowSize)
		{
			base.RunningOnNewWindow(windowSize);

			var sprite = this[SceneTestScene.GROSSINI_TAG];
			var s = windowSize;
			sprite.Position = new CCPoint(s.Width - 40, s.Height / 2);
			sprite.RepeatForever(SceneTestScene.rotate);

		}

        public override void OnEnter()
        {
            base.OnEnter();
			CCLog.Log("SceneTestLayer1#onEnter - Can pop Scene = {0}", Director.CanPopScene);
            popMenuItem.Visible = Director.CanPopScene;
            theMenu.AlignItemsVertically(12f);
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
            Director.PushScene(scene);
        }

        public void onPushSceneTran(object pSender)
        {
            CCScene scene = new SceneTestScene();
            CCLayer pLayer = new SceneTestLayer2();
            scene.AddChild(pLayer, 0);

            Director.PushScene(new CCTransitionSlideInT(1f, scene));
        }

        public void onPopSceneTran(object pSender)
        {
            CCScene scene = new SceneTestScene();
            CCLayer pLayer = new SceneTestLayer2();
            scene.AddChild(pLayer, 0);

            Director.PopScene(1f, new CCTransitionSlideInB(1f, scene));
        }

        public void onQuit(object pSender) 
        {
            Director.PopToRootScene();
        }
    }
}
