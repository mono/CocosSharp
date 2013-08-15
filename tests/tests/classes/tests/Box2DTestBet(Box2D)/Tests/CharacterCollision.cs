using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;

namespace Box2D.TestBed.Tests
{
    public class CharacterCollision : Test
    {
        public CharacterCollision()
        {
            // Ground body
            {
                b2BodyDef bd  = b2BodyDef.Create();
                b2Body ground = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();
                shape.Set(new b2Vec2(-20.0f, 0.0f), new b2Vec2(20.0f, 0.0f));
                ground.CreateFixture(shape, 0.0f);
            }

            // Collinear edges with no adjacency information.
            // This shows the problematic case where a box shape can hit
            // an internal vertex.
            {
                b2BodyDef bd  = b2BodyDef.Create();
                b2Body ground = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();
                shape.Set(new b2Vec2(-8.0f, 1.0f), new b2Vec2(-6.0f, 1.0f));
                ground.CreateFixture(shape, 0.0f);
                shape.Set(new b2Vec2(-6.0f, 1.0f), new b2Vec2(-4.0f, 1.0f));
                ground.CreateFixture(shape, 0.0f);
                shape.Set(new b2Vec2(-4.0f, 1.0f), new b2Vec2(-2.0f, 1.0f));
                ground.CreateFixture(shape, 0.0f);
            }

            // Chain shape
            {
                b2BodyDef bd  = b2BodyDef.Create();
                bd.angle = 0.25f * b2Settings.b2_pi;
                b2Body ground = m_world.CreateBody(bd);

                b2Vec2[] vs = new b2Vec2[4];
                vs[0].Set(5.0f, 7.0f);
                vs[1].Set(6.0f, 8.0f);
                vs[2].Set(7.0f, 8.0f);
                vs[3].Set(8.0f, 7.0f);
                b2ChainShape shape = new b2ChainShape();
                shape.CreateChain(vs, 4);
                ground.CreateFixture(shape, 0.0f);
            }

            // Square tiles. This shows that adjacency shapes may
            // have non-smooth collision. There is no solution
            // to this problem.
            {
                b2BodyDef bd  = b2BodyDef.Create();
                b2Body ground = m_world.CreateBody(bd);

                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(1.0f, 1.0f, new b2Vec2(4.0f, 3.0f), 0.0f);
                ground.CreateFixture(shape, 0.0f);
                shape.SetAsBox(1.0f, 1.0f, new b2Vec2(6.0f, 3.0f), 0.0f);
                ground.CreateFixture(shape, 0.0f);
                shape.SetAsBox(1.0f, 1.0f, new b2Vec2(8.0f, 3.0f), 0.0f);
                ground.CreateFixture(shape, 0.0f);
            }

            // Square made from an edge loop. Collision should be smooth.
            {
                b2BodyDef bd  = b2BodyDef.Create();
                b2Body ground = m_world.CreateBody(bd);

                b2Vec2[] vs = new b2Vec2[4];
                vs[0].Set(-1.0f, 3.0f);
                vs[1].Set(1.0f, 3.0f);
                vs[2].Set(1.0f, 5.0f);
                vs[3].Set(-1.0f, 5.0f);
                b2ChainShape shape = new b2ChainShape();
                shape.CreateLoop(vs, 4);
                ground.CreateFixture(shape, 0.0f);
            }

            // Edge loop. Collision should be smooth.
            {
                b2BodyDef bd  = b2BodyDef.Create();
                bd.position.Set(-10.0f, 4.0f);
                b2Body ground = m_world.CreateBody(bd);

                b2Vec2[] vs = new b2Vec2[10];
                vs[0].Set(0.0f, 0.0f);
                vs[1].Set(6.0f, 0.0f);
                vs[2].Set(6.0f, 2.0f);
                vs[3].Set(4.0f, 1.0f);
                vs[4].Set(2.0f, 2.0f);
                vs[5].Set(0.0f, 2.0f);
                vs[6].Set(-2.0f, 2.0f);
                vs[7].Set(-4.0f, 3.0f);
                vs[8].Set(-6.0f, 2.0f);
                vs[9].Set(-6.0f, 0.0f);
                b2ChainShape shape = new b2ChainShape();
                shape.CreateLoop(vs, 10);
                ground.CreateFixture(shape, 0.0f);
            }

            // Square character 1
            {
                b2BodyDef bd  = b2BodyDef.Create();
                bd.position.Set(-3.0f, 8.0f);
                bd.type = b2BodyType.b2_dynamicBody;
                bd.fixedRotation = true;
                bd.allowSleep = false;

                b2Body body = m_world.CreateBody(bd);

                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.5f, 0.5f);

                b2FixtureDef fd = new b2FixtureDef();
                fd.Defaults();
                fd.shape = shape;
                fd.density = 20.0f;
                body.CreateFixture(fd);
            }

