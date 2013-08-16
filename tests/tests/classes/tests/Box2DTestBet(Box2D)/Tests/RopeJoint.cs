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
    public class RopeJoint : Test
    {
        public RopeJoint()
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
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.5f, 0.125f);

                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = shape;
                fd.density = 20.0f;
                fd.friction = 0.2f;
                fd.filter.categoryBits = 0x0001;
                fd.filter.maskBits = 0xFFFF & ~0x0002;

                b2RevoluteJointDef jd = new b2RevoluteJointDef();
                jd.CollideConnected = false;

                const int N = 10;
                const float y = 15.0f;
                m_ropeDef.localAnchorA.Set(0.0f, y);

                b2Body prevBody = ground;
                for (int i = 0; i < N; ++i)
                {
                    b2BodyDef bd  = new b2BodyDef();
                    bd.type = b2BodyType.b2_dynamicBody;
                    bd.position.Set(0.5f + 1.0f * i, y);
                    if (i == N - 1)
                    {
                        shape.SetAsBox(1.5f, 1.5f);
                        fd.density = 100.0f;
                        fd.filter.categoryBits = 0x0002;
                        bd.position.Set(1.0f * i, y);
                        bd.angularDamping = 0.4f;
                    }

                    b2Body body = m_world.CreateBody(bd);

                    body.CreateFixture(fd);

                    b2Vec2 anchor = new b2Vec2(i, y);
                    jd.Initialize(prevBody, body, anchor);
                    m_world.CreateJoint(jd);

                    prevBody = body;
                }

                m_ropeDef.localAnchorB.SetZero();

                float extraLength = 0.01f;
                m_ropeDef.maxLength = N - 1.0f + extraLength;
                m_ropeDef.BodyB = prevBody;
            }

            {
                m_ropeDef.BodyA = ground;
                m_rope = m_world.CreateJoint(m_ropeDef);
            }
        }

        public override void Keyboard(char key)
        {
            switch (key)
            {
                case 'j':
                    if (m_rope != null)
                    {
                        m_world.DestroyJoint(m_rope);
                        m_rope = null;
                    }
                    else
                    {
                        m_rope = m_world.CreateJoint(m_ropeDef);
                    }
                    break;
            }
        }

        protected override void Draw(Settings settings)
        {
            base.Draw(settings);
            m_debugDraw.DrawString(5, m_textLine, "Press (j) to toggle the rope joint.");
            m_textLine += 15;
            if (m_rope != null)
            {
                m_debugDraw.DrawString(5, m_textLine, "Rope ON");
            }
            else
            {
                m_debugDraw.DrawString(5, m_textLine, "Rope OFF");
            }
            m_textLine += 15;
        }

        public b2RopeJointDef m_ropeDef = new b2RopeJointDef();
        public b2Joint m_rope;
    }
}
