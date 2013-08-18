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

        public bool Equals(b2Vec2 o)
        {
            return (x == o.x && y == o.y);
        }

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
            //_bNormalized = false;
            //_Length = 0f;
           // _LengthSquared = 0f;
//            _LengthSquared = m_x * m_x + m_y * m_y;
//            _Length = b2Math.b2Sqrt(_LengthSquared);
#if DEBUG
            if (!IsValid())
            {
                System.Diagnostics.Debug.WriteLine("Invalid vector - this message is here for the sake of a breakpoint ({0},{1})", x, y);
            }
#endif
        }

        /// Set this vector to all zeros.
        public void SetZero() { x = 0f; y = 0f; /*_Length = 0f; _LengthSquared = 0f;*/ }

        /// Set this vector to some specified coordinates.
        public void Set(float x_, float y_) 
        { 
            x = x_; 
            y = y_;
            //_Length = 0f;
            //_LengthSquared = 0f;
            //_bNormalized = false;
//            _LengthSquared = m_x * m_x + m_y * m_y;
//            _Length = b2Math.b2Sqrt(_LengthSquared);
#if DEBUG
            if (!IsValid())
            {
                System.Diagnostics.Debug.WriteLine("Invalid vector - this message is here for the sake of a breakpoint Set({0},{1})", x, y);
            }
#endif
        }

        /// Negate this vector.
        public static b2Vec2 operator -(b2Vec2 b)
        {
            b2Vec2 v;
            v.x = -b.x;
            v.y = -b.y;
            return v;
        }

        /// Add a vector to this vector.
        public static b2Vec2 operator +(b2Vec2 v1, b2Vec2 v2)
        {
            b2Vec2 b;
            b.x = v1.x + v2.x;
            b.y = v1.y + v2.y;
            return (b);
        }

        /// Subtract a vector from this vector.
        public static b2Vec2 operator -(b2Vec2 v1, b2Vec2 v2)
        {
            b2Vec2 b;
            b.x = v1.x - v2.x;
            b.y = v1.y - v2.y;
            return (b);
        }

        /// Multiply this vector by a scalar.
        public static b2Vec2 operator *(b2Vec2 v1, float a)
        {
            b2Vec2 b;
            b.x = v1.x * a;
            b.y = v1.y * a;
            return (b);
        }


        /// Divide this vector by a scalar.
        public static b2Vec2 operator /(b2Vec2 v1, float a)
        {
            b2Vec2 b;
            b.x = v1.x / a;
            b.y = v1.y / a;
            return (b);
        }

        /// Multiply this vector by a scalar.
        public static b2Vec2 operator *(float a, b2Vec2 v1)
        {
            b2Vec2 b;
            b.x = v1.x * a;
            b.y = v1.y * a;
            return (b);
        }

        /// Get the length of this vector (the norm).
        [Obsolete("Use the property accessor")]
        public float GetLength()
        {
            return (float)Math.Sqrt(x * x + y * y);
        }

        public float Length
        {
            get { return (float) Math.Sqrt(x * x + y * y); }
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
        [Obsolete("Use the property accessor instead")]
        public float GetLengthSquared()
        {
            return x * x + y * y;
        }

        public float LengthSquared
        {
            get { return x * x + y * y; }
        }

        /// Convert this vector into a unit vector. Returns the length.
        public float Normalize()
        {
            var length = (float)Math.Sqrt(x * x + y * y);
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

        /// <summary>
        /// The mathematical equivalent of this cross with 1.0f
        /// </summary>
        /// <returns></returns>
        public b2Vec2 UnitCross()
        {
            b2Vec2 b;
            b.x = y;
            b.y = -x;
            return (b);
        }
        /// <summary>
        /// The mathematical equivalent of 1 cross with this
        /// </summary>
        /// <returns></returns>
        public b2Vec2 NegUnitCross()
        {
            b2Vec2 b;
            b.x = -y;
            b.y = x;
            return (b);
        }

        /// Get the skew vector such that dot(skewvec, other) == cross(vec, other)
        public b2Vec2 Skew()
        {
            b2Vec2 b;
            b.x = -y;
            b.y = x;
            return (b);
        }

        public override string ToString()
        {
            return String.Format("x={0} y={1}", x, y);
        }

        public float x, y;
    }
}
