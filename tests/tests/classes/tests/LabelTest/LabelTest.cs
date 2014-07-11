using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using tests;
using System.Diagnostics;

namespace CocosSharp
{
    public enum TagSprite
    {
        kTagTileMap = 1,
        kTagSpriteManager = 1,
        kTagAnimation1 = 1,
        kTagBitmapAtlas1 = 1,
        kTagBitmapAtlas2 = 2,
        kTagBitmapAtlas3 = 3,

        kTagSprite1,
        kTagSprite2,
        kTagSprite3,
        kTagSprite4,
        kTagSprite5,
        kTagSprite6,
        kTagSprite7,
        kTagSprite8
    }

    public class AtlasDemo : TestNavigationLayer
    {
        //protected:

        public AtlasDemo()
        {

        }

        public enum LabelTestConstant
        {
            IDC_NEXT = 100,
            IDC_BACK,
            IDC_RESTART
        }


		public override string Title
		{
			get
			{
				return title();
			}
		}

        public virtual string title()
        {
            return "No title";
        }

		public override string Subtitle
		{
			get
			{
				return subtitle();
			}
		}

        public virtual string subtitle()
        {
            return "";
        }

		public override void RestartCallback(object sender)
		{
			base.RestartCallback(sender);
            CCScene s = new AtlasTestScene();
            s.AddChild(AtlasTestScene.restartAtlasAction());

            Director.ReplaceScene(s);
        }

		public override void NextCallback(object sender)
		{
			base.NextCallback(sender);

            CCScene s = new AtlasTestScene();

            s.AddChild(AtlasTestScene.nextAtlasAction());

            Director.ReplaceScene(s);

        }

		public override void BackCallback(object sender)
		{
			base.BackCallback(sender);

            CCScene s = new AtlasTestScene();

            s.AddChild(AtlasTestScene.backAtlasAction());

            Director.ReplaceScene(s);

        }

    }

}
