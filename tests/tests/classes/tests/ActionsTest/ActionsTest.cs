using System;
using System.Collections.Generic;
using Cocos2D;

namespace tests
{
    public enum ActionTest
    {
        ACTION_MANUAL_LAYER = 0,
        ACTION_MOVE_LAYER,
        ACTION_SCALE_LAYER,
        ACTION_ROTATE_LAYER,
        ACTION_SKEW_LAYER,
        ACTION_SKEWROTATE_LAYER,
        ACTION_JUMP_LAYER,
        ACTION_CARDINALSPLINE_LAYER,
        ACTION_CATMULLROM_LAYER,
        ACTION_BEZIER_LAYER,
        ACTION_BLINK_LAYER,
        ACTION_FADE_LAYER,
        ACTION_TINT_LAYER,
        ACTION_ANIMATE_LAYER,
        ACTION_SEQUENCE_LAYER,
        ACTION_SEQUENCE2_LAYER,
        ACTION_SPAWN_LAYER,
        ACTION_REVERSE,
        ACTION_DELAYTIME_LAYER,
        ACTION_REPEAT_LAYER,
        ACTION_REPEATEFOREVER_LAYER,
        ACTION_ROTATETOREPEATE_LAYER,
        ACTION_ROTATEJERK_LAYER,
        ACTION_CALLFUNC_LAYER,
        ACTION_CALLFUNCND_LAYER,
        ACTION_REVERSESEQUENCE_LAYER,
        ACTION_REVERSESEQUENCE2_LAYER,
        ACTION_ORBIT_LAYER,
        ACTION_FLLOW_LAYER,
        ACTION_TARGETED_LAYER,
        PAUSERESUMEACTIONS_LAYER,
        ACTION_ISSUE1305_LAYER,
        ACTION_ISSUE1305_2_LAYER,
        ACTION_ISSUE1288_LAYER,
        ACTION_ISSUE1288_2_LAYER,
        ACTION_ISSUE1327_LAYER,
        ACTION_LAYER_COUNT,
    };


    // the class inherit from TestScene
    // every Scene each test used must inherit from TestScene,
    // make sure the test have the menu item for back to main menu
    public class ActionsTestScene : TestScene
    {
        public static int s_nActionIdx = -1;

        public static CCLayer CreateLayer(int nIndex)
        {
            CCLayer pLayer = null;

            switch (nIndex)
            {
                case (int) ActionTest.ACTION_MANUAL_LAYER:
                    pLayer = new ActionManual();
                    break;
                case (int) ActionTest.ACTION_MOVE_LAYER:
                    pLayer = new ActionMove();
                    break;
                case (int) ActionTest.ACTION_SCALE_LAYER:
                    pLayer = new ActionScale();
                    break;
                case (int) ActionTest.ACTION_ROTATE_LAYER:
                    pLayer = new ActionRotate();
                    break;
                case (int) ActionTest.ACTION_SKEW_LAYER:
                    pLayer = new ActionSkew();
                    break;
                case (int) ActionTest.ACTION_SKEWROTATE_LAYER:
                    pLayer = new ActionSkewRotateScale();
                    break;
                case (int) ActionTest.ACTION_JUMP_LAYER:
                    pLayer = new ActionJump();
                    break;
                case (int) ActionTest.ACTION_BEZIER_LAYER:
                    pLayer = new ActionBezier();
                    break;
                case (int) ActionTest.ACTION_BLINK_LAYER:
                    pLayer = new ActionBlink();
                    break;
                case (int) ActionTest.ACTION_FADE_LAYER:
                    pLayer = new ActionFade();
                    break;
                case (int) ActionTest.ACTION_TINT_LAYER:
                    pLayer = new ActionTint();
                    break;
                case (int) ActionTest.ACTION_ANIMATE_LAYER:
                    pLayer = new ActionAnimate();
                    break;
                case (int) ActionTest.ACTION_SEQUENCE_LAYER:
                    pLayer = new ActionSequence();
                    break;
                case (int) ActionTest.ACTION_SEQUENCE2_LAYER:
                    pLayer = new ActionSequence2();
                    break;
                case (int) ActionTest.ACTION_SPAWN_LAYER:
                    pLayer = new ActionSpawn();
                    break;
                case (int) ActionTest.ACTION_REVERSE:
                    pLayer = new ActionReverse();
                    break;
                case (int) ActionTest.ACTION_DELAYTIME_LAYER:
                    pLayer = new ActionDelayTime();
                    break;
                case (int) ActionTest.ACTION_REPEAT_LAYER:
                    pLayer = new ActionRepeat();
                    break;
                case (int) ActionTest.ACTION_REPEATEFOREVER_LAYER:
                    pLayer = new ActionRepeatForever();
                    break;
                case (int) ActionTest.ACTION_ROTATETOREPEATE_LAYER:
                    pLayer = new ActionRotateToRepeat();
                    break;
                case (int) ActionTest.ACTION_ROTATEJERK_LAYER:
                    pLayer = new ActionRotateJerk();
                    break;
                case (int) ActionTest.ACTION_CALLFUNC_LAYER:
                    pLayer = new ActionCallFunc();
                    break;
                case (int) ActionTest.ACTION_CALLFUNCND_LAYER:
                    pLayer = new ActionCallFuncND();
                    break;
                case (int) ActionTest.ACTION_REVERSESEQUENCE_LAYER:
                    pLayer = new ActionReverseSequence();
                    break;
                case (int) ActionTest.ACTION_REVERSESEQUENCE2_LAYER:
                    pLayer = new ActionReverseSequence2();
                    break;
                case (int) ActionTest.ACTION_ORBIT_LAYER:
                    pLayer = new ActionOrbit();
                    break;
                case (int) ActionTest.ACTION_FLLOW_LAYER:
                    pLayer = new ActionFollow();
                    break;
                case (int) ActionTest.ACTION_TARGETED_LAYER:
                    pLayer = new ActionTargeted();
                    break;
                case (int) ActionTest.ACTION_ISSUE1305_LAYER:
                    pLayer = new Issue1305();
                    break;
                case (int) ActionTest.ACTION_ISSUE1305_2_LAYER:
                    pLayer = new Issue1305_2();
                    break;
                case (int) ActionTest.ACTION_ISSUE1288_LAYER:
                    pLayer = new Issue1288();
                    break;
                case (int) ActionTest.ACTION_ISSUE1288_2_LAYER:
                    pLayer = new Issue1288_2();
                    break;
                case (int) ActionTest.ACTION_ISSUE1327_LAYER:
                    pLayer = new Issue1327();
                    break;
                case (int) ActionTest.ACTION_CARDINALSPLINE_LAYER:
                    pLayer = new ActionCardinalSpline();
                    break;
                case (int) ActionTest.ACTION_CATMULLROM_LAYER:
                    pLayer = new ActionCatmullRom();
                    break;
                case (int) ActionTest.PAUSERESUMEACTIONS_LAYER:
                    pLayer = new PauseResumeActions();
                    break;
                default:
                    break;
            }

            return pLayer;
        }
        protected override void NextTestCase()
        {
            NextAction();
        }
        protected override void PreviousTestCase()
        {
            BackAction();
        }
        protected override void RestTestCase()
        {
            RestartAction();
        }

