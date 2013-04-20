using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box2D.Common
{
    /// Rotation
    public struct b2Rot
    {

        public static b2Rot Create()
        {
            var rot = new b2Rot();
            rot.SetIdentity();
            return rot;
        }

        /*public b2Rot() 
        {
            s = 0f; // sine of zero
            c = 1f; // cosine of zero
        }*/

        /// Initialize from an angle in radians
        public b2Rot(float angle)
        {
            /// TODO_ERIN optimize
            s = (float)Math.Sin(angle);
            c = (float)Math.Cos(angle);
        }

        /// Set using an angle in radians.
        public void Set(float angle)
        {
            /// TODO_ERIN optimize
            s = (float)Math.Sin(angle);
            c = (float)Math.Cos(angle);
        }

        /// Set to the identity rotation
        public void SetIdentity()
        {
            s = 0.0f;
            c = 1.0f;
        }

        /// Get the angle in radians
        public float GetAngle()
        {
            return b2Math.b2Atan2(s, c);
        }

        /// Get the x-axis
        public b2Vec2 GetXAxis()
        {
            return new b2Vec2(c, s);
        }

        /// Get the u-axis
        public b2Vec2 GetYAxis()
        {
            return new b2Vec2(-s, c);
        }

        /// Sine and cosine
        public float s, c;
    }
}
