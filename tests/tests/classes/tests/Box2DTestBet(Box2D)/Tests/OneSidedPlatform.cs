using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using Box2D.Dynamics.Contacts;

namespace Box2D.TestBed.Tests
{
    public class OneSidedPlatform : Test
    {
        public enum State
        {
            e_unknown = 0,
            e_above,
            e_below
        };

        public OneSidedPlatform()
        {
            // Ground
            {
                b2BodyDef bd  = b2BodyDef.Create();
                b2Body ground = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();
                shape.Set(new b2Vec2(-20.0f, 0.0f), new b2Vec2(20.0f, 0.0f));
                ground.CreateFixture(shape, 0.0f);
            }

            // Platform
            {
                b2BodyDef bd  = b2BodyDef.Create();
                bd.position.Set(0.0f, 10.0f);
                b2Body body = m_world.CreateBody(bd);

                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(3.0f, 0.5f);
                m_platform = body.CreateFixture(shape, 0.0f);

                m_bottom = 10.0f - 0.5f;
                m_top = 10.0f + 0.5f;
            }

            // Actor
            {
                b2BodyDef bd  = b2BodyDef.Create();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position.Set(0.0f, 12.0f);
                b2Body body = m_world.CreateBody(bd);

                m_radius = 0.5f;
                b2CircleShape shape = new b2CircleShape();
                shape.Radius = m_radius;
                m_character = body.CreateFixture(shape, 20.0f);

                body.LinearVelocity = new b2Vec2(0.0f, -50.0f);

                m_state = State.e_unknown;
            }
        }

        public override void PreSolve(b2Contact contact, ref b2Manifold oldManifold)
        {
            base.PreSolve(contact, ref oldManifold);

            b2Fixture fixtureA = contact.GetFixtureA();
            b2Fixture fixtureB = contact.GetFixtureB();

            if (fixtureA != m_platform && fixtureA != m_character)
            {
                return;
            }

            if (fixtureB != m_platform && fixtureB != m_character)
            {
                return;
            }

            b2Vec2 position = m_character.Body.Position;

            if (position.y < m_top + m_radius - 3.0f * b2Settings.b2_linearSlop)
            {
                contact.SetEnabled(false);
            }
        }

        public override void Step(Settings settings)
        {
            base.Step(settings);
            m_debugDraw.DrawString(5, m_textLine, "Press: (c) create a shape, (d) destroy a shape.");
            m_textLine += 15;
        }

        public float m_radius, m_top, m_bottom;
        public State m_state = new State();
        public b2Fixture m_platform = new b2Fixture();
        public b2Fixture m_character = new b2Fixture();
    }
}
