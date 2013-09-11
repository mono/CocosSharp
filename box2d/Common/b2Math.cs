using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace Box2D.Common
{
    public class b2Math
    {
        /// Useful ant
        public static b2Vec2 b2Vec2_zero = b2Vec2.Zero;


        /// Friction mixing law. The idea is to allow either fixture to drive the restitution to zero.
        /// For example, anything slides on ice.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static float b2MixFriction(float friction1, float friction2)
        {
            return (float)Math.Sqrt(friction1 * friction2);
        }

        /// Restitution mixing law. The idea is allow for anything to bounce off an inelastic surface.
        /// For example, a superball bounces on anything.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static float b2MixRestitution(float restitution1, float restitution2)
        {
            return restitution1 > restitution2 ? restitution1 : restitution2;
        }

        /// This function is used to ensure that a floating point number is
        /// not a NaN or infinity.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static bool b2IsValid(float x)
        {
            if (float.IsNaN(x))
            {
                return (false);
            }

            if (float.IsNegativeInfinity(x) || float.IsPositiveInfinity(x))
            {
                return (false);
            }
            if (x == float.MaxValue || x == float.MinValue)
            {
                return (false);
            }
            return (true);
        }

        /// This is an approximate yet fast inverse square-root.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static float b2InvSqrt(float x)
        {

            float cx;
            int ci;

            cx = x;
            float xhalf = x / 2f;
            ci = (int)cx;
            ci = 0x5f3759df - (ci >> 1);
            x = (int)ci;
            x = x * (1.5f - xhalf * x * x);
            return x;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static float b2Sqrt(float x) { return ((float)Math.Sqrt(x)); }
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static float b2Atan2(float y, float x) { return ((float)Math.Atan2(y, x)); }

        /// Perform the dot product on two vectors.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static float b2Dot(ref b2Vec2 a, ref b2Vec2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        [Obsolete("Use the ref b2Dot instead")]
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static float b2Dot(b2Vec2 a, b2Vec2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static float b2Dot(float ax, float ay, float bx, float by)
        {
            return ax * bx + ay * by;
        }
        
        /// Perform the cross product on two vectors. In 2D this produces a scalar.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static float b2Cross(ref b2Vec2 a, ref b2Vec2 b)
        {
            return a.x * b.y - a.y * b.x;
        }

        [Obsolete("Use the ref b2Cross")]
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static float b2Cross(b2Vec2 a, b2Vec2 b)
        {
            return a.x * b.y - a.y * b.x;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static float b2Cross(float ax, float ay, float bx, float by)
        {
            return ax * by - ay * bx;
        }

        /// Perform the cross product on a vector and a scalar. In 2D this produces
        /// a vector.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec2 b2Cross(ref b2Vec2 a, float s)
        {
            b2Vec2 b;
            b.x = s * a.y;
            b.y = -s * a.x;
            return b;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec2 b2Cross(float ax, float ay, float s)
        {
            b2Vec2 b;
            b.x = s * ay;
            b.y =  -s * ax;
            return b;
        }

        /// Perform the cross product on a scalar and a vector. In 2D this produces
        /// a vector.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec2 b2Cross(float s, ref b2Vec2 a)
        {
            b2Vec2 b;
            b.x = -s * a.y;
            b.y = s * a.x;
            return b;
        }

        /// Multiply a matrix times a vector. If a rotation matrix is provided,
        /// then this transforms the vector from one frame to another.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec2 b2Mul(ref b2Mat22 A, ref b2Vec2 v)
        {
            b2Vec2 b;
            b.x = A.ex.x * v.x + A.ey.x * v.y;
            b.y = A.ex.y * v.x + A.ey.y * v.y;
            return b;
        }

        /// Multiply a matrix times a vector. If a rotation matrix is provided,
        /// then this transforms the vector from one frame to another.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec2 b2Mul(b2Mat22 A, b2Vec2 v)
        {
            b2Vec2 b;
            b.x = A.ex.x * v.x + A.ey.x * v.y;
            b.y = A.ex.y * v.x + A.ey.y * v.y;
            return b;
        }

        /// Multiply a matrix transpose times a vector. If a rotation matrix is provided,
        /// then this transforms the vector from one frame to another (inverse transform).
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec2 b2MulT(ref b2Mat22 A, ref b2Vec2 v)
        {
            b2Vec2 b;
            b.x = b2Dot(ref v, ref A.ex);
            b.y = b2Dot(ref v, ref A.ey);
            return b;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static float b2Distance(b2Vec2 a, b2Vec2 b)
        {
            b2Vec2 c = a - b;
            return c.Length;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static float b2Distance(ref b2Vec2 a, ref b2Vec2 b)
        {
            float x = a.x - b.x;
            float y = a.y - b.y;
            return (float)Math.Sqrt(x * x + y * y);
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static float b2DistanceSquared(b2Vec2 a, b2Vec2 b)
        {
            float x = a.x - b.x;
            float y = a.y - b.y;
            return x * x + y * y;
        }

        public static float b2DistanceSquared(ref b2Vec2 a, ref b2Vec2 b)
        {
            float x = a.x - b.x;
            float y = a.y - b.y;
            return x * x + y * y;
        }

        /// Perform the dot product on two vectors.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static float b2Dot(b2Vec3 a, b2Vec3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        /// Perform the dot product on two vectors.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static float b2Dot(ref b2Vec3 a, ref b2Vec3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        /// Perform the cross product on two vectors.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec3 b2Cross(ref b2Vec3 a, ref b2Vec3 b)
        {
            return new b2Vec3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }

        /// Perform the cross product on two vectors.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec3 b2Cross(b2Vec3 a, b2Vec3 b)
        {
            return new b2Vec3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }

        // A * B
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Mat22 b2Mul(b2Mat22 A, b2Mat22 B)
        {
            return new b2Mat22(b2Mul(A, B.ex), b2Mul(A, B.ey));
        }

        // A^T * B
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Mat22 b2MulT(b2Mat22 A, b2Mat22 B)
        {
            b2Vec2 c1;
            b2Vec2 c2;

            c1.x = b2Dot(ref A.ex, ref B.ex);
            c1.y = b2Dot(ref A.ey, ref B.ex);
            c2.x = b2Dot(ref A.ex, ref B.ey);
            c2.y = b2Dot(ref A.ey, ref B.ey);

            return new b2Mat22(c1, c2);
        }

        /// Multiply a matrix times a vector.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec3 b2Mul(b2Mat33 A, b2Vec3 v)
        {
            return v.x * A.ex + v.y * A.ey + v.z * A.ez;
        }

        /// Multiply a matrix times a vector.
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec2 b2Mul22(b2Mat33 A, b2Vec2 v)
        {
            b2Vec2 b;
            b.x = A.ex.x * v.x + A.ey.x * v.y;
            b.y = A.ex.y * v.x + A.ey.y * v.y;
            return b;
        }

        /// Multiply two rotations: q * r
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Rot b2Mul(b2Rot q, b2Rot r)
        {
            // [qc -qs] * [rc -rs] = [qc*rc-qs*rs -qc*rs-qs*rc]
            // [qs  qc]   [rs  rc]   [qs*rc+qc*rs -qs*rs+qc*rc]
            // s = qs * rc + qc * rs
            // c = qc * rc - qs * rs
            b2Rot qr;
            qr.s = q.s * r.c + q.c * r.s;
            qr.c = q.c * r.c - q.s * r.s;
            return qr;
        }

        /// Transpose multiply two rotations: qT * r
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Rot b2MulT(b2Rot q, b2Rot r)
        {
            // [ qc qs] * [rc -rs] = [qc*rc+qs*rs -qc*rs+qs*rc]
            // [-qs qc]   [rs  rc]   [-qs*rc+qc*rs qs*rs+qc*rc]
            // s = qc * rs - qs * rc
            // c = qc * rc + qs * rs
            b2Rot qr;
            qr.s = q.c * r.s - q.s * r.c;
            qr.c = q.c * r.c + q.s * r.s;
            return qr;
        }

        /// Rotate a vector
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec2 b2Mul(b2Rot q, b2Vec2 v)
        {
            b2Vec2 b;
            b.x = q.c * v.x - q.s * v.y;
            b.y = q.s * v.x + q.c * v.y;
            return b;
        }

        /// Rotate a vector
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec2 b2Mul(ref b2Rot q, ref b2Vec2 v)
        {
            b2Vec2 b;
            b.x = q.c * v.x - q.s * v.y;
            b.y = q.s * v.x + q.c * v.y;
            return b;
        }

        /// Inverse rotate a vector
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec2 b2MulT(b2Rot q, b2Vec2 v)
        {
            b2Vec2 b;
            b.x = q.c * v.x + q.s * v.y;
            b.y = -q.s * v.x + q.c * v.y;
            return b;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec2 b2MulT(ref b2Rot q, ref b2Vec2 v)
        {
            b2Vec2 b;
            b.x = q.c * v.x + q.s * v.y;
            b.y = -q.s * v.x + q.c * v.y;
            return b;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec2 b2Mul(b2Transform T, b2Vec2 v)
        {
            b2Vec2 b;
            b.x = (T.q.c * v.x - T.q.s * v.y) + T.p.x;
            b.y = (T.q.s * v.x + T.q.c * v.y) + T.p.y;
            return b;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec2 b2Mul(ref b2Transform T, ref b2Vec2 v)
        {
            b2Vec2 b;
            b.x = (T.q.c * v.x - T.q.s * v.y) + T.p.x;
            b.y = (T.q.s * v.x + T.q.c * v.y) + T.p.y;
            return b;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec2 b2MulT(b2Transform T, b2Vec2 v)
        {
            b2Vec2 b;
            float px = v.x - T.p.x;
            float py = v.y - T.p.y;
            b.x = (T.q.c * px + T.q.s * py);
            b.y = (-T.q.s * px + T.q.c * py);
            return b;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec2 b2MulT(ref b2Transform T, ref b2Vec2 v)
        {
            b2Vec2 b;
            float px = v.x - T.p.x;
            float py = v.y - T.p.y;
            b.x = (T.q.c * px + T.q.s * py);
            b.y = (-T.q.s * px + T.q.c * py);
            return b;
        }

        // v2 = A.q.Rot(B.q.Rot(v1) + B.p) + A.p
        //    = (A.q * B.q).Rot(v1) + A.q.Rot(B.p) + A.p
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Transform b2Mul(b2Transform A, b2Transform B)
        {
            b2Transform C;
            C.q = b2Mul(A.q, B.q);
            C.p = b2Mul(A.q, B.p) + A.p;
            return C;
        }

        // v2 = A.q' * (B.q * v1 + B.p - A.p)
        //    = A.q' * B.q * v1 + A.q' * (B.p - A.p)
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Transform b2MulT(b2Transform A, b2Transform B)
        {
            b2Transform C;
            C.q = b2MulT(A.q, B.q);
            C.p = b2MulT(A.q, B.p - A.p);
            return C;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static float b2Abs(float a)
        {
            return (Math.Abs(a));
        }
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec2 b2Abs(b2Vec2 a)
        {
            b2Vec2 bx;
            bx.x = Math.Abs(a.x);
            bx.y = Math.Abs(a.y);
            return bx;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Mat22 b2Abs(b2Mat22 A)
        {
            return new b2Mat22(b2Abs(A.ex), b2Abs(A.ey));
        }

        /// <summary>
        /// Returns a vectors that uses the minimum value of the individual
        /// components, not a minimum length vector or other minimum attribute
        /// of the vectors. Min ( (5,4), (3,5) ) = (3,4)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec2 b2Min(b2Vec2 a, b2Vec2 b)
        {
            b2Vec2 bx;
            bx.x = Math.Min(a.x, b.x);
            bx.y = Math.Min(a.y, b.y);
            return bx;
        }

        public static void b2Min(ref b2Vec2 a, ref b2Vec2 b, out b2Vec2 output)
        {
            output.x = Math.Min(a.x, b.x);
            output.y = Math.Min(a.y, b.y);
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec2 b2Max(b2Vec2 a, b2Vec2 b)
        {
            b2Vec2 bx;
            bx.x = Math.Max(a.x, b.x);
            bx.y = Math.Max(a.y, b.y);
            return bx;
        }

        public static void b2Max(ref b2Vec2 a, ref b2Vec2 b, out b2Vec2 output)
        {
            output.x = Math.Max(a.x, b.x);
            output.y = Math.Max(a.y, b.y);
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static float b2Clamp(float a, float low, float high)
        {
            return (a < low ? low : (a > high ? high : a));
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static b2Vec2 b2Clamp(b2Vec2 a, b2Vec2 low, b2Vec2 high)
        {
            return b2Max(low, b2Min(a, high));
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static void b2Swap<T>(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }

        /// "Next Largest Power of 2
        /// Given a binary integer value x, the next largest power of 2 can be computed by a SWAR algorithm
        /// that recursively "folds" the upper bits into the lower bits. This process yields a bit vector with
        /// the same most significant 1 as x, but all 1's below it. Adding 1 to that value yields the next
        /// largest power of 2. For a 32-bit value:"
#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static int b2NextPowerOfTwo(int x)
        {
            x |= (x >> 1);
            x |= (x >> 2);
            x |= (x >> 4);
            x |= (x >> 8);
            x |= (x >> 16);
            return x + 1;
        }

#if AGGRESSIVE_INLINING
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
#endif
        public static bool b2IsPowerOfTwo(int x)
        {
            bool result = x > 0 && (x & (x - 1)) == 0;
            return result;
        }
    }
}
