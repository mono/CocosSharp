using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SceneTestLayer2 : CCLayer
    {
        float m_timeCounter;
		private CCMenuItemFont popMenuItem;
		private CCMenu theMenu;

        public SceneTestLayer2()
        {
            m_timeCounter = 0;

            CCMenuItemFont item1 = new CCMenuItemFont("(2) replaceScene", onReplaceScene);
            CCMenuItemFont item2 = new CCMenuItemFont("(2) replaceScene w/transition", onReplaceSceneTran);
            CCMenuItemFont item3 = new CCMenuItemFont("(2) Go Back", onGoBack);
            popMenuItem = new CCMenuItemFont("(2) Test popScene w/transition", onPopSceneTran);

			theMenu = new CCMenu(item1, item2, item3, popMenuItem);
            theMenu.AlignItemsVertically();

            AddChild(theMenu);

			CCSprite sprite = new CCSprite(SceneTestScene.grossini) { Tag = SceneTestScene.GROSSINI_TAG };
            AddChild(sprite);
        }


		public override void OnEnter()
		{
			base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CCLog.Log("SceneTestLayer2#OnEnter - Can Pop Scene = {0} - {1}", Director.CanPopScene, Director.SceneCount);
            popMenuItem.Visible = Director.CanPopScene;
            theMenu.AlignItemsVertically(12f);

			var sprite = this[SceneTestScene.GROSSINI_TAG];
			var s = windowSize;
			sprite.Position = new CCPoint(s.Width - 40, s.Height / 2);
			sprite.RepeatForever(SceneTestScene.rotate);

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
            Director.PopScene();
        }

		public override void OnExit()
		{
			base.OnExit();
			CCLog.Log("SceneTestLayer2#OnExit - Can Pop Scene = {0} - {1}", Director.CanPopScene, Director.SceneCount);
		}


        public void onReplaceScene(object pSender)
        {
            CCScene pScene = new SceneTestScene();
            CCLayer pLayer = new SceneTestLayer3();
            pScene.AddChild(pLayer, 0);
           	Director.ReplaceScene(pScene);
        }

        public void onReplaceSceneTran(object pSender)
        {
            CCScene pScene = new SceneTestScene();
            CCLayer pLayer = new SceneTestLayer3();
            pScene.AddChild(pLayer, 0);
            Director.ReplaceScene(new CCTransitionFlipX(2, pScene, CCTransitionOrientation.UpOver));
        }

        public void onPopSceneTran(object pSender)
        {
            CCScene scene = new SceneTestScene();
            CCLayer pLayer = new SceneTestLayer1();
            scene.AddChild(pLayer, 0);

            Director.PopScene(1f, new CCTransitionSlideInB(1f, scene));
        }
    }
}
