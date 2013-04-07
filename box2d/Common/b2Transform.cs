using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box2D.Common
{
    /// A transform contains translation and rotation. It is used to represent
    /// the position and orientation of rigid frames.
    public struct b2Transform
    {

        public static b2Transform Default = b2Transform.Create();
        
        public static b2Transform Create()
        {
            var transform = b2Transform.Default;
            transform.p = b2Vec2.Zero;
            transform.q = b2Rot.Default;
            return transform;
        }
        /// The default ructor does nothing.
        /*public b2Transform()
        {
            _p = new b2Vec2();
            _q = new b2Rot();
        }*/

        /// Initialize using a position vector and a rotation.
        b2Transform(b2Vec2 position, b2Rot rotation)
        {
            _p = position;
            _q = rotation;
        }

        /// Set this to the identity transform.
        void SetIdentity()
        {
            _p.SetZero();
            _q.SetIdentity();
        }

        /// Set this based on the position and angle.
        void Set(b2Vec2 position, float angle)
        {
            _p = position;
            _q.Set(angle);
        }

        public b2Vec2 p { get { return (_p); } set { _p = value; } }
        public b2Rot q { get { return (_q); } set { _q = value; } }

        private b2Vec2 _p;
        private b2Rot _q;
    }
}
