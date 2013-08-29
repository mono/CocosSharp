/*
* Copyright (c) 2007 Erin Catto http://www.box2d.org
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

// Pulley:
// length1 = norm(p1 - s1)
// length2 = norm(p2 - s2)
// C0 = (length1 + ratio * length2)_initial
// C = C0 - (length1 + ratio * length2)
// u1 = (p1 - s1) / norm(p1 - s1)
// u2 = (p2 - s2) / norm(p2 - s2)
// Cdot = -dot(u1, v1 + cross(w1, r1)) - ratio * dot(u2, v2 + cross(w2, r2))
// J = -[u1 cross(r1, u1) ratio * u2  ratio * cross(r2, u2)]
// K = J * invM * JT
//   = invMass1 + invI1 * cross(r1, u1)^2 + ratio^2 * (invMass2 + invI2 * cross(r2, u2)^2)

using System;
using Box2D.Common;
using System.Diagnostics;

namespace Box2D.Dynamics.Joints
{
    /// The pulley joint is connected to two bodies and two fixed ground points.
    /// The pulley supports a ratio such that:
    /// length1 + ratio * length2 <= constant
    /// Yes, the force transmitted is scaled by the ratio.
    /// Warning: the pulley joint can get a bit squirrelly by itself. They often
    /// work better when combined with prismatic joints. You should also cover the
    /// the anchor points with static shapes to prevent one side from going to
    /// zero length.
    public class b2PulleyJoint : b2Joint
    {
        protected b2Vec2 m_groundAnchorA;
        protected b2Vec2 m_groundAnchorB;
        protected float m_lengthA;
        protected float m_lengthB;

        // Solver shared
        protected b2Vec2 m_localAnchorA;
        protected b2Vec2 m_localAnchorB;
        protected float m_constant;
        protected float m_ratio;
        protected float m_impulse;

        // Solver temp
        protected int m_indexA;
        protected int m_indexB;
        protected b2Vec2 m_uA;
        protected b2Vec2 m_uB;
        protected b2Vec2 m_rA;
        protected b2Vec2 m_rB;
        protected b2Vec2 m_localCenterA;
        protected b2Vec2 m_localCenterB;
        protected float m_invMassA;
        protected float m_invMassB;
        protected float m_invIA;
        protected float m_invIB;
        protected float m_mass;

        public b2PulleyJoint(b2PulleyJointDef def)
            : base(def)
        {
            m_groundAnchorA = def.groundAnchorA;
            m_groundAnchorB = def.groundAnchorB;
            m_localAnchorA = def.localAnchorA;
            m_localAnchorB = def.localAnchorB;

            m_lengthA = def.lengthA;
            m_lengthB = def.lengthB;

            Debug.Assert(def.ratio != 0.0f);
            m_ratio = def.ratio;

            m_constant = def.lengthA + m_ratio * def.lengthB;

            m_impulse = 0.0f;
        }

        public override void InitVelocityConstraints(b2SolverData data)
        {
            m_indexA = m_bodyA.IslandIndex;
            m_indexB = m_bodyB.IslandIndex;
            m_localCenterA = m_bodyA.Sweep.localCenter;
            m_localCenterB = m_bodyB.Sweep.localCenter;
            m_invMassA = m_bodyA.InvertedMass;
            m_invMassB = m_bodyB.InvertedMass;
            m_invIA = m_bodyA.InvertedI;
            m_invIB = m_bodyB.InvertedI;

            b2Vec2 cA = m_bodyA.InternalPosition.c;
            float aA = m_bodyA.InternalPosition.a;
            b2Vec2 vA = m_bodyA.InternalVelocity.v;
            float wA = m_bodyA.InternalVelocity.w;

            b2Vec2 cB = m_bodyB.InternalPosition.c;
            float aB = m_bodyB.InternalPosition.a;
            b2Vec2 vB = m_bodyB.InternalVelocity.v;
            float wB = m_bodyB.InternalVelocity.w;

            b2Rot qA = new b2Rot(aA);
            b2Rot qB = new b2Rot(aB);

            m_rA = b2Math.b2Mul(qA, m_localAnchorA - m_localCenterA);
            m_rB = b2Math.b2Mul(qB, m_localAnchorB - m_localCenterB);

            // Get the pulley axes.
            m_uA = cA + m_rA - m_groundAnchorA;
            m_uB = cB + m_rB - m_groundAnchorB;

            float lengthA = m_uA.Length;
            float lengthB = m_uB.Length;

            if (lengthA > 10.0f * b2Settings.b2_linearSlop)
            {
                m_uA *= 1.0f / lengthA;
            }
            else
            {
                m_uA.SetZero();
            }

            if (lengthB > 10.0f * b2Settings.b2_linearSlop)
            {
                m_uB *= 1.0f / lengthB;
            }
            else
            {
                m_uB.SetZero();
            }

            // Compute effective mass.
            float ruA = b2Math.b2Cross(m_rA, m_uA);
            float ruB = b2Math.b2Cross(m_rB, m_uB);

            float mA = m_invMassA + m_invIA * ruA * ruA;
            float mB = m_invMassB + m_invIB * ruB * ruB;

            m_mass = mA + m_ratio * m_ratio * mB;

            if (m_mass > 0.0f)
            {
                m_mass = 1.0f / m_mass;
            }

            if (data.step.warmStarting)
            {
                // Scale impulses to support variable time steps.
                m_impulse *= data.step.dtRatio;

                // Warm starting.
                b2Vec2 PA = -(m_impulse) * m_uA;
                b2Vec2 PB = (-m_ratio * m_impulse) * m_uB;

                vA += m_invMassA * PA;
                wA += m_invIA * b2Math.b2Cross(m_rA, PA);
                vB += m_invMassB * PB;
                wB += m_invIB * b2Math.b2Cross(m_rB, PB);
            }
            else
            {
                m_impulse = 0.0f;
            }

            m_bodyA.InternalVelocity.v = vA;
            m_bodyA.InternalVelocity.w = wA;
            m_bodyB.InternalVelocity.v = vB;
            m_bodyB.InternalVelocity.w = wB;
        }

        public override void SolveVelocityConstraints(b2SolverData data)
        {
            b2Vec2 vA = m_bodyA.InternalVelocity.v;
            float wA = m_bodyA.InternalVelocity.w;
            b2Vec2 vB = m_bodyB.InternalVelocity.v;
            float wB = m_bodyB.InternalVelocity.w;

            b2Vec2 vpA = vA + b2Math.b2Cross(wA, ref m_rA);
            b2Vec2 vpB = vB + b2Math.b2Cross(wB, ref m_rB);

            float Cdot = -b2Math.b2Dot(m_uA, vpA) - m_ratio * b2Math.b2Dot(m_uB, vpB);
            float impulse = -m_mass * Cdot;
            m_impulse += impulse;

            b2Vec2 PA = -impulse * m_uA;
            b2Vec2 PB = -m_ratio * impulse * m_uB;
            vA += m_invMassA * PA;
            wA += m_invIA * b2Math.b2Cross(m_rA, PA);
            vB += m_invMassB * PB;
            wB += m_invIB * b2Math.b2Cross(m_rB, PB);

            m_bodyA.InternalVelocity.v = vA;
            m_bodyA.InternalVelocity.w = wA;
            m_bodyB.InternalVelocity.v = vB;
            m_bodyB.InternalVelocity.w = wB;
        }

        public override bool SolvePositionConstraints(b2SolverData data)
        {
            b2Vec2 cA = m_bodyA.InternalPosition.c;
            float aA = m_bodyA.InternalPosition.a;
            b2Vec2 cB = m_bodyB.InternalPosition.c;
            float aB = m_bodyB.InternalPosition.a;

            b2Rot qA = new b2Rot(aA);
            b2Rot qB = new b2Rot(aB);

            b2Vec2 rA = b2Math.b2Mul(qA, m_localAnchorA - m_localCenterA);
            b2Vec2 rB = b2Math.b2Mul(qB, m_localAnchorB - m_localCenterB);

            // Get the pulley axes.
            b2Vec2 uA = cA + rA - m_groundAnchorA;
            b2Vec2 uB = cB + rB - m_groundAnchorB;

            float lengthA = uA.Length;
            float lengthB = uB.Length;

            if (lengthA > 10.0f * b2Settings.b2_linearSlop)
            {
                uA *= 1.0f / lengthA;
            }
            else
            {
                uA.SetZero();
            }

            if (lengthB > 10.0f * b2Settings.b2_linearSlop)
            {
                uB *= 1.0f / lengthB;
            }
            else
            {
                uB.SetZero();
            }

            // Compute effective mass.
            float ruA = b2Math.b2Cross(rA, uA);
            float ruB = b2Math.b2Cross(rB, uB);

            float mA = m_invMassA + m_invIA * ruA * ruA;
            float mB = m_invMassB + m_invIB * ruB * ruB;

            float mass = mA + m_ratio * m_ratio * mB;

            if (mass > 0.0f)
            {
                mass = 1.0f / mass;
            }

            float C = m_constant - lengthA - m_ratio * lengthB;
            float linearError = b2Math.b2Abs(C);

            float impulse = -mass * C;

            b2Vec2 PA = -impulse * uA;
            b2Vec2 PB = -m_ratio * impulse * uB;

            cA += m_invMassA * PA;
            aA += m_invIA * b2Math.b2Cross(rA, PA);
            cB += m_invMassB * PB;
            aB += m_invIB * b2Math.b2Cross(rB, PB);

            m_bodyA.InternalPosition.c = cA;
            m_bodyA.InternalPosition.a = aA;
            m_bodyB.InternalPosition.c = cB;
            m_bodyB.InternalPosition.a = aB;

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
            b2Vec2 P = m_impulse * m_uB;
            return inv_dt * P;
        }

        public virtual float GetReactionTorque(float inv_dt)
        {
            return 0.0f;
        }

        public virtual b2Vec2 GetGroundAnchorA()
        {
            return m_groundAnchorA;
        }

        public virtual b2Vec2 GetGroundAnchorB()
        {
            return m_groundAnchorB;
        }

        public virtual float GetLengthA()
        {
            b2Vec2 p = m_bodyA.GetWorldPoint(m_localAnchorA);
            b2Vec2 s = m_groundAnchorA;
            b2Vec2 d = p - s;
            return d.Length;
        }

        public virtual float GetLengthB()
        {
            b2Vec2 p = m_bodyB.GetWorldPoint(m_localAnchorB);
            b2Vec2 s = m_groundAnchorB;
            b2Vec2 d = p - s;
            return d.Length;
        }

        public virtual float GetRatio()
        {
            return m_ratio;
        }

        public override void Dump()
        {
            int indexA = m_bodyA.IslandIndex;
            int indexB = m_bodyB.IslandIndex;

            b2Settings.b2Log("  b2PulleyJointDef jd;\n");
            b2Settings.b2Log("  jd.bodyA = bodies[{0}];\n", indexA);
            b2Settings.b2Log("  jd.bodyB = bodies[{0}];\n", indexB);
            b2Settings.b2Log("  jd.collideConnected = bool({0});\n", m_collideConnected);
            b2Settings.b2Log("  jd.groundAnchorA.Set({0:F5},{1:F5});\n", m_groundAnchorA.x, m_groundAnchorA.y);
            b2Settings.b2Log("  jd.groundAnchorB.Set({0:F5},{1:F5});\n", m_groundAnchorB.x, m_groundAnchorB.y);
            b2Settings.b2Log("  jd.localAnchorA.Set({0:F5},{1:F5});\n", m_localAnchorA.x, m_localAnchorA.y);
            b2Settings.b2Log("  jd.localAnchorB.Set({0:F5},{1:F5});\n", m_localAnchorB.x, m_localAnchorB.y);
            b2Settings.b2Log("  jd.lengthA = {0:F5};\n", m_lengthA);
            b2Settings.b2Log("  jd.lengthB = {0:F5};\n", m_lengthB);
            b2Settings.b2Log("  jd.ratio = {0:F5};\n", m_ratio);
            b2Settings.b2Log("  joints[{0}] = m_world.CreateJoint(&jd);\n", m_index);
        }
    }
}