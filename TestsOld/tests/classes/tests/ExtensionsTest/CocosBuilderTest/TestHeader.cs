using CocosSharp;

namespace tests.Extensions
{
    public class TestHeaderLayer : BaseLayer
    {
        public void onBackClicked(object pSender)
        {
            Scene.Director.PopScene();
        }
    }
}