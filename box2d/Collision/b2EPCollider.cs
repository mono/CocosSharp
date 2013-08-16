using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision.Shapes;
using Box2D.Common;

namespace Box2D.Collision
{
    public enum b2EPAxisType
    {
        e_unknown,
        e_edgeA,
        e_edgeB
    }

    // This structure is used to keep track of the best separating axis.
    public struct b2EPAxis
    {
        public b2EPAxisType type;
        public int index;
        public float separation;
    }

    // This holds polygon B expressed in frame A.
    public struct b2TempPolygon
    {
        public static b2TempPolygon Create()
        {
            b2TempPolygon bt;
            bt.vertices = new b2Vec2[b2Settings.b2_maxPolygonVertices];
            bt.normals = new b2Vec2[b2Settings.b2_maxPolygonVertices];
            bt.count = 0;
            return (bt);
        }
        public b2Vec2[] vertices;
        public b2Vec2[] normals;
        public int count;
    };

    // Reference face used for clipping
    public struct b2ReferenceFace
    {
        public int i1, i2;

        public b2Vec2 v1, v2;

        public b2Vec2 normal;

        public b2Vec2 sideNormal1;
        public float sideOffset1;

        public b2Vec2 sideNormal2;
        public float sideOffset2;
    }

    public class b2EPCollider
    {
        // Algorithm:
        // 1. Classify v1 and v2
        // 2. Classify polygon centroid as front or back
        // 3. Flip normal if necessary
        // 4. Initialize normal range to [-pi, pi] about face normal
        // 5. Adjust normal range according to adjacent edges
        // 6. Visit each separating axes, only accept axes within the range
        // 7. Return if _any_ axis indicates separation
        // 8. Clip
        public void Collide(ref b2Manifold manifold, b2EdgeShape edgeA, ref b2Transform xfA,
                                   b2PolygonShape polygonB, ref b2Transform xfB)
        {
            m_xf = b2Math.b2MulT(xfA, xfB);

            m_centroidB = b2Math.b2Mul(m_xf, polygonB.Centroid);

            m_v0 = edgeA.Vertex0;
            m_v1 = edgeA.Vertex1;
            m_v2 = edgeA.Vertex2;
            m_v3 = edgeA.Vertex3;

            bool hasVertex0 = edgeA.HasVertex0;
            bool hasVertex3 = edgeA.HasVertex3;

            b2Vec2 edge1;// = m_v2 - m_v1;
            edge1.x = m_v2.x - m_v1.x;
            edge1.y = m_v2.y - m_v1.y;
            edge1.Normalize();
            m_normal1.Set(edge1.y, -edge1.x);
            b2Vec2 cenMinusV1;// = m_centroidB - m_v1;
            cenMinusV1.x = m_centroidB.x - m_v1.x;
            cenMinusV1.y = m_centroidB.y - m_v1.y;
            float offset1 = m_normal1.x * cenMinusV1.x + m_normal1.y * cenMinusV1.y;// b2Math.b2Dot(ref m_normal1, ref cenMinusV1);
            float offset0 = 0.0f, offset2 = 0.0f;
            bool convex1 = false, convex2 = false;

            // Is there a preceding edge?
            if (hasVertex0)
            {
                b2Vec2 edge0;// = m_v1 - m_v0;
                edge0.x = m_v1.x - m_v0.x;
                edge0.y = m_v1.y - m_v0.y;
                edge0.Normalize();
                m_normal0.Set(edge0.y, -edge0.x);
                convex1 = b2Math.b2Cross(ref edge0, ref edge1) >= 0.0f;
                b2Vec2 cenMinusV0;// = m_centroidB - m_v0;
                cenMinusV0.x = m_centroidB.x - m_v0.x;
                cenMinusV0.y = m_centroidB.y - m_v0.y;
                offset0 = m_normal0.x * cenMinusV0.x + m_normal0.y * cenMinusV0.y; // b2Math.b2Dot(ref m_normal0, ref cenMinusV0);
            }

            // Is there a following edge?
            if (hasVertex3)
            {
                b2Vec2 edge2;// = m_v3 - m_v2;
                edge2.x = m_v3.x - m_v2.x;
                edge2.y = m_v3.y - m_v2.y;
                edge2.Normalize();
                m_normal2.Set(edge2.y, -edge2.x);
                convex2 = b2Math.b2Cross(ref edge1, ref edge2) > 0.0f;
                b2Vec2 tmp;
                tmp.x = m_centroidB.x - m_v2.x;
                tmp.y = m_centroidB.y - m_v2.y;
                offset2 = m_normal2.x * tmp.x + m_normal2.y * tmp.y;// b2Math.b2Dot(m_normal2, m_centroidB - m_v2);
            }

            // Determine front or back collision. Determine collision normal limits.
            if (hasVertex0 && hasVertex3)
            {
                if (convex1 && convex2)
                {
                    m_front = offset0 >= 0.0f || offset1 >= 0.0f || offset2 >= 0.0f;
                    if (m_front)
                    {
                        m_normal = m_normal1;
                        m_lowerLimit = m_normal0;
                        m_upperLimit = m_normal2;
                    }
                    else
                    {
                        m_normal = -m_normal1;
                        m_lowerLimit = -m_normal1;
                        m_upperLimit = -m_normal1;
                    }
                }
                else if (convex1)
                {
                    m_front = offset0 >= 0.0f || (offset1 >= 0.0f && offset2 >= 0.0f);
                    if (m_front)
                    {
                        m_normal = m_normal1;
                        m_lowerLimit = m_normal0;
                        m_upperLimit = m_normal1;
                    }
                    else
                    {
                        m_normal = -m_normal1;
                        m_lowerLimit = -m_normal2;
                        m_upperLimit = -m_normal1;
                    }
                }
                else if (convex2)
                {
                    m_front = offset2 >= 0.0f || (offset0 >= 0.0f && offset1 >= 0.0f);
                    if (m_front)
                    {
                        m_normal = m_normal1;
                        m_lowerLimit = m_normal1;
                        m_upperLimit = m_normal2;
                    }
                    else
                    {
                        m_normal = -m_normal1;
                        m_lowerLimit = -m_normal1;
                        m_upperLimit = -m_normal0;
                    }
                }
                else
                {
                    m_front = offset0 >= 0.0f && offset1 >= 0.0f && offset2 >= 0.0f;
                    if (m_front)
                    {
                        m_normal = m_normal1;
                        m_lowerLimit = m_normal1;
                        m_upperLimit = m_normal1;
                    }
                    else
                    {
                        m_normal = -m_normal1;
                        m_lowerLimit = -m_normal2;
                        m_upperLimit = -m_normal0;
                    }
                }
            }
            else if (hasVertex0)
            {
                if (convex1)
                {
                    m_front = offset0 >= 0.0f || offset1 >= 0.0f;
                    if (m_front)
                    {
                        m_normal = m_normal1;
                        m_lowerLimit = m_normal0;
                        m_upperLimit = -m_normal1;
                    }
                    else
                    {
                        m_normal = -m_normal1;
                        m_lowerLimit = m_normal1;
                        m_upperLimit = -m_normal1;
                    }
                }
                else
                {
                    m_front = offset0 >= 0.0f && offset1 >= 0.0f;
                    if (m_front)
                    {
                        m_normal = m_normal1;
                        m_lowerLimit = m_normal1;
                        m_upperLimit = -m_normal1;
                    }
                    else
                    {
                        m_normal = -m_normal1;
                        m_lowerLimit = m_normal1;
                        m_upperLimit = -m_normal0;
                    }
                }
            }
            else if (hasVertex3)
            {
                if (convex2)
                {
                    m_front = offset1 >= 0.0f || offset2 >= 0.0f;
                    if (m_front)
                    {
                        m_normal = m_normal1;
                        m_lowerLimit = -m_normal1;
                        m_upperLimit = m_normal2;
                    }
                    else
                    {
                        m_normal = -m_normal1;
                        m_lowerLimit = -m_normal1;
                        m_upperLimit = m_normal1;
                    }
                }
                else
                {
                    m_front = offset1 >= 0.0f && offset2 >= 0.0f;
                    if (m_front)
                    {
                        m_normal = m_normal1;
                        m_lowerLimit = -m_normal1;
                        m_upperLimit = m_normal1;
                    }
                    else
                    {
                        m_normal = -m_normal1;
                        m_lowerLimit = -m_normal2;
                        m_upperLimit = m_normal1;
                    }
                }
            }
            else
            {
                m_front = offset1 >= 0.0f;
                if (m_front)
                {
                    m_normal = m_normal1;
                    m_lowerLimit = -m_normal1;
                    m_upperLimit = -m_normal1;
                }
                else
                {
                    m_normal = -m_normal1;
                    m_lowerLimit = m_normal1;
                    m_upperLimit = m_normal1;
                }
            }

            // Get polygonB in frameA
            m_polygonB = b2TempPolygon.Create();
            m_polygonB.count = polygonB.VertexCount;
            for (int i = 0; i < polygonB.VertexCount; ++i)
            {
                m_polygonB.vertices[i] = b2Math.b2Mul(m_xf, polygonB.Vertices[i]);
                m_polygonB.normals[i] = b2Math.b2Mul(m_xf.q, polygonB.Normals[i]);
            }

            m_radius = 2.0f * b2Settings.b2_polygonRadius;

            manifold.pointCount = 0;

            b2EPAxis edgeAxis = ComputeEdgeSeparation();

//            Console.WriteLine("b2EPAxis: {0} {1} {2}", edgeAxis.index, edgeAxis.separation, edgeAxis.type);

            // If no valid normal can be found than this edge should not collide.
            if (edgeAxis.type == b2EPAxisType.e_unknown)
            {
                return;
            }

            if (edgeAxis.separation > m_radius)
            {
                return;
            }

            b2EPAxis polygonAxis = ComputePolygonSeparation();
            if (polygonAxis.type != b2EPAxisType.e_unknown && polygonAxis.separation > m_radius)
            {
                return;
            }

            // Use hysteresis for jitter reduction.
            const float k_relativeTol = 0.98f;
            const float k_absoluteTol = 0.001f;

            b2EPAxis primaryAxis;
            if (polygonAxis.type == b2EPAxisType.e_unknown)
            {
                primaryAxis = edgeAxis;
            }
            else if (polygonAxis.separation > k_relativeTol * edgeAxis.separation + k_absoluteTol)
            {
                primaryAxis = polygonAxis;
            }
            else
            {
                primaryAxis = edgeAxis;
            }

            b2ClipVertex[] ie = new b2ClipVertex[2];
            b2ReferenceFace rf;
            if (primaryAxis.type == b2EPAxisType.e_edgeA)
            {
                manifold.type = b2ManifoldType.e_faceA;

                // Search for the polygon normal that is most anti-parallel to the edge normal.
                int bestIndex = 0;
                float bestValue = b2Math.b2Dot(ref m_normal, ref m_polygonB.normals[0]);
                for (int i = 1; i < m_polygonB.count; ++i)
                {
                    float value = b2Math.b2Dot(ref m_normal, ref m_polygonB.normals[i]);
                    if (value < bestValue)
                    {
                        bestValue = value;
                        bestIndex = i;
                    }
                }

                int i1 = bestIndex;
                int i2 = i1 + 1 < m_polygonB.count ? i1 + 1 : 0;

                ie[0].v = m_polygonB.vertices[i1];
                ie[0].id.indexA = 0;
                ie[0].id.indexB = (byte)i1;
                ie[0].id.typeA = b2ContactFeatureType.e_face;
                ie[0].id.typeB = b2ContactFeatureType.e_vertex;

                ie[1].v = m_polygonB.vertices[i2];
                ie[1].id.indexA = 0;
                ie[1].id.indexB = (byte)i2;
                ie[1].id.typeA = b2ContactFeatureType.e_face;
                ie[1].id.typeB = b2ContactFeatureType.e_vertex;

                if (m_front)
                {
                    rf.i1 = 0;
                    rf.i2 = 1;
                    rf.v1 = m_v1;
                    rf.v2 = m_v2;
                    rf.normal = m_normal1;
                }
                else
                {
                    rf.i1 = 1;
                    rf.i2 = 0;
                    rf.v1 = m_v2;
                    rf.v2 = m_v1;
                    rf.normal = -m_normal1;
                }
            }
            else
            {
                manifold.type = b2ManifoldType.e_faceB;

                ie[0].v = m_v1;
                ie[0].id.indexA = 0;
                ie[0].id.indexB = (byte)primaryAxis.index;
                ie[0].id.typeA = b2ContactFeatureType.e_vertex;
                ie[0].id.typeB = b2ContactFeatureType.e_face;

                ie[1].v = m_v2;
                ie[1].id.indexA = 0;
                ie[1].id.indexB = (byte)primaryAxis.index;
                ie[1].id.typeA = b2ContactFeatureType.e_vertex;
                ie[1].id.typeB = b2ContactFeatureType.e_face;

                rf.i1 = primaryAxis.index;
                rf.i2 = rf.i1 + 1 < m_polygonB.count ? rf.i1 + 1 : 0;
                rf.v1 = m_polygonB.vertices[rf.i1];
                rf.v2 = m_polygonB.vertices[rf.i2];
                rf.normal = m_polygonB.normals[rf.i1];
            }

            rf.sideNormal1 = new b2Vec2(rf.normal.y, -rf.normal.x);
            rf.sideNormal2 = -rf.sideNormal1;
            rf.sideOffset1 = b2Math.b2Dot(ref rf.sideNormal1, ref rf.v1);
            rf.sideOffset2 = b2Math.b2Dot(ref rf.sideNormal2, ref rf.v2);

            // Clip incident edge against extruded edge1 side edges.
            b2ClipVertex[] clipPoints1 = new b2ClipVertex[2];
            b2ClipVertex[] clipPoints2 = new b2ClipVertex[2];
            int np;

            // Clip to box side 1
            np = b2Collision.b2ClipSegmentToLine(clipPoints1, ie, rf.sideNormal1, rf.sideOffset1, (byte)rf.i1);

            if (np < b2Settings.b2_maxManifoldPoints)
            {
                return;
            }

            // Clip to negative box side 1
            np = b2Collision.b2ClipSegmentToLine(clipPoints2, clipPoints1, rf.sideNormal2, rf.sideOffset2, (byte)rf.i2);

            if (np < b2Settings.b2_maxManifoldPoints)
            {
                return;
            }

            // Now clipPoints2 contains the clipped points.
            if (primaryAxis.type == b2EPAxisType.e_edgeA)
            {
                manifold.localNormal = rf.normal;
                manifold.localPoint = rf.v1;
            }
            else
            {
                manifold.localNormal = polygonB.Normals[rf.i1];
                manifold.localPoint = polygonB.Vertices[rf.i1];
            }

            int pointCount = 0;
            for (int i = 0; i < b2Settings.b2_maxManifoldPoints; ++i)
            {
                float separation;

                separation = b2Math.b2Dot(rf.normal, clipPoints2[i].v - rf.v1);

                if (separation <= m_radius)
                {
                    b2ManifoldPoint cp = manifold.points[pointCount];

                    if (primaryAxis.type == b2EPAxisType.e_edgeA)
                    {
                        cp.localPoint = b2Math.b2MulT(m_xf, clipPoints2[i].v);
                        cp.id = clipPoints2[i].id;
                    }
                    else
                    {
                        cp.localPoint = clipPoints2[i].v;
                        cp.id.typeA = clipPoints2[i].id.typeB;
                        cp.id.typeB = clipPoints2[i].id.typeA;
                        cp.id.indexA = clipPoints2[i].id.indexB;
                        cp.id.indexB = clipPoints2[i].id.indexA;
                    }

                    manifold.points[pointCount] = cp;
                    ++pointCount;
                }
            }

            manifold.pointCount = pointCount;
        }

