using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;

namespace Box2D.Dynamics.Joints
{
    [Flags]
    public enum b2JointType : short
    {
        e_unknownJoint = 1,
        e_revoluteJoint = 1 << 2,
        e_prismaticJoint = 1 << 3,
        e_distanceJoint = 1 << 4,
        e_pulleyJoint = 1 << 5,
        e_mouseJoint = 1 << 6,
        e_gearJoint = 1 << 7,
        e_wheelJoint = 1 << 8,
        e_weldJoint = 1 << 9,
        e_frictionJoint = 1 << 10,
        e_ropeJoint = 1 << 11
    };

    [Flags]
    public enum b2LimitState : short
    {
        e_inactiveLimit = 1,
        e_atLowerLimit = 1 << 1,
        e_atUpperLimit = 1 << 2,
        e_equalLimits = 1 << 3
    };

    public struct b2Jacobian
    {
        public b2Vec2 Linear;
        public float AngularA;
        public float AngularB;
    };

    /// <summary>
    /// A joint edge is used to connect bodies and joints together
    /// in a joint graph where each body is a node and each joint
    /// is an edge. A joint edge belongs to a doubly linked list
    /// maintained in each attached body. Each joint has two joint
    /// nodes, one for each attached body.
    /// </summary>
    public class b2JointEdge
    {
        /// <summary>
        /// provides quick access to the other body attached.
        /// </summary>
        public b2Body Other;
        /// <summary>
        /// the joint
        /// </summary>
        public b2Joint Joint;
        /// <summary>
        /// the previous joint edge in the body's joint list
        /// </summary>
        public b2JointEdge Prev;
        /// <summary>
        /// the next joint edge in the body's joint list
        /// </summary>
        public b2JointEdge Next;
    };


    public class b2JointDef
    {
        public b2JointDef()
        {
            JointType = b2JointType.e_unknownJoint;
            UserData = null;
            BodyA = null;
            BodyB = null;
            CollideConnected = false;
        }
        /// The joint type is set automatically for concrete joint types.
        public b2JointType JointType;

        /// Use this to attach application specific data to your joints.
        public object UserData;

        /// The first attached body.
        public b2Body BodyA;

        /// The second attached body.
        public b2Body BodyB;

        /// Set this flag to true if the attached bodies should collide.
        public bool CollideConnected;

    }
}
