using cocos2d;

namespace tests.Extensions
{
    public class TestHeaderLayer : BaseLayer
    {
        public void onBackClicked(object pSender)
        {
            CCDirector.SharedDirector.PopScene();
        }
    }
}