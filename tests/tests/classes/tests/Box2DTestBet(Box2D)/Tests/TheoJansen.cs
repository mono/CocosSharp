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
    public class TheoJansen : Test
    {
        public void CreateLeg(float s, b2Vec2 wheelAnchor)
        {
            b2Vec2 p1 = new b2Vec2(5.4f * s, -6.1f);
            b2Vec2 p2 = new b2Vec2(7.2f * s, -1.2f);
            b2Vec2 p3 = new b2Vec2(4.3f * s, -1.9f);
            b2Vec2 p4 = new b2Vec2(3.1f * s, 0.8f);
            b2Vec2 p5 = new b2Vec2(6.0f * s, 1.5f);
            b2Vec2 p6 = new b2Vec2(2.5f * s, 3.7f);

            b2FixtureDef fd1 = new b2FixtureDef();
            b2FixtureDef fd2 = new b2FixtureDef();
            fd1.filter.groupIndex = -1;
            fd2.filter.groupIndex = -1;
            fd1.density = 1.0f;
            fd2.density = 1.0f;

            b2PolygonShape poly1 = new b2PolygonShape();
            b2PolygonShape poly2 = new b2PolygonShape();

            if (s > 0.0f)
            {
                b2Vec2[] vertices = new b2Vec2[3];

                vertices[0] = p1;
                vertices[1] = p2;
                vertices[2] = p3;
                poly1.Set(vertices, 3);

                vertices[0] = b2Vec2.Zero;
                vertices[1] = p5 - p4;
                vertices[2] = p6 - p4;
                poly2.Set(vertices, 3);
            }
            else
            {
                b2Vec2[] vertices = new b2Vec2[3];

                vertices[0] = p1;
                vertices[1] = p3;
                vertices[2] = p2;
                poly1.Set(vertices, 3);

                vertices[0] = b2Vec2.Zero;
                vertices[1] = p6 - p4;
                vertices[2] = p5 - p4;
                poly2.Set(vertices, 3);
            }

            fd1.shape = poly1;
            fd2.shape = poly2;

            b2BodyDef bd1  = b2BodyDef.Create();
            b2BodyDef bd2  = b2BodyDef.Create();
            bd1.type = b2BodyType.b2_dynamicBody;
            bd2.type = b2BodyType.b2_dynamicBody;
            bd1.position = m_offset;
            bd2.position = p4 + m_offset;

            bd1.angularDamping = 10.0f;
            bd2.angularDamping = 10.0f;

            b2Body body1 = m_world.CreateBody(bd1);
            b2Body body2 = m_world.CreateBody(bd2);

            body1.CreateFixture(fd1);
            body2.CreateFixture(fd2);

            b2DistanceJointDef djd = new b2DistanceJointDef();

            // Using a soft distance constraint can reduce some jitter.
            // It also makes the structure seem a bit more fluid by
            // acting like a suspension system.
            djd.dampingRatio = 0.5f;
            djd.frequencyHz = 10.0f;

            djd.Initialize(body1, body2, p2 + m_offset, p5 + m_offset);
            m_world.CreateJoint(djd);

            djd.Initialize(body1, body2, p3 + m_offset, p4 + m_offset);
            m_world.CreateJoint(djd);

            djd.Initialize(body1, m_wheel, p3 + m_offset, wheelAnchor + m_offset);
            m_world.CreateJoint(djd);

            djd.Initialize(body2, m_wheel, p6 + m_offset, wheelAnchor + m_offset);
            m_world.CreateJoint(djd);

            b2RevoluteJointDef rjd = new b2RevoluteJointDef();

            rjd.Initialize(body2, m_chassis, p4 + m_offset);
            m_world.CreateJoint(rjd);
        }

        public TheoJansen()
        {
            m_offset.Set(0.0f, 8.0f);
            m_motorSpeed = 2.0f;
            m_motorOn = true;
            b2Vec2 pivot = new b2Vec2(0.0f, 0.8f);

            // Ground
            {
                b2BodyDef bd  = b2BodyDef.Create();
                b2Body ground = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();
                shape.Set(new b2Vec2(-50.0f, 0.0f), new b2Vec2(50.0f, 0.0f));
                ground.CreateFixture(shape, 0.0f);

                shape.Set(new b2Vec2(-50.0f, 0.0f), new b2Vec2(-50.0f, 10.0f));
                ground.CreateFixture(shape, 0.0f);

                shape.Set(new b2Vec2(50.0f, 0.0f), new b2Vec2(50.0f, 10.0f));
                ground.CreateFixture(shape, 0.0f);
            }

            // Balls
            for (int i = 0; i < 40; ++i)
            {
                b2CircleShape shape = new b2CircleShape();
                shape.Radius = 0.25f;

                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position.Set(-40.0f + 2.0f * i, 0.5f);

                b2Body body = m_world.CreateBody(bd);
                body.CreateFixture(shape, 1.0f);
            }

            // Chassis
            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(2.5f, 1.0f);

                b2FixtureDef sd = new b2FixtureDef();
                sd.density = 1.0f;
                sd.shape = shape;
                sd.filter.groupIndex = -1;
                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position = pivot + m_offset;
                m_chassis = m_world.CreateBody(bd);
                m_chassis.CreateFixture(sd);
            }

            {
                b2CircleShape shape = new b2CircleShape();
                shape.Radius = 1.6f;

                b2FixtureDef sd = new b2FixtureDef();
                sd.density = 1.0f;
                sd.shape = shape;
                sd.filter.groupIndex = -1;
                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position = pivot + m_offset;
                m_wheel = m_world.CreateBody(bd);
                m_wheel.CreateFixture(sd);
            }

            {
                b2RevoluteJointDef jd = new b2RevoluteJointDef();
                jd.Initialize(m_wheel, m_chassis, pivot + m_offset);
                jd.CollideConnected = false;
                jd.motorSpeed = m_motorSpeed;
                jd.maxMotorTorque = 400.0f;
                jd.enableMotor = m_motorOn;
                m_motorJoint = (b2RevoluteJoint) m_world.CreateJoint(jd);
            }

            b2Vec2 wheelAnchor = new b2Vec2();

            wheelAnchor = pivot + new b2Vec2(0.0f, -0.8f);

            CreateLeg(-1.0f, wheelAnchor);
            CreateLeg(1.0f, wheelAnchor);

            m_wheel.SetTransform(m_wheel.Position, 120.0f * b2Settings.b2_pi / 180.0f);
            CreateLeg(-1.0f, wheelAnchor);
            CreateLeg(1.0f, wheelAnchor);

            m_wheel.SetTransform(m_wheel.Position, -120.0f * b2Settings.b2_pi / 180.0f);
            CreateLeg(-1.0f, wheelAnchor);
            CreateLeg(1.0f, wheelAnchor);
        }

        public override void Step(Settings settings)
        {
            m_debugDraw.DrawString(5, m_textLine, "Keys: left = a, brake = s, right = d, toggle motor = m");
            m_textLine += 15;

            base.Step(settings);
        }

        public override void Keyboard(char key)
        {
            switch (key)
            {
                case 'a':
                    m_motorJoint.SetMotorSpeed(-m_motorSpeed);
                    break;

                case 's':
                    m_motorJoint.SetMotorSpeed(0.0f);
                    break;

                case 'd':
                    m_motorJoint.SetMotorSpeed(m_motorSpeed);
                    break;

                case 'm':
                    m_motorJoint.EnableMotor(!m_motorJoint.IsMotorEnabled());
                    break;
            }
        }

        public b2Vec2 m_offset = new b2Vec2();
        public b2Body m_chassis;
        public b2Body m_wheel;
        public b2RevoluteJoint m_motorJoint;
        public bool m_motorOn;
        public float m_motorSpeed;
    }
}
