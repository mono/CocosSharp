using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision;
using Box2D.Collision.Shapes;
using Box2D.Common;

namespace Box2D.TestBed.Tests
{
    public class PolyCollision : Test
    {
        public PolyCollision()
        {
            {
                m_polygonA.SetAsBox(0.2f, 0.4f);
                m_transformA.Set(new b2Vec2(0.0f, 0.0f), 0.0f);
            }

            {
                m_polygonB.SetAsBox(0.5f, 0.5f);
                m_positionB.Set(19.345284f, 1.5632932f);
                m_angleB = 1.9160721f;
                m_transformB.Set(m_positionB, m_angleB);
            }
        }

        private void Step(Settings settings)
        {
            b2Manifold manifold = new b2Manifold();
            b2Collision.b2CollidePolygons(ref manifold, m_polygonA, ref m_transformA, m_polygonB, ref m_transformB);

            b2WorldManifold worldManifold = new b2WorldManifold();
            worldManifold.Initialize(ref manifold, m_transformA, m_polygonA.Radius, m_transformB, m_polygonB.Radius);

            m_debugDraw.DrawString(5, m_textLine, "point count = %d", manifold.pointCount);
            m_textLine += 15;

            {
                b2Color color = new b2Color(0.9f, 0.9f, 0.9f);
                b2Vec2[] v = new b2Vec2[b2Settings.b2_maxPolygonVertices];
                for (int i = 0; i < m_polygonA.VertexCount; ++i)
                {
                    v[i] = b2Math.b2Mul(m_transformA, m_polygonA.Vertices[i]);
                }
                m_debugDraw.DrawPolygon(v, m_polygonA.VertexCount, color);

                for (int i = 0; i < m_polygonB.VertexCount; ++i)
                {
                    v[i] = b2Math.b2Mul(m_transformB, m_polygonB.Vertices[i]);
                }
                m_debugDraw.DrawPolygon(v, m_polygonB.VertexCount, color);
            }

            for (int i = 0; i < manifold.pointCount; ++i)
            {
                m_debugDraw.DrawPoint(worldManifold.points[i], 4.0f, new b2Color(0.9f, 0.3f, 0.3f));
            }
        }

        public override void Keyboard(char key)
        {
            switch (key)
            {
                case 'a':
                    m_positionB.x -= 0.1f;
                    break;

                case 'd':
                    m_positionB.x += 0.1f;
                    break;

                case 's':
                    m_positionB.y -= 0.1f;
                    break;

                case 'w':
                    m_positionB.y += 0.1f;
                    break;

                case 'q':
                    m_angleB += 0.1f * b2Settings.b2_pi;
                    break;

                case 'e':
                    m_angleB -= 0.1f * b2Settings.b2_pi;
                    break;
            }

            m_transformB.Set(m_positionB, m_angleB);
        }

        public b2PolygonShape m_polygonA = new b2PolygonShape();
        public b2PolygonShape m_polygonB = new b2PolygonShape();

        public b2Transform m_transformA = new b2Transform();
        public b2Transform m_transformB = new b2Transform();

        public b2Vec2 m_positionB = new b2Vec2();
        public float m_angleB;
    }
}