        public static CCLayer NextAction()
        {
            ++s_nActionIdx;
            s_nActionIdx = s_nActionIdx % (int) ActionTest.ACTION_LAYER_COUNT;

            var pLayer = CreateLayer(s_nActionIdx);

            return pLayer;
        }

        public static CCLayer BackAction()
        {
            --s_nActionIdx;
            if (s_nActionIdx < 0)
                s_nActionIdx += (int) ActionTest.ACTION_LAYER_COUNT;

            var pLayer = CreateLayer(s_nActionIdx);

            return pLayer;
        }

        public static CCLayer RestartAction()
        {
            var pLayer = CreateLayer(s_nActionIdx);

            return pLayer;
        }


        public override void runThisTest()
        {
            s_nActionIdx = -1;
            AddChild(NextAction());

            CCDirector.SharedDirector.ReplaceScene(this);
        }
    }

    public class ActionsDemo : CCLayer
    {
        protected CCSprite m_grossini;
        protected CCSprite m_kathia;
        protected CCSprite m_tamara;

        public virtual string title()
        {
            return "ActionsTest";
        }

        public virtual string subtitle()
        {
            return "";
        }

        public override void OnEnter()
        {
            base.OnEnter();

            // Or you can create an sprite using a filename. only PNG is supported now. Probably TIFF too
            m_grossini = new CCSprite(TestResource.s_pPathGrossini);

            m_tamara = new CCSprite(TestResource.s_pPathSister1);

            m_kathia = new CCSprite(TestResource.s_pPathSister2);

            AddChild(m_grossini, 1);
            AddChild(m_tamara, 2);
            AddChild(m_kathia, 3);

            var s = CCDirector.SharedDirector.WinSize;

            m_grossini.Position = new CCPoint(s.Width / 2, s.Height / 3);
            m_tamara.Position = new CCPoint(s.Width / 2, 2 * s.Height / 3);
            m_kathia.Position = new CCPoint(s.Width / 2, s.Height / 2);

            // add title and subtitle
            var str = title();
            var pTitle = str;
            var label = new CCLabelTTF(pTitle, "arial", 18);
            AddChild(label, 1);
            label.Position = new CCPoint(s.Width / 2, s.Height - 30);

            var strSubtitle = subtitle();
            if (! strSubtitle.Equals(""))
            {
                var l = new CCLabelTTF(strSubtitle, "arial", 22);
                AddChild(l, 1);
                l.Position = new CCPoint(s.Width / 2, s.Height - 60);
            }

            // add menu
            var item1 = new CCMenuItemImage(TestResource.s_pPathB1, TestResource.s_pPathB2, backCallback);
            var item2 = new CCMenuItemImage(TestResource.s_pPathR1, TestResource.s_pPathR2, restartCallback);
            var item3 = new CCMenuItemImage(TestResource.s_pPathF1, TestResource.s_pPathF2, nextCallback);

            var menu = new CCMenu(item1, item2, item3);

            menu.Position = new CCPoint(0, 0);
            item1.Position = new CCPoint(s.Width / 2 - 100, 30);
            item2.Position = new CCPoint(s.Width / 2, 30);
            item3.Position = new CCPoint(s.Width / 2 + 100, 30);

            AddChild(menu, 1);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public void restartCallback(object pSender)
        {
            var s = new ActionsTestScene();
            s.AddChild(ActionsTestScene.RestartAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(object pSender)
        {
            var s = new ActionsTestScene();
            s.AddChild(ActionsTestScene.NextAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void backCallback(object pSender)
        {
            var s = new ActionsTestScene();
            s.AddChild(ActionsTestScene.BackAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void centerSprites(uint numberOfSprites)
        {
            var s = CCDirector.SharedDirector.WinSize;

            if (numberOfSprites == 0)
            {
                m_tamara.Visible = false;
                m_kathia.Visible = false;
                m_grossini.Visible = false;
            }
            else if (numberOfSprites == 1)
            {
                m_tamara.Visible = false;
                m_kathia.Visible = false;
                m_grossini.Position = new CCPoint(s.Width / 2, s.Height / 2);
            }
            else if (numberOfSprites == 2)
            {
                m_kathia.Position = new CCPoint(s.Width / 3, s.Height / 2);
                m_tamara.Position = new CCPoint(2 * s.Width / 3, s.Height / 2);
                m_grossini.Visible = false;
            }
            else if (numberOfSprites == 3)
            {
                m_grossini.Position = new CCPoint(s.Width / 2, s.Height / 2);
                m_tamara.Position = new CCPoint(s.Width / 4, s.Height / 2);
                m_kathia.Position = new CCPoint(3 * s.Width / 4, s.Height / 2);
            }
        }

        public void alignSpritesLeft(uint numberOfSprites)
        {
            var s = CCDirector.SharedDirector.WinSize;

            if (numberOfSprites == 1)
            {
                m_tamara.Visible = false;
                m_kathia.Visible = false;
                m_grossini.Position = new CCPoint(60, s.Height / 2);
            }
            else if (numberOfSprites == 2)
            {
                m_kathia.Position = new CCPoint(60, s.Height / 3);
                m_tamara.Position = new CCPoint(60, 2 * s.Height / 3);
                m_grossini.Visible = false;
            }
            else if (numberOfSprites == 3)
            {
                m_grossini.Position = new CCPoint(60, s.Height / 2);
                m_tamara.Position = new CCPoint(60, 2 * s.Height / 3);
                m_kathia.Position = new CCPoint(60, s.Height / 3);
            }
        }
    };

    public class ActionManual : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;

            m_tamara.ScaleX = 2.5f;
            m_tamara.ScaleY = -1.0f;
            m_tamara.Position = new CCPoint(100, 70);
            m_tamara.Opacity = 128;

            m_grossini.Rotation = 120;
            m_grossini.Position = new CCPoint(s.Width / 2, s.Height / 2);
            m_grossini.Color = new CCColor3B(255, 0, 0);

            m_kathia.Position = new CCPoint(s.Width - 100, s.Height / 2);
            m_kathia.Color = new CCColor3B(0, 0, 255); // ccTypes.ccBLUE
        }

        public override string subtitle()
        {
            return "Manual Transformation";
        }
    };

    public class ActionMove : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(3);

            var s = CCDirector.SharedDirector.WinSize;

            var actionTo = new CCMoveTo (2, new CCPoint(s.Width - 40, s.Height - 40));
            var actionBy = new CCMoveBy (2, new CCPoint(80, 80));
            var actionByBack = actionBy.Reverse();

            m_tamara.RunAction(actionTo);
            m_grossini.RunAction(CCSequence.FromActions(actionBy, actionByBack));
            m_kathia.RunAction(new CCMoveTo (1, new CCPoint(40, 40)));
        }

        public override string subtitle()
        {
            return "MoveTo / MoveBy";
        }
    }

    public class ActionScale : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(3);

            var actionTo = new CCScaleTo(2, 0.5f);
            var actionBy = new CCScaleBy(2, 1, 10);
            var actionBy2 = new CCScaleBy(2, 5f, 1.0f);
            var actionByBack = actionBy.Reverse();

            m_grossini.RunAction(actionTo);
            m_tamara.RunAction(CCSequence.FromActions(actionBy, actionBy.Reverse()));
            m_kathia.RunAction(CCSequence.FromActions(actionBy2, actionBy2.Reverse()));
        }

        public override string subtitle()
        {
            return "ScaleTo / ScaleBy";
        }
    };

