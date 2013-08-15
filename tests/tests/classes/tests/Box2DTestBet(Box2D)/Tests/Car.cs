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
    public class Car : Test
    {
        public Car()
        {
            m_hz = 4.0f;
            m_zeta = 0.7f;
            m_speed = 50.0f;

            b2Body ground = null;
            {
                b2BodyDef bd  = b2BodyDef.Create();
                ground = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();

                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = shape;
                fd.density = 0.0f;
                fd.friction = 0.6f;

                shape.Set(new b2Vec2(-20.0f, 0.0f), new b2Vec2(20.0f, 0.0f));
                ground.CreateFixture(fd);

                float[] hs = {0.25f, 1.0f, 4.0f, 0.0f, 0.0f, -1.0f, -2.0f, -2.0f, -1.25f, 0.0f};

                float x = 20.0f, y1 = 0.0f, dx = 5.0f;

                for (int i = 0; i < 10; ++i)
                {
                    float y2 = hs[i];
                    shape.Set(new b2Vec2(x, y1), new b2Vec2(x + dx, y2));
                    ground.CreateFixture(fd);
                    y1 = y2;
                    x += dx;
                }

                for (int i = 0; i < 10; ++i)
                {
                    float y2 = hs[i];
                    shape.Set(new b2Vec2(x, y1), new b2Vec2(x + dx, y2));
                    ground.CreateFixture(fd);
                    y1 = y2;
                    x += dx;
                }

                shape.Set(new b2Vec2(x, 0.0f), new b2Vec2(x + 40.0f, 0.0f));
                ground.CreateFixture(fd);

                x += 80.0f;
                shape.Set(new b2Vec2(x, 0.0f), new b2Vec2(x + 40.0f, 0.0f));
                ground.CreateFixture(fd);

                x += 40.0f;
                shape.Set(new b2Vec2(x, 0.0f), new b2Vec2(x + 10.0f, 5.0f));
                ground.CreateFixture(fd);

                x += 20.0f;
                shape.Set(new b2Vec2(x, 0.0f), new b2Vec2(x + 40.0f, 0.0f));
                ground.CreateFixture(fd);

                x += 40.0f;
                shape.Set(new b2Vec2(x, 0.0f), new b2Vec2(x, 20.0f));
                ground.CreateFixture(fd);
            }

            // Teeter
            {
                b2BodyDef bd  = b2BodyDef.Create();
                bd.position.Set(140.0f, 1.0f);
                bd.type = b2BodyType.b2_dynamicBody;
                b2Body body = m_world.CreateBody(bd);

                b2PolygonShape box = new b2PolygonShape();
                box.SetAsBox(10.0f, 0.25f);
                body.CreateFixture(box, 1.0f);

                b2RevoluteJointDef jd = new b2RevoluteJointDef();
                jd.Initialize(ground, body, body.Position);
                jd.lowerAngle = -8.0f * b2Settings.b2_pi / 180.0f;
                jd.upperAngle = 8.0f * b2Settings.b2_pi / 180.0f;
                jd.enableLimit = true;
                m_world.CreateJoint(jd);

                body.ApplyAngularImpulse(100.0f);
            }

            // Bridge
            {
                int N = 20;
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(1.0f, 0.125f);

                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = shape;
                fd.density = 1.0f;
                fd.friction = 0.6f;

                b2RevoluteJointDef jd = new b2RevoluteJointDef();

                b2Body prevBody = ground;
                for (int i = 0; i < N; ++i)
                {
                    b2BodyDef bd  = b2BodyDef.Create();
                    bd.type = b2BodyType.b2_dynamicBody;
                    bd.position.Set(161.0f + 2.0f * i, -0.125f);
                    b2Body body = m_world.CreateBody(bd);
                    body.CreateFixture(fd);

                    b2Vec2 anchor = new b2Vec2(160.0f + 2.0f * i, -0.125f);
                    jd.Initialize(prevBody, body, anchor);
                    m_world.CreateJoint(jd);

                    prevBody = body;
                }

                b2Vec2 anchor1 = new b2Vec2(160.0f + 2.0f * N, -0.125f);
                jd.Initialize(prevBody, ground, anchor1);
                m_world.CreateJoint(jd);
            }

            // Boxes
            {
                b2PolygonShape box = new b2PolygonShape();
                box.SetAsBox(0.5f, 0.5f);

                b2Body body = null;
                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;

                bd.position.Set(230.0f, 0.5f);
                body = m_world.CreateBody(bd);
                body.CreateFixture(box, 0.5f);

                bd.position.Set(230.0f, 1.5f);
                body = m_world.CreateBody(bd);
                body.CreateFixture(box, 0.5f);

                bd.position.Set(230.0f, 2.5f);
                body = m_world.CreateBody(bd);
                body.CreateFixture(box, 0.5f);

                bd.position.Set(230.0f, 3.5f);
                body = m_world.CreateBody(bd);
                body.CreateFixture(box, 0.5f);

                bd.position.Set(230.0f, 4.5f);
                body = m_world.CreateBody(bd);
                body.CreateFixture(box, 0.5f);
            }

            // Car
            {
                b2PolygonShape chassis = new b2PolygonShape();
                b2Vec2[] vertices = new b2Vec2[8];
                vertices[0].Set(-1.5f, -0.5f);
                vertices[1].Set(1.5f, -0.5f);
                vertices[2].Set(1.5f, 0.0f);
                vertices[3].Set(0.0f, 0.9f);
                vertices[4].Set(-1.15f, 0.9f);
                vertices[5].Set(-1.5f, 0.2f);
                chassis.Set(vertices, 6);

                b2CircleShape circle = new b2CircleShape();
                circle.Radius = 0.4f;

                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position.Set(0.0f, 1.0f);
                m_car = m_world.CreateBody(bd);
                m_car.CreateFixture(chassis, 1.0f);

                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = circle;
                fd.density = 1.0f;
                fd.friction = 0.9f;

                bd.position.Set(-1.0f, 0.35f);
                m_wheel1 = m_world.CreateBody(bd);
                m_wheel1.CreateFixture(fd);

                bd.position.Set(1.0f, 0.4f);
                m_wheel2 = m_world.CreateBody(bd);
                m_wheel2.CreateFixture(fd);

                b2WheelJointDef jd = new b2WheelJointDef();
                b2Vec2 axis = new b2Vec2(0.0f, 1.0f);

                jd.Initialize(m_car, m_wheel1, m_wheel1.Position, axis);
                jd.motorSpeed = 0.0f;
                jd.maxMotorTorque = 20.0f;
                jd.enableMotor = true;
                jd.frequencyHz = m_hz;
                jd.dampingRatio = m_zeta;
                m_spring1 = (b2WheelJoint) m_world.CreateJoint(jd);

                jd.Initialize(m_car, m_wheel2, m_wheel2.Position, axis);
                jd.motorSpeed = 0.0f;
                jd.maxMotorTorque = 10.0f;
                jd.enableMotor = false;
                jd.frequencyHz = m_hz;
                jd.dampingRatio = m_zeta;
                m_spring2 = (b2WheelJoint) m_world.CreateJoint(jd);
            }
        }

        public override void Keyboard(char key)
        {
            switch (key)
            {
                case 'a':
                    m_spring1.SetMotorSpeed(m_speed);
                    break;

                case 's':
                    m_spring1.SetMotorSpeed(0.0f);
                    break;

                case 'd':
                    m_spring1.SetMotorSpeed(-m_speed);
                    break;

                case 'q':
                    m_hz = Math.Max(0.0f, m_hz - 1.0f);
                    m_spring1.SetSpringFrequencyHz(m_hz);
                    m_spring2.SetSpringFrequencyHz(m_hz);
                    break;

                case 'e':
                    m_hz += 1.0f;
                    m_spring1.SetSpringFrequencyHz(m_hz);
                    m_spring2.SetSpringFrequencyHz(m_hz);
                    break;
            }
        }

        public override void Step(Settings settings)
        {
            m_debugDraw.DrawString(5, m_textLine, "Keys: left = a, brake = s, right = d, hz down = q, hz up = e");
            m_textLine += 15;
            m_debugDraw.DrawString(5, m_textLine, "frequency = %g hz, damping ratio = %g", m_hz, m_zeta);
            m_textLine += 15;

            settings.viewCenter.x = m_car.Position.x;
            base.Step(settings);
        }

        public b2Body m_car;
        public b2Body m_wheel1;
        public b2Body m_wheel2;

        public float m_hz;
        public float m_zeta;
        public float m_speed;
        public b2WheelJoint m_spring1;
        public b2WheelJoint m_spring2;
    }
}
