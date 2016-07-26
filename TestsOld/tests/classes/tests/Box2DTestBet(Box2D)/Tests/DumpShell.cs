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
    public class DumpShell : Test
    {

        public DumpShell()
        {

            b2Vec2 g = new b2Vec2(0.000000000000000e+00f, 0.000000000000000e+00f);
            m_world.Gravity = g;
            b2Body[] bodies = new b2Body[3];
            b2Joint[] joints = new b2Joint[2];
            {
                b2BodyDef bd  = new b2BodyDef();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position.Set(1.304347801208496e+01f, 2.500000000000000e+00f);
                bd.angle = 0.000000000000000e+00f;
                bd.linearVelocity.Set(0.000000000000000e+00f, 0.000000000000000e+00f);
                bd.angularVelocity = 0.000000000000000e+00f;
                bd.linearDamping = 5.000000000000000e-01f;
                bd.angularDamping = 5.000000000000000e-01f;
                bd.allowSleep = true;
                bd.awake = true;
                bd.fixedRotation = false;
                bd.bullet = false;
                bd.active = true;
                bd.gravityScale = 1.000000000000000e+00f;
                bodies[0] = m_world.CreateBody(bd);

                {
                    b2FixtureDef fd = new b2FixtureDef();
                    fd.friction = 1.000000000000000e+00f;
                    fd.restitution = 5.000000000000000e-01f;
                    fd.density = 1.000000000000000e+01f;
                    fd.isSensor = false;
                    fd.filter.categoryBits = 1;
                    fd.filter.maskBits = 65535;
                    fd.filter.groupIndex = 0;
                    b2PolygonShape shape = new b2PolygonShape();
                    b2Vec2[] vs = new b2Vec2[8];
                    vs[0].Set(-6.900000095367432e+00f, -3.000000119209290e-01f);
                    vs[1].Set(2.000000029802322e-01f, -3.000000119209290e-01f);
                    vs[2].Set(2.000000029802322e-01f, 2.000000029802322e-01f);
                    vs[3].Set(-6.900000095367432e+00f, 2.000000029802322e-01f);
                    shape.Set(vs, 4);

                    fd.shape = shape;

                    bodies[0].CreateFixture(fd);
                }
            }
            {
                b2BodyDef bd  = new b2BodyDef();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position.Set(8.478260636329651e-01f, 2.500000000000000e+00f);
                bd.angle = 0.000000000000000e+00f;
                bd.linearVelocity.Set(0.000000000000000e+00f, 0.000000000000000e+00f);
                bd.angularVelocity = 0.000000000000000e+00f;
                bd.linearDamping = 5.000000000000000e-01f;
                bd.angularDamping = 5.000000000000000e-01f;
                bd.allowSleep = true;
                bd.awake = true;
                bd.fixedRotation = false;
                bd.bullet = false;
                bd.active = true;
                bd.gravityScale = 1.000000000000000e+00f;
                bodies[1] = m_world.CreateBody(bd);

                {
                    b2FixtureDef fd = new b2FixtureDef();
                    fd.friction = 1.000000000000000e+00f;
                    fd.restitution = 5.000000000000000e-01f;
                    fd.density = 1.000000000000000e+01f;
                    fd.isSensor = false;
                    fd.filter.categoryBits = 1;
                    fd.filter.maskBits = 65535;
                    fd.filter.groupIndex = 0;
                    b2PolygonShape shape = new b2PolygonShape();
                    b2Vec2[] vs = new b2Vec2[8];
                    vs[0].Set(-3.228000104427338e-01f, -2.957000136375427e-01f);
                    vs[1].Set(6.885900020599365e+00f, -3.641000092029572e-01f);
                    vs[2].Set(6.907599925994873e+00f, 3.271999955177307e-01f);
                    vs[3].Set(-3.228000104427338e-01f, 2.825999855995178e-01f);
                    shape.Set(vs, 4);

                    fd.shape = shape;

                    bodies[1].CreateFixture(fd);
                }
            }

            {
                b2BodyDef bd  = new b2BodyDef();
                bd.type = b2BodyType.b2_staticBody;
                bd.position.Set(0.000000000000000e+00f, 0.000000000000000e+00f);
                bd.angle = 0.000000000000000e+00f;
                bd.linearVelocity.Set(0.000000000000000e+00f, 0.000000000000000e+00f);
                bd.angularVelocity = 0.000000000000000e+00f;
                bd.linearDamping = 0.000000000000000e+00f;
                bd.angularDamping = 0.000000000000000e+00f;
                bd.allowSleep = true;
                bd.awake = true;
                bd.fixedRotation = false;
                bd.bullet = false;
                bd.active = true;
                bd.gravityScale = 1.000000000000000e+00f;
                bodies[2] = m_world.CreateBody(bd);

                {
                    b2FixtureDef fd = new b2FixtureDef();
                    fd.friction = 1.000000000000000e+01f;
                    fd.restitution = 0.000000000000000e+00f;
                    fd.density = 0.000000000000000e+00f;
                    fd.isSensor = false;
                    fd.filter.categoryBits = 1;
                    fd.filter.maskBits = 65535;
                    fd.filter.groupIndex = 0;
                    b2EdgeShape shape = new b2EdgeShape();
                    shape.Radius = 9.999999776482582e-03f;
                    shape.Vertex0 = new b2Vec2(0.000000000000000e+00f, 0.000000000000000e+00f);
                    shape.Vertex1 = new b2Vec2(4.452173995971680e+01f, 1.669565200805664e+01f);
                    shape.Vertex2 = new b2Vec2(4.452173995971680e+01f, 0.000000000000000e+00f);
                    shape.Vertex3 = new b2Vec2(0.000000000000000e+00f, 0.000000000000000e+00f);
                    shape.HasVertex0 = false;
                    shape.HasVertex3 = false;

                    fd.shape = shape;

                    bodies[2].CreateFixture(fd);
                }
                {
                    b2FixtureDef fd = new b2FixtureDef();
                    fd.friction = 1.000000000000000e+01f;
                    fd.restitution = 0.000000000000000e+00f;
                    fd.density = 0.000000000000000e+00f;
                    fd.isSensor = false;
                    fd.filter.categoryBits = 1;
                    fd.filter.maskBits = 65535;
                    fd.filter.groupIndex = 0;
                    b2EdgeShape shape = new b2EdgeShape();
                    shape.Radius = 9.999999776482582e-03f;
                    shape.Vertex0 = new b2Vec2(0.000000000000000e+00f, 0.000000000000000e+00f);
                    shape.Vertex1 = new b2Vec2(0.000000000000000e+00f, 1.669565200805664e+01f);
                    shape.Vertex2 = new b2Vec2(0.000000000000000e+00f, 0.000000000000000e+00f);
                    shape.Vertex3 = new b2Vec2(0.000000000000000e+00f, 0.000000000000000e+00f);
                    shape.HasVertex0 = false;
                    shape.HasVertex3 = false;

                    fd.shape = shape;

                    bodies[2].CreateFixture(fd);
                }
                {
                    b2FixtureDef fd = new b2FixtureDef();
                    fd.friction = 1.000000000000000e+01f;
                    fd.restitution = 0.000000000000000e+00f;
                    fd.density = 0.000000000000000e+00f;
                    fd.isSensor = false;
                    fd.filter.categoryBits = 1;
                    fd.filter.maskBits = 65535;
                    fd.filter.groupIndex = 0;
                    b2EdgeShape shape = new b2EdgeShape();
                    shape.Radius = 9.999999776482582e-03f;
                    shape.Vertex0 = new b2Vec2(0.000000000000000e+00f, 0.000000000000000e+00f);
                    shape.Vertex1 = new b2Vec2(0.000000000000000e+00f, 1.669565200805664e+01f);
                    shape.Vertex2 = new b2Vec2(4.452173995971680e+01f, 1.669565200805664e+01f);
                    shape.Vertex3 = new b2Vec2(0.000000000000000e+00f, 0.000000000000000e+00f);
                    shape.HasVertex0 = false;
                    shape.HasVertex3 = false;

                    fd.shape = shape;

                    bodies[2].CreateFixture(fd);
                }
                {
                    b2FixtureDef fd = new b2FixtureDef();
                    fd.friction = 1.000000000000000e+01f;
                    fd.restitution = 0.000000000000000e+00f;
                    fd.density = 0.000000000000000e+00f;
                    fd.isSensor = false;
                    fd.filter.categoryBits = 1;
                    fd.filter.maskBits = 65535;
                    fd.filter.groupIndex = 0;
                    b2EdgeShape shape = new b2EdgeShape();
                    shape.Radius = 9.999999776482582e-03f;
                    shape.Vertex0 = new b2Vec2(0.000000000000000e+00f, 0.000000000000000e+00f);
                    shape.Vertex1 = new b2Vec2(0.000000000000000e+00f, 0.000000000000000e+00f);
                    shape.Vertex2 = new b2Vec2(4.452173995971680e+01f, 0.000000000000000e+00f);
                    shape.Vertex3 = new b2Vec2(0.000000000000000e+00f, 0.000000000000000e+00f);
                    shape.HasVertex0 = false;
                    shape.HasVertex3 = false;

                    fd.shape = shape;

                    bodies[2].CreateFixture(fd);
                }
            }

            {
                b2PrismaticJointDef jd = new b2PrismaticJointDef();
                jd.BodyA = bodies[1];
                jd.BodyB = bodies[0];
                jd.CollideConnected = false;
                jd.localAnchorA.Set(0.000000000000000e+00f, 0.000000000000000e+00f);
                jd.localAnchorB.Set(-1.219565200805664e+01f, 0.000000000000000e+00f);
                jd.localAxisA.Set(-1.219565200805664e+01f, 0.000000000000000e+00f);
                jd.referenceAngle = 0.000000000000000e+00f;
                jd.enableLimit = true;
                jd.lowerTranslation = -2.000000000000000e+01f;
                jd.upperTranslation = 0.000000000000000e+00f;
                jd.enableMotor = true;
                jd.motorSpeed = 0.000000000000000e+00f;
                jd.maxMotorForce = 1.000000000000000e+01f;
                joints[0] = m_world.CreateJoint(jd);
            }
            {
                b2RevoluteJointDef jd = new b2RevoluteJointDef();
                jd.BodyA = bodies[1];
                jd.BodyB = bodies[2];
                jd.CollideConnected = false;
                jd.localAnchorA.Set(0.000000000000000e+00f, 0.000000000000000e+00f);
                jd.localAnchorB.Set(8.478260636329651e-01f, 2.500000000000000e+00f);
                jd.referenceAngle = 0.000000000000000e+00f;
                jd.enableLimit = false;
                jd.lowerAngle = 0.000000000000000e+00f;
                jd.upperAngle = 0.000000000000000e+00f;
                jd.enableMotor = false;
                jd.motorSpeed = 0.000000000000000e+00f;
                jd.maxMotorTorque = 0.000000000000000e+00f;
                joints[1] = m_world.CreateJoint(jd);
            }

        }
    }
}
