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
    public class SliderCrank : Test
    {
        public SliderCrank()
        {
            b2Body ground;
            {
                b2BodyDef bd  = new b2BodyDef();
                ground = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();
                shape.Set(new b2Vec2(-40.0f, 0.0f), new b2Vec2(40.0f, 0.0f));
                ground.CreateFixture(shape, 0.0f);
            }

            {
                b2Body prevBody = ground;

                // Define crank.
                {
                    b2PolygonShape shape = new b2PolygonShape();
                    shape.SetAsBox(0.5f, 2.0f);

                    b2BodyDef bd  = new b2BodyDef();
                    bd.type = b2BodyType.b2_dynamicBody;
                    bd.position.Set(0.0f, 7.0f);
                    b2Body body = m_world.CreateBody(bd);
                    body.CreateFixture(shape, 2.0f);

                    b2RevoluteJointDef rjd = new b2RevoluteJointDef();
                    rjd.Initialize(prevBody, body, new b2Vec2(0.0f, 5.0f));
                    rjd.motorSpeed = 1.0f * b2Settings.b2_pi;
                    rjd.maxMotorTorque = 10000.0f;
                    rjd.enableMotor = true;
                    m_joint1 = (b2RevoluteJoint) m_world.CreateJoint(rjd);

                    prevBody = body;
                }

                // Define follower.
                {
                    b2PolygonShape shape = new b2PolygonShape();
                    shape.SetAsBox(0.5f, 4.0f);

                    b2BodyDef bd  = new b2BodyDef();
                    bd.type = b2BodyType.b2_dynamicBody;
                    bd.position.Set(0.0f, 13.0f);
                    b2Body body = m_world.CreateBody(bd);
                    body.CreateFixture(shape, 2.0f);

                    b2RevoluteJointDef rjd = new b2RevoluteJointDef();
                    rjd.Initialize(prevBody, body, new b2Vec2(0.0f, 9.0f));
                    rjd.enableMotor = false;
                    m_world.CreateJoint(rjd);

                    prevBody = body;
                }

                // Define piston
                {
                    b2PolygonShape shape = new b2PolygonShape();
                    shape.SetAsBox(1.5f, 1.5f);

                    b2BodyDef bd  = new b2BodyDef();
                    bd.type = b2BodyType.b2_dynamicBody;
                    bd.fixedRotation = true;
                    bd.position.Set(0.0f, 17.0f);
                    b2Body body = m_world.CreateBody(bd);
                    body.CreateFixture(shape, 2.0f);

                    b2RevoluteJointDef rjd = new b2RevoluteJointDef();
                    rjd.Initialize(prevBody, body, new b2Vec2(0.0f, 17.0f));
                    m_world.CreateJoint(rjd);

                    b2PrismaticJointDef pjd = new b2PrismaticJointDef();
                    pjd.Initialize(ground, body, new b2Vec2(0.0f, 17.0f), new b2Vec2(0.0f, 1.0f));

                    pjd.maxMotorForce = 1000.0f;
                    pjd.enableMotor = true;

                    m_joint2 = (b2PrismaticJoint) m_world.CreateJoint(pjd);
                }

                // Create a payload
                {
                    b2PolygonShape shape = new b2PolygonShape();
                    shape.SetAsBox(1.5f, 1.5f);

                    b2BodyDef bd  = new b2BodyDef();
                    bd.type = b2BodyType.b2_dynamicBody;
                    bd.position.Set(0.0f, 23.0f);
                    b2Body body = m_world.CreateBody(bd);
                    body.CreateFixture(shape, 2.0f);
                }
            }
        }

        public override void Keyboard(char key)
        {
            switch (key)
            {
                case 'f':
                    m_joint2.EnableMotor(!m_joint2.IsMotorEnabled());
                    m_joint2.GetBodyB().SetAwake(true);
                    break;

                case 'm':
                    m_joint1.EnableMotor(!m_joint1.IsMotorEnabled());
                    m_joint1.GetBodyB().SetAwake(true);
                    break;
            }
        }

        protected override void Draw(Settings settings)
        {
            base.Draw(settings);
            m_debugDraw.DrawString(5, m_textLine, "Keys: (f) toggle friction, (m) toggle motor");
            m_textLine += 15;
            float torque = m_joint1.GetMotorTorque(settings.hz);
            m_debugDraw.DrawString(5, m_textLine, "Motor Torque = {0:00000}", torque);
            m_textLine += 15;
        }

        public b2RevoluteJoint m_joint1;
        public b2PrismaticJoint m_joint2;
    }
}
