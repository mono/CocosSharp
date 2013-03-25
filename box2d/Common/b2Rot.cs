using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box2D.Common
{
    /// Rotation
    public struct b2Rot
    {
        public float s { get { return (_s); } set { _s = value; } }
        public float c { get { return (_c); } set { _c = value; } }

        /*public b2Rot() 
        {
            _s = 0f; // sine of zero
            _c = 1f; // cosine of zero
        }*/

        /// Initialize from an angle in radians
        public b2Rot(float angle)
        {
            /// TODO_ERIN optimize
            _s = (float)Math.Sin(angle);
            _c = (float)Math.Cos(angle);
        }

        /// Set using an angle in radians.
        public void Set(float angle)
        {
            /// TODO_ERIN optimize
            _s = (float)Math.Sin(angle);
            _c = (float)Math.Cos(angle);
        }

        /// Set to the identity rotation
        public void SetIdentity()
        {
            _s = 0.0f;
            _c = 1.0f;
        }

        /// Get the angle in radians
        public float GetAngle()
        {
            return b2Math.b2Atan2(_s, _c);
        }

        /// Get the x-axis
        public b2Vec2 GetXAxis()
        {
            return new b2Vec2(_c, _s);
        }

        /// Get the u-axis
        public b2Vec2 GetYAxis()
        {
            return new b2Vec2(-_s, _c);
        }

        /// Sine and cosine
        private float _s, _c;
    }
}
