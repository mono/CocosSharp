using System;
using System.Collections.Generic;
using Box2D;
using Box2D.Common;
using Box2D.Collision.Shapes;
using Box2D.Dynamics;
using Microsoft.Xna.Framework;
using Cocos2D;
using Random = Cocos2D.CCRandom;

namespace tests
{
    internal class CCPhysicsSprite : CCSprite
    {
        private b2Body m_pBody; // strong ref

        public CCPhysicsSprite(CCTexture2D f, CCRect r)
            : base(f, r)
        {
        }

        public override bool Dirty
        {
            get { return true; }
            set { base.Dirty = value; }
        }

        public b2Body PhysicsBody { get { return (m_pBody); } set { m_pBody = value; } }

        public override CCAffineTransform NodeToParentTransform()
        {
            b2Vec2 pos = m_pBody.Position;

            float x = pos.x * Box2DTestLayer.PTM_RATIO;
            float y = pos.y * Box2DTestLayer.PTM_RATIO;

            if (IgnoreAnchorPointForPosition)
            {
                x += m_obAnchorPointInPoints.X;
                y += m_obAnchorPointInPoints.Y;
            }

            // Make matrix
            float radians = m_pBody.Angle;
            var c = (float)Math.Cos(radians);
            var s = (float)Math.Sin(radians);

            if (!m_obAnchorPointInPoints.Equals(CCPoint.Zero))
            {
                x += c * -m_obAnchorPointInPoints.X + -s * -m_obAnchorPointInPoints.Y;
                y += s * -m_obAnchorPointInPoints.X + c * -m_obAnchorPointInPoints.Y;
            }

            // Rot, Translate Matrix
            m_sTransform = new CCAffineTransform(c, s,
                                                 -s, c,
                                                 x, y);

            return m_sTransform;
        }
    }

    public class Box2DTestLayer : CCLayer
    {
        public class Myb2Listener : b2ContactListener
        {

            public override void PreSolve(Box2D.Dynamics.Contacts.b2Contact contact, ref Box2D.Collision.b2Manifold oldManifold)
            {
            }

            public override void PostSolve(Box2D.Dynamics.Contacts.b2Contact contact, ref b2ContactImpulse impulse)
            {
            }
        }

        public const int PTM_RATIO = 32;

        private const int kTagParentNode = 1;
        private readonly CCTexture2D m_pSpriteTexture; // weak ref
        private b2World _world;
        private CCSpriteBatchNode _batch;

        public Box2DTestLayer()
        {
            TouchEnabled = true;
            AccelerometerEnabled = true;
            CCSize s = CCDirector.SharedDirector.WinSize;
            // init physics
            initPhysics();
            // create reset button
            createResetButton();

            //Set up sprite
            // Use batch node. Faster
            _batch = new CCSpriteBatchNode("Images/blocks", 100);
            m_pSpriteTexture = _batch.Texture;
            AddChild(_batch, 0, kTagParentNode);

            addNewSpriteAtPosition(new CCPoint(s.Width / 2, s.Height / 2));

            CCLabelTTF label = new CCLabelTTF("Tap screen", "MarkerFelt", 32);
            AddChild(label, 0);
            label.Color = new CCColor3B(0, 0, 255);
            label.Position = new CCPoint(s.Width / 2, s.Height - 50);

            ScheduleUpdate();
        }


        private void initPhysics()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            var gravity = new b2Vec2(0.0f, -10.0f);
            _world = new b2World(gravity);
            float debugWidth = s.Width / PTM_RATIO * 2f;
            float debugHeight = s.Height / PTM_RATIO * 2f;
            CCDraw debugDraw = new CCDraw(new b2Vec2(debugWidth / 2f + 10, s.Height - debugHeight - 10), 2);
            debugDraw.AppendFlags(b2DrawFlags.e_shapeBit);
            _world.SetDebugDraw(debugDraw);
            _world.SetAllowSleeping(true);
            _world.SetContinuousPhysics(true);

            //m_debugDraw = new GLESDebugDraw( PTM_RATIO );
            //world->SetDebugDraw(m_debugDraw);

            //uint32 flags = 0;
            //flags += b2Draw::e_shapeBit;
            //        flags += b2Draw::e_jointBit;
            //        flags += b2Draw::e_aabbBit;
            //        flags += b2Draw::e_pairBit;
            //        flags += b2Draw::e_centerOfMassBit;
            //m_debugDraw->SetFlags(flags);


            // Call the body factory which allocates memory for the ground body
            // from a pool and creates the ground box shape (also from a pool).
            // The body is also added to the world.
            b2BodyDef def = new b2BodyDef();
            def.allowSleep = true;
            def.position = b2Vec2.Zero;
            def.type = b2BodyType.b2_staticBody;
            b2Body groundBody = _world.CreateBody(def);
            groundBody.SetActive(true);

            // Define the ground box shape.

            // bottom
            b2EdgeShape groundBox = new b2EdgeShape();
            groundBox.Set(b2Vec2.Zero, new b2Vec2(s.Width / PTM_RATIO, 0));
            b2FixtureDef fd = new b2FixtureDef();
            fd.shape = groundBox;
            groundBody.CreateFixture(fd);

            // top
            groundBox = new b2EdgeShape();
            groundBox.Set(new b2Vec2(0, s.Height / PTM_RATIO), new b2Vec2(s.Width / PTM_RATIO, s.Height / PTM_RATIO));
            fd.shape = groundBox;
            groundBody.CreateFixture(fd);

            // left
            groundBox = new b2EdgeShape();
            groundBox.Set(new b2Vec2(0, s.Height / PTM_RATIO), b2Vec2.Zero);
            fd.shape = groundBox;
            groundBody.CreateFixture(fd);

