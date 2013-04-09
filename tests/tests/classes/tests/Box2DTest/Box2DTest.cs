using System;
using System.Collections.Generic;
using Box2D;
using Box2D.Common;
using Box2D.Collision.Shapes;
using Box2D.Dynamics;
using Microsoft.Xna.Framework;
using cocos2d;
using Random = cocos2d.Random;

namespace tests
{
    internal class PhysicsSprite : CCSprite
    {
        private b2Body m_pBody; // strong ref

        public override bool Dirty
        {
            get { return true; }
            set { base.Dirty = value; }
        }

        public void setPhysicsBody(b2Body body)
        {
            m_pBody = body;
        }

        public override CCAffineTransform NodeToParentTransform()
        {
            b2Vec2 pos = m_pBody.Position;

            float x = pos.x * Box2DTestLayer.PTM_RATIO;
            float y = pos.y * Box2DTestLayer.PTM_RATIO;

            if (IgnoreAnchorPointForPosition)
            {
                x += m_tAnchorPointInPoints.X;
                y += m_tAnchorPointInPoints.Y;
            }

            // Make matrix
            float radians = m_pBody.Angle;
            var c = (float) Math.Cos(radians);
            var s = (float) Math.Sin(radians);

            if (! m_tAnchorPointInPoints.Equals(CCPoint.Zero))
            {
                x += c * -m_tAnchorPointInPoints.X + -s * -m_tAnchorPointInPoints.Y;
                y += s * -m_tAnchorPointInPoints.X + c * -m_tAnchorPointInPoints.Y;
            }

            // Rot, Translate Matrix
            m_tTransform = new CCAffineTransform(c, s,
                                                 -s, c,
                                                 x, y);

            return m_tTransform;
        }
    }

    public class Box2DTestLayer : CCLayer
    {
        public const int PTM_RATIO = 32;

        private const int kTagParentNode = 1;
        private readonly CCTexture2D m_pSpriteTexture; // weak ref
        private b2World world;
        private CCSpriteBatchNode batch;

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
            batch = CCSpriteBatchNode.Create("Images/blocks", 100);
            m_pSpriteTexture = batch.Texture;
            AddChild(batch, 0, kTagParentNode);

            addNewSpriteAtPosition(new CCPoint(s.Width / 2, s.Height / 2));

            CCLabelTTF label = CCLabelTTF.Create("Tap screen", "Marker Felt", 32);
            AddChild(label, 0);
            label.Color = new CCColor3B(0, 0, 255);
            label.Position = new CCPoint(s.Width / 2, s.Height - 50);

            ScheduleUpdate();
        }


        private void initPhysics()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            var gravity = new b2Vec2(0.0f, -10.0f);
            world = new b2World(gravity);
            float debugWidth = s.Width / PTM_RATIO * 2f;
            float debugHeight = s.Height / PTM_RATIO * 2f;
            CCDraw debugDraw = new CCDraw(new b2Vec2(debugWidth / 2f + 10, s.Height - debugHeight - 10), 2);
            debugDraw.AppendFlags(b2DrawFlags.e_shapeBit);
            world.SetDebugDraw(debugDraw);
            world.SetAllowSleeping(true);
            world.SetContinuousPhysics(true);

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
            b2BodyDef def = b2BodyDef.Default;
            def.allowSleep = true;
            def.position = b2Vec2.Zero;
            def.type = b2BodyType.b2_staticBody;
            b2Body groundBody = world.CreateBody(def);
            groundBody.SetActive(true);

            // Define the ground box shape.

            // bottom
            b2EdgeShape groundBox = new b2EdgeShape();
            groundBox.Set(b2Vec2.Zero, new b2Vec2(s.Width / PTM_RATIO, 0));
            b2FixtureDef fd = b2FixtureDef.Create();
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
            world.DrawDebugData();
            CCDrawingPrimitives.End();

            //world.DrawDebugData();

            //kmGLPopMatrix();
        }

        public void addNewSpriteAtPosition(CCPoint p)
        {
            CCLog.Log("Add sprite #{2} : {0} x {1}", p.X, p.Y, batch.ChildrenCount+1);

            //We have a 64x64 sprite sheet with 4 different 32x32 images.  The following code is
            //just randomly picking one of the images
            int idx = (Random.Float_0_1() > .5 ? 0 : 1);
            int idy = (Random.Float_0_1() > .5 ? 0 : 1);
            var sprite = new PhysicsSprite();
            sprite.InitWithTexture(m_pSpriteTexture, new CCRect(32 * idx, 32 * idy, 32, 32));

            batch.AddChild(sprite);

            sprite.Position = new CCPoint(p.X, p.Y);

            // Define the dynamic body.
            //Set up a 1m squared box in the physics world
            b2BodyDef def = b2BodyDef.Create();
            def.position = new b2Vec2(p.X / PTM_RATIO, p.Y / PTM_RATIO);
            def.type = b2BodyType.b2_dynamicBody;
            b2Body body = world.CreateBody(def);
            //body.SetActive(true);
            // Define another box shape for our dynamic body.
            var dynamicBox = new b2PolygonShape();
            dynamicBox.Radius = 5f;
            dynamicBox.SetAsBox(.5f, .5f); //These are mid points for our 1m box

            // Define the dynamic body fixture.
            b2FixtureDef fd = b2FixtureDef.Create();
            fd.shape = dynamicBox;
            fd.friction = 0.3f;
            fd.density = 1f;
            b2Fixture fixture = body.CreateFixture(fd);

            sprite.setPhysicsBody(body);
        }

        public override void Update(float dt)
        {
            world.Step(dt, 8, 1);

            /*
            b2Profile profile = world.Profile;
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
             */
        }

        public override void TouchesEnded(List<CCTouch> touches, CCEvent e)
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