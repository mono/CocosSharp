using CocosSharp;

namespace tests
{
    public class RenderTextureTestDemo : TestNavigationLayer
    {
        #region Properties

        public override string Title
        {
            get { return "Render Texture Test"; }
        }

        #endregion Properties


        #region Callbacks

        public override void RestartCallback(object sender)
        {
            CCScene s = new RenderTextureScene();
            s.AddChild(RenderTextureScene.restartTestCase());
            Director.ReplaceScene(s);
        }

        public override void NextCallback(object sender)
        {
            CCScene s = new RenderTextureScene();
            s.AddChild(RenderTextureScene.nextTestCase());
            Director.ReplaceScene(s);
        }

        public override void BackCallback(object sender)
        {
            CCScene s = new RenderTextureScene();
            s.AddChild(RenderTextureScene.backTestCase());
            Director.ReplaceScene(s);
        }

        #endregion Callbacks
    }
}