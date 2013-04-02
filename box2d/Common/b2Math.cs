using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box2D.Common
{
    public class b2Math
    {
        /// Useful ant
        public static b2Vec2 b2Vec2_zero = b2Vec2.Zero;


        /// Friction mixing law. The idea is to allow either fixture to drive the restitution to zero.
        /// For example, anything slides on ice.
        public static float b2MixFriction(float friction1, float friction2)
        {
            return (friction1 * friction2);
        }

        /// Restitution mixing law. The idea is allow for anything to bounce off an inelastic surface.
        /// For example, a superball bounces on anything.
        public static float b2MixRestitution(float restitution1, float restitution2)
        {
            return restitution1 > restitution2 ? restitution1 : restitution2;
        }

        /// This function is used to ensure that a floating point number is
        /// not a NaN or infinity.
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

        /// This is a approximate yet fast inverse square-root.
        public static float b2InvSqrt(float x)
        {

            float cx;
            int ci;

            cx = x;
            float xhalf = 0.5f * x;
            ci = (int)cx;
            ci = 0x5f3759df - (ci >> 1);
            x = (int)ci;
            x = x * (1.5f - xhalf * x * x);
            return x;
        }

        public static float b2Sqrt(float x) { return ((float)Math.Sqrt(x)); }
        public static float b2Atan2(float y, float x) { return ((float)Math.Atan2(y, x)); }

        /// Perform the dot product on two vectors.
        public static float b2Dot(b2Vec2 a, b2Vec2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        /// Perform the cross product on two vectors. In 2D this produces a scalar.
        public static float b2Cross(b2Vec2 a, b2Vec2 b)
        {
            return a.x * b.y - a.y * b.x;
        }

        /// Perform the cross product on a vector and a scalar. In 2D this produces
        /// a vector.
        public static b2Vec2 b2Cross(b2Vec2 a, float s)
        {
            return new b2Vec2(s * a.y, -s * a.x);
        }

        /// Perform the cross product on a scalar and a vector. In 2D this produces
        /// a vector.
        public static b2Vec2 b2Cross(float s, b2Vec2 a)
        {
            return new b2Vec2(-s * a.y, s * a.x);
        }

        /// Multiply a matrix times a vector. If a rotation matrix is provided,
        /// then this transforms the vector from one frame to another.
        public static b2Vec2 b2Mul(b2Mat22 A, b2Vec2 v)
        {
            return new b2Vec2(A.ex.x * v.x + A.ey.x * v.y, A.ex.y * v.x + A.ey.y * v.y);
        }

        /// Multiply a matrix transpose times a vector. If a rotation matrix is provided,
        /// then this transforms the vector from one frame to another (inverse transform).
        public static b2Vec2 b2MulT(b2Mat22 A, b2Vec2 v)
        {
            return new b2Vec2(b2Dot(v, A.ex), b2Dot(v, A.ey));
        }

        public static float b2Distance(b2Vec2 a, b2Vec2 b)
        {
            b2Vec2 c = a - b;
            return c.Length();
        }

        public static float b2DistanceSquared(b2Vec2 a, b2Vec2 b)
        {
            b2Vec2 c = a - b;
            return b2Dot(c, c);
        }


        /// Perform the dot product on two vectors.
        public static float b2Dot(b2Vec3 a, b2Vec3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        /// Perform the cross product on two vectors.
        public static b2Vec3 b2Cross(b2Vec3 a, b2Vec3 b)
        {
            return new b2Vec3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }


        // A * B
        public static b2Mat22 b2Mul(b2Mat22 A, b2Mat22 B)
        {
            return new b2Mat22(b2Mul(A, B.ex), b2Mul(A, B.ey));
        }

        // A^T * B
        public static b2Mat22 b2MulT(b2Mat22 A, b2Mat22 B)
        {
            b2Vec2 c1 = new b2Vec2(b2Dot(A.ex, B.ex), b2Dot(A.ey, B.ex));
            b2Vec2 c2 = new b2Vec2(b2Dot(A.ex, B.ey), b2Dot(A.ey, B.ey));
            return new b2Mat22(c1, c2);
        }

        /// Multiply a matrix times a vector.
        public static b2Vec3 b2Mul(b2Mat33 A, b2Vec3 v)
        {
            return v.x * A.ex + v.y * A.ey + v.z * A.ez;
        }

        /// Multiply a matrix times a vector.
        public static b2Vec2 b2Mul22(b2Mat33 A, b2Vec2 v)
        {
            return new b2Vec2(A.ex.x * v.x + A.ey.x * v.y, A.ex.y * v.x + A.ey.y * v.y);
        }

        /// Multiply two rotations: q * r
        public static b2Rot b2Mul(b2Rot q, b2Rot r)
        {
            // [qc -qs] * [rc -rs] = [qc*rc-qs*rs -qc*rs-qs*rc]
            // [qs  qc]   [rs  rc]   [qs*rc+qc*rs -qs*rs+qc*rc]
            // s = qs * rc + qc * rs
            // c = qc * rc - qs * rs
            b2Rot qr = new b2Rot();
            qr.s = q.s * r.c + q.c * r.s;
            qr.c = q.c * r.c - q.s * r.s;
            return qr;
        }

        /// Transpose multiply two rotations: qT * r
        public static b2Rot b2MulT(b2Rot q, b2Rot r)
        {
            // [ qc qs] * [rc -rs] = [qc*rc+qs*rs -qc*rs+qs*rc]
            // [-qs qc]   [rs  rc]   [-qs*rc+qc*rs qs*rs+qc*rc]
            // s = qc * rs - qs * rc
            // c = qc * rc + qs * rs
            b2Rot qr = new b2Rot();
            qr.s = q.c * r.s - q.s * r.c;
            qr.c = q.c * r.c + q.s * r.s;
            return qr;
        }

        /// Rotate a vector
        public static b2Vec2 b2Mul(b2Rot q, b2Vec2 v)
        {
            return new b2Vec2(q.c * v.x - q.s * v.y, q.s * v.x + q.c * v.y);
        }

        /// Inverse rotate a vector
        public static b2Vec2 b2MulT(b2Rot q, b2Vec2 v)
        {
            return new b2Vec2(q.c * v.x + q.s * v.y, -q.s * v.x + q.c * v.y);
        }

        public static b2Vec2 b2Mul(b2Transform T, b2Vec2 v)
        {
            float x = (T.q.c * v.x - T.q.s * v.y) + T.p.x;
            float y = (T.q.s * v.x + T.q.c * v.y) + T.p.y;

            return new b2Vec2(x, y);
        }

        public static b2Vec2 b2MulT(b2Transform T, b2Vec2 v)
        {
            float px = v.x - T.p.x;
            float py = v.y - T.p.y;
            float x = (T.q.c * px + T.q.s * py);
            float y = (-T.q.s * px + T.q.c * py);

            return new b2Vec2(x, y);
        }

        // v2 = A.q.Rot(B.q.Rot(v1) + B.p) + A.p
        //    = (A.q * B.q).Rot(v1) + A.q.Rot(B.p) + A.p
        public static b2Transform b2Mul(b2Transform A, b2Transform B)
        {
            b2Transform C = new b2Transform();
            C.q = b2Mul(A.q, B.q);
            C.p = b2Mul(A.q, B.p) + A.p;
            return C;
        }

        // v2 = A.q' * (B.q * v1 + B.p - A.p)
        //    = A.q' * B.q * v1 + A.q' * (B.p - A.p)
        public static b2Transform b2MulT(b2Transform A, b2Transform B)
        {
            b2Transform C = new b2Transform();
            C.q = b2MulT(A.q, B.q);
            C.p = b2MulT(A.q, B.p - A.p);
            return C;
        }

        public static float b2Abs(float a)
        {
            return (Math.Abs(a));
        }
        public static b2Vec2 b2Abs(b2Vec2 a)
        {
            return new b2Vec2(b2Abs(a.x), b2Abs(a.y));
        }

        public static b2Mat22 b2Abs(b2Mat22 A)
        {
            return new b2Mat22(b2Abs(A.ex), b2Abs(A.ey));
        }

        public static b2Vec2 b2Min(b2Vec2 a, b2Vec2 b)
        {
            return new b2Vec2(b2Min(a.x, b.x), b2Min(a.y, b.y));
        }

        public static b2Vec2 b2Max(b2Vec2 a, b2Vec2 b)
        {
            return new b2Vec2(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
        }

        public static float b2Clamp(float a, float low, float high)
        {
            return (a < low ? low : (a > high ? high : a));
        }

        public static b2Vec2 b2Clamp(b2Vec2 a, b2Vec2 low, b2Vec2 high)
        {
            return b2Max(low, b2Min(a, high));
        }

        public static void b2Swap<T>(T a, T b)
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
        public static uint b2NextPowerOfTwo(uint x)
        {
            x |= (x >> 1);
            x |= (x >> 2);
            x |= (x >> 4);
            x |= (x >> 8);
            x |= (x >> 16);
            return x + 1;
        }

        public static bool b2IsPowerOfTwo(uint x)
        {
            bool result = x > 0 && (x & (x - 1)) == 0;
            return result;
        }
    }
}
