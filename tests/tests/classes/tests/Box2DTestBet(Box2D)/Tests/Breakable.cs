using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using Box2D.Dynamics.Contacts;

namespace Box2D.TestBed.Tests
{
    public class Breakable : Test
    {
        public const int e_count = 7;

        public Breakable()
        {
            // Ground body
            {
                b2BodyDef bd  = b2BodyDef.Create();
                b2Body ground = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();
                shape.Set(new b2Vec2(-40.0f, 0.0f), new b2Vec2(40.0f, 0.0f));
                ground.CreateFixture(shape, 0.0f);
            }

            // Breakable dynamic body
            {
                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position = new b2Vec2(0.0f, 40.0f);
                bd.angle = 0.25f * b2Settings.b2_pi;
                m_body1 = m_world.CreateBody(bd);

                m_shape1.SetAsBox(0.5f, 0.5f, new b2Vec2(-0.5f, 0.0f), 0.0f);
                m_piece1 = m_body1.CreateFixture(m_shape1, 1.0f);

                m_shape2.SetAsBox(0.5f, 0.5f, new b2Vec2(0.5f, 0.0f), 0.0f);
                m_piece2 = m_body1.CreateFixture(m_shape2, 1.0f);
            }

            m_break = false;
            m_broke = false;
        }

        public override void PostSolve(b2Contact contact, ref b2ContactImpulse impulse)
        {
            if (m_broke)
            {
                // The body already broke.
                return;
            }

            // Should the body break?
            int count = contact.GetManifold().pointCount;

            float maxImpulse = 0.0f;
            for (int i = 0; i < count; ++i)
            {
                maxImpulse = Math.Max(maxImpulse, impulse.normalImpulses[i]);
            }

            if (maxImpulse > 40.0f)
            {
                // Flag the body for breaking.
                m_break = true;
            }
        }

        public void Break()
        {
            // Create two bodies from one.
            b2Body body1 = m_piece1.Body;
            b2Vec2 center = body1.WorldCenter;

            body1.DestroyFixture(m_piece2);
            m_piece2 = null;

            b2BodyDef bd  = b2BodyDef.Create();
            bd.type = b2BodyType.b2_dynamicBody;
            bd.position = body1.Position;
            bd.angle = body1.Angle;

            b2Body body2 = m_world.CreateBody(bd);
            m_piece2 = body2.CreateFixture(m_shape2, 1.0f);

            // Compute consistent velocities for new bodies based on
            // cached velocity.
            b2Vec2 center1 = body1.WorldCenter;
            b2Vec2 center2 = body2.WorldCenter;

            var diff1 = center1 - center;
            var diff2 = center2 - center;

            b2Vec2 velocity1 = m_velocity + b2Math.b2Cross(m_angularVelocity, ref diff1);
            b2Vec2 velocity2 = m_velocity + b2Math.b2Cross(m_angularVelocity, ref diff2);

            body1.AngularVelocity = m_angularVelocity;
            body1.LinearVelocity = velocity1;

            body2.AngularVelocity = m_angularVelocity;
            body2.LinearVelocity = velocity2;
        }

        public override void Step(Settings settings)
        {
            if (m_break)
            {
                Break();
                m_broke = true;
                m_break = false;
            }

            // Cache velocities to improve movement on breakage.
            if (m_broke == false)
            {
                m_velocity = m_body1.LinearVelocity;
                m_angularVelocity = m_body1.AngularVelocity;
            }

            base.Step(settings);
        }

        public b2Body m_body1;
        public b2Vec2 m_velocity = new b2Vec2();
        public float m_angularVelocity;
        public b2PolygonShape m_shape1 = new b2PolygonShape();
        public b2PolygonShape m_shape2 = new b2PolygonShape();
        public b2Fixture m_piece1 = new b2Fixture();
        public b2Fixture m_piece2 = new b2Fixture();

        public bool m_broke;
        public bool m_break;
    }
}
