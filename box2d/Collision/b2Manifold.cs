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
        public b2Vec2 localPoint;       //< usage depends on manifold type
        public float normalImpulse;     //< the non-penetration impulse
        public float tangentImpulse;    //< the friction impulse
        public b2ContactFeature id;     //< uniquely identifies a contact point between two shapes
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

    // This is used to compute the current state of a contact manifold.
    public class b2WorldManifold
    {
        // Evaluate the manifold with supplied transforms. This assumes
        // modest motion from the original state. This does not change the
        // point count, impulses, etc. The radii must come from the shapes
        // that generated the manifold.
        public void Initialize(ref b2Manifold manifold,
                        b2Transform xfA, float radiusA,
                        b2Transform xfB, float radiusB)
        {
            //points = new b2Vec2[b2Settings.b2_maxManifoldPoints];
            
            //for (int p = 0; p < b2Settings.b2_maxManifoldPoints; p++)
            //    points[p] = b2Vec2.Zero;

            normal = b2Vec2.Zero;

            if (manifold.pointCount == 0)
            {
                return;
            }
            
            switch (manifold.type)
            {
            case b2ManifoldType.e_circles:
            {
                normal.Set(1.0f, 0.0f);
                b2Vec2 pointA = b2Math.b2Mul(ref xfA, ref manifold.localPoint);
                b2Vec2 pointB = b2Math.b2Mul(ref xfB, ref manifold.points[0].localPoint);
                if (b2Math.b2DistanceSquared(pointA, pointB) > b2Settings.b2_epsilonSqrd)
                {
                    normal = pointB - pointA;
                    normal.Normalize();
                }
                
                b2Vec2 cA = pointA + radiusA * normal;
                b2Vec2 cB = pointB - radiusB * normal;
                points[0] = 0.5f * (cA + cB);
            }
                break;
                
            case b2ManifoldType.e_faceA:
            {
                normal = b2Math.b2Mul(xfA.q, manifold.localNormal);
                b2Vec2 planePoint = b2Math.b2Mul(ref xfA, ref manifold.localPoint);
                
                for (int i = 0; i < manifold.pointCount; ++i)
                {
                    b2Vec2 clipPoint = b2Math.b2Mul(ref xfB, ref manifold.points[i].localPoint);
                    b2Vec2 clipMinusPlane = clipPoint - planePoint;
                    b2Vec2 cA = clipPoint + (radiusA - b2Math.b2Dot(ref clipMinusPlane, ref normal)) * normal;
                    b2Vec2 cB = clipPoint - radiusB * normal;
                    points[i] = 0.5f * (cA + cB);
                }
            }
                break;
                
            case b2ManifoldType.e_faceB:
            {
                normal = b2Math.b2Mul(ref xfB.q, ref manifold.localNormal);
                b2Vec2 planePoint = b2Math.b2Mul(ref xfB, ref manifold.localPoint);
                
                for (int i = 0; i < manifold.pointCount; ++i)
                {
                    b2Vec2 clipPoint = b2Math.b2Mul(ref xfA, ref manifold.points[i].localPoint);
                    b2Vec2 tmp = b2Vec2.Zero;
                    tmp.x = clipPoint.x - planePoint.x;
                    tmp.y = clipPoint.y - planePoint.y;
                    // b2Vec2 cB = clipPoint + (radiusB - b2Math.b2Dot(clipPoint - planePoint, normal)) * normal; 
                    b2Vec2 cB = clipPoint + (radiusB - b2Math.b2Dot(ref tmp, ref normal)) * normal;
                    b2Vec2 cA = clipPoint - radiusA * normal;
                    points[i] = 0.5f * (cA + cB);
                }
                
                // Ensure normal points from A to B.
                normal = -normal;
            }
                break;
            }
        }

        public b2Vec2 normal;      //< world vector pointing from A to B
        public b2Vec2[] points = new b2Vec2[b2Settings.b2_maxManifoldPoints];    //< world contact point (point of intersection)
    }

    public struct b2Manifold
    {
        
        public static b2Manifold Create()
        {
            b2Manifold m = new b2Manifold();
            m.points =  new b2ManifoldPoint[b2Settings.b2_maxManifoldPoints];
            return (m);
        }
        
        
        public void CopyPointsFrom(ref b2Manifold other)
        {
            Array.Copy(other.points, points, b2Settings.b2_maxManifoldPoints);
        }
        

        public b2ManifoldPoint[] points;//  = new b2ManifoldPoint[b2Settings.b2_maxManifoldPoints];    //< the points of contact
        public b2Vec2 localNormal;                                //< not use for Type::e_points
        public b2Vec2 localPoint;                                //< usage depends on manifold type
        public b2ManifoldType type;
        public int pointCount;                                //< the number of manifold points
    }

}
