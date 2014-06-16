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
		protected CCNode target1;
		protected CCNode target2;

		public EffectAdvanceTextLayer()
		{

		}

		protected CCSprite grossini;
		protected CCSprite tamara;

		public override void OnEnter()
		{
			base.OnEnter();

			bgNode = new CCNode ();
			bgNode.AnchorPoint = CCPoint.AnchorMiddle;
			AddChild (bgNode);

			//_bgNode.Position = CCVisibleRect.Center;

			var bg = new CCSprite("Images/background3");
			bg.Position = CCVisibleRect.Center;
			//AddChild(bg, 0, EffectAdvanceScene.kTagBackground);
			bgNode.AddChild (bg, 0, EffectAdvanceScene.kTagBackground);

			target1 = new CCNode ();
			target1.AnchorPoint = CCPoint.AnchorMiddle;
			grossini = new CCSprite("Images/grossinis_sister2");

			target1.AddChild (grossini);
			bgNode.AddChild (target1);

			grossini.Position = new CCPoint(CCVisibleRect.Left.X + CCVisibleRect.VisibleRect.Size.Width / 3, CCVisibleRect.Bottom.Y + 200);

			var sc = new CCScaleBy(2, 5);
			var sc_back = sc.Reverse();
			grossini.RepeatForever (sc, sc_back);

			target2 = new CCNode ();
			target2.AnchorPoint = CCPoint.AnchorMiddle;

			tamara = new CCSprite("Images/grossinis_sister1");

			target2.AddChild (tamara);
			bgNode.AddChild (target2);
			tamara.Position = new CCPoint (CCVisibleRect.Left.X + 2 * CCVisibleRect.VisibleRect.Size.Width / 3, CCVisibleRect.Bottom.Y + 200);

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

			CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(s);
		}

		public override void NextCallback(object sender)
		{
			var s = new EffectAdvanceScene();
			s.AddChild(EffectAdvanceScene.nextEffectAdvanceAction());
			CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(s);
		}

		public override void BackCallback(object sender)
		{
			var s = new EffectAdvanceScene();
			s.AddChild(EffectAdvanceScene.backEffectAdvanceAction());
			CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(s);
		}
	}

}
