using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box2D.Common
{
    /// A 3-by-3 matrix. Stored in column-major order.
    public struct b2Mat33
    {
        public float exx { get { return ex.x; } set { ex.x = value; } }
        public float exy { get { return ex.y; } set { ex.y = value; } }
        public float exz { get { return ex.z; } set { ex.z = value; } }

        public float eyx { get { return ey.x; } set { ey.x = value; } }
        public float eyy { get { return ey.y; } set { ey.y = value; } }
        public float eyz { get { return ey.z; } set { ey.z = value; } }

        public float ezx { get { return ez.x; } set { ez.x = value; } }
        public float ezy { get { return ez.y; } set { ez.y = value; } }
        public float ezz { get { return ez.z; } set { ez.z = value; } }

        /*
                public b2Mat33()
                {
                    ex = new b2Vec3();
                    ey = new b2Vec3();
                    ez = new b2Vec3();
                }
        */

        /// ruct this matrix using columns.
        public b2Mat33(b2Vec3 c1, b2Vec3 c2, b2Vec3 c3)
        {
            ex = c1;
            ey = c2;
            ez = c3;
        }

        /// Set this matrix to all zeros.
        public void SetZero()
        {
            ex.SetZero();
            ey.SetZero();
            ez.SetZero();
        }

        /// Solve A * x = b, where b is a column vector. This is more efficient
        /// than computing the inverse in one-shot cases.
        public b2Vec3 Solve33(b2Vec3 b)
        {
            b2Vec3 cx = b2Math.b2Cross(ref ey, ref ez);
            float det = b2Math.b2Dot(ref ex, ref cx);
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }
            b2Vec3 x = new b2Vec3();
            cx = b2Math.b2Cross(ref ey, ref ez);
            x.x = det * b2Math.b2Dot(ref b, ref cx);
            cx = b2Math.b2Cross(ref b, ref ez);
            x.y = det * b2Math.b2Dot(ref ex, ref cx);
            cx = b2Math.b2Cross(ref ey, ref b);
            x.z = det * b2Math.b2Dot(ref ex, ref cx);
            return x;
        }

        /// Solve A * x = b, where b is a column vector. This is more efficient
        /// than computing the inverse in one-shot cases. Solve only the upper
        /// 2-by-2 matrix equation.
        public b2Vec2 Solve22(b2Vec2 b)
        {
            float a11 = ex.x, a12 = ey.x, a21 = ex.y, a22 = ey.y;
            float det = a11 * a22 - a12 * a21;
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }
            b2Vec2 x = new b2Vec2();
            x.x = det * (a22 * b.x - a12 * b.y);
            x.y = det * (a11 * b.y - a21 * b.x);
            return x;
        }

        /// Get the inverse of this matrix as a 2-by-2.
        /// Returns the zero matrix if singular.
        public b2Mat33 GetInverse22(b2Mat33 M)
        {
            float a = ex.x, b = ey.x, c = ex.y, d = ey.y;
            float det = a * d - b * c;
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }

            M.ex.x = det * d; M.ey.x = -det * b; M.ex.z = 0.0f;
            M.ex.y = -det * c; M.ey.y = det * a; M.ey.z = 0.0f;
            M.ez.x = 0.0f; M.ez.y = 0.0f; M.ez.z = 0.0f;
            return (M);
        }

        /// Get the symmetric inverse of this matrix as a 3-by-3.
        /// Returns the zero matrix if singular.
        public b2Mat33 GetSymInverse33(b2Mat33 M)
        {
            b2Vec3 cross = b2Math.b2Cross(ey, ez);
            float det = b2Math.b2Dot(ex, cross);
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }

            float a11 = ex.x, a12 = ey.x, a13 = ez.x;
            float a22 = ey.y, a23 = ez.y;
            float a33 = ez.z;

            M.ex.x = det * (a22 * a33 - a23 * a23);
            M.ex.y = det * (a13 * a23 - a12 * a33);
            M.ex.z = det * (a12 * a23 - a13 * a22);

            M.ey.x = M.ex.y;
            M.ey.y = det * (a11 * a33 - a13 * a13);
            M.ey.z = det * (a13 * a12 - a11 * a23);

            M.ez.x = M.ex.z;
            M.ez.y = M.ey.z;
            M.ez.z = det * (a11 * a22 - a12 * a12);
            return (M);
        }

        public b2Vec3 ex, ey, ez;
    }
}
