using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;

namespace Box2D.Dynamics.Joints
{
    /// Wheel joint definition. This requires defining a line of
    /// motion using an axis and an anchor point. The definition uses local
    /// anchor points and a local axis so that the initial configuration
    /// can violate the constraint slightly. The joint translation is zero
    /// when the local anchor points coincide in world space. Using local
    /// anchors and a local axis helps when saving and loading a game.
    public class b2WheelJointDef : b2JointDef
    {
        public b2WheelJointDef()
        {
            JointType = b2JointType.e_wheelJoint;
            localAnchorA.SetZero();
            localAnchorB.SetZero();
            localAxisA.Set(1.0f, 0.0f);
            enableMotor = false;
            maxMotorTorque = 0.0f;
            motorSpeed = 0.0f;
            frequencyHz = 2.0f;
            dampingRatio = 0.7f;
        }

        /// Initialize the bodies, anchors, axis, and reference angle using the world
        /// anchor and world axis.
        public void Initialize(b2Body bA, b2Body bB, b2Vec2 anchor, b2Vec2 axis)
        {
            BodyA = bA;
            BodyB = bB;
            localAnchorA = BodyA.GetLocalPoint(anchor);
            localAnchorB = BodyB.GetLocalPoint(anchor);
            localAxisA = BodyA.GetLocalVector(axis);
        }

        /// The local anchor point relative to bodyA's origin.
        public b2Vec2 localAnchorA;

        /// The local anchor point relative to bodyB's origin.
        public b2Vec2 localAnchorB;

        /// The local translation axis in bodyA.
        public b2Vec2 localAxisA;

        /// Enable/disable the joint motor.
        public bool enableMotor;

        /// The maximum motor torque, usually in N-m.
        public float maxMotorTorque;

        /// The desired motor speed in radians per second.
        public float motorSpeed;

        /// Suspension frequency, zero indicates no suspension
        public float frequencyHz;

        /// Suspension damping ratio, one indicates critical damping
        public float dampingRatio;
    }
}
