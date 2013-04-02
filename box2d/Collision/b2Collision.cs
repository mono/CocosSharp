using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;
using Box2D.Collision.Shapes;

namespace Box2D.Collision
{

    /// This is used for determining the state of contact points.
    public enum b2PointState
    {
        b2_nullState,        ///< point does not exist
        b2_addState,        ///< point was added in the update
        b2_persistState,    ///< point persisted across the update
        b2_removeState        ///< point was removed in the update
    }

    /// Used for computing contact manifolds.
    public struct b2ClipVertex
    {
        public b2Vec2 v;
        public b2ContactFeature id;
    };

    /// Ray-cast input data. The ray extends from p1 to p1 + maxFraction * (p2 - p1).
    public struct b2RayCastInput
    {
        public b2Vec2 p1, p2;
        public float maxFraction;
    };

    /// Ray-cast output data. The ray hits at p1 + fraction * (p2 - p1), where p1 and p2
    /// come from b2RayCastInput.
    public struct b2RayCastOutput
    {
        public b2Vec2 normal;
        public float fraction;

        public b2RayCastOutput(b2Vec2 b, float f)
        {
            normal = b;
            fraction = f;
        }
        public static b2RayCastOutput Zero = new b2RayCastOutput(new b2Vec2(), 0f);
    }

    public abstract class b2Collision
    {
        public static byte b2_nullFeature = byte.MaxValue;

        /// Compute the point states given two manifolds. The states pertain to the transition from manifold1
        /// to manifold2. So state1 is either persist or remove while state2 is either add or persist.
        public static void b2GetPointStates(b2PointState[] state1, b2PointState[] state2,
                              b2Manifold manifold1, b2Manifold manifold2)
        {
            for (int i = 0; i < b2Settings.b2_maxManifoldPoints; ++i)
            {
                state1[i] = b2PointState.b2_nullState;
                state2[i] = b2PointState.b2_nullState;
            }

            // Detect persists and removes.
            for (int i = 0; i < manifold1.pointCount; ++i)
            {
                b2ContactFeature id = manifold1.points[i].id;

                state1[i] = b2PointState.b2_removeState;

                for (int j = 0; j < manifold2.pointCount; ++j)
                {
                    if (manifold2.points[j].id.Equals(id))
                    {
                        state1[i] = b2PointState.b2_persistState;
                        break;
                    }
                }
            }

            // Detect persists and adds.
            for (int i = 0; i < manifold2.pointCount; ++i)
            {
                b2ContactFeature id = manifold2.points[i].id;

                state2[i] = b2PointState.b2_addState;

                for (int j = 0; j < manifold1.pointCount; ++j)
                {
                    if (manifold1.points[j].id.Equals(id))
                    {
                        state2[i] = b2PointState.b2_persistState;
                        break;
                    }
                }
            }
        }

        /// Compute the collision manifold between two circles.
        public static void b2CollideCircles(b2Manifold manifold,
                               b2CircleShape circleA, b2Transform xfA,
                               b2CircleShape circleB, b2Transform xfB)
        {
            manifold.pointCount = 0;

            b2Vec2 pA = b2Math.b2Mul(xfA, circleA.Position);
            b2Vec2 pB = b2Math.b2Mul(xfB, circleB.Position);

            b2Vec2 d = pB - pA;
            float distSqr = b2Math.b2Dot(d, d);
            float rA = circleA.Radius, rB = circleB.Radius;
            float radius = rA + rB;
            if (distSqr > radius * radius)
            {
                return;
            }

            manifold.type = b2ManifoldType.e_circles;
            manifold.localPoint = circleA.Position;
            manifold.localNormal.SetZero();
            manifold.pointCount = 1;

            manifold.points[0].localPoint = circleB.Position;
            manifold.points[0].id = b2ContactFeature.Zero;
        }

