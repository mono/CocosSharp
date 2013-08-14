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

// Linear raint (point-to-line)
// d = pB - pA = xB + rB - xA - rA
// C = dot(ay, d)
// Cdot = dot(d, cross(wA, ay)) + dot(ay, vB + cross(wB, rB) - vA - cross(wA, rA))
//      = -dot(ay, vA) - dot(cross(d + rA, ay), wA) + dot(ay, vB) + dot(cross(rB, ay), vB)
// J = [-ay, -cross(d + rA, ay), ay, cross(rB, ay)]

// Spring linear raint
// C = dot(ax, d)
// Cdot = = -dot(ax, vA) - dot(cross(d + rA, ax), wA) + dot(ax, vB) + dot(cross(rB, ax), vB)
// J = [-ax -cross(d+rA, ax) ax cross(rB, ax)]

// Motor rotational raint
// Cdot = wB - wA
// J = [0 0 -1 0 0 1]
using System;
using System.Diagnostics;
using Box2D.Common;

namespace Box2D.Dynamics.Joints
{
    /// A wheel joint. This joint provides two degrees of freedom: translation
    /// along an axis fixed in bodyA and rotation in the plane. You can use a
    /// joint limit to restrict the range of motion and a joint motor to drive
    /// the rotation or to model rotational friction.
    /// This joint is designed for vehicle suspensions.
    public class b2WheelJoint : b2Joint
    {
        protected float m_frequencyHz;
        protected float m_dampingRatio;

        // Solver shared
        protected b2Vec2 m_localAnchorA;
        protected b2Vec2 m_localAnchorB;
        protected b2Vec2 m_localXAxisA;
        protected b2Vec2 m_localYAxisA;

        protected float m_impulse;
        protected float m_motorImpulse;
        protected float m_springImpulse;

        protected float m_maxMotorTorque;
        protected float m_motorSpeed;
        protected bool m_enableMotor;

        // Solver temp
        protected int m_indexA;
        protected int m_indexB;
        protected b2Vec2 m_localCenterA;
        protected b2Vec2 m_localCenterB;
        protected float m_invMassA;
        protected float m_invMassB;
        protected float m_invIA;
        protected float m_invIB;

        protected b2Vec2 m_ax, m_ay;
        protected float m_sAx, m_sBx;
        protected float m_sAy, m_sBy;

        protected float m_mass;
        protected float m_motorMass;
        protected float m_springMass;

        protected float m_bias;
        protected float m_gamma;

        public b2WheelJoint(b2WheelJointDef def)
            : base(def)
        {
            m_localAnchorA = def.localAnchorA;
            m_localAnchorB = def.localAnchorB;
            m_localXAxisA = def.localAxisA;
            m_localYAxisA = m_localXAxisA.NegUnitCross();
//            m_localYAxisA = b2Math.b2Cross(1.0f, m_localXAxisA);

            m_mass = 0.0f;
            m_impulse = 0.0f;
            m_motorMass = 0.0f;
            m_motorImpulse = 0.0f;
            m_springMass = 0.0f;
            m_springImpulse = 0.0f;

            m_maxMotorTorque = def.maxMotorTorque;
            m_motorSpeed = def.motorSpeed;
            m_enableMotor = def.enableMotor;

            m_frequencyHz = def.frequencyHz;
            m_dampingRatio = def.dampingRatio;

            m_bias = 0.0f;
            m_gamma = 0.0f;

            m_ax.SetZero();
            m_ay.SetZero();
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

            float mA = m_invMassA, mB = m_invMassB;
            float iA = m_invIA, iB = m_invIB;

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

            // Compute the effective masses.
            b2Vec2 rA = b2Math.b2Mul(qA, m_localAnchorA - m_localCenterA);
            b2Vec2 rB = b2Math.b2Mul(qB, m_localAnchorB - m_localCenterB);
            b2Vec2 d = cB + rB - cA - rA;

            // Point to line raint
            {
                m_ay = b2Math.b2Mul(qA, m_localYAxisA);
                m_sAy = b2Math.b2Cross(d + rA, m_ay);
                m_sBy = b2Math.b2Cross(rB, m_ay);

                m_mass = mA + mB + iA * m_sAy * m_sAy + iB * m_sBy * m_sBy;

                if (m_mass > 0.0f)
                {
                    m_mass = 1.0f / m_mass;
                }
            }

            // Spring raint
            m_springMass = 0.0f;
            m_bias = 0.0f;
            m_gamma = 0.0f;
            if (m_frequencyHz > 0.0f)
            {
                m_ax = b2Math.b2Mul(qA, m_localXAxisA);
                m_sAx = b2Math.b2Cross(d + rA, m_ax);
                m_sBx = b2Math.b2Cross(rB, m_ax);

                float invMass = mA + mB + iA * m_sAx * m_sAx + iB * m_sBx * m_sBx;

                if (invMass > 0.0f)
                {
                    m_springMass = 1.0f / invMass;

                    float C = b2Math.b2Dot(d, m_ax);

                    // Frequency
                    float omega = 2.0f * (float)Math.PI * m_frequencyHz;

                    // Damping coefficient
                    float dx = 2.0f * m_springMass * m_dampingRatio * omega;

                    // Spring stiffness
                    float k = m_springMass * omega * omega;

                    // magic formulas
                    float h = data.step.dt;
                    m_gamma = h * (dx + h * k);
                    if (m_gamma > 0.0f)
                    {
                        m_gamma = 1.0f / m_gamma;
                    }

                    m_bias = C * h * k * m_gamma;

                    m_springMass = invMass + m_gamma;
                    if (m_springMass > 0.0f)
                    {
                        m_springMass = 1.0f / m_springMass;
                    }
                }
            }
            else
            {
                m_springImpulse = 0.0f;
            }

            // Rotational motor
            if (m_enableMotor)
            {
                m_motorMass = iA + iB;
                if (m_motorMass > 0.0f)
                {
                    m_motorMass = 1.0f / m_motorMass;
                }
            }
            else
            {
                m_motorMass = 0.0f;
                m_motorImpulse = 0.0f;
            }

            if (data.step.warmStarting)
            {
                // Account for variable time step.
                m_impulse *= data.step.dtRatio;
                m_springImpulse *= data.step.dtRatio;
                m_motorImpulse *= data.step.dtRatio;

                b2Vec2 P = m_impulse * m_ay + m_springImpulse * m_ax;
                float LA = m_impulse * m_sAy + m_springImpulse * m_sAx + m_motorImpulse;
                float LB = m_impulse * m_sBy + m_springImpulse * m_sBx + m_motorImpulse;

                vA -= m_invMassA * P;
                wA -= m_invIA * LA;

                vB += m_invMassB * P;
                wB += m_invIB * LB;
            }
            else
            {
                m_impulse = 0.0f;
                m_springImpulse = 0.0f;
                m_motorImpulse = 0.0f;
            }

            data.velocities[m_indexA].v = vA;
            data.velocities[m_indexA].w = wA;
            data.velocities[m_indexB].v = vB;
            data.velocities[m_indexB].w = wB;
        }

