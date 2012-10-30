using cocos2d;

namespace tests.Extensions
{
    public class TestHeaderLayer : BaseLayer
    {
        public void onBackClicked(CCObject pSender)
        {
            CCDirector.SharedDirector.PopScene();
        }
    }
}