        /// Compute the collision manifold between a polygon and a circle.
        public static void b2CollidePolygonAndCircle(b2Manifold manifold,
                                        b2PolygonShape polygonA, b2Transform xfA,
                                        b2CircleShape circleB, b2Transform xfB)
        {
            manifold.pointCount = 0;

            // Compute circle position in the frame of the polygon.
            b2Vec2 c = b2Math.b2Mul(xfB, circleB.Position);
            b2Vec2 cLocal = b2Math.b2MulT(xfA, c);

            // Find the min separating edge.
            int normalIndex = 0;
            float separation = -b2Settings.b2_maxFloat;
            float radius = polygonA.Radius + circleB.Radius;
            int vertexCount = polygonA.VertexCount;
            b2Vec2[] vertices = polygonA.Vertices;
            b2Vec2[] normals = polygonA.Normals;

            for (int i = 0; i < vertexCount; ++i)
            {
                float s = b2Math.b2Dot(normals[i], cLocal - vertices[i]);

                if (s > radius)
                {
                    // Early out.
                    return;
                }

                if (s > separation)
                {
                    separation = s;
                    normalIndex = i;
                }
            }

            // Vertices that subtend the incident face.
            int vertIndex1 = normalIndex;
            int vertIndex2 = vertIndex1 + 1 < vertexCount ? vertIndex1 + 1 : 0;
            b2Vec2 v1 = vertices[vertIndex1];
            b2Vec2 v2 = vertices[vertIndex2];

            // If the center is inside the polygon ...
            if (separation < b2Settings.b2_epsilon)
            {
                manifold.pointCount = 1;
                manifold.type = b2ManifoldType.e_faceA;
                manifold.localNormal = normals[normalIndex];
                manifold.localPoint = 0.5f * (v1 + v2);
                manifold.points[0].localPoint = circleB.Position;
                manifold.points[0].id = b2ContactFeature.Zero;
                return;
            }

            // Compute barycentric coordinates
            float u1 = b2Math.b2Dot(cLocal - v1, v2 - v1);
            float u2 = b2Math.b2Dot(cLocal - v2, v1 - v2);
            if (u1 <= 0.0f)
            {
                if (b2Math.b2DistanceSquared(cLocal, v1) > radius * radius)
                {
                    return;
                }

                manifold.pointCount = 1;
                manifold.type = b2ManifoldType.e_faceA;
                manifold.localNormal = cLocal - v1;
                manifold.localNormal.Normalize();
                manifold.localPoint = v1;
                manifold.points[0].localPoint = circleB.Position;
                manifold.points[0].id = b2ContactFeature.Zero;
            }
            else if (u2 <= 0.0f)
            {
                if (b2Math.b2DistanceSquared(cLocal, v2) > radius * radius)
                {
                    return;
                }

                manifold.pointCount = 1;
                manifold.type = b2ManifoldType.e_faceA;
                manifold.localNormal = cLocal - v2;
                manifold.localNormal.Normalize();
                manifold.localPoint = v2;
                manifold.points[0].localPoint = circleB.Position;
                manifold.points[0].id = b2ContactFeature.Zero;
            }
            else
            {
                b2Vec2 faceCenter = 0.5f * (v1 + v2);
                separation = b2Math.b2Dot(cLocal - faceCenter, normals[vertIndex1]);
                if (separation > radius)
                {
                    return;
                }

                manifold.pointCount = 1;
                manifold.type = b2ManifoldType.e_faceA;
                manifold.localNormal = normals[vertIndex1];
                manifold.localPoint = faceCenter;
                manifold.points[0].localPoint = circleB.m_p;
                manifold.points[0].id = b2ContactFeature.Zero;
            }
        }

        /// Compute the collision manifold between two polygons.
        public static void b2CollidePolygons(b2Manifold manifold,
                                b2PolygonShape polygonA, b2Transform xfA,
                                b2PolygonShape polygonB, b2Transform xfB)
        {
        }

