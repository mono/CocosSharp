using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class PerformBasicLayer : CCLayer
    {
        public PerformBasicLayer(bool bControlMenuVisible, int nMaxCases, int nCurCase)
        {
            m_bControlMenuVisible = bControlMenuVisible;
            m_nMaxCases = nMaxCases;
            m_nCurCase = nCurCase;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;

            CCMenuItemFont.FontName = "arial";
            CCMenuItemFont.FontSize = 24;
            var pMainItem = CCMenuItemFont.Create("Back", toMainLayer);
            pMainItem.Position = new CCPoint(s.Width - 50, 25);
            var pMenu = CCMenu.Create(pMainItem);
            pMenu.Position = new CCPoint(0, 0);

            if (m_bControlMenuVisible)
            {
                var item1 = new CCMenuItemImage("Images/b1", "Images/b2", backCallback);
                var item2 = new CCMenuItemImage("Images/r1", "Images/r2", restartCallback);
                var item3 = new CCMenuItemImage("Images/f1", "Images/f2", nextCallback);
                item1.Position = new CCPoint(s.Width / 2 - 100, 30);
                item2.Position = new CCPoint(s.Width / 2, 30);
                item3.Position = new CCPoint(s.Width / 2 + 100, 30);

                pMenu.AddChild(item1, PerformanceTestScene.kItemTagBasic);
                pMenu.AddChild(item2, PerformanceTestScene.kItemTagBasic);
                pMenu.AddChild(item3, PerformanceTestScene.kItemTagBasic);
            }
            AddChild(pMenu);
        }

        public virtual void restartCallback(object pSender)
        {
            showCurrentTest();
        }

        public virtual void nextCallback(object pSender)
        {
            m_nCurCase++;
            m_nCurCase = m_nCurCase % m_nMaxCases;

            showCurrentTest();
        }

        public virtual void backCallback(object pSender)
        {
            m_nCurCase--;
            if (m_nCurCase < 0)
                m_nCurCase += m_nMaxCases;

            showCurrentTest();
        }

        public virtual void showCurrentTest()
        {
            throw new NotFiniteNumberException();
        }

        public virtual void toMainLayer(object pSender)
        {
            var pScene = new PerformanceTestScene();
            pScene.runThisTest();
        }

        protected bool m_bControlMenuVisible;
        protected int m_nMaxCases;
        public static int m_nCurCase;
        protected int nMaxCases;
        protected int nCurCase;
    }
}
