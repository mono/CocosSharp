using CocosSharp;

namespace tests
{

	public class SpriteDemo : TestNavigationLayer
	{

		public override string Title
		{
			get
			{
				return "ProgressActionsTest";
			}
		}

		public override void OnEnter()
		{
			base.OnEnter();
            CCLayerColor background = new CCLayerColor(new CCColor4B(255,0,0,255));
            AddChild(background, -10);
		}

		public override void RestartCallback(object sender)
		{
			CCScene s = new ProgressActionsTestScene();
			s.AddChild(ProgressActionsTestScene.restartAction());

			Director.ReplaceScene(s);
		}

		public override void NextCallback(object sender)
		{
			CCScene s = new ProgressActionsTestScene();
			s.AddChild(ProgressActionsTestScene.nextAction());
			Director.ReplaceScene(s);
		}

		public override void BackCallback(object sender)
		{
			CCScene s = new ProgressActionsTestScene();
			s.AddChild(ProgressActionsTestScene.backAction());
			Director.ReplaceScene(s);
		}
	}
}