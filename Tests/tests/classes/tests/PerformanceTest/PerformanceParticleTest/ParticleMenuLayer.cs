using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class ParticleMenuLayer : PerformBasicLayer
    {
        public ParticleMenuLayer(bool bControlMenuVisible, int nMaxCases, int nCurCase)
            : base(bControlMenuVisible, nMaxCases, nCurCase)
        {
        }

        public override void showCurrentTest()
        {
            var pScene = (ParticleMainScene)Parent;
            int subTest = pScene.getSubTestNum();
            int parNum = pScene.getParticlesNum();

            ParticleMainScene pNewScene = null;

            switch (m_nCurCase)
            {
                case 0:
                    pNewScene = new ParticlePerformTest1();
                    break;
                case 1:
                    pNewScene = new ParticlePerformTest2();
                    break;
                case 2:
                    pNewScene = new ParticlePerformTest3();
                    break;
                case 3:
                    pNewScene = new ParticlePerformTest4();
                    break;
            }

            PerformanceParticleTest.s_nParCurIdx = m_nCurCase;
            if (pNewScene != null)
            {
                pNewScene.initWithSubTest(subTest, parNum);

                Scene.Director.ReplaceScene(pNewScene);
            }
        }
    }
}
