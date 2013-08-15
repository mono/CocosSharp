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
    public class BodyTypes : Test
    {
        public BodyTypes()
        {
            b2Body ground = null;
            {
                b2BodyDef bd  = b2BodyDef.Create();
                ground = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();
                shape.Set(new b2Vec2(-20.0f, 0.0f), new b2Vec2(20.0f, 0.0f));

                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = shape;

                ground.CreateFixture(fd);
            }

            // Define attachment
            {
                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;

                bd.position = new b2Vec2(0.0f, 3.0f);
                m_attachment = m_world.CreateBody(bd);

                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.5f, 2.0f);
                m_attachment.CreateFixture(shape, 2.0f);
            }

            // Define platform
            {
                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;
                
                bd.position = new b2Vec2(-4.0f, 5.0f);
                m_platform = m_world.CreateBody(bd);

                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.5f, 4.0f, new b2Vec2(4.0f, 0.0f), 0.5f * b2Settings.b2_pi);

                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = shape;
                fd.friction = 0.6f;
                fd.density = 2.0f;
                m_platform.CreateFixture(fd);

                b2RevoluteJointDef rjd = new b2RevoluteJointDef();
                rjd.Initialize(m_attachment, m_platform, new b2Vec2(0.0f, 5.0f));
                rjd.maxMotorTorque = 50.0f;
                rjd.enableMotor = true;
                m_world.CreateJoint(rjd);

                b2PrismaticJointDef pjd = new b2PrismaticJointDef();
                pjd.Initialize(ground, m_platform, new b2Vec2(0.0f, 5.0f), new b2Vec2(1.0f, 0.0f));

                pjd.maxMotorForce = 1000.0f;
                pjd.enableMotor = true;
                pjd.lowerTranslation = -10.0f;
                pjd.upperTranslation = 10.0f;
                pjd.enableLimit = true;

                m_world.CreateJoint(pjd);

                m_speed = 3.0f;
            }

            // Create a payload
            {
                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position = new b2Vec2(0.0f, 8.0f);
                b2Body body = m_world.CreateBody(bd);

                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.75f, 0.75f);

                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = shape;
                fd.friction = 0.6f;
                fd.density = 2.0f;

                body.CreateFixture(fd);
            }
        }

        public override void Keyboard(char key)
        {
            switch (key)
            {
                case 'd':
                    m_platform.BodyType = b2BodyType.b2_dynamicBody;
                    break;

                case 's':
                    m_platform.BodyType = b2BodyType.b2_staticBody;
                    break;

                case 'k':
                    m_platform.BodyType = b2BodyType.b2_kinematicBody;
                    m_platform.LinearVelocity = new b2Vec2(-m_speed, 0.0f);
                    m_platform.AngularVelocity = 0.0f;
                    break;
            }
        }

        public override void Step(Settings settings)
        {
            // Drive the kinematic body.
            if (m_platform.BodyType == b2BodyType.b2_kinematicBody)
            {
                b2Vec2 p = m_platform.Transform.p;
                b2Vec2 v = m_platform.LinearVelocity;

                if ((p.x < -10.0f && v.x < 0.0f) ||
                    (p.x > 10.0f && v.x > 0.0f))
                {
                    v.x = -v.x;
                    m_platform.LinearVelocity = v;
                }
            }

            base.Step(settings);
            m_debugDraw.DrawString(5, m_textLine, "Keys: (d) dynamic, (s) static, (k) kinematic");
            m_textLine += 15;
        }

        public b2Body m_attachment;
        public b2Body m_platform;
        public float m_speed;
    }
}
