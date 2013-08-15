using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Box2D.Collision;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;

namespace Box2D.TestBed.Tests
{
    public class PolyShapesCallback : b2QueryCallback
    {
        public const int e_maxCount = 4;

        public PolyShapesCallback()
        {
            m_count = 0;
            m_circle = new b2CircleShape();
        }

        public void DrawFixture(b2Fixture fixture)
        {
            b2Color color = new b2Color(0.95f, 0.95f, 0.6f);
            b2Transform xf = fixture.Body.Transform;

            switch (fixture.ShapeType)
            {
                case b2ShapeType.e_circle:
                    {
                        b2CircleShape circle = (b2CircleShape) fixture.Shape;

                        b2Vec2 center = b2Math.b2Mul(xf, circle.Position);
                        float radius = circle.Radius;

                        m_debugDraw.DrawCircle(center, radius, color);
                    }
                    break;

                case b2ShapeType.e_polygon:
                    {
                        b2PolygonShape poly = (b2PolygonShape) fixture.Shape;
                        int vertexCount = poly.VertexCount;
                        Debug.Assert(vertexCount <= b2Settings.b2_maxPolygonVertices);
                        b2Vec2[] vertices = new b2Vec2[b2Settings.b2_maxPolygonVertices];

                        for (int i = 0; i < vertexCount; ++i)
                        {
                            vertices[i] = b2Math.b2Mul(xf, poly.Vertices[i]);
                        }

                        m_debugDraw.DrawPolygon(vertices, vertexCount, color);
                    }
                    break;
            }
        }

        /// Called for each fixture found in the query AABB.
        /// @return false to terminate the query.
        public override bool ReportFixture(b2Fixture fixture)
        {
            if (m_count == e_maxCount)
            {
                return false;
            }

            b2Body body = fixture.Body;
            b2Shape shape = fixture.Shape;
            var transform = body.Transform;
            bool overlap = b2Collision.b2TestOverlap(shape, 0, m_circle, 0, ref transform, ref m_transform);

            if (overlap)
            {
                DrawFixture(fixture);
                ++m_count;
            }

            return true;
        }

        public b2CircleShape m_circle;
        public b2Transform m_transform;
        public b2Draw m_debugDraw;
        public int m_count;
    }

    public class PolyShapes : Test
    {
        private const int k_maxBodies = 256;

        public PolyShapes()
        {
            // Ground body
            {
                b2BodyDef bd  = b2BodyDef.Create();
                b2Body ground = m_world.CreateBody(bd);

                b2EdgeShape shape = new b2EdgeShape();
                shape.Set(new b2Vec2(-40.0f, 0.0f), new b2Vec2(40.0f, 0.0f));
                ground.CreateFixture(shape, 0.0f);
            }

            {
                b2Vec2[] vertices = new b2Vec2[3];
                vertices[0].Set(-0.5f, 0.0f);
                vertices[1].Set(0.5f, 0.0f);
                vertices[2].Set(0.0f, 1.5f);
                m_polygons[0] = new b2PolygonShape();
                m_polygons[0].Set(vertices, 3);
            }

            {
                b2Vec2[] vertices = new b2Vec2[3];
                vertices[0].Set(-0.1f, 0.0f);
                vertices[1].Set(0.1f, 0.0f);
                vertices[2].Set(0.0f, 1.5f);
                m_polygons[1] = new b2PolygonShape();
                m_polygons[1].Set(vertices, 3);
            }

            {
                float w = 1.0f;
                float b = w / (2.0f + b2Math.b2Sqrt(2.0f));
                float s = b2Math.b2Sqrt(2.0f) * b;

                b2Vec2[] vertices = new b2Vec2[8];
                vertices[0].Set(0.5f * s, 0.0f);
                vertices[1].Set(0.5f * w, b);
                vertices[2].Set(0.5f * w, b + s);
                vertices[3].Set(0.5f * s, w);
                vertices[4].Set(-0.5f * s, w);
                vertices[5].Set(-0.5f * w, b + s);
                vertices[6].Set(-0.5f * w, b);
                vertices[7].Set(-0.5f * s, 0.0f);

                m_polygons[2] = new b2PolygonShape();
                m_polygons[2].Set(vertices, 8);
            }

            {
                m_polygons[3] = new b2PolygonShape();
                m_polygons[3].SetAsBox(0.5f, 0.5f);
            }

            {
                m_circle.Radius = 0.5f;
            }

            m_bodyIndex = 0;
        }

