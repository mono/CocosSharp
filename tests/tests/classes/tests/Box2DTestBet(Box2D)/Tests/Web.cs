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
    public class Web : Test
    {
        public Web()
        {
            b2Body ground = null;
            {
                b2BodyDef bd  = b2BodyDef.Create();
                ground = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();
                shape.Set(new b2Vec2(-40.0f, 0.0f), new b2Vec2(40.0f, 0.0f));
                ground.CreateFixture(shape, 0.0f);
            }

            {
                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.5f, 0.5f);

                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;

                bd.position.Set(-5.0f, 5.0f);
                m_bodies[0] = m_world.CreateBody(bd);
                m_bodies[0].CreateFixture(shape, 5.0f);

                bd.position.Set(5.0f, 5.0f);
                m_bodies[1] = m_world.CreateBody(bd);
                m_bodies[1].CreateFixture(shape, 5.0f);

                bd.position.Set(5.0f, 15.0f);
                m_bodies[2] = m_world.CreateBody(bd);
                m_bodies[2].CreateFixture(shape, 5.0f);

                bd.position.Set(-5.0f, 15.0f);
                m_bodies[3] = m_world.CreateBody(bd);
                m_bodies[3].CreateFixture(shape, 5.0f);

                b2DistanceJointDef jd = new b2DistanceJointDef();
                b2Vec2 p1 = new b2Vec2();
                b2Vec2 p2 = new b2Vec2();
                b2Vec2 d = new b2Vec2();

                jd.frequencyHz = 2.0f;
                jd.dampingRatio = 0.0f;

                jd.BodyA = ground;
                jd.BodyB = m_bodies[0];
                jd.localAnchorA.Set(-10.0f, 0.0f);
                jd.localAnchorB.Set(-0.5f, -0.5f);
                p1 = jd.BodyA.GetWorldPoint(jd.localAnchorA);
                p2 = jd.BodyB.GetWorldPoint(jd.localAnchorB);
                d = p2 - p1;
                jd.length = d.Length;
                m_joints[0] = m_world.CreateJoint(jd);

                jd.BodyA = ground;
                jd.BodyB = m_bodies[1];
                jd.localAnchorA.Set(10.0f, 0.0f);
                jd.localAnchorB.Set(0.5f, -0.5f);
                p1 = jd.BodyA.GetWorldPoint(jd.localAnchorA);
                p2 = jd.BodyB.GetWorldPoint(jd.localAnchorB);
                d = p2 - p1;
                jd.length = d.Length;
                m_joints[1] = m_world.CreateJoint(jd);

                jd.BodyA = ground;
                jd.BodyB = m_bodies[2];
                jd.localAnchorA.Set(10.0f, 20.0f);
                jd.localAnchorB.Set(0.5f, 0.5f);
                p1 = jd.BodyA.GetWorldPoint(jd.localAnchorA);
                p2 = jd.BodyB.GetWorldPoint(jd.localAnchorB);
                d = p2 - p1;
                jd.length = d.Length;
                m_joints[2] = m_world.CreateJoint(jd);

                jd.BodyA = ground;
                jd.BodyB = m_bodies[3];
                jd.localAnchorA.Set(-10.0f, 20.0f);
                jd.localAnchorB.Set(-0.5f, 0.5f);
                p1 = jd.BodyA.GetWorldPoint(jd.localAnchorA);
                p2 = jd.BodyB.GetWorldPoint(jd.localAnchorB);
                d = p2 - p1;
                jd.length = d.Length;
                m_joints[3] = m_world.CreateJoint(jd);

                jd.BodyA = m_bodies[0];
                jd.BodyB = m_bodies[1];
                jd.localAnchorA.Set(0.5f, 0.0f);
                jd.localAnchorB.Set(-0.5f, 0.0f);
                ;
                p1 = jd.BodyA.GetWorldPoint(jd.localAnchorA);
                p2 = jd.BodyB.GetWorldPoint(jd.localAnchorB);
                d = p2 - p1;
                jd.length = d.Length;
                m_joints[4] = m_world.CreateJoint(jd);

                jd.BodyA = m_bodies[1];
                jd.BodyB = m_bodies[2];
                jd.localAnchorA.Set(0.0f, 0.5f);
                jd.localAnchorB.Set(0.0f, -0.5f);
                p1 = jd.BodyA.GetWorldPoint(jd.localAnchorA);
                p2 = jd.BodyB.GetWorldPoint(jd.localAnchorB);
                d = p2 - p1;
                jd.length = d.Length;
                m_joints[5] = m_world.CreateJoint(jd);

                jd.BodyA = m_bodies[2];
                jd.BodyB = m_bodies[3];
                jd.localAnchorA.Set(-0.5f, 0.0f);
                jd.localAnchorB.Set(0.5f, 0.0f);
                p1 = jd.BodyA.GetWorldPoint(jd.localAnchorA);
                p2 = jd.BodyB.GetWorldPoint(jd.localAnchorB);
                d = p2 - p1;
                jd.length = d.Length;
                m_joints[6] = m_world.CreateJoint(jd);

                jd.BodyA = m_bodies[3];
                jd.BodyB = m_bodies[0];
                jd.localAnchorA.Set(0.0f, -0.5f);
                jd.localAnchorB.Set(0.0f, 0.5f);
                p1 = jd.BodyA.GetWorldPoint(jd.localAnchorA);
                p2 = jd.BodyB.GetWorldPoint(jd.localAnchorB);
                d = p2 - p1;
                jd.length = d.Length;
                m_joints[7] = m_world.CreateJoint(jd);
            }
        }

        public override void Keyboard(char key)
        {
            switch (key)
            {
                case 'b':
                    for (int i = 0; i < 4; ++i)
                    {
                        if (m_bodies[i] != null)
                        {
                            m_world.DestroyBody(m_bodies[i]);
                            m_bodies[i] = null;
                            break;
                        }
                    }
                    break;

                case 'j':
                    for (int i = 0; i < 8; ++i)
                    {
                        if (m_joints[i] != null)
                        {
                            m_world.DestroyJoint(m_joints[i]);
                            m_joints[i] = null;
                            break;
                        }
                    }
                    break;
            }
        }

        public override void Step(Settings settings)
        {
            base.Step(settings);
            m_debugDraw.DrawString(5, m_textLine, "This demonstrates a soft distance joint.");
            m_textLine += 15;
            m_debugDraw.DrawString(5, m_textLine, "Press: (b) to delete a body, (j) to delete a joint");
            m_textLine += 15;
        }

        public override void JointDestroyed(b2Joint joint)
        {
            for (int i = 0; i < 8; ++i)
            {
                if (m_joints[i] == joint)
                {
                    m_joints[i] = null;
                    break;
                }
            }
        }

        public b2Body[] m_bodies = new b2Body[4];
        public b2Joint[] m_joints = new b2Joint[8];
    }
}
