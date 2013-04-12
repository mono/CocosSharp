using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box2D.Common
{
    /// A 3D column vector.
    public struct b2Vec3
    {
        /// Construct using coordinates.
        public b2Vec3(float ix, float iy, float iz)
        {
            x = ix;
            y = iy;
            z = iz;
        }

        /// Set this vector to all zeros.
        public void SetZero() { x = 0f; y = 0f; z = 0f; }

        /// Set this vector to some specified coordinates.
        public void Set(float ix, float iy, float iz) { x = ix; y = iy; z = iz; }

        /// Negate this vector.
        public static b2Vec3 operator -(b2Vec3 b)
        {
            b2Vec3 v = new b2Vec3(-b.x, -b.y, -b.z);
            return v;
        }

        /// Add a vector to this vector.
        public static b2Vec3 operator +(b2Vec3 v1, b2Vec3 v2)
        {
            return (new b2Vec3(v1.x + v2.x, v1.y + v2.y, v1.z+v2.z));
        }

        /// Subtract a vector from this vector.
        public static b2Vec3 operator -(b2Vec3 v1, b2Vec3 v2)
        {
            return (new b2Vec3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z));
        }

        /// Multiply this vector by a scalar.
        public static b2Vec3 operator *(b2Vec3 v1, float a)
        {
            return (new b2Vec3(v1.x * a, v1.y * a, v1.z*a));
        }

        /// Multiply this vector by a scalar.
        public static b2Vec3 operator *(float a, b2Vec3 v1)
        {
            return (new b2Vec3(v1.x * a, v1.y * a, v1.z * a));
        }

        /// Get the length of this vector (the norm).
        public float Length()
        {
            return b2Math.b2Sqrt(x * x + y * y + z*z);
        }

        /// Get the length squared. For performance, use this instead of
        /// b2Vec2::Length (if possible).
        public float LengthSquared()
        {
            return x * x + y * y + z*z;
        }

        /// Convert this vector into a unit vector. Returns the length.
        public float Normalize()
        {
            float length = Length();
            if (length < b2Settings.b2_epsilon)
            {
                return 0.0f;
            }
            float invLength = 1.0f / length;
            x *= invLength;
            y *= invLength;
            z *= invLength;

            return length;
        }

        /// Does this vector contain finite coordinates?
        public bool IsValid()
        {
            return b2Math.b2IsValid(x) && b2Math.b2IsValid(y) && b2Math.b2IsValid(z);
        }

        /// Get the skew vector such that dot(skewvec, other) == cross(vec, other)
        public b2Vec3 Skew()
        {
            return new b2Vec3(-y, x, z);
        }

        public float x, y, z;
    }
}