        private void Create(int index)
        {
            if (m_bodies[m_bodyIndex] != null)
            {
                m_world.DestroyBody(m_bodies[m_bodyIndex]);
                m_bodies[m_bodyIndex] = null;
            }

            b2BodyDef bd  = b2BodyDef.Create();
            bd.type = b2BodyType.b2_dynamicBody;

            float x = Rand.RandomFloat(-2.0f, 2.0f);
            bd.position.Set(x, 10.0f);
            bd.angle = Rand.RandomFloat(-b2Settings.b2_pi, b2Settings.b2_pi);

            if (index == 4)
            {
                bd.angularDamping = 0.02f;
            }

            m_bodies[m_bodyIndex] = m_world.CreateBody(bd);

            if (index < 4)
            {
                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = m_polygons[index];
                fd.density = 1.0f;
                fd.friction = 0.3f;
                m_bodies[m_bodyIndex].CreateFixture(fd);
            }
            else
            {
                b2FixtureDef fd = new b2FixtureDef();
                fd.shape = m_circle;
                fd.density = 1.0f;
                fd.friction = 0.3f;

                m_bodies[m_bodyIndex].CreateFixture(fd);
            }

            m_bodyIndex = (m_bodyIndex + 1) % k_maxBodies;
        }

        public void DestroyBody()
        {
            for (int i = 0; i < k_maxBodies; ++i)
            {
                if (m_bodies[i] != null)
                {
                    m_world.DestroyBody(m_bodies[i]);
                    m_bodies[i] = null;
                    return;
                }
            }
        }

        public override void Keyboard(char key)
        {
            switch (key)
            {
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                    Create(key - '1');
                    break;

                case 'a':
                    for (int i = 0; i < k_maxBodies; i += 2)
                    {
                        if (m_bodies[i] != null)
                        {
                            bool active = m_bodies[i].IsActive();
                            m_bodies[i].SetActive(!active);
                        }
                    }
                    break;

                case 'd':
                    DestroyBody();
                    break;
            }
        }

        public override void Step(Settings settings)
        {
            base.Step(settings);

            PolyShapesCallback callback = new PolyShapesCallback();
            callback.m_circle.Radius = 2.0f;
            callback.m_circle.Position.Set(0.0f, 1.1f);
            callback.m_transform.SetIdentity();
            callback.m_debugDraw = m_debugDraw;

            b2AABB aabb = callback.m_circle.ComputeAABB(callback.m_transform, 0);

            m_world.QueryAABB(callback, aabb);

            b2Color color = new b2Color(0.4f, 0.7f, 0.8f);
            m_debugDraw.DrawCircle(callback.m_circle.Position, callback.m_circle.Radius, color);

            m_debugDraw.DrawString(5, m_textLine, "Press 1-5 to drop stuff");
            m_textLine += 15;
            m_debugDraw.DrawString(5, m_textLine, "Press 'a' to (de)activate some bodies");
            m_textLine += 15;
            m_debugDraw.DrawString(5, m_textLine, "Press 'd' to destroy a body");
            m_textLine += 15;
        }

        public int m_bodyIndex;
        public b2Body[] m_bodies = new b2Body[k_maxBodies];
        public b2PolygonShape[] m_polygons = new b2PolygonShape[4];
        public b2CircleShape m_circle = new b2CircleShape();
    }
}
