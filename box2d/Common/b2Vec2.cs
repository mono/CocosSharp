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
            return (m_x == o.m_x && m_y == o.m_y);
        }

        public override bool Equals(object obj)
        {
            b2Vec2 o = (b2Vec2)obj;
            return (m_x == o.m_x && m_y == o.m_y);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// Construct using coordinates.
        public b2Vec2(float x_, float y_)
        {
            m_x = x_;
            m_y = y_;
            _bNormalized = false;
            _Length = 0f;
            _LengthSquared = 0f;
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
        public void SetZero() { m_x = 0f; m_y = 0f; _Length = 0f; _LengthSquared = 0f; }

        /// Set this vector to some specified coordinates.
        public void Set(float x_, float y_) 
        { 
            m_x = x_; 
            m_y = y_;
            _Length = 0f;
            _LengthSquared = 0f;
            _bNormalized = false;
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
            b2Vec2 v = b2Vec2.Zero;
            v.m_x = -b.m_x;
            v.m_y = -b.m_y;
            return v;
        }

        /// Add a vector to this vector.
        public static b2Vec2 operator +(b2Vec2 v1, b2Vec2 v2)
        {
            b2Vec2 b = b2Vec2.Zero;
            b.m_x = v1.m_x + v2.m_x;
            b.m_y = v1.m_y + v2.m_y;
            return (b);
        }

        /// Subtract a vector from this vector.
        public static b2Vec2 operator -(b2Vec2 v1, b2Vec2 v2)
        {
            b2Vec2 b = b2Vec2.Zero;
            b.m_x = v1.m_x - v2.m_x;
            b.m_y = v1.m_y - v2.m_y;
            return (b);
        }

        /// Multiply this vector by a scalar.
        public static b2Vec2 operator *(b2Vec2 v1, float a)
        {
            b2Vec2 b = b2Vec2.Zero;
            b.m_x = v1.m_x * a;
            b.m_y = v1.m_y * a;
            return (b);
        }


        /// Divide this vector by a scalar.
        public static b2Vec2 operator /(b2Vec2 v1, float a)
        {
            b2Vec2 b = b2Vec2.Zero;
            b.m_x = v1.m_x / a;
            b.m_y = v1.m_y / a;
            return (b);
        }

        /// Multiply this vector by a scalar.
        public static b2Vec2 operator *(float a, b2Vec2 v1)
        {
            b2Vec2 b = b2Vec2.Zero;
            b.m_x = v1.m_x * a;
            b.m_y = v1.m_y * a;
            return (b);
        }

        /// Get the length of this vector (the norm).
        [Obsolete("Use the property accessor")]
        public float GetLength()
        {
            return (_Length);
        }

        public float Length
        {
            get
            {
                if (_Length == 0f)
                {
                    _LengthSquared = m_x * m_x + m_y * m_y;
                    _Length = (float)Math.Sqrt(_LengthSquared);
                }
                return (_Length);
            }
        }

        public static bool operator ==(b2Vec2 a, b2Vec2 b)
        {
            return a.m_x == b.m_x && a.m_y == b.m_y;
        }

        public static bool operator !=(b2Vec2 a, b2Vec2 b)
        {
            return a.m_x != b.m_x || a.m_y != b.m_y;
        }

        /// Get the length squared. For performance, use this instead of
        /// b2Vec2::Length (if possible).
        [Obsolete("Use the property accessor instead")]
        public float GetLengthSquared()
        {
            if (_LengthSquared == 0f || _Length == 0f)
            {
                _LengthSquared = m_x * m_x + m_y * m_y;
                _Length = (float)Math.Sqrt(_LengthSquared);
            }
            return _LengthSquared;
        }

        public float LengthSquared
        {
            get
            {
                if (_Length == 0f || _LengthSquared == 0f)
                {
                    _LengthSquared = m_x * m_x + m_y * m_y;
                    _Length = (float)Math.Sqrt(_LengthSquared);
                }
                return (_LengthSquared);
            }
        }
        /// Convert this vector into a unit vector. Returns the length.
        public float Normalize()
        {
            if (_Length == 0f || _LengthSquared == 0f)
            {
                _LengthSquared = m_x * m_x + m_y * m_y;
                _Length = (float)Math.Sqrt(_LengthSquared);
            }
            float length = _Length;
            if (length < b2Settings.b2_epsilon)
            {
                return 0.0f;
            }
            if (!_bNormalized)
            {
                float invLength = 1.0f / length;
                m_x *= invLength;
                m_y *= invLength;
                _bNormalized = true;
            }
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
            b2Vec2 b = b2Vec2.Zero;
            b.m_x = m_y;
            b.m_y = -m_x;
            return (b);
        }
        /// <summary>
        /// The mathematical equivalent of 1 cross with this
        /// </summary>
        /// <returns></returns>
        public b2Vec2 NegUnitCross()
        {
            b2Vec2 b = b2Vec2.Zero;
            b.m_x = -m_y;
            b.m_y = m_x;
            return (b);
        }

        /// Get the skew vector such that dot(skewvec, other) == cross(vec, other)
        public b2Vec2 Skew()
        {
            b2Vec2 b = b2Vec2.Zero;
            b.m_x = -m_y;
            b.m_y = m_x;
            return (b);
        }

        public float x { get { return (m_x); } set { m_x = value; _Length = 0f; } }
        public float y { get { return (m_y); } set { m_y = value; _Length = 0f; } }

        internal float m_x, m_y;
        private bool _bNormalized;
        private float _Length, _LengthSquared;
    }
}
