using System;
using System.Collections.Generic;
using CocosSharp;

namespace tests
{
    public enum ActionTest
    {
        ACTION_MANUAL_LAYER = 0,
        ACTION_MOVE_LAYER,
        ACTION_SCALE_LAYER,
        ACTION_ROTATE_LAYER,
        ACTION_SKEW_LAYER,
        ACTION_ROTATIONAL_SKEW_LAYER,
        ACTION_COMPARISON_SKEW_LAYER,
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
        ACTION_RemoveSelfActions,
        ACTION_ORBIT_LAYER,
        ACTION_FLLOW_LAYER,
        ACTION_TARGETED_LAYER,
        PAUSERESUMEACTIONS_LAYER,
        ACTION_ISSUE1305_LAYER,
        ACTION_ISSUE1305_2_LAYER,
        ACTION_ISSUE1288_LAYER,
        ACTION_ISSUE1288_2_LAYER,
        ACTION_ISSUE1327_LAYER,
        ACTION_ISSUE1389_LAYER,
        ACTION_ActionMoveStacked,
        ACTION_ActionMoveJumpStacked,
        ACTION_ActionMoveBezierStacked,
        ACTION_ActionCatmullRomStacked,
        ACTION_ActionCardinalSplineStacked,
        ACTION_PARALLEL,
        ACTION_LAYER_COUNT
    };


    // the class inherit from TestScene
    // every Scene each test used must inherit from TestScene,
    // make sure the test have the menu item for back to main menu
    public class ActionsTestScene : TestScene
    {
        static int actionIdx = -1;

        public static CCLayer CreateLayer(int index)
        {
            CCLayer layer = null;

            switch (index)
            {
                case (int) ActionTest.ACTION_MANUAL_LAYER:
                    layer = new ActionManual();
                    break;
                case (int) ActionTest.ACTION_MOVE_LAYER:
                    layer = new ActionMove();
                    break;
                case (int) ActionTest.ACTION_SCALE_LAYER:
                    layer = new ActionScale();
                    break;
                case (int) ActionTest.ACTION_ROTATE_LAYER:
                    layer = new ActionRotate();
                    break;
                case (int) ActionTest.ACTION_SKEW_LAYER:
                    layer = new ActionSkew();
                    break;
                case (int) ActionTest.ACTION_ROTATIONAL_SKEW_LAYER:
                    layer = new ActionRotationalSkew();
                    break;
                case (int) ActionTest.ACTION_COMPARISON_SKEW_LAYER:
                    layer = new ActionRotationalSkewVSStandardSkew();
                    break;
                case (int) ActionTest.ACTION_SKEWROTATE_LAYER:
                    layer = new ActionSkewRotateScale();
                    break;
                case (int) ActionTest.ACTION_JUMP_LAYER:
                    layer = new ActionJump();
                    break;
                case (int) ActionTest.ACTION_BEZIER_LAYER:
                    layer = new ActionBezier();
                    break;
                case (int) ActionTest.ACTION_BLINK_LAYER:
                    layer = new ActionBlink();
                    break;
                case (int) ActionTest.ACTION_FADE_LAYER:
                    layer = new ActionFade();
                    break;
                case (int) ActionTest.ACTION_TINT_LAYER:
                    layer = new ActionTint();
                    break;
                case (int) ActionTest.ACTION_ANIMATE_LAYER:
                    layer = new ActionAnimate();
                    break;
                case (int) ActionTest.ACTION_SEQUENCE_LAYER:
                    layer = new ActionSequence();
                    break;
                case (int) ActionTest.ACTION_SEQUENCE2_LAYER:
                    layer = new ActionSequence2();
                    break;
                case (int) ActionTest.ACTION_SPAWN_LAYER:
                    layer = new ActionSpawn();
                    break;
                case (int) ActionTest.ACTION_REVERSE:
                    layer = new ActionReverse();
                    break;
                case (int) ActionTest.ACTION_DELAYTIME_LAYER:
                    layer = new ActionDelayTime();
                    break;
                case (int) ActionTest.ACTION_REPEAT_LAYER:
                    layer = new ActionRepeat();
                    break;
                case (int) ActionTest.ACTION_REPEATEFOREVER_LAYER:
                    layer = new ActionRepeatForever();
                    break;
                case (int) ActionTest.ACTION_ROTATETOREPEATE_LAYER:
                    layer = new ActionRotateToRepeat();
                    break;
                case (int) ActionTest.ACTION_ROTATEJERK_LAYER:
                    layer = new ActionRotateJerk();
                    break;
                case (int) ActionTest.ACTION_CALLFUNC_LAYER:
                    layer = new ActionCallFunc();
                    break;
                case (int) ActionTest.ACTION_CALLFUNCND_LAYER:
                    layer = new ActionCallFuncND();
                    break;
                case (int) ActionTest.ACTION_REVERSESEQUENCE_LAYER:
                    layer = new ActionReverseSequence();
                    break;
                case (int) ActionTest.ACTION_REVERSESEQUENCE2_LAYER:
                    layer = new ActionReverseSequence2();
                    break;
                case (int)ActionTest.ACTION_RemoveSelfActions:
                    layer = new RemoveSelfActions();
                    break;
                case (int) ActionTest.ACTION_ORBIT_LAYER:
                    layer = new ActionOrbit();
                    break;
                case (int) ActionTest.ACTION_FLLOW_LAYER:
                    layer = new ActionFollow();
                    break;
                case (int) ActionTest.ACTION_TARGETED_LAYER:
                    layer = new ActionTargeted();
                    break;
                case (int) ActionTest.ACTION_ISSUE1305_LAYER:
                    layer = new Issue1305();
                    break;
                case (int) ActionTest.ACTION_ISSUE1305_2_LAYER:
                    layer = new Issue1305_2();
                    break;
                case (int) ActionTest.ACTION_ISSUE1288_LAYER:
                    layer = new Issue1288();
                    break;
                case (int) ActionTest.ACTION_ISSUE1288_2_LAYER:
                    layer = new Issue1288_2();
                    break;
                case (int) ActionTest.ACTION_ISSUE1327_LAYER:
                    layer = new Issue1327();
                    break;
                case (int)ActionTest.ACTION_ISSUE1389_LAYER:
                    layer = new Issue1389();
                    break;
                case (int) ActionTest.ACTION_CARDINALSPLINE_LAYER:
                    layer = new ActionCardinalSpline();
                    break;
                case (int) ActionTest.ACTION_CATMULLROM_LAYER:
                    layer = new ActionCatmullRom();
                    break;
                case (int) ActionTest.PAUSERESUMEACTIONS_LAYER:
                    layer = new PauseResumeActions();
                    break;
                case (int)ActionTest.ACTION_ActionMoveStacked:
                    layer = new ActionMoveStacked();
                    break;
                case (int)ActionTest.ACTION_ActionMoveJumpStacked:
                    layer = new ActionMoveJumpStacked();
                    break;
                case (int)ActionTest.ACTION_ActionMoveBezierStacked:
                    layer = new ActionMoveBezierStacked();
                    break;
                case (int)ActionTest.ACTION_ActionCatmullRomStacked:
                    layer = new ActionCatmullRomStacked();
                    break;
                case (int)ActionTest.ACTION_ActionCardinalSplineStacked:
                    layer = new ActionCardinalSplineStacked();
                    break;
                case (int)ActionTest.ACTION_PARALLEL:
                    layer = new ActionParallel();
                    break;
                default:
                    break;
            }

            layer.Camera = AppDelegate.SharedCamera;

            return layer;
        }

        public static CCLayer NextAction()
        {
            ++actionIdx;
            actionIdx = actionIdx % (int) ActionTest.ACTION_LAYER_COUNT;

            var layer = CreateLayer(actionIdx);

            return layer;
        }

        public static CCLayer BackAction()
        {
            --actionIdx;
            if (actionIdx < 0)
                actionIdx += (int) ActionTest.ACTION_LAYER_COUNT;

            var layer = CreateLayer(actionIdx);

            return layer;
        }

        public static CCLayer RestartAction()
        {
            var layer = CreateLayer(actionIdx);

            return layer;
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

        public override void runThisTest()
        {
            actionIdx = -1;
            AddChild(NextAction());

            Director.ReplaceScene(this);
        }
    }


    public class ActionsDemo : TestNavigationLayer
    {
        protected CCSprite Grossini;
        protected CCSprite Kathia;
        protected CCSprite Tamara;