        public static float b2EdgeSeparation(b2PolygonShape poly1, b2Transform xf1, int edge1,
                                      b2PolygonShape poly2, b2Transform xf2)
        {
            b2Vec2[] vertices1 = poly1.Vertices;
            b2Vec2[] normals1 = poly1.Normals;

            int count2 = poly2.GetVertexCount();
            b2Vec2[] vertices2 = poly2.Vertices;

            // Convert normal from poly1's frame into poly2's frame.
            b2Vec2 normal1World = b2Math.b2Mul(xf1.q, normals1[edge1]);
            b2Vec2 normal1 = b2Math.b2MulT(xf2.q, normal1World);

            // Find support vertex on poly2 for -normal.
            int index = 0;
            float minDot = b2Settings.b2_maxFloat;

            for (int i = 0; i < count2; ++i)
            {
                float dot = b2Math.b2Dot(vertices2[i], normal1);
                if (dot < minDot)
                {
                    minDot = dot;
                    index = i;
                }
            }

            b2Vec2 v1 = b2Math.b2Mul(xf1, vertices1[edge1]);
            b2Vec2 v2 = b2Math.b2Mul(xf2, vertices2[index]);
            float separation = b2Math.b2Dot(v2 - v1, normal1World);
            return separation;
        }

        /// Compute the collision manifold between an edge and a circle.
        public static void b2CollideEdgeAndCircle(b2Manifold manifold,
                                        b2EdgeShape polygonA, b2Transform xfA,
                                        b2CircleShape circleB, b2Transform xfB)
        {
        }

        /// Compute the collision manifold between an edge and a circle.
        public static void b2CollideEdgeAndPolygon(b2Manifold manifold,
                                        b2EdgeShape edgeA, b2Transform xfA,
                                        b2PolygonShape circleB, b2Transform xfB)
        {
        }

        /// Clipping for contact manifolds.
        public static int b2ClipSegmentToLine(b2ClipVertex[] vOut, b2ClipVertex[] vIn,
                                     b2Vec2 normal, float offset, byte vertexIndexA)
        {
            // Start with no output points
            int numOut = 0;

            // Calculate the distance of end points to the line
            float distance0 = b2Math.b2Dot(normal, vIn[0].v) - offset;
            float distance1 = b2Math.b2Dot(normal, vIn[1].v) - offset;

            // If the points are behind the plane
            if (distance0 <= 0.0f) vOut[numOut++] = vIn[0];
            if (distance1 <= 0.0f) vOut[numOut++] = vIn[1];

            // If the points are on different sides of the plane
            if (distance0 * distance1 < 0.0f)
            {
                // Find intersection point of edge and plane
                float interp = distance0 / (distance0 - distance1);
                vOut[numOut].v = vIn[0].v + interp * (vIn[1].v - vIn[0].v);

                // VertexA is hitting edgeB.
                vOut[numOut].id.indexA = vertexIndexA;
                vOut[numOut].id.indexB = vIn[0].id.indexB;
                vOut[numOut].id.typeA = b2ContactFeatureType.e_vertex;
                vOut[numOut].id.typeB = b2ContactFeatureType.e_face;
                ++numOut;
            }

            return numOut;
        }

        public static bool b2TestOverlap(b2AABB a, b2AABB b)
        {
            b2Vec2 d1, d2;
            d1 = b.lowerBound - a.upperBound;
            d2 = a.lowerBound - b.upperBound;

            if (d1.x > 0.0f || d1.y > 0.0f)
                return false;

            if (d2.x > 0.0f || d2.y > 0.0f)
                return false;

            return true;
        }

        /// Determine if two generic shapes overlap.
        public static bool b2TestOverlap(b2Shape shapeA, int indexA,
                             b2Shape shapeB, int indexB,
                            b2Transform xfA, b2Transform xfB)
        {
        }

