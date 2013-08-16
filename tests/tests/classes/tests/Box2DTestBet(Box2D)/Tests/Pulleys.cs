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
    public class Pulleys : Test
    {

        public Pulleys()
        {
            float y = 16.0f;
            float L = 12.0f;
            float a = 1.0f;
            float b = 2.0f;

            b2Body ground = null;
            {
                b2BodyDef bd  = new b2BodyDef();
                ground = m_world.CreateBody(bd);

                b2EdgeShape edge = new b2EdgeShape();
                edge.Set(new b2Vec2(-40.0f, 0.0f), new b2Vec2(40.0f, 0.0f));
                //ground->CreateFixture(&shape, 0.0f);

                b2CircleShape circle = new b2CircleShape();
                circle.Radius = 2.0f;

                circle.Position = new b2Vec2(-10.0f, y + b + L);
                ground.CreateFixture(circle, 0.0f);

                circle.Position = new b2Vec2(10.0f, y + b + L);
                ground.CreateFixture(circle, 0.0f);
            }

            {

                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(a, b);

                b2BodyDef bd  = new b2BodyDef();
                bd.type = b2BodyType.b2_dynamicBody;

                //bd.fixedRotation = true;
                bd.position.Set(-10.0f, y);
                b2Body body1 = m_world.CreateBody(bd);
                body1.CreateFixture(shape, 5.0f);

                bd.position.Set(10.0f, y);
                b2Body body2 = m_world.CreateBody(bd);
                body2.CreateFixture(shape, 5.0f);

                b2PulleyJointDef pulleyDef = new b2PulleyJointDef();
                b2Vec2 anchor1 = new b2Vec2(-10.0f, y + b);
                b2Vec2 anchor2 = new b2Vec2(10.0f, y + b);
                b2Vec2 groundAnchor1 = new b2Vec2(-10.0f, y + b + L);
                b2Vec2 groundAnchor2 = new b2Vec2(10.0f, y + b + L);
                pulleyDef.Initialize(body1, body2, groundAnchor1, groundAnchor2, anchor1, anchor2, 1.5f);

                m_joint1 = (b2PulleyJoint) m_world.CreateJoint(pulleyDef);
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

        protected override void Draw(Settings settings)
        {
            base.Draw(settings);

            float ratio = m_joint1.GetRatio();
            float L = m_joint1.GetLengthA() + ratio * m_joint1.GetLengthB();
            m_debugDraw.DrawString(5, m_textLine, "L1 + {0:0000.00} * L2 = {1:0000.00}", ratio, L);
            m_textLine += 15;
        }

        public b2PulleyJoint m_joint1;
    }
}
