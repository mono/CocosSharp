using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SpriteTestDemo : TestNavigationLayer
    {
        protected string m_strTitle;

        #region Properties

        public override string Title
        {
            get { return "No title"; }
        }

        #endregion Properties


        #region Constructors

        public SpriteTestDemo()
        { 
        }

        #endregion Constructors


        #region Callbacks

        public override void RestartCallback(object sender)
        {
            ClearCaches();

            CCScene s = new SpriteTestScene();
            s.AddChild(SpriteTestScene.RestartSpriteTestAction());
            Director.ReplaceScene(s);
        }

        public override void NextCallback(object sender)
        {
            ClearCaches();

            CCScene s = new SpriteTestScene();
            s.AddChild(SpriteTestScene.NextSpriteTestAction());
            Director.ReplaceScene(s);
        }

        public override void BackCallback(object sender)
        {
            ClearCaches();
            
            CCScene s = new SpriteTestScene();
            s.AddChild(SpriteTestScene.BackSpriteTestAction());
            Director.ReplaceScene(s);
        }

        #endregion Callbacks


        void ClearCaches()
        {

            CCSpriteFrameCache.PurgeSharedSpriteFrameCache();
            CCTextureCache.PurgeSharedTextureCache();
        }
    }
}
