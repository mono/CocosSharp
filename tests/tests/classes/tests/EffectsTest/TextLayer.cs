using CocosSharp;
using System;
using System.Collections.Generic;

namespace tests
{

    public class TextLayer : TestNavigationLayer
    {
        private static int MAX_LAYER = 0;
        //public static CCNode BaseNode;
        public static CCNodeGrid BaseNode;

        public TextLayer() : base()
        {

            Color = new CCColor3B(32, 128, 32);
            Opacity = 255;

            MAX_LAYER = createEffectFunctions.Count;

            BaseNode = new CCNodeGrid();
            var effect = CurrentAction;
            BaseNode.RunAction(effect);
            AddChild(BaseNode, 0, EffectTestScene.kTagBackground);

            var bg = new CCSprite(TestResource.s_back3);
            BaseNode.AddChild(bg, 0);

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

        }

        public override void OnEnter()
        {
            base.OnEnter(); 

            CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            visibleBounds = Layer.VisibleBoundsWorldspace;

            BaseNode.Position = windowSize.Center;

            var size = visibleBounds.Size;

            BaseNode[EffectTestScene.kTagKathia].PositionX = -size.Width / 6;
            BaseNode[EffectTestScene.kTagTamara].PositionX = size.Width / 6;

            Schedule(checkAnim);

        }

        static CCRect visibleBounds;

        public static CCRect VisibleBounds
        {
            get
            {
                return visibleBounds;
            }
        }

        public override string Title
        {
            get
            {
                return EffectTestScene.effectsList[EffectTestScene.actionIdx];
            }
        }

        List<Func<float, CCFiniteTimeAction>> createEffectFunctions = new List<Func<float, CCFiniteTimeAction>> ()
            {

                (t) => new Shaky3DDemo(t),
                (t) => new Waves3DDemo(t),
                (t) => FlipX3DDemo.ActionWithDuration(t),
                (t) => FlipY3DDemo.ActionWithDuration(t),
                (t) => new Lens3DDemo(t),
                (t) => new Ripple3DDemo(t),
                (t) => new LiquidDemo(t),
                (t) => new WavesDemo(t),
                (t) => new TwirlDemo(t, visibleBounds.Size.Center),
                (t) => new ShakyTiles3DDemo(t),
                (t) => new ShatteredTiles3DDemo(t),
                (t) => ShuffleTilesDemo.ActionWithDuration(t),
                (t) => FadeOutTRTilesDemo.ActionWithDuration(t),
                (t) => FadeOutBLTilesDemo.ActionWithDuration(t),
                (t) => FadeOutUpTilesDemo.ActionWithDuration(t),
                (t) => FadeOutDownTilesDemo.ActionWithDuration(t),
                (t) => TurnOffTilesDemo.ActionWithDuration(t),
                (t) => new WavesTiles3DDemo(t),
                (t) => new JumpTiles3DDemo(t),
                (t) => new SplitRowsDemo(t),
                (t) => new SplitColsDemo(t),
                (t) => new PageTurn3DDemo(t),

            };

        public CCFiniteTimeAction createEffect(int nIndex, float t)
        {
            // This fixes issue https://github.com/totallyevil/cocos2d-xna/issues/148
            // TransitionTests and TileTests may have set the DepthTest to true so we need
            // to make sure we reset it.

            return createEffectFunctions[nIndex](t);

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
            //var s2 = this[EffectTestScene.kTagBackground];
            if (BaseNode.NumberOfRunningActions == 0 && BaseNode.Grid != null)
                BaseNode.Grid = null;

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