            // right
            groundBox = new b2EdgeShape();
            groundBox.Set(new b2Vec2(s.Width / PTM_RATIO, s.Height / PTM_RATIO), new b2Vec2(s.Width / PTM_RATIO, 0));
            fd.shape = groundBox;
            groundBody.CreateFixture(fd);

            // _world.Dump();
        }

        public void createResetButton()
        {
            CCMenuItemImage res = new CCMenuItemImage("Images/r1", "Images/r2", reset);

            CCMenu menu = new CCMenu(res);

            CCSize s = CCDirector.SharedDirector.WinSize;

            menu.Position = new CCPoint(s.Width / 2, 30);
            AddChild(menu, -1);
        }

        public void reset(object sender)
        {
            CCScene s = new Box2DTestScene();
            var child = new Box2DTestLayer();
            s.AddChild(child);
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public override void Draw()
        {
            //
            // IMPORTANT:
            // This is only for debug purposes
            // It is recommend to disable it
            //
            base.Draw();

            //ccGLEnableVertexAttribs( kCCVertexAttribFlag_Position );

            //kmGLPushMatrix();

            CCDrawingPrimitives.Begin();
            _world.DrawDebugData();
            CCDrawingPrimitives.End();

            //world.DrawDebugData();

            //kmGLPopMatrix();
        }

        private const int kTagForPhysicsSprite = 99999;

        public void addNewSpriteAtPosition(CCPoint p)
        {
            CCLog.Log("Add sprite #{2} : {0} x {1}", p.X, p.Y, _batch.ChildrenCount + 1);

            //We have a 64x64 sprite sheet with 4 different 32x32 images.  The following code is
            //just randomly picking one of the images
            int idx = (CCRandom.Float_0_1() > .5 ? 0 : 1);
            int idy = (CCRandom.Float_0_1() > .5 ? 0 : 1);
            var sprite = new CCPhysicsSprite(m_pSpriteTexture, new CCRect(32 * idx, 32 * idy, 32, 32));

            _batch.AddChild(sprite, 0, kTagForPhysicsSprite);

            sprite.Position = new CCPoint(p.X, p.Y);

            // Define the dynamic body.
            //Set up a 1m squared box in the physics world
            b2BodyDef def = new b2BodyDef();
            def.position = new b2Vec2(p.X / PTM_RATIO, p.Y / PTM_RATIO);
            def.type = b2BodyType.b2_dynamicBody;
            b2Body body = _world.CreateBody(def);
            // Define another box shape for our dynamic body.
            var dynamicBox = new b2PolygonShape();
            dynamicBox.SetAsBox(.5f, .5f); //These are mid points for our 1m box

            // Define the dynamic body fixture.
            b2FixtureDef fd = new b2FixtureDef();
            fd.shape = dynamicBox;
            fd.density = 1f;
            fd.friction = 0.3f;
            b2Fixture fixture = body.CreateFixture(fd);

            sprite.PhysicsBody = body;
            //_world.SetContactListener(new Myb2Listener());

            // _world.Dump();
        }

        public override void Update(float dt)
        {
            _world.Step(dt, 8, 1);

            foreach (CCPhysicsSprite sprite in _batch.Children)
            {
                if (sprite.Visible && sprite.PhysicsBody.Position.y < 0f)
                {
                    _world.DestroyBody(sprite.PhysicsBody);
                    sprite.Visible = false;
                }
            }

#if WINDOWS || WINDOWSGL || LINUX || MACOS
            CCInputState.Instance.Update(dt);
            PlayerIndex p;
            if (CCInputState.Instance.IsKeyPress(Microsoft.Xna.Framework.Input.Keys.D, PlayerIndex.One, out p))
            {
                _world.Dump();
#if PROFILING
                b2Profile profile = _world.Profile;
                CCLog.Log("]-----------[{0:F4}]-----------------------[", profile.step);
                CCLog.Log("Solve Time = {0:F4}", profile.solve);
                CCLog.Log("# bodies = {0}", profile.bodyCount);
                CCLog.Log("# contacts = {0}", profile.contactCount);
                CCLog.Log("# joints = {0}", profile.jointCount);
                CCLog.Log("# toi iters = {0}", profile.toiSolverIterations);
                if (profile.step > 0f)
                {
                    CCLog.Log("Solve TOI Time = {0:F4} {1:F2}%", profile.solveTOI, profile.solveTOI / profile.step * 100f);
                    CCLog.Log("Solve TOI Advance Time = {0:F4} {1:F2}%", profile.solveTOIAdvance, profile.solveTOIAdvance / profile.step * 100f);
                }

                CCLog.Log("BroadPhase Time = {0:F4}", profile.broadphase);
                CCLog.Log("Collision Time = {0:F4}", profile.collide);
                CCLog.Log("Solve Velocity Time = {0:F4}", profile.solveVelocity);
                CCLog.Log("Solve Position Time = {0:F4}", profile.solvePosition);
                CCLog.Log("Step Time = {0:F4}", profile.step);
#endif
            }
#endif
        }

        public override void TouchesEnded(List<CCTouch> touches)
        {
            //Add a new body/atlas sprite at the touched location
            foreach (CCTouch touch in touches)
            {
                CCPoint location = touch.Location;

                addNewSpriteAtPosition(location);
            }
        }
    }

    internal class Box2DTestScene : TestScene
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
            CCLayer pLayer = new Box2DTestLayer();
            AddChild(pLayer);

            CCDirector.SharedDirector.ReplaceScene(this);
        }
    }
}