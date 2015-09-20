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

    public class AlignmentPanel : CCDrawNode
    {

        public AlignmentPanel (CCColor4B color)
        {
            Color = new CCColor3B(color);
            Opacity = color.A;
            //AnchorPoint = CCPoint.AnchorLowerLeft;
        }

        public AlignmentPanel(CCSize size, CCColor4B color)
        {
            Color = new CCColor3B(color);
            Opacity = color.A;
            //AnchorPoint = CCPoint.AnchorMiddle;
            ContentSize = size;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            var fillColor = new CCColor4B(Color.R, Color.G, Color.B, Opacity);
            DrawRect(new CCRect(0,0,ContentSize.Width,ContentSize.Height), fillColor, 1f, CCColor4B.White);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            if (ContentSize == CCSize.Zero)
                ContentSize = VisibleBoundsWorldspace.Size;
        }
    }


    public class AtlasDemoNew : TestNavigationLayer
    {
        //protected:

        public AtlasDemoNew()
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
                return "No title";
			}
		}


		public override string Subtitle
		{
			get
			{
                return string.Empty;
			}
		}


		public override void RestartCallback(object sender)
		{
			base.RestartCallback(sender);
            CCScene s = new AtlasTestSceneNew();
            s.AddChild(AtlasTestSceneNew.restartAtlasAction());

            Director.ReplaceScene(s);
        }

		public override void NextCallback(object sender)
		{
			base.NextCallback(sender);

            CCScene s = new AtlasTestSceneNew();

            s.AddChild(AtlasTestSceneNew.nextAtlasAction());

            Director.ReplaceScene(s);
        }

		public override void BackCallback(object sender)
		{
			base.BackCallback(sender);

            CCScene s = new AtlasTestSceneNew();

            s.AddChild(AtlasTestSceneNew.backAtlasAction());

            Director.ReplaceScene(s);
        }

    }

}
