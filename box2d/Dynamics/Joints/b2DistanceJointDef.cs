using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Dynamics;
using Box2D.Common;

namespace Box2D.Dynamics.Joints
{
    public class b2DistanceJointDef : b2JointDef
    {
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

        public b2DistanceJointDef()
        {
            JointType = b2JointType.e_distanceJoint;
            localAnchorA.Set(0.0f, 0.0f);
            localAnchorB.Set(0.0f, 0.0f);
            length = 1.0f;
            frequencyHz = 0.0f;
            dampingRatio = 0.0f;
        }
        public void Initialize(b2Body b1, b2Body b2,
                                    b2Vec2 anchor1, b2Vec2 anchor2)
        {
            BodyA = b1;
            BodyB = b2;
            localAnchorA = BodyA.GetLocalPoint(anchor1);
            localAnchorB = BodyB.GetLocalPoint(anchor2);
            b2Vec2 d = anchor2 - anchor1;
            length = d.Length();
        }
    }
}
