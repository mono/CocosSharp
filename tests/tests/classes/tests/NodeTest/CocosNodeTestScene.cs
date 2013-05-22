using Cocos2D;

namespace tests
{
    public class CocosNodeTestScene : TestScene
    {
        private static int sceneIdx = -1;

        private static int MAX_LAYER = 12;

        public static CCLayer createCocosNodeLayer(int nIndex)
        {
            switch (nIndex)
            {
            case 0:
                return new Test2();
            case 1:
                return new Test4();
            case 2:
                return new Test5();
            case 3:
                return new Test6();
            case 4:
                return new StressTest1();
            case 5:
                return new StressTest2();
            case 6:
                return new NodeToWorld();
            case 7:
                return new SchedulerTest1();
            case 8:
                return new CameraOrbitTest();
            case 9:
                return new CameraZoomTest();
            case 10:
                return new CameraCenterTest();
            case 11:
                return new ConvertToNode();            
            }

            return null;
        }

        protected override void NextTestCase()
        {
            nextCocosNodeAction();
        }
        protected override void PreviousTestCase()
        {
            backCocosNodeAction();
        }
        protected override void RestTestCase()
        {
            restartCocosNodeAction();
        }
        public static CCLayer nextCocosNodeAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = createCocosNodeLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer backCocosNodeAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = createCocosNodeLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer restartCocosNodeAction()
        {
            CCLayer pLayer = createCocosNodeLayer(sceneIdx);

            return pLayer;
        }

        public override void runThisTest()
        {
            CCLayer pLayer = nextCocosNodeAction();
            AddChild(pLayer);

            CCDirector.SharedDirector.ReplaceScene(this);
        }
    }
}