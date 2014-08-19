using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using CocosSharp;
using Microsoft.Xna.Framework;
using ChipmunkSharp;

namespace tests
{

	public enum PhysicsTests
	{
		CLICK_ADD = 0,
        PYRAMID_STACK,
		TEST_CASE_COUNT
	};

	// the class inherit from TestScene
	// every Scene each test used must inherit from TestScene,
	// make sure the test have the menu item for back to main menu
	public class PhysicsTestScene : TestScene
	{

        public PhysicsTestScene ()
        : base(true)
        { }

		public static int sceneIndex = -1;

		public static CCLayer CreateLayer(int index)
		{
			CCLayer testLayer = null;

			switch (index)
			{
			case (int) PhysicsTests.CLICK_ADD:
				testLayer = new ClickAddTest();
				break;
            case (int) PhysicsTests.PYRAMID_STACK:
                testLayer = new PyramidStackTest();
                break;
			default:
				break;
			}

			return testLayer;
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
			++sceneIndex;
			sceneIndex = sceneIndex % (int)EventDispatchTests.TEST_CASE_COUNT;

			var testLayer = CreateLayer(sceneIndex);

			return testLayer;
		}

		public static CCLayer BackAction()
		{
			--sceneIndex;
			if (sceneIndex < 0)
				sceneIndex += (int)EventDispatchTests.TEST_CASE_COUNT;

			var testLayer = CreateLayer(sceneIndex);

			return testLayer;
		}

		public static CCLayer RestartAction()
		{
			var testLayer = CreateLayer(sceneIndex);

			return testLayer;
		}


		public override void runThisTest()
		{
			sceneIndex = -1;
			AddChild(NextAction());

			Director.ReplaceScene(this);
		}
	}


	public class PhysicsTest : TestNavigationLayer
	{

        public const int GRABBABLE_MASK_BIT = 1 << 31;
        public const int DRAG_BODYS_TAG = 0x80;

        public cpShapeFilter GRAB_FILTER = new cpShapeFilter(cp.NO_GROUP, GRABBABLE_MASK_BIT, GRABBABLE_MASK_BIT);
        public cpShapeFilter NOT_GRABBABLE_FILTER = new cpShapeFilter(cp.NO_GROUP, ~GRABBABLE_MASK_BIT, ~GRABBABLE_MASK_BIT);


        private float prevX = 0, prevY = 0;
        private float kFilterFactor = 0.05f;


        //cpSpace space = null;
        public CCEventListenerTouchAllAtOnce eTouch;
        public CCEventListenerAccelerometer eAccel;

        public CCNode _mouseJointNode;
        public CCPhysicsJoint _mouseJoint;

        public CCScene scene;

        CCTexture2D spriteTexture;

        protected override void AddedToScene()
        {
            base.AddedToScene();

            Scene.PhysicsWorld.DebugDrawMask = CCPhysicsWorld.DEBUGDRAW_SHAPE;
            //Scene.Position = Window.WindowSizeInPixels.Center;
            eTouch = new CCEventListenerTouchAllAtOnce();
            eTouch.OnTouchesEnded = OnTouchesEnded;
            AddEventListener(eTouch);

            eAccel = new CCEventListenerAccelerometer();
            eAccel.OnAccelerate = OnAccelerate;

        }

        public virtual void OnAccelerate(CCEventAccelerate acc)
        {
            float accelX = (float)acc.Acceleration.X * kFilterFactor + (1 - kFilterFactor) * prevX;
            float accelY = (float)acc.Acceleration.Y * kFilterFactor + (1 - kFilterFactor) * prevY;

            prevX = accelX;
            prevY = accelY;

            var v = new CCPoint(accelX, accelY);
            v = v * 200;

            if (Scene != null)
            {
                Scene.PhysicsWorld.Gravity = v;
            }
        }

        public virtual void OnTouchesEnded(List<CCTouch> touches, CCEvent e)
        {

        }

        public CCSprite MakeBox(CCPoint point, CCSize size, int color, CCPhysicsMaterial material)
        {

            bool yellow = (color == 0) ? CCRandom.Float_0_1() > 0.5f : color == 1;

            CCSprite ball = new CCSprite(yellow ? "Images/YellowSquare.png" : "Images/CyanSquare.png");

            ball.ScaleX = Window.WindowSizeInPixels.Width / 100.0f;
            ball.ScaleY = Window.WindowSizeInPixels.Height / 100.0f;

            var body = CCPhysicsBody.CreateBox(size, material, 0.0f);
            ball.PhysicsBody = body;
            ball.Position = point;// new cpVect(point.X, point.Y);

            return ball;
        }


        public CCSprite MakeBall(CCPoint point, float radius, CCPhysicsMaterial material)
        {
            CCSprite ball = new CCSprite("ball.png");
            ball.Scale = 0.13f * radius;
            var body = CCPhysicsBody.CreateCircle(radius, material, CCPoint.Zero);
            ball.PhysicsBody = body;
            ball.Position = point;// new cpVect(point.X, point.Y);
            return ball;
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            Scene.Update(dt);
        }

