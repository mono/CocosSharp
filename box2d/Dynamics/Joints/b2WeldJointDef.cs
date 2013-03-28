using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;

namespace Box2D.Dynamics.Joints
{
    /// Weld joint definition. You need to specify local anchor points
    /// where they are attached and the relative body angle. The position
    /// of the anchor points is important for computing the reaction torque.
    public class b2WeldJointDef : b2JointDef
    {
        public b2WeldJointDef()
        {
            type = b2JointType.e_weldJoint;
            localAnchorA.Set(0.0f, 0.0f);
            localAnchorB.Set(0.0f, 0.0f);
            referenceAngle = 0.0f;
            frequencyHz = 0.0f;
            dampingRatio = 0.0f;
        }

        /// Initialize the bodies, anchors, and reference angle using a world
        /// anchor point.
        public void Initialize(b2Body bA, b2Body bB, b2Vec2 anchor)
        {
            bodyA = bA;
            bodyB = bB;
            localAnchorA = bodyA.GetLocalPoint(anchor);
            localAnchorB = bodyB.GetLocalPoint(anchor);
            referenceAngle = bodyB.Angle - bodyA.Angle;
        }

        /// The local anchor point relative to bodyA's origin.
        public b2Vec2 localAnchorA;

        /// The local anchor point relative to bodyB's origin.
        public b2Vec2 localAnchorB;

        /// The bodyB angle minus bodyA angle in the reference state (radians).
        public float referenceAngle;

        /// The mass-spring-damper frequency in Hertz. Rotation only.
        /// Disable softness with a value of 0.
        public float frequencyHz;

        /// The damping ratio. 0 = no damping, 1 = critical damping.
        public float dampingRatio;
    }
}
