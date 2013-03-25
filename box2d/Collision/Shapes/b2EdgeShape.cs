using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;

namespace Box2D.Collision.Shapes
{
    public class b2EdgeShape : b2Shape
    {
        /// Optional adjacent vertices. These are used for smooth collision.
        protected b2Vec2 m_vertex0 = new b2Vec2();
        protected b2Vec2 m_vertex3 = new b2Vec2();
        protected bool m_hasVertex0, m_hasVertex3;
        /// These are the edge vertices
        protected b2Vec2 m_vertex1 = new b2Vec2();
        protected b2Vec2 m_vertex2 = new b2Vec2();

        public bool HasVertex0
        {
            get { return (m_hasVertex0); }
            set { m_hasVertex0 = value; }
        }
        public bool HasVertex3
        {
            get { return (m_hasVertex3); }
            set { m_hasVertex3 = value; }
        }
        public b2Vec2 Vertex0
        {
            get { return (m_vertex0); }
            set { m_vertex0 = value; }
        }
        public b2Vec2 Vertex1
        {
            get { return (m_vertex1); }
            set { m_vertex1 = value; }
        }
        public b2Vec2 Vertex2
        {
            get { return (m_vertex2); }
            set { m_vertex2 = value; }
        }
        public b2Vec2 Vertex3
        {
            get { return (m_vertex3); }
            set { m_vertex3 = value; }
        }

        public b2EdgeShape()
        {
            m_type = b2ShapeType.e_edge;
            m_radius = b2Settings.b2_polygonRadius;
            m_vertex0.x = 0.0f;
            m_vertex0.y = 0.0f;
            m_vertex3.x = 0.0f;
            m_vertex3.y = 0.0f;
            m_hasVertex0 = false;
            m_hasVertex3 = false;
        }

        public b2EdgeShape(b2EdgeShape e)
            : base((b2Shape)e)
        {
            m_vertex1 = e.m_vertex1;
            m_vertex2 = e.m_vertex2;
            m_vertex3 = e.m_vertex3;
            m_vertex0 = e.m_vertex0;
            m_hasVertex0 = e.m_hasVertex0;
            m_hasVertex3 = e.m_hasVertex3;
        }

        public virtual void Set(b2Vec2 v1, b2Vec2 v2)
        {
            m_vertex1 = v1;
            m_vertex2 = v2;
            m_hasVertex0 = false;
            m_hasVertex3 = false;
        }

        public virtual b2Shape Clone()
        {
            b2EdgeShape clone = new b2EdgeShape(this);
            return clone;
        }

        public virtual int GetChildCount()
        {
            return 1;
        }

        public virtual bool TestPoint(b2Transform xf, b2Vec2 p)
        {
            return false;
        }

        // p = p1 + t * d
        // v = v1 + s * e
        // p1 + t * d = v1 + s * e
        // s * e - t * d = p1 - v1
        public virtual bool RayCast(out b2RayCastOutput output, b2RayCastInput input,
                                    b2Transform xf, int childIndex)
        {
            output = b2RayCastOutput.Zero;

            // Put the ray into the edge's frame of reference.
            b2Vec2 p1 = b2Math.b2MulT(xf.q, input.p1 - xf.p);
            b2Vec2 p2 = b2Math.b2MulT(xf.q, input.p2 - xf.p);
            b2Vec2 d = p2 - p1;

            b2Vec2 v1 = m_vertex1;
            b2Vec2 v2 = m_vertex2;
            b2Vec2 e = v2 - v1;
            b2Vec2 normal = new b2Vec2(e.y, -e.x);
            normal.Normalize();

            // q = p1 + t * d
            // dot(normal, q - v1) = 0
            // dot(normal, p1 - v1) + t * dot(normal, d) = 0
            float numerator = b2Math.b2Dot(normal, v1 - p1);
            float denominator = b2Math.b2Dot(normal, d);

            if (denominator == 0.0f)
            {
                return false;
            }

            float t = numerator / denominator;
            if (t < 0.0f || input.maxFraction < t)
            {
                return false;
            }

            b2Vec2 q = p1 + t * d;

            // q = v1 + s * r
            // s = dot(q - v1, r) / dot(r, r)
            b2Vec2 r = v2 - v1;
            float rr = b2Math.b2Dot(r, r);
            if (rr == 0.0f)
            {
                return false;
            }

            float s = b2Math.b2Dot(q - v1, r) / rr;
            if (s < 0.0f || 1.0f < s)
            {
                return false;
            }

            output.fraction = t;
            if (numerator > 0.0f)
            {
                output.normal = -normal;
            }
            else
            {
                output.normal = normal;
            }
            return true;
        }

        public virtual b2AABB ComputeAABB(b2Transform xf, int childIndex)
        {
            b2Vec2 v1 = b2Math.b2Mul(xf, m_vertex1);
            b2Vec2 v2 = b2Math.b2Mul(xf, m_vertex2);

            b2Vec2 lower = b2Math.b2Min(v1, v2);
            b2Vec2 upper = b2Math.b2Max(v1, v2);

            b2Vec2 r(m_radius, m_radius);
            b2AABB aabb = new b2AABB();
            aabb.lowerBound = lower - r;
            aabb.upperBound = upper + r;
            return(aabb);
        }

        public virtual b2MassData ComputeMass(float density)
        {
            b2MassData massData = new b2MassData();
            massData.mass = 0.0f;
            massData.center = 0.5f * (m_vertex1 + m_vertex2);
            massData.I = 0.0f;
            return (massData);
        }
    }
}