        // Find the max separation between poly1 and poly2 using edge normals from poly1.
        public static float b2FindMaxSeparation(out int edgeIndex,
                                         b2PolygonShape poly1, b2Transform xf1,
                                         b2PolygonShape poly2, b2Transform xf2)
        {
            int count1 = poly1.GetVertexCount();
            b2Vec2[] normals1 = poly1.Normals;

            // Vector pointing from the centroid of poly1 to the centroid of poly2.
            b2Vec2 d = b2Math.b2Mul(xf2, poly2.Centroid) - b2Math.b2Mul(xf1, poly1.Centroid);
            b2Vec2 dLocal1 = b2Math.b2MulT(xf1.q, d);

            // Find edge normal on poly1 that has the largest projection onto d.
            int edge = 0;
            float maxDot = -b2Settings.b2_maxFloat;
            for (int i = 0; i < count1; ++i)
            {
                float dot = b2Math.b2Dot(normals1[i], dLocal1);
                if (dot > maxDot)
                {
                    maxDot = dot;
                    edge = i;
                }
            }

            // Get the separation for the edge normal.
            float s = b2EdgeSeparation(poly1, xf1, edge, poly2, xf2);

            // Check the separation for the previous edge normal.
            int prevEdge = edge - 1 >= 0 ? edge - 1 : count1 - 1;
            float sPrev = b2EdgeSeparation(poly1, xf1, prevEdge, poly2, xf2);

            // Check the separation for the next edge normal.
            int nextEdge = edge + 1 < count1 ? edge + 1 : 0;
            float sNext = b2EdgeSeparation(poly1, xf1, nextEdge, poly2, xf2);

            // Find the best edge and the search direction.
            int bestEdge;
            float bestSeparation;
            int increment;
            if (sPrev > s && sPrev > sNext)
            {
                increment = -1;
                bestEdge = prevEdge;
                bestSeparation = sPrev;
            }
            else if (sNext > s)
            {
                increment = 1;
                bestEdge = nextEdge;
                bestSeparation = sNext;
            }
            else
            {
                edgeIndex = edge;
                return s;
            }

            // Perform a local search for the best edge normal.
            for (; ; )
            {
                if (increment == -1)
                    edge = bestEdge - 1 >= 0 ? bestEdge - 1 : count1 - 1;
                else
                    edge = bestEdge + 1 < count1 ? bestEdge + 1 : 0;

                s = b2EdgeSeparation(poly1, xf1, edge, poly2, xf2);

                if (s > bestSeparation)
                {
                    bestEdge = edge;
                    bestSeparation = s;
                }
                else
                {
                    break;
                }
            }

            edgeIndex = bestEdge;
            return bestSeparation;
        }

        public static void b2FindIncidentEdge(b2ClipVertex[] c,
                                     b2PolygonShape poly1, b2Transform xf1, int edge1,
                                     b2PolygonShape poly2, b2Transform xf2)
        {
            b2Vec2[] normals1 = poly1.Normals;

            int count2 = poly2.GetVertexCount();
            b2Vec2[] vertices2 = poly2.Vertices;
            b2Vec2[] normals2 = poly2.Normals;

            // Get the normal of the reference edge in poly2's frame.
            b2Vec2 normal1 = b2Math.b2MulT(xf2.q, b2Math.b2Mul(xf1.q, normals1[edge1]));

            // Find the incident edge on poly2.
            int index = 0;
            float minDot = b2Settings.b2_maxFloat;
            for (int i = 0; i < count2; ++i)
            {
                float dot = b2Math.b2Dot(normal1, normals2[i]);
                if (dot < minDot)
                {
                    minDot = dot;
                    index = i;
                }
            }

            // Build the clip vertices for the incident edge.
            int i1 = index;
            int i2 = i1 + 1 < count2 ? i1 + 1 : 0;

            c[0].v = b2Math.b2Mul(xf2, vertices2[i1]);
            c[0].id.indexA = (byte)edge1;
            c[0].id.indexB = (byte)i1;
            c[0].id.typeA = b2ContactFeatureType.e_face;
            c[0].id.typeB = b2ContactFeatureType.e_vertex;

            c[1].v = b2Math.b2Mul(xf2, vertices2[i2]);
            c[1].id.indexA = (byte)edge1;
            c[1].id.indexB = (byte)i2;
            c[1].id.typeA = b2ContactFeatureType.e_face;
            c[1].id.typeB = b2ContactFeatureType.e_vertex;
        }
    }
}
