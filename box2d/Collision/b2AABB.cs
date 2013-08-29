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
            b2Vec2 d = UpperBound - LowerBound;
            bool valid = d.x >= 0.0f && d.y >= 0.0f;
            valid = valid && LowerBound.IsValid() && UpperBound.IsValid();
            return valid;
        }

        public bool Equals(b2AABB o)
        {
            return (LowerBound == o.LowerBound && UpperBound == o.UpperBound);
        }

        public override bool Equals(object obj)
        {
            b2AABB o = (b2AABB) obj;
            return (LowerBound == o.LowerBound && UpperBound == o.UpperBound);
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
                return (LowerBound + UpperBound) / 2f;
            }
        }

        public b2Vec2 Extents
        {
            get
            {
                return (UpperBound - LowerBound) / 2f;
            }
        }

        public float Perimeter
        {
            get
            {
                return 2.0f * (UpperBound.x - LowerBound.x + UpperBound.y - LowerBound.y);
            }
        }

        /// Get the center of the AABB.
        [Obsolete("Use the property accessor")]
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public b2Vec2 GetCenter()
        {
            return (Center);
        }

        /// Get the extents of the AABB (half-widths).
        [Obsolete("Use the property accessor")]
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public b2Vec2 GetExtents()
        {
            return (Extents);
        }

        /// Get the perimeter length
        [Obsolete("Use the property accessor")]
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public float GetPerimeter()
        {
            return (Perimeter);
        }

        /// Combine an AABB into this one.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public void Combine(ref b2AABB aabb)
        {
            LowerBound.x = aabb.LowerBound.x < LowerBound.x ? aabb.LowerBound.x : LowerBound.x;
            LowerBound.y = aabb.LowerBound.y < LowerBound.y ? aabb.LowerBound.y : LowerBound.y;

            UpperBound.x = aabb.UpperBound.x > UpperBound.x ? aabb.UpperBound.x : UpperBound.x;
            UpperBound.y = aabb.UpperBound.y > UpperBound.y ? aabb.UpperBound.y : UpperBound.y;
        }

        /// Combine two AABBs into this one.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public void Combine(ref b2AABB aabb1, ref b2AABB aabb2)
        {
            LowerBound.x = aabb1.LowerBound.x < aabb2.LowerBound.x ? aabb1.LowerBound.x : aabb2.LowerBound.x;
            LowerBound.y = aabb1.LowerBound.y < aabb2.LowerBound.y ? aabb1.LowerBound.y : aabb2.LowerBound.y;

            UpperBound.x = aabb1.UpperBound.x > aabb2.UpperBound.x ? aabb1.UpperBound.x : aabb2.UpperBound.x;
            UpperBound.y = aabb1.UpperBound.y > aabb2.UpperBound.y ? aabb1.UpperBound.y : aabb2.UpperBound.y;
        }

        public static void Combine(ref b2AABB aabb1, ref b2AABB aabb2, out b2AABB output)
        {
            output.LowerBound.x = aabb1.LowerBound.x < aabb2.LowerBound.x ? aabb1.LowerBound.x : aabb2.LowerBound.x;
            output.LowerBound.y = aabb1.LowerBound.y < aabb2.LowerBound.y ? aabb1.LowerBound.y : aabb2.LowerBound.y;

            output.UpperBound.x = aabb1.UpperBound.x > aabb2.UpperBound.x ? aabb1.UpperBound.x : aabb2.UpperBound.x;
            output.UpperBound.y = aabb1.UpperBound.y > aabb2.UpperBound.y ? aabb1.UpperBound.y : aabb2.UpperBound.y;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif

        public void Set(b2Vec2 lower, b2Vec2 upper)
        {
            LowerBound = lower;
            UpperBound = upper;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif

        public void Set(float lx, float ly, float ux, float uy)
        {
            LowerBound.Set(lx, ly);
            UpperBound.Set(ux, uy);
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif

        public void SetLowerBound(float x, float y)
        {
            LowerBound.Set(x, y);
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif

        public void SetUpperBound(float x, float y)
        {
            UpperBound.Set(x, y);
        }

        public float LowerBoundX
        {
            get { return (LowerBound.x); }
            set { LowerBound.x = value; }
        }

        public float LowerBoundY
        {
            get { return (LowerBound.y); }
            set { LowerBound.y = value; }
        }

        public float UpperBoundX
        {
            get { return (UpperBound.x); }
            set { UpperBound.x = value; }
        }

        public float UpperBoundY
        {
            get { return (UpperBound.y); }
            set { UpperBound.y = value; }
        }

        /// Does this aabb contain the provided AABB.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public bool Contains(ref b2AABB aabb)
        {
            bool result = true;
            result = result && LowerBound.x <= aabb.LowerBoundX;
            if (result)
                result = result && LowerBound.y <= aabb.LowerBoundY;
            if (result)
                result = result && aabb.UpperBoundX <= UpperBound.x;
            if (result)
                result = result && aabb.UpperBoundY <= UpperBound.y;
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
                lb = (i == 0 ? LowerBound.x : LowerBound.y);
                ub = (i == 0 ? UpperBound.x : UpperBound.y);
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
            UpperBound.x += amt;
            UpperBound.y += amt;

            LowerBound.x -= amt;
            LowerBound.y -= amt;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif

        public void Fatten()
        {
            UpperBound.x += b2Settings.b2_aabbExtensionVec.x;
            UpperBound.y += b2Settings.b2_aabbExtensionVec.y;

            LowerBound.x -= b2Settings.b2_aabbExtensionVec.x;
            LowerBound.y -= b2Settings.b2_aabbExtensionVec.y;
        }

        public b2Vec2 LowerBound; //< the lower vertex
        public b2Vec2 UpperBound; //< the upper vertex

        public static b2AABB Default = new b2AABB()
        {
            LowerBound = b2Vec2.Zero,
            UpperBound = b2Vec2.Zero,
        };

    }
}
