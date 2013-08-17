using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;
using Box2D.Collision.Shapes;
using System.Diagnostics;

namespace Box2D.Collision
{
    public class b2DistanceProxy
    {
        // GJK using Voronoi regions (Christer Ericson) and Barycentric coordinates.
        public static int b2_gjkCalls, b2_gjkIters, b2_gjkMaxIters;

        private b2Vec2[] m_buffer;
        private b2Vec2[] m_vertices;
        private int m_count;
        private float m_radius;

        public float Radius { get { return (m_radius); } set { m_radius = value; } }
        public int Count { get { return (m_count); } set { m_count = value; } }

        public static b2DistanceProxy Create()
        {
            b2DistanceProxy bp = new b2DistanceProxy();
            bp.m_vertices = null;
            bp.m_count = 0;
            bp.m_radius = 0.0f;
            bp.m_buffer = new b2Vec2[2];
            return (bp);
        }

        public static b2DistanceProxy Create(b2Shape shape, int index)
        {
            b2DistanceProxy bp = Create();
            bp.Set(shape, index);
            return (bp);
        }

        public int GetVertexCount()
        {
            return m_count;
        }

        public b2Vec2 GetVertex(int index)
        {
            Debug.Assert(0 <= index && index < m_count);
            return m_vertices[index];
        }
        public b2Vec2 GetVertex(uint index)
        {
            Debug.Assert(0 <= index && index < m_count);
            return m_vertices[index];
        }

        public int GetSupport(b2Vec2 d)
        {
            int bestIndex = 0;
            float bestValue = b2Math.b2Dot(ref m_vertices[0], ref d);
            for (int i = 1; i < m_count; ++i)
            {
                float value = b2Math.b2Dot(ref m_vertices[i], ref d);
                if (value > bestValue)
                {
                    bestIndex = i;
                    bestValue = value;
                }
            }

            return bestIndex;
        }

        public b2Vec2 GetSupportVertex(b2Vec2 d)
        {
            int bestIndex = 0;
            float bestValue = b2Math.b2Dot(ref m_vertices[0], ref d);
            for (int i = 1; i < m_count; ++i)
            {
                float value = b2Math.b2Dot(ref m_vertices[i], ref d);
                if (value > bestValue)
                {
                    bestIndex = i;
                    bestValue = value;
                }
            }

            return m_vertices[bestIndex];
        }

        public void Set(b2Shape shape, int index)
        {
            switch (shape.ShapeType)
            {
                case b2ShapeType.e_circle:
                    {
                        b2CircleShape circle = (b2CircleShape)shape;
                        m_vertices = new b2Vec2[] { circle.Position };
                        m_count = 1;
                        m_radius = circle.Radius;
                    }
                    break;

                case b2ShapeType.e_polygon:
                    {
                        b2PolygonShape polygon = (b2PolygonShape)shape;
                        m_vertices = polygon.Vertices;
                        m_count = polygon.VertexCount;
                        m_radius = polygon.Radius;
                    }
                    break;

                case b2ShapeType.e_chain:
                    {
                        b2ChainShape chain = (b2ChainShape)shape;
                        Debug.Assert(0 <= index && index < chain.Count);

                        m_buffer[0] = chain.Vertices[index];
                        if (index + 1 < chain.Count)
                        {
                            m_buffer[1] = chain.Vertices[index + 1];
                        }
                        else
                        {
                            m_buffer[1] = chain.Vertices[0];
                        }

                        m_vertices = m_buffer;
                        m_count = 2;
                        m_radius = chain.Radius;
                    }
                    break;

                case b2ShapeType.e_edge:
                    {
                        b2EdgeShape edge = (b2EdgeShape)shape;
                        m_vertices = new b2Vec2[] { edge.Vertex1, edge.Vertex2 };
                        m_count = 2;
                        m_radius = edge.Radius;
                    }
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }
        }
    }
}
