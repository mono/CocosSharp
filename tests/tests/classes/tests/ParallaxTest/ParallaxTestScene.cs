using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class ParallaxTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = nextParallaxAction();

            AddChild(pLayer);
            CCDirector.SharedDirector.ReplaceScene(this);
        }

        static int sceneIdx = -1;
        static int MAX_LAYER = 2;

        protected override void NextTestCase()
        {
            nextParallaxAction();
        }
        protected override void PreviousTestCase()
        {
            backParallaxAction();
        }
        protected override void RestTestCase()
        {
            restartParallaxAction();
        }


        public static CCLayer nextParallaxAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = createParallaxTestLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer createParallaxTestLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0: return new Parallax1();
                case 1: return new Parallax2();
            }

            return null;
        }

        public static CCLayer backParallaxAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = createParallaxTestLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer restartParallaxAction()
        {
            CCLayer pLayer = createParallaxTestLayer(sceneIdx);

            return pLayer;
        } 
    }
}
