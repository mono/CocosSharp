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
    public class BulletTest : Test
    {

        public BulletTest()
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

            {
                b2BodyDef bd  = new b2BodyDef();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position.Set(0.0f, 4.0f);

                b2PolygonShape box = new b2PolygonShape();
                box.SetAsBox(2.0f, 0.1f);

                m_body = m_world.CreateBody(bd);
                m_body.CreateFixture(box, 1.0f);

                box.SetAsBox(0.25f, 0.25f);

                //m_x = RandomFloat(-1.0f, 1.0f);
                m_x = 0.20352793f;
                bd.position.Set(m_x, 10.0f);
                bd.bullet = true;

                m_bullet = m_world.CreateBody(bd);
                m_bullet.CreateFixture(box, 100.0f);

                m_bullet.LinearVelocity = new b2Vec2(0.0f, -50.0f);
            }
        }

        public void Launch()
        {
            m_body.SetTransform(new b2Vec2(0.0f, 4.0f), 0.0f);
            m_body.LinearVelocity = b2Vec2.Zero;
            m_body.AngularVelocity = 0.0f;

            m_x = Rand.RandomFloat(-1.0f, 1.0f);
            m_bullet.SetTransform(new b2Vec2(m_x, 10.0f), 0.0f);
            m_bullet.LinearVelocity = new b2Vec2(0.0f, -50.0f);
            m_bullet.AngularVelocity = 0.0f;

            b2DistanceProxy.b2_gjkCalls = 0;
            b2DistanceProxy.b2_gjkIters = 0;
            b2DistanceProxy.b2_gjkMaxIters = 0;

            b2TimeOfImpact.b2_toiCalls = 0;
            b2TimeOfImpact.b2_toiIters = 0;
            b2TimeOfImpact.b2_toiMaxIters = 0;
            b2TimeOfImpact.b2_toiRootIters = 0;
            b2TimeOfImpact.b2_toiMaxRootIters = 0;
        }

        protected override void Draw(Settings settings)
        {
            base.Draw(settings);

            if (b2DistanceProxy.b2_gjkCalls > 0)
            {
                m_debugDraw.DrawString(5, m_textLine, "gjk calls = {0}, ave gjk iters = {1:000.0}, max gjk iters = {2}",
                                       b2DistanceProxy.b2_gjkCalls,
                                       b2DistanceProxy.b2_gjkIters / (float)b2DistanceProxy.b2_gjkCalls,
                                       b2DistanceProxy.b2_gjkMaxIters);
                m_textLine += 15;
            }

            if (b2TimeOfImpact.b2_toiCalls > 0)
            {
                m_debugDraw.DrawString(5, m_textLine, "toi calls = {0}, ave toi iters = {1:000.0}, max toi iters = {2}",
                                       b2TimeOfImpact.b2_toiCalls,
                                       b2TimeOfImpact.b2_toiIters / (float)b2TimeOfImpact.b2_toiCalls,
                                       b2TimeOfImpact.b2_toiMaxRootIters);
                m_textLine += 15;

                m_debugDraw.DrawString(5, m_textLine, "ave toi root iters = {0:000.0}, max toi root iters = {1}",
                                       b2TimeOfImpact.b2_toiRootIters / (float)b2TimeOfImpact.b2_toiCalls,
                                       b2TimeOfImpact.b2_toiMaxRootIters);
                m_textLine += 15;
            }
        }

        public override void Step(Settings settings)
        {
            base.Step(settings);

            if (m_stepCount % 60 == 0)
            {
                Launch();
            }
        }

        public b2Body m_body;
        public b2Body m_bullet;
        public float m_x;
    }
}
