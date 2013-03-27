using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;

namespace Box2D.Dynamics.Joints
{
    public class b2MouseJointDef : b2JointDef
    {
        public void b2MouseJointDef()
        {
            type = b2JointType.e_mouseJoint;
            target.Set(0.0f, 0.0f);
            maxForce = 0.0f;
            frequencyHz = 5.0f;
            dampingRatio = 0.7f;
        }

        /// The initial world target point. This is assumed
        /// to coincide with the body anchor initially.
        public b2Vec2 target;

        /// The maximum constraint force that can be exerted
        /// to move the candidate body. Usually you will express
        /// as some multiple of the weight (multiplier * mass * gravity).
        public float maxForce;

        /// The response speed.
        public float frequencyHz;

        /// The damping ratio. 0 = no damping, 1 = critical damping.
        public float dampingRatio;
    }
}
