using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TextLayer : CCLayerColor
    {
        //UxString	m_strTitle;
        static int MAX_LAYER = 22;

        public TextLayer()
        {
            InitWithColor(CCTypes.CreateColor(32, 32, 32, 255));

            float x, y;

            CCSize size = CCDirector.SharedDirector.WinSize;
            x = size.Width;
            y = size.Height;


            CCNode node = CCNode.Create();
            CCActionInterval effect = getAction();
            node.RunAction(effect);
            AddChild(node, 0, EffectTestScene.kTagBackground);

            CCSprite bg = CCSprite.Create(TestResource.s_back3);
            node.AddChild(bg, 0);
            bg.AnchorPoint = new CCPoint(0.5f, 0.5f);
            bg.Position = new CCPoint(size.Width / 2, size.Height / 2);

            CCSprite grossini = CCSprite.Create(TestResource.s_pPathSister2);
            node.AddChild(grossini, 1);
            grossini.Position = new CCPoint(x / 3, y / 2);
            CCActionInterval sc = CCScaleBy.Create(2, 5);
            CCFiniteTimeAction sc_back = sc.Reverse();
            grossini.RunAction(CCRepeatForever.Create((CCActionInterval)(CCSequence.Create(sc, sc_back))));
            //grossini.runAction(effect);

            CCSprite tamara = CCSprite.Create(TestResource.s_pPathSister1);
            node.AddChild(tamara, 1);
            tamara.Position = new CCPoint(2 * x / 3, y / 2);
            CCActionInterval sc2 = CCScaleBy.Create(2, 5);
            CCFiniteTimeAction sc2_back = sc2.Reverse();
            tamara.RunAction(CCRepeatForever.Create((CCActionInterval)(CCSequence.Create(sc2, sc2_back))));

            CCLabelTTF label = CCLabelTTF.Create(EffectTestScene.effectsList[EffectTestScene.actionIdx], "arial", 32);

            label.Position = new CCPoint(x / 2, y - 80);
            AddChild(label);
            label.Tag = EffectTestScene.kTagLabel;

            CCMenuItemImage item1 = CCMenuItemImage.Create(TestResource.s_pPathB1, TestResource.s_pPathB2, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.Create(TestResource.s_pPathR1, TestResource.s_pPathR2, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.Create(TestResource.s_pPathF1, TestResource.s_pPathF2, nextCallback);

            CCMenu menu = CCMenu.Create(item1, item2, item3);

            menu.Position = new CCPoint(0, 0);
            item1.Position = new CCPoint(size.Width / 2 - 100, 30);
            item2.Position = new CCPoint(size.Width / 2, 30);
            item3.Position = new CCPoint(size.Width / 2 + 100, 30);

            AddChild(menu, 1);

            Schedule(checkAnim);
        }

        public CCActionInterval createEffect(int nIndex, float t)
        {
//            CCDirector.SharedDirector.SetDepthTest(false);

            switch (nIndex)
            {
                case 0: return Shaky3DDemo.actionWithDuration(t);
                case 1: return Waves3DDemo.actionWithDuration(t);
                case 2: return FlipX3DDemo.actionWithDuration(t);
                case 3: return FlipY3DDemo.actionWithDuration(t);
                case 4: return Lens3DDemo.actionWithDuration(t);
                case 5: return Ripple3DDemo.actionWithDuration(t);
                case 6: return LiquidDemo.actionWithDuration(t);
                case 7: return WavesDemo.actionWithDuration(t);
                case 8: return TwirlDemo.actionWithDuration(t);
                case 9: return ShakyTiles3DDemo.actionWithDuration(t);
                case 10: return ShatteredTiles3DDemo.actionWithDuration(t);
                case 11: return ShuffleTilesDemo.actionWithDuration(t);
                case 12: return FadeOutTRTilesDemo.actionWithDuration(t);
                case 13: return FadeOutBLTilesDemo.actionWithDuration(t);
                case 14: return FadeOutUpTilesDemo.actionWithDuration(t);
                case 15: return FadeOutDownTilesDemo.actionWithDuration(t);
                case 16: return TurnOffTilesDemo.actionWithDuration(t);
                case 17: return WavesTiles3DDemo.actionWithDuration(t);
                case 18: return JumpTiles3DDemo.actionWithDuration(t);
                case 19: return SplitRowsDemo.actionWithDuration(t);
                case 20: return SplitColsDemo.actionWithDuration(t);
                case 21: return PageTurn3DDemo.actionWithDuration(t);
            }

            return null;
        }

        public CCActionInterval getAction()
        {
            CCActionInterval pEffect = createEffect(EffectTestScene.actionIdx, 3);

            return pEffect;
        }

        public void checkAnim(float dt)
        {
            CCNode s2 = GetChildByTag(EffectTestScene.kTagBackground);
            if (s2.NumberOfRunningActions() == 0 && s2.Grid != null)
                s2.Grid = null; ;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public void restartCallback(CCObject pSender)
        {
            /*newOrientation();*/
            newScene();
        }

        public void nextCallback(CCObject pSender)
        {
            // update the action index
            EffectTestScene.actionIdx++;
            EffectTestScene.actionIdx = EffectTestScene.actionIdx % MAX_LAYER;

            /*newOrientation();*/
            newScene();
        }

        public void backCallback(CCObject pSender)
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
            CCNode child = TextLayer.node();
            s.AddChild(child);
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public new static TextLayer node()
        {
            TextLayer pLayer = new TextLayer();

            return pLayer;
        }
    }
}
