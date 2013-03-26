using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;

namespace Box2D.Dynamics.Joints
{
    public class b2FrictionJointDef : b2JointDef
    {
        public void Initialize(b2Body bA, b2Body bB, b2Vec2 anchor)
        {
            bodyA = bA;
            bodyB = bB;
            localAnchorA = bodyA.GetLocalPoint(anchor);
            localAnchorB = bodyB.GetLocalPoint(anchor);
        }
        /// The local anchor point relative to bodyA's origin.
        public b2Vec2 localAnchorA;

        /// The local anchor point relative to bodyB's origin.
        public b2Vec2 localAnchorB;

        /// The natural length between the anchor points.
        public float length;

        /// The mass-spring-damper frequency in Hertz. A value of 0
        /// disables softness.
        public float frequencyHz;

        /// The damping ratio. 0 = no damping, 1 = critical damping.
        public float dampingRatio;
    }
}
