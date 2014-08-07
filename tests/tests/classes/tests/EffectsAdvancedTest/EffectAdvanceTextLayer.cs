using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{

	public class EffectAdvanceTextLayer : TestNavigationLayer
	{
		protected CCTextureAtlas m_atlas;
		protected string m_strTitle;
		protected CCNode bgNode;

        protected CCLayer contentLayer;

		public EffectAdvanceTextLayer()
		{
            contentLayer = new CCLayer();
		}

		protected CCSprite grossini;
		protected CCSprite tamara;

        protected override void AddedToScene ()
        {
            base.AddedToScene();
            Scene.AddChild(contentLayer);
        }

		public override void OnEnter()
		{
            CCRect visibleBounds = Layer.VisibleBoundsWorldspace;

            base.OnEnter();

			bgNode = new CCNode ();
			bgNode.AnchorPoint = CCPoint.AnchorMiddle;
            contentLayer.AddChild (bgNode);
            bgNode.Position = visibleBounds.Center;

			var bg = new CCSprite("Images/background3");
            bg.AnchorPoint = CCPoint.Zero;
            bg.Position = CCPoint.Zero;

            bgNode.ContentSize = bg.ContentSize;

			bgNode.AddChild (bg, 0, EffectAdvanceScene.kTagBackground);

			grossini = new CCSprite("Images/grossinis_sister2");

            bgNode.AddChild (grossini);

            grossini.Position = bg.BoundingBox.Center + new CCPoint(-100.0f, 0.0f);

			var sc = new CCScaleBy(2, 5);
			var sc_back = sc.Reverse();

			tamara = new CCSprite("Images/grossinis_sister1");

            bgNode.AddChild (tamara);
            tamara.Position = bg.BoundingBox.Center + new CCPoint(100.0f, 0.0f);

			var sc2 = new CCScaleBy(2, 5);
			var sc2_back = sc2.Reverse();

            grossini.RepeatForever (sc, sc_back);
			tamara.RepeatForever(sc2, sc2_back);

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
			var s = new EffectAdvanceScene();
			s.AddChild(EffectAdvanceScene.restartEffectAdvanceAction());

			Director.ReplaceScene(s);
		}

		public override void NextCallback(object sender)
		{
			var s = new EffectAdvanceScene();
			s.AddChild(EffectAdvanceScene.nextEffectAdvanceAction());
			Director.ReplaceScene(s);
		}

		public override void BackCallback(object sender)
		{
			var s = new EffectAdvanceScene();
			s.AddChild(EffectAdvanceScene.backEffectAdvanceAction());
			Director.ReplaceScene(s);
		}
	}

}
