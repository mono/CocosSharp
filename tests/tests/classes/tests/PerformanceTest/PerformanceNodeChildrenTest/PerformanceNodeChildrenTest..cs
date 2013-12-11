using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class PerformanceNodeChildrenTest
    {

        public static int kTagInfoLayer = 1;
        public static int kTagMainLayer = 2;
        public static int kTagLabelAtlas = 3;

        public static int kTagBase = 20000;
        public static int TEST_COUNT = 4;

        public static int kMaxNodes = 50000;
        public static int kNodesIncrease = 500;

        public static int s_nCurCase = 0;

        public static void runNodeChildrenTest()
        {
            IterateSpriteSheet pScene = new IterateSpriteSheetCArray();
            pScene.initWithQuantityOfNodes(kNodesIncrease);

            CCDirector.SharedDirector.ReplaceScene(pScene);
        }
    }
}
