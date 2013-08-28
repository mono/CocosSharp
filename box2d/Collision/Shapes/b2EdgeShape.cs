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
        public b2Vec2 Vertex0 = new b2Vec2();
        public b2Vec2 Vertex3 = new b2Vec2();
        public bool HasVertex0, HasVertex3;
        /// These are the edge vertices
        public b2Vec2 Vertex1 = new b2Vec2();
        public b2Vec2 Vertex2 = new b2Vec2();

        public b2EdgeShape()
        {
            ShapeType = b2ShapeType.e_edge;
            Radius = b2Settings.b2_polygonRadius;
            Vertex0.x = 0.0f;
            Vertex0.y = 0.0f;
            Vertex3.x = 0.0f;
            Vertex3.y = 0.0f;
            HasVertex0 = false;
            HasVertex3 = false;
        }

        public b2EdgeShape(b2EdgeShape e)
            : base((b2Shape)e)
        {
            Vertex1 = e.Vertex1;
            Vertex2 = e.Vertex2;
            Vertex3 = e.Vertex3;
            Vertex0 = e.Vertex0;
            HasVertex0 = e.HasVertex0;
            HasVertex3 = e.HasVertex3;
        }

        public virtual void Set(b2Vec2 v1, b2Vec2 v2)
        {
            Vertex1 = v1;
            Vertex2 = v2;
            HasVertex0 = false;
            HasVertex3 = false;
        }

        public override b2Shape Clone()
        {
            b2EdgeShape clone = new b2EdgeShape(this);
            return clone;
        }

        public override int GetChildCount()
        {
            return 1;
        }

        public override bool TestPoint(ref b2Transform xf, b2Vec2 p)
        {
            return false;
        }

        // p = p1 + t * d
        // v = v1 + s * e
        // p1 + t * d = v1 + s * e
        // s * e - t * d = p1 - v1
        public override bool RayCast(out b2RayCastOutput output, b2RayCastInput input, ref b2Transform xf, int childIndex)
        {
            output = b2RayCastOutput.Zero;

            // Put the ray into the edge's frame of reference.
            b2Vec2 p1 = b2Math.b2MulT(xf.q, input.p1 - xf.p);
            b2Vec2 p2 = b2Math.b2MulT(xf.q, input.p2 - xf.p);
            b2Vec2 d = p2 - p1;

            b2Vec2 v1 = Vertex1;
            b2Vec2 v2 = Vertex2;
            b2Vec2 e = v2 - v1;
            b2Vec2 normal = b2Vec2.Zero; // new b2Vec2(e.y, -e.x);
            normal.x = e.y;
            normal.y = -e.x;
            normal.Normalize();

            // q = p1 + t * d
            // dot(normal, q - v1) = 0
            // dot(normal, p1 - v1) + t * dot(normal, d) = 0
            b2Vec2 diff = v1 - p1;
            float numerator = b2Math.b2Dot(ref normal, ref diff);
            float denominator = b2Math.b2Dot(ref normal, ref d);

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
            float rr = r.LengthSquared; // b2Math.b2Dot(r, r);
            if (rr == 0.0f)
            {
                return false;
            }

            diff = q - v1;
            float s = b2Math.b2Dot(ref diff, ref r) / rr;
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

        public override void ComputeAABB(out b2AABB output, ref b2Transform xf, int childIndex)
        {
            b2Vec2 v1;
            v1.x = (xf.q.c * Vertex1.x - xf.q.s * Vertex1.y) + xf.p.x;
            v1.y = (xf.q.s * Vertex1.x + xf.q.c * Vertex1.y) + xf.p.y;

            b2Vec2 v2;
            v2.x = (xf.q.c * Vertex2.x - xf.q.s * Vertex2.y) + xf.p.x;
            v2.y = (xf.q.s * Vertex2.x + xf.q.c * Vertex2.y) + xf.p.y;

            b2Vec2 lower;
            lower.x = v1.x < v2.x ? v1.x : v2.x;
            lower.y = v1.y < v2.y ? v1.y : v2.y;
            
            //b2Math.b2Min(v1, v2);
            b2Vec2 upper;
            upper.x = v1.x > v2.x ? v1.x : v2.x;
            upper.y = v1.y > v2.y ? v1.y : v2.y; 
            // = b2Math.b2Max(v1, v2);

            output.LowerBound = lower;
            output.UpperBound = upper;
            output.Fatten(Radius);
        }

        public override b2MassData ComputeMass(float density)
        {
            b2MassData massData = b2MassData.Default;
            massData.mass = 0.0f;
            massData.center = 0.5f * (Vertex1 + Vertex2);
            massData.I = 0.0f;
            return (massData);
        }
    }
}
