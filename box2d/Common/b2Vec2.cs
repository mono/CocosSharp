using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box2D.Common
{
    /// A 2D column vector.
    public struct b2Vec2
    {
        public static b2Vec2 Zero = new b2Vec2(0f, 0f);

        public override bool Equals(object obj)
        {
            b2Vec2 o = (b2Vec2)obj;
            return (x == o.x && y == o.y);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// Construct using coordinates.
        public b2Vec2(float x_, float y_)
        {
            x = x_;
            y = y_;
#if DEBUG
            if (!IsValid())
            {
                Console.WriteLine("Invalid vector - this message is here for the sake of a breakpoint");
            }
#endif
        }

        /// Set this vector to all zeros.
        public void SetZero() { x = 0f; y = 0f; }

        /// Set this vector to some specified coordinates.
        public void Set(float x_, float y_) 
        { 
            x = x_; 
            y = y_;
#if DEBUG
            if (!IsValid())
            {
                Console.WriteLine("Invalid vector - this message is here for the sake of a breakpoint");
            }
#endif
        }

        /// Negate this vector.
        public static b2Vec2 operator -(b2Vec2 b)
        {
            b2Vec2 v = b2Vec2.Zero;
            v.x = -b.x;
            v.y = -b.y;
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
            return b2Math.b2Sqrt(x * x + y * y);
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
            return x * x + y * y;
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

        /// Get the skew vector such that dot(skewvec, other) == cross(vec, other)
        public b2Vec2 Skew()
        {
            return new b2Vec2(-y, x);
        }

        public float x, y;
    }
}