        #region Properties

        public override string Title
        {
            get { return "ActionsTest"; }
        }

        #endregion Properties


        #region Constructors

        public ActionsDemo()
        {
            Grossini = new CCSprite(TestResource.s_pPathGrossini);
            Tamara = new CCSprite(TestResource.s_pPathSister1);
            Kathia = new CCSprite(TestResource.s_pPathSister2);

            AddChild(Grossini, 1);
            AddChild(Tamara, 2);
            AddChild(Kathia, 3);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); 
            CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            Grossini.Position = new CCPoint(windowSize.Width / 2, windowSize.Height / 3);
            Tamara.Position = new CCPoint(windowSize.Width / 2, 2 * windowSize.Height / 3);
            Kathia.Position = new CCPoint(windowSize.Width / 2, windowSize.Height / 2);
        }
            
        #endregion Setup content


        #region Callbacks

        public override void RestartCallback(object sender)
        {
            var s = new ActionsTestScene();
            s.AddChild(ActionsTestScene.RestartAction());
            Director.ReplaceScene(s);
        }

        public override void NextCallback(object sender)
        {
            var s = new ActionsTestScene();
            s.AddChild(ActionsTestScene.NextAction());
            Director.ReplaceScene(s);
        }

        public override void BackCallback(object sender)
        {
            var s = new ActionsTestScene();
            s.AddChild(ActionsTestScene.BackAction());
            Director.ReplaceScene(s);
        }

        #endregion Callbacks


        protected void CenterSprites(uint numberOfSprites)
        {
            var s = Layer.VisibleBoundsWorldspace.Size;

            if (numberOfSprites == 0)
            {
                Tamara.Visible = false;
                Kathia.Visible = false;
                Grossini.Visible = false;
            }
            else if (numberOfSprites == 1)
            {
                Tamara.Visible = false;
                Kathia.Visible = false;
                Grossini.Position = new CCPoint(s.Width / 2, s.Height / 2);
            }
            else if (numberOfSprites == 2)
            {
                Kathia.Position = new CCPoint(s.Width / 3, s.Height / 2);
                Tamara.Position = new CCPoint(2 * s.Width / 3, s.Height / 2);
                Grossini.Visible = false;
            }
            else if (numberOfSprites == 3)
            {
                Grossini.Position = new CCPoint(s.Width / 2, s.Height / 2);
                Tamara.Position = new CCPoint(s.Width / 4, s.Height / 2);
                Kathia.Position = new CCPoint(3 * s.Width / 4, s.Height / 2);
            }
        }

        protected void AlignSpritesLeft(uint numberOfSprites)
        {
            var s = Layer.VisibleBoundsWorldspace.Size;

            if (numberOfSprites == 1)
            {
                Tamara.Visible = false;
                Kathia.Visible = false;
                Grossini.Position = new CCPoint(60, s.Height / 2);
            }
            else if (numberOfSprites == 2)
            {
                Kathia.Position = new CCPoint(60, s.Height / 3);
                Tamara.Position = new CCPoint(60, 2 * s.Height / 3);
                Grossini.Visible = false;
            }
            else if (numberOfSprites == 3)
            {
                Grossini.Position = new CCPoint(60, s.Height / 2);
                Tamara.Position = new CCPoint(60, 2 * s.Height / 3);
                Kathia.Position = new CCPoint(60, s.Height / 3);
            }
        }
    }


    public class ActionParallel : ActionsDemo
    {
        #region Properties

        public override string Title
        {
            get { return ("CCParallel Test"); }
        }
        public override string Subtitle
        {
            get { return ("Tamara - parallel move to and fade in."); }
        }

        #endregion Properties


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); 
            CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(3);

            var actionTo = new CCMoveTo(2, new CCPoint(windowSize.Width - 40, windowSize.Height - 40));
            var actionBy = new CCMoveBy(2, new CCPoint(80, 80));
            var actionByBack = actionBy.Reverse();

