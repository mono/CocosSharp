using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Box2D.Dynamics.Joints
{
    public class b2GearJointDef : b2JointDef
    {
        public b2GearJointDef()
        {
            JointType = b2JointType.e_gearJoint;
            joint1 = null;
            joint2 = null;
            ratio = 1.0f;
        }

        /// The first revolute/prismatic joint attached to the gear joint.
        public b2Joint joint1;

        /// The second revolute/prismatic joint attached to the gear joint.
        public b2Joint joint2;

        /// The gear ratio.
        /// @see b2GearJoint for explanation.
        public float ratio;
    }
}
