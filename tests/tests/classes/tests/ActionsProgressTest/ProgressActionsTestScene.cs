using cocos2d;

namespace tests
{
    public class ProgressActionsTestScene : TestScene
    {
        public static int sceneIdx = -1;
        public static int MAX_LAYER = 7;

        public static CCLayer createLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0:
                    return new SpriteProgressToRadial();
                case 1:
                    return new SpriteProgressToHorizontal();
                case 2:
                    return new SpriteProgressToVertical();
                case 3:
                    return new SpriteProgressToRadialMidpointChanged();
                case 4:
                    return new SpriteProgressBarVarious();
                case 5:
                    return new SpriteProgressBarTintAndFade();
                case 6:
                    return new SpriteProgressWithSpriteFrame();
            }

            return null;
        }
        protected override void NextTestCase()
        {
            nextAction();
        }
        protected override void PreviousTestCase()
        {
            backAction();
        }
        protected override void RestTestCase()
        {
            restartAction();
        }

        public static CCLayer nextAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = createLayer(sceneIdx);
            return pLayer;
        }

        public static CCLayer backAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
            {
                sceneIdx += total;
            }

            CCLayer pLayer = createLayer(sceneIdx);
            return pLayer;
        }

        public static CCLayer restartAction()
        {
            CCLayer pLayer = createLayer(sceneIdx);
            return pLayer;
        }

        public override void runThisTest()
        {
            AddChild(nextAction());
            CCDirector.SharedDirector.ReplaceScene(this);
        }
    }
}