            Tamara.RunAction(new CCSequence(new CCParallel(actionTo, new CCFadeIn(2)), actionBy, actionByBack));
            Grossini.RunAction(new CCSequence(actionBy, new CCParallel(actionByBack, new CCScaleTo(2, 0.25f))));
            Kathia.RunAction(new CCMoveTo(1, new CCPoint(40, 40)));
        }

        #endregion Setup content
    }


    public class ActionManual : ActionsDemo
    {
        #region Properties

        public override string Subtitle
        {
            get { return "Manual Transformation"; }
        }

        #endregion Properties


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            Tamara.ScaleX = 2.5f;
            Tamara.ScaleY = -1.0f;
            Tamara.Position = new CCPoint(100, 70);
            Tamara.Opacity = 128;

            Grossini.Rotation = 120;
            Grossini.Position = new CCPoint(windowSize.Width / 2, windowSize.Height / 2);
            Grossini.Color = new CCColor3B(255, 0, 0);

            Kathia.Position = new CCPoint(windowSize.Width - 100, windowSize.Height / 2);
            Kathia.Color = new CCColor3B(0, 0, 255);
        }

        #endregion Setup content
    }


    public class ActionMove : ActionsDemo
    {
        #region Properties

        public override string Subtitle
        {
            get { return "MoveTo / MoveBy"; }
        }

        #endregion Properties


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(3);

            var actionTo = new CCMoveTo (2, new CCPoint(windowSize.Width - 40, windowSize.Height - 40));
            var actionBy = new CCMoveBy (2, new CCPoint(80, 80));
            var actionByBack = actionBy.Reverse();

            Tamara.RunAction(new CCSequence(actionTo, new CCCallFunc(new Action(() =>
                {
                    if ((Tamara.Position.X != windowSize.Width - 40) || (Tamara.Position.Y != windowSize.Height - 40))
                    {
                        CCLog.Log("ERROR: MoveTo on tamara has failed. Position of tamara = {0}, expected = {1},{2}", Tamara.Position, windowSize.Width - 40, windowSize.Height - 40);
                    }
                }))));
            Grossini.RunAction(new CCSequence(actionBy, actionByBack));
            Kathia.RunAction(new CCSequence(new CCMoveTo(1, new CCPoint(40, 40)), new CCCallFunc(new Action(() =>
                {
                    if (Kathia.Position.X != 40 || Kathia.Position.Y != 40)
                    {
                        CCLog.Log("ERROR: MoveTo on kathia failed. Expected 40,40 but ended at {0}", Kathia.Position);
                    }
                }))));
        }

        #endregion Setup content
    }


    public class ActionScale : ActionsDemo
    {
        #region Properties

        public override string Subtitle
        {
            get { return "ScaleTo / ScaleBy"; }
        }

        #endregion Properties


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(3);

            var actionTo = new CCScaleTo(2, 0.5f);
            var actionBy = new CCScaleBy(2, 1, 10);
            var actionBy2 = new CCScaleBy(2, 5f, 1.0f);

            Grossini.RunAction(actionTo);
            Tamara.RunAction(new CCSequence(actionBy, actionBy.Reverse()));
            Kathia.RunAction(new CCSequence(actionBy2, actionBy2.Reverse()));
        }

        #endregion Setup content
    }


    public class ActionSkew : ActionsDemo
    {
        #region Properties

        public override string Subtitle
        {
            get { return "SkewTo / SkewBy"; }
        }

        #endregion Properties


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(3);

            var actionTo = new CCSkewTo (2, 37.2f, -37.2f);
            var actionToBack = new CCSkewTo (2, 0, 0);
            var actionBy = new CCSkewBy (2, 0.0f, -90.0f);
            var actionBy2 = new CCSkewBy (2, 45.0f, 45.0f);
            var actionByBack = actionBy.Reverse();

            Tamara.RunActions(actionTo, actionToBack);
            Grossini.RunActions(actionBy, actionByBack);

            Kathia.RunActions(actionBy2, actionBy2.Reverse());
        }

        #endregion Setup content
    }


    public class ActionRotationalSkew : ActionsDemo
    {
        #region Properties

        public override string Subtitle
        {
            get { return "RotationalSkewTo / RotationalSkewBy"; }
        }

        #endregion Properties


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(3);

            var actionTo = new CCRotateTo(2, 180, 180);
            var actionToBack = new CCRotateTo(2, 0, 0);
            var actionBy = new CCRotateBy(2, 0.0f, 360);
            var actionByBack = actionBy.Reverse();

            var actionBy2 = new CCRotateBy(2, 360, 0.0f);
            var actionBy2Back = actionBy2.Reverse ();

            Tamara.RunActions(actionBy, actionByBack);
            Grossini.RunActions(actionTo, actionToBack);
            Kathia.RunActions(actionBy2, actionBy2Back);
        }

        #endregion Setup content
    }


    public class ActionRotationalSkewVSStandardSkew : ActionsDemo
    {
        CCDrawNode box1;
        CCDrawNode box2;

        CCLabelTtf boxLabel1;
        CCLabelTtf boxLabel2;

        CCSkewBy actionTo;
        CCSkewBy actionToBack;

        CCRotateBy actionTo2;
        CCRotateBy actionToBack2;


        #region Properties

        public override string Subtitle
        {
            get { return "Skew Comparison"; }
        }

        #endregion Properties


        #region Constructors

        public ActionRotationalSkewVSStandardSkew()
        {
            box1 = new CCDrawNode ();
            box1.DrawRect(new CCRect (0.0f, 0.0f, 200.0f, 200.0f), new CCColor4B(255, 255, 0, 255));
            //new CCLayerColor();
            this.AddChild(box1);

            box1.AnchorPoint = new CCPoint(0.5f, 0.5f);
            box1.IgnoreAnchorPointForPosition = false;

            box2 = new CCDrawNode ();
            box2.DrawRect(new CCRect (0.0f, 0.0f, 200.0f, 200.0f), new CCColor4B(255, 255, 0, 255));
            this.AddChild(box2);

            box2.AnchorPoint = new CCPoint(0.5f, 0.5f);
            box2.IgnoreAnchorPointForPosition = false;

            boxLabel1 = new CCLabelTtf("Standard cocos2d Skew", "Marker Felt", 16);
            this.AddChild(boxLabel1);

            boxLabel2 = new CCLabelTtf("Rotational Skew", "Marker Felt", 16);
            this.AddChild(boxLabel2);

            actionTo = new CCSkewBy(2, 360, 0);
            actionToBack = new CCSkewBy(2, -360, 0);

            actionTo2 = new CCRotateBy(2, 360, 0);
            actionToBack2 = new CCRotateBy(2, -360, 0);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            Tamara.RemoveFromParent(true);
            Grossini.RemoveFromParent(true);
            Kathia.RemoveFromParent(true);

            CCSize boxSize = Layer.ScreenToWorldspace(new CCSize(200.0f, 200.0f));

            box1.ContentSize = boxSize;
            box1.Position = new CCPoint(windowSize.Width / 2, windowSize.Height - 100 - box1.ContentSize.Height / 2);
            box1.RunAction(new CCSequence(actionTo, actionToBack));

            box2.ContentSize = boxSize;
            box2.Position = new CCPoint(windowSize.Width / 2, windowSize.Height - 250 - box2.ContentSize.Height / 2);
            box2.RunAction(new CCSequence(actionTo2, actionToBack2));

            boxLabel1.Position = new CCPoint(windowSize.Width / 2, windowSize.Height - 100 + boxLabel1.ContentSize.Height);
            boxLabel2.Position = new CCPoint(windowSize.Width / 2, windowSize.Height - 250 + boxLabel2.ContentSize.Height / 2);
        }

        #endregion Setup content
    }


    public class ActionSkewRotateScale : ActionsDemo
    {
        const float markrside = 10.0f;

        CCDrawNode box;
        CCDrawNode uL;
        CCDrawNode uR;

        CCSkewTo actionTo;
        CCRotateTo rotateTo;
        CCScaleTo actionScaleTo;

        CCScaleTo actionScaleToBack;
        CCRotateTo rotateToBack;
        CCSkewTo actionToBack;

        #region Properties

        public override string Subtitle
        {
            get { return "Skew + Rotate + Scale"; }
        }

        #endregion Properties


        #region Constructors

        public ActionSkewRotateScale()
        {
            box = new CCDrawNode();
            box.DrawRect(new CCRect (0.0f, 0.0f, 100.0f, 100.0f), new CCColor4B(255, 255, 0, 255));
            box.AnchorPoint = new CCPoint(0, 0);

            uL = new CCDrawNode();
            uL.DrawRect(new CCRect (0.0f, 0.0f, markrside, markrside), new CCColor4B(255, 0, 0, 255));
            uL.AnchorPoint = new CCPoint(0, 0);
            box.AddChild(uL);

            uR = new CCDrawNode();
            uR.DrawRect(new CCRect (0.0f, 0.0f, markrside, markrside), new CCColor4B(0, 0, 255, 255));
            uR.AnchorPoint = new CCPoint(0, 0);
            box.AddChild(uR);

            AddChild(box);

            actionTo = new CCSkewTo (2, 0.0f, 2.0f);
            rotateTo = new CCRotateTo (2, 61.0f);
            actionScaleTo = new CCScaleTo(2, -0.44f, 0.47f);

            actionScaleToBack = new CCScaleTo(2, 1.0f, 1.0f);
            rotateToBack = new CCRotateTo (2, 0);
            actionToBack = new CCSkewTo (2, 0, 0);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            Tamara.RemoveFromParent(true);
            Grossini.RemoveFromParent(true);
            Kathia.RemoveFromParent(true);

            var boxSize = new CCSize(100.0f, 100.0f);

            box.Position = new CCPoint(windowSize.Center.X - (boxSize.Width / 2), windowSize.Center.Y - (boxSize.Height / 2));
            box.ContentSize = boxSize;

            uL.ContentSize = new CCSize(markrside, markrside);
            uL.Position = new CCPoint(0.0f, boxSize.Height - markrside);

            uR.ContentSize = new CCSize(markrside, markrside);
            uR.Position = new CCPoint(boxSize.Width - markrside, boxSize.Height - markrside);

            box.RunActions(actionTo, actionToBack);
            box.RunActions(rotateTo, rotateToBack);
            box.RunActions(actionScaleTo, actionScaleToBack);
        }

        #endregion Setup content
    }


    public class ActionRotate : ActionsDemo
    {
        CCRotateTo actionTo;
        CCRotateTo actionTo2;
        CCRotateTo actionTo0;
        CCRotateBy actionBy;
        CCRotateBy actionByBack;

        #region Properties

        public override string Subtitle
        {
            get { return "RotateTo / RotateBy"; }
        }

        #endregion Properties


        #region Constructors

        public ActionRotate()
        {
            actionTo = new CCRotateTo (2, 45);
            actionTo2 = new CCRotateTo (2, -45);
            actionTo0 = new CCRotateTo (2, 0);
            actionBy = new CCRotateBy (2, 360);
            actionByBack = (CCRotateBy)actionBy.Reverse();
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(3);

            Tamara.RunActions(actionTo, actionTo0);
            Grossini.RunActions(actionBy, actionByBack);
            Kathia.RunActions(actionTo2, actionTo0);
        }

        #endregion Setup content
    }


    public class ActionJump : ActionsDemo
    {
        CCJumpTo actionTo;
        CCJumpBy actionBy;
        CCJumpBy actionUp;
        CCJumpBy actionByBack;


        #region Properties

        public override string Subtitle
        {
            get { return "JumpTo / JumpBy"; }
        }

        #endregion Properties


        #region Constructors

        public ActionJump()
        {
            actionTo = new CCJumpTo (2, new CCPoint(300, 300), 50, 4);
            actionBy = new CCJumpBy (2, new CCPoint(300, 0), 50, 4);
            actionUp = new CCJumpBy (2, new CCPoint(0, 0), 80, 4);
            actionByBack = (CCJumpBy)actionBy.Reverse();
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(3);

            Tamara.RunAction (actionTo);
            Grossini.RunActions (actionBy, actionByBack);
            Kathia.RepeatForever (actionUp);
        }

        #endregion Setup content
    }


    public class ActionBezier : ActionsDemo
    {
        #region Properties

        public override string Subtitle
        {
            get { return "BezierBy / BezierTo"; }
        }

        #endregion Properties


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(3);

            // sprite 1
            CCBezierConfig bezier;
            bezier.ControlPoint1 = new CCPoint(0, windowSize.Height / 2);
            bezier.ControlPoint2 = new CCPoint(300, -windowSize.Height / 2);
            bezier.EndPosition = new CCPoint(300, 100);

            var bezierForward = new CCBezierBy(3, bezier);
            var bezierBack = bezierForward.Reverse();
            var rep = new CCRepeatForever (bezierForward, bezierBack);


            // sprite 2
            Tamara.Position = new CCPoint(80, 160);
            CCBezierConfig bezier2;
            bezier2.ControlPoint1 = new CCPoint(100, windowSize.Height / 2);
            bezier2.ControlPoint2 = new CCPoint(200, -windowSize.Height / 2);
            bezier2.EndPosition = new CCPoint(240, 160);

            var bezierTo1 = new CCBezierTo (2, bezier2);

            // sprite 3
            Kathia.Position = new CCPoint(400, 160);
            var bezierTo2 = new CCBezierTo (2, bezier2);

            Grossini.RunAction(rep);
            Tamara.RunAction(bezierTo1);
            Kathia.RunAction(bezierTo2);
        }

        #endregion Setup content
    }


    public class ActionBlink : ActionsDemo
    {
        CCBlink action1;
        CCBlink action2;


        #region Properties

        public override string Subtitle
        {
            get { return "Blink"; }
        }

        #endregion Properties


        #region Constructors

        public ActionBlink()
        {
            action1 = new CCBlink(2, 10);
            action2 = new CCBlink(2, 5);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(2);

            Tamara.RunAction(action1);
            Kathia.RunAction(action2);
        }

        #endregion Setup content
    }


    public class ActionFade : ActionsDemo
    {
        CCFadeIn action1;
        CCFiniteTimeAction action1Back;

        CCFadeOut action2;
        CCFiniteTimeAction action2Back;

        #region Properties

        public override string Subtitle
        {
            get { return "FadeIn / FadeOut"; }
        }

        #endregion Properties


        #region Constructors

        public ActionFade()
        {
            action1 = new CCFadeIn(1.0f);
            action1Back = action1.Reverse();

            action2 = new CCFadeOut(1.0f);
            action2Back = action2.Reverse();
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(2);

            Tamara.Opacity = 0;
            Tamara.RunActions(action1, action1Back);
            Kathia.RunActions(action2, action2Back);
        }

        #endregion Setup content
    }


    public class ActionTint : ActionsDemo
    {
        CCTintTo action1;
        CCTintBy action2;
        CCFiniteTimeAction action2Back;

        #region Properties

        public override string Subtitle
        {
            get { return "TintTo / TintBy"; }
        }

        #endregion Properties


        #region Constructors

        public ActionTint()
        {
            action1 = new CCTintTo (2, 255, 0, 255);
            action2 = new CCTintBy (2, -127, -255, -127);
            action2Back = action2.Reverse();
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(2);

            Tamara.RunAction(action1);
            Kathia.RunActions(action2, action2Back);
        }

        #endregion Setup content
    }


    public class ActionAnimate : ActionsDemo
    {
        CCAnimate action;
        CCAnimate action2;
        CCAnimate action3;


        #region Properties

        public override string Title
        {
            get { return "Animation"; }
        }

        public override string Subtitle
        {
            get { return "Center: Manual animation. Border: using file format animation"; }
        }

        #endregion Properties


        #region Constructors

        public ActionAnimate()
        {
            var animation = new CCAnimation();
            for (var i = 1; i < 15; i++)
            {
                var szName = String.Format("Images/grossini_dance_{0:00}", i);
                animation.AddSpriteFrame(szName);
            }

            // Should last 2.8 seconds. And there are 14 frames.
            animation.DelayPerUnit = 2.8f / 14.0f;
            animation.RestoreOriginalFrame = true;

            action = new CCAnimate(animation);

            var cache = CCAnimationCache.SharedAnimationCache;
            cache.AddAnimations("animations/animations-2.plist");
            var animation2 = cache["dance_1"];

            action2 = new CCAnimate (animation2);

            var animation3 = animation2.Copy();
            animation3.Loops = 4;

            action3 = new CCAnimate (animation3);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(3);

            Grossini.RunAction(new CCSequence(action, action.Reverse()));
            Tamara.RunAction(new CCSequence(action2, action2.Reverse()));
            Kathia.RunAction(action3);
        }

        #endregion Setup content
    }


    public class ActionSequence : ActionsDemo
    {
        CCSequence action;

        #region Properties

        public override string Subtitle
        {
            get { return "Sequence: Move + Rotate"; }
        }

        #endregion Properties


        #region Constructors

        public ActionSequence()
        {
            action = new CCSequence(new CCMoveBy (2, new CCPoint(240, 0)), new CCRotateBy (2, 540));
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            AlignSpritesLeft(1);

            Grossini.RunAction(action);
        }

        #endregion Setup content
    }


    public class ActionSequence2 : ActionsDemo
    {
        CCSequence action;


        #region Properties

        public override string Subtitle
        {
            get { return "Sequence of InstantActions"; }
        }

        #endregion Properties


        #region Constructors

        public ActionSequence2()
        {
            action = new CCSequence(
                new CCPlace(new CCPoint(200, 200)),
                new CCShow(),
                new CCMoveBy (1, new CCPoint(100, 0)),
                new CCCallFunc(Callback1),
                new CCCallFuncN(Callback2),
                new CCCallFuncND(Callback3, 0xbebabeba));
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            AlignSpritesLeft(1);

            Grossini.Visible = false;
            Grossini.RunAction(action);
        }

        #endregion Setup content


        #region Callbacks

        void Callback1()
        {
            var s = Layer.VisibleBoundsWorldspace.Size;
            var label = new CCLabelTtf("callback 1 called", "arial", 16);
            label.Position = new CCPoint(s.Width / 4 * 1, s.Height / 2);

            AddChild(label);
        }

        void Callback2(CCNode sender)
        {
            var s = Layer.VisibleBoundsWorldspace.Size;
            var label = new CCLabelTtf("callback 2 called", "arial", 16);
            label.Position = new CCPoint(s.Width / 4 * 2, s.Height / 2);

            AddChild(label);
        }

        void Callback3(CCNode sender, object data)
        {
            var s = Layer.VisibleBoundsWorldspace.Size;
            var label = new CCLabelTtf("callback 3 called", "arial", 16);
            label.Position = new CCPoint(s.Width / 4 * 3, s.Height / 2);

            AddChild(label);
        }

        #endregion Callbacks
    }


    public class ActionCallFunc : ActionsDemo
    {
        #region Properties

        public override string Subtitle
        {
            get { return "Callbacks: CallFunc and friends"; }
        }

        #endregion Properties


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(3);

            var action = new CCSequence(
                new CCMoveBy (2, new CCPoint(200, 0)),
                new CCCallFunc(Callback1));

            var action2 = new CCSequence(
                new CCScaleBy(2, 2),
                new CCFadeOut  (2),
                new CCCallFuncN(Callback2));

            var action3 = new CCSequence(
                new CCRotateBy (3, 360),
                new CCFadeOut  (2),
                new CCCallFuncND(Callback3, 0xbebabeba));

            Grossini.RunAction(action);
            Tamara.RunAction(action2);
            Kathia.RunAction(action3);
        }

        #endregion Setup content


        #region Callbacks

        void Callback1()
        {
            var s = Layer.VisibleBoundsWorldspace.Size;
            var label = new CCLabelTtf("callback 1 called", "arial", 16);
            label.Position = new CCPoint(s.Width / 4 * 1, s.Height / 2);

            AddChild(label);
        }

        void Callback2(CCNode sender)
        {
            var s = Layer.VisibleBoundsWorldspace.Size;
            var label = new CCLabelTtf("callback 2 called", "arial", 16);
            label.Position = new CCPoint(s.Width / 4 * 2, s.Height / 2);

            AddChild(label);
        }

        void Callback3(CCNode target, object data)
        {
            var s = Layer.VisibleBoundsWorldspace.Size;
            var label = new CCLabelTtf("callback 3 called", "arial", 16);
            label.Position = new CCPoint(s.Width / 4 * 3, s.Height / 2);
            AddChild(label);
        }

        #endregion Callbacks
    }


    public class ActionCallFuncND : ActionsDemo
    {
        CCSequence action;


        #region Properties

        public override string Title
        {
            get { return "CallFuncND + auto remove"; }
        }

        public override string Subtitle
        {
            get { return "CallFuncND + removeFromParentAndCleanup. Grossini dissapears in 2s"; }
        }

        #endregion Properties


        #region Constructors

        public ActionCallFuncND()
        {
            action = new CCSequence(
                new CCMoveBy (2.0f, new CCPoint(200, 0)),
                new CCCallFuncND(RemoveFromParentAndCleanup, true)
            );
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(1);

            Grossini.RunAction(action);
        }

        #endregion Setup content


        void RemoveFromParentAndCleanup(CCNode sender, object data)
        {
            var cleanUp = (bool) data;
            Grossini.RemoveFromParent(cleanUp);
        }
    }


    public class ActionSpawn : ActionsDemo
    {
        CCSpawn action;


        #region Properties

        public override string Subtitle
        {
            get { return "Spawn: Jump + Rotate"; }
        }

        #endregion Properties


        #region Constructors

        public ActionSpawn()
        {
            action = new CCSpawn(new CCJumpBy (2, new CCPoint(300, 0), 50, 4), new CCRotateBy (2, 720));
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            AlignSpritesLeft(1);
            Grossini.RunAction(action);
        }

        #endregion Setup content
    }


    public class ActionRepeatForever : ActionsDemo
    {
        CCSequence action;
        CCRepeatForever repeat;


        #region Properties

        public override string Subtitle
        {
            get { return "CallFuncN + RepeatForever"; }
        }

        #endregion Properties


        #region Constructors

        public ActionRepeatForever()
        {
            action = new CCSequence(new CCDelayTime (1), new CCCallFuncN(RepeatForever));
            repeat = new CCRepeatForever(new CCRotateBy(1.0f, 360));
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(1);
            Grossini.RunAction(action);
        }

        #endregion Setup content


        public void RepeatForever(CCNode sender)
        {
            sender.RunAction(repeat);
        }
    }


    public class ActionRotateToRepeat : ActionsDemo
    {
        CCRepeatForever rep1;
        CCRepeat rep2;

        #region Properties

        public override string Subtitle
        {
            get { return "Repeat/RepeatForever + RotateTo"; }
        }

        #endregion Properties


        #region Constructors

        public ActionRotateToRepeat()
        {
            var act1 = new CCRotateTo(1, 90);
            var act2 = new CCRotateTo(1, 0);
            var seq = new CCSequence(act1, act2);

            rep1 = new CCRepeatForever(seq);
            rep2 = new CCRepeat(seq, 10);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(2);

            Tamara.RunAction(rep1);
            Kathia.RunAction(rep2);
        }

        #endregion Setup content
    }


    public class ActionRotateJerk : ActionsDemo
    {
        CCRepeat rep1;
        CCRepeatForever rep2;

        #region Properties

        public override string Subtitle
        {
            get { return "RepeatForever / Repeat + Rotate"; }
        }

        #endregion Properties


        #region Constructors

        public ActionRotateJerk()
        {
            var seq = new CCSequence(
                new CCRotateTo (0.5f, -20),
                new CCRotateTo (0.5f, 20));

            rep1 = new CCRepeat (seq, 10);
            rep2 = new CCRepeatForever (seq);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(2);

            Tamara.RunAction(rep1);
            Kathia.RunAction(rep2);
        }

        #endregion Setup content
    }


    public class ActionReverse : ActionsDemo
    {
        #region Properties

        public override string Subtitle
        {
            get { return "Reverse an action"; }
        }

        #endregion Properties


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            AlignSpritesLeft(1);

            var jump = new CCJumpBy (2, new CCPoint(300, 0), 50, 4);
            var action = new CCSequence(jump, jump.Reverse());

            Grossini.RunAction(action);
        }

        #endregion Setup content
    }


    public class ActionDelayTime : ActionsDemo
    {
        CCSequence action;


        #region Properties

        public override string Subtitle
        {
            get { return "DelayTime: m + delay + m"; }
        }

        #endregion Properties


        #region Constructors

        public ActionDelayTime()
        {
            var move = new CCMoveBy (1, new CCPoint(150, 0));
            action = new CCSequence(move, new CCDelayTime (2), move);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            AlignSpritesLeft(1);
            Grossini.RunAction(action);
        }

        #endregion Setup content
    }


    public class ActionReverseSequence : ActionsDemo
    {
        CCSequence action;

        #region Properties

        public override string Subtitle
        {
            get { return "Reverse a sequence"; } 
        }

        #endregion Properties


        #region Constructors

        public ActionReverseSequence()
        {

            var move1 = new CCMoveBy (1, new CCPoint(250, 0));
            var move2 = new CCMoveBy (1, new CCPoint(0, 50));
            var seq = new CCSequence(move1, move2, move1.Reverse());
            action = new CCSequence(seq, seq.Reverse());
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            AlignSpritesLeft(1);
            Grossini.RunAction(action);
        }

        #endregion Setup content
    }


    public class ActionReverseSequence2 : ActionsDemo
    {
        CCRepeat action;
        CCSequence seq_tamara;
        CCFiniteTimeAction seq_back;

        #region Properties

        public override string Subtitle
        {
            get { return "Reverse sequence 2"; } 
        }

        #endregion Properties


        #region Constructors

        public ActionReverseSequence2()
        {
            var move1 = new CCMoveBy (1, new CCPoint(250, 0));
            var move2 = new CCMoveBy (1, new CCPoint(0, 50));
            var toggle = new CCToggleVisibility();
            var seq = new CCSequence(move1, toggle, move2, toggle, move1.Reverse());
            action = new CCRepeat ((new CCSequence(seq, seq.Reverse())), 3);

            var move_tamara = new CCMoveBy (1, new CCPoint(100, 0));
            var move_tamara2 = new CCMoveBy (1, new CCPoint(50, 0));
            var hide = new CCHide();

            seq_tamara = new CCSequence(move_tamara, hide, move_tamara2);
            seq_back = seq_tamara.Reverse();
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            AlignSpritesLeft(2);
            Kathia.RunAction(action);
            Tamara.RunAction(new CCSequence(seq_tamara, seq_back));
        }

        #endregion Setup content
    }


    public class ActionRepeat : ActionsDemo
    {
        CCMoveBy a1;
        CCFiniteTimeAction a1Reverse;

        #region Properties

        public override string Subtitle
        {
            get { return "Repeat / RepeatForever actions"; }
        }

        #endregion Properties


        #region Constructors

        public ActionRepeat()
        {
            a1 = new CCMoveBy (1, new CCPoint(150, 0));
            a1Reverse = a1.Reverse();
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            AlignSpritesLeft(2);

            // Repeat 3 times the Move to position (CCPlace) and then the MoveBy
            Kathia.Repeat(3, new CCPlace(new CCPoint(60, 60)), a1);

            // Repeat forever the MoveBy
            Tamara.RepeatForever(a1, a1Reverse);
        }

        #endregion Setup content
    }


    public class ActionOrbit : ActionsDemo
    {
        CCSequence action1;
        CCSequence action2;
        CCSequence action3;
        CCRepeatForever rfe;

        #region Properties

        public override string Subtitle
        {
            get { return "OrbitCamera action"; }
        }

        #endregion Properties


        #region Constructors

        public ActionOrbit()
        {
            var orbit1 = new CCOrbitCamera(2, 1, 0, 0, 180, 0, 0);
            action1 = new CCSequence(orbit1,orbit1.Reverse());

            var orbit2 = new CCOrbitCamera(2, 1, 0, 0, 180, -45, 0);
            action2 = new CCSequence(orbit2, orbit2.Reverse());

            var orbit3 = new CCOrbitCamera(2, 1, 0, 0, 90, 90, 0);
            action3 = new CCSequence(orbit3, orbit3.Reverse());

            var move = new CCMoveBy (3, new CCPoint(100, -100));
            var move_back = move.Reverse();
            var seq = new CCSequence(move, move_back);
            rfe = new CCRepeatForever (seq);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            //Camera.Projection = CCCameraProjection.Projection3D;

            CenterSprites(3);

            Kathia.RunAction(new CCRepeatForever (action1));
            Tamara.RunAction(new CCRepeatForever (action2));
            Grossini.RunAction(new CCRepeatForever (action3));

            Kathia.RunAction(rfe);
            Tamara.RunAction(rfe);
            Grossini.RunAction(rfe);
        }

        public override void OnExit ()
        {
            base.OnExit();

            //Camera.Projection = CCCameraProjection.Projection2D;
        }

        #endregion Setup content
    }


    public class ActionFollow : ActionsDemo
    {
        #region Properties

        public override string Subtitle
        {
            get { return "Follow action"; } 
        }

        #endregion Properties


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(1);

            Grossini.Position = new CCPoint(-200, windowSize.Height / 2);
            var move = new CCMoveBy (2, new CCPoint(windowSize.Width * 3, 0));
            var move_back = move.Reverse();
            var seq = new CCSequence(move, move_back);
            var rep = new CCRepeatForever(seq);

            Grossini.RunAction(rep);

            RunAction(new CCFollow (Grossini, new CCRect(0, 0, windowSize.Width * 2 - 100, windowSize.Height)));
        }

        #endregion Setup content
    }


    public class ActionCardinalSpline : ActionsDemo
    {
        readonly List<CCPoint> pointList = new List<CCPoint>();


        #region Properties

        public override string Title
        {
            get { return "CardinalSplineBy / CardinalSplineAt"; }
        }

        public override string Subtitle
        {
            get { return "Cardinal Spline paths. Testing different tensions for one array"; }
        }

        #endregion Properties


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); 
            CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(2);

            pointList.Clear();

            pointList.Add(new CCPoint(0, 0));
            pointList.Add(new CCPoint(windowSize.Width / 2 - 30, 0));
            pointList.Add(new CCPoint(windowSize.Width / 2 - 30, windowSize.Height - 80));
            pointList.Add(new CCPoint(0, windowSize.Height - 80));
            pointList.Add(new CCPoint(0, 0));

            var action = new CCCardinalSplineBy (3, pointList, 0);
            var reverse = action.Reverse();

            var seq = new CCSequence(action, reverse);

            var action2 = new CCCardinalSplineBy (3, pointList, 1);
            var reverse2 = action2.Reverse();

            var seq2 = new CCSequence(action2, reverse2);

            Tamara.Position = new CCPoint(50, 50);
            Tamara.RunAction(seq);

            Kathia.Position = new CCPoint(windowSize.Width / 2, 50);
            Kathia.RunAction(seq2);
        }

        #endregion Setup content


        protected override void Draw()
        {
            base.Draw();

			// move to 50,50 since the "by" path will start at 50,50
			CCDrawManager.SharedDrawManager.PushMatrix();
            CCDrawManager.SharedDrawManager.Translate(50, 50, 0);

			CCDrawingPrimitives.Begin();
            CCDrawingPrimitives.DrawCardinalSpline(pointList, 0, 100);
			CCDrawingPrimitives.End();

            CCDrawManager.SharedDrawManager.PopMatrix();

            var s = Layer.VisibleBoundsWorldspace.Size;

            CCDrawManager.SharedDrawManager.PushMatrix();
            CCDrawManager.SharedDrawManager.Translate(s.Width / 2, 50, 0);

			CCDrawingPrimitives.Begin();
            CCDrawingPrimitives.DrawCardinalSpline(pointList, 1, 100);
			CCDrawingPrimitives.End();

            CCDrawManager.SharedDrawManager.PopMatrix();
        }
    }


    public class ActionCatmullRom : ActionsDemo
    {
        readonly List<CCPoint> pointList = new List<CCPoint>();
        readonly List<CCPoint> pointList2 = new List<CCPoint>();


        #region Properties

        public override string Title
        {
            get { return "CatmullRomBy / CatmullRomTo"; }
        }

        public override string Subtitle
        {
            get { return "Catmull Rom spline paths. Testing reverse too"; }
        }

        #endregion Properties


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); 
            CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(2);

            Tamara.Position = new CCPoint(50, 50);

            pointList.Clear();

            pointList.Add(new CCPoint(0, 0));
            pointList.Add(new CCPoint(80, 80));
            pointList.Add(new CCPoint(windowSize.Width - 80, 80));
            pointList.Add(new CCPoint(windowSize.Width - 80, windowSize.Height - 80));
            pointList.Add(new CCPoint(80, windowSize.Height - 80));
            pointList.Add(new CCPoint(80, 80));
            pointList.Add(new CCPoint(windowSize.Width / 2, windowSize.Height / 2));

            var action = new CCCatmullRomBy (3, pointList);
            var reverse = action.Reverse();
            CCFiniteTimeAction seq = new CCSequence(action, reverse);

            pointList2.Clear();

            pointList2.Add(new CCPoint(windowSize.Width / 2, 30));
            pointList2.Add(new CCPoint(windowSize.Width - 80, 30));
            pointList2.Add(new CCPoint(windowSize.Width - 80, windowSize.Height - 80));
            pointList2.Add(new CCPoint(windowSize.Width / 2, windowSize.Height - 80));
            pointList2.Add(new CCPoint(windowSize.Width / 2, 30));

            var action2 = new CCCatmullRomTo (3, pointList2);
            var reverse2 = action2.Reverse();

            CCFiniteTimeAction seq2 = new CCSequence(action2, reverse2);

            Tamara.RunAction(seq);
            Kathia.RunAction(seq2);
        }

        #endregion Setup content


        protected override void Draw()
        {
            base.Draw();

            // move to 50,50 since the "by" path will start at 50,50
            CCDrawManager.SharedDrawManager.PushMatrix();
            CCDrawManager.SharedDrawManager.Translate(50, 50, 0);
            CCDrawingPrimitives.Begin();
            CCDrawingPrimitives.DrawCatmullRom(pointList, 50);
            CCDrawingPrimitives.End();
            CCDrawManager.SharedDrawManager.PopMatrix();

            CCDrawingPrimitives.Begin();
            CCDrawingPrimitives.DrawCatmullRom(pointList2, 50);
            CCDrawingPrimitives.End();
        }
    }


    public class ActionTargeted : ActionsDemo
    {
        CCRepeatForever always;

        #region Properties

        public override string Title
        {
            get { return "ActionTargeted"; }
        }

        public override string Subtitle
        {
            get { return "Action that runs on another target. Useful for sequences"; }
        }

        #endregion Properties


        #region Constructors

        public ActionTargeted()
        {
            var jump1 = new CCJumpBy(2, CCPoint.Zero, 100, 3);
            var rot1 = new CCRotateBy(1, 360);

            var t1 = new CCTargetedAction (Kathia, jump1);
            var t2 = new CCTargetedAction (Kathia, rot1);


            var seq = new CCSequence(jump1, t1, rot1, t2);
            always = new CCRepeatForever (seq);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); 
            CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(2);

            Tamara.RunAction(always);
        }

        #endregion Setup content
    }


    #region ActionStacked

    public class ActionStacked : ActionsDemo
    {
        #region Properties

        public override string Title
        {
            get { return "Override me"; }
        }

        public override string Subtitle
        {
            get { return "Tap screen"; }
        }

        #endregion Properties


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); 
            CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(0);

            var listener = new CCEventListenerTouchAllAtOnce();
            listener.OnTouchesEnded = OnTouchesEnded;

            AddEventListener(listener);    

            AddNewSpriteWithCoords(new CCPoint(windowSize.Width/2, windowSize.Height/2));
        }

        #endregion Setup content


        void AddNewSpriteWithCoords(CCPoint p)
        {
            int idx = (int) (CCRandom.Float_0_1() * 1400 / 100);
            int x = (idx % 5) * 85;
            int y = (idx / 5) * 121;


            CCSprite sprite = new CCSprite("Images/grossini_dance_atlas.png", new CCRect(x, y, 85, 121));

            sprite.Position = p;
            this.AddChild(sprite);

            this.RunActionsInSprite(sprite);
        }

        public virtual void RunActionsInSprite(CCSprite sprite)
        {
            // override me
        }

        void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            foreach (var touch in touches)
            {
                if (touch == null)
                    break;

                CCPoint location = Layer.ScreenToWorldspace(touch.LocationOnScreen);

                AddNewSpriteWithCoords(location);
            }
        }
    }


    public class ActionMoveStacked : ActionStacked
    {
        #region Properties

        public override string Title
        {
            get { return "Stacked CCMoveBy/To actions"; }
        }

        #endregion Properties


        public override void RunActionsInSprite(CCSprite sprite)
        {
            sprite.RunAction(
                new CCRepeatForever(
                    new CCSequence(
                        new CCMoveBy(0.05f, new CCPoint(10, 10)),
                        new CCMoveBy(0.05f, new CCPoint(-10, -10)))
                )
            );

            CCMoveBy action = new CCMoveBy(2.0f, new CCPoint(400, 0));
            CCMoveBy action_back = (CCMoveBy) action.Reverse();

            sprite.RunAction(
                new CCRepeatForever(
                    new CCSequence(action, action_back)
                ));
        }
    }


    public class ActionMoveJumpStacked : ActionStacked
    {
        #region Properties

        public override string Title
        {
            get { return "Stacked Move + Jump actions"; }
        }

        #endregion Properties


        public override void RunActionsInSprite(CCSprite sprite)
        {
            sprite.RunAction(
                new CCRepeatForever(
                    new CCSequence(
                        new CCMoveBy(0.05f, new CCPoint(10, 2)),
                        new CCMoveBy(0.05f, new CCPoint(-10, -2)))));

            CCJumpBy jump = new CCJumpBy(2.0f, new CCPoint(400, 0), 100, 5);
            CCJumpBy jump_back = (CCJumpBy) jump.Reverse();

            sprite.RunAction(
                new CCRepeatForever(
                    new CCSequence(jump, jump_back)
                ));
        }
    }

    public class ActionMoveBezierStacked : ActionStacked
    {
        #region Properties

        public override string Title
        {
            get { return "Stacked Move + Bezier actions"; }
        }

        #endregion Properties


        public override void RunActionsInSprite(CCSprite sprite)
        {
            CCSize s = Layer.VisibleBoundsWorldspace.Size;

            // sprite 1
            CCBezierConfig bezier;
            bezier.ControlPoint1 = new CCPoint(0, s.Height / 2);
            bezier.ControlPoint2 = new CCPoint(300, -s.Height / 2);
            bezier.EndPosition = new CCPoint(300, 100);

            CCBezierBy bezierForward = new CCBezierBy(3, bezier);
            CCBezierBy bezierBack = (CCBezierBy) bezierForward.Reverse();
            CCSequence seq = new CCSequence(bezierForward, bezierBack);
            CCRepeatForever rep = new CCRepeatForever(seq);
            sprite.RunAction(rep);

            sprite.RunAction(
                new CCRepeatForever(
                    new CCSequence(
                        new CCMoveBy(0.05f, new CCPoint(10, 0)),
                        new CCMoveBy(0.05f, new CCPoint(-10, 0))
                    )
                )
            );
        }
    }

    public class ActionCatmullRomStacked : ActionsDemo
    {
        List<CCPoint> pointArray;
        List<CCPoint> pointArray2;


        #region Properties

        public override string Title
        {
            get { return "Stacked MoveBy + CatmullRom actions"; }
        }

        public override string Subtitle
        {
            get { return "MoveBy + CatmullRom at the same time in the same sprite"; }
        }

        #endregion Properties


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(2);

            //
            // sprite 1 (By)
            //
            // startPosition can be any coordinate, but since the movement
            // is relative to the Catmull Rom curve, it is better to start with (0,0).
            //

            Tamara.Position = new CCPoint(50, 50);

            pointArray = new List<CCPoint>();

            pointArray.Add(new CCPoint(0, 0));
            pointArray.Add(new CCPoint(80, 80));
            pointArray.Add(new CCPoint(windowSize.Width - 80, 80));
            pointArray.Add(new CCPoint(windowSize.Width - 80, windowSize.Height - 80));
            pointArray.Add(new CCPoint(80, windowSize.Height - 80));
            pointArray.Add(new CCPoint(80, 80));
            pointArray.Add(new CCPoint(windowSize.Width / 2, windowSize.Height / 2));

            var action = new CCCatmullRomBy(3, pointArray);
            var reverse = action.Reverse();

            var seq = new CCSequence(action, reverse);

            Tamara.RunAction(seq);

            Tamara.RunAction(
                new CCRepeatForever(
                    new CCSequence(
                        new CCMoveBy(0.05f, new CCPoint(10, 0)),
                        new CCMoveBy(0.05f, new CCPoint(-10, 0))
                    )
                )
            );

            //
            // sprite 2 (To)
            //
            // The startPosition is not important here, because it uses a "To" action.
            // The initial position will be the 1st point of the Catmull Rom path
            //

            pointArray2 = new List<CCPoint>();

            pointArray2.Add(new CCPoint(windowSize.Width / 2, 30));
            pointArray2.Add(new CCPoint(windowSize.Width - 80, 30));
            pointArray2.Add(new CCPoint(windowSize.Width - 80, windowSize.Height - 80));
            pointArray2.Add(new CCPoint(windowSize.Width / 2, windowSize.Height - 80));
            pointArray2.Add(new CCPoint(windowSize.Width / 2, 30));


            var action2 = new CCCatmullRomTo(3, pointArray2);
            var reverse2 = action2.Reverse();

            var seq2 = new CCSequence(action2, reverse2);

            Kathia.RunAction(seq2);

            Kathia.RunAction(
                new CCRepeatForever(
                    new CCSequence(
                        new CCMoveBy(0.05f, new CCPoint(10, 0)),
                        new CCMoveBy(0.05f, new CCPoint(-10, 0))
                    )
                )
            );

        }

        #endregion Setup content


        protected override void Draw()
        {
            base.Draw();

            // move to 50,50 since the "by" path will start at 50,50

            CCDrawManager.SharedDrawManager.PushMatrix();
            CCDrawManager.SharedDrawManager.Translate(50, 50, 0);
			CCDrawingPrimitives.Begin();
            CCDrawingPrimitives.DrawCatmullRom(pointArray, 50);
			CCDrawingPrimitives.End();
            CCDrawManager.SharedDrawManager.PopMatrix();

			CCDrawingPrimitives.Begin();
            CCDrawingPrimitives.DrawCatmullRom(pointArray2, 50);
			CCDrawingPrimitives.End();
        }
    }


    public class ActionCardinalSplineStacked : ActionsDemo
    {
        List<CCPoint> pointArray = new List<CCPoint>();


        #region Properties

        public override string Title
        {
            get { return "Stacked MoveBy + CardinalSpline actions"; }
        }

        public override string Subtitle
        {
            get { return "CCMoveBy + CCCardinalSplineBy/To at the same time"; }
        }

        #endregion Properties


        #region Setup content

        public override void OnEnter()
        {
            base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(2);

            pointArray.Add(new CCPoint(0, 0));
            pointArray.Add(new CCPoint(windowSize.Width / 2 - 30, 0));
            pointArray.Add(new CCPoint(windowSize.Width / 2 - 30, windowSize.Height - 80));
            pointArray.Add(new CCPoint(0, windowSize.Height - 80));
            pointArray.Add(new CCPoint(0, 0));

            //
            // sprite 1 (By)
            //
            // Spline with no tension (tension==0)
            //

            var action = new CCCardinalSplineBy(3, pointArray, 0);
            var reverse = action.Reverse();

            var seq = new CCSequence(action, reverse);

            Tamara.Position = new CCPoint(50, 50);
            Tamara.RunAction(seq);

            Tamara.RunAction(
                new CCRepeatForever(
                    new CCSequence(
                        new CCMoveBy(0.05f, new CCPoint(10, 0)),
                        new CCMoveBy(0.05f, new CCPoint(-10, 0))
                    )
                )
            );
            //
            // sprite 2 (By)
            //
            // Spline with high tension (tension==1)
            //

            var action2 = new CCCardinalSplineBy(3, pointArray, 1);
            var reverse2 = action2.Reverse();

            var seq2 = new CCSequence(action2, reverse2);

            Kathia.Position = new CCPoint(windowSize.Width / 2, 50);
            Kathia.RunAction(seq2);

            Kathia.RunAction(
                new CCRepeatForever(
                    new CCSequence(
                        new CCMoveBy(0.05f, new CCPoint(10, 0)),
                        new CCMoveBy(0.05f, new CCPoint(-10, 0))
                    )
                )
            );

        }

        #endregion Setup content


        protected override void Draw()
        {
            base.Draw();

            // move to 50,50 since the "by" path will start at 50,50

            CCDrawManager.SharedDrawManager.PushMatrix();
            CCDrawManager.SharedDrawManager.Translate(50, 50, 0);
			CCDrawingPrimitives.Begin();
            CCDrawingPrimitives.DrawCardinalSpline(pointArray, 0, 100);
			CCDrawingPrimitives.End();
            CCDrawManager.SharedDrawManager.PopMatrix();

            var s = Layer.VisibleBoundsWorldspace.Size;

            CCDrawManager.SharedDrawManager.PushMatrix();
            CCDrawManager.SharedDrawManager.Translate(s.Width / 2, 50, 0);
			CCDrawingPrimitives.Begin();
            CCDrawingPrimitives.DrawCardinalSpline(pointArray, 1, 100);
			CCDrawingPrimitives.End();
            CCDrawManager.SharedDrawManager.PopMatrix();
        }
    }

    #endregion


    public class Issue1305 : ActionsDemo
    {
        CCSprite spriteTmp;

        #region Properties

        public override string Title
        {
            get { return "Issue 1305"; }
        }

        public override string Subtitle
        {
            get { return "In two seconds you should see a message on the console. NOT BEFORE."; }
        }

        #endregion Properties


        #region Constructors

        public Issue1305()
        {
            spriteTmp = new CCSprite("Images/grossini");
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(0);

            spriteTmp.RunAction(new CCCallFuncN(Log));

            ScheduleOnce(AddSprite, 2);
        }

        #endregion Setup content


        void Log(CCNode sender)
        {
            CCLog.Log("This message SHALL ONLY appear when the sprite is added to the scene, NOT BEFORE");
        }

        void AddSprite(float dt)
        {
            spriteTmp.Position = new CCPoint(250, 250);
            AddChild(spriteTmp);
        }
    }


    public class Issue1305_2 : ActionsDemo
    {
        #region Properties

        public override string Title
        {
            get { return "Issue 1305 #2"; }
        }

        public override string Subtitle
        {
            get { return "See console. You should only see one message for each block"; }
        }

        #endregion Properties


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(0);

            var spr = new CCSprite("Images/grossini");
            spr.Position = new CCPoint(200, 200);
            AddChild(spr);

            var act1 = new CCMoveBy (2, new CCPoint(0, 100));

            var act2 = new CCCallFunc(Log1);
            var act3 = new CCMoveBy (2, new CCPoint(0, -100));
            var act4 = new CCCallFunc(Log2);
            var act5 = new CCMoveBy (2, new CCPoint(100, -100));
            var act6 = new CCCallFunc(Log3);
            var act7 = new CCMoveBy (2, new CCPoint(-100, 0));
            var act8 = new CCCallFunc(Log4);

            var actF = new CCSequence(act1, act2, act3, act4, act5, act6, act7, act8);

            spr.RunAction(actF);
        }

        #endregion Setup content


        void Log4()
        {
            CCLog.Log("4st block");
        }

        void Log3()
        {
            CCLog.Log("3st block");
        }

        void Log2()
        {
            CCLog.Log("2st block");
        }

        void Log1()
        {
            CCLog.Log("1st block");
        }
    }


    public class Issue1288 : ActionsDemo
    {
        CCSprite spr;
        CCRepeat act4;

        #region Properties

        public override string Title
        {
            get { return "Issue 1288"; }
        }

        public override string Subtitle
        {
            get { return "Sprite should end at the position where it started."; }
        }

        #endregion Properties


        #region Constructors

        public Issue1288()
        {
            spr = new CCSprite("Images/grossini");
            AddChild(spr);

            var act1 = new CCMoveBy (0.5f, new CCPoint(100, 0));
            var act2 = (CCMoveBy) act1.Reverse();
            var act3 = new CCSequence(act1, act2);
            act4 = new CCRepeat (act3, 2);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(0);

            spr.Position = new CCPoint(100, 100);

            spr.RunAction(act4);
        }

        #endregion Setup content
    }


    public class Issue1288_2 : ActionsDemo
    {
        CCSprite spr;
        CCMoveBy act1;

        #region Properties

        public override string Title
        {
            get { return "Issue 1288 #2"; }
        }

        public override string Subtitle
        {
            get { return "Sprite should move 100 pixels, and stay there"; }
        }

        #endregion Properties


        #region Constructors

        public Issue1288_2()
        {
            spr = new CCSprite("Images/grossini");
            AddChild(spr);

            act1 = new CCMoveBy(0.5f, new CCPoint(100, 0));
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(0);

            spr.Position = new CCPoint(100, 100);

            spr.RunAction(new CCRepeat (act1, 1));
        }

        #endregion Setup content
    }

    public class Issue1327 : ActionsDemo
    {
        CCSequence actF;


        #region Properties

        public override string Title
        {
            get { return "Issue 1327"; }
        }

        public override string Subtitle
        {
            get { return "See console: You should see: 0, 45, 90, 135, 180"; }
        }

        #endregion Properties


        #region Constructors

        public Issue1327()
        {
            var act1 = new CCCallFuncN(LogSprRotation);
            var act2 = new CCRotateBy (0.25f, 45);
            var act3 = new CCCallFuncN(LogSprRotation);
            var act4 = new CCRotateBy (0.25f, 45);
            var act5 = new CCCallFuncN(LogSprRotation);
            var act6 = new CCRotateBy (0.25f, 45);
            var act7 = new CCCallFuncN(LogSprRotation);
            var act8 = new CCRotateBy (0.25f, 45);
            var act9 = new CCCallFuncN(LogSprRotation);

            actF = new CCSequence(act1, act2, act3, act4, act5, act6, act7, act8, act9);
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(0);

            var spr = new CCSprite("Images/grossini");
            spr.Position = new CCPoint(100, 100);
            AddChild(spr);

            spr.RunAction(actF);
        }

        #endregion Setup content


        void LogSprRotation(CCNode sender)
        {
            CCLog.Log("{0},{1}", sender.RotationX, sender.RotationY);
        }
    }

    public class Issue1389 : ActionsDemo
    {
        int testInteger;


        #region Properties

        public override string Subtitle
        {
            get { return "See console: You should see an 8"; }
        }

        #endregion Properties


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(0);

            testInteger = 0;
            CCLog.Log("testInt = {0}", testInteger);

            this.RunAction(
                new CCSequence(
                    new CCCallFuncND(IncrementIntegerCallback, "1"),
                    new CCCallFuncND(IncrementIntegerCallback, "2"),
                    new CCCallFuncND(IncrementIntegerCallback, "3"),
                    new CCCallFuncND(IncrementIntegerCallback, "4"),
                    new CCCallFuncND(IncrementIntegerCallback, "5"),
                    new CCCallFuncND(IncrementIntegerCallback, "6"),
                    new CCCallFuncND(IncrementIntegerCallback, "7"),
                    new CCCallFuncND(IncrementIntegerCallback, "8")
                )
            );
        }

        #endregion Setup content


        void IncrementInteger()
        {
            testInteger++;
        }

        void IncrementIntegerCallback(CCNode sender, object data)
        {
            IncrementInteger();
            CCLog.Log("{0}", data);
        }
    }


    public class PauseResumeActions : ActionsDemo
    {
        List<object> pausedTargets;


        #region Properties

        public override string Title
        {
            get { return "PauseResumeActions"; }
        }

        public override string Subtitle
        {
            get { return "All actions pause at 3s and resume at 5s"; }
        }

        #endregion Properties


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            CenterSprites(2);

            Tamara.RunAction(new CCRepeatForever (new CCRotateBy (3, 360)));
            Grossini.RunAction(new CCRepeatForever (new CCRotateBy (3, -360)));
            Kathia.RunAction(new CCRepeatForever (new CCRotateBy (3, 360)));

            ScheduleOnce(Pause, 3);
            ScheduleOnce(Resume, 5);
        }

        #endregion Setup content


        void Pause(float dt)
        {
            CCLog.Log("Pausing");

            pausedTargets = Application.ActionManager.PauseAllRunningActions();
        }

        void Resume(float dt)
        {
            CCLog.Log("Resuming");
            Application.ActionManager.ResumeTargets(pausedTargets);
        }
    }

    public class RemoveSelfActions : ActionsDemo
    {
        CCSequence action;

        #region Properties

        public override string Subtitle
        {
            get { return "Sequence: Move + Rotate + Scale + RemoveSelf"; }
        }

        #endregion Properties


        #region Constructors

        public RemoveSelfActions()
        {
            action = new CCSequence(
                new CCMoveBy(2, new CCPoint(240, 0)),
                new CCRotateBy(2, 540),
                new CCScaleTo(1, 0.1f),
                new CCRemoveSelf()
            );
        }

        #endregion Constructors


        #region Setup content

        public override void OnEnter()
        {
base.OnEnter(); CCSize windowSize = Layer.VisibleBoundsWorldspace.Size;

            AlignSpritesLeft(1);

            Grossini.RunAction(action);
        }

        #endregion Setup content
    }

}