            // Square character 2
            {
                b2BodyDef bd  = b2BodyDef.Create();
                bd.position.Set(-5.0f, 5.0f);
                bd.type = b2BodyType.b2_dynamicBody;
                bd.fixedRotation = true;
                bd.allowSleep = false;

                b2Body body = m_world.CreateBody(bd);

                b2PolygonShape shape = new b2PolygonShape();
                shape.SetAsBox(0.25f, 0.25f);

                b2FixtureDef fd = new b2FixtureDef();
                fd.Defaults();
                fd.shape = shape;
                fd.density = 20.0f;
                body.CreateFixture(fd);
            }

            // Hexagon character
            {
                b2BodyDef bd  = b2BodyDef.Create();
                bd.position.Set(-5.0f, 8.0f);
                bd.type = b2BodyType.b2_dynamicBody;
                bd.fixedRotation = true;
                bd.allowSleep = false;

                b2Body body = m_world.CreateBody(bd);

                float angle = 0.0f;
                float delta = b2Settings.b2_pi / 3.0f;
                b2Vec2[] vertices = new b2Vec2[6];
                for (int i = 0; i < 6; ++i)
                {
                    vertices[i].Set(0.5f * (float) Math.Cos(angle), 0.5f * (float) Math.Sin(angle));
                    angle += delta;
                }

                b2PolygonShape shape = new b2PolygonShape();
                shape.Set(vertices, 6);

                b2FixtureDef fd = new b2FixtureDef();
                fd.Defaults();
                fd.shape = shape;
                fd.density = 20.0f;
                body.CreateFixture(fd);
            }

            // Circle character
            {
                b2BodyDef bd  = b2BodyDef.Create();
                bd.position.Set(3.0f, 5.0f);
                bd.type = b2BodyType.b2_dynamicBody;
                bd.fixedRotation = true;
                bd.allowSleep = false;

                b2Body body = m_world.CreateBody(bd);

                b2CircleShape shape = new b2CircleShape();
                shape.Radius = 0.5f;

                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = shape;
                fd.density = 20.0f;
                body.CreateFixture(fd);
            }

            // Circle character
            {
                b2BodyDef bd  = b2BodyDef.Create();
                bd.position.Set(-7.0f, 6.0f);
                bd.type = b2BodyType.b2_dynamicBody;
                bd.allowSleep = false;

                m_character = m_world.CreateBody(bd);

                b2CircleShape shape = new b2CircleShape();
                shape.Radius = 0.25f;

                b2FixtureDef fd = new b2FixtureDef();
                fd.Defaults();
                fd.shape = shape;
                fd.density = 20.0f;
                fd.friction = 1.0f;
                m_character.CreateFixture(fd);
            }
        }

        public override void Step(Settings settings)
        {
            b2Vec2 v = m_character.LinearVelocity;
            v.x = -5.0f;
            m_character.LinearVelocity = v;

            base.Step(settings);
            m_debugDraw.DrawString(5, m_textLine, "This tests various character collision shapes.");
            m_textLine += 15;
            m_debugDraw.DrawString(5, m_textLine, "Limitation: square and hexagon can snag on aligned boxes.");
            m_textLine += 15;
            m_debugDraw.DrawString(5, m_textLine, "Feature: edge chains have smooth collision inside and out.");
            m_textLine += 15;
        }

        public b2Body m_character;
    }
}
