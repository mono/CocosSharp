using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;

namespace Box2D.Collision.Shapes
{
    public class b2ChainShape : b2Shape
    {
        /// The vertices. Owned by this class.
        public b2Vec2[] Vertices;

        /// The vertex count.
        public int Count;

        public b2Vec2 PrevVertex = b2Vec2.Zero;

        public b2Vec2 NextVertex = b2Vec2.Zero;
        public bool HasPrevVertex, HasNextVertex;

        public b2ChainShape()
        {
            ShapeType = b2ShapeType.e_chain;
            Radius = b2Settings.b2_polygonRadius;
            Vertices = null;
            Count = 0;
            HasPrevVertex = false;
            HasNextVertex = false;
        }


        public virtual void CreateLoop(b2Vec2[] vertices, int count)
        {
            Count = count + 1;
            Vertices = new b2Vec2[Count];
            Array.Copy(vertices, Vertices, count);
            Vertices[count] = Vertices[0];
            PrevVertex = Vertices[Count - 2];
            NextVertex = Vertices[1];
            HasPrevVertex = true;
            HasNextVertex = true;
        }

        public virtual void CreateChain(b2Vec2[] vertices, int count)
        {
            Count = count;
            Vertices = new b2Vec2[count];
            Array.Copy(vertices, Vertices, count);
            HasPrevVertex = false;
            HasNextVertex = false;
        }

        public virtual void SetPrevVertex(b2Vec2 prevVertex)
        {
            PrevVertex = prevVertex;
            HasPrevVertex = true;
        }

        public virtual void SetNextVertex(b2Vec2 nextVertex)
        {
            NextVertex = nextVertex;
            HasNextVertex = true;
        }

        public b2ChainShape(b2ChainShape clone)
            : base((b2Shape)clone)
        {
            CreateChain(clone.Vertices, clone.Count);
            PrevVertex = clone.PrevVertex;
            NextVertex = clone.NextVertex;
            HasPrevVertex = clone.HasPrevVertex;
            HasNextVertex = clone.HasNextVertex;
        }

        public override b2Shape Clone()
        {
            b2ChainShape clone = new b2ChainShape(this);
            return clone;
        }

        public override int GetChildCount()
        {
            // edge count = vertex count - 1
            return Count - 1;
        }

        public virtual b2EdgeShape GetChildEdge(int index)
        {
            b2EdgeShape edge = new b2EdgeShape();
            edge.ShapeType = b2ShapeType.e_edge;
            edge.Radius = Radius;

            edge.Vertex1 = Vertices[index + 0];
            edge.Vertex2 = Vertices[index + 1];

            if (index > 0)
            {
                edge.Vertex0 = Vertices[index - 1];
                edge.HasVertex0 = true;
            }
            else
            {
                edge.Vertex0 = PrevVertex;
                edge.HasVertex0 = HasPrevVertex;
            }

            if (index < Count - 2)
            {
                edge.Vertex3 = Vertices[index + 2];
                edge.HasVertex3 = true;
            }
            else
            {
                edge.Vertex3 = NextVertex;
                edge.HasVertex3 = HasNextVertex;
            }
            return (edge);
        }

        public override bool TestPoint(ref b2Transform xf, b2Vec2 p)
        {
            return false;
        }

        public override bool RayCast(out b2RayCastOutput output, b2RayCastInput input, ref b2Transform xf, int childIndex)
        {
            b2EdgeShape edgeShape = new b2EdgeShape();
            output = b2RayCastOutput.Zero;

            int i1 = childIndex;
            int i2 = childIndex + 1;
            if (i2 == Count)
            {
                i2 = 0;
            }

            edgeShape.Vertex1 = Vertices[i1];
            edgeShape.Vertex2 = Vertices[i2];

            b2RayCastOutput co = b2RayCastOutput.Zero;
            bool b = edgeShape.RayCast(out co, input, ref xf, 0);
            output = co;
            return (b);
        }

        public override void ComputeAABB(out b2AABB output, ref b2Transform xf, int childIndex)
        {
            int i1 = childIndex;
            int i2 = childIndex + 1;
            if (i2 == Count)
            {
                i2 = 0;
            }

            b2Vec2 v1 = b2Math.b2Mul(ref xf, ref Vertices[i1]);
            b2Vec2 v2 = b2Math.b2Mul(ref xf, ref Vertices[i2]);

            b2Math.b2Min(ref v1, ref v2, out output.LowerBound);
            b2Math.b2Max(ref v1, ref v2, out output.UpperBound);
        }

        public override b2MassData ComputeMass(float density)
        {
            b2MassData massData = b2MassData.Default;
            massData.mass = 0.0f;
            massData.center.SetZero();
            massData.I = 0.0f;
            return (massData);
        }

    }
}
