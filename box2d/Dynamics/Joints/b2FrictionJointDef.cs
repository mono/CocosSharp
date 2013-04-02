using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;

namespace Box2D.Dynamics.Joints
{
    public class b2FrictionJointDef : b2JointDef
    {
        public b2FrictionJointDef()
        {
            JointType = b2JointType.e_frictionJoint;
            localAnchorA.SetZero();
            localAnchorB.SetZero();
            maxForce = 0.0f;
            maxTorque = 0.0f;
        }

        public void Initialize(b2Body bA, b2Body bB, b2Vec2 anchor)
        {
            BodyA = bA;
            BodyB = bB;
            localAnchorA = BodyA.GetLocalPoint(anchor);
            localAnchorB = BodyB.GetLocalPoint(anchor);
        }

        /// The local anchor point relative to bodyA's origin.
        public b2Vec2 localAnchorA;

        /// The local anchor point relative to bodyB's origin.
        public b2Vec2 localAnchorB;

        /// The maximum friction force in N.
        public float maxForce;

        /// The maximum friction torque in N-m.
        public float maxTorque;
    }
}
