using CocosSharp;

namespace tests
{
    public class ZwoptexTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = ZwoptexTest.nextZwoptexTest();
            AddChild(pLayer);

            CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(this);
        }

        protected override void NextTestCase()
        {
        }
        protected override void PreviousTestCase()
        {
        }
        protected override void RestTestCase()
        {
        }


        public static ZwoptexTestScene node()
        {
            var pRet = new ZwoptexTestScene();
            if (pRet != null)
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