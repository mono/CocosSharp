using cocos2d;

namespace tests
{
    public class ZwoptexTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = ZwoptexTest.nextZwoptexTest();
            AddChild(pLayer);

            CCDirector.SharedDirector.ReplaceScene(this);
        }

        public static ZwoptexTestScene node()
        {
            var pRet = new ZwoptexTestScene();
            if (pRet != null && pRet.Init())
            {
                return pRet;
            }
            else
            {
                pRet = null;
                return null;
            }
        }
    }
}