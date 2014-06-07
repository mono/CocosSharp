using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class NodeChildrenMenuLayer : PerformBasicLayer
    {
        public NodeChildrenMenuLayer(bool bControlMenuVisible, int nMaxCases, int nCurCase)
            :base(bControlMenuVisible, nMaxCases, nCurCase)
        {
        }

        public override void showCurrentTest()
        {
            int nNodes = ((NodeChildrenMainScene)Parent).getQuantityOfNodes();
            NodeChildrenMainScene pScene = null;

            switch (PerformBasicLayer.m_nCurCase)
            {
                //     case 0:
                //         pScene = new IterateSpriteSheetFastEnum();
                //         break;
                case 0:
                    pScene = new IterateSpriteSheetCArray();
                    break;
                case 1:
                    pScene = new AddSpriteSheet();
                    break;
                case 2:
                    pScene = new RemoveSpriteSheet();
                    break;
                case 3:
                    pScene = new ReorderSpriteSheet();
                    break;
            }

            PerformanceNodeChildrenTest.s_nCurCase = PerformBasicLayer.m_nCurCase;

            if (pScene != null)
            {
                pScene.initWithQuantityOfNodes(nNodes);

                CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(pScene);
            }
        }
    }
}