        public override void SolveVelocityConstraints(b2SolverData data)
        {
            float mA = m_invMassA, mB = m_invMassB;
            float iA = m_invIA, iB = m_invIB;

            b2Vec2 vA = data.velocities[m_indexA].v;
            float wA = data.velocities[m_indexA].w;
            b2Vec2 vB = data.velocities[m_indexB].v;
            float wB = data.velocities[m_indexB].w;

            // Solve spring raint
            {
                float Cdot = b2Math.b2Dot(m_ax, vB - vA) + m_sBx * wB - m_sAx * wA;
                float impulse = -m_springMass * (Cdot + m_bias + m_gamma * m_springImpulse);
                m_springImpulse += impulse;

                b2Vec2 P = impulse * m_ax;
                float LA = impulse * m_sAx;
                float LB = impulse * m_sBx;

                vA -= mA * P;
                wA -= iA * LA;

                vB += mB * P;
                wB += iB * LB;
            }

            // Solve rotational motor raint
            {
                float Cdot = wB - wA - m_motorSpeed;
                float impulse = -m_motorMass * Cdot;

                float oldImpulse = m_motorImpulse;
                float maxImpulse = data.step.dt * m_maxMotorTorque;
                m_motorImpulse = b2Math.b2Clamp(m_motorImpulse + impulse, -maxImpulse, maxImpulse);
                impulse = m_motorImpulse - oldImpulse;

                wA -= iA * impulse;
                wB += iB * impulse;
            }

            // Solve point to line raint
            {
                float Cdot = b2Math.b2Dot(m_ay, vB - vA) + m_sBy * wB - m_sAy * wA;
                float impulse = -m_mass * Cdot;
                m_impulse += impulse;

                b2Vec2 P = impulse * m_ay;
                float LA = impulse * m_sAy;
                float LB = impulse * m_sBy;

                vA -= mA * P;
                wA -= iA * LA;

                vB += mB * P;
                wB += iB * LB;
            }

            data.velocities[m_indexA].v = vA;
            data.velocities[m_indexA].w = wA;
            data.velocities[m_indexB].v = vB;
            data.velocities[m_indexB].w = wB;
        }

