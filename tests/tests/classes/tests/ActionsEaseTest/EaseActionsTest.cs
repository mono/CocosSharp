using System;
using CocosSharp;
using Random = CocosSharp.CCRandom;

namespace tests
{
    public class EaseSpriteDemo : CCLayer
    {
        protected CCSprite m_grossini;
        protected CCSprite m_kathia;

        protected String m_strTitle;
        protected CCSprite m_tamara;

        public virtual String title()
        {
            return "No title";
        }

        public override void OnEnter()
        {
            base.OnEnter();
            m_grossini = new CCSprite(TestResource.s_pPathGrossini);
            m_tamara = new CCSprite(TestResource.s_pPathSister1);
            m_kathia = new CCSprite(TestResource.s_pPathSister2);

            AddChild(m_grossini, 3);
            AddChild(m_kathia, 2);
            AddChild(m_tamara, 1);

            var s = Director.WindowSizeInPoints;

            m_grossini.Position = new CCPoint(60, 50);
            m_kathia.Position = new CCPoint(60, 150);
            m_tamara.Position = new CCPoint(60, 250);

            var label = new CCLabelTtf(title(), "arial", 32);
            AddChild(label);
            label.Position = new CCPoint(s.Width / 2, s.Height - 50);

            var item1 = new CCMenuItemImage(TestResource.s_pPathB1, TestResource.s_pPathB2, backCallback);
            var item2 = new CCMenuItemImage(TestResource.s_pPathR1, TestResource.s_pPathR2, restartCallback);
            var item3 = new CCMenuItemImage(TestResource.s_pPathF1, TestResource.s_pPathF2, nextCallback);

            var menu = new CCMenu(item1, item2, item3);
            menu.Position = CCPoint.Zero;
            item1.Position = new CCPoint(s.Width / 2 - 100, 30);
            item2.Position = new CCPoint(s.Width / 2, 30);
            item3.Position = new CCPoint(s.Width / 2 + 100, 30);

            AddChild(menu, 1);
        }

        public void restartCallback(object pSender)
        {
            CCScene s = new EaseActionsTestScene();
            s.AddChild(EaseTest.restartEaseAction());
            Director.ReplaceScene(s);
        }

        public void nextCallback(object pSender)
        {
            CCScene s = new EaseActionsTestScene();
            s.AddChild(EaseTest.nextEaseAction());
            Director.ReplaceScene(s);
            
        }

        public void backCallback(object pSender)
        {
            CCScene s = new EaseActionsTestScene();
            s.AddChild(EaseTest.backEaseAction());
            Director.ReplaceScene(s);
        }

        public void positionForTwo()
        {
            m_grossini.Position = new CCPoint(60, 120);
            m_tamara.Position = new CCPoint(60, 220);
            m_kathia.Visible = false;
        }
    }

    public class SpriteEase : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var size = Director.WindowSizeInPoints;

            var move = new CCMoveBy(3, new CCPoint(size.Width - 130, 0));
            var move_back = (CCActionInterval) move.Reverse();

            var move_ease_in = new CCEaseIn(move, 2.5f);
            var move_ease_in_back = move_ease_in.Reverse();

            var move_ease_out = new CCEaseOut(move, 2.5f);
            var move_ease_out_back = move_ease_out.Reverse();

			var delay = new CCDelayTime(0.25f);

            var seq1 = new CCSequence(move, delay, move_back, delay);
            var seq2 = new CCSequence(move_ease_in, delay, move_ease_in_back, delay);
            var seq3 = new CCSequence(move_ease_out, delay, move_ease_out_back,
                                                delay);

            var a2 = m_grossini.RunAction(new CCRepeatForever ((CCActionInterval)seq1));
            //a2.Tag = 1;

            var a1 = m_tamara.RunAction(new CCRepeatForever ((CCActionInterval)seq2));
            //a1.Tag = 1;

            var a = m_kathia.RunAction(new CCRepeatForever ((CCActionInterval)seq3));
            //a.Tag = 1;

