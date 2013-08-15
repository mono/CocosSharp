using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision;
using Box2D.Collision.Shapes;
using Box2D.Common;

namespace Box2D.TestBed.Tests
{
    public class DistanceTest : Test
    {

        public DistanceTest()
        {
            {
                m_transformA.SetIdentity();
                m_transformA.p.Set(0.0f, -0.2f);
                m_polygonA.SetAsBox(10.0f, 0.2f);
            }

            {
                m_positionB.Set(12.017401f, 0.13678508f);
                m_angleB = -0.0109265f;
                m_transformB.Set(m_positionB, m_angleB);

                m_polygonB.SetAsBox(2.0f, 0.1f);
            }
        }

        protected override void Draw(Settings settings)
        {
            base.Draw(settings);

            b2DistanceInput input = b2DistanceInput.Create();
            input.proxyA.Set(m_polygonA, 0);
            input.proxyB.Set(m_polygonB, 0);
            input.transformA = m_transformA;
            input.transformB = m_transformB;
            input.useRadii = true;
            b2SimplexCache cache = b2SimplexCache.Create();
            cache.count = 0;
            b2DistanceOutput output = new b2DistanceOutput();
            b2Simplex.b2Distance(ref output, ref cache, ref input);

            m_debugDraw.DrawString(5, m_textLine, "distance = {0}", output.distance);
            m_textLine += 15;

            m_debugDraw.DrawString(5, m_textLine, "iterations = {0}", output.iterations);
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

            b2Vec2 x1 = output.pointA;
            b2Vec2 x2 = output.pointB;

            b2Color c1 = new b2Color(1.0f, 0.0f, 0.0f);
            m_debugDraw.DrawPoint(x1, 4.0f, c1);

            b2Color c2 = new b2Color(1.0f, 1.0f, 0.0f);
            m_debugDraw.DrawPoint(x2, 4.0f, c2);
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

        public b2Vec2 m_positionB = new b2Vec2();
        public float m_angleB;

        public b2Transform m_transformA = new b2Transform();
        public b2Transform m_transformB = new b2Transform();
        public b2PolygonShape m_polygonA = new b2PolygonShape();
        public b2PolygonShape m_polygonB = new b2PolygonShape();
    }
}
