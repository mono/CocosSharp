/*
* Copyright (c) 2006-2011 Erin Catto http://www.box2d.org
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

// 1-D constrained system
// m (v2 - v1) = lambda
// v2 + (beta/h) * x1 + gamma * lambda = 0, gamma has units of inverse mass.
// x2 = x1 + h * v2

// 1-D mass-damper-spring system
// m (v2 - v1) + h * d * v2 + h * k * 

// C = norm(p2 - p1) - L
// u = (p2 - p1) / norm(p2 - p1)
// Cdot = dot(u, v2 + cross(w2, r2) - v1 - cross(w1, r1))
// J = [-u -cross(r1, u) u cross(r2, u)]
// K = J * invM * JT
//   = invMass1 + invI1 * cross(r1, u)^2 + invMass2 + invI2 * cross(r2, u)^2
using System;
using Box2D.Common;

namespace Box2D.Dynamics.Joints
{
    public class b2DistanceJoint : b2Joint
    {

        private float m_frequencyHz;
        private float m_dampingRatio;
        private float m_bias;

        // Solver shared
        private b2Vec2 m_localAnchorA;
        private b2Vec2 m_localAnchorB;
        private float m_gamma;
        private float m_impulse;
        private float m_length;

        // Solver temp
        private int m_indexA;
        private int m_indexB;
        private b2Vec2 m_u;
        private b2Vec2 m_rA;
        private b2Vec2 m_rB;
        private b2Vec2 m_localCenterA;
        private b2Vec2 m_localCenterB;
        private float m_invMassA;
        private float m_invMassB;
        private float m_invIA;
        private float m_invIB;
        private float m_mass;

        public b2DistanceJoint(b2DistanceJointDef def)
            : base(def)
        {
            m_localAnchorA = def.localAnchorA;
            m_localAnchorB = def.localAnchorB;
            m_length = def.length;
            m_frequencyHz = def.frequencyHz;
            m_dampingRatio = def.dampingRatio;
            m_impulse = 0.0f;
            m_gamma = 0.0f;
            m_bias = 0.0f;
        }

        public void InitVelocityConstraints(b2SolverData data)
        {
            m_indexA = m_bodyA.m_islandIndex;
            m_indexB = m_bodyB.m_islandIndex;
            m_localCenterA = m_bodyA.m_sweep.localCenter;
            m_localCenterB = m_bodyB.m_sweep.localCenter;
            m_invMassA = m_bodyA.m_invMass;
            m_invMassB = m_bodyB.m_invMass;
            m_invIA = m_bodyA.m_invI;
            m_invIB = m_bodyB.m_invI;

            b2Vec2 cA = data.positions[m_indexA].c;
            float aA = data.positions[m_indexA].a;
            b2Vec2 vA = data.velocities[m_indexA].v;
            float wA = data.velocities[m_indexA].w;

            b2Vec2 cB = data.positions[m_indexB].c;
            float aB = data.positions[m_indexB].a;
            b2Vec2 vB = data.velocities[m_indexB].v;
            float wB = data.velocities[m_indexB].w;

            b2Rot qA = new b2Rot(aA);
            b2Rot qB = new b2Rot(aB);

            m_rA = b2Math.b2Mul(qA, m_localAnchorA - m_localCenterA);
            m_rB = b2Math.b2Mul(qB, m_localAnchorB - m_localCenterB);
            m_u = cB + m_rB - cA - m_rA;

            // Handle singularity.
            float length = m_u.Length();
            if (length > b2Settings.b2_linearSlop)
            {
                m_u *= 1.0f / length;
            }
            else
            {
                m_u.Set(0.0f, 0.0f);
            }

            float crAu = b2Math.b2Cross(m_rA, m_u);
            float crBu = b2Math.b2Cross(m_rB, m_u);
            float invMass = m_invMassA + m_invIA * crAu * crAu + m_invMassB + m_invIB * crBu * crBu;

            // Compute the effective mass matrix.
            m_mass = invMass != 0.0f ? 1.0f / invMass : 0.0f;

            if (m_frequencyHz > 0.0f)
            {
                float C = length - m_length;

                // Frequency
                float omega = 2.0f * (float)Math.PI * m_frequencyHz;

                // Damping coefficient
                float d = 2.0f * m_mass * m_dampingRatio * omega;

                // Spring stiffness
                float k = m_mass * omega * omega;

                // magic formulas
                float h = data.step.dt;
                m_gamma = h * (d + h * k);
                m_gamma = m_gamma != 0.0f ? 1.0f / m_gamma : 0.0f;
                m_bias = C * h * k * m_gamma;

                invMass += m_gamma;
                m_mass = invMass != 0.0f ? 1.0f / invMass : 0.0f;
            }
            else
            {
                m_gamma = 0.0f;
                m_bias = 0.0f;
            }

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

            data.velocities[m_indexA].v = vA;
            data.velocities[m_indexA].w = wA;
            data.velocities[m_indexB].v = vB;
            data.velocities[m_indexB].w = wB;
        }

        public virtual void SolveVelocityConstraints(b2SolverData data)
        {
            b2Vec2 vA = data.velocities[m_indexA].v;
            float wA = data.velocities[m_indexA].w;
            b2Vec2 vB = data.velocities[m_indexB].v;
            float wB = data.velocities[m_indexB].w;

            // Cdot = dot(u, v + cross(w, r))
            b2Vec2 vpA = vA + b2Math.b2Cross(wA, m_rA);
            b2Vec2 vpB = vB + b2Math.b2Cross(wB, m_rB);
            float Cdot = b2Math.b2Dot(m_u, vpB - vpA);

            float impulse = -m_mass * (Cdot + m_bias + m_gamma * m_impulse);
            m_impulse += impulse;

            b2Vec2 P = impulse * m_u;
            vA -= m_invMassA * P;
            wA -= m_invIA * b2Math.b2Cross(m_rA, P);
            vB += m_invMassB * P;
            wB += m_invIB * b2Math.b2Cross(m_rB, P);

            data.velocities[m_indexA].v = vA;
            data.velocities[m_indexA].w = wA;
            data.velocities[m_indexB].v = vB;
            data.velocities[m_indexB].w = wB;
        }

        public virtual bool SolvePositionConstraints(b2SolverData data)
        {
            if (m_frequencyHz > 0.0f)
            {
                // There is no position correction for soft distance constraints.
                return true;
            }

            b2Vec2 cA = data.positions[m_indexA].c;
            float aA = data.positions[m_indexA].a;
            b2Vec2 cB = data.positions[m_indexB].c;
            float aB = data.positions[m_indexB].a;

            b2Rot qA = new b2Rot(aA);
            b2Rot qB = new b2Rot(aB);

            b2Vec2 rA = b2Math.b2Mul(qA, m_localAnchorA - m_localCenterA);
            b2Vec2 rB = b2Math.b2Mul(qB, m_localAnchorB - m_localCenterB);
            b2Vec2 u = cB + rB - cA - rA;

            float length = u.Normalize();
            float C = length - m_length;
            C = b2Math.b2Clamp(C, -b2Settings.b2_maxLinearCorrection, b2Settings.b2_maxLinearCorrection);

            float impulse = -m_mass * C;
            b2Vec2 P = impulse * u;

            cA -= m_invMassA * P;
            aA -= m_invIA * b2Math.b2Cross(rA, P);
            cB += m_invMassB * P;
            aB += m_invIB * b2Math.b2Cross(rB, P);

            data.positions[m_indexA].c = cA;
            data.positions[m_indexA].a = aA;
            data.positions[m_indexB].c = cB;
            data.positions[m_indexB].a = aB;

            return b2Math.b2Abs(C) < b2Settings.b2_linearSlop;
        }

        public virtual b2Vec2 GetAnchorA()
        {
            return m_bodyA.GetWorldPoint(m_localAnchorA);
        }

        public virtual b2Vec2 GetAnchorB()
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

        public virtual void Dump()
        {
            int indexA = m_bodyA.m_islandIndex;
            int indexB = m_bodyB.m_islandIndex;

            System.Diagnostics.Debug.WriteLine("  b2DistanceJointDef jd;\n");
            b2Settings.b2Log("  jd.bodyA = bodies[{0}];\n", indexA);
            b2Settings.b2Log("  jd.bodyB = bodies[{0}];\n", indexB);
            b2Settings.b2Log("  jd.collideConnected = bool({0});\n", m_collideConnected);
            b2Settings.b2Log("  jd.localAnchorA.Set({0:f5}, {1:f5});\n", m_localAnchorA.x, m_localAnchorA.y);
            b2Settings.b2Log("  jd.localAnchorB.Set({0:f5}, {1:f5});\n", m_localAnchorB.x, m_localAnchorB.y);
            b2Settings.b2Log("  jd.length = {0:f5};\n", m_length);
            b2Settings.b2Log("  jd.frequencyHz = {0:f5};\n", m_frequencyHz);
            b2Settings.b2Log("  jd.dampingRatio = {0:f5};\n", m_dampingRatio);
            b2Settings.b2Log("  joints[{0}] = m_world.CreateJoint(jd);\n", m_index);
        }
    }
}
