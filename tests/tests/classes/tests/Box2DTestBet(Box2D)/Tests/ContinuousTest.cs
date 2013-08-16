using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;

namespace Box2D.TestBed.Tests
{
    public class ContinuousTest : Test
    {
        public ContinuousTest()
        {
            {
                b2BodyDef bd  = new b2BodyDef();
                bd.position.Set(0.0f, 0.0f);
                b2Body body = m_world.CreateBody(bd);

                b2EdgeShape edge = new b2EdgeShape();

                edge.Set(new b2Vec2(-10.0f, 0.0f), new b2Vec2(10.0f, 0.0f));
                body.CreateFixture(edge, 0.0f);

                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.2f, 1.0f, new b2Vec2(0.5f, 1.0f), 0.0f);
                body.CreateFixture(shape, 0.0f);
            }

#if true
            {
                b2BodyDef bd  = new b2BodyDef();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position.Set(0.0f, 20.0f);
                //bd.angle = 0.1f;

                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(2.0f, 0.1f);

                m_body = m_world.CreateBody(bd);
                m_body.CreateFixture(shape, 1.0f);

                m_angularVelocity = Rand.RandomFloat(-50.0f, 50.0f);
                //m_angularVelocity = 46.661274f;
                m_body.LinearVelocity = new b2Vec2(0.0f, -100.0f);
                m_body.AngularVelocity = m_angularVelocity;
            }
#else
        {
            b2BodyDef bd  = new b2BodyDef();
            bd.type = b2BodyType.b2_dynamicBody;
            bd.position.Set(0.0f, 2.0f);
            b2Body body = m_world.CreateBody(bd);

            b2CircleShape shape = new b2CircleShape();
            shape.Position = b2Vec2.Zero;
            shape.Radius = 0.5f;
            body.CreateFixture(shape, 1.0f);

            bd.bullet = true;
            bd.position.Set(0.0f, 10.0f);
            body = m_world.CreateBody(bd);
            body.CreateFixture(shape, 1.0f);
            body.LinearVelocity = new b2Vec2(0.0f, -100.0f);
        }
#endif
        }

        public void Launch()
        {
            m_body.SetTransform(new b2Vec2(0.0f, 20.0f), 0.0f);
            m_angularVelocity = Rand.RandomFloat(-50.0f, 50.0f);
            m_body.LinearVelocity = new b2Vec2(0.0f, -100.0f);
            m_body.AngularVelocity = m_angularVelocity;
        }

        protected override void Draw(Settings settings)
        {
            base.Draw(settings);

            if (b2DistanceProxy.b2_gjkCalls > 0)
            {
                m_debugDraw.DrawString(5, m_textLine, "gjk calls = {0}, ave gjk iters = {1:000.0}, max gjk iters = {2}",
                                       b2DistanceProxy.b2_gjkCalls,
                                       b2DistanceProxy.b2_gjkIters / (float)(b2DistanceProxy.b2_gjkCalls),
                                       b2DistanceProxy.b2_gjkMaxIters);
                m_textLine += 15;
            }

            if (b2TimeOfImpact.b2_toiCalls > 0)
            {
                m_debugDraw.DrawString(5, m_textLine, "toi calls = {0}, ave toi iters = {1:000.0}, max toi iters = {2}",
                                       b2TimeOfImpact.b2_toiCalls,
                                       b2TimeOfImpact.b2_toiIters / (float)(b2TimeOfImpact.b2_toiCalls),
                                       b2TimeOfImpact.b2_toiMaxRootIters);
                m_textLine += 15;

                m_debugDraw.DrawString(5, m_textLine, "ave toi root iters = {0:000.0}, max toi root iters = {1}",
                                       b2TimeOfImpact.b2_toiRootIters / (float)(b2TimeOfImpact.b2_toiCalls),
                                       b2TimeOfImpact.b2_toiMaxRootIters);
                m_textLine += 15;
            }
        }

        public override void Step(Settings settings)
        {
            if (m_stepCount == 12)
            {
                m_stepCount += 0;
            }

            base.Step(settings);

            if (m_stepCount % 60 == 0)
            {
                //Launch();
            }
        }

        public b2Body m_body;
        public float m_angularVelocity;
    }
}
