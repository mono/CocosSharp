using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;

namespace Box2D.TestBed.Tests
{
    public class CompoundShapes : Test
    {
        public CompoundShapes()
        {
            {
                b2BodyDef bd  = new b2BodyDef();
                bd.position.Set(0.0f, 0.0f);
                b2Body body = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();
                shape.Set(new b2Vec2(50.0f, 0.0f), new b2Vec2(-50.0f, 0.0f));

                body.CreateFixture(shape, 0.0f);
            }

            {
                b2CircleShape circle1 = new b2CircleShape();
                circle1.Radius = 0.5f;
                circle1.Position = new b2Vec2(-0.5f, 0.5f);

                b2CircleShape circle2 = new b2CircleShape();
                circle2.Radius = 0.5f;
                circle2.Position = new b2Vec2(0.5f, 0.5f);

                for (int i = 0; i < 10; ++i)
                {
                    float x = Rand.RandomFloat(-0.1f, 0.1f);
                    b2BodyDef bd  = new b2BodyDef();
                    bd.type = b2BodyType.b2_dynamicBody;
                    bd.position.Set(x + 5.0f, 1.05f + 2.5f * i);
                    bd.angle = Rand.RandomFloat(-b2Settings.b2_pi, b2Settings.b2_pi);
                    b2Body body = m_world.CreateBody(bd);
                    body.CreateFixture(circle1, 2.0f);
                    body.CreateFixture(circle2, 0.0f);
                }
            }

            {
                b2PolygonShape polygon1 = new b2PolygonShape();
                polygon1.SetAsBox(0.25f, 0.5f);

                b2PolygonShape polygon2 = new b2PolygonShape();
                polygon2.SetAsBox(0.25f, 0.5f, new b2Vec2(0.0f, -0.5f), 0.5f * b2Settings.b2_pi);

                for (int i = 0; i < 10; ++i)
                {
                    float x = Rand.RandomFloat(-0.1f, 0.1f);
                    b2BodyDef bd  = new b2BodyDef();
                    bd.type = b2BodyType.b2_dynamicBody;
                    bd.position.Set(x - 5.0f, 1.05f + 2.5f * i);
                    bd.angle = Rand.RandomFloat(-b2Settings.b2_pi, b2Settings.b2_pi);
                    b2Body body = m_world.CreateBody(bd);
                    body.CreateFixture(polygon1, 2.0f);
                    body.CreateFixture(polygon2, 2.0f);
                }
            }

            {
                b2Transform xf1 = new b2Transform();
                xf1.q.Set(0.3524f * b2Settings.b2_pi);
                xf1.p = xf1.q.GetXAxis();

                b2Vec2[] vertices = new b2Vec2[3];

                b2PolygonShape triangle1 = new b2PolygonShape();
                vertices[0] = b2Math.b2Mul(xf1, new b2Vec2(-1.0f, 0.0f));
                vertices[1] = b2Math.b2Mul(xf1, new b2Vec2(1.0f, 0.0f));
                vertices[2] = b2Math.b2Mul(xf1, new b2Vec2(0.0f, 0.5f));
                triangle1.Set(vertices, 3);

                b2Transform xf2 = new b2Transform();
                xf2.q.Set(-0.3524f * b2Settings.b2_pi);
                xf2.p = -xf2.q.GetXAxis();

                b2PolygonShape triangle2 = new b2PolygonShape();
                vertices[0] = b2Math.b2Mul(xf2, new b2Vec2(-1.0f, 0.0f));
                vertices[1] = b2Math.b2Mul(xf2, new b2Vec2(1.0f, 0.0f));
                vertices[2] = b2Math.b2Mul(xf2, new b2Vec2(0.0f, 0.5f));
                triangle2.Set(vertices, 3);

                for (int i = 0; i < 10; ++i)
                {
                    float x = Rand.RandomFloat(-0.1f, 0.1f);
                    b2BodyDef bd  = new b2BodyDef();
                    bd.type = b2BodyType.b2_dynamicBody;
                    bd.position.Set(x, 2.05f + 2.5f * i);
                    bd.angle = 0.0f;
                    b2Body body = m_world.CreateBody(bd);
                    body.CreateFixture(triangle1, 2.0f);
                    body.CreateFixture(triangle2, 2.0f);
                }
            }

            {
                b2PolygonShape bottom = new b2PolygonShape();
                bottom.SetAsBox(1.5f, 0.15f);

                b2PolygonShape left = new b2PolygonShape();
                left.SetAsBox(0.15f, 2.7f, new b2Vec2(-1.45f, 2.35f), 0.2f);

                b2PolygonShape right = new b2PolygonShape();
                right.SetAsBox(0.15f, 2.7f, new b2Vec2(1.45f, 2.35f), -0.2f);

                b2BodyDef bd  = new b2BodyDef();
                bd.type = b2BodyType.b2_dynamicBody;
                bd.position.Set(0.0f, 2.0f);
                b2Body body = m_world.CreateBody(bd);
                body.CreateFixture(bottom, 4.0f);
                body.CreateFixture(left, 4.0f);
                body.CreateFixture(right, 4.0f);
            }
        }
    }
}
