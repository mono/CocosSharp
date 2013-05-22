using Cocos2D;

namespace tests
{
    public class SpriteMenuLayer : PerformBasicLayer
    {
        public SpriteMenuLayer(bool bControlMenuVisible, int nMaxCases, int nCurCase)
            : base(bControlMenuVisible, nMaxCases, nCurCase)
        {
        }

        public override void showCurrentTest()
        {
            var pScene = (SpriteMainScene) Parent;
            int nSubTest = pScene.getSubTestNum();
            int nNodes = pScene.getNodesNum();

            switch (m_nCurCase)
            {
                case 0:
                    pScene = new SpritePerformTest1();
                    break;
                case 1:
                    pScene = new SpritePerformTest2();
                    break;
                case 2:
                    pScene = new SpritePerformTest3();
                    break;
                case 3:
                    pScene = new SpritePerformTest4();
                    break;
                case 4:
                    pScene = new SpritePerformTest5();
                    break;
                case 5:
                    pScene = new SpritePerformTest6();
                    break;
                case 6:
                    pScene = new SpritePerformTest7();
                    break;
            }
            PerformanceSpriteTest.s_nSpriteCurCase = m_nCurCase;

            if (pScene != null)
            {
                pScene.initWithSubTest(nSubTest, nNodes);
                CCDirector.SharedDirector.ReplaceScene(pScene);
            }
        }
    }
}