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
            if (manifold.pointCount == 0)
            {
                normal = b2Vec2.Zero;
                return;
            }

            switch (manifold.type)
            {
                case b2ManifoldType.e_circles:
                {
#if false                    
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
#else                    
                    normal.x = 1.0f;
                    normal.y = 0.0f;

                    var localPoint = manifold.points[0].localPoint;

                    float pointAx = (xfA.q.c * manifold.localPoint.x - xfA.q.s * manifold.localPoint.y) + xfA.p.x;
                    float pointAy = (xfA.q.s * manifold.localPoint.x + xfA.q.c * manifold.localPoint.y) + xfA.p.y;

                    float pointBx = (xfB.q.c * localPoint.x - xfB.q.s * localPoint.y) + xfB.p.x;
                    float pointBy = (xfB.q.s * localPoint.x + xfB.q.c * localPoint.y) + xfB.p.y;

                    float cx = pointAx - pointBx;
                    float cy = pointAy - pointBy;

                    float distance = (cx * cx + cy * cy);

                    if (distance > b2Settings.b2_epsilonSqrd)
                    {
                        normal.x = pointBx - pointAx;
                        normal.y = pointBy - pointAy;
                        normal.Normalize();
                    }

                    float cAx = pointAx + radiusA * normal.x;
                    float cAy = pointAy + radiusA * normal.y;
                    float cBx = pointBx - radiusB * normal.x;
                    float cBy = pointBy - radiusB * normal.y;

                    b2Vec2 p;
                    p.x = 0.5f * (cAx + cBx);
                    p.y = 0.5f * (cAy + cBy);

                    points[0] = p;
#endif
                }
                    break;

                case b2ManifoldType.e_faceA:
                {
#if false
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
#else
                    float normalx = xfA.q.c * manifold.localNormal.x - xfA.q.s * manifold.localNormal.y;
                    float normaly = xfA.q.s * manifold.localNormal.x + xfA.q.c * manifold.localNormal.y;

                    normal.x = normalx;
                    normal.y = normaly;

                    float planePointx = (xfA.q.c * manifold.localPoint.x - xfA.q.s * manifold.localPoint.y) + xfA.p.x;
                    float planePointy = (xfA.q.s * manifold.localPoint.x + xfA.q.c * manifold.localPoint.y) + xfA.p.y;

                    for (int i = 0; i < manifold.pointCount; ++i)
                    {
                        var localPoint = manifold.points[i].localPoint;

                        float clipPointx = (xfB.q.c * localPoint.x - xfB.q.s * localPoint.y) + xfB.p.x;
                        float clipPointy = (xfB.q.s * localPoint.x + xfB.q.c * localPoint.y) + xfB.p.y;

                        float clipMinusPlanex = clipPointx - planePointx;
                        float clipMinusPlaney = clipPointy - planePointy;

                        float d = clipMinusPlanex * normalx + clipMinusPlaney * normaly;

                        float cAx = clipPointx + (radiusA - d) * normalx;
                        float cAy = clipPointy + (radiusA - d) * normaly;

                        float cBx = clipPointx - radiusB * normalx;
                        float cBy = clipPointy - radiusB * normaly;

                        b2Vec2 p;
                        p.x = 0.5f * (cAx + cBx);
                        p.y = 0.5f * (cAy + cBy);

                        points[i] = p;
                    }
#endif
                }
                break;

                case b2ManifoldType.e_faceB:
                {
#if false
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
#else
                    float normalx = xfB.q.c * manifold.localNormal.x - xfB.q.s * manifold.localNormal.y;
                    float normaly = xfB.q.s * manifold.localNormal.x + xfB.q.c * manifold.localNormal.y;

                    float planePointx = (xfB.q.c * manifold.localPoint.x - xfB.q.s * manifold.localPoint.y) + xfB.p.x;
                    float planePointy = (xfB.q.s * manifold.localPoint.x + xfB.q.c * manifold.localPoint.y) + xfB.p.y;

                    for (int i = 0; i < manifold.pointCount; ++i)
                    {
                        var localPoint = manifold.points[i].localPoint;

                        float clipPointx = (xfA.q.c * localPoint.x - xfA.q.s * localPoint.y) + xfA.p.x;
                        float clipPointy = (xfA.q.s * localPoint.x + xfA.q.c * localPoint.y) + xfA.p.y;

                        float distx = clipPointx - planePointx;
                        float disty = clipPointy - planePointy;

                        var d = (distx * normalx + disty * normaly);

                        float cBx = clipPointx + (radiusB - d) * normalx;
                        float cBy = clipPointy + (radiusB - d) * normaly;

                        float cAx = clipPointx - radiusA * normalx;
                        float cAy = clipPointy - radiusA * normaly;

                        b2Vec2 p;
                        p.x = 0.5f * (cAx + cBx);
                        p.y = 0.5f * (cAy + cBy);

                        points[i] = p;
                    }

                    // Ensure normal points from A to B.
                    normal.x = -normalx;
                    normal.y = -normaly;
#endif
                }
                    break;
            }
        }

        public b2Vec2 normal; //< world vector pointing from A to B
        public b2Vec2[] points = new b2Vec2[b2Settings.b2_maxManifoldPoints]; //< world contact point (point of intersection)
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
