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

// Limit:
// C = norm(pB - pA) - L
// u = (pB - pA) / norm(pB - pA)
// Cdot = dot(u, vB + cross(wB, rB) - vA - cross(wA, rA))
// J = [-u -cross(rA, u) u cross(rB, u)]
// K = J * invM * JT
//   = invMassA + invIA * cross(rA, u)^2 + invMassB + invIB * cross(rB, u)^2
using System;
using System.Diagnostics;
using Box2D.Common;

namespace Box2D.Dynamics.Joints
{

    /// A rope joint enforces a maximum distance between two points
    /// on two bodies. It has no other effect.
    /// Warning: if you attempt to change the maximum length during
    /// the simulation you will get some non-physical behavior.
    /// A model that would allow you to dynamically modify the length
    /// would have some sponginess, so I chose not to implement it
    /// that way. See b2DistanceJoint if you want to dynamically
    /// control length.
    public class b2RopeJoint : b2Joint
    {
        // Solver shared
        protected b2Vec2 m_localAnchorA;
        protected b2Vec2 m_localAnchorB;
        protected float m_maxLength;
        protected float m_length;
        protected float m_impulse;

        // Solver temp
        protected int m_indexA;
        protected int m_indexB;
        protected b2Vec2 m_u;
        protected b2Vec2 m_rA;
        protected b2Vec2 m_rB;
        protected b2Vec2 m_localCenterA;
        protected b2Vec2 m_localCenterB;
        protected float m_invMassA;
        protected float m_invMassB;
        protected float m_invIA;
        protected float m_invIB;
        protected float m_mass;
        protected b2LimitState m_state;

        /// The local anchor point relative to bodyA's origin.
        public b2Vec2 GetLocalAnchorA() { return m_localAnchorA; }

        /// The local anchor point relative to bodyB's origin.
        public b2Vec2 GetLocalAnchorB() { return m_localAnchorB; }

        /// Set/Get the maximum length of the rope.
        public void SetMaxLength(float length) { m_maxLength = length; }

        public b2RopeJoint(b2RopeJointDef def)
            : base(def)
        {
            m_localAnchorA = def.localAnchorA;
            m_localAnchorB = def.localAnchorB;

            m_maxLength = def.maxLength;

            m_mass = 0.0f;
            m_impulse = 0.0f;
            m_state = b2LimitState.e_inactiveLimit;
            m_length = 0.0f;
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
            m_u = cB + m_rB - cA - m_rA;

            m_length = m_u.Length;

            float C = m_length - m_maxLength;
            if (C > 0.0f)
            {
                m_state = b2LimitState.e_atUpperLimit;
            }
            else
            {
                m_state = b2LimitState.e_inactiveLimit;
            }

            if (m_length > b2Settings.b2_linearSlop)
            {
                m_u *= 1.0f / m_length;
            }
            else
            {
                m_u.SetZero();
                m_mass = 0.0f;
                m_impulse = 0.0f;
                return;
            }

            // Compute effective mass.
            float crA = b2Math.b2Cross(m_rA, m_u);
            float crB = b2Math.b2Cross(m_rB, m_u);
            float invMass = m_invMassA + m_invIA * crA * crA + m_invMassB + m_invIB * crB * crB;

            m_mass = invMass != 0.0f ? 1.0f / invMass : 0.0f;

            if (data.step.warmStarting)
            {
                // Scale the impulse to support a variable time step.
                m_impulse *= data.step.dtRatio;

                b2Vec2 P = m_impulse * m_u;
                vA -= m_invMassA * P;
                wA -= m_invIA * b2Math.b2Cross(m_rA, P);
                vB += m_invMassB * P;
                wB += m_invIB * b2Math.b2Cross(m_rB, P);
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

            // Cdot = dot(u, v + cross(w, r))
            b2Vec2 vpA = vA + b2Math.b2Cross(wA, ref m_rA);
            b2Vec2 vpB = vB + b2Math.b2Cross(wB, ref m_rB);
            float C = m_length - m_maxLength;
            float Cdot = b2Math.b2Dot(m_u, vpB - vpA);

            // Predictive constraint.
            if (C < 0.0f)
            {
                Cdot += data.step.inv_dt * C;
            }

            float impulse = -m_mass * Cdot;
            float oldImpulse = m_impulse;
            m_impulse = Math.Min(0.0f, m_impulse + impulse);
            impulse = m_impulse - oldImpulse;

            b2Vec2 P = impulse * m_u;
            vA -= m_invMassA * P;
            wA -= m_invIA * b2Math.b2Cross(m_rA, P);
            vB += m_invMassB * P;
            wB += m_invIB * b2Math.b2Cross(m_rB, P);

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
    b2Vec2 u = cB + rB - cA - rA;

    float length = u.Normalize();
    float C = length - m_maxLength;

    C = b2Math.b2Clamp(C, 0.0f, b2Settings.b2_maxLinearCorrection);

    float impulse = -m_mass * C;
    b2Vec2 P = impulse * u;

    cA -= m_invMassA * P;
    aA -= m_invIA * b2Math.b2Cross(rA, P);
    cB += m_invMassB * P;
    aB += m_invIB * b2Math.b2Cross(rB, P);

    m_bodyA.InternalPosition.c = cA;
    m_bodyA.InternalPosition.a = aA;
    m_bodyB.InternalPosition.c = cB;
    m_bodyB.InternalPosition.a = aB;

    return length - m_maxLength < b2Settings.b2_linearSlop;
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
            b2Vec2 F = (inv_dt * m_impulse) * m_u;
            return F;
        }

        public virtual float GetReactionTorque(float inv_dt)
        {
            return 0.0f;
        }

        public virtual float GetMaxLength()
        {
            return m_maxLength;
        }

        public virtual b2LimitState GetLimitState()
        {
            return m_state;
        }

        public override void Dump()
        {
            int indexA = m_bodyA.IslandIndex;
            int indexB = m_bodyB.IslandIndex;

            b2Settings.b2Log("  b2RopeJointDef jd;\n");
            b2Settings.b2Log("  jd.bodyA = bodies[{0}];\n", indexA);
            b2Settings.b2Log("  jd.bodyB = bodies[{0}];\n", indexB);
            b2Settings.b2Log("  jd.collideConnected = bool({0});\n", m_collideConnected);
            b2Settings.b2Log("  jd.localAnchorA.Set({0:F5}, {1:F5});\n", m_localAnchorA.x, m_localAnchorA.y);
            b2Settings.b2Log("  jd.localAnchorB.Set({0:F5}, {1:F5});\n", m_localAnchorB.x, m_localAnchorB.y);
            b2Settings.b2Log("  jd.maxLength = {0:F5};\n", m_maxLength);
            b2Settings.b2Log("  joints[{0}] = m_world.CreateJoint(&jd);\n", m_index);
        }
    }
}