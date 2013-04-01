using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;

namespace Box2D.Collision.Shapes
{
    public enum b2ShapeType : int
    {
        e_circle = 0,
        e_edge = 1,
        e_polygon = 2,
        e_chain = 3,
        e_typeCount = 4
    }

    /// <summary>
    /// This holds the mass data computed for a shape.
    /// </summary>
    public struct b2MassData
    {
        /// The mass of the shape, usually in kilograms.
        public float mass;

        /// The position of the shape's centroid relative to the shape's origin.
        public b2Vec2 center;

        /// The rotational inertia of the shape about the local origin.
        public float I;
    }

    /// <summary>
    /// A shape is used for collision detection. You can create a shape however you like.
    /// Shapes used for simulation in b2World are created automatically when a b2Fixture
    /// is created. Shapes may encapsulate a one or more child shapes.
    /// </summary>
    public abstract class b2Shape
    {
        public b2Shape()
        {
        }

        public virtual b2ShapeType ShapeType
        {
            get { return (m_type); }
            set { m_type = value; }
        }

        public virtual float Radius
        {
            get { return (m_radius); }
            set { m_radius = value; }
        }

        public b2Shape(b2Shape copy)
        {
            m_type = copy.m_type;
            m_radius = copy.m_radius;
        }

        /// Clone the concrete shape using the provided allocator.
        public abstract b2Shape Clone();

        /// Get the type of this shape. You can use this to down cast to the concrete shape.
        /// @return the shape type.
        public b2ShapeType GetShapeType()
        {
            return (m_type);
        }

        /// Get the number of child primitives.
        public abstract int GetChildCount();

        /// Test a point for containment in this shape. This only works for convex shapes.
        /// @param xf the shape world transform.
        /// @param p a point in world coordinates.
        public abstract bool TestPoint(b2Transform xf, b2Vec2 p);

        /// Cast a ray against a child shape.
        /// @param output the ray-cast results.
        /// @param input the ray-cast input parameters.
        /// @param transform the transform to be applied to the shape.
        /// @param childIndex the child shape index
        public abstract bool RayCast(out b2RayCastOutput output, b2RayCastInput input,
                            b2Transform transform, int childIndex);

        /// Given a transform, compute the associated axis aligned bounding box for a child shape.
        /// @param aabb returns the axis aligned box.
        /// @param xf the world transform of the shape.
        /// @param childIndex the child shape
        public abstract b2AABB ComputeAABB(b2Transform xf, int childIndex);

        /// Compute the mass properties of this shape using its dimensions and density.
        /// The inertia tensor is computed about the local origin.
        /// @param massData returns the mass data for this shape.
        /// @param density the density in kilograms per meter squared.
        public abstract b2MassData ComputeMass(float density);

        protected b2ShapeType m_type;
        protected float m_radius;
    }
}
