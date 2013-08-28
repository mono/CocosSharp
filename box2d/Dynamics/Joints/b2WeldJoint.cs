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

// Point-to-point constraint
// C = p2 - p1
// Cdot = v2 - v1
//      = v2 + cross(w2, r2) - v1 - cross(w1, r1)
// J = [-I -r1_skew I r2_skew ]
// Identity used:
// w k % (rx i + ry j) = w * (-ry i + rx j)

// Angle constraint
// C = angle2 - angle1 - referenceAngle
// Cdot = w2 - w1
// J = [0 0 -1 0 0 1]
// K = invI1 + invI2
using System;
using System.Diagnostics;
using Box2D.Common;

namespace Box2D.Dynamics.Joints
{
    /// A weld joint essentially glues two bodies together. A weld joint may
    /// distort somewhat because the island constraint solver is approximate.
    public class b2WeldJoint : b2Joint
    {
        protected float m_frequencyHz;
        protected float m_dampingRatio;
        protected float m_bias;

        // Solver shared
        protected b2Vec2 m_localAnchorA;
        protected b2Vec2 m_localAnchorB;
        protected float m_referenceAngle;
        protected float m_gamma;
        protected b2Vec3 m_impulse;

        // Solver temp
        protected int m_indexA;
        protected int m_indexB;
        protected b2Vec2 m_rA;
        protected b2Vec2 m_rB;
        protected b2Vec2 m_localCenterA;
        protected b2Vec2 m_localCenterB;
        protected float m_invMassA;
        protected float m_invMassB;
        protected float m_invIA;
        protected float m_invIB;
        protected b2Mat33 m_mass;

