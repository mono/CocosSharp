using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;

namespace Box2D.Collision
{
    /// A manifold point is a contact point belonging to a contact
    /// manifold. It holds details related to the geometry and dynamics
    /// of the contact points.
    /// The local point usage depends on the manifold type:
    /// -e_circles: the local center of circleB
    /// -e_faceA: the local center of cirlceB or the clip point of polygonB
    /// -e_faceB: the clip point of polygonA
    /// This structure is stored across time steps, so we keep it small.
    /// Note: the impulses are used for internal caching and may not
    /// provide reliable contact forces, especially for high speed collisions.
    public struct b2ManifoldPoint
    {
        public b2Vec2 localPoint;       ///< usage depends on manifold type
        public float normalImpulse;     ///< the non-penetration impulse
        public float tangentImpulse;    ///< the friction impulse
        public b2ContactFeature id;     ///< uniquely identifies a contact point between two shapes
    }

    /// A manifold for two touching convex shapes.
    /// Box2D supports multiple types of contact:
    /// - clip point versus plane with radius
    /// - point versus point with radius (circles)
    /// The local point usage depends on the manifold type:
    /// -e_circles: the local center of circleA
    /// -e_faceA: the center of faceA
    /// -e_faceB: the center of faceB
    /// Similarly the local normal usage:
    /// -e_circles: not used
    /// -e_faceA: the normal on polygonA
    /// -e_faceB: the normal on polygonB
    /// We store contacts in this way so that position correction can
    /// account for movement, which is critical for continuous physics.
    /// All contact scenarios must be expressed in one of these types.
    /// This structure is stored across time steps, so we keep it small.
    public enum b2ManifoldType
    {
        e_circles,
        e_faceA,
        e_faceB
    }

    /// This is used to compute the current state of a contact manifold.
    public struct b2WorldManifold
    {
        /// Evaluate the manifold with supplied transforms. This assumes
        /// modest motion from the original state. This does not change the
        /// point count, impulses, etc. The radii must come from the shapes
        /// that generated the manifold.
        public void Initialize(b2Manifold manifold,
                        b2Transform xfA, float radiusA,
                        b2Transform xfB, float radiusB)
        {
            points = new b2Vec2[b2Settings.b2_maxManifoldPoints];
        }

        public b2Vec2 normal;      ///< world vector pointing from A to B
        public b2Vec2[] points;    ///< world contact point (point of intersection)
    }

    public class b2Manifold
    {
        public b2ManifoldPoint[] points = new b2ManifoldPoint[b2Settings.b2_maxManifoldPoints];    ///< the points of contact
        public b2Vec2 localNormal;                                ///< not use for Type::e_points
        public b2Vec2 localPoint;                                ///< usage depends on manifold type
        public b2ManifoldType type;
        public int pointCount;                                ///< the number of manifold points
    }

}
