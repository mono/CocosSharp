/*
* Copyright (c) 2007-2011 Erin Catto http://www.box2d.org
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
// Gear Joint:
// C0 = (coordinate1 + ratio * coordinate2)_initial
// C = (coordinate1 + ratio * coordinate2) - C0 = 0
// J = [J1 ratio * J2]
// K = J * invM * JT
//   = J1 * invM1 * J1T + ratio * ratio * J2 * invM2 * J2T
//
// Revolute:
// coordinate = rotation
// Cdot = angularVelocity
// J = [0 0 1]
// K = J * invM * JT = invI
//
// Prismatic:
// coordinate = dot(p - pg, ug)
// Cdot = dot(v + cross(w, r), ug)
// J = [ug cross(r, ug)]
// K = J * invM * JT = invMass + invI * cross(r, ug)^2
using System;
using System.Diagnostics;
using Box2D.Common;

namespace Box2D.Dynamics.Joints
{
    /// A gear joint is used to connect two joints together. Either joint
    /// can be a revolute or prismatic joint. You specify a gear ratio
    /// to bind the motions together:
    /// coordinate1 + ratio * coordinate2 = constant
    /// The ratio can be negative or positive. If one joint is a revolute joint
    /// and the other joint is a prismatic joint, then the ratio will have units
    /// of length or units of 1/length.
    /// @warning You have to manually destroy the gear joint if joint1 or joint2
    /// is destroyed.
    public class b2GearJoint : b2Joint
    {
        protected b2Joint m_joint1;
        protected b2Joint m_joint2;

        protected b2JointType m_typeA;
        protected b2JointType m_typeB;

        // Body A is connected to body C
        // Body B is connected to body D
        protected b2Body m_bodyC;
        protected b2Body m_bodyD;

        // Solver shared
        protected b2Vec2 m_localAnchorA;
        protected b2Vec2 m_localAnchorB;
        protected b2Vec2 m_localAnchorC;
        protected b2Vec2 m_localAnchorD;

        protected b2Vec2 m_localAxisC;
        protected b2Vec2 m_localAxisD;

        protected float m_referenceAngleA;
        protected float m_referenceAngleB;

        protected float m_constant;
        protected float m_ratio;

        protected float m_impulse;

        // Solver temp
        protected int m_indexA, m_indexB, m_indexC, m_indexD;
        protected b2Vec2 m_lcA, m_lcB, m_lcC, m_lcD;
        protected float m_mA, m_mB, m_mC, m_mD;
        protected float m_iA, m_iB, m_iC, m_iD;
        protected b2Vec2 m_JvAC, m_JvBD;
        protected float m_JwA, m_JwB, m_JwC, m_JwD;
        protected float m_mass;

        public b2GearJoint(b2GearJointDef def)
            : base(def)
        {
            m_joint1 = def.joint1;
            m_joint2 = def.joint2;

            m_typeA = m_joint1.GetJointType();
            m_typeB = m_joint2.GetJointType();

            Debug.Assert(m_typeA == b2JointType.e_revoluteJoint || m_typeA == b2JointType.e_prismaticJoint);
            Debug.Assert(m_typeB == b2JointType.e_revoluteJoint || m_typeB == b2JointType.e_prismaticJoint);

            float coordinateA, coordinateB;

            // TODO_ERIN there might be some problem with the joint edges in b2Joint.

            m_bodyC = m_joint1.GetBodyA();
            m_bodyA = m_joint1.GetBodyB();

            // Get geometry of joint1
            b2Transform xfA = m_bodyA.XF;
            float aA = m_bodyA.Sweep.a;
            b2Transform xfC = m_bodyC.XF;
            float aC = m_bodyC.Sweep.a;

            if (m_typeA == b2JointType.e_revoluteJoint)
            {
                b2RevoluteJoint revolute = (b2RevoluteJoint)def.joint1;
                m_localAnchorC = revolute.GetLocalAnchorA();
                m_localAnchorA = revolute.GetLocalAnchorB();
                m_referenceAngleA = revolute.GetReferenceAngle();
                m_localAxisC.SetZero();

                coordinateA = aA - aC - m_referenceAngleA;
            }
            else
            {
                b2PrismaticJoint prismatic = (b2PrismaticJoint)def.joint1;
                m_localAnchorC = prismatic.GetLocalAnchorA();
                m_localAnchorA = prismatic.GetLocalAnchorB();
                m_referenceAngleA = prismatic.GetReferenceAngle();
                m_localAxisC = prismatic.GetLocalXAxisA();

                b2Vec2 pC = m_localAnchorC;
                b2Vec2 pA = b2Math.b2MulT(xfC.q, b2Math.b2Mul(xfA.q, m_localAnchorA) + (xfA.p - xfC.p));
                coordinateA = b2Math.b2Dot(pA - pC, m_localAxisC);
            }

            m_bodyD = m_joint2.GetBodyA();
            m_bodyB = m_joint2.GetBodyB();

            // Get geometry of joint2
            b2Transform xfB = m_bodyB.XF;
            float aB = m_bodyB.Sweep.a;
            b2Transform xfD = m_bodyD.XF;
            float aD = m_bodyD.Sweep.a;

            if (m_typeB == b2JointType.e_revoluteJoint)
            {
                b2RevoluteJoint revolute = (b2RevoluteJoint)def.joint2;
                m_localAnchorD = revolute.GetLocalAnchorA();
                m_localAnchorB = revolute.GetLocalAnchorB();
                m_referenceAngleB = revolute.GetReferenceAngle();
                m_localAxisD.SetZero();

                coordinateB = aB - aD - m_referenceAngleB;
            }
            else
            {
                b2PrismaticJoint prismatic = (b2PrismaticJoint)def.joint2;
                m_localAnchorD = prismatic.GetLocalAnchorA();
                m_localAnchorB = prismatic.GetLocalAnchorB();
                m_referenceAngleB = prismatic.GetReferenceAngle();
                m_localAxisD = prismatic.GetLocalXAxisA();

                b2Vec2 pD = m_localAnchorD;
                b2Vec2 pB = b2Math.b2MulT(xfD.q, b2Math.b2Mul(xfB.q, m_localAnchorB) + (xfB.p - xfD.p));
                coordinateB = b2Math.b2Dot(pB - pD, m_localAxisD);
            }

            m_ratio = def.ratio;

            m_constant = coordinateA + m_ratio * coordinateB;

            m_impulse = 0.0f;
        }
        /// Get the first joint.
        public virtual b2Joint GetJoint1() { return m_joint1; }

        /// Get the second joint.
        public virtual b2Joint GetJoint2() { return m_joint2; }

        public override void InitVelocityConstraints(b2SolverData data)
        {
            m_indexA = m_bodyA.IslandIndex;
            m_indexB = m_bodyB.IslandIndex;
            m_indexC = m_bodyC.IslandIndex;
            m_indexD = m_bodyD.IslandIndex;
            m_lcA = m_bodyA.Sweep.localCenter;
            m_lcB = m_bodyB.Sweep.localCenter;
            m_lcC = m_bodyC.Sweep.localCenter;
            m_lcD = m_bodyD.Sweep.localCenter;
            m_mA = m_bodyA.InvertedMass;
            m_mB = m_bodyB.InvertedMass;
            m_mC = m_bodyC.InvertedMass;
            m_mD = m_bodyD.InvertedMass;
            m_iA = m_bodyA.InvertedI;
            m_iB = m_bodyB.InvertedI;
            m_iC = m_bodyC.InvertedI;
            m_iD = m_bodyD.InvertedI;

            b2Vec2 cA = m_bodyA.InternalPosition.c;
            float aA = m_bodyA.InternalPosition.a;
            b2Vec2 vA = m_bodyA.InternalVelocity.v;
            float wA = m_bodyA.InternalVelocity.w;

            b2Vec2 cB = m_bodyB.InternalPosition.c;
            float aB = m_bodyB.InternalPosition.a;
            b2Vec2 vB = m_bodyB.InternalVelocity.v;
            float wB = m_bodyB.InternalVelocity.w;

            b2Vec2 cC = m_bodyC.InternalPosition.c;
            float aC = m_bodyC.InternalPosition.a;
            b2Vec2 vC = m_bodyC.InternalVelocity.v;
            float wC = m_bodyC.InternalVelocity.w;

            b2Vec2 cD = m_bodyD.InternalPosition.c;
            float aD = m_bodyD.InternalPosition.a;
            b2Vec2 vD = m_bodyD.InternalVelocity.v;
            float wD = m_bodyD.InternalVelocity.w;

            b2Rot qA = new b2Rot(aA);
            b2Rot qB = new b2Rot(aB);
            b2Rot qC = new b2Rot(aC);
            b2Rot qD = new b2Rot(aD);

            m_mass = 0.0f;

            if (m_typeA == b2JointType.e_revoluteJoint)
            {
                m_JvAC.SetZero();
                m_JwA = 1.0f;
                m_JwC = 1.0f;
                m_mass += m_iA + m_iC;
            }
            else
            {
                b2Vec2 u = b2Math.b2Mul(qC, m_localAxisC);
                b2Vec2 rC = b2Math.b2Mul(qC, m_localAnchorC - m_lcC);
                b2Vec2 rA = b2Math.b2Mul(qA, m_localAnchorA - m_lcA);
                m_JvAC = u;
                m_JwC = b2Math.b2Cross(rC, u);
                m_JwA = b2Math.b2Cross(rA, u);
                m_mass += m_mC + m_mA + m_iC * m_JwC * m_JwC + m_iA * m_JwA * m_JwA;
            }

            if (m_typeB == b2JointType.e_revoluteJoint)
            {
                m_JvBD.SetZero();
                m_JwB = m_ratio;
                m_JwD = m_ratio;
                m_mass += m_ratio * m_ratio * (m_iB + m_iD);
            }
            else
            {
                b2Vec2 u = b2Math.b2Mul(qD, m_localAxisD);
                b2Vec2 rD = b2Math.b2Mul(qD, m_localAnchorD - m_lcD);
                b2Vec2 rB = b2Math.b2Mul(qB, m_localAnchorB - m_lcB);
                m_JvBD = m_ratio * u;
                m_JwD = m_ratio * b2Math.b2Cross(rD, u);
                m_JwB = m_ratio * b2Math.b2Cross(rB, u);
                m_mass += m_ratio * m_ratio * (m_mD + m_mB) + m_iD * m_JwD * m_JwD + m_iB * m_JwB * m_JwB;
            }

            // Compute effective mass.
            m_mass = m_mass > 0.0f ? 1.0f / m_mass : 0.0f;

            if (data.step.warmStarting)
            {
                vA += (m_mA * m_impulse) * m_JvAC;
                wA += m_iA * m_impulse * m_JwA;
                vB += (m_mB * m_impulse) * m_JvBD;
                wB += m_iB * m_impulse * m_JwB;
                vC -= (m_mC * m_impulse) * m_JvAC;
                wC -= m_iC * m_impulse * m_JwC;
                vD -= (m_mD * m_impulse) * m_JvBD;
                wD -= m_iD * m_impulse * m_JwD;
            }
            else
            {
                m_impulse = 0.0f;
            }

            m_bodyA.InternalVelocity.v = vA;
            m_bodyA.InternalVelocity.w = wA;
            m_bodyB.InternalVelocity.v = vB;
            m_bodyB.InternalVelocity.w = wB;
            m_bodyC.InternalVelocity.v = vC;
            m_bodyC.InternalVelocity.w = wC;
            m_bodyD.InternalVelocity.v = vD;
            m_bodyD.InternalVelocity.w = wD;
        }

        public override void SolveVelocityConstraints(b2SolverData data)
        {
            b2Vec2 vA = m_bodyA.InternalVelocity.v;
            float wA = m_bodyA.InternalVelocity.w;
            b2Vec2 vB = m_bodyB.InternalVelocity.v;
            float wB = m_bodyB.InternalVelocity.w;
            b2Vec2 vC = m_bodyC.InternalVelocity.v;
            float wC = m_bodyC.InternalVelocity.w;
            b2Vec2 vD = m_bodyD.InternalVelocity.v;
            float wD = m_bodyD.InternalVelocity.w;

            float Cdot = b2Math.b2Dot(m_JvAC, vA - vC) + b2Math.b2Dot(m_JvBD, vB - vD);
            Cdot += (m_JwA * wA - m_JwC * wC) + (m_JwB * wB - m_JwD * wD);

            float impulse = -m_mass * Cdot;
            m_impulse += impulse;

            vA += (m_mA * impulse) * m_JvAC;
            wA += m_iA * impulse * m_JwA;
            vB += (m_mB * impulse) * m_JvBD;
            wB += m_iB * impulse * m_JwB;
            vC -= (m_mC * impulse) * m_JvAC;
            wC -= m_iC * impulse * m_JwC;
            vD -= (m_mD * impulse) * m_JvBD;
            wD -= m_iD * impulse * m_JwD;

            m_bodyA.InternalVelocity.v = vA;
            m_bodyA.InternalVelocity.w = wA;
            m_bodyB.InternalVelocity.v = vB;
            m_bodyB.InternalVelocity.w = wB;
            m_bodyC.InternalVelocity.v = vC;
            m_bodyC.InternalVelocity.w = wC;
            m_bodyD.InternalVelocity.v = vD;
            m_bodyD.InternalVelocity.w = wD;
        }

        public override bool SolvePositionConstraints(b2SolverData data)
        {
            b2Vec2 cA = m_bodyA.InternalPosition.c;
            float aA = m_bodyA.InternalPosition.a;
            b2Vec2 cB = m_bodyB.InternalPosition.c;
            float aB = m_bodyB.InternalPosition.a;
            b2Vec2 cC = m_bodyC.InternalPosition.c;
            float aC = m_bodyC.InternalPosition.a;
            b2Vec2 cD = m_bodyD.InternalPosition.c;
            float aD = m_bodyD.InternalPosition.a;

            b2Rot qA = new b2Rot(aA);
            b2Rot qB = new b2Rot(aB);
            b2Rot qC = new b2Rot(aC);
            b2Rot qD = new b2Rot(aD);

            float linearError = 0.0f;

            float coordinateA, coordinateB;

            b2Vec2 JvAC = b2Vec2.Zero, JvBD = b2Vec2.Zero;
            float JwA, JwB, JwC, JwD;
            float mass = 0.0f;

            if (m_typeA == b2JointType.e_revoluteJoint)
            {
                JvAC.SetZero();
                JwA = 1.0f;
                JwC = 1.0f;
                mass += m_iA + m_iC;

                coordinateA = aA - aC - m_referenceAngleA;
            }
            else
            {
                b2Vec2 u = b2Math.b2Mul(qC, m_localAxisC);
                b2Vec2 rC = b2Math.b2Mul(qC, m_localAnchorC - m_lcC);
                b2Vec2 rA = b2Math.b2Mul(qA, m_localAnchorA - m_lcA);
                JvAC = u;
                JwC = b2Math.b2Cross(rC, u);
                JwA = b2Math.b2Cross(rA, u);
                mass += m_mC + m_mA + m_iC * JwC * JwC + m_iA * JwA * JwA;

                b2Vec2 pC = m_localAnchorC - m_lcC;
                b2Vec2 pA = b2Math.b2MulT(qC, rA + (cA - cC));
                coordinateA = b2Math.b2Dot(pA - pC, m_localAxisC);
            }

            if (m_typeB == b2JointType.e_revoluteJoint)
            {
                JvBD.SetZero();
                JwB = m_ratio;
                JwD = m_ratio;
                mass += m_ratio * m_ratio * (m_iB + m_iD);

                coordinateB = aB - aD - m_referenceAngleB;
            }
            else
            {
                b2Vec2 u = b2Math.b2Mul(qD, m_localAxisD);
                b2Vec2 rD = b2Math.b2Mul(qD, m_localAnchorD - m_lcD);
                b2Vec2 rB = b2Math.b2Mul(qB, m_localAnchorB - m_lcB);
                JvBD = m_ratio * u;
                JwD = m_ratio * b2Math.b2Cross(rD, u);
                JwB = m_ratio * b2Math.b2Cross(rB, u);
                mass += m_ratio * m_ratio * (m_mD + m_mB) + m_iD * JwD * JwD + m_iB * JwB * JwB;

                b2Vec2 pD = m_localAnchorD - m_lcD;
                b2Vec2 pB = b2Math.b2MulT(qD, rB + (cB - cD));
                coordinateB = b2Math.b2Dot(pB - pD, m_localAxisD);
            }

            float C = (coordinateA + m_ratio * coordinateB) - m_constant;

            float impulse = 0.0f;
            if (mass > 0.0f)
            {
                impulse = -C / mass;
            }

            cA += m_mA * impulse * JvAC;
            aA += m_iA * impulse * JwA;
            cB += m_mB * impulse * JvBD;
            aB += m_iB * impulse * JwB;
            cC -= m_mC * impulse * JvAC;
            aC -= m_iC * impulse * JwC;
            cD -= m_mD * impulse * JvBD;
            aD -= m_iD * impulse * JwD;

            m_bodyA.InternalPosition.c = cA;
            m_bodyA.InternalPosition.a = aA;
            m_bodyB.InternalPosition.c = cB;
            m_bodyB.InternalPosition.a = aB;
            m_bodyC.InternalPosition.c = cC;
            m_bodyC.InternalPosition.a = aC;
            m_bodyD.InternalPosition.c = cD;
            m_bodyD.InternalPosition.a = aD;

            // TODO_ERIN not implemented
            return linearError < b2Settings.b2_linearSlop;
        }

        public override b2Vec2 GetAnchorA()
        {
            return m_bodyA.GetWorldPoint(m_localAnchorA);
        }

        public override b2Vec2 GetAnchorB()
        {
            return m_bodyB.GetWorldPoint(m_localAnchorB);
        }

        public virtual b2Vec2 GetReactionForce(float inv_dt)
        {
            b2Vec2 P = m_impulse * m_JvAC;
            return inv_dt * P;
        }

        public virtual float GetReactionTorque(float inv_dt)
        {
            float L = m_impulse * m_JwA;
            return inv_dt * L;
        }

        public virtual void SetRatio(float ratio)
        {
            Debug.Assert(b2Math.b2IsValid(ratio));
            m_ratio = ratio;
        }

        public virtual float GetRatio()
        {
            return m_ratio;
        }

        public override void Dump()
        {
            int indexA = m_bodyA.IslandIndex;
            int indexB = m_bodyB.IslandIndex;

            int index1 = m_joint1.Index;
            int index2 = m_joint2.Index;

            b2Settings.b2Log("  b2GearJointDef jd;\n");
            b2Settings.b2Log("  jd.bodyA = bodies[{0}];\n", indexA);
            b2Settings.b2Log("  jd.bodyB = bodies[{0}];\n", indexB);
            b2Settings.b2Log("  jd.collideConnected = bool({0});\n", m_collideConnected);
            b2Settings.b2Log("  jd.joint1 = joints[{0}];\n", index1);
            b2Settings.b2Log("  jd.joint2 = joints[{0}];\n", index2);
            b2Settings.b2Log("  jd.ratio = {0:F5};\n", m_ratio);
            b2Settings.b2Log("  joints[{0}] = m_world.CreateJoint(&jd);\n", m_index);
        }
    }
}