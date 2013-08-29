using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box2D.Common
{
    /// A 2-by-2 matrix. Stored in column-major order.
    public struct b2Mat22
    {
        public static readonly b2Mat22 Zero = new b2Mat22(0f, 0f, 0f, 0f);
        public static readonly b2Mat22 Identity = new b2Mat22(1f, 0f, 0f, 1f);

        /// Construct this matrix using columns.
        public b2Mat22(b2Vec2 c1, b2Vec2 c2)
        {
            ex = c1;
            ey = c2;
        }

        /// Construct this matrix using scalars.
        public b2Mat22(float a11, float a12, float a21, float a22)
        {
            ex.x = a11; ex.y = a21;
            ey.x = a12; ey.y = a22;
        }

        public float exx { set { ex.x = value; } }
        public float exy { set { ex.y = value; } }
        public float eyx { set { ey.x = value; } }
        public float eyy { set { ey.y = value; } }
        
        /// Initialize this matrix using columns.
        public void Set(b2Vec2 c1, b2Vec2 c2)
        {
            ex = c1;
            ey = c2;
        }

        /// Multiply a matrix times a vector. If a rotation matrix is provided,
        /// then this transforms the vector from one frame to another.
        public static b2Vec2 operator *(b2Mat22 A, b2Vec2 v)
        {
            return new b2Vec2(A.ex.x * v.x + A.ey.x * v.y, A.ex.y * v.x + A.ey.y * v.y);
        }

        public static b2Vec2 operator *(b2Vec2 v, b2Mat22 A)
        {
            return new b2Vec2(A.ex.x * v.x + A.ey.x * v.y, A.ex.y * v.x + A.ey.y * v.y);
        }

        // A * B
        public static b2Mat22 operator *(b2Mat22 A, b2Mat22 B)
        {
            return new b2Mat22(A * B.ex, A * B.ey);
        }

        public static b2Mat22 operator +(b2Mat22 A, b2Mat22 B)
        {
            return new b2Mat22(A.ex + B.ex, A.ey + B.ey);
        }

        /// Set this to the identity matrix.
        public void SetIdentity()
        {
            ex.x = 1.0f; ey.x = 0.0f;
            ex.y = 0.0f; ey.y = 1.0f;
        }

        /// Set this matrix to all zeros.
        public void SetZero()
        {
            ex.x = 0.0f; ey.x = 0.0f;
            ex.y = 0.0f; ey.y = 0.0f;
        }

        public b2Mat22 GetInverse()
        {
            float a = ex.x, b = ey.x, c = ex.y, d = ey.y;
            b2Mat22 B;
            float det = a * d - b * c;
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }
            B.ex.x = det * d; B.ey.x = -det * b;
            B.ex.y = -det * c; B.ey.y = det * a;
            return B;
        }

        public void GetInverse(out b2Mat22 matrix)
        {
            float a = ex.x, b = ey.x, c = ex.y, d = ey.y;
            float det = a * d - b * c;
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }
            matrix.ex.x = det * d; matrix.ey.x = -det * b;
            matrix.ex.y = -det * c; matrix.ey.y = det * a;
        }

        /// Solve A * x = b, where b is a column vector. This is more efficient
        /// than computing the inverse in one-shot cases.
        public b2Vec2 Solve(b2Vec2 b)
        {
            float a11 = ex.x, a12 = ey.x, a21 = ex.y, a22 = ey.y;
            float det = a11 * a22 - a12 * a21;
            if (det != 0.0f)
            {
                det = 1.0f / det;
            }

            b2Vec2 x;
            x.x = det * (a22 * b.x - a12 * b.y);
            x.y = det * (a11 * b.y - a21 * b.x);
            return x;
        }

        public b2Vec2 ex, ey;
    }
}
