using CocosSharp;

namespace tests
{
    public class TestCocosNodeDemo : TestNavigationLayer
    {

		public override string Title
		{
			get
			{
				return title();
			}
		}

		public override string Subtitle
		{
			get
			{
				return subtitle();
			}
		}
        public virtual string title()
        {
            return "No title";
        }

        public virtual string subtitle()
        {
            return "";
        }

		public override void RestartCallback(object sender)
		{
			base.RestartCallback(sender);
            CCScene s = new CocosNodeTestScene(); //CCScene.node();
            s.AddChild(CocosNodeTestScene.restartCocosNodeAction());

            Director.ReplaceScene(s);
        }

		public override void NextCallback(object sender)
		{
			base.NextCallback(sender);
            CCScene s = new CocosNodeTestScene(); //CCScene.node();
            s.AddChild(CocosNodeTestScene.nextCocosNodeAction());
            Director.ReplaceScene(s);
        }

		public override void BackCallback(object sender)
		{
			base.BackCallback(sender);
            CCScene s = new CocosNodeTestScene(); //CCScene.node();
            s.AddChild(CocosNodeTestScene.backCocosNodeAction());
            Director.ReplaceScene(s);
        }
    }
}