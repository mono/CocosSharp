using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box2D.Common
{
    /// A 2D column vector.
    public struct b2Vec2
    {
        /// Construct using coordinates.
        public b2Vec2(float x, float y)
        {
            _x = x;
            _y = y;
        }

        public float x { get { return (_x); } set { _x = value; } }
        public float y { get { return (_y); } set { _y = value; } }

        /// Set this vector to all zeros.
        public void SetZero() { _x = 0f; _y = 0f; }

        /// Set this vector to some specified coordinates.
        public void Set(float x_, float y_) { _x = x_; _y = y_; }

        /// Negate this vector.
        public static b2Vec2 operator -(b2Vec2 b)
        {
            b2Vec2 v = new b2Vec2(-b.x, -b.y);
            return v;
        }

        /// Add a vector to this vector.
        public static b2Vec2 operator +(b2Vec2 v1, b2Vec2 v2)
        {
            return (new b2Vec2(v1.x + v2.x, v1.y + v2.y));
        }

        /// Subtract a vector from this vector.
        public static b2Vec2 operator -(b2Vec2 v1, b2Vec2 v2)
        {
            return (new b2Vec2(v1.x - v2.x, v1.y - v2.y));
        }

        /// Multiply this vector by a scalar.
        public static b2Vec2 operator *(b2Vec2 v1, float a)
        {
            return (new b2Vec2(v1.x * a, v1.y * a));
        }

        /// Multiply this vector by a scalar.
        public static b2Vec2 operator *(float a, b2Vec2 v1)
        {
            return (new b2Vec2(v1.x * a, v1.y * a));
        }

        /// Get the length of this vector (the norm).
        public float Length()
        {
            return b2Math.b2Sqrt(_x * _x + _y * _y);
        }

        public static bool operator ==(b2Vec2 a, b2Vec2 b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(b2Vec2 a, b2Vec2 b)
        {
            return a.x != b.x || a.y != b.y;
        }

        /// Get the length squared. For performance, use this instead of
        /// b2Vec2::Length (if possible).
        public float LengthSquared()
        {
            return _x * _x + _y * _y;
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

            return length;
        }

        /// Does this vector contain finite coordinates?
        public bool IsValid()
        {
            return b2Math.b2IsValid(x) && b2Math.b2IsValid(y);
        }

        /// Get the skew vector such that dot(skew_vec, other) == cross(vec, other)
        public b2Vec2 Skew()
        {
            return new b2Vec2(-y, x);
        }

        private float _x, _y;
    }
}