            Schedule(testStopAction, 6.25f);
        }

        public override String title()
        {
            return "EaseIn - EaseOut - Stop";
        }

        public void testStopAction(float dt)
        {
            Unschedule(testStopAction);
            m_kathia.StopAction(1);
            m_tamara.StopAction(1);
            m_grossini.StopAction(1);
        }
    }

    public class SpriteEaseInOut : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var size = Director.WindowSizeInPoints;

            var move = new CCMoveBy (3, new CCPoint(size.Width - 130, 0));

            var move_ease_inout1 = new CCEaseInOut(move, 0.65f);
            var move_ease_inout_back1 = move_ease_inout1.Reverse();

            var move_ease_inout2 = new CCEaseInOut(move, 1.35f);
            var move_ease_inout_back2 = move_ease_inout2.Reverse();

            var move_ease_inout3 = new CCEaseInOut(move, 1.0f);
            var move_ease_inout_back3 = move_ease_inout3.Reverse() as CCActionInterval;

            var delay = new CCDelayTime (0.25f);

            var seq1 = new CCSequence(move_ease_inout1, delay, move_ease_inout_back1, delay);
            var seq2 = new CCSequence(move_ease_inout2, delay, move_ease_inout_back2,
                                                delay);
            var seq3 = new CCSequence(move_ease_inout3, delay, move_ease_inout_back3,
                                                delay);

            m_tamara.RunAction(new CCRepeatForever ((CCActionInterval)seq1));
            m_kathia.RunAction(new CCRepeatForever ((CCActionInterval)seq2));
            m_grossini.RunAction(new CCRepeatForever ((CCActionInterval)seq3));
        }

        public override String title()
        {
            return "EaseInOut and rates";
        }
    }

    public class SpriteEaseExponential : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = Director.WindowSizeInPoints;

            var move = new CCMoveBy (3, new CCPoint(s.Width - 130, 0));
            var move_back = move.Reverse();

            var move_ease_in = new CCEaseExponentialIn(move);
            var move_ease_in_back = move_ease_in.Reverse();

            var move_ease_out = new CCEaseExponentialOut(move);
            var move_ease_out_back = move_ease_out.Reverse();

            var delay = new CCDelayTime (0.25f);

            var seq1 = new CCSequence(move, delay, move_back, delay);
            var seq2 = new CCSequence(move_ease_in, delay, move_ease_in_back, delay);
            var seq3 = new CCSequence(move_ease_out, delay, move_ease_out_back,
                                                delay);


            m_grossini.RunAction(new CCRepeatForever (seq1));
            m_tamara.RunAction(new CCRepeatForever (seq2));
            m_kathia.RunAction(new CCRepeatForever (seq3));
        }

        public override String title()
        {
            return "ExpIn - ExpOut actions";
        }
    }

    public class SpriteEaseExponentialInOut : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = Director.WindowSizeInPoints;

            var move = new CCMoveBy (3, new CCPoint(s.Width - 130, 0));
            var move_back = move.Reverse();

            var move_ease = new CCEaseExponentialInOut(move);
            var move_ease_back = move_ease.Reverse(); //-. reverse()

            var delay = new CCDelayTime (0.25f);

            var seq1 = new CCSequence(move, delay, move_back, delay);
            var seq2 = new CCSequence(move_ease, delay, move_ease_back, delay);

            positionForTwo();

            m_grossini.RunAction(new CCRepeatForever (seq1));
            m_tamara.RunAction(new CCRepeatForever (seq2));
        }

        public override String title()
        {
            return "EaseExponentialInOut action";
        }
    }

    public class SpriteEaseSine : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = Director.WindowSizeInPoints;

            var move = new CCMoveBy (3, new CCPoint(s.Width - 130, 0));
            var move_back = move.Reverse();

            var move_ease_in = new CCEaseSineIn(move);
            var move_ease_in_back = move_ease_in.Reverse();

            var move_ease_out = new CCEaseSineOut(move);
            var move_ease_out_back = move_ease_out.Reverse();

            var delay = new CCDelayTime (0.25f);

            var seq1 = new CCSequence(move, delay, move_back, delay);
            var seq2 = new CCSequence(move_ease_in, delay, move_ease_in_back, delay);
            var seq3 = new CCSequence(move_ease_out, delay, move_ease_out_back,
                                                delay);

            m_grossini.RunAction(new CCRepeatForever (seq1));
            m_tamara.RunAction(new CCRepeatForever (seq2));
            m_kathia.RunAction(new CCRepeatForever (seq3));
        }

        public override String title()
        {
            return "EaseSineIn - EaseSineOut";
        }
    }

    public class SpriteEaseSineInOut : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = Director.WindowSizeInPoints;

            var move = new CCMoveBy (3, new CCPoint(s.Width - 130, 0));
            var move_back = move.Reverse();

            var move_ease = new CCEaseSineInOut(move);
            var move_ease_back = move_ease.Reverse();

            var delay = new CCDelayTime (0.25f);

            var seq1 = new CCSequence(move, delay, move_back, delay);
            var seq2 = new CCSequence(move_ease, delay, move_ease_back, delay);

            positionForTwo();

            m_grossini.RunAction(new CCRepeatForever (seq1));
            m_tamara.RunAction(new CCRepeatForever (seq2));
        }

        public override String title()
        {
            return "EaseSineInOut action";
        }
    }

    public class SpriteEaseElastic : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = Director.WindowSizeInPoints;

            var move = new CCMoveBy (3, new CCPoint(s.Width - 130, 0));
            var move_back = move.Reverse();

            var move_ease_in = new CCEaseElasticIn(move);
            var move_ease_in_back = move_ease_in.Reverse();

            var move_ease_out = new CCEaseElasticOut(move);
            var move_ease_out_back = move_ease_out.Reverse();

            var delay = new CCDelayTime (0.25f);

            var seq1 = new CCSequence(move, delay, move_back, delay);
            var seq2 = new CCSequence(move_ease_in, delay, move_ease_in_back, delay);
            var seq3 = new CCSequence(move_ease_out, delay, move_ease_out_back,
                                                delay);

            m_grossini.RunAction(new CCRepeatForever (seq1));
            m_tamara.RunAction(new CCRepeatForever (seq2));
            m_kathia.RunAction(new CCRepeatForever (seq3));
        }

        public override String title()
        {
            return "Elastic In - Out actions";
        }
    }

    public class SpriteEaseElasticInOut : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = Director.WindowSizeInPoints;

            var move = new CCMoveBy (3, new CCPoint(s.Width - 130, 0));

            var move_ease_inout1 = new CCEaseElasticInOut(move, 0.3f);
            var move_ease_inout_back1 = move_ease_inout1.Reverse();

            var move_ease_inout2 = new CCEaseElasticInOut(move, 0.45f);
            var move_ease_inout_back2 = move_ease_inout2.Reverse();

            var move_ease_inout3 = new CCEaseElasticInOut(move, 0.6f);
            var move_ease_inout_back3 = move_ease_inout3.Reverse();

            var delay = new CCDelayTime (0.25f);

            var seq1 = new CCSequence(move_ease_inout1, delay, move_ease_inout_back1, delay);
            var seq2 = new CCSequence(move_ease_inout2, delay, move_ease_inout_back2,
                                                delay);
            var seq3 = new CCSequence(move_ease_inout3, delay, move_ease_inout_back3,
                                                delay);

            m_tamara.RunAction(new CCRepeatForever (seq1));
            m_kathia.RunAction(new CCRepeatForever (seq2));
            m_grossini.RunAction(new CCRepeatForever (seq3));
        }

        public override String title()
        {
            return "EaseElasticInOut action";
        }
    }

    public class SpriteEaseBounce : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = Director.WindowSizeInPoints;

            var move = new CCMoveBy (3, new CCPoint(s.Width - 130, 0));
            var move_back = move.Reverse();

            var move_ease_in = new CCEaseBounceIn(move);
            var move_ease_in_back = move_ease_in.Reverse();

            var move_ease_out = new CCEaseBounceOut(move);
            var move_ease_out_back = move_ease_out.Reverse();

            var delay = new CCDelayTime (0.25f);

            var seq1 = new CCSequence(move, delay, move_back, delay);
            var seq2 = new CCSequence(move_ease_in, delay, move_ease_in_back, delay);
            var seq3 = new CCSequence(move_ease_out, delay, move_ease_out_back,
                                                delay);

            m_grossini.RunAction(new CCRepeatForever (seq1));
            m_tamara.RunAction(new CCRepeatForever (seq2));
            m_kathia.RunAction(new CCRepeatForever (seq3));
        }

        public override String title()
        {
            return "Bounce In - Out actions";
        }
    }

    public class SpriteEaseBounceInOut : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = Director.WindowSizeInPoints;

            var move = new CCMoveBy (3, new CCPoint(s.Width - 130, 0));
            var move_back = move.Reverse();

            var move_ease = new CCEaseBounceInOut(move);
            var move_ease_back = move_ease.Reverse();

            var delay = new CCDelayTime (0.25f);

            var seq1 = new CCSequence(move, delay, move_back, delay);
            var seq2 = new CCSequence(move_ease, delay, move_ease_back, delay);

            positionForTwo();

            m_grossini.RunAction(new CCRepeatForever (seq1));
            m_tamara.RunAction(new CCRepeatForever (seq2));
        }

        public override String title()
        {
            return "EaseBounceInOut action";
        }
    }

    public class SpriteEaseBack : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = Director.WindowSizeInPoints;

            var move = new CCMoveBy (3, new CCPoint(s.Width - 130, 0));
            var move_back = move.Reverse();

            var move_ease_in = new CCEaseBackIn(move);
            var move_ease_in_back = move_ease_in.Reverse();

            var move_ease_out = new CCEaseBackOut(move);
            var move_ease_out_back = move_ease_out.Reverse();

            var delay = new CCDelayTime (0.25f);

            var seq1 = new CCSequence(move, delay, move_back, delay);
            var seq2 = new CCSequence(move_ease_in, delay, move_ease_in_back, delay);
            var seq3 = new CCSequence(move_ease_out, delay, move_ease_out_back,
                                                delay);

            m_grossini.RunAction(new CCRepeatForever (seq1));
            m_tamara.RunAction(new CCRepeatForever (seq2));
            m_kathia.RunAction(new CCRepeatForever (seq3));
        }

        public override String title()
        {
            return "Back In - Out actions";
        }
    }

    public class SpriteEaseBackInOut : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = Director.WindowSizeInPoints;

            var move = new CCMoveBy (3, new CCPoint(s.Width - 130, 0));
            var move_back = move.Reverse();

            var move_ease = new CCEaseBackInOut(move);
            var move_ease_back = move_ease.Reverse() as CCActionInterval;

            var delay = new CCDelayTime (0.25f);

            var seq1 = new CCSequence(move, delay, move_back, delay);
            var seq2 = new CCSequence(move_ease, delay, move_ease_back, delay);

            positionForTwo();

            m_grossini.RunAction(new CCRepeatForever (seq1));
            m_tamara.RunAction(new CCRepeatForever (seq2));
        }

        public override String title()
        {
            return "EaseBackInOut action";
        }
    }

    public class SpeedTest : EaseSpriteDemo
    {
        CCSpeed speedAction1, speedAction2, speedAction3;
       
        public override void OnEnter()
        {
            base.OnEnter();

            var s = Director.WindowSizeInPoints;

            // rotate and jump
            var jump1 = new CCJumpBy (4, new CCPoint(-s.Width + 80, 0), 100, 4);
            var jump2 = jump1.Reverse();
            var rot1 = new CCRotateBy (4, 360 * 2);
            var rot2 = rot1.Reverse();

            var seq3_1 = new CCSequence(jump2, jump1);
            var seq3_2 = new CCSequence(rot1, rot2);
            var spawn = new CCSpawn(seq3_1, seq3_2);

            speedAction1 = new CCSpeed(new CCRepeatForever (spawn), 1.0f);
            speedAction2 = new CCSpeed(new CCRepeatForever (spawn), 2.0f);
            speedAction3 = new CCSpeed(new CCRepeatForever (spawn), 0.5f);

            m_grossini.RunAction(speedAction1);
			m_tamara.RunAction(speedAction2);
			m_kathia.RunAction(speedAction3);
        }

        public override String title()
        {
            return "Speed action";
        }
    }

    public class EaseActionsTestScene : TestScene
    {
        protected override void NextTestCase()
        {
        }
        protected override void PreviousTestCase()
        {
        }
        protected override void RestTestCase()
        {
        }
        public override void runThisTest()
        {
            var pLayer = EaseTest.nextEaseAction();
            AddChild(pLayer);

            Director.ReplaceScene(this);
        }
    }

    public static class EaseTest
    {
        public const int MAX_LAYER = 13;
        public const int kTagAction1 = 1;
        public const int kTagAction2 = 2;
        public const int kTagSlider = 1;
        private static int sceneIdx = -1;

        public static CCLayer createEaseLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0:
                    return new SpriteEase();
				case 1:
                    return new SpriteEaseInOut();
                case 2:
                    return new SpriteEaseExponential();
                case 3:
                    return new SpriteEaseExponentialInOut();
                case 4:
                    return new SpriteEaseSine();
                case 5:
                    return new SpriteEaseSineInOut();
                case 6:
                    return new SpriteEaseElastic();
                case 7:
                    return new SpriteEaseElasticInOut();
                case 8:
                    return new SpriteEaseBounce();
                case 9:
                    return new SpriteEaseBounceInOut();
                case 10:
                    return new SpriteEaseBack();
                case 11:
                    return new SpriteEaseBackInOut();
				case 12:
                    return new SpeedTest();
            }
            return null;
        }

        public static CCLayer nextEaseAction()
        {
            sceneIdx++;
            sceneIdx %= MAX_LAYER;

            var pLayer = createEaseLayer(sceneIdx);
            return pLayer;
        }

        public static CCLayer backEaseAction()
        {
            sceneIdx--;
            var total = MAX_LAYER;
            if (sceneIdx < 0) sceneIdx += total;
            var pLayer = createEaseLayer(sceneIdx);
            return pLayer;
        }

        public static CCLayer restartEaseAction()
        {
            var pLayer = createEaseLayer(sceneIdx);
            return pLayer;
        }
    }
}