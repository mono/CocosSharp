using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;
using System.Diagnostics;

namespace Box2D.Dynamics.Joints
{
    /// Prismatic joint definition. This requires defining a line of
    /// motion using an axis and an anchor point. The definition uses local
    /// anchor points and a local axis so that the initial configuration
    /// can violate the constraint slightly. The joint translation is zero
    /// when the local anchor points coincide in world space. Using local
    /// anchors and a local axis helps when saving and loading a game.
    public class b2PrismaticJointDef : b2JointDef
    {
        public b2PrismaticJointDef()
        {
            JointType = b2JointType.e_prismaticJoint;
            localAnchorA.SetZero();
            localAnchorB.SetZero();
            localAxisA.Set(1.0f, 0.0f);
            referenceAngle = 0.0f;
            enableLimit = false;
            lowerTranslation = 0.0f;
            upperTranslation = 0.0f;
            enableMotor = false;
            maxMotorForce = 0.0f;
            motorSpeed = 0.0f;
        }

        /// Initialize the bodies, anchors, axis, and reference angle using the world
        /// anchor and unit world axis.
        public void Initialize(b2Body bA, b2Body bB, b2Vec2 anchor, b2Vec2 axis)
        {
            BodyA = bA;
            BodyB = bB;
            localAnchorA = BodyA.GetLocalPoint(anchor);
            localAnchorB = BodyB.GetLocalPoint(anchor);
            localAxisA = BodyA.GetLocalVector(axis);
            referenceAngle = BodyB.Angle - BodyA.Angle;
        }

        /// The local anchor point relative to bodyA's origin.
        public b2Vec2 localAnchorA;

        /// The local anchor point relative to bodyB's origin.
        public b2Vec2 localAnchorB;

        /// The local translation unit axis in bodyA.
        public b2Vec2 localAxisA;

        /// The constrained angle between the bodies: bodyB_angle - bodyA_angle.
        public float referenceAngle;

        /// Enable/disable the joint limit.
        public bool enableLimit;

        /// The lower translation limit, usually in meters.
        public float lowerTranslation;

        /// The upper translation limit, usually in meters.
        public float upperTranslation;

        /// Enable/disable the joint motor.
        public bool enableMotor;

        /// The maximum motor torque, usually in N-m.
        public float maxMotorForce;

        /// The desired motor speed in radians per second.
        public float motorSpeed;
    }
}