        public b2EPAxis ComputeEdgeSeparation()
        {
            b2EPAxis axis;
            axis.type = b2EPAxisType.e_edgeA;
            axis.index = m_front ? 0 : 1;
            axis.separation = float.MaxValue;

            for (int i = 0; i < m_polygonB.count; ++i)
            {
                float s = b2Math.b2Dot(m_normal, m_polygonB.vertices[i] - m_v1);
                if (s < axis.separation)
                {
                    axis.separation = s;
                }
            }

            return axis;
        }
        public b2EPAxis ComputePolygonSeparation()
        {
            b2EPAxis axis;
            axis.type = b2EPAxisType.e_unknown;
            axis.index = -1;
            axis.separation = -float.MaxValue;

            b2Vec2 perp = new b2Vec2(-m_normal.y, m_normal.x);

            for (int i = 0; i < m_polygonB.count; ++i)
            {
                b2Vec2 n = -m_polygonB.normals[i];

                float s1 = b2Math.b2Dot(n, m_polygonB.vertices[i] - m_v1);
                float s2 = b2Math.b2Dot(n, m_polygonB.vertices[i] - m_v2);
                float s = Math.Min(s1, s2);

                if (s > m_radius)
                {
                    // No collision
                    axis.type = b2EPAxisType.e_edgeB;
                    axis.index = i;
                    axis.separation = s;
                    return axis;
                }

                // Adjacency
                if (b2Math.b2Dot(n, perp) >= 0.0f)
                {
                    if (b2Math.b2Dot(n - m_upperLimit, m_normal) < -b2Settings.b2_angularSlop)
                    {
                        continue;
                    }
                }
                else
                {
                    if (b2Math.b2Dot(n - m_lowerLimit, m_normal) < -b2Settings.b2_angularSlop)
                    {
                        continue;
                    }
                }

                if (s > axis.separation)
                {
                    axis.type = b2EPAxisType.e_edgeB;
                    axis.index = i;
                    axis.separation = s;
                }
            }

            return axis;
        }

        public enum VertexType
        {
            e_isolated,
            e_concave,
            e_convex
        };

        protected b2TempPolygon m_polygonB;

        protected b2Transform m_xf;
        protected b2Vec2 m_centroidB;
        protected b2Vec2 m_v0, m_v1, m_v2, m_v3;
        protected b2Vec2 m_normal0, m_normal1, m_normal2;
        protected b2Vec2 m_normal;
        protected VertexType m_type1, m_type2;
        protected b2Vec2 m_lowerLimit, m_upperLimit;
        protected float m_radius;
        bool m_front;
    }
}
