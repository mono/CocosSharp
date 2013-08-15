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
    public class Revolute : Test
    {
        public Revolute()
        {
            b2Body ground;
            {
                b2BodyDef bd  = b2BodyDef.Create();
                ground = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();
                shape.Set(new b2Vec2(-40.0f, 0.0f), new b2Vec2(40.0f, 0.0f));

                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = shape;
                //fd.filter.categoryBits = 2;

                ground.CreateFixture(fd);
            }

            {
                b2CircleShape shape = new b2CircleShape();
                shape.Radius = 0.5f;

                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;

                b2RevoluteJointDef rjd = new b2RevoluteJointDef();

                bd.position.Set(-10.0f, 20.0f);
                b2Body body = m_world.CreateBody(bd);
                body.CreateFixture(shape, 5.0f);

                float w = 100.0f;
                body.AngularVelocity = w;
                body.LinearVelocity = new b2Vec2(-8.0f * w, 0.0f);

                rjd.Initialize(ground, body, new b2Vec2(-10.0f, 12.0f));
                rjd.motorSpeed = 1.0f * b2Settings.b2_pi;
                rjd.maxMotorTorque = 10000.0f;
                rjd.enableMotor = false;
                rjd.lowerAngle = -0.25f * b2Settings.b2_pi;
                rjd.upperAngle = 0.5f * b2Settings.b2_pi;
                rjd.enableLimit = true;
                rjd.CollideConnected = true;

                m_joint = (b2RevoluteJoint) m_world.CreateJoint(rjd);
            }

            {
                b2CircleShape circle_shape = new b2CircleShape();
                circle_shape.Radius = 3.0f;

                b2BodyDef circle_bd  = b2BodyDef.Create();
                circle_bd.type = b2BodyType.b2_dynamicBody;
                circle_bd.position.Set(5.0f, 30.0f);

                b2FixtureDef fd = new b2FixtureDef();
                fd.density = 5.0f;
                fd.filter.maskBits = 1;
                fd.shape = circle_shape;

                m_ball = m_world.CreateBody(circle_bd);
                m_ball.CreateFixture(fd);

                b2PolygonShape polygon_shape = new b2PolygonShape();
                polygon_shape.SetAsBox(10.0f, 0.2f, new b2Vec2(-10.0f, 0.0f), 0.0f);

                b2BodyDef polygon_bd  = b2BodyDef.Create();
                polygon_bd.position.Set(20.0f, 10.0f);
                polygon_bd.type = b2BodyType.b2_dynamicBody;
                polygon_bd.bullet = true;
                b2Body polygon_body = m_world.CreateBody(polygon_bd);
                polygon_body.CreateFixture(polygon_shape, 2.0f);

                b2RevoluteJointDef rjd = new b2RevoluteJointDef();
                rjd.Initialize(ground, polygon_body, new b2Vec2(20.0f, 10.0f));
                rjd.lowerAngle = -0.25f * b2Settings.b2_pi;
                rjd.upperAngle = 0.0f * b2Settings.b2_pi;
                rjd.enableLimit = true;
                m_world.CreateJoint(rjd);
            }

            // Tests mass computation of a small object far from the origin
            {
                b2BodyDef bodyDef  = b2BodyDef.Create();
                bodyDef.type = b2BodyType.b2_dynamicBody;
                b2Body body = m_world.CreateBody(bodyDef);

                b2PolygonShape polyShape = new b2PolygonShape();
                b2Vec2[] verts = new b2Vec2[3];
                verts[0].Set(17.63f, 36.31f);
                verts[1].Set(17.52f, 36.69f);
                verts[2].Set(17.19f, 36.36f);
                polyShape.Set(verts, 3);

                b2FixtureDef polyFixtureDef = new b2FixtureDef();
                polyFixtureDef.shape = polyShape;
                polyFixtureDef.density = 1;

                body.CreateFixture(polyFixtureDef); //assertion hits inside here
            }

        }

        public override void Keyboard(char key)
        {
            switch (key)
            {
                case 'l':
                    m_joint.EnableLimit(!m_joint.IsLimitEnabled());
                    break;

                case 'm':
                    m_joint.EnableMotor(!m_joint.IsMotorEnabled());
                    break;
            }
        }

        public override void Step(Settings settings)
        {
            base.Step(settings);
            m_debugDraw.DrawString(5, m_textLine, "Keys: (l) limits, (m) motor");
            m_textLine += 15;

            //if (m_stepCount == 360)
            //{
            //    m_ball->SetTransform(b2Vec2(0.0f, 0.5f), 0.0f);
            //}

            //float32 torque1 = m_joint1->GetMotorTorque();
            //m_debugDraw.DrawString(5, m_textLine, "Motor Torque = %4.0f, %4.0f : Motor Force = %4.0f", (float) torque1, (float) torque2, (float) force3);
            //m_textLine += 15;
        }

        public b2Body m_ball;
        public b2RevoluteJoint m_joint;
    }
}
