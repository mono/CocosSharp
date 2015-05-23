using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class TestLayer1 : TestNavigationLayer
    {

		CCSprite bg1;
		CCLabel title;
		CCLabel label;

		public TestLayer1()
			: base()
        {
			bg1 = new CCSprite(TransitionsTestScene.s_back1);
			AddChild(bg1, -1);

            title = new CCLabel(TransitionsTestScene.transitions.Keys.ElementAt(TransitionsTestScene.s_nSceneIdx), "arial", 32, CCLabelFormat.SpriteFont);
            title.AnchorPoint = new CCPoint(0.5f, 0.5f);
			AddChild(title);
			title.Color = new CCColor3B(255, 32, 32);


			label = new CCLabel("SCENE 1", "MarkerFelt", 38, CCLabelFormat.SpriteFont);
            label.AnchorPoint = new CCPoint(0.5f, 0.5f);
			label.Color = (new CCColor3B(16, 16, 255));
			AddChild(label);

        }

        protected override void AddedToScene()
        {
            base.AddedToScene();


			Schedule(step, 1.0f);
            float x, y;

            CCSize size = Layer.VisibleBoundsWorldspace.Size;
            x = size.Width;
            y = size.Height;

			bg1.Position = size.Center;
			bg1.ScaleX = size.Width / bg1.ContentSize.Width;
			bg1.ScaleY = size.Height / bg1.ContentSize.Height;

			title.Position = new CCPoint(x / 2, y - 100);

			label.Position = size.Center;
		}

		public override void RestartCallback(object sender)
		{
			base.RestartCallback(sender);

			CCScene s = new TransitionsTestScene();
            CCLayer pLayer = new TestLayer2();
            s.AddChild(pLayer);

            CCScene pScene = TransitionsTestScene.createTransition(TransitionsTestScene.s_nSceneIdx, TransitionsTestScene.TRANSITION_DURATION, s);
            if (pScene != null)
            {
               Director.ReplaceScene(pScene);
            }
        }

		public override void NextCallback(object sender)
		{
			base.NextCallback(sender);

			TransitionsTestScene.s_nSceneIdx++;
            TransitionsTestScene.s_nSceneIdx = TransitionsTestScene.s_nSceneIdx % TransitionsTestScene.MAX_LAYER;

            CCScene s = new TransitionsTestScene();
            CCLayer pLayer = new TestLayer2();
            s.AddChild(pLayer);

            CCScene pScene = TransitionsTestScene.createTransition(TransitionsTestScene.s_nSceneIdx, TransitionsTestScene.TRANSITION_DURATION, s);
            if (pScene != null)
            {
                Director.ReplaceScene(pScene);
            }
        }

		public override void BackCallback(object sender)
		{
			base.BackCallback(sender);

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
               Director.ReplaceScene(pScene);
            }
        }

		public override void OnEnter()
		{
			base.OnEnter();
			CCLog.Log("Scene 1: onEnter");
		}

		public override void OnEnterTransitionDidFinish()
		{
			base.OnEnterTransitionDidFinish();
			CCLog.Log("Scene 1: onEnterTransitionDidFinish");
		}

		public override void OnExitTransitionDidStart()
		{
			base.OnExitTransitionDidStart();
			CCLog.Log("Scene 1: onExitTransitionDidStart");
		}


		public override void OnExit()
		{
			base.OnExit();
			CCLog.Log("Scene 1: onExit");
		}

        public void step(float dt)
        {

        }
    }
}
