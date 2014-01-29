using CocosSharp;

namespace tests
{
    public class TextLayer : CCLayerColor
    {
        //UxString	m_strTitle;
        private static int MAX_LAYER = 22;
		public static CCNode BaseNode;

        public TextLayer() : base(CCTypes.CreateColor(32, 32, 32, 255))
        {
            //BaseNode = new CCNode();
			//BaseNode.ContentSize = CCVisibleRect.VisibleRect.Size;

			//AddChild(BaseNode, 0, EffectTestScene.kTagBackground);

            var bg = new CCSprite(TestResource.s_back3);
            BaseNode = bg;
			AddChild(bg, 0, EffectTestScene.kTagBackground);
            bg.Position = CCVisibleRect.Center;

			BaseNode.RunAction(CurrentAction);

			var Kathia = new CCSprite(TestResource.s_pPathSister2);
            BaseNode.AddChild(Kathia, 1);

            Kathia.Position = new CCPoint(bg.ContentSize.Width / 3, bg.ContentSize.Center.Y);

            var sc = new CCScaleBy(2, 5);
            var sc_back = sc.Reverse();
            Kathia.RunAction(new CCRepeatForever(sc, sc_back));

			var Tamara = new CCSprite(TestResource.s_pPathSister1);
			BaseNode.AddChild(Tamara, 1);
			Tamara.Position = new CCPoint(CCVisibleRect.Left.X + 2 * CCVisibleRect.VisibleRect.Size.Width / 3,
                                          CCVisibleRect.Center.Y);
            Tamara.Position = new CCPoint(2 * bg.ContentSize.Width / 3, bg.ContentSize.Center.Y);

            var sc2 = new CCScaleBy(2, 5);
            var sc2_back = sc2.Reverse();
			Tamara.RunAction(new CCRepeatForever(sc2, sc2_back));

            var label = new CCLabelTTF(EffectTestScene.effectsList[EffectTestScene.actionIdx], "arial", 32);

            label.Position = new CCPoint(CCVisibleRect.Center.X, CCVisibleRect.Top.Y - 80);
            AddChild(label);
            label.Tag = EffectTestScene.kTagLabel;

            var item1 = new CCMenuItemImage(TestResource.s_pPathB1, TestResource.s_pPathB2, backCallback);
            var item2 = new CCMenuItemImage(TestResource.s_pPathR1, TestResource.s_pPathR2, restartCallback);
            var item3 = new CCMenuItemImage(TestResource.s_pPathF1, TestResource.s_pPathF2, nextCallback);

            var menu = new CCMenu(item1, item2, item3);

            menu.Position = CCPoint.Zero;
            item1.Position = new CCPoint(CCVisibleRect.Center.X - item2.ContentSize.Width * 2,
                                         CCVisibleRect.Bottom.Y + item2.ContentSize.Height / 2);
            item2.Position = new CCPoint(CCVisibleRect.Center.X, CCVisibleRect.Bottom.Y + item2.ContentSize.Height / 2);
            item3.Position = new CCPoint(CCVisibleRect.Center.X + item2.ContentSize.Width * 2,
                                         CCVisibleRect.Bottom.Y + item2.ContentSize.Height / 2);

            AddChild(menu, 1);

            Schedule(checkAnim);
        }

        public CCActionInterval createEffect(int nIndex, float t)
        {
            // This fixes issue https://github.com/totallyevil/cocos2d-xna/issues/148
            // TransitionTests and TileTests may have set the DepthTest to true so we need
            // to make sure we reset it.
            CCDirector.SharedDirector.SetDepthTest(false);

            switch (nIndex)
            {
                case 0:
					return new Shaky3DDemo (t);
                case 1:
					return new Waves3DDemo(t);
                case 2:
                    return FlipX3DDemo.actionWithDuration(t);
                case 3:
                    return FlipY3DDemo.actionWithDuration(t);
                case 4:
                    return new Lens3DDemo(t);
                case 5:
					return new Ripple3DDemo(t);
                case 6:
                    return LiquidDemo.actionWithDuration(t);
                case 7:
                    return WavesDemo.actionWithDuration(t);
                case 8:
                    return TwirlDemo.actionWithDuration(t);
                case 9:
                    return ShakyTiles3DDemo.actionWithDuration(t);
                case 10:
                    return ShatteredTiles3DDemo.actionWithDuration(t);
                case 11:
                    return ShuffleTilesDemo.actionWithDuration(t);
                case 12:
                    return FadeOutTRTilesDemo.actionWithDuration(t);
                case 13:
                    return FadeOutBLTilesDemo.actionWithDuration(t);
                case 14:
                    return FadeOutUpTilesDemo.actionWithDuration(t);
                case 15:
                    return FadeOutDownTilesDemo.actionWithDuration(t);
                case 16:
                    return TurnOffTilesDemo.actionWithDuration(t);
                case 17:
                    return WavesTiles3DDemo.actionWithDuration(t);
                case 18:
                    return JumpTiles3DDemo.actionWithDuration(t);
                case 19:
                    return SplitRowsDemo.actionWithDuration(t);
                case 20:
                    return SplitColsDemo.actionWithDuration(t);
                case 21:
                    return PageTurn3DDemo.actionWithDuration(t);
            }

            return null;
        }

        public CCActionInterval CurrentAction
        {
            get
            {
                var pEffect = createEffect(EffectTestScene.actionIdx, 3);
                return pEffect;
            }
        }

        public void checkAnim(float dt)
        {
            var s2 = this[EffectTestScene.kTagBackground];
            if (s2.NumberOfRunningActions() == 0 && s2.Grid != null)
                s2.Grid = null;
            ;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public void restartCallback(object pSender)
        {
            /*newOrientation();*/
            newScene();
        }

        public void nextCallback(object pSender)
        {
            // update the action index
            EffectTestScene.actionIdx++;
            EffectTestScene.actionIdx = EffectTestScene.actionIdx % MAX_LAYER;

            /*newOrientation();*/
            newScene();
        }

        public void backCallback(object pSender)
        {
            // update the action index
            EffectTestScene.actionIdx--;
            int total = MAX_LAYER;
            if (EffectTestScene.actionIdx < 0)
                EffectTestScene.actionIdx += total;

            /*newOrientation();*/
            newScene();
        }

        public void newScene()
        {
            CCScene s = new EffectTestScene();
			CCNode child = new TextLayer();
            s.AddChild(child);
            CCDirector.SharedDirector.ReplaceScene(s);
        }


    }
}