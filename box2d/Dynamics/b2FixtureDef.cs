using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision.Shapes;
using Box2D.Collision;

namespace Box2D.Dynamics
{
    /// This proxy is used internally to connect fixtures to the broad-phase.
    public struct b2FixtureProxy
    {
        public b2AABB aabb;
        public b2Fixture fixture;
        public int childIndex;
        public int proxyId;
    }

    /// <summary>
    /// A fixture definition is used to create a fixture. This class defines an
    /// abstract fixture definition. You can reuse fixture definitions safely.
    /// </summary>
    public class b2FixtureDef
    {

        public b2FixtureDef()
        {
            Defaults();
        }

        public void Defaults() 
        {
            shape = null;
            userData = null;
            friction = 0.2f;
            restitution = 0.0f;
            density = 0.0f;
            isSensor = false;
        }

        /// The shape, this must be set. The shape will be cloned, so you
        /// can create the shape on the stack.
        public b2Shape shape;

        /// Use this to store application specific fixture data.
        public object userData;

        /// The friction coefficient, usually in the range [0,1].
        public float friction;

        /// The restitution (elasticity) usually in the range [0,1].
        public float restitution;

        /// The density, usually in kg/m^2.
        public float density;

        /// A sensor shape collects contact information but never generates a collision
        /// response.
        public bool isSensor;

        /// Contact filtering data.
        public b2Filter filter = b2Filter.Default;
    }
}
