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
            TitleLabel = new CCLabelTtf(Title, "arial", 60);
            TitleLabel.AnchorPoint = new CCPoint(0.5f, 0.5f);
            AddChild(TitleLabel, TestScene.TITLE_LEVEL);

            string subtitleStr = Subtitle;
			if (!string.IsNullOrEmpty(subtitleStr))
            {
                SubtitleLabel = new CCLabelTtf(subtitleStr, "arial", 30);
                SubtitleLabel.AnchorPoint = new CCPoint(0.5f, 0.5f);
                AddChild(SubtitleLabel, TestScene.TITLE_LEVEL);
            }

            backMenuItem = new CCMenuItemImage(TestResource.s_pPathB1, TestResource.s_pPathB2, BackCallback);
            restartMenuItem = new CCMenuItemImage(TestResource.s_pPathR1, TestResource.s_pPathR2, RestartCallback);
            nextMenuItem = new CCMenuItemImage(TestResource.s_pPathF1, TestResource.s_pPathF2, NextCallback);

            navigationMenu = new CCMenu(backMenuItem, restartMenuItem, nextMenuItem);

            AddChild(navigationMenu, TestScene.MENU_LEVEL);
            Camera = AppDelegate.SharedCamera;
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

			if (!string.IsNullOrEmpty(Title))
				TitleLabel.Text = Title;

            if(TitleLabel != null)
                TitleLabel.Position = (new CCPoint(windowSize.Width / 2, windowSize.Height - 50));

			if (!string.IsNullOrEmpty(Subtitle))
				SubtitleLabel.Text = Subtitle;

            if(SubtitleLabel != null)
                SubtitleLabel.Position = (new CCPoint(windowSize.Width / 2, windowSize.Height - 100));


            float padding = 10.0f;
            float halfRestartWidth = restartMenuItem.ContentSize.Width / 2.0f;

            navigationMenu.Position = new CCPoint(0, 0);
            backMenuItem.Position = (new CCPoint(windowSize.Width / 2 - backMenuItem.ContentSize.Width / 2.0f - halfRestartWidth - padding, 
                padding + halfRestartWidth));
            restartMenuItem.Position = (new CCPoint(windowSize.Width / 2, padding + halfRestartWidth));
            nextMenuItem.Position = (new CCPoint(windowSize.Width / 2 + nextMenuItem.ContentSize.Width / 2.0f + halfRestartWidth + padding, 
                padding + halfRestartWidth));
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

