#if USE_PHYSICS
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
        RAYCAST,
        ACTIONS,
        PUMP,
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
                case (int)PhysicsTests.RAYCAST:
                    testLayer = new RayCastTest();
                    break;
                case (int)PhysicsTests.ACTIONS:
                    testLayer = new ActionsTest();
                    break;
                case (int)PhysicsTests.PUMP:
                    testLayer = new PumpTest();
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
            sceneIndex = sceneIndex % (int)PhysicsTests.TEST_CASE_COUNT;

            var testLayer = CreateLayer(sceneIndex);

            return testLayer;
        }

        public static CCLayer BackAction()
        {
            --sceneIndex;
            if (sceneIndex < 0)
                sceneIndex += (int)PhysicsTests.TEST_CASE_COUNT;

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
        Dictionary<int, CCNode> mouses;

        protected override void AddedToScene()
        {
            base.AddedToScene();

            mouses = new Dictionary<int, CCNode>();

            eTouch = new CCEventListenerTouchAllAtOnce();
            eTouch.OnTouchesBegan = OnTouchesBegan;
            eTouch.OnTouchesMoved = OnTouchesMoved;
            eTouch.OnTouchesEnded = OnTouchesEnded;
            AddEventListener(eTouch);

            eAccel = new CCEventListenerAccelerometer();
            eAccel.OnAccelerate = OnAccelerate;

        }

        public virtual void OnTouchesBegan(List<CCTouch> touches, CCEvent arg2)
        {
            CCTouch touch = touches.FirstOrDefault();
            CCPoint location = touch.Location;

            List<CCPhysicsShape> shapes = Scene.PhysicsWorld.GetShapes(location);

            CCPhysicsBody body = null;

            foreach (var obj in shapes)
            {
                if ((obj.Body.Tag & DRAG_BODYS_TAG) != 0)
                {
                    body = obj.Body;
                    break;
                }
            }

            if (body != null)
            {
                CCNode mouse = new CCNode();

                mouse.PhysicsBody = new CCPhysicsBody();
                mouse.PhysicsBody.IsDynamic = false;
                mouse.Position = location;
                AddChild(mouse);

                CCPhysicsJointPin join = CCPhysicsJointPin.Construct(mouse.PhysicsBody, body, location);
                join.SetMaxForce(5000 * body.GetMass());
                Scene.PhysicsWorld.AddJoint(join);
                mouses.Add(touch.Id, mouse);

            }


        }

        public virtual void OnTouchesMoved(List<CCTouch> touches, CCEvent arg2)
        {

            var touch = touches.FirstOrDefault();
            CCNode node;
            if (mouses.TryGetValue(touch.Id, out node))
            {
                node.Position = touch.Location;
            }

        }

        public virtual void OnTouchesEnded(List<CCTouch> touches, CCEvent e)
        {
            var touch = touches.FirstOrDefault();

            CCNode it;
            if (mouses.TryGetValue(touch.Id, out it))
            {
                RemoveChild(it);
                mouses.Remove(touch.Id);
            }

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

        public CCSprite MakeBox(CCPoint point, CCSize size)
        {
            return MakeBox(point, size, 0, CCPhysicsMaterial.PHYSICSSHAPE_MATERIAL_DEFAULT);
        }

        public CCSprite MakeBox(CCPoint point, CCSize size, int color, CCPhysicsMaterial material)
        {

            bool yellow = (color == 0) ? CCRandom.Float_0_1() > 0.5f : color == 1;

            CCSprite box = new CCSprite(yellow ? "Images/YellowSquare" : "Images/CyanSquare");

            box.ScaleX = size.Width / 100.0f;
            box.ScaleY = size.Height / 100.0f;

            var body = CCPhysicsBody.CreateBox(size, material, 0.0f);
            box.PhysicsBody = body;
            box.Position = point;// new cpVect(point.X, point.Y);

            return box;
        }

        public CCSprite MakeBall(CCPoint point, float radius)
        {
            return MakeBall(point, radius, CCPhysicsMaterial.PHYSICSSHAPE_MATERIAL_DEFAULT);
        }

        public CCSprite MakeBall(CCPoint point, float radius, CCPhysicsMaterial material)
        {
            CCSprite ball = new CCSprite("Images/ball");
            ball.Scale = 0.13f * radius;
            var body = CCPhysicsBody.CreateCircle(radius, material, CCPoint.Zero);
            ball.PhysicsBody = body;
            ball.Position = point;// new cpVect(point.X, point.Y);
            return ball;
        }

        public CCSprite MakeTriangle(CCPoint point, CCSize size)
        {
            return MakeTriangle(point, size, 0, CCPhysicsMaterial.PHYSICSSHAPE_MATERIAL_DEFAULT);
        }

        public CCSprite MakeTriangle(CCPoint point, CCSize size, int color, CCPhysicsMaterial material)
        {
            bool yellow = false;
            if (color == 0)
            {
                yellow = CCRandom.Float_0_1() > 0.5f;
            }
            else
            {
                yellow = color == 1;
            }

            var triangle = yellow ? new CCSprite("Images/YellowTriangle") : new CCSprite("Images/CyanTriangle");

            if (size.Height == 0)
            {
                triangle.Scale = size.Width / 100.0f;
            }
            else
            {
                triangle.ScaleX = size.Width / 50.0f;
                triangle.ScaleY = size.Height / 43.5f;
            }

            CCPoint[] vers = new CCPoint[] {
        new CCPoint(0, size.Height/2), 
        new CCPoint(size.Width/2, -size.Height/2),
        new CCPoint(-size.Width/2, -size.Height/2)
    };

            var body = CCPhysicsBody.CreateEdgePolygon(vers, 3, material);
            triangle.PhysicsBody = body;
            triangle.Position = new CCPoint(point.X, point.Y);

            return triangle;
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

            spriteTexture = new CCSpriteBatchNode("Images/grossini_dance_atlas", 100).Texture;

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

        public void ToggleDebug()
        {
            if (Scene.PhysicsWorld.debugDraw != null)
            {
                Scene.PhysicsWorld.DebugDrawMask = PhysicsDrawFlags.Shapes | PhysicsDrawFlags.Joints;
            }
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
                return "Physics Test";
            }
        }

        public override string Subtitle
        {
            get
            {
                return string.Empty;
            }
        }

        #region Position Properties

        public CCPoint Center { get { return new CCPoint(Window.WindowSizeInPixels.Width * .5f, Window.WindowSizeInPixels.Height * .5f); } }
        public CCPoint Left { get { return new CCPoint(0, Window.WindowSizeInPixels.Height * .5f); } }
        public CCPoint LeftBottom { get { return new CCPoint(0, 0); } }
        public CCPoint LeftTop { get { return new CCPoint(0, Window.WindowSizeInPixels.Height); } }
        public CCPoint RightBottom { get { return new CCPoint(Window.WindowSizeInPixels.Width, 0); } }
        public CCPoint RightTop { get { return new CCPoint(Window.WindowSizeInPixels.Width, Window.WindowSizeInPixels.Height); } }
        public CCPoint Right { get { return new CCPoint(Window.WindowSizeInPixels.Width, Window.WindowSizeInPixels.Height * .5f); } }
        public CCPoint Top { get { return new CCPoint(Window.WindowSizeInPixels.Width * .5f, Window.WindowSizeInPixels.Height); } }
        public CCPoint Bottom { get { return new CCPoint(Window.WindowSizeInPixels.Width * .5f, 0); } }

        #endregion

    }

    public class ClickAddTest : PhysicsTest
    {

        public override void OnEnter()
        {
            base.OnEnter();

            //Create a boundin box container room
            var node = new CCNode();
            node.PhysicsBody = CCPhysicsBody.CreateEdgeBox(VisibleBoundsWorldspace.Size, 1.0f, CCPoint.Zero);
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

        public override string Subtitle
        {
            get
            {
                return "Multi touch to add Grossini";
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

            for(int i=0; i<7; i++)
            {
                for(int j=0; j<=i; j++)
                {
                    var sp = AddGrossiniAtPosition(visibleRect.Bottom() + new CCPoint((i/2 - j) * 11, (14 - i) * 23 + 100), 0.2f);
                    sp.PhysicsBody.Tag = DRAG_BODYS_TAG;
                }
            }

            Schedule();

        }

        void UpdateOnce(float delta)
        {
            var ball = this[100];
            ball.ScaleX = ball.ScaleX * 3;
            ball.ScaleY = ball.ScaleY * 3;
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

            CCSpriteBatchNode _ball = new CCSpriteBatchNode("Images/ball.png");

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
                        2 * (x - image_width / 2 + x_jitter) + VisibleBoundsWorldspace.Size.Width * .5f,
                        2 * (image_height / 2 - y + y_jitter) + VisibleBoundsWorldspace.Size.Height * .5f
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

            bullet.Position = new CCPoint(200, VisibleBoundsWorldspace.Size.Height * .5f);

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

            float width = (VisibleBoundsWorldspace.Size.Width - 10) / 4;
            float height = (VisibleBoundsWorldspace.Size.Height - 50) / 4;

            Scene.PhysicsWorld.DebugDrawMask = PhysicsDrawFlags.Shapes | PhysicsDrawFlags.Joints;

            CCNode node = new CCNode();
            CCPhysicsBody box = new CCPhysicsBody();
            node.PhysicsBody = box;
            box.IsDynamic = false;
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


    public class RayCastTest : PhysicsTest
    {

        float _angle;
        CCDrawNode _node;
        int _mode;

        public CCColor4F STATIC_COLOR = new CCColor4F(1.0f, 0.0f, 0.0f, 1.0f);

        public RayCastTest()
        {
        }
        
        public override void OnEnter()
        {
            base.OnEnter();


            Scene.PhysicsWorld.Gravity = CCPoint.Zero;

            CCDrawNode node = new CCDrawNode();
            node.PhysicsBody = CCPhysicsBody.CreateEdgeSegment(
                new CCPoint(0, 50),
                new CCPoint(Window.WindowSizeInPixels.Width, 0) + new CCPoint(0, 50)
                );

            node.DrawSegment(new CCPoint(0, 50),
                new CCPoint(Window.WindowSizeInPixels.Width, 0) + new CCPoint(0, 50), 1, STATIC_COLOR);
            AddChild(node);

            var item = new CCMenuItemFont("Change Mode(any)", ChangeModeCallback);
            var menu = new CCMenu(item);
            AddChild(menu);

            menu.Position = new CCPoint(100, 100);

            Schedule();

        }

        public void ChangeModeCallback(object sender)
        {
            _mode = (_mode + 1) % 3;

            switch (_mode)
            {
                case 0:
                    ((CCMenuItemFont)sender).LabelTTF.Text = "Change Mode(any)";
                    break;
                case 1:
                    ((CCMenuItemFont)sender).LabelTTF.Text = "Change Mode(nearest)";
                    break;
                case 2:
                    ((CCMenuItemFont)sender).LabelTTF.Text = "Change Mode(multiple)";
                    break;

                default:
                    break;
            }
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            float L = 150.0f;
            CCPoint point1 = Window.WindowSizeInPixels.Center;
            CCPoint d = new CCPoint(L * (float)Math.Cos(_angle), L * (float)Math.Sin(_angle));
            CCPoint point2 = point1 + d;

            RemoveChild(_node);
            _node = new CCDrawNode();
            switch (_mode)
            {
                case 0:
                    {
                        CCPoint point3 = point2;
                        //var func = Func< (PhysicsDemoRayCast::anyRay, this);

                        var ctx = new RayCastContext(point3);

                        Scene.PhysicsWorld.RayCast((world, info, data) =>
                        {
                            point3 = info.Contact;
                            return false;

                        }, point1, point2, ctx);

                        _node.DrawSegment(point1, point3, 1, STATIC_COLOR);

                        if (point2 != point3)
                        {
                            _node.DrawDot(point3, 2, new CCColor4F(1.0f, 1.0f, 1.0f, 1.0f));
                        }

                        AddChild(_node);

                        break;
                    }
                case 1:
                    {
                        CCPoint point3 = point2;
                        float friction = 1.0f;

                        Func<CCPhysicsWorld, CCPhysicsRayCastInfo, RayCastContext, bool> func = new Func<CCPhysicsWorld, CCPhysicsRayCastInfo, RayCastContext, bool>(
                           (world, info, ctx) =>
                           {
                               if (friction > info.Fraction)
                               {
                                   point3 = info.Contact;
                                   friction = info.Fraction;
                               }

                               return true;
                           });

                        _node.DrawSegment(point1, point3, 1, STATIC_COLOR);

                        if (point2 != point3)
                        {
                            _node.DrawDot(point3, 2, new CCColor4F(1.0f, 1.0f, 1.0f, 1.0f));
                        }
                        AddChild(_node);

                        break;
                    }
                case 2:
                    {
                        int MAX_MULTI_RAYCAST_NUM = 5;
                        CCPoint[] points = new CCPoint[MAX_MULTI_RAYCAST_NUM];
                        int num = 0;

                        Func<CCPhysicsWorld, CCPhysicsRayCastInfo, RayCastContext, bool> func = new Func<CCPhysicsWorld, CCPhysicsRayCastInfo, RayCastContext, bool>(
                             (world, info, data) =>
                             {
                                 if (num < MAX_MULTI_RAYCAST_NUM)
                                 {
                                     points[num++] = info.Contact;
                                 }

                                 return true;
                             });

                        // Scene.PhysicsWorld.RayCast(func, point1, point2, null);

                        _node.DrawSegment(point1, point2, 1, STATIC_COLOR);

                        for (int i = 0; i < num; ++i)
                        {
                            _node.DrawDot(points[i], 2, new CCColor4F(1.0f, 1.0f, 1.0f, 1.0f));
                        }

                        AddChild(_node);

                        break;
                    }

                default:
                    break;
            }

            _angle += 0.25f * (float)cp.M_PI / 180.0f;


        }

        public override void OnTouchesEnded(List<CCTouch> touches, CCEvent e)
        {
            base.OnTouchesEnded(touches, e);

            CCPoint location = touches.FirstOrDefault().Location;
            //location.Y = Window.WindowSizeInPixels.Height - location.Y;

            float r = CCRandom.Float_0_1();

            if (r < 1.0f / 3.0f)
            {
                AddChild(MakeBall(location, 5 + CCRandom.Float_0_1() * 10));
            }
            else if (r < 2.0f / 3.0f)
            {
                AddChild(MakeBox(location, new CCSize(10 + CCRandom.Float_0_1() * 15, 10 + CCRandom.Float_0_1() * 15)));
            }
            else
            {
                AddChild(MakeTriangle(location, new CCSize(10 + CCRandom.Float_0_1() * 20, 10 + CCRandom.Float_0_1() * 20)));
            }

        }

        public override string Title
        {
            get
            {
                return "RayCast Test";
            }
        }


    }

    public class ActionsTest : PhysicsTest
    {
        public ActionsTest()
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            Scene.PhysicsWorld.Gravity = CCPoint.Zero;

            //Create a boundin box container room
            var node = new CCNode();
            node.PhysicsBody = CCPhysicsBody.CreateEdgeBox(Window.WindowSizeInPixels, 1.0f, CCPoint.Zero);
            node.Position = Window.WindowSizeInPixels.Center;
            AddChild(node);

            //drops a grosini sprite on center on window
            CCSprite sp1 = AddGrossiniAtPosition(Center);
            CCSprite sp2 = AddGrossiniAtPosition(Left + new CCPoint(50, 0));
            CCSprite sp3 = AddGrossiniAtPosition(Right + new CCPoint(20, 0));
            CCSprite sp4 = AddGrossiniAtPosition(LeftTop + new CCPoint(50, -50));

            sp1.PhysicsBody.Tag = DRAG_BODYS_TAG;
            sp2.PhysicsBody.Tag = DRAG_BODYS_TAG;
            sp3.PhysicsBody.Tag = DRAG_BODYS_TAG;
            sp4.PhysicsBody.Tag = DRAG_BODYS_TAG;

            var actionTo = new CCJumpTo(2, new CCPoint(100, 100), 50, 4);
            var actionBy = new CCJumpTo(2, new CCPoint(300, 0), 50, 4);
            var actionUp = new CCJumpTo(2, new CCPoint(0, 50), 80, 4);
            var actionByBack = actionBy.Reverse();
            var rotateBy = new CCRotateBy(2, 180);
            var rotateByBack = new CCRotateBy(2, -180);

            sp1.RunAction(new CCRepeatForever(actionUp));
            sp2.RunAction(new CCRepeatForever(actionBy, actionByBack));
            sp3.RunAction(actionTo);
            sp4.RunAction(new CCRepeatForever(rotateBy, rotateByBack));

            ToggleDebug();

            Schedule();
        }
        

        public override string Title
        {
            get
            {
                return "Actions Test";
            }
        }

    }

    public class PumpTest : PhysicsTest
    {

        float _distance;
        float _rotationV;


        public PumpTest()
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            ToggleDebug();

            _distance = 0.0f;
            _rotationV = 0.0f;

            Scene.Scale = 0.5f;

            //Create a boundin box container room
            var node = new CCNode();
            var body = new CCPhysicsBody();
            body.IsDynamic = false;

            CCPhysicsMaterial staticMaterial = new CCPhysicsMaterial(cp.PHYSICS_INFINITY,
                0, 0.5f);

            body.AddShape(new CCPhysicsShapeEdgeSegment(LeftTop + new CCPoint(50, 0), LeftTop + new CCPoint(50, -130), staticMaterial, 2.0f));
            body.AddShape(new CCPhysicsShapeEdgeSegment(LeftTop + new CCPoint(190, 0), LeftTop + new CCPoint(100, -50), staticMaterial, 2.0f));
            body.AddShape(new CCPhysicsShapeEdgeSegment(LeftTop + new CCPoint(100, -50), LeftTop + new CCPoint(100, -90), staticMaterial, 2.0f));
            body.AddShape(new CCPhysicsShapeEdgeSegment(LeftTop + new CCPoint(50, -130), LeftTop + new CCPoint(100, -145), staticMaterial, 2.0f));
            body.AddShape(new CCPhysicsShapeEdgeSegment(LeftTop + new CCPoint(100, -145), LeftBottom + new CCPoint(100, 80), staticMaterial, 2.0f));
            body.AddShape(new CCPhysicsShapeEdgeSegment(LeftTop + new CCPoint(150, -80), LeftBottom + new CCPoint(150, 80), staticMaterial, 2.0f));
            body.AddShape(new CCPhysicsShapeEdgeSegment(LeftTop + new CCPoint(150, -80), RightTop + new CCPoint(-100, -150), staticMaterial, 2.0f));

            body.SetCategoryBitmask(0x01);

            for (int i = 0; i < 6; ++i)
            {
                var ball = MakeBall(LeftTop + new CCPoint(75 + CCRandom.Float_0_1() * 90, 0), 22, new CCPhysicsMaterial(0.05f, 0, 0.1f));
                ball.PhysicsBody.Tag = DRAG_BODYS_TAG;
                AddChild(ball);
            }

            node.PhysicsBody = body;
            AddChild(node);

            CCPoint[] vec = new CCPoint[4] {
				new CCPoint(LeftTop + new CCPoint(102,-148)),
				new CCPoint(LeftTop + new CCPoint(148,-161)),
				new CCPoint(LeftBottom + new CCPoint(148,20)),
				new CCPoint(LeftBottom + new CCPoint(102,20))
			};

            var world = Scene.PhysicsWorld;

            // small gear
            var sgear = new CCNode();// Node::create();
            var sgearB = CCPhysicsBody.CreateCircle(44, CCPoint.Zero);
            sgear.PhysicsBody = sgearB;
            sgear.Position = LeftBottom + new CCPoint(125, 0);
            this.AddChild(sgear);
            //sgearB.SetCategoryBitmask(0x04);
            //sgearB.SetCollisionBitmask(0x04);
            sgearB.Tag = 1;
            world.AddJoint(CCPhysicsJointPin.Construct(body, sgearB, sgearB.Position));

            // big gear
            var bgear = new CCNode();
            var bgearB = CCPhysicsBody.CreateCircle(100);
            bgear.PhysicsBody = (bgearB);
            bgear.Position = LeftBottom + new CCPoint(275, 0);
            this.AddChild(bgear);
            //bgearB.SetCategoryBitmask(0x04);
            world.AddJoint(CCPhysicsJointPin.Construct(body, bgearB, bgearB.Position));

            // pump
            var pump = new CCNode();
            var center = CCPhysicsShape.GetPolygonCenter(vec, 4);
            pump.Position = center;
            var pumpB = CCPhysicsBody.CreatePolygon(vec, 4, CCPhysicsMaterial.PHYSICSSHAPE_MATERIAL_DEFAULT, 0.0f);
            pump.PhysicsBody = pumpB;
            this.AddChild(pump);
            //pumpB.SetCategoryBitmask(0x02);
            pumpB.SetGravityEnable(false);
            world.AddJoint(CCPhysicsJointDistance.Construct(pumpB, sgearB, new CCPoint(0, 0), new CCPoint(0, -44)));

            // plugger
            CCPoint[] seg = new CCPoint[] { LeftTop + new CCPoint(75, -120), LeftBottom + new CCPoint(75, -100) };
            CCPoint segCenter = (seg[1] + seg[0]) / 2;
            seg[1] -= segCenter;
            seg[0] -= segCenter;
            var plugger = new CCNode();
            var pluggerB = CCPhysicsBody.CreateEdgeSegment(seg[0], seg[1], new CCPhysicsMaterial(0.01f, 0.0f, 0.5f), 20);
            pluggerB.IsDynamic = true;
            pluggerB.SetMass(30);
            pluggerB.Moment = 100000;
            plugger.PhysicsBody = pluggerB;
            plugger.Position = segCenter;
            this.AddChild(plugger);
            //pluggerB.SetCategoryBitmask(0x02);
            //sgearB.SetCollisionBitmask(0x04 | 0x01);
            world.AddJoint(CCPhysicsJointPin.Construct(body, pluggerB, LeftBottom + new CCPoint(75, -90)));
            world.AddJoint(CCPhysicsJointDistance.Construct(pluggerB, sgearB, pluggerB.World2Local(LeftBottom + new CCPoint(75, 0)), new CCPoint(44, 0)));

            //drops a grosini sprite on center on window

            Schedule();
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            foreach (var body in Scene.PhysicsWorld.Bodies)
            {
                if (body.Tag == DRAG_BODYS_TAG && body.Position.Y < 0.0f)
                {
                    body.Node.Position = LeftTop + new CCPoint(75 + CCRandom.Float_0_1() * 90, 0);
                    body.SetVelocity(CCPoint.Zero);
                }
            }

            CCPhysicsBody gear = Scene.PhysicsWorld.GetBody(1);

            if (gear != null)
            {
                if (_distance != 0.0f)
                {
                    _rotationV += _distance / 2500.0f;

                    if (_rotationV > 30) _rotationV = 30.0f;
                    if (_rotationV < -30) _rotationV = -30.0f;
                }

                gear.SetAngularVelocity(_rotationV);
                _rotationV *= 0.995f;
            }

        }

        public override void OnTouchesBegan(List<CCTouch> touches, CCEvent arg2)
        {
            base.OnTouchesBegan(touches, arg2);
            CCTouch touch = touches.FirstOrDefault();
            _distance = touch.Location.X - Center.X;
        }

        public override void OnTouchesMoved(List<CCTouch> touches, CCEvent arg2)
        {
            base.OnTouchesMoved(touches, arg2);
            CCTouch touch = touches.FirstOrDefault();
            _distance = touch.Location.X - Center.X;
        }

        public override void OnTouchesEnded(List<CCTouch> touches, CCEvent e)
        {
            base.OnTouchesEnded(touches, e);
            _distance = 0;

        }


    }


}
#endif
