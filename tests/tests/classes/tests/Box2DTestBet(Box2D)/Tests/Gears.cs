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
    public class Gears : Test
    {
        public Gears()
        {
            b2Body ground = null;
            {
                b2BodyDef bd  = b2BodyDef.Create();
                ground = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();
                shape.Set(new b2Vec2(50.0f, 0.0f), new b2Vec2(-50.0f, 0.0f));
                ground.CreateFixture(shape, 0.0f);
            }

            // Gears co
            {
                b2CircleShape circle1 = new b2CircleShape();
                circle1.Radius = 1.0f;

                b2PolygonShape box = new b2PolygonShape();
                box.SetAsBox(0.5f, 5.0f);

                b2CircleShape circle2 = new b2CircleShape();
                circle2.Radius = 2.0f;

                b2BodyDef bd1  = b2BodyDef.Create();
                bd1.type = b2BodyType.b2_staticBody;
                bd1.position.Set(10.0f, 9.0f);
                b2Body body1 = m_world.CreateBody(bd1);
                body1.CreateFixture(circle1, 0.0f);

                b2BodyDef bd2  = b2BodyDef.Create();
                bd2.type = b2BodyType.b2_dynamicBody;
                bd2.position.Set(10.0f, 8.0f);
                b2Body body2 = m_world.CreateBody(bd2);
                body2.CreateFixture(box, 5.0f);

                b2BodyDef bd3  = b2BodyDef.Create();
                bd3.type = b2BodyType.b2_dynamicBody;
                bd3.position.Set(10.0f, 6.0f);
                b2Body body3 = m_world.CreateBody(bd3);
                body3.CreateFixture(circle2, 5.0f);

                b2RevoluteJointDef jd1 = new b2RevoluteJointDef();
                jd1.Initialize(body2, body1, bd1.position);
                b2Joint joint1 = m_world.CreateJoint(jd1);

                b2RevoluteJointDef jd2 = new b2RevoluteJointDef();
                jd2.Initialize(body2, body3, bd3.position);
                b2Joint joint2 = m_world.CreateJoint(jd2);

                b2GearJointDef jd4 = new b2GearJointDef();
                jd4.BodyA = body1;
                jd4.BodyB = body3;
                jd4.joint1 = joint1;
                jd4.joint2 = joint2;
                jd4.ratio = circle2.Radius / circle1.Radius;
                m_world.CreateJoint(jd4);
            }

            {
                b2CircleShape circle1 = new b2CircleShape();
                circle1.Radius = 1.0f;

                b2CircleShape circle2 = new b2CircleShape();
                circle2.Radius = 2.0f;

                b2PolygonShape box = new b2PolygonShape();
                box.SetAsBox(0.5f, 5.0f);

                b2BodyDef bd1  = b2BodyDef.Create();
                bd1.type = b2BodyType.b2_dynamicBody;
                bd1.position.Set(-3.0f, 12.0f);
                b2Body body1 = m_world.CreateBody(bd1);
                body1.CreateFixture(circle1, 5.0f);

                b2RevoluteJointDef jd1 = new b2RevoluteJointDef();
                jd1.BodyA = ground;
                jd1.BodyB = body1;
                jd1.localAnchorA = ground.GetLocalPoint(bd1.position);
                jd1.localAnchorB = body1.GetLocalPoint(bd1.position);
                jd1.referenceAngle = body1.Angle - ground.Angle;
                m_joint1 = (b2RevoluteJoint) m_world.CreateJoint(jd1);

                b2BodyDef bd2  = b2BodyDef.Create();
                bd2.type = b2BodyType.b2_dynamicBody;
                bd2.position.Set(0.0f, 12.0f);
                b2Body body2 = m_world.CreateBody(bd2);
                body2.CreateFixture(circle2, 5.0f);

                b2RevoluteJointDef jd2 = new b2RevoluteJointDef();
                jd2.Initialize(ground, body2, bd2.position);
                m_joint2 = (b2RevoluteJoint) m_world.CreateJoint(jd2);

                b2BodyDef bd3  = b2BodyDef.Create();
                bd3.type = b2BodyType.b2_dynamicBody;
                bd3.position.Set(2.5f, 12.0f);
                b2Body body3 = m_world.CreateBody(bd3);
                body3.CreateFixture(box, 5.0f);

                b2PrismaticJointDef jd3 = new b2PrismaticJointDef();
                jd3.Initialize(ground, body3, bd3.position, new b2Vec2(0.0f, 1.0f));
                jd3.lowerTranslation = -5.0f;
                jd3.upperTranslation = 5.0f;
                jd3.enableLimit = true;

                m_joint3 = (b2PrismaticJoint) m_world.CreateJoint(jd3);

                b2GearJointDef jd4 = new b2GearJointDef();
                jd4.BodyA = body1;
                jd4.BodyB = body2;
                jd4.joint1 = m_joint1;
                jd4.joint2 = m_joint2;
                jd4.ratio = circle2.Radius / circle1.Radius;
                m_joint4 = (b2GearJoint) m_world.CreateJoint(jd4);

                b2GearJointDef jd5 = new b2GearJointDef();
                jd5.BodyA = body2;
                jd5.BodyB = body3;
                jd5.joint1 = m_joint2;
                jd5.joint2 = m_joint3;
                jd5.ratio = -1.0f / circle2.Radius;
                m_joint5 = (b2GearJoint) m_world.CreateJoint(jd5);
            }
        }

        public override void Keyboard(char key)
        {
            switch (key)
            {
                case '0':
                    break;
            }
        }

        private void Step(Settings settings)
        {
            base.Step(settings);

            float ratio, value;

            ratio = m_joint4.GetRatio();
            value = m_joint1.GetJointAngle() + ratio * m_joint2.GetJointAngle();
            m_debugDraw.DrawString(5, m_textLine, "theta1 + %4.2f * theta2 = %4.2f", ratio, value);
            m_textLine += 15;

            ratio = m_joint5.GetRatio();
            value = m_joint2.GetJointAngle() + ratio * m_joint3.GetJointTranslation();
            m_debugDraw.DrawString(5, m_textLine, "theta2 + %4.2f * delta = %4.2f", ratio, value);
            m_textLine += 15;
        }

        public b2RevoluteJoint m_joint1;
        public b2RevoluteJoint m_joint2;
        public b2PrismaticJoint m_joint3;
        public b2GearJoint m_joint4;
        public b2GearJoint m_joint5;
    }
}
