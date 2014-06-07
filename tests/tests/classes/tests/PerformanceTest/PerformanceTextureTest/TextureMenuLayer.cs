using System;
using CocosSharp;

namespace tests
{
    public class TextureMenuLayer : PerformBasicLayer
    {
        public TextureMenuLayer(bool bControlMenuVisible, int nMaxCases, int nCurCase)
            : base(bControlMenuVisible, nMaxCases, nCurCase)
        {
        }

        public override void showCurrentTest()
        {
            CCScene pScene = null;

            switch (m_nCurCase)
            {
                case 0:
                    pScene = TextureTest.scene();
                    break;
            }
            PerformanceTextureTest.s_nTexCurCase = m_nCurCase;

            if (pScene != null)
            {
                CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(pScene);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCApplication.SharedApplication.MainWindowDirector.WindowSizeInPoints;

            // Title
            CCLabelTtf label = new CCLabelTtf(title(), "arial", 38);
            AddChild(label, 1);
            label.Position = new CCPoint(s.Width / 2, s.Height - 32);
            label.Color = new CCColor3B(255, 255, 40);

            // Subtitle
            string strSubTitle = subtitle();
            if (strSubTitle.Length > 0)
            {
                CCLabelTtf l = new CCLabelTtf(strSubTitle, "Thonburi", 16);
                AddChild(l, 1);
                l.Position = new CCPoint(s.Width / 2, s.Height - 80);
            }

            performTests();
        }

        public virtual string title()
        {
            return "no title";
        }

        public virtual string subtitle()
        {
            return "no subtitle";
        }

        public virtual void performTests()
        {
            throw new NotFiniteNumberException();
        }
    }
}