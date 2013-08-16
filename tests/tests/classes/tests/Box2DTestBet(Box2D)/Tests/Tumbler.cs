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
    public class Tumbler : Test
    {
        public const int e_count = 800;

        public Tumbler()
        {
            b2Body ground;
            {
                b2BodyDef bd  = new b2BodyDef();
                ground = m_world.CreateBody(bd);
            }

            {
                b2BodyDef bd  = new b2BodyDef();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.allowSleep = false;
                bd.position.Set(0.0f, 10.0f);
                b2Body body = m_world.CreateBody(bd);

                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.5f, 10.0f, new b2Vec2(10.0f, 0.0f), 0.0f);
                body.CreateFixture(shape, 5.0f);
                shape.SetAsBox(0.5f, 10.0f, new b2Vec2(-10.0f, 0.0f), 0.0f);
                body.CreateFixture(shape, 5.0f);
                shape.SetAsBox(10.0f, 0.5f, new b2Vec2(0.0f, 10.0f), 0.0f);
                body.CreateFixture(shape, 5.0f);
                shape.SetAsBox(10.0f, 0.5f, new b2Vec2(0.0f, -10.0f), 0.0f);
                body.CreateFixture(shape, 5.0f);

                b2RevoluteJointDef jd = new b2RevoluteJointDef();
                jd.BodyA = ground;
                jd.BodyB = body;
                jd.localAnchorA.Set(0.0f, 10.0f);
                jd.localAnchorB.Set(0.0f, 0.0f);
                jd.referenceAngle = 0.0f;
                jd.motorSpeed = 0.05f * b2Settings.b2_pi;
                jd.maxMotorTorque = 1e8f;
                jd.enableMotor = true;
                m_joint = (b2RevoluteJoint) m_world.CreateJoint(jd);
            }

            m_count = 0;
        }

        public override void Step(Settings settings)
        {
            base.Step(settings);

            if (m_count < e_count)
            {
                b2BodyDef bd  = new b2BodyDef();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position.Set(0.0f, 10.0f);
                b2Body body = m_world.CreateBody(bd);

                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.125f, 0.125f);
                body.CreateFixture(shape, 1.0f);

                ++m_count;
            }
        }

        public b2RevoluteJoint m_joint;
        public int m_count;
    }
}