        public b2WeldJoint(b2WeldJointDef def)
            : base(def)
        {
            m_localAnchorA = def.localAnchorA;
            m_localAnchorB = def.localAnchorB;
            m_referenceAngle = def.referenceAngle;
            m_frequencyHz = def.frequencyHz;
            m_dampingRatio = def.dampingRatio;

            m_impulse.SetZero();
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

            // J = [-I -r1_skew I r2_skew]
            //     [ 0       -1 0       1]
            // r_skew = [-ry; rx]

            // Matlab
            // K = [ mA+r1y^2*iA+mB+r2y^2*iB,  -r1y*iA*r1x-r2y*iB*r2x,          -r1y*iA-r2y*iB]
            //     [  -r1y*iA*r1x-r2y*iB*r2x, mA+r1x^2*iA+mB+r2x^2*iB,           r1x*iA+r2x*iB]
            //     [          -r1y*iA-r2y*iB,           r1x*iA+r2x*iB,                   iA+iB]

            float mA = m_invMassA, mB = m_invMassB;
            float iA = m_invIA, iB = m_invIB;

            b2Vec3 ex = new b2Vec3();
            b2Vec3 ey = new b2Vec3();
            b2Vec3 ez = new b2Vec3();
            ex.x = mA + mB + m_rA.y * m_rA.y * iA + m_rB.y * m_rB.y * iB;
            ey.x = -m_rA.y * m_rA.x * iA - m_rB.y * m_rB.x * iB;
            ez.x = -m_rA.y * iA - m_rB.y * iB;
            ex.y = ey.x;
            ey.y = mA + mB + m_rA.x * m_rA.x * iA + m_rB.x * m_rB.x * iB;
            ez.y = m_rA.x * iA + m_rB.x * iB;
            ex.z = ez.x;
            ey.z = ez.y;
            ez.z = iA + iB;
            b2Mat33 K = new b2Mat33(ex, ey, ez);

            if (m_frequencyHz > 0.0f)
            {
                m_mass = K.GetInverse22(m_mass);

                float invM = iA + iB;
                float m = invM > 0.0f ? 1.0f / invM : 0.0f;

                float C = aB - aA - m_referenceAngle;

                // Frequency
                float omega = 2.0f * b2Settings.b2_pi * m_frequencyHz;

                // Damping coefficient
                float d = 2.0f * m * m_dampingRatio * omega;

                // Spring stiffness
                float k = m * omega * omega;

                // magic formulas
                float h = data.step.dt;
                m_gamma = h * (d + h * k);
                m_gamma = m_gamma != 0.0f ? 1.0f / m_gamma : 0.0f;
                m_bias = C * h * k * m_gamma;

                invM += m_gamma;
                m_mass.ezz = invM != 0.0f ? 1.0f / invM : 0.0f;
            }
            else
            {
                m_mass = K.GetSymInverse33(m_mass);
                m_gamma = 0.0f;
                m_bias = 0.0f;
            }

            if (data.step.warmStarting)
            {
                // Scale impulses to support a variable time step.
                m_impulse *= data.step.dtRatio;

                b2Vec2 P = new b2Vec2(m_impulse.x, m_impulse.y);

                vA -= mA * P;
                wA -= iA * (b2Math.b2Cross(m_rA, P) + m_impulse.z);

                vB += mB * P;
                wB += iB * (b2Math.b2Cross(m_rB, P) + m_impulse.z);
            }
            else
            {
                m_impulse.SetZero();
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

            float mA = m_invMassA, mB = m_invMassB;
            float iA = m_invIA, iB = m_invIB;

            if (m_frequencyHz > 0.0f)
            {
                float Cdot2 = wB - wA;

                float impulse2 = -m_mass.ez.z * (Cdot2 + m_bias + m_gamma * m_impulse.z);
                m_impulse.z += impulse2;

                wA -= iA * impulse2;
                wB += iB * impulse2;

                b2Vec2 Cdot1 = vB + b2Math.b2Cross(wB, ref m_rB) - vA - b2Math.b2Cross(wA, ref m_rA);

                b2Vec2 impulse1 = -b2Math.b2Mul22(m_mass, Cdot1);
                m_impulse.x += impulse1.x;
                m_impulse.y += impulse1.y;

                b2Vec2 P = impulse1;

                vA -= mA * P;
                wA -= iA * b2Math.b2Cross(ref m_rA, ref P);

                vB += mB * P;
                wB += iB * b2Math.b2Cross(ref m_rB, ref P);
            }
            else
            {
                b2Vec2 Cdot1 = vB + b2Math.b2Cross(wB, ref m_rB) - vA - b2Math.b2Cross(wA, ref m_rA);
                float Cdot2 = wB - wA;
                b2Vec3 Cdot = new b2Vec3(Cdot1.x, Cdot1.y, Cdot2);

                b2Vec3 impulse = -b2Math.b2Mul(m_mass, Cdot);
                m_impulse += impulse;

                b2Vec2 P = b2Vec2.Zero;
                P.Set(impulse.x, impulse.y);

                vA -= mA * P;
                wA -= iA * (b2Math.b2Cross(ref m_rA, ref P) + impulse.z);

                vB += mB * P;
                wB += iB * (b2Math.b2Cross(ref m_rB, ref P) + impulse.z);
            }

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

            float mA = m_invMassA, mB = m_invMassB;
            float iA = m_invIA, iB = m_invIB;

            b2Vec2 rA = b2Math.b2Mul(qA, m_localAnchorA - m_localCenterA);
            b2Vec2 rB = b2Math.b2Mul(qB, m_localAnchorB - m_localCenterB);

            float positionError, angularError;

            b2Vec3 ex = new b2Vec3();
            b2Vec3 ey = new b2Vec3();
            b2Vec3 ez = new b2Vec3();
            ex.x = mA + mB + rA.y * rA.y * iA + rB.y * rB.y * iB;
            ey.x = -rA.y * rA.x * iA - rB.y * rB.x * iB;
            ez.x = -rA.y * iA - rB.y * iB;
            ex.y = ey.x;
            ey.y = mA + mB + rA.x * rA.x * iA + rB.x * rB.x * iB;
            ez.y = rA.x * iA + rB.x * iB;
            ex.z = ez.x;
            ey.z = ez.y;
            ez.z = iA + iB;
            b2Mat33 K = new b2Mat33(ex, ey, ez);

            if (m_frequencyHz > 0.0f)
            {
                b2Vec2 C1 = cB + rB - cA - rA;

                positionError = C1.Length;
                angularError = 0.0f;

                b2Vec2 P = -K.Solve22(C1);

                cA -= mA * P;
                aA -= iA * b2Math.b2Cross(rA, P);

                cB += mB * P;
                aB += iB * b2Math.b2Cross(rB, P);
            }
            else
            {
                b2Vec2 C1 = cB + rB - cA - rA;
                float C2 = aB - aA - m_referenceAngle;

                positionError = C1.Length;
                angularError = b2Math.b2Abs(C2);

                b2Vec3 C = new b2Vec3(C1.x, C1.y, C2);

                b2Vec3 impulse = -K.Solve33(C);
                b2Vec2 P = new b2Vec2(impulse.x, impulse.y);

                cA -= mA * P;
                aA -= iA * (b2Math.b2Cross(rA, P) + impulse.z);

                cB += mB * P;
                aB += iB * (b2Math.b2Cross(rB, P) + impulse.z);
            }

            m_bodyA.InternalPosition.c = cA;
            m_bodyA.InternalPosition.a = aA;
            m_bodyB.InternalPosition.c = cB;
            m_bodyB.InternalPosition.a = aB;

            return positionError <= b2Settings.b2_linearSlop && angularError <= b2Settings.b2_angularSlop;
        }

        /// The local anchor point relative to bodyA's origin.
        public virtual b2Vec2 LocalAnchorA
        {
            get { return (m_localAnchorA); }
            set { m_localAnchorA = value; }
        }
        /// The local anchor point relative to bodyB's origin.
        public virtual b2Vec2 LocalAnchorB
        {
            get { return (m_localAnchorB); }
            set { m_localAnchorB = value; }
        }

        /// Get the reference angle.
        public virtual float ReferenceAngle
        {
            get { return (m_referenceAngle); }
            set { m_referenceAngle = value; }
        }

        /// Set/get frequency in Hz.
        public virtual float Frequency
        {
            get { return (m_frequencyHz); }
            set { m_frequencyHz = value; }
        }

        /// Set/get damping ratio.
        public float DampingRatio
        {
            get { return (m_dampingRatio); }
            set { m_dampingRatio = value; }
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
            b2Vec2 P = new b2Vec2(m_impulse.x, m_impulse.y);
            return inv_dt * P;
        }

        public virtual float GetReactionTorque(float inv_dt)
        {
            return inv_dt * m_impulse.z;
        }

        public override void Dump()
        {
            int indexA = m_bodyA.IslandIndex;
            int indexB = m_bodyB.IslandIndex;

            b2Settings.b2Log("  b2WeldJointDef jd;\n");
            b2Settings.b2Log("  jd.bodyA = bodies[{0}];\n", indexA);
            b2Settings.b2Log("  jd.bodyB = bodies[{0}];\n", indexB);
            b2Settings.b2Log("  jd.collideConnected = bool({0});\n", m_collideConnected);
            b2Settings.b2Log("  jd.localAnchorA.Set({0:F5}, {1:F5});\n", m_localAnchorA.x, m_localAnchorA.y);
            b2Settings.b2Log("  jd.localAnchorB.Set({0:F5}, {1:F5});\n", m_localAnchorB.x, m_localAnchorB.y);
            b2Settings.b2Log("  jd.referenceAngle = {0:F5};\n", m_referenceAngle);
            b2Settings.b2Log("  jd.frequencyHz = {0:F5};\n", m_frequencyHz);
            b2Settings.b2Log("  jd.dampingRatio = {0:F5};\n", m_dampingRatio);
            b2Settings.b2Log("  joints[{0}] = m_world.CreateJoint(&jd);\n", m_index);
        }
    }
}
