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
        JOINTS,
        LOGO_SMASH,
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
            case (int)PhysicsTests.JOINTS:
                testLayer = new JointTest();
                break;
            case (int)PhysicsTests.LOGO_SMASH:
                testLayer = new LogoSmashTest();
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

        public CCSprite MakeBox(CCPoint point, CCSize size)
        {
            return MakeBox(point, size, 0, CCPhysicsMaterial.PHYSICSSHAPE_MATERIAL_DEFAULT);
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

        public CCSprite MakeBall(CCPoint point, float radius)
        {
            return MakeBall(point, radius, CCPhysicsMaterial.PHYSICSSHAPE_MATERIAL_DEFAULT);
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

    public class LogoSmashTest : PhysicsTest
    {

        	int bodyCount = 0;

		int image_width = 188;
		int image_height = 35;

        public int get_pixel(int x, int y)
        {
            return image_bitmap[x + (y * 188)];
        }


        public LogoSmashTest()
        {

        }

        void UpdateOnce(float delta)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();


            Scene.PhysicsWorld.Gravity = CCPoint.Zero;
            Scene.PhysicsWorld.UpdateRate = 5;

            CCSpriteBatchNode _ball = new CCSpriteBatchNode("ball.png");

            AddChild(_ball);

            bodyCount = 0;


            for (var y = 0; y < image_height; y++)
            {
                for (var x = 0; x < image_width; x++)
                {
                    if (get_pixel(x, y) == 0)
                        continue;

                    float x_jitter = 0.05f * CCRandom.Float_0_1();
                    float y_jitter = 0.05f * CCRandom.Float_0_1();

                    CCPoint position = new CCPoint(
                        2 * (x - image_width / 2 + x_jitter) + Window.WindowSizeInPixels.Width * .5f,
                        2 * (image_height / 2 - y + y_jitter) + Window.WindowSizeInPixels.Height * .5f
                        );

                    CCSprite ball = MakeBall(position, 0.95f, new CCPhysicsMaterial(0.01f, 0.0f, 0.0f));

                    ball.PhysicsBody.SetMass(1.0f);
                    ball.PhysicsBody.Moment = cp.Infinity;


                    //body.PhysicsBody.SetGravityEnable(false);

                    AddChild(ball);

                    bodyCount++;
                }
            }

            var bullet = MakeBall(new CCPoint(400, 0),
                10,
                new CCPhysicsMaterial(
                    CCPhysicsMaterial.PHYSICSSHAPE_MATERIAL_DEFAULT_DENSITY,
                    0.0f,
                    0.0f)
                    );

            bullet.PhysicsBody.SetVelocity(new CCPoint(200, 0));

            bullet.Position = new CCPoint(200, Window.WindowSizeInPixels.Height * .5f);

            bullet.PhysicsBody.SetFilter(NOT_GRABBABLE_FILTER);

            bullet.PhysicsBody.SetMass(100);

            _ball.AddChild(bullet);

            bodyCount++;

            Schedule();

        }

        public override string Title
        {
            get
            {
                return "Logo Smash Test";
            }
        }


        #region image

        int[] image_bitmap = new int[] {
	0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,51,119,102,17,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,102,255,255,255,238,51,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,119,255,255,255,255,255,187,0,
0,0,0,0,0,0,0,0,0,0,0,68,255,255,255,255,255,119,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,17,255,255,255,255,255,170,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,17,221,255,255,255,255,255,68,0,0,0,0,0,0,0,0,0,0,0,204,
255,255,255,255,221,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,68,255,255,255,255,255,187,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,85,255,255,255,255,255,187,0,0,0,0,0,0,0,0,0,0,102,255,255,255,255,255,68,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,238,255,255,255,255,
153,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,187,255,255,255,255,255,102,0,0,0,0,0,0,0,0,17,238,255,255,255,255,153,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,51,238,255,255,187,17,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,34,238,255,255,
255,255,221,0,0,0,0,0,0,0,0,119,255,255,255,255,238,17,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,68,51,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,119,255,255,255,255,255,102,0,0,0,0,0,0,34,238,
255,255,255,255,102,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,221,255,255,255,255,238,17,0,0,0,0,0,153,255,255,255,255,187,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,68,255,255,255,255,255,119,0,0,0,0,51,255,255,255,255,255,34,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,170,255,255,255,255,238,17,0,0,0,187,255,255,255,255,119,0,0,0,0,0,0,0,0,0,0,0,0,34,85,119,119,187,170,119,119,34,0,0,0,0,0,0,0,0,0,51,68,68,
68,68,17,0,0,0,51,119,136,187,119,85,0,0,0,0,0,0,0,0,85,119,153,170,119,102,17,0,0,0,0,0,0,0,0,0,0,0,0,34,85,119,119,187,170,119,119,34,0,0,
0,0,0,0,0,0,0,0,51,68,68,68,68,17,0,0,0,85,119,119,102,0,0,51,68,68,68,68,17,0,0,0,0,0,51,68,68,68,68,17,0,0,0,34,119,119,187,136,119,34,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,34,238,255,255,255,255,119,0,0,85,255,
255,255,255,221,17,0,0,0,0,0,0,0,0,0,17,119,204,255,255,255,255,255,255,255,255,255,187,51,0,0,0,0,0,0,0,187,255,255,255,255,68,0,34,187,255,255,255,255,255,255,
221,68,0,0,0,0,85,221,255,255,255,255,255,255,238,102,0,0,0,0,0,0,0,0,17,119,204,255,255,255,255,255,255,255,255,255,187,51,0,0,0,0,0,0,0,0,187,255,255,255,
255,68,0,51,204,255,255,255,255,0,0,187,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,0,17,153,255,255,255,255,255,255,255,170,17,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,119,255,255,255,255,255,34,0,221,255,255,255,255,68,0,0,0,0,0,0,0,0,
0,34,238,255,255,255,255,255,255,255,255,255,255,255,255,238,51,0,0,0,0,0,0,187,255,255,255,255,68,51,238,255,255,255,255,255,255,255,255,255,102,0,0,119,255,255,255,255,255,255,
255,255,255,255,102,0,0,0,0,0,0,34,238,255,255,255,255,255,255,255,255,255,255,255,255,238,51,0,0,0,0,0,0,0,187,255,255,255,255,68,51,238,255,255,255,255,255,0,0,187,
255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,51,221,255,255,255,255,255,255,255,255,255,204,17,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,221,255,255,255,255,153,102,255,255,255,255,170,0,0,0,0,0,0,0,0,0,0,68,255,255,255,255,255,255,255,255,255,255,
255,255,255,255,238,17,0,0,0,0,0,187,255,255,255,255,102,238,255,255,255,255,255,255,255,255,255,255,238,34,102,255,255,255,255,255,255,255,255,255,255,255,255,34,0,0,0,0,0,68,
255,255,255,255,255,255,255,255,255,255,255,255,255,255,238,17,0,0,0,0,0,0,187,255,255,255,255,68,187,255,255,255,255,255,255,0,0,187,255,255,255,255,68,0,0,0,0,0,187,255,
255,255,255,85,204,255,255,255,255,255,255,255,255,255,255,255,153,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,68,255,255,255,255,238,221,255,255,255,238,17,0,0,0,0,0,0,0,0,0,0,68,255,255,255,153,102,68,0,34,85,170,255,255,255,255,255,136,0,0,0,0,0,187,
255,255,255,255,238,255,255,153,102,119,153,255,255,255,255,255,255,170,238,255,255,187,119,102,136,238,255,255,255,255,255,153,0,0,0,0,0,68,255,255,255,153,102,68,0,34,85,170,255,255,
255,255,255,136,0,0,0,0,0,0,187,255,255,255,255,119,255,255,255,255,255,255,255,0,0,187,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,204,255,255,187,119,68,119,187,255,
255,255,255,255,255,34,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,153,255,255,255,255,255,
255,255,255,119,0,0,0,0,0,0,0,0,0,0,0,68,255,136,17,0,0,0,0,0,0,0,136,255,255,255,255,221,0,0,0,0,0,187,255,255,255,255,255,238,51,0,0,0,0,51,
238,255,255,255,255,255,255,255,102,0,0,0,0,17,221,255,255,255,255,238,0,0,0,0,0,68,255,136,17,0,0,0,0,0,0,0,136,255,255,255,255,221,0,0,0,0,0,0,187,255,
255,255,255,221,255,238,119,17,0,0,85,0,0,187,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,255,255,102,0,0,0,0,0,119,255,255,255,255,255,119,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,17,238,255,255,255,255,255,255,187,0,0,0,0,0,0,0,0,0,
0,0,0,51,68,0,0,0,0,0,0,0,0,0,17,238,255,255,255,255,17,0,0,0,0,187,255,255,255,255,255,119,0,0,0,0,0,0,119,255,255,255,255,255,255,136,0,0,0,0,
0,0,102,255,255,255,255,255,34,0,0,0,0,51,68,0,0,0,0,0,0,0,0,0,17,238,255,255,255,255,17,0,0,0,0,0,187,255,255,255,255,255,238,51,0,0,0,0,0,0,
0,187,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,255,119,0,0,0,0,0,0,0,187,255,255,255,255,187,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,153,255,255,255,255,255,255,119,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,187,255,255,255,255,68,0,0,0,0,187,255,255,255,255,221,0,0,0,0,0,0,0,17,255,255,255,255,255,238,17,0,0,0,0,0,0,0,255,255,255,255,255,68,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,255,119,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,
187,255,255,255,255,221,0,0,0,0,0,0,0,0,102,255,255,255,255,221,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,51,255,255,255,255,255,255,255,221,17,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,153,255,255,255,255,68,0,0,0,
0,187,255,255,255,255,136,0,0,0,0,0,0,0,0,221,255,255,255,255,153,0,0,0,0,0,0,0,0,204,255,255,255,255,68,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,153,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,221,0,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,136,0,0,0,0,0,0,
0,0,68,255,255,255,255,255,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,204,255,255,255,
255,255,255,255,255,136,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,68,85,119,153,187,238,255,255,255,255,255,255,68,0,0,0,0,187,255,255,255,255,102,0,0,0,0,0,
0,0,0,187,255,255,255,255,119,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,68,85,119,153,187,238,255,255,255,255,255,255,68,0,0,0,0,0,
187,255,255,255,255,153,0,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,85,0,0,0,0,0,0,0,0,0,255,255,255,255,255,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,119,255,255,255,255,170,255,255,255,255,255,51,0,0,0,0,0,
0,0,0,0,0,0,0,85,170,255,255,255,255,255,255,255,255,255,255,255,255,255,68,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,
0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,85,170,255,255,255,255,255,255,255,255,255,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,119,0,0,0,0,0,0,
0,0,0,187,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,255,255,255,255,255,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,51,255,255,255,255,255,34,153,255,255,255,255,204,0,0,0,0,0,0,0,0,0,0,17,187,255,255,255,255,255,
255,255,255,255,255,255,255,255,255,255,68,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,
0,0,0,17,187,255,255,255,255,255,255,255,255,255,255,255,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,
0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,255,255,255,255,255,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,187,255,255,255,255,153,0,17,238,255,255,255,255,119,0,0,0,0,0,0,0,0,17,204,255,255,255,255,255,255,204,187,119,119,68,153,255,255,255,255,68,0,
0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,17,204,255,255,255,255,255,255,204,187,
119,119,68,153,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,
0,0,0,0,0,255,255,255,255,255,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,119,255,255,255,255,
238,17,0,0,119,255,255,255,255,238,34,0,0,0,0,0,0,0,153,255,255,255,255,204,85,0,0,0,0,0,0,119,255,255,255,255,68,0,0,0,0,187,255,255,255,255,68,0,0,0,
0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,153,255,255,255,255,204,85,0,0,0,0,0,0,119,255,255,255,255,68,0,0,0,
0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,255,255,255,255,255,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,34,238,255,255,255,255,119,0,0,0,17,221,255,255,255,255,170,0,
0,0,0,0,0,0,238,255,255,255,221,17,0,0,0,0,0,0,0,136,255,255,255,255,68,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,
0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,238,255,255,255,221,17,0,0,0,0,0,0,0,136,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,
0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,255,255,255,255,255,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,187,255,255,255,255,221,0,0,0,0,0,102,255,255,255,255,255,85,0,0,0,0,0,68,255,255,255,255,119,0,
0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,187,255,255,255,255,
68,0,0,68,255,255,255,255,119,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,
0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,255,255,255,255,255,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,119,255,255,255,255,255,85,0,0,0,0,0,0,187,255,255,255,255,221,17,0,0,0,0,68,255,255,255,255,119,0,0,0,0,0,0,0,17,255,255,255,255,255,
68,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,68,255,255,255,255,119,0,0,0,
0,0,0,0,17,255,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,0,0,
0,0,0,0,0,0,0,255,255,255,255,255,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,34,238,255,255,255,255,
187,0,0,0,0,0,0,0,68,255,255,255,255,255,136,0,0,0,0,102,255,255,255,255,136,0,0,0,0,0,0,0,153,255,255,255,255,255,68,0,0,0,0,187,255,255,255,255,68,0,
0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,102,255,255,255,255,136,0,0,0,0,0,0,0,153,255,255,255,255,255,68,0,
0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,255,255,255,255,255,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,170,255,255,255,255,255,51,0,0,0,0,0,0,0,0,170,255,255,
255,255,255,51,0,0,0,68,255,255,255,255,238,51,0,0,0,0,0,119,255,255,255,255,255,255,68,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,187,255,255,255,255,
68,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,68,255,255,255,255,238,51,0,0,0,0,0,119,255,255,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,0,0,
0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,255,255,255,255,255,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,85,255,255,255,255,255,136,0,0,0,0,0,0,0,0,0,34,255,255,255,255,255,204,0,0,0,0,238,255,255,255,
255,238,136,68,68,119,221,255,255,204,255,255,255,255,68,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,187,255,255,
255,255,68,0,0,0,238,255,255,255,255,238,136,68,68,119,221,255,255,204,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,187,255,255,255,255,
68,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,255,255,255,255,255,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,17,238,255,255,255,255,238,17,0,0,0,0,0,0,0,0,0,0,136,255,255,255,255,255,119,0,0,0,119,255,255,255,255,255,255,255,255,255,255,255,170,119,255,255,
255,255,68,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,119,255,255,255,255,255,
255,255,255,255,255,255,170,119,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,
0,0,0,0,0,0,0,0,0,255,255,255,255,255,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,170,255,255,255,255,255,
119,0,0,0,0,0,0,0,0,0,0,0,17,238,255,255,255,255,238,34,0,0,0,204,255,255,255,255,255,255,255,255,255,204,17,119,255,255,255,255,68,0,0,0,0,187,255,255,255,255,
68,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,204,255,255,255,255,255,255,255,255,255,204,17,119,255,255,255,255,
68,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,255,255,255,
255,255,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,85,255,255,255,255,255,221,0,0,0,0,0,0,0,0,0,0,0,0,
0,119,255,255,255,255,255,170,0,0,0,0,119,238,255,255,255,255,255,238,119,0,0,119,255,255,255,255,68,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,187,255,255,
255,255,68,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,119,238,255,255,255,255,255,238,119,0,0,119,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,
0,0,0,0,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,187,255,255,255,255,68,0,0,0,0,0,0,0,0,0,255,255,255,255,255,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,17,85,119,119,119,85,17,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,17,85,119,119,119,85,17,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,

		};
        #endregion



    }

    public class JointTest : PhysicsTest
    {
        public JointTest()
        {

        }

        void UpdateOnce(float delta)
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();

            float width = (Window.WindowSizeInPixels.Width - 10) / 4;
            float height = (Window.WindowSizeInPixels.Height - 50) / 4;


            CCNode node = new CCNode();
            CCPhysicsBody box = new CCPhysicsBody();
            node.PhysicsBody = box;
            box.SetDynamic(false);
            node.Position = CCPoint.Zero;
            AddChild(node);

            CCPhysicsJoint joint;
            CCSprite sp1, sp2;

            for (int i = 0; i < 4; i++)
            {

                for (int j = 0; j < 4; j++)
                {

                    CCPoint offset = new CCPoint(
                       0 + 5 + j * width + width / 2,
                       0 + 50 + i * height + height / 2
                       );
                    //CCPoint offset = new CCPoint()
                    box.AddShape(
                        new CCPhysicsShapeEdgeBox(
                            new CCSize(width, height), CCPhysicsMaterial.PHYSICSSHAPE_MATERIAL_DEFAULT,
                            offset, 1)
                        );

                    switch (i * 4 + j)
                    {
                        case 0:

                            sp1 = MakeBall(offset - new CCPoint(30, 0), 10);
                            sp1.PhysicsBody.Tag = DRAG_BODYS_TAG;
                            sp2 = MakeBall(offset + new CCPoint(30, 0), 10);
                            sp2.PhysicsBody.Tag = DRAG_BODYS_TAG;

                            joint = CCPhysicsJointPin.Construct(sp1.PhysicsBody, sp2.PhysicsBody, offset);
                            Scene.PhysicsWorld.AddJoint(joint);

                            AddChild(sp1);
                            AddChild(sp2);

                            break;

                        case 1:


                            sp1 = MakeBall(offset - new CCPoint(30, 0), 10);
                            sp1.PhysicsBody.Tag = DRAG_BODYS_TAG;
                            sp2 = MakeBox(offset + new CCPoint(30, 0), new CCSize(30, 10));
                            sp2.PhysicsBody.Tag = DRAG_BODYS_TAG;

                            joint = CCPhysicsJointFixed.Construct(sp1.PhysicsBody, sp2.PhysicsBody, offset);
                            Scene.PhysicsWorld.AddJoint(joint);

                            AddChild(sp1);
                            AddChild(sp2);

                            break;

                        case 2:


                            sp1 = MakeBall(offset - new CCPoint(30, 0), 10);
                            sp1.PhysicsBody.Tag = DRAG_BODYS_TAG;
                            sp2 = MakeBox(offset + new CCPoint(30, 0), new CCSize(30, 10));
                            sp2.PhysicsBody.Tag = DRAG_BODYS_TAG;

                            joint = CCPhysicsJointDistance.Construct(sp1.PhysicsBody, sp2.PhysicsBody, CCPoint.Zero, CCPoint.Zero);
                            Scene.PhysicsWorld.AddJoint(joint);

                            AddChild(sp1);
                            AddChild(sp2);


                            break;

                        case 3:

                            sp1 = MakeBall(offset - new CCPoint(30, 0), 10);
                            sp1.PhysicsBody.Tag = DRAG_BODYS_TAG;
                            sp2 = MakeBox(offset + new CCPoint(30, 0), new CCSize(30, 10));
                            sp2.PhysicsBody.Tag = DRAG_BODYS_TAG;

                            joint =
                     CCPhysicsJointSpring.Construct(sp1.PhysicsBody,
                     sp2.PhysicsBody, CCPoint.Zero, CCPoint.Zero, 500, 0.3f);
                            Scene.PhysicsWorld.AddJoint(joint);

                            AddChild(sp1);
                            AddChild(sp2);


                            break;

                        case 4:


                            sp1 = MakeBall(offset - new CCPoint(30, 0), 10);
                            sp1.PhysicsBody.Tag = DRAG_BODYS_TAG;
                            sp2 = MakeBox(offset + new CCPoint(30, 0), new CCSize(30, 10));
                            sp2.PhysicsBody.Tag = DRAG_BODYS_TAG;

                            joint = CCPhysicsJointGroove.Construct(sp1.PhysicsBody, sp2.PhysicsBody, new CCPoint(30, 15), new CCPoint(30, -15), new CCPoint(-30, 0));
                            Scene.PhysicsWorld.AddJoint(joint);

                            AddChild(sp1);
                            AddChild(sp2);


                            break;

                        case 5:

                            sp1 = MakeBall(offset - new CCPoint(30, 0), 10);
                            sp1.PhysicsBody.Tag = DRAG_BODYS_TAG;
                            sp2 = MakeBox(offset + new CCPoint(30, 0), new CCSize(30, 10));
                            sp2.PhysicsBody.Tag = DRAG_BODYS_TAG;

                            joint = CCPhysicsJointGroove.Construct(sp1.PhysicsBody, sp2.PhysicsBody, new CCPoint(30, 15), new CCPoint(30, -15), new CCPoint(-30, 0));
                            Scene.PhysicsWorld.AddJoint(joint);

                            AddChild(sp1);
                            AddChild(sp2);

                            break;

                        case 6:

                            sp1 = MakeBox(offset - new CCPoint(30, 0), new CCSize(30, 10));
                            sp1.PhysicsBody.Tag = DRAG_BODYS_TAG;
                            sp2 = MakeBox(offset + new CCPoint(30, 0), new CCSize(30, 10));
                            sp2.PhysicsBody.Tag = DRAG_BODYS_TAG;

                            Scene.PhysicsWorld.AddJoint(CCPhysicsJointPin.Construct(sp1.PhysicsBody, sp2.PhysicsBody, sp1.Position));
                            Scene.PhysicsWorld.AddJoint(CCPhysicsJointPin.Construct(sp1.PhysicsBody, sp2.PhysicsBody, sp1.Position));
                            joint = CCPhysicsJointRotarySpring.Construct(sp1.PhysicsBody, sp2.PhysicsBody, 3000.0f, 60.0f);

                            Scene.PhysicsWorld.AddJoint(joint);

                            AddChild(sp1);
                            AddChild(sp2);

                            break;
                        case 7:

                            sp1 = MakeBox(offset - new CCPoint(30, 0), new CCSize(30, 10));
                            sp1.PhysicsBody.Tag = DRAG_BODYS_TAG;
                            sp2 = MakeBox(offset + new CCPoint(30, 0), new CCSize(30, 10));
                            sp2.PhysicsBody.Tag = DRAG_BODYS_TAG;

                            Scene.PhysicsWorld.AddJoint(CCPhysicsJointPin.Construct(sp1.PhysicsBody, sp2.PhysicsBody, sp1.Position));
                            Scene.PhysicsWorld.AddJoint(CCPhysicsJointPin.Construct(sp1.PhysicsBody, sp2.PhysicsBody, sp1.Position));
                            joint = CCPhysicsJointRotaryLimit.Construct(sp1.PhysicsBody, sp2.PhysicsBody, 0.0f, ChipmunkSharp.cp.M_PI_2);

                            Scene.PhysicsWorld.AddJoint(joint);

                            AddChild(sp1);
                            AddChild(sp2);

                            break;
                        case 8:

                            sp1 = MakeBox(offset - new CCPoint(30, 0), new CCSize(30, 10));
                            sp1.PhysicsBody.Tag = DRAG_BODYS_TAG;
                            sp2 = MakeBox(offset + new CCPoint(30, 0), new CCSize(30, 10));
                            sp2.PhysicsBody.Tag = DRAG_BODYS_TAG;

                            Scene.PhysicsWorld.AddJoint(CCPhysicsJointPin.Construct(sp1.PhysicsBody, sp2.PhysicsBody, sp1.Position));
                            Scene.PhysicsWorld.AddJoint(CCPhysicsJointPin.Construct(sp1.PhysicsBody, sp2.PhysicsBody, sp1.Position));
                            joint = CCPhysicsJointRatchet.Construct(sp1.PhysicsBody, sp2.PhysicsBody, 0.0f, ChipmunkSharp.cp.M_PI_2);

                            Scene.PhysicsWorld.AddJoint(joint);

                            AddChild(sp1);
                            AddChild(sp2);

                            break;
                        case 9:

                            sp1 = MakeBox(offset - new CCPoint(30, 0), new CCSize(30, 10));
                            sp1.PhysicsBody.Tag = DRAG_BODYS_TAG;
                            sp2 = MakeBox(offset + new CCPoint(30, 0), new CCSize(30, 10));
                            sp2.PhysicsBody.Tag = DRAG_BODYS_TAG;

                            Scene.PhysicsWorld.AddJoint(CCPhysicsJointPin.Construct(sp1.PhysicsBody, sp2.PhysicsBody, sp1.Position));
                            Scene.PhysicsWorld.AddJoint(CCPhysicsJointPin.Construct(sp1.PhysicsBody, sp2.PhysicsBody, sp1.Position));
                            joint = CCPhysicsJointGear.Construct(sp1.PhysicsBody, sp2.PhysicsBody, 0.0f, 2f);

                            Scene.PhysicsWorld.AddJoint(joint);

                            AddChild(sp1);
                            AddChild(sp2);

                            break;
                        case 10:

                            sp1 = MakeBox(offset - new CCPoint(30, 0), new CCSize(30, 10));
                            sp1.PhysicsBody.Tag = DRAG_BODYS_TAG;
                            sp2 = MakeBox(offset + new CCPoint(30, 0), new CCSize(30, 10));
                            sp2.PhysicsBody.Tag = DRAG_BODYS_TAG;

                            Scene.PhysicsWorld.AddJoint(CCPhysicsJointPin.Construct(sp1.PhysicsBody, sp2.PhysicsBody, sp1.Position));
                            Scene.PhysicsWorld.AddJoint(CCPhysicsJointPin.Construct(sp1.PhysicsBody, sp2.PhysicsBody, sp1.Position));
                            joint = CCPhysicsJointMotor.Construct(sp1.PhysicsBody, sp2.PhysicsBody, ChipmunkSharp.cp.M_PI_2);

                            Scene.PhysicsWorld.AddJoint(joint);

                            AddChild(sp1);
                            AddChild(sp2);

                            break;


                        default:
                            break;
                    }



                }

            }

            Schedule();


        }

        public override string Title
        {
            get
            {
                return "Join Test";
            }
        }
    }

}
