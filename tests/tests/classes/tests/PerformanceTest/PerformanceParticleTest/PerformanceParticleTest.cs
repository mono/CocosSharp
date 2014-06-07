using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class PerformanceParticleTest
    {
        public static int kTagInfoLayer = 1;

        public static int kTagMainLayer = 2;
        public static int kTagParticleSystem = 3;
        public static int kTagLabelAtlas = 4;
        public static int kTagMenuLayer = 1000;

        public static int TEST_COUNT = 4;

        public static int kMaxParticles = 20000;
        public static int kNodesIncrease = 500;

        public static int s_nParCurIdx = 0;

        public static void runParticleTest()
        {
            ParticleMainScene pScene = new ParticlePerformTest1();
            pScene.initWithSubTest(1, kNodesIncrease);

            CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(pScene);
        }
    }
}
