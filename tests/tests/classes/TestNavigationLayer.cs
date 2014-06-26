using System;
using CocosSharp;

namespace tests
{
    // Subclass this for test cases with a back/restart/next navigation bar as well as a title/subtitle labels
    public abstract class TestNavigationLayer : CCLayer
    {
        CCMenu navigationMenu;
        CCMenuItem backMenuItem;
        CCMenuItem restartMenuItem;
        CCMenuItem nextMenuItem;


        #region Properties

        protected CCLabelTtf TitleLabel { get; private set; }
        protected CCLabelTtf SubtitleLabel { get; private set; }

        public virtual string Title
        {
            get { return ""; }
        }

        public virtual string Subtitle
        {
            get { return ""; }
        }

        #endregion Properties


        #region Constructors

        public TestNavigationLayer()
        {
            TitleLabel = new CCLabelTtf(Title, "arial", 32);
            AddChild(TitleLabel, TestScene.TITLE_LEVEL);

            string subtitleStr = Subtitle;
			if (!string.IsNullOrEmpty(subtitleStr))
            {
                SubtitleLabel = new CCLabelTtf(subtitleStr, "arial", 16);
                AddChild(SubtitleLabel, TestScene.TITLE_LEVEL);
            }

            backMenuItem = new CCMenuItemImage(TestResource.s_pPathB1, TestResource.s_pPathB2, BackCallback);
            restartMenuItem = new CCMenuItemImage(TestResource.s_pPathR1, TestResource.s_pPathR2, RestartCallback);
            nextMenuItem = new CCMenuItemImage(TestResource.s_pPathF1, TestResource.s_pPathF2, NextCallback);

            navigationMenu = new CCMenu(backMenuItem, restartMenuItem, nextMenuItem);

            AddChild(navigationMenu, TestScene.MENU_LEVEL);
        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow(windowSize);

			if (!string.IsNullOrEmpty(Title))
				TitleLabel.Text = Title;

            if(TitleLabel != null)
                TitleLabel.Position = (new CCPoint(windowSize.Width / 2, windowSize.Height - 50));

			if (!string.IsNullOrEmpty(Subtitle))
				SubtitleLabel.Text = Subtitle;

            if(SubtitleLabel != null)
                SubtitleLabel.Position = (new CCPoint(windowSize.Width / 2, windowSize.Height - 80));

            navigationMenu.Position = new CCPoint(0, 0);
            backMenuItem.Position = new CCPoint(windowSize.Width / 2 - 100, 30);
            restartMenuItem.Position = new CCPoint(windowSize.Width / 2, 30);
            nextMenuItem.Position = new CCPoint(windowSize.Width / 2 + 100, 30);
        }

        #endregion Setup content


        #region Callbacks

        public virtual void RestartCallback(object sender)
        {
        }

        public virtual void NextCallback(object sender)
        {
        }

        public virtual void BackCallback(object sender)
        {
        }

        #endregion Callbacks
    }
}

