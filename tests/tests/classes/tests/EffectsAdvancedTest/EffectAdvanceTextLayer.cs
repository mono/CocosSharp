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
		protected CCNodeGrid bgNode;

        protected CCLayer contentLayer;

        protected CCSprite grossini;
        protected CCSprite tamara;

        protected CCNodeGrid Target1;
        protected CCNodeGrid Target2;

		public EffectAdvanceTextLayer()
		{
            contentLayer = new CCLayer();
		}

        protected override void AddedToScene ()
        {
            base.AddedToScene();
            Scene.AddChild(contentLayer);
        }

		public override void OnEnter()
		{
            CCRect visibleBounds = Layer.VisibleBoundsWorldspace;

            base.OnEnter();

			bgNode = new CCNodeGrid ();
			bgNode.AnchorPoint = CCPoint.AnchorMiddle;
            contentLayer.AddChild (bgNode);

			var bg = new CCSprite("Images/background3");
            bg.Position = visibleBounds.Center;

			bgNode.AddChild (bg);

            Target1 = new CCNodeGrid();
            Target1.AnchorPoint = CCPoint.AnchorMiddle;

			grossini = new CCSprite("Images/grossinis_sister2");
            Target1.AddChild(grossini);
            bgNode.AddChild (Target1);

            Target1.Position = bg.BoundingBox.Center + new CCPoint(-100.0f, 0.0f);

			var sc = new CCScaleBy(2, 5);
			var sc_back = sc.Reverse();
            Target1.RepeatForever(sc, sc_back);

            Target2 = new CCNodeGrid();
            Target2.AnchorPoint = CCPoint.AnchorMiddle;

            tamara = new CCSprite("Images/grossinis_sister1");

            Target2.AddChild(tamara);
            bgNode.AddChild(Target2);

            Target2.Position = bg.BoundingBox.Center + new CCPoint(100.0f, 0.0f);

			var sc2 = new CCScaleBy(2, 5);
			var sc2_back = sc2.Reverse();

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