        public override bool SolvePositionConstraints(b2SolverData data)
        {
            b2Vec2 cA = data.positions[m_indexA].c;
            float aA = data.positions[m_indexA].a;
            b2Vec2 cB = data.positions[m_indexB].c;
            float aB = data.positions[m_indexB].a;

            b2Rot qA = new b2Rot(aA);
            b2Rot qB = new b2Rot(aB);

            b2Vec2 rA = b2Math.b2Mul(qA, m_localAnchorA - m_localCenterA);
            b2Vec2 rB = b2Math.b2Mul(qB, m_localAnchorB - m_localCenterB);
            b2Vec2 d = (cB - cA) + rB - rA;

            b2Vec2 ay = b2Math.b2Mul(qA, m_localYAxisA);

            float sAy = b2Math.b2Cross(d + rA, ay);
            float sBy = b2Math.b2Cross(ref rB, ref ay);

            float C = b2Math.b2Dot(ref d, ref ay);

            float k = m_invMassA + m_invMassB + m_invIA * m_sAy * m_sAy + m_invIB * m_sBy * m_sBy;

            float impulse;
            if (k != 0.0f)
            {
                impulse = -C / k;
            }
            else
            {
                impulse = 0.0f;
            }

            b2Vec2 P = impulse * ay;
            float LA = impulse * sAy;
            float LB = impulse * sBy;

            cA -= m_invMassA * P;
            aA -= m_invIA * LA;
            cB += m_invMassB * P;
            aB += m_invIB * LB;

            data.positions[m_indexA].c = cA;
            data.positions[m_indexA].a = aA;
            data.positions[m_indexB].c = cB;
            data.positions[m_indexB].a = aB;

            return b2Math.b2Abs(C) <= b2Settings.b2_linearSlop;
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
            return inv_dt * (m_impulse * m_ay + m_springImpulse * m_ax);
        }

        public virtual float GetReactionTorque(float inv_dt)
        {
            return inv_dt * m_motorImpulse;
        }

        public virtual float GetJointTranslation()
        {
            b2Body bA = m_bodyA;
            b2Body bB = m_bodyB;

            b2Vec2 pA = bA.GetWorldPoint(m_localAnchorA);
            b2Vec2 pB = bB.GetWorldPoint(m_localAnchorB);
            b2Vec2 d = pB - pA;
            b2Vec2 axis = bA.GetWorldVector(m_localXAxisA);

            float translation = b2Math.b2Dot(ref d, ref axis);
            return translation;
        }

        public virtual float GetJointSpeed()
        {
            float wA = m_bodyA.AngularVelocity;
            float wB = m_bodyB.AngularVelocity;
            return wB - wA;
        }

        public virtual bool IsMotorEnabled()
        {
            return m_enableMotor;
        }

        public virtual void EnableMotor(bool flag)
        {
            m_bodyA.SetAwake(true);
            m_bodyB.SetAwake(true);
            m_enableMotor = flag;
        }

        public virtual void SetMotorSpeed(float speed)
        {
            m_bodyA.SetAwake(true);
            m_bodyB.SetAwake(true);
            m_motorSpeed = speed;
        }

        public virtual void SetMaxMotorTorque(float torque)
        {
            m_bodyA.SetAwake(true);
            m_bodyB.SetAwake(true);
            m_maxMotorTorque = torque;
        }

        public virtual float GetMotorTorque(float inv_dt)
        {
            return inv_dt * m_motorImpulse;
        }

        public override void Dump()
        {
            int indexA = m_bodyA.IslandIndex;
            int indexB = m_bodyB.IslandIndex;

            b2Settings.b2Log("  b2WheelJointDef jd;\n");
            b2Settings.b2Log("  jd.bodyA = bodies[{0}];\n", indexA);
            b2Settings.b2Log("  jd.bodyB = bodies[{0}];\n", indexB);
            b2Settings.b2Log("  jd.collideConnected = bool({0});\n", m_collideConnected);
            b2Settings.b2Log("  jd.localAnchorA.Set({0:F5}, {1:F5});\n", m_localAnchorA.x, m_localAnchorA.y);
            b2Settings.b2Log("  jd.localAnchorB.Set({0:F5}, {1:F5});\n", m_localAnchorB.x, m_localAnchorB.y);
            b2Settings.b2Log("  jd.localAxisA.Set({0:F5}, {1:F5});\n", m_localXAxisA.x, m_localXAxisA.y);
            b2Settings.b2Log("  jd.enableMotor = bool({0});\n", m_enableMotor);
            b2Settings.b2Log("  jd.motorSpeed = {0:F5};\n", m_motorSpeed);
            b2Settings.b2Log("  jd.maxMotorTorque = {0:F5};\n", m_maxMotorTorque);
            b2Settings.b2Log("  jd.frequencyHz = {0:F5};\n", m_frequencyHz);
            b2Settings.b2Log("  jd.dampingRatio = {0:F5};\n", m_dampingRatio);
            b2Settings.b2Log("  joints[{0}] = m_world.CreateJoint(&jd);\n", m_index);
        }
        public virtual float GetMotorSpeed()
        {
            return m_motorSpeed;
        }

        public virtual float GetMaxMotorTorque()
        {
            return m_maxMotorTorque;
        }

        public virtual void SetSpringFrequencyHz(float hz)
        {
            m_frequencyHz = hz;
        }

        public virtual float GetSpringFrequencyHz()
        {
            return m_frequencyHz;
        }

        public virtual void SetSpringDampingRatio(float ratio)
        {
            m_dampingRatio = ratio;
        }

        public virtual float GetSpringDampingRatio()
        {
            return m_dampingRatio;
        }
    }
}
