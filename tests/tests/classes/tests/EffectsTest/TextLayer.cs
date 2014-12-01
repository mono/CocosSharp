using CocosSharp;
using System;

namespace tests
{

	public class TextLayer : TestNavigationLayer
	{
		private static int MAX_LAYER = 22;
		public static CCNode BaseNode;

		public TextLayer() : base()
		{
            var backGround = new CCLayerColor(new CCColor4B(32, 128, 32, 255));
            AddChild(backGround, -20);

            BaseNode = new CCNode();
            var bg = new CCSprite(TestResource.s_back3);
            BaseNode.ContentSize = bg.ContentSize;
            BaseNode.AddChild(bg, 0, EffectTestScene.kTagBackground);

            var Kathia = new CCSprite(TestResource.s_pPathSister2);
            BaseNode.AddChild(Kathia, 1, EffectTestScene.kTagKathia);

            var sc = new CCScaleBy(2, 5);
            var sc_back = sc.Reverse();
            Kathia.RunAction(new CCRepeatForever(sc, sc_back));


            var Tamara = new CCSprite(TestResource.s_pPathSister1);
            BaseNode.AddChild(Tamara, 1, EffectTestScene.kTagTamara);

            var sc2 = new CCScaleBy(2, 5);
            var sc2_back = sc2.Reverse();
            Tamara.RunAction(new CCRepeatForever(sc2, sc2_back));

            AddChild(BaseNode);
		}

		public override void OnEnter()
		{
			base.OnEnter(); 

            CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CCRect visibleBounds = Layer.VisibleBoundsWorldspace;

            BaseNode.Position = windowSize.Center;
            var size = BaseNode.ContentSize;

			BaseNode[EffectTestScene.kTagKathia].PositionX = -size.Width / 3;
			BaseNode[EffectTestScene.kTagTamara].PositionX = size.Width / 3;

            BaseNode.RunAction(CurrentAction);

            Schedule(checkAnim);

		}

		public override string Title
		{
			get
			{
				return EffectTestScene.effectsList[EffectTestScene.actionIdx];
			}
		}

		public CCFiniteTimeAction createEffect(int nIndex, float t)
		{
			// This fixes issue https://github.com/totallyevil/cocos2d-xna/issues/148
			// TransitionTests and TileTests may have set the DepthTest to true so we need
			// to make sure we reset it.

			switch (nIndex)
			{
				case 0:
                    return new Shaky3DDemo(t);
				case 1:
					return new Waves3DDemo(t);
				case 2:
					return FlipX3DDemo.ActionWithDuration(t);
				case 3:
					return FlipY3DDemo.ActionWithDuration(t);
				case 4:
					return new Lens3DDemo(t);
				case 5:
					return new Ripple3DDemo(t);
				case 6:
					return new LiquidDemo(t);
				case 7:
					return new WavesDemo(t);
				case 8:
					return new TwirlDemo(t, Layer.VisibleBoundsWorldspace.Size.Center);
				case 9:
					return new ShakyTiles3DDemo(t);
				case 10:
					return new ShatteredTiles3DDemo(t);
				case 11:
					return ShuffleTilesDemo.ActionWithDuration(t);
				case 12:
					return FadeOutTRTilesDemo.ActionWithDuration(t);
				case 13:
					return FadeOutBLTilesDemo.ActionWithDuration(t);
				case 14:
					return FadeOutUpTilesDemo.ActionWithDuration(t);
				case 15:
					return FadeOutDownTilesDemo.ActionWithDuration(t);
				case 16:
					return TurnOffTilesDemo.ActionWithDuration(t);
				case 17:
					return new WavesTiles3DDemo(t);
				case 18:
					return new JumpTiles3DDemo(t);
				case 19:
					return new SplitRowsDemo(t);
				case 20:                             
					return new SplitColsDemo(t);
				case 21:
					return new PageTurn3DDemo(t);
			}

			return null;
		}

		public CCFiniteTimeAction CurrentAction
		{
			get
			{
				var pEffect = createEffect(EffectTestScene.actionIdx, 3);
				return pEffect;
			}
		}

		public void checkAnim(float dt)
		{
            var s2 = BaseNode;//[EffectTestScene.kTagBackground];
			if (s2.NumberOfRunningActions == 0 && s2.Grid != null)
				s2.Grid = null;
			
		}

		public override void RestartCallback(object sender)
		{
			NewScene();
		}

		public override void NextCallback(object sender)
		{
			// update the action index
			EffectTestScene.actionIdx++;
			EffectTestScene.actionIdx = EffectTestScene.actionIdx % MAX_LAYER;

			NewScene();
		}

		public override void BackCallback(object sender)
		{
			// update the action index
			EffectTestScene.actionIdx--;
			int total = MAX_LAYER;
			if (EffectTestScene.actionIdx < 0)
				EffectTestScene.actionIdx += total;

			NewScene();
		}

		public void NewScene()
		{
			CCScene s = new EffectTestScene();
			CCLayer child = new TextLayer();
			s.AddChild(child);
			Director.ReplaceScene(s);
		}


	}

}