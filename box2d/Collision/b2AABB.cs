using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;
using System.Runtime.CompilerServices;

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

        public bool Equals(b2AABB o)
        {
            return (m_lowerBound == o.LowerBound && m_upperBound == o.UpperBound);
        }

        public override bool Equals(object obj)
        {
            b2AABB o = (b2AABB)obj;
            return (m_lowerBound == o.LowerBound && m_upperBound == o.UpperBound);
        }

        public static bool operator ==(b2AABB a, b2AABB b)
        {
            return (a.Equals(b));
        }
        public static bool operator !=(b2AABB a, b2AABB b)
        {
            return (a.LowerBound != b.LowerBound || a.UpperBound != b.UpperBound);
        }

        public b2Vec2 Center
        {
            get
            {
                if (_Dirty)
                {
                    UpdateAttributes();
                }
                return _Center;
            }
        }

        public b2Vec2 Extents
        {
            get
            {
                if (_Dirty)
                {
                    UpdateAttributes();
                }
                return _Extents;
            }
        }

        public float Perimeter
        {
            get
            {
                if (_Dirty)
                {
                    UpdateAttributes();
                }
                return (_Perimeter);
            }
        }

        /// Get the center of the AABB.
        [Obsolete("Use the property accessor")]
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public b2Vec2 GetCenter()
        {
            if (_Dirty)
            {
                UpdateAttributes();
            }
            return (Center);
        }

        /// Get the extents of the AABB (half-widths).
        [Obsolete("Use the property accessor")]
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public b2Vec2 GetExtents()
        {
            if (_Dirty)
            {
                UpdateAttributes();
            }
            return (Extents);
        }

        /// Get the perimeter length
        [Obsolete("Use the property accessor")]
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public float GetPerimeter()
        {
            if (_Dirty)
            {
                UpdateAttributes();
            }
            return (Perimeter);
        }

        public void UpdateAttributes()
        {
            float wx = m_upperBound.x - m_lowerBound.x;
            float wy = m_upperBound.y - m_lowerBound.y;
            _Perimeter = 2.0f * (wx + wy);
            _Extents = (m_upperBound - m_lowerBound)/2f;
            _Center = (m_lowerBound + m_upperBound)/2f;
            _Dirty = false;
        }

        /// Combine an AABB into this one.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public void Combine(ref b2AABB aabb)
        {
            m_lowerBound.x = aabb.LowerBoundX < LowerBoundX ? aabb.LowerBoundX : LowerBoundX;
            m_lowerBound.y = aabb.LowerBoundY < LowerBoundY ? aabb.LowerBoundY : LowerBoundY;

            m_upperBound.x = aabb.UpperBoundX > UpperBoundX ? aabb.UpperBoundX : UpperBoundX;
            m_upperBound.y = aabb.UpperBoundY > UpperBoundY ? aabb.UpperBoundY : UpperBoundY;

            //m_lowerBound = b2Math.b2Min(m_lowerBound, aabb.LowerBound);
            //m_upperBound = b2Math.b2Max(m_upperBound, aabb.UpperBound);

            _Dirty = true;
        }

        /// Combine two AABBs into this one.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public void Combine(ref b2AABB aabb1, ref b2AABB aabb2)
        {
            m_lowerBound.x = aabb1.LowerBoundX < aabb2.LowerBoundX ? aabb1.LowerBoundX : aabb2.LowerBoundX;
            m_lowerBound.y = aabb1.LowerBoundY < aabb2.LowerBoundY ? aabb1.LowerBoundY : aabb2.LowerBoundY;

            m_upperBound.x = aabb1.UpperBoundX > aabb2.UpperBoundX ? aabb1.UpperBoundX : aabb2.UpperBoundX;
            m_upperBound.y = aabb1.UpperBoundY > aabb2.UpperBoundY ? aabb1.UpperBoundY : aabb2.UpperBoundY;

//            m_lowerBound = b2Math.b2Min(aabb1.LowerBound, aabb2.LowerBound);
//            m_upperBound = b2Math.b2Max(aabb1.UpperBound, aabb2.UpperBound);

            _Dirty = true;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public void Set(b2Vec2 lower, b2Vec2 upper)
        {
            m_lowerBound = lower;
            m_upperBound = upper;
            _Dirty = true;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public void Set(float lx, float ly, float ux, float uy)
        {
            m_lowerBound.Set(lx,ly);
            m_upperBound.Set(ux,uy);
            _Dirty = true;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public void SetLowerBound(float x, float y)
        {
            m_lowerBound.Set(x,y);
            _Dirty = true;
        }
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public void SetUpperBound(float x, float y)
        {
            m_upperBound.Set(x, y);
            _Dirty = true;
        }

        public float LowerBoundX
        {
            get { return (m_lowerBound.x); }
            set { m_lowerBound.x = value; _Dirty = true; }
        }
        public float LowerBoundY
        {
            get { return (m_lowerBound.y); }
            set { m_lowerBound.y = value; _Dirty = true; }
        }
        public float UpperBoundX
        {
            get { return (m_upperBound.x); }
            set { m_upperBound.x = value; _Dirty = true; }
        }
        public float UpperBoundY
        {
            get { return (m_upperBound.y); }
            set { m_upperBound.y = value; _Dirty = true; }
        }

        /// Does this aabb contain the provided AABB.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public bool Contains(ref b2AABB aabb)
        {
            bool result = true;
            result = result && m_lowerBound.x <= aabb.LowerBoundX;
            if(result)
                result = result && m_lowerBound.y <= aabb.LowerBoundY;
            if(result)
                result = result && aabb.UpperBoundX <= m_upperBound.x;
            if(result)
                result = result && aabb.UpperBoundY <= m_upperBound.y;
            return result;
        }

        public bool RayCast(out b2RayCastOutput output, b2RayCastInput input)
        {
            float tmin = -float.MaxValue;
            float tmax = float.MaxValue;

            b2Vec2 p = input.p1;
            b2Vec2 d = input.p2 - input.p1;
            b2Vec2 absD = b2Math.b2Abs(d);

            b2Vec2 normal = b2Vec2.Zero;

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
                        output.normal = b2Vec2.Zero;
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
                        b2Math.b2Swap(ref t1, ref t2);
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
                    tmax = tmax < t2 ? tmax : t2;

                    if (tmin > tmax)
                    {
                        output.fraction = 0f;
                        output.normal = b2Vec2.Zero;
                        return false;
                    }
                }
            }

            // Does the ray start inside the box?
            // Does the ray intersect beyond the max fraction?
            if (tmin < 0.0f || input.maxFraction < tmin)
            {
                output.fraction = 0f;
                output.normal = b2Vec2.Zero;
                return false;
            }

            // Intersection.
            output.fraction = tmin;
            output.normal = normal;
            return true;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public void Fatten(float amt)
        {
            m_upperBound.x += amt;
            m_upperBound.y += amt;

            m_lowerBound.x -= amt;
            m_lowerBound.y -= amt;
            _Dirty = true;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public void Fatten()
        {
            m_upperBound.x += b2Settings.b2_aabbExtensionVec.x;
            m_upperBound.y += b2Settings.b2_aabbExtensionVec.y;

            m_lowerBound.x -= b2Settings.b2_aabbExtensionVec.x;
            m_lowerBound.y -= b2Settings.b2_aabbExtensionVec.y;
            _Dirty = true;
        }

        private bool _Dirty;

        // Private attributes
        private float _Perimeter;
        private b2Vec2 _Extents, _Center;

        public b2Vec2 LowerBound { get { return (m_lowerBound); } set { m_lowerBound = value; _Dirty = true; } }
        public b2Vec2 UpperBound { get { return (m_upperBound); } set { m_upperBound = value; _Dirty = true; } }

        private b2Vec2 m_lowerBound;    //< the lower vertex
        private b2Vec2 m_upperBound;    //< the upper vertex

        public static b2AABB Default = new b2AABB()
        {
            m_lowerBound = b2Vec2.Zero,
            m_upperBound = b2Vec2.Zero,
            _Dirty = true,
            _Perimeter = 0f,
            _Extents = b2Vec2.Zero,
            _Center = b2Vec2.Zero
        };

    }
}
