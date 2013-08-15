using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;

namespace Box2D.Dynamics
{
    /// <summary>
    /// A body definition holds all the data needed to construct a rigid body.
    /// You can safely re-use body definitions. Shapes are added to a body after construction.
    /// </summary>
    public struct b2BodyDef
    {

		public void Defaults() 
		{
			userData = null;
			position = b2Vec2.Zero;
			position.Set(0.0f, 0.0f);
			angle = 0.0f;
			linearVelocity = b2Vec2.Zero;
			linearVelocity.Set(0.0f, 0.0f);
			angularVelocity = 0.0f;
			linearDamping = 0.0f;
			angularDamping = 0.0f;
			allowSleep = true;
			awake = true;
			fixedRotation = false;
			bullet = false;
			type = b2BodyType.b2_staticBody;
			active = true;
			gravityScale = 1.0f;
		}

		static public b2BodyDef Create() 
        {
			var bodyDef  = new b2BodyDef();
			bodyDef.Defaults();
			return bodyDef;
        }

        /// The body type: static, kinematic, or dynamic.
        /// Note: if a dynamic body would have zero mass, the mass is set to one.
        public b2BodyType type;

        /// The world position of the body. Avoid creating bodies at the origin
        /// since this can lead to many overlapping shapes.
        public b2Vec2 position;

        /// The world angle of the body in radians.
        public float angle;

        /// The linear velocity of the body's origin in world co-ordinates.
        public b2Vec2 linearVelocity;

        /// The angular velocity of the body.
        public float angularVelocity;

        /// Linear damping is use to reduce the linear velocity. The damping parameter
        /// can be larger than 1.0f but the damping effect becomes sensitive to the
        /// time step when the damping parameter is large.
        public float linearDamping;

        /// Angular damping is use to reduce the angular velocity. The damping parameter
        /// can be larger than 1.0f but the damping effect becomes sensitive to the
        /// time step when the damping parameter is large.
        public float angularDamping;

        /// Set this flag to false if this body should never fall asleep. Note that
        /// this increases CPU usage.
        public bool allowSleep;

        /// Is this body initially awake or sleeping?
        public bool awake;

        /// Should this body be prevented from rotating? Useful for characters.
        public bool fixedRotation;

        /// Is this a fast moving body that should be prevented from tunneling through
        /// other moving bodies? Note that all bodies are prevented from tunneling through
        /// kinematic and static bodies. This setting is only considered on dynamic bodies.
        /// @warning You should use this flag sparingly since it increases processing time.
        public bool bullet;

        /// Does this body start out active?
        public bool active;

        /// Use this to store application specific body data.
        public object userData;

        /// Scale the gravity applied to this body.
        public float gravityScale;
    }
}
