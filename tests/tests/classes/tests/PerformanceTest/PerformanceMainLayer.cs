using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class PerformanceMainLayer : CCLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCApplication.SharedApplication.MainWindowDirector.WinSize;

            CCMenu pMenu = new CCMenu(null);
            pMenu.Position = new CCPoint(0, 0);

			CCMenuItemFont.FontSize = 18;
			CCMenuItemFont.FontName = "arial";

            for (int i = 0; i < PerformanceTestScene.MAX_COUNT; ++i)
            {
                CCMenuItemFont pItem = new CCMenuItemFont(PerformanceTestScene.testsName[i], menuCallback);
                pItem.Position = new CCPoint(s.Width / 2, s.Height - (i + 1) * PerformanceTestScene.LINE_SPACE);
                pMenu.AddChild(pItem, PerformanceTestScene.kItemTagBasic + i);
            }

            AddChild(pMenu);
        }

        public void menuCallback(object pSender)
        {
            CCMenuItemFont pItem = (CCMenuItemFont)pSender;
            int nIndex = pItem.ZOrder - PerformanceTestScene.kItemTagBasic;

            switch (nIndex)
            {
                case 0:
                    PerformanceNodeChildrenTest.runNodeChildrenTest();
                    break;
                case 1:
                    PerformanceParticleTest.runParticleTest();
                    break;
                case 2:
                    PerformanceSpriteTest.runSpriteTest();
                    break;
                case 3:
                    PerformanceTextureTest.runTextureTest();
                    break;
                case 4:
                    PerformanceTouchesTest.runTouchesTest();
                    break;
                default:
                    break;
            }
        }
    }
}
