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
            CCMenuItemFont item1 = new CCMenuItemFont("(1) Test PushScene", onPushScene);
            CCMenuItemFont item2 = new CCMenuItemFont("(1) Test PushScene w/Transition", onPushSceneTran);
            CCMenuItemFont item3 = new CCMenuItemFont("(1) Quit", onQuit);
            popMenuItem = new CCMenuItemFont("(1) Test PopScene w/Transition", onPopSceneTran);

            theMenu = new CCMenu(item1, item2, item3, popMenuItem);
            theMenu.AlignItemsVertically();

            AddChild(theMenu);

			CCSprite sprite = new CCSprite(SceneTestScene.grossini) { Tag = SceneTestScene.GROSSINI_TAG };
            AddChild(sprite);
            
        }

		public override void OnEnter()
		{
			base.OnEnter(); CCSize windowSize = Scene.VisibleBoundsWorldspace.Size;

            CCLog.Log("SceneTestLayer1#OnEnter - Can Pop Scene = {0} - {1}", Director.CanPopScene, Director.SceneCount);
            popMenuItem.Visible = Director.CanPopScene;
            theMenu.AlignItemsVertically(12f);

			CCLog.Log("SceneTestLayer1#RunningOnNewWindow - Can Pop Scene = {0} - {1}", Director.CanPopScene, Director.SceneCount);

			var sprite = this[SceneTestScene.GROSSINI_TAG];
			var s = windowSize;
			sprite.Position = new CCPoint(s.Width - 40, s.Height / 2);
			sprite.RepeatForever(SceneTestScene.rotate);

		}

		public override void OnExit()
		{
			base.OnExit();
			CCLog.Log("SceneTestLayer1#OnExit - Can Pop Scene = {0} - {1}", Director.CanPopScene, Director.SceneCount);
		}


        public override void OnEnterTransitionDidFinish()
        {
            CCLog.Log("SceneTestLayer1#OnEnterTransitionDidFinish");
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
