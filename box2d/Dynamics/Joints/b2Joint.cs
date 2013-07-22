/*
* Copyright (c) 2006-2007 Erin Catto http://www.box2d.org
*
* This software is provided 'as-is', without any express or implied
* warranty.  In no event will the authors be held liable for any damages
* arising from the use of this software.
* Permission is granted to anyone to use this software for any purpose,
* including commercial applications, and to alter it and redistribute it
* freely, subject to the following restrictions:
* 1. The origin of this software must not be misrepresented; you must not
* claim that you wrote the original software. If you use this software
* in a product, an acknowledgment in the product documentation would be
* appreciated but is not required.
* 2. Altered source versions must be plainly marked as such, and must not be
* misrepresented as being the original software.
* 3. This notice may not be removed or altered from any source distribution.
*/
using System;
using System.Diagnostics;
using Box2D.Common;

namespace Box2D.Dynamics.Joints
{
    public class b2Joint
    {
        protected b2JointType m_type;
        public b2Joint Prev;
        public b2Joint Next;
        public b2JointEdge m_edgeA;
        public b2JointEdge m_edgeB;
        public b2Body m_bodyA;
        public b2Body m_bodyB;

        protected int m_index;

        public bool m_islandFlag;
        protected bool m_collideConnected;

        protected object m_userData;

        public int Index
        {
            get { return (m_index); }
            set { m_index = value; }
        }

        public static b2Joint Create(b2JointDef def)
        {
            b2Joint joint = null;

            switch (def.JointType)
            {
                case b2JointType.e_distanceJoint:
                    {
                        joint = new b2DistanceJoint((b2DistanceJointDef)def);
                    }
                    break;

                case b2JointType.e_mouseJoint:
                    {
                        joint = new b2MouseJoint((b2MouseJointDef)def);
                    }
                    break;

                case b2JointType.e_prismaticJoint:
                    {
                        joint = new b2PrismaticJoint((b2PrismaticJointDef)def);
                    }
                    break;

                case b2JointType.e_revoluteJoint:
                    {
                        joint = new b2RevoluteJoint((b2RevoluteJointDef)def);
                    }
                    break;

                case b2JointType.e_pulleyJoint:
                    {
                        joint = new b2PulleyJoint((b2PulleyJointDef)def);
                    }
                    break;

                case b2JointType.e_gearJoint:
                    {
                        joint = new b2GearJoint((b2GearJointDef)def);
                    }
                    break;

                case b2JointType.e_wheelJoint:
                    {
                        joint = new b2WheelJoint((b2WheelJointDef)def);
                    }
                    break;

                case b2JointType.e_weldJoint:
                    {
                        joint = new b2WeldJoint((b2WeldJointDef)def);
                    }
                    break;

                case b2JointType.e_frictionJoint:
                    {
                        joint = new b2FrictionJoint((b2FrictionJointDef)def);
                    }
                    break;

                case b2JointType.e_ropeJoint:
                    {
                        joint = new b2RopeJoint((b2RopeJointDef)def);
                    }
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }

            return joint;
        }

        public b2Joint(b2JointDef def)
        {
            Debug.Assert(def.BodyA != def.BodyB);

            m_type = def.JointType;
            Prev = null;
            Next = null;
            m_bodyA = def.BodyA;
            m_bodyB = def.BodyB;
            m_index = 0;
            m_collideConnected = def.CollideConnected;
            m_islandFlag = false;
            m_userData = def.UserData;
            m_edgeA = new b2JointEdge();
            m_edgeB = new b2JointEdge();
        }

        public bool IsActive()
        {
            return m_bodyA.IsActive() && m_bodyB.IsActive();
        }

        public virtual b2JointType GetJointType()
        {
            return m_type;
        }

        public virtual b2Body GetBodyA()
        {
            return m_bodyA;
        }

        public virtual b2Body GetBodyB()
        {
            return m_bodyB;
        }

        public virtual b2Joint GetNext()
        {
            return Next;
        }

        public virtual object GetUserData()
        {
            return m_userData;
        }

        public virtual void SetUserData(object data)
        {
            m_userData = data;
        }

        public virtual bool GetCollideConnected()
        {
            return m_collideConnected;
        }

        public virtual void InitVelocityConstraints(b2SolverData data) { }
        public virtual void SolveVelocityConstraints(b2SolverData data) { }
        public virtual bool SolvePositionConstraints(b2SolverData data) { return (false); }
        public virtual void Dump() { }
        public virtual b2Vec2 GetAnchorA() { return(b2Vec2.Zero); }
        public virtual b2Vec2 GetAnchorB() { return (b2Vec2.Zero); }
    }
}