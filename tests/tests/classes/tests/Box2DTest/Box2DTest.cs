using System;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using cocos2d;
using Random = cocos2d.Random;

namespace tests
{
    internal class PhysicsSprite : CCSprite
    {
        private Body m_pBody; // strong ref

        public override bool Dirty
        {
            get { return true; }
            set { base.Dirty = value; }
        }

        public void setPhysicsBody(Body body)
        {
            m_pBody = body;
        }

        public override CCAffineTransform NodeToParentTransform()
        {
            Vector2 pos = m_pBody.Position;

            float x = pos.X * Box2DTestLayer.PTM_RATIO;
            float y = pos.Y * Box2DTestLayer.PTM_RATIO;

            if (IgnoreAnchorPointForPosition)
            {
                x += m_tAnchorPointInPoints.x;
                y += m_tAnchorPointInPoints.y;
            }

            // Make matrix
            float radians = m_pBody.Rotation;
            var c = (float) Math.Cos(radians);
            var s = (float) Math.Sin(radians);

            if (! m_tAnchorPointInPoints.Equals(CCPoint.Zero))
            {
                x += c * -m_tAnchorPointInPoints.x + -s * -m_tAnchorPointInPoints.y;
                y += s * -m_tAnchorPointInPoints.x + c * -m_tAnchorPointInPoints.y;
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
        private World world;

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
            CCSpriteBatchNode parent = CCSpriteBatchNode.Create("Images/blocks", 100);
            m_pSpriteTexture = parent.Texture;
            AddChild(parent, 0, kTagParentNode);

            addNewSpriteAtPosition(new CCPoint(s.Width / 2, s.Height / 2));

            CCLabelTTF label = CCLabelTTF.Create("Tap screen", "Marker Felt", 32);
            AddChild(label, 0);
            label.Color = new CCColor3B(0, 0, 255);
            label.Position = new CCPoint(s.Width / 2, s.Height - 50);

            ScheduleUpdate();
        }


        public void initPhysics()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            var gravity = new Vector2(0.0f, -10.0f);
            world = new World(gravity);

            // Do we want to let bodies sleep?
            Settings.AllowSleep = true;
            Settings.ContinuousPhysics = true;
            Settings.VelocityIterations = 8;
            Settings.PositionIterations = 1;

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
            Body groundBody = BodyFactory.CreateBody(world, new Vector2(0f, 0f));

            // Define the ground box shape.

            // bottom
            var groundBox = new EdgeShape(new Vector2(0f, 0f), new Vector2(s.Width / PTM_RATIO, 0));
            groundBody.CreateFixture(groundBox);

            // top
            groundBox.Set(new Vector2(0, s.Height / PTM_RATIO), new Vector2(s.Width / PTM_RATIO, s.Height / PTM_RATIO));
            groundBody.CreateFixture(groundBox);

            // left
            groundBox.Set(new Vector2(0, s.Height / PTM_RATIO), new Vector2(0, 0));
            groundBody.CreateFixture(groundBox);

            // right
            groundBox.Set(new Vector2(s.Width / PTM_RATIO, s.Height / PTM_RATIO), new Vector2(s.Width / PTM_RATIO, 0));
            groundBody.CreateFixture(groundBox);
        }

        public void createResetButton()
        {
            CCMenuItemImage res = CCMenuItemImage.Create("Images/r1", "Images/r2", reset);

            CCMenu menu = CCMenu.Create(res);

            CCSize s = CCDirector.SharedDirector.WinSize;

            menu.Position = new CCPoint(s.Width / 2, 30);
            AddChild(menu, -1);
        }

        public void reset(CCObject sender)
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

            //world.DrawDebugData();

            //kmGLPopMatrix();
        }

        public void addNewSpriteAtPosition(CCPoint p)
        {
            CCLog.Log("Add sprite {0} x {1}", p.x, p.y);
            CCNode parent = GetChildByTag(kTagParentNode);

            //We have a 64x64 sprite sheet with 4 different 32x32 images.  The following code is
            //just randomly picking one of the images
            int idx = (Random.Float_0_1() > .5 ? 0 : 1);
            int idy = (Random.Float_0_1() > .5 ? 0 : 1);
            var sprite = new PhysicsSprite();
            sprite.InitWithTexture(m_pSpriteTexture, new CCRect(32 * idx, 32 * idy, 32, 32));

            parent.AddChild(sprite);

            sprite.Position = new CCPoint(p.x, p.y);

            // Define the dynamic body.
            //Set up a 1m squared box in the physics world
            Body body = BodyFactory.CreateBody(world, new Vector2(p.x / PTM_RATIO, p.y / PTM_RATIO));
            body.BodyType = BodyType.Dynamic;

            // Define another box shape for our dynamic body.
            var dynamicBox = new PolygonShape(5.0f);
            dynamicBox.SetAsBox(.5f, .5f); //These are mid points for our 1m box

            // Define the dynamic body fixture.
            Fixture fixture = body.CreateFixture(dynamicBox);
            fixture.Friction = 0.3f;

            sprite.setPhysicsBody(body);
        }

        public override void Update(float dt)
        {
            world.Step(dt);
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