using CocosSharp;

namespace tests.Extensions
{
    public class TestHeaderLayer : BaseLayer
    {
        public void onBackClicked(object pSender)
        {
            CCApplication.SharedApplication.MainWindowDirector.PopScene();
        }
    }
}