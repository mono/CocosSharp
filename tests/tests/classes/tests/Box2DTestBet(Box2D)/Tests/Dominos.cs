using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using Box2D.Dynamics.Joints;

namespace Box2D.TestBed.Tests
{
    public class Dominos : Test
    {

        public Dominos()
        {
            b2Body b1;
            {
                b2EdgeShape shape = new b2EdgeShape();
                shape.Set(new b2Vec2(-40.0f, 0.0f), new b2Vec2(40.0f, 0.0f));

                b2BodyDef bd  = b2BodyDef.Create();
                b1 = m_world.CreateBody(bd);
                b1.CreateFixture(shape, 0.0f);
            }

            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(6.0f, 0.25f);

                b2BodyDef bd  = b2BodyDef.Create();
                bd.position.Set(-1.5f, 10.0f);
                b2Body ground = m_world.CreateBody(bd);
                ground.CreateFixture(shape, 0.0f);
            }

            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.1f, 1.0f);

                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = shape;
                fd.density = 20.0f;
                fd.friction = 0.1f;

                for (int i = 0; i < 10; ++i)
                {
                    b2BodyDef bd  = b2BodyDef.Create();
                    bd.type = b2BodyType.b2_dynamicBody;
                    bd.position.Set(-6.0f + 1.0f * i, 11.25f);
                    b2Body body = m_world.CreateBody(bd);
                    body.CreateFixture(fd);
                }
            }

            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(7.0f, 0.25f, b2Vec2.Zero, 0.3f);

                b2BodyDef bd  = b2BodyDef.Create();
                bd.position.Set(1.0f, 6.0f);
                b2Body ground = m_world.CreateBody(bd);
                ground.CreateFixture(shape, 0.0f);
            }

            b2Body b2;
            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.25f, 1.5f);

                b2BodyDef bd  = b2BodyDef.Create();
                bd.position.Set(-7.0f, 4.0f);
                b2 = m_world.CreateBody(bd);
                b2.CreateFixture(shape, 0.0f);
            }

            b2Body b3;
            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(6.0f, 0.125f);

                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position.Set(-0.9f, 1.0f);
                bd.angle = -0.15f;

                b3 = m_world.CreateBody(bd);
                b3.CreateFixture(shape, 10.0f);
            }

            b2RevoluteJointDef jd = new b2RevoluteJointDef();
            b2Vec2 anchor = new b2Vec2();

            anchor.Set(-2.0f, 1.0f);
            jd.Initialize(b1, b3, anchor);
            jd.CollideConnected = true;
            m_world.CreateJoint(jd);

            b2Body b4;
            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.25f, 0.25f);

                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position.Set(-10.0f, 15.0f);
                b4 = m_world.CreateBody(bd);
                b4.CreateFixture(shape, 10.0f);
            }

            anchor.Set(-7.0f, 15.0f);
            jd.Initialize(b2, b4, anchor);
            m_world.CreateJoint(jd);

            b2Body b5;
            {
                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position.Set(6.5f, 3.0f);
                b5 = m_world.CreateBody(bd);

                b2PolygonShape shape = new b2PolygonShape();
                b2FixtureDef fd = new b2FixtureDef();

                fd.shape = shape;
                fd.density = 10.0f;
                fd.friction = 0.1f;

                shape.SetAsBox(1.0f, 0.1f, new b2Vec2(0.0f, -0.9f), 0.0f);
                b5.CreateFixture(fd);

                shape.SetAsBox(0.1f, 1.0f, new b2Vec2(-0.9f, 0.0f), 0.0f);
                b5.CreateFixture(fd);

                shape.SetAsBox(0.1f, 1.0f, new b2Vec2(0.9f, 0.0f), 0.0f);
                b5.CreateFixture(fd);
            }

            anchor.Set(6.0f, 2.0f);
            jd.Initialize(b1, b5, anchor);
            m_world.CreateJoint(jd);

            b2Body b6;
            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(1.0f, 0.1f);

                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position.Set(6.5f, 4.1f);
                b6 = m_world.CreateBody(bd);
                b6.CreateFixture(shape, 30.0f);
            }

            anchor.Set(7.5f, 4.0f);
            jd.Initialize(b5, b6, anchor);
            m_world.CreateJoint(jd);

            b2Body b7;
            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.1f, 1.0f);

                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position.Set(7.4f, 1.0f);

                b7 = m_world.CreateBody(bd);
                b7.CreateFixture(shape, 10.0f);
            }

            b2DistanceJointDef djd = new b2DistanceJointDef();
            djd.BodyA = b3;
            djd.BodyB = b7;
            djd.localAnchorA.Set(6.0f, 0.0f);
            djd.localAnchorB.Set(0.0f, -1.0f);
            b2Vec2 d = djd.BodyB.GetWorldPoint(djd.localAnchorB) - djd.BodyA.GetWorldPoint(djd.localAnchorA);
            djd.length = d.Length;
            m_world.CreateJoint(djd);

            {
                float radius = 0.2f;

                b2CircleShape shape = new b2CircleShape();
                shape.Radius = radius;

                for (int i = 0; i < 4; ++i)
                {
                    b2BodyDef bd  = b2BodyDef.Create();
                    bd.type = b2BodyType.b2_dynamicBody;
                    bd.position.Set(5.9f + 2.0f * radius * i, 2.4f);
                    b2Body body = m_world.CreateBody(bd);
                    body.CreateFixture(shape, 10.0f);
                }
            }
        }
    }
}