    public class ActionSkew : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(3);

            var actionTo = new CCSkewTo (2, 37.2f, -37.2f);
            var actionToBack = new CCSkewTo (2, 0, 0);
            var actionBy = new CCSkewBy (2, 0.0f, -90.0f);
            var actionBy2 = new CCSkewBy (2, 45.0f, 45.0f);
            var actionByBack = actionBy.Reverse();

            m_tamara.RunAction(CCSequence.FromActions(actionTo, actionToBack));
            m_grossini.RunAction(CCSequence.FromActions(actionBy, actionByBack));

            m_kathia.RunAction(CCSequence.FromActions(actionBy2, actionBy2.Reverse()));
        }

        public override string subtitle()
        {
            return "SkewTo / SkewBy";
        }
    };

    public class ActionSkewRotateScale : ActionsDemo
    {
        private const float markrside = 10.0f;

        public override void OnEnter()
        {

            base.OnEnter();

            m_tamara.RemoveFromParentAndCleanup(true);
            m_grossini.RemoveFromParentAndCleanup(true);
            m_kathia.RemoveFromParentAndCleanup(true);

			// Get window size so that we can center the box layer
			var winSize = CCDirector.SharedDirector.WinSize;

			var boxSize = new CCSize(100.0f, 100.0f);

            var box = new CCLayerColor(new CCColor4B(255, 255, 0, 255));
            box.AnchorPoint = new CCPoint(0, 0);
			box.Position = new CCPoint(winSize.Center.X - (boxSize.Width / 2), winSize.Center.Y - (boxSize.Height / 2));
            box.ContentSize = boxSize;

            var uL = new CCLayerColor(new CCColor4B(255, 0, 0, 255));
            box.AddChild(uL);
            uL.ContentSize = new CCSize(markrside, markrside);
            uL.Position = new CCPoint(0.0f, boxSize.Height - markrside);
            uL.AnchorPoint = new CCPoint(0, 0);

            var uR = new CCLayerColor(new CCColor4B(0, 0, 255, 255));
            box.AddChild(uR);
            uR.ContentSize = new CCSize(markrside, markrside);
            uR.Position = new CCPoint(boxSize.Width - markrside, boxSize.Height - markrside);
            uR.AnchorPoint = new CCPoint(0, 0);
            AddChild(box);

            var actionTo = new CCSkewTo (2, 0.0f, 2.0f);
            var rotateTo = new CCRotateTo (2, 61.0f);
            var actionScaleTo = new CCScaleTo(2, -0.44f, 0.47f);

            var actionScaleToBack = new CCScaleTo(2, 1.0f, 1.0f);
            var rotateToBack = new CCRotateTo (2, 0);
            var actionToBack = new CCSkewTo (2, 0, 0);

            box.RunAction(CCSequence.FromActions(actionTo, actionToBack));
            box.RunAction(CCSequence.FromActions(rotateTo, rotateToBack));
            box.RunAction(CCSequence.FromActions(actionScaleTo, actionScaleToBack));
        }

        public override string subtitle()
        {
            return "Skew + Rotate + Scale";
        }
    };

    public class ActionRotate : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(3);

            var actionTo = new CCRotateTo (2, 45);
            var actionTo2 = new CCRotateTo (2, -45);
            var actionTo0 = new CCRotateTo (2, 0);
            m_tamara.RunAction(CCSequence.FromActions(actionTo, actionTo0));

            var actionBy = new CCRotateBy (2, 360);
            var actionByBack = actionBy.Reverse();
            m_grossini.RunAction(CCSequence.FromActions(actionBy, actionByBack));

            // m_kathia->runAction( CCSequence::actions(actionTo2, actionTo0->copy()->autorelease(), NULL));
            m_kathia.RunAction(CCSequence.FromActions(actionTo2, (CCActionInterval) actionTo0.Copy()));
        }

        public override string subtitle()
        {
            return "RotateTo / RotateBy";
        }
    };

    public class ActionJump : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(3);

            var actionTo = new CCJumpTo (2, new CCPoint(300, 300), 50, 4);
            var actionBy = new CCJumpBy (2, new CCPoint(300, 0), 50, 4);
            var actionUp = new CCJumpBy (2, new CCPoint(0, 0), 80, 4);
            var actionByBack = actionBy.Reverse();

            m_tamara.RunAction(actionTo);
            m_grossini.RunAction(CCSequence.FromActions(actionBy, actionByBack));
            m_kathia.RunAction(new CCRepeatForever (actionUp));
        }

        public override string subtitle()
        {
            return "JumpTo / JumpBy";
        }
    };

    public class ActionBezier : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;

            //
            // startPosition can be any coordinate, but since the movement
            // is relative to the Bezier curve, make it (0,0)
            //

            centerSprites(3);

            // sprite 1
            ccBezierConfig bezier;
            bezier.ControlPoint1 = new CCPoint(0, s.Height / 2);
            bezier.ControlPoint2 = new CCPoint(300, -s.Height / 2);
            bezier.EndPosition = new CCPoint(300, 100);

            var bezierForward = new CCBezierBy (3, bezier);
            var bezierBack = bezierForward.Reverse();
            var rep = new CCRepeatForever ((CCActionInterval)CCSequence.FromActions(bezierForward, bezierBack));


            // sprite 2
            m_tamara.Position = new CCPoint(80, 160);
            ccBezierConfig bezier2;
            bezier2.ControlPoint1 = new CCPoint(100, s.Height / 2);
            bezier2.ControlPoint2 = new CCPoint(200, -s.Height / 2);
            bezier2.EndPosition = new CCPoint(240, 160);

            var bezierTo1 = new CCBezierTo (2, bezier2);

            // sprite 3
            m_kathia.Position = new CCPoint(400, 160);
            var bezierTo2 = new CCBezierTo (2, bezier2);

            m_grossini.RunAction(rep);
            m_tamara.RunAction(bezierTo1);
            m_kathia.RunAction(bezierTo2);
        }

        public override string subtitle()
        {
            return "BezierBy / BezierTo";
        }
    };

    public class ActionBlink : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(2);

            var action1 = new CCBlink (2, 10);
            var action2 = new CCBlink (2, 5);

            m_tamara.RunAction(action1);
            m_kathia.RunAction(action2);
        }

        public override string subtitle()
        {
            return "Blink";
        }
    };

    public class ActionFade : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(2);

            m_tamara.Opacity = 0;
            var action1 = new CCFadeIn  (1.0f);
            var action1Back = action1.Reverse();

            var action2 = new CCFadeOut  (1.0f);
            var action2Back = action2.Reverse();

            m_tamara.RunAction(CCSequence.FromActions(action1, action1Back));
            m_kathia.RunAction(CCSequence.FromActions(action2, action2Back));
        }

        public override string subtitle()
        {
            return "FadeIn / FadeOut";
        }
    };

    public class ActionTint : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(2);

            var action1 = new CCTintTo (2, 255, 0, 255);
            var action2 = new CCTintBy (2, -127, -255, -127);
            var action2Back = action2.Reverse();

            m_tamara.RunAction(action1);
            m_kathia.RunAction(CCSequence.FromActions(action2, action2Back));
        }

        public override string subtitle()
        {
            return "TintTo / TintBy";
        }
    };

    public class ActionAnimate : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(3);

            //
            // Manual animation
            //
            var animation = new CCAnimation();
            for (var i = 1; i < 15; i++)
            {
                var szName = String.Format("Images/grossini_dance_{0:00}", i);
                animation.AddSpriteFrameWithFileName(szName);
            }

            // should last 2.8 seconds. And there are 14 frames.
            animation.DelayPerUnit = 2.8f / 14.0f;
            animation.RestoreOriginalFrame = true;

            var action = new CCAnimate (animation);
            m_grossini.RunAction(CCSequence.FromActions(action, action.Reverse()));

            //
            // File animation
            //
            // With 2 loops and reverse
            var cache = CCAnimationCache.SharedAnimationCache;
            cache.AddAnimationsWithFile("animations/animations-2.plist");
            var animation2 = cache.AnimationByName("dance_1");

            var action2 = new CCAnimate (animation2);
            m_tamara.RunAction(CCSequence.FromActions(action2, action2.Reverse()));

            // TODO:
            //     observer_ = [[NSNotificationCenter defaultCenter] addObserverForName:CCAnimationFrameDisplayedNotification object:nil queue:nil usingBlock:^(NSNotification* notification) {
            // 
            //         NSDictionary *userInfo = [notification userInfo];
            //         NSLog(@"object %@ with data %@", [notification object], userInfo );
            //     }];


            //
            // File animation
            //
            // with 4 loops
            var animation3 = (CCAnimation) animation2.Copy();
            animation3.Loops = 4;


            var action3 = new CCAnimate (animation3);
            m_kathia.RunAction(action3);
        }

        public override string title()
        {
            return "Animation";
        }

        public override string subtitle()
        {
            return "Center: Manual animation. Border: using file format animation";
        }
    }

    public class ActionSequence : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            alignSpritesLeft(1);

            var action = CCSequence.FromActions(
                new CCMoveBy (2, new CCPoint(240, 0)),
                new CCRotateBy (2, 540));

            m_grossini.RunAction(action);
        }

        public override string subtitle()
        {
            return "Sequence: Move + Rotate";
        }
    };

    public class ActionSequence2 : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            alignSpritesLeft(1);

            m_grossini.Visible = false;

            var action = CCSequence.FromActions(
                new CCPlace(new CCPoint(200, 200)),
                new CCShow(),
                new CCMoveBy (1, new CCPoint(100, 0)),
                new CCCallFunc(callback1),
                new CCCallFuncN(callback2),
                new CCCallFuncND(callback3, 0xbebabeba));

            m_grossini.RunAction(action);
        }

        public void callback1()
        {
            var s = CCDirector.SharedDirector.WinSize;
            var label = new CCLabelTTF("callback 1 called", "arial", 16);
            label.Position = new CCPoint(s.Width / 4 * 1, s.Height / 2);

            AddChild(label);
        }

        public void callback2(CCNode sender)
        {
            var s = CCDirector.SharedDirector.WinSize;
            var label = new CCLabelTTF("callback 2 called", "arial", 16);
            label.Position = new CCPoint(s.Width / 4 * 2, s.Height / 2);

            AddChild(label);
        }

        public void callback3(CCNode sender, object data)
        {
            var s = CCDirector.SharedDirector.WinSize;
            var label = new CCLabelTTF("callback 3 called", "arial", 16);
            label.Position = new CCPoint(s.Width / 4 * 3, s.Height / 2);

            AddChild(label);
        }

        public override string subtitle()
        {
            return "Sequence of InstantActions";
        }
    };

    public class ActionCallFunc : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(3);

            var action = CCSequence.FromActions(
                new CCMoveBy (2, new CCPoint(200, 0)),
                new CCCallFunc(callback1));

            var action2 = CCSequence.FromActions(
                new CCScaleBy(2, 2),
                new CCFadeOut  (2),
                new CCCallFuncN(callback2));

            var action3 = CCSequence.FromActions(
                new CCRotateBy (3, 360),
                new CCFadeOut  (2),
                new CCCallFuncND(callback3, 0xbebabeba));

            m_grossini.RunAction(action);
            m_tamara.RunAction(action2);
            m_kathia.RunAction(action3);
        }


        public void callback1()
        {
            var s = CCDirector.SharedDirector.WinSize;
            var label = new CCLabelTTF("callback 1 called", "arial", 16);
            label.Position = new CCPoint(s.Width / 4 * 1, s.Height / 2);

            AddChild(label);
        }

        public void callback2(CCNode pSender)
        {
            var s = CCDirector.SharedDirector.WinSize;
            var label = new CCLabelTTF("callback 2 called", "arial", 16);
            label.Position = new CCPoint(s.Width / 4 * 2, s.Height / 2);

            AddChild(label);
        }

        public void callback3(CCNode target, object data)
        {
            var s = CCDirector.SharedDirector.WinSize;
            var label = new CCLabelTTF("callback 3 called", "arial", 16);
            label.Position = new CCPoint(s.Width / 4 * 3, s.Height / 2);
            AddChild(label);
        }

        public override string subtitle()
        {
            return "Callbacks: CallFunc and friends";
        }
    };

    public class ActionCallFuncND : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(1);

            var action = CCSequence.FromActions(
                new CCMoveBy (2.0f, new CCPoint(200, 0)),
                new CCCallFuncND(removeFromParentAndCleanup, true)
                );

            m_grossini.RunAction(action);
        }

        public override string title()
        {
            return "CallFuncND + auto remove";
        }

        public override string subtitle()
        {
            return "CallFuncND + removeFromParentAndCleanup. Grossini dissapears in 2s";
        }

        private void removeFromParentAndCleanup(CCNode pSender, object data)
        {
            var bCleanUp = (bool) data;
            m_grossini.RemoveFromParentAndCleanup(bCleanUp);
        }
    }

    public class ActionSpawn : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            alignSpritesLeft(1);

            var action = CCSpawn.FromActions(
                new CCJumpBy (2, new CCPoint(300, 0), 50, 4),
                new CCRotateBy (2, 720));

            m_grossini.RunAction(action);
        }

        public override string subtitle()
        {
            return "Spawn: Jump + Rotate";
        }
    };

    public class ActionRepeatForever : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(1);

            var action = CCSequence.FromActions(
                new CCDelayTime (1),
                new CCCallFuncN(repeatForever));

            m_grossini.RunAction(action);
        }

        public void repeatForever(CCNode pSender)
        {
            var repeat = new CCRepeatForever (new CCRotateBy (1.0f, 360));

            pSender.RunAction(repeat);
        }

        public override string subtitle()
        {
            return "CallFuncN + RepeatForever";
        }
    };

    public class ActionRotateToRepeat : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(2);

            var act1 = new CCRotateTo (1, 90);
            var act2 = new CCRotateTo (1, 0);
            var seq = (CCSequence.FromActions(act1, act2));
            var rep1 = new CCRepeatForever ((CCActionInterval)seq);
            var rep2 = new CCRepeat ((CCFiniteTimeAction)(seq.Copy()), 10);

            m_tamara.RunAction(rep1);
            m_kathia.RunAction(rep2);
        }

        public override string subtitle()
        {
            return "Repeat/RepeatForever + RotateTo";
        }
    };

    public class ActionRotateJerk : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(2);

            var seq = CCSequence.FromActions(
                new CCRotateTo (0.5f, -20),
                new CCRotateTo (0.5f, 20));

            var rep1 = new CCRepeat (seq, 10);
            var rep2 = new CCRepeatForever ((CCActionInterval)(seq.Copy()));

            m_tamara.RunAction(rep1);
            m_kathia.RunAction(rep2);
        }

        public override string subtitle()
        {
            return "RepeatForever / Repeat + Rotate";
        }
    };

    public class ActionReverse : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            alignSpritesLeft(1);

            var jump = new CCJumpBy (2, new CCPoint(300, 0), 50, 4);
            var action = CCSequence.FromActions(jump, jump.Reverse());

            m_grossini.RunAction(action);
        }

        public override string subtitle()
        {
            return "Reverse an action";
        }
    };

    public class ActionDelayTime : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            alignSpritesLeft(1);

            var move = new CCMoveBy (1, new CCPoint(150, 0));
            var action = CCSequence.FromActions(move, new CCDelayTime (2), move);

            m_grossini.RunAction(action);
        }

        public override string subtitle()
        {
            return "DelayTime: m + delay + m";
        }
    };

    public class ActionReverseSequence : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            alignSpritesLeft(1);

            var move1 = new CCMoveBy (1, new CCPoint(250, 0));
            var move2 = new CCMoveBy (1, new CCPoint(0, 50));
            var seq = CCSequence.FromActions(move1, move2, move1.Reverse());
            var action = CCSequence.FromActions(seq, seq.Reverse());

            m_grossini.RunAction(action);
        }

        public override string subtitle()
        {
            return "Reverse a sequence";
        }
    };

    public class ActionReverseSequence2 : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            alignSpritesLeft(2);

            // Test:
            //   Sequence should work both with IntervalAction and InstantActions
            var move1 = new CCMoveBy (1, new CCPoint(250, 0));
            var move2 = new CCMoveBy (1, new CCPoint(0, 50));
            var tog1 = new CCToggleVisibility();
            var tog2 = new CCToggleVisibility();
            var seq = CCSequence.FromActions(move1, tog1, move2, tog2, move1.Reverse());
            var action = new CCRepeat ((CCSequence.FromActions(seq, seq.Reverse())), 3);

            // Test:
            //   Also test that the reverse of Hide is Show, and vice-versa
            m_kathia.RunAction(action);

            var move_tamara = new CCMoveBy (1, new CCPoint(100, 0));
            var move_tamara2 = new CCMoveBy (1, new CCPoint(50, 0));
            var hide = new CCHide();
            var seq_tamara = CCSequence.FromActions(move_tamara, hide, move_tamara2);
            var seq_back = seq_tamara.Reverse();
            m_tamara.RunAction(CCSequence.FromActions(seq_tamara, seq_back));
        }

        public override string subtitle()
        {
            return "Reverse sequence 2";
        }
    }

    public class ActionRepeat : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            alignSpritesLeft(2);

            var a1 = new CCMoveBy (1, new CCPoint(150, 0));
            var action1 = new CCRepeat (
                CCSequence.FromActions(new CCPlace(new CCPoint(60, 60)), a1),
                3);
            var action2 = new CCRepeatForever (
                (CCSequence.FromActions((CCActionInterval) (a1.Copy()), a1.Reverse()))
                );

            m_kathia.RunAction(action1);
            m_tamara.RunAction(action2);
        }

        public override string subtitle()
        {
            return "Repeat / RepeatForever actions";
        }
    };

    public class ActionOrbit : ActionsDemo
    {
        public override void OnEnter()
        {
            // todo : CCOrbitCamera hasn't been implement

            base.OnEnter();

            centerSprites(3);

            var orbit1 = new CCOrbitCamera(2, 1, 0, 0, 180, 0, 0);
            var action1 = CCSequence.FromActions(orbit1,orbit1.Reverse());

            var orbit2 = new CCOrbitCamera(2, 1, 0, 0, 180, -45, 0);
            var action2 = CCSequence.FromActions(orbit2, orbit2.Reverse());

            var orbit3 = new CCOrbitCamera(2, 1, 0, 0, 180, 90, 0);
            var action3 = CCSequence.FromActions(orbit3, orbit3.Reverse());

            m_kathia.RunAction(new CCRepeatForever (action1));
            m_tamara.RunAction(new CCRepeatForever (action2));
            m_grossini.RunAction(new CCRepeatForever (action3));

            var move = new CCMoveBy (3, new CCPoint(100, -100));
            var move_back = move.Reverse();
            var seq = CCSequence.FromActions(move, move_back);
            var rfe = new CCRepeatForever (seq);
            m_kathia.RunAction(rfe);
            m_tamara.RunAction((CCAction) (rfe.Copy()));
            m_grossini.RunAction((CCAction) (rfe.Copy()));
        }

        public override string subtitle()
        {
            return "OrbitCamera action";
        }
    };

    public class ActionFollow : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(1);
            var s = CCDirector.SharedDirector.WinSize;

            m_grossini.Position = new CCPoint(-200, s.Height / 2);
            var move = new CCMoveBy (2, new CCPoint(s.Width * 3, 0));
            var move_back = move.Reverse();
            var seq = CCSequence.FromActions(move, move_back);
            var rep = new CCRepeatForever (seq);

            m_grossini.RunAction(rep);

            RunAction(new CCFollow (m_grossini, new CCRect(0, 0, s.Width * 2 - 100, s.Height)));
        }

        public override string subtitle()
        {
            return "Follow action";
        }
    }


    public class ActionCardinalSpline : ActionsDemo
    {
        private readonly List<CCPoint> m_pArray = new List<CCPoint>();

        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(2);

            var s = CCDirector.SharedDirector.WinSize;

            m_pArray.Add(new CCPoint(0, 0));
            m_pArray.Add(new CCPoint(s.Width / 2 - 30, 0));
            m_pArray.Add(new CCPoint(s.Width / 2 - 30, s.Height - 80));
            m_pArray.Add(new CCPoint(0, s.Height - 80));
            m_pArray.Add(new CCPoint(0, 0));

            //
            // sprite 1 (By)
            //
            // Spline with no tension (tension==0)
            //

            var action = new CCCardinalSplineBy (3, m_pArray, 0);
            var reverse = action.Reverse();

            var seq = CCSequence.FromActions(action, reverse);

            m_tamara.Position = new CCPoint(50, 50);
            m_tamara.RunAction(seq);

            //
            // sprite 2 (By)
            //
            // Spline with high tension (tension==1)
            //

            var action2 = new CCCardinalSplineBy (3, m_pArray, 1);
            var reverse2 = action2.Reverse();

            var seq2 = CCSequence.FromActions(action2, reverse2);

            m_kathia.SetPosition(s.Width / 2, 50);
            m_kathia.RunAction(seq2);
        }

        public override void Draw()
        {
            base.Draw();

            // move to 50,50 since the "by" path will start at 50,50
            CCDrawManager.PushMatrix();
            CCDrawManager.Translate(50, 50, 0);
            CCDrawingPrimitives.DrawCardinalSpline(m_pArray, 0, 100);
            CCDrawManager.PopMatrix();

            var s = CCDirector.SharedDirector.WinSize;

            CCDrawManager.PushMatrix();
            CCDrawManager.Translate(s.Width / 2, 50, 0);
            CCDrawingPrimitives.DrawCardinalSpline(m_pArray, 1, 100);
            CCDrawManager.PopMatrix();
        }

        public override string title()
        {
            return "CardinalSplineBy / CardinalSplineAt";
        }

        public override string subtitle()
        {
            return "Cardinal Spline paths. Testing different tensions for one array";
        }
    }

    public class ActionCatmullRom : ActionsDemo
    {
        private readonly List<CCPoint> m_pArray = new List<CCPoint>();
        private readonly List<CCPoint> m_pArray2 = new List<CCPoint>();

        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(2);

            var s = CCDirector.SharedDirector.WinSize;

            //
            // sprite 1 (By)
            //
            // startPosition can be any coordinate, but since the movement
            // is relative to the Catmull Rom curve, it is better to start with (0,0).
            //

            m_tamara.Position = new CCPoint(50, 50);

            m_pArray.Clear();

            m_pArray.Add(new CCPoint(0, 0));
            m_pArray.Add(new CCPoint(80, 80));
            m_pArray.Add(new CCPoint(s.Width - 80, 80));
            m_pArray.Add(new CCPoint(s.Width - 80, s.Height - 80));
            m_pArray.Add(new CCPoint(80, s.Height - 80));
            m_pArray.Add(new CCPoint(80, 80));
            m_pArray.Add(new CCPoint(s.Width / 2, s.Height / 2));

            var action = new CCCatmullRomBy (3, m_pArray);
            var reverse = action.Reverse();

            CCFiniteTimeAction seq = CCSequence.FromActions(action, reverse);

            m_tamara.RunAction(seq);

            //
            // sprite 2 (To)
            //
            // The startPosition is not important here, because it uses a "To" action.
            // The initial position will be the 1st point of the Catmull Rom path
            //    

            m_pArray2.Clear();

            m_pArray2.Add(new CCPoint(s.Width / 2, 30));
            m_pArray2.Add(new CCPoint(s.Width - 80, 30));
            m_pArray2.Add(new CCPoint(s.Width - 80, s.Height - 80));
            m_pArray2.Add(new CCPoint(s.Width / 2, s.Height - 80));
            m_pArray2.Add(new CCPoint(s.Width / 2, 30));

            var action2 = new CCCatmullRomTo (3, m_pArray2);
            var reverse2 = action2.Reverse();

            CCFiniteTimeAction seq2 = CCSequence.FromActions(action2, reverse2);

            m_kathia.RunAction(seq2);
        }

        public override void Draw()
        {
            base.Draw();

            // move to 50,50 since the "by" path will start at 50,50
            CCDrawManager.PushMatrix();
            CCDrawManager.Translate(50, 50, 0);
            CCDrawingPrimitives.DrawCatmullRom(m_pArray, 50);
            CCDrawManager.PopMatrix();

            CCDrawingPrimitives.DrawCatmullRom(m_pArray2, 50);
        }

        public override string title()
        {
            return "CatmullRomBy / CatmullRomTo";
        }

        public override string subtitle()
        {
            return "Catmull Rom spline paths. Testing reverse too";
        }
    }

    public class ActionTargeted : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(2);

            var jump1 = new CCJumpBy (2, CCPoint.Zero, 100, 3);
            var jump2 = (CCJumpBy) jump1.Copy();
            var rot1 = new CCRotateBy (1, 360);
            var rot2 = (CCRotateBy) rot1.Copy();

            var t1 = new CCTargetedAction (m_kathia, jump2);
            var t2 = new CCTargetedAction (m_kathia, rot2);


            var seq = CCSequence.FromActions(jump1, t1, rot1, t2);
            var always = new CCRepeatForever (seq);

            m_tamara.RunAction(always);
        }

        public override string title()
        {
            return "ActionTargeted";
        }

        public override string subtitle()
        {
            return "Action that runs on another target. Useful for sequences";
        }
    }


    public class Issue1305 : ActionsDemo
    {
        private CCSprite m_pSpriteTmp;

        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(0);

            m_pSpriteTmp = new CCSprite("Images/grossini");
            /* c++ can't support block, so we use CCCallFuncN instead.
            [spriteTmp_ runAction:[CCCallBlockN actionWithBlock:^(CCNode* node) {
                NSLog(@"This message SHALL ONLY appear when the sprite is added to the scene, NOT BEFORE");
            }] ];
            */

            m_pSpriteTmp.RunAction(new CCCallFuncN(log));

            ScheduleOnce(addSprite, 2);
        }

        private void log(CCNode pSender)
        {
            CCLog.Log("This message SHALL ONLY appear when the sprite is added to the scene, NOT BEFORE");
        }

        private void addSprite(float dt)
        {
            m_pSpriteTmp.Position = new CCPoint(250, 250);
            AddChild(m_pSpriteTmp);
        }

        public override string title()
        {
            return "Issue 1305";
        }

        public override string subtitle()
        {
            return "In two seconds you should see a message on the console. NOT BEFORE.";
        }
    }

    public class Issue1305_2 : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(0);

            var spr = new CCSprite("Images/grossini");
            spr.Position = new CCPoint(200, 200);
            AddChild(spr);

            var act1 = new CCMoveBy (2, new CCPoint(0, 100));

            var act2 = new CCCallFunc(log1);
            var act3 = new CCMoveBy (2, new CCPoint(0, -100));
            var act4 = new CCCallFunc(log2);
            var act5 = new CCMoveBy (2, new CCPoint(100, -100));
            var act6 = new CCCallFunc(log3);
            var act7 = new CCMoveBy (2, new CCPoint(-100, 0));
            var act8 = new CCCallFunc(log4);

            var actF = CCSequence.FromActions(act1, act2, act3, act4, act5, act6, act7, act8);

            CCDirector.SharedDirector.ActionManager.AddAction(actF, spr, false);
        }

        private void log4()
        {
            CCLog.Log("4st block");
        }

        private void log3()
        {
            CCLog.Log("3st block");
        }

        private void log2()
        {
            CCLog.Log("2st block");
        }

        private void log1()
        {
            CCLog.Log("1st block");
        }


        public override string title()
        {
            return "Issue 1305 #2";
        }

        public override string subtitle()
        {
            return "See console. You should only see one message for each block";
        }
    }

    public class Issue1288 : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(0);

            var spr = new CCSprite("Images/grossini");
            spr.Position = new CCPoint(100, 100);
            AddChild(spr);

            var act1 = new CCMoveBy (0.5f, new CCPoint(100, 0));
            var act2 = (CCMoveBy) act1.Reverse();
            var act3 = CCSequence.FromActions(act1, act2);
            var act4 = new CCRepeat (act3, 2);

            spr.RunAction(act4);
        }

        public override string title()
        {
            return "Issue 1288";
        }

        public override string subtitle()
        {
            return "Sprite should end at the position where it started.";
        }
    }

    public class Issue1288_2 : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(0);

            var spr = new CCSprite("Images/grossini");
            spr.Position = new CCPoint(100, 100);
            AddChild(spr);

            var act1 = new CCMoveBy (0.5f, new CCPoint(100, 0));
            spr.RunAction(new CCRepeat (act1, 1));
        }

        public override string title()
        {
            return "Issue 1288 #2";
        }

        public override string subtitle()
        {
            return "Sprite should move 100 pixels, and stay there";
        }
    }

    public class Issue1327 : ActionsDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(0);

            var spr = new CCSprite("Images/grossini");
            spr.Position = new CCPoint(100, 100);
            AddChild(spr);

            var act1 = new CCCallFuncN(logSprRotation);
            var act2 = new CCRotateBy (0.25f, 45);
            var act3 = new CCCallFuncN(logSprRotation);
            var act4 = new CCRotateBy (0.25f, 45);
            var act5 = new CCCallFuncN(logSprRotation);
            var act6 = new CCRotateBy (0.25f, 45);
            var act7 = new CCCallFuncN(logSprRotation);
            var act8 = new CCRotateBy (0.25f, 45);
            var act9 = new CCCallFuncN(logSprRotation);

            var actF = CCSequence.FromActions(act1, act2, act3, act4, act5, act6, act7, act8, act9);
            spr.RunAction(actF);
        }

        private void logSprRotation(CCNode sender)
        {
            CCLog.Log("{0}", sender.Rotation);
        }

        public override string title()
        {
            return "Issue 1327";
        }

        public override string subtitle()
        {
            return "See console: You should see: 0, 45, 90, 135, 180";
        }
    }

    public class PauseResumeActions : ActionsDemo
    {
        private List<object> m_pPausedTargets;

        public override void OnEnter()
        {
            base.OnEnter();

            centerSprites(2);

            m_tamara.RunAction(new CCRepeatForever (new CCRotateBy (3, 360)));
            m_grossini.RunAction(new CCRepeatForever (new CCRotateBy (3, -360)));
            m_kathia.RunAction(new CCRepeatForever (new CCRotateBy (3, 360)));

            ScheduleOnce(pause, 3);
            ScheduleOnce(resume, 5);
        }

        public override string title()
        {
            return "PauseResumeActions";
        }

        public override string subtitle()
        {
            return "All actions pause at 3s and resume at 5s";
        }

        private void pause(float dt)
        {
            CCLog.Log("Pausing");
            var director = CCDirector.SharedDirector;

            m_pPausedTargets = director.ActionManager.PauseAllRunningActions();
        }

        private void resume(float dt)
        {
            CCLog.Log("Resuming");
            var director = CCDirector.SharedDirector;
            director.ActionManager.ResumeTargets(m_pPausedTargets);
        }
    }
}