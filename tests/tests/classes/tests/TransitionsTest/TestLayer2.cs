using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class TestLayer2 : TestNavigationLayer
    {

		CCSprite bg2;
		CCLabelTtf title;
		CCLabelTtf label;

		public TestLayer2()
        {

			bg2 = new CCSprite(TransitionsTestScene.s_back2);
			AddChild(bg2, -1);

			title = new CCLabelTtf((TransitionsTestScene.transitions[TransitionsTestScene.s_nSceneIdx]), "arial", 32);
			AddChild(title);
			title.Color = new CCColor3B(255, 32, 32);


			label = new CCLabelTtf("SCENE 2", "MarkerFelt", 38);
			label.Color = (new CCColor3B(16, 16, 255));
			AddChild(label);

        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            Schedule(step, 1.0f);

			float x, y;

            var size = Layer.VisibleBoundsWorldspace.Size;
			x = size.Width;
			y = size.Height;

			bg2.Position = size.Center;
			bg2.ScaleX = size.Width / bg2.ContentSize.Width;
			bg2.ScaleY = size.Height / bg2.ContentSize.Height;

			title.Position = new CCPoint(x / 2, y - 100);

			label.Position = size.Center;
		}

		public override void RestartCallback(object sender)
		{
            CCScene s = new TransitionsTestScene();

            CCLayer pLayer = new TestLayer1();
            s.AddChild(pLayer);

            CCScene pScene = TransitionsTestScene.createTransition(TransitionsTestScene.s_nSceneIdx, TransitionsTestScene.TRANSITION_DURATION, s);

            if (pScene != null)
            {
                Director.ReplaceScene(pScene);
            }
        }

		public override void NextCallback(object sender)
		{
            TransitionsTestScene.s_nSceneIdx++;
            TransitionsTestScene.s_nSceneIdx = TransitionsTestScene.s_nSceneIdx % TransitionsTestScene.MAX_LAYER;

            CCScene s = new TransitionsTestScene();

            CCLayer pLayer = new TestLayer1();
            s.AddChild(pLayer);
            CCScene pScene = TransitionsTestScene.createTransition(TransitionsTestScene.s_nSceneIdx, TransitionsTestScene.TRANSITION_DURATION, s);

            if (pScene != null)
            {
                Director.ReplaceScene(pScene);
            }
        }

		public override void BackCallback(object sender)
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
                Director.ReplaceScene(pScene);
            }
        }

		public override void OnEnter()
		{
			base.OnEnter();
			CCLog.Log("Scene 2: onEnter");
		}

		public override void OnEnterTransitionDidFinish()
		{
			base.OnEnterTransitionDidFinish();
			CCLog.Log("Scene 2: onEnterTransitionDidFinish");
		}

		public override void OnExitTransitionDidStart()
		{
			base.OnExitTransitionDidStart();
			CCLog.Log("Scene 2: onExitTransitionDidStart");
		}


		public override void OnExit()
		{
			base.OnExit();
			CCLog.Log("Scene 2: onExit");
		}

        public void step(float dt) { }
    }
}
