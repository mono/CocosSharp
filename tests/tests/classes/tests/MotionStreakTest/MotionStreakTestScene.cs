using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class MotionStreakTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = nextMotionAction();
            AddChild(pLayer);

            Scene.Director.ReplaceScene(this);
        }

        static int sceneIdx = -1;
        static int MAX_LAYER = 2;

        public static CCLayer createMotionLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0: return new MotionStreakTest1();
                case 1: return new MotionStreakTest2();
            }

            return null;
        }
        protected override void NextTestCase()
        {
            nextMotionAction();
        }
        protected override void PreviousTestCase()
        {
            backMotionAction();
        }
        protected override void RestTestCase()
        {
            restartMotionAction();
        }
        public static CCLayer nextMotionAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = createMotionLayer(sceneIdx);
            return pLayer;
        }

        public static CCLayer backMotionAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = createMotionLayer(sceneIdx);
            return pLayer;
        }

        public static CCLayer restartMotionAction()
        {
            CCLayer pLayer = createMotionLayer(sceneIdx);
            return pLayer;
        }
    }
}
