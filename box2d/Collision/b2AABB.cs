using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;

namespace Box2D.Collision
{
    public struct b2AABB
    {

        public bool IsValid()
        {
            b2Vec2 d = m_upperBound - m_lowerBound;
            bool valid = d.x >= 0.0f && d.y >= 0.0f;
            valid = valid && m_lowerBound.IsValid() && m_upperBound.IsValid();
            return valid;
        }

        /// Get the center of the AABB.
        public b2Vec2 GetCenter()
        {
            return 0.5f * (m_lowerBound + m_upperBound);
        }

        /// Get the extents of the AABB (half-widths).
        public b2Vec2 GetExtents()
        {
            return 0.5f * (m_upperBound - m_lowerBound);
        }

        /// Get the perimeter length
        public float GetPerimeter()
        {
            float wx = m_upperBound.x - m_lowerBound.x;
            float wy = m_upperBound.y - m_lowerBound.y;
            return 2.0f * (wx + wy);
        }

        /// Combine an AABB into this one.
        public void Combine(b2AABB aabb)
        {
            m_lowerBound = b2Math.b2Min(m_lowerBound, aabb.m_lowerBound);
            m_upperBound = b2Math.b2Max(m_upperBound, aabb.m_upperBound);
        }

        /// Combine two AABBs into this one.
        public void Combine(b2AABB aabb1, b2AABB aabb2)
        {
            m_lowerBound = b2Math.b2Min(aabb1.m_lowerBound, aabb2.m_lowerBound);
            m_upperBound = b2Math.b2Max(aabb1.m_upperBound, aabb2.m_upperBound);
        }

        /// Does this aabb contain the provided AABB.
        public bool Contains(b2AABB aabb)
        {
            bool result = true;
            result = result && m_lowerBound.x <= aabb.m_lowerBound.x;
            result = result && m_lowerBound.y <= aabb.m_lowerBound.y;
            result = result && aabb.m_upperBound.x <= m_upperBound.x;
            result = result && aabb.m_upperBound.y <= m_upperBound.y;
            return result;
        }

        public bool RayCast(out b2RayCastOutput output, b2RayCastInput input)
        {
            float tmin = -float.MaxValue;
            float tmax = float.MaxValue;

            b2Vec2 p = input.p1;
            b2Vec2 d = input.p2 - input.p1;
            b2Vec2 absD = b2Math.b2Abs(d);

            b2Vec2 normal = new b2Vec2(0,0);

            for (int i = 0; i < 2; ++i)
            {
                float p_i, lb, ub, d_i, absd_i;
                p_i = (i == 0 ? p.x : p.y);
                lb = (i == 0 ? m_lowerBound.x : m_lowerBound.y);
                ub = (i == 0 ? m_upperBound.x : m_upperBound.y);
                absd_i = (i == 0 ? absD.x : absD.y);
                d_i = (i == 0 ? d.x : d.y);

                if (absd_i < b2Settings.b2_epsilon)
                {
                    // Parallel.
                    if (p_i < lb || ub < p_i)
                    {
                        output.fraction = 0f;
                        output.normal = new b2Vec2(0, 0);
                        return false;
                    }
                }
                else
                {
                    float inv_d = 1.0f / d_i;
                    float t1 = (lb - p_i) * inv_d;
                    float t2 = (ub - p_i) * inv_d;

                    // Sign of the normal vector.
                    float s = -1.0f;

                    if (t1 > t2)
                    {
                        b2Math.b2Swap(t1, t2);
                        s = 1.0f;
                    }

                    // Push the min up
                    if (t1 > tmin)
                    {
                        normal.SetZero();
                        if (i == 0)
                        {
                            normal.x = s;
                        }
                        else
                        {
                            normal.y = s;
                        }
                        tmin = t1;
                    }

                    // Pull the max down
                    tmax = Math.Min(tmax, t2);

                    if (tmin > tmax)
                    {
                        output.fraction = 0f;
                        output.normal = new b2Vec2(0, 0);
                        return false;
                    }
                }
            }

            // Does the ray start inside the box?
            // Does the ray intersect beyond the max fraction?
            if (tmin < 0.0f || input.maxFraction < tmin)
            {
                output.fraction = 0f;
                output.normal = new b2Vec2(0, 0);
                return false;
            }

            // Intersection.
            output.fraction = tmin;
            output.normal = normal;
            return true;
        }

        public float lowerBoundx { get { return (m_lowerBound.x); } set { m_lowerBound.x = value; } }
        public float lowerBoundy { get { return (m_lowerBound.y); } set { m_lowerBound.y = value; } }

        public float upperBoundx { get { return (m_upperBound.x); } set { m_upperBound.x = value; } }
        public float upperBoundy { get { return (m_upperBound.y); } set { m_upperBound.y = value; } }

        public b2Vec2 lowerBound { get { return (m_lowerBound); } set { m_lowerBound = value; } }
        public b2Vec2 upperBound { get { return (m_upperBound); } set { m_upperBound = value; } }

        private b2Vec2 m_lowerBound;    ///< the lower vertex
        private b2Vec2 m_upperBound;    ///< the upper vertex

    }
}
