using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;

namespace Box2D.Collision.Shapes
{
    public class b2CircleShape : b2Shape
    {
        /// Position
        protected b2Vec2 m_p = new b2Vec2();

        /// Get the vertex count.
        public virtual int GetVertexCount() { return 1; }

        public b2CircleShape()
        {
            m_type = b2ShapeType.e_circle;
            m_radius = 0.0f;
            m_p.SetZero();
        }

        public b2Vec2 Position
        {
            get { return (m_p); }
            set { m_p = value; }
        }

        public virtual int GetSupport(b2Vec2 d)
        {
            return 0;
        }

        public virtual b2Vec2 GetSupportVertex(b2Vec2 d)
        {
            return m_p;
        }

        public virtual b2Vec2 GetVertex(int index)
        {
            return m_p;
        }
        /// Get the vertex count.
        public override int GetChildCount()
        {
            return 1;
        }

        public b2CircleShape(b2CircleShape copy)
            : base(copy)
        {
            m_p = copy.m_p;
        }

        public override b2Shape Clone()
        {
            b2CircleShape clone = new b2CircleShape(this);
            return clone;
        }

        public override bool TestPoint(b2Transform transform, b2Vec2 p)
        {
            b2Vec2 tx = b2Math.b2Mul(transform.q, m_p);
            b2Vec2 center;
            center.x = transform.p.x + tx.x;
            center.y = transform.p.y + tx.y;
//            b2Vec2 center = transform.p + tx;
            b2Vec2 d; // = p - center;
            d.x = p.x - center.x;
            d.y = p.y - center.y;
            return d.LengthSquared <= m_radius * m_radius;
        }

        // Collision Detection in Interactive 3D Environments by Gino van den Bergen
        // From Section 3.1.2
        // x = s + a * r
        // norm(x) = radius
        public override bool RayCast(out b2RayCastOutput output, b2RayCastInput input,
                                    b2Transform transform, int childIndex)
        {
            output = b2RayCastOutput.Zero;

            b2Vec2 tx = b2Math.b2Mul(transform.q, m_p);
//            b2Vec2 position = transform.p + tx;
            b2Vec2 position;
            position.x = transform.p.x + tx.x;
            position.y = transform.p.y + tx.y;
//            b2Vec2 s = input.p1 - position;
            b2Vec2 s;
            s.x = input.p1.x - position.x;
            s.y = input.p1.y - position.y;
            float b = s.LengthSquared - m_radius * m_radius;

            // Solve quadratic equation.
            b2Vec2 r;
            r.x = input.p2.x - input.p1.x;
            r.y = input.p2.y - input.p1.y;
//            b2Vec2 r = input.p2 - input.p1;
            float c = s.x * r.x + s.y * r.y; // b2Math.b2Dot(ref s, ref r);
            float rr = r.LengthSquared; //  b2Math.b2Dot(r, r);
            float sigma = c * c - rr * b;

            // Check for negative discriminant and short segment.
            if (sigma < 0.0f || rr < b2Settings.b2_epsilon)
            {
                return false;
            }

            // Find the point of intersection of the line with the circle.
            float a = -(c + b2Math.b2Sqrt(sigma));

            // Is the intersection point on the segment?
            if (0.0f <= a && a <= input.maxFraction * rr)
            {
                a /= rr;
                output.fraction = a;
                output.normal.x = s.x + a * r.x;
                output.normal.y = s.y + a * r.y;
//                output.normal = s + a * r;
                output.normal.Normalize();
                return true;
            }

            return false;
        }

        public override b2AABB ComputeAABB(b2Transform transform, int childIndex)
        {
            b2Vec2 p = transform.p + b2Math.b2Mul(transform.q, m_p);
            b2AABB aabb = b2AABB.Default;
            aabb.SetLowerBound(p.x - m_radius, p.y - m_radius);
            aabb.SetUpperBound(p.x + m_radius, p.y + m_radius);
            aabb.UpdateAttributes();
            return (aabb);
        }

        public override b2MassData ComputeMass(float density)
        {
            b2MassData massData = new b2MassData();
            massData.mass = density * (float)Math.PI * m_radius * m_radius;
            massData.center = m_p;

            // inertia about the local origin
            massData.I = massData.mass * (0.5f * m_radius * m_radius + m_p.LengthSquared);
            return (massData);
        }
    }
}
