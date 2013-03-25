using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box2D.Common
{
    /// A 3-by-3 matrix. Stored in column-major order.
    public struct b2Mat33
    {
        public b2Vec3 ex { get { return (_ex); } set { _ex = value; } }
        public b2Vec3 ey { get { return (_ey); } set { _ey = value; } }
        public b2Vec3 ez { get { return (_ez); } set { _ez = value; } }

/*
        public b2Mat33()
        {
            _ex = new b2Vec3();
            _ey = new b2Vec3();
            _ez = new b2Vec3();
        }
 */

        /// ruct this matrix using columns.
        public b2Mat33(b2Vec3 c1, b2Vec3 c2, b2Vec3 c3)
        {
            _ex = c1;
            _ey = c2;
            _ez = c3;
        }

        /// Set this matrix to all zeros.
        public void SetZero()
        {
            _ex.SetZero();
            _ey.SetZero();
            _ez.SetZero();
        }

        /// Solve A * x = b, where b is a column vector. This is more efficient
        /// than computing the inverse in one-shot cases.
        public b2Vec3 Solve33(b2Vec3 b)
        {
            float det = b2Math.b2Dot(_ex, b2Math.b2Cross(_ey, _ez));
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }
            b2Vec3 x = new b2Vec3();
            x.x = det * b2Math.b2Dot(b, b2Math.b2Cross(_ey, _ez));
            x.y = det * b2Math.b2Dot(_ex, b2Math.b2Cross(b, _ez));
            x.z = det * b2Math.b2Dot(_ex, b2Math.b2Cross(_ey, b));
            return x;
        }

        /// Solve A * x = b, where b is a column vector. This is more efficient
        /// than computing the inverse in one-shot cases. Solve only the upper
        /// 2-by-2 matrix equation.
        public b2Vec2 Solve22(b2Vec2 b)
        {
            float a11 = _ex.x, a12 = _ey.x, a21 = _ex.y, a22 = _ey.y;
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
        public void GetInverse22(b2Mat33 M)
        {
            float a = _ex.x, b = _ey.x, c = _ex.y, d = _ey.y;
            float det = a * d - b * c;
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }

            M._ex.x = det * d; M._ey.x = -det * b; M._ex.z = 0.0f;
            M._ex.y = -det * c; M._ey.y = det * a; M._ey.z = 0.0f;
            M._ez.x = 0.0f; M._ez.y = 0.0f; M._ez.z = 0.0f;
        }

        /// Get the symmetric inverse of this matrix as a 3-by-3.
        /// Returns the zero matrix if singular.
        public void GetSymInverse33(b2Mat33 M)
        {
            float det = b2Math.b2Dot(_ex, b2Math.b2Cross(_ey, _ez));
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }

            float a11 = _ex.x, a12 = _ey.x, a13 = _ez.x;
            float a22 = _ey.y, a23 = _ez.y;
            float a33 = _ez.z;

            M._ex.x = det * (a22 * a33 - a23 * a23);
            M._ex.y = det * (a13 * a23 - a12 * a33);
            M._ex.z = det * (a12 * a23 - a13 * a22);

            M._ey.x = M._ex.y;
            M._ey.y = det * (a11 * a33 - a13 * a13);
            M._ey.z = det * (a13 * a12 - a11 * a23);

            M._ez.x = M._ex.z;
            M._ez.y = M._ey.z;
            M._ez.z = det * (a11 * a22 - a12 * a12);
        }

        private b2Vec3 _ex, _ey, _ez;
    }
}