        protected override void Draw()
        {
            base.Draw();
            Scene.PhysicsWorld.DebugDraw();
        }

        public override void OnExit()
        {
            base.OnExit();

            RemoveEventListener(eTouch);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            spriteTexture = new CCSpriteBatchNode("images/grossini_dance_atlas", 100).Texture;

        }

        public CCSprite AddGrossiniAtPosition(CCPoint location, float scale = 1.0f)
        {
            int posx, posy;

            posx = CCRandom.Next() * 200;
            posy = CCRandom.Next() * 200;

            posx = (Math.Abs(posx) % 4) * 85;
            posy = (Math.Abs(posy) % 3) * 121;

            CCSprite sp = new CCSprite(spriteTexture, new CCRect(posx, posy, 85, 121));
            sp.Scale = scale;
            sp.PhysicsBody = CCPhysicsBody.CreateBox(new CCSize(48.0f * scale, 108.0f * scale), 3);

            AddChild(sp);
            sp.Position = location;
            return sp;
        }


		public override void RestartCallback(object sender)
		{
			CCScene s = new PhysicsTestScene();
			s.AddChild(PhysicsTestScene.RestartAction ());

			Director.ReplaceScene(s);
		}


		public override void NextCallback(object sender)
		{

			CCScene s = new PhysicsTestScene();
			s.AddChild(PhysicsTestScene.NextAction ());

			Director.ReplaceScene(s);
		}

		public override void BackCallback(object sender)
		{

			CCScene s = new PhysicsTestScene();
			s.AddChild(PhysicsTestScene.BackAction ());

			Director.ReplaceScene(s);
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
	}

	public class ClickAddTest : PhysicsTest
	{
 
        public override void OnEnter()
        {
            base.OnEnter();

            //Create a boundin box container room
            var node = new CCNode();
            node.PhysicsBody = CCPhysicsBody.CreateEdgeBox(VisibleBoundsWorldspace.Size, 1.0f, CCPoint.Zero);
            node.PhysicsBody.SetType(cpBodyType.STATIC);
            node.Position = VisibleBoundsWorldspace.Center;
            AddChild(node);

            //drops a grosini sprite on center on window
            AddGrossiniAtPosition(VisibleBoundsWorldspace.Center);

            Schedule();
        }

        public override void OnTouchesEnded(System.Collections.Generic.List<CCTouch> touches, CCEvent arg2)
        {
            CCPoint location = touches.FirstOrDefault().Location;
            AddGrossiniAtPosition(location);

        }


		public override string Title
		{
			get
			{
				return "Click Add Test";
			}
		}

		public override string Subtitle
		{
			get
			{
				return "Mouse Move, Buttons and Scroll";
			}
		}

	}

    public class PyramidStackTest : PhysicsTest
    {
        public PyramidStackTest()
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();

            var touchListener = new CCEventListenerTouchOneByOne();
//            touchListener.OnTouchBegan = 
//            touchListener->onTouchBegan = CC_CALLBACK_2(PhysicsDemoPyramidStack::onTouchBegan, this);
//            touchListener->onTouchMoved = CC_CALLBACK_2(PhysicsDemoPyramidStack::onTouchMoved, this);
//            touchListener->onTouchEnded = CC_CALLBACK_2(PhysicsDemoPyramidStack::onTouchEnded, this);
//            AddEventListener(touchListener);

            var visibleRect = VisibleBoundsWorldspace;

            var node = new CCNode();
            node.PhysicsBody = CCPhysicsBody.CreateEdgeSegment(visibleRect.LeftBottom() + new CCPoint(0, 50), visibleRect.RightBottom() + new CCPoint(0, 50));
            AddChild(node);

            var ball = new CCSprite("Images/ball.png");
            ball.Scale = 1;
            ball.Tag = 100;
            ball.PhysicsBody = CCPhysicsBody.CreateCircle(10, CCPoint.Zero);
            ball.PhysicsBody.Tag = DRAG_BODYS_TAG;
            ball.Position = visibleRect.Bottom() + new CCPoint(0, 60);
            AddChild(ball);

            ScheduleOnce(UpdateOnce, 3.0f);
//
//            for(int i=0; i<14; i++)
//            {
//                for(int j=0; j<=i; j++)
//                {
//                    var sp = AddGrossiniAtPosition(visibleRect.Bottom() + new CCPoint((i/2 - j) * 11, (14 - i) * 23 + 100), 0.2f);
//                    sp.PhysicsBody.Tag = DRAG_BODYS_TAG;
//                }
//            }
//


        }

        void UpdateOnce(float delta)
        {
            var ball = this[100];
            ball.PhysicsBody = CCPhysicsBody.CreateCircle(30, CCPoint.Zero);
            ball.PhysicsBody.Tag = DRAG_BODYS_TAG;
        }

        public override string Title
        {
            get
            {
                return "Pyramid Stack";
            }
        }

//        public override string Subtitle
//        {
//            get
//            {
//                return "Mouse Move, Buttons and Scroll";
//            }
//        }

    }

}
