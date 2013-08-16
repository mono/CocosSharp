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
    public class ApplyForce : Test
    {
        public ApplyForce()
        {
            m_world.Gravity = new b2Vec2(0.0f, 0.0f);

            float k_restitution = 0.4f;

            b2Body ground;
            {
                b2BodyDef bd  = new b2BodyDef();
                bd.position = new b2Vec2(0.0f, 20.0f);
                ground = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();

                b2FixtureDef sd = new b2FixtureDef();
                sd.shape = shape;
                sd.density = 0.0f;
                sd.restitution = k_restitution;

                // Left vertical
                shape.Set(new b2Vec2(-20.0f, -20.0f), new b2Vec2(-20.0f, 20.0f));
                ground.CreateFixture(sd);

                // Right vertical
                shape.Set(new b2Vec2(20.0f, -20.0f), new b2Vec2(20.0f, 20.0f));
                ground.CreateFixture(sd);

                // Top horizontal
                shape.Set(new b2Vec2(-20.0f, 20.0f), new b2Vec2(20.0f, 20.0f));
                ground.CreateFixture(sd);

                // Bottom horizontal
                shape.Set(new b2Vec2(-20.0f, -20.0f), new b2Vec2(20.0f, -20.0f));
                ground.CreateFixture(sd);
            }

            {
                b2Transform xf1;
                xf1.q = new b2Rot(0.3524f * b2Settings.b2_pi);
                xf1.p = xf1.q.GetXAxis();

                b2Vec2[] vertices = new b2Vec2[3];
                vertices[0] = b2Math.b2Mul(xf1, new b2Vec2(-1.0f, 0.0f));
                vertices[1] = b2Math.b2Mul(xf1, new b2Vec2(1.0f, 0.0f));
                vertices[2] = b2Math.b2Mul(xf1, new b2Vec2(0.0f, 0.5f));

                b2PolygonShape poly1 = new b2PolygonShape();
                poly1.Set(vertices, 3);

                b2FixtureDef sd1 = new b2FixtureDef();
                sd1.shape = poly1;
                sd1.density = 4.0f;

                b2Transform xf2 = new b2Transform();
                xf2.q.Set(-0.3524f * b2Settings.b2_pi);
                xf2.p = -xf2.q.GetXAxis();

                vertices[0] = b2Math.b2Mul(xf2, new b2Vec2(-1.0f, 0.0f));
                vertices[1] = b2Math.b2Mul(xf2, new b2Vec2(1.0f, 0.0f));
                vertices[2] = b2Math.b2Mul(xf2, new b2Vec2(0.0f, 0.5f));

                b2PolygonShape poly2 = new b2PolygonShape();
                poly2.Set(vertices, 3);

                b2FixtureDef sd2 = new b2FixtureDef();
                sd2.shape = poly2;
                sd2.density = 2.0f;

                b2BodyDef bd  = new b2BodyDef();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.angularDamping = 5.0f;
                bd.linearDamping = 0.1f;

                bd.position.Set(0.0f, 2.0f);
                bd.angle = b2Settings.b2_pi;
                bd.allowSleep = false;
                m_body = m_world.CreateBody(bd);
                m_body.CreateFixture(sd1);
                m_body.CreateFixture(sd2);
            }

            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.5f, 0.5f);

                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = shape;
                fd.density = 1.0f;
                fd.friction = 0.3f;

                for (int i = 0; i < 10; ++i)
                {
                    b2BodyDef bd  = new b2BodyDef();
                    bd.type = b2BodyType.b2_dynamicBody;

                    bd.position = new b2Vec2(0.0f, 5.0f + 1.54f * i);
                    b2Body body = m_world.CreateBody(bd);

                    body.CreateFixture(fd);

                    float gravity = 10.0f;
                    float I = body.Inertia;
                    float mass = body.Mass;

                    // For a circle: I = 0.5 * m * r * r ==> r = sqrt(2 * I / m)
                    float radius = b2Math.b2Sqrt(2.0f * I / mass);

                    b2FrictionJointDef jd = new b2FrictionJointDef();
                    jd.localAnchorA = b2Vec2.Zero;
                    jd.localAnchorB = b2Vec2.Zero;
                    jd.BodyA = ground;
                    jd.BodyB = body;
                    jd.CollideConnected = true;
                    jd.maxForce = mass * gravity;
                    jd.maxTorque = mass * radius * gravity;

                    m_world.CreateJoint(jd);
                }
            }
        }

        public override void Keyboard(char key)
        {
            switch (key)
            {
                case 'w':
                    {
                        b2Vec2 f = m_body.GetWorldVector(new b2Vec2(0.0f, -200.0f));
                        b2Vec2 p = m_body.GetWorldPoint(new b2Vec2(0.0f, 2.0f));
                        m_body.ApplyForce(f, p);
                    }
                    break;

                case 'a':
                    {
                        m_body.ApplyTorque(50.0f);
                    }
                    break;

                case 'd':
                    {
                        m_body.ApplyTorque(-50.0f);
                    }
                    break;
            }
        }

        public b2Body m_body;
    }


}
