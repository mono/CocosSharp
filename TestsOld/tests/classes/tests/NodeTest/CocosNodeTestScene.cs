using CocosSharp;

namespace tests
{
    public class CocosNodeTestScene : TestScene
    {
        private static int sceneIdx = -1;

		private static int MAX_LAYER = 14;

        public static CCLayer createCocosNodeLayer(int nIndex)
        {
            switch (nIndex)
            {
			case 0:
				return new CameraTest1 ();
			case 1: 
				return new CameraCenterTest ();
			case 2:
				return new Test2 ();
			case 3:
                return new Test4();
			case 4:
                return new Test5();
			case 5:
                return new Test6();
			case 6:
                return new StressTest1();
			case 7:
                return new StressTest2();
			case 8:
                return new NodeToWorld();
			case 9:
				return new NodeToWorld3D();
			case 10:
                return new SchedulerTest1();
			case 11:
                return new CameraOrbitTest();
			case 12:
                return new CameraZoomTest();
			case 13:
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

			var pLayer = createCocosNodeLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer backCocosNodeAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

			var pLayer = createCocosNodeLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer restartCocosNodeAction()
        {
			var pLayer = createCocosNodeLayer(sceneIdx);

            return pLayer;
        }

        public override void runThisTest()
        {
			var pLayer = nextCocosNodeAction();
            AddChild(pLayer);

            Director.ReplaceScene(this);
        }
    }
}