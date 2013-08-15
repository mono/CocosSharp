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

// Linear constraint (point-to-line)
// d = p2 - p1 = x2 + r2 - x1 - r1
// C = dot(perp, d)
// Cdot = dot(d, cross(w1, perp)) + dot(perp, v2 + cross(w2, r2) - v1 - cross(w1, r1))
//      = -dot(perp, v1) - dot(cross(d + r1, perp), w1) + dot(perp, v2) + dot(cross(r2, perp), v2)
// J = [-perp, -cross(d + r1, perp), perp, cross(r2,perp)]
//
// Angular constraint
// C = a2 - a1 + a_initial
// Cdot = w2 - w1
// J = [0 0 -1 0 0 1]
//
// K = J * invM * JT
//
// J = [-a -s1 a s2]
//     [0  -1  0  1]
// a = perp
// s1 = cross(d + r1, a) = cross(p2 - x1, a)
// s2 = cross(r2, a) = cross(p2 - x2, a)


// Motor/Limit linear constraint
// C = dot(ax1, d)
// Cdot = = -dot(ax1, v1) - dot(cross(d + r1, ax1), w1) + dot(ax1, v2) + dot(cross(r2, ax1), v2)
// J = [-ax1 -cross(d+r1,ax1) ax1 cross(r2,ax1)]

// Block Solver
// We develop a block solver that includes the joint limit. This makes the limit stiff (inelastic) even
// when the mass has poor distribution (leading to large torques about the joint anchor points).
//
// The Jacobian has 3 rows:
// J = [-uT -s1 uT s2] // linear
//     [0   -1   0  1] // angular
//     [-vT -a1 vT a2] // limit
//
// u = perp
// v = axis
// s1 = cross(d + r1, u), s2 = cross(r2, u)
// a1 = cross(d + r1, v), a2 = cross(r2, v)

// M * (v2 - v1) = JT * df
// J * v2 = bias
//
// v2 = v1 + invM * JT * df
// J * (v1 + invM * JT * df) = bias
// K * df = bias - J * v1 = -Cdot
// K = J * invM * JT
// Cdot = J * v1 - bias
//
// Now solve for f2.
// df = f2 - f1
// K * (f2 - f1) = -Cdot
// f2 = invK * (-Cdot) + f1
//
// Clamp accumulated limit impulse.
// lower: f2(3) = max(f2(3), 0)
// upper: f2(3) = min(f2(3), 0)
//
// Solve for correct f2(1:2)
// K(1:2, 1:2) * f2(1:2) = -Cdot(1:2) - K(1:2,3) * f2(3) + K(1:2,1:3) * f1
//                       = -Cdot(1:2) - K(1:2,3) * f2(3) + K(1:2,1:2) * f1(1:2) + K(1:2,3) * f1(3)
// K(1:2, 1:2) * f2(1:2) = -Cdot(1:2) - K(1:2,3) * (f2(3) - f1(3)) + K(1:2,1:2) * f1(1:2)
// f2(1:2) = invK(1:2,1:2) * (-Cdot(1:2) - K(1:2,3) * (f2(3) - f1(3))) + f1(1:2)
//
// Now compute impulse to be applied:
// df = f2 - f1
using System;
using Box2D.Common;

namespace Box2D.Dynamics.Joints
{

    public class b2PrismaticJoint : b2Joint
    {

        // Solver shared
        protected b2Vec2 m_localAnchorA;
        protected b2Vec2 m_localAnchorB;
        protected b2Vec2 m_localXAxisA;
        protected b2Vec2 m_localYAxisA;
        protected float m_referenceAngle;
        protected b2Vec3 m_impulse;
        protected float m_motorImpulse;
        protected float m_lowerTranslation;
        protected float m_upperTranslation;
        protected float m_maxMotorForce;
        protected float m_motorSpeed;
        protected bool m_enableLimit;
        protected bool m_enableMotor;
        protected b2LimitState m_limitState;

        // Solver temp
        protected int m_indexA;
        protected int m_indexB;
        protected b2Vec2 m_localCenterA;
        protected b2Vec2 m_localCenterB;
        protected float InvertedMassA;
        protected float InvertedMassB;
        protected float InvertedIA;
        protected float InvertedIB;
        protected b2Vec2 m_axis, m_perp;
        protected float m_s1, m_s2;
        protected float m_a1, m_a2;
        protected b2Mat33 m_K;
        protected float m_motorMass;

        public b2PrismaticJoint(b2PrismaticJointDef def)
            : base(def)
        {
            m_localAnchorA = def.localAnchorA;
            m_localAnchorB = def.localAnchorB;
            m_localXAxisA = def.localAxisA;
            m_localXAxisA.Normalize();
            m_localYAxisA = m_localXAxisA.NegUnitCross(); //  b2Math.b2Cross(1.0f, m_localXAxisA);
            m_referenceAngle = def.referenceAngle;

            m_impulse.SetZero();
            m_motorMass = 0.0f;
            m_motorImpulse = 0.0f;

            m_lowerTranslation = def.lowerTranslation;
            m_upperTranslation = def.upperTranslation;
            m_maxMotorForce = def.maxMotorForce;
            m_motorSpeed = def.motorSpeed;
            m_enableLimit = def.enableLimit;
            m_enableMotor = def.enableMotor;
            m_limitState = b2LimitState.e_inactiveLimit;

            m_axis.SetZero();
            m_perp.SetZero();
        }

        /// The local anchor point relative to bodyA's origin.
        public virtual b2Vec2 GetLocalAnchorA() { return m_localAnchorA; }

        /// The local anchor point relative to bodyB's origin.
        public virtual b2Vec2 GetLocalAnchorB() { return m_localAnchorB; }

        /// The local joint axis relative to bodyA.
        public virtual b2Vec2 GetLocalAxisA() { return m_localXAxisA; }

        /// Get the reference angle.
        public virtual float GetReferenceAngle() { return m_referenceAngle; }

        public virtual float GetMaxMotorForce() { return m_maxMotorForce; }

        public virtual float GetMotorSpeed()
        {
            return m_motorSpeed;
        }

        public override void InitVelocityConstraints(b2SolverData data)
        {
            m_indexA = m_bodyA.IslandIndex;
            m_indexB = m_bodyB.IslandIndex;
            m_localCenterA = m_bodyA.Sweep.localCenter;
            m_localCenterB = m_bodyB.Sweep.localCenter;
            InvertedMassA = m_bodyA.InvertedMass;
            InvertedMassB = m_bodyB.InvertedMass;
            InvertedIA = m_bodyA.InvertedI;
            InvertedIB = m_bodyB.InvertedI;

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
            b2Vec2 d = (cB - cA) + rB - rA;

            float mA = InvertedMassA, mB = InvertedMassB;
            float iA = InvertedIA, iB = InvertedIB;

            // Compute motor Jacobian and effective mass.
            {
                m_axis = b2Math.b2Mul(qA, m_localXAxisA);
                m_a1 = b2Math.b2Cross(d + rA, m_axis);
                m_a2 = b2Math.b2Cross(rB, m_axis);

                m_motorMass = mA + mB + iA * m_a1 * m_a1 + iB * m_a2 * m_a2;
                if (m_motorMass > 0.0f)
                {
                    m_motorMass = 1.0f / m_motorMass;
                }
            }

            // Prismatic constraint.
            {
                m_perp = b2Math.b2Mul(qA, m_localYAxisA);

                m_s1 = b2Math.b2Cross(d + rA, m_perp);
                m_s2 = b2Math.b2Cross(rB, m_perp);

                float k11 = mA + mB + iA * m_s1 * m_s1 + iB * m_s2 * m_s2;
                float k12 = iA * m_s1 + iB * m_s2;
                float k13 = iA * m_s1 * m_a1 + iB * m_s2 * m_a2;
                float k22 = iA + iB;
                if (k22 == 0.0f)
                {
                    // For bodies with fixed rotation.
                    k22 = 1.0f;
                }
                float k23 = iA * m_a1 + iB * m_a2;
                float k33 = mA + mB + iA * m_a1 * m_a1 + iB * m_a2 * m_a2;

                m_K.ex.Set(k11, k12, k13);
                m_K.ey.Set(k12, k22, k23);
                m_K.ez.Set(k13, k23, k33);
            }

            // Compute motor and limit terms.
            if (m_enableLimit)
            {
                float jointTranslation = b2Math.b2Dot(m_axis, d);
                if (b2Math.b2Abs(m_upperTranslation - m_lowerTranslation) < 2.0f * b2Settings.b2_linearSlop)
                {
                    m_limitState = b2LimitState.e_equalLimits;
                }
                else if (jointTranslation <= m_lowerTranslation)
                {
                    if (m_limitState != b2LimitState.e_atLowerLimit)
                    {
                        m_limitState = b2LimitState.e_atLowerLimit;
                        m_impulse.z = 0.0f;
                    }
                }
                else if (jointTranslation >= m_upperTranslation)
                {
                    if (m_limitState != b2LimitState.e_atUpperLimit)
                    {
                        m_limitState = b2LimitState.e_atUpperLimit;
                        m_impulse.z = 0.0f;
                    }
                }
                else
                {
                    m_limitState = b2LimitState.e_inactiveLimit;
                    m_impulse.z = 0.0f;
                }
            }
            else
            {
                m_limitState = b2LimitState.e_inactiveLimit;
                m_impulse.z = 0.0f;
            }

            if (m_enableMotor == false)
            {
                m_motorImpulse = 0.0f;
            }

            if (data.step.warmStarting)
            {
                // Account for variable time step.
                m_impulse *= data.step.dtRatio;
                m_motorImpulse *= data.step.dtRatio;

                b2Vec2 P = m_impulse.x * m_perp + (m_motorImpulse + m_impulse.z) * m_axis;
                float LA = m_impulse.x * m_s1 + m_impulse.y + (m_motorImpulse + m_impulse.z) * m_a1;
                float LB = m_impulse.x * m_s2 + m_impulse.y + (m_motorImpulse + m_impulse.z) * m_a2;

                vA -= mA * P;
                wA -= iA * LA;

                vB += mB * P;
                wB += iB * LB;
            }
            else
            {
                m_impulse.SetZero();
                m_motorImpulse = 0.0f;
            }

            data.velocities[m_indexA].v = vA;
            data.velocities[m_indexA].w = wA;
            data.velocities[m_indexB].v = vB;
            data.velocities[m_indexB].w = wB;
        }

        public override void SolveVelocityConstraints(b2SolverData data)
{
    b2Vec2 vA = data.velocities[m_indexA].v;
    float wA = data.velocities[m_indexA].w;
    b2Vec2 vB = data.velocities[m_indexB].v;
    float wB = data.velocities[m_indexB].w;

    float mA = InvertedMassA, mB = InvertedMassB;
    float iA = InvertedIA, iB = InvertedIB;

    // Solve linear motor constraint.
    if (m_enableMotor && m_limitState != b2LimitState.e_equalLimits)
    {
        float Cdot = b2Math.b2Dot(m_axis, vB - vA) + m_a2 * wB - m_a1 * wA;
        float impulse = m_motorMass * (m_motorSpeed - Cdot);
        float oldImpulse = m_motorImpulse;
        float maxImpulse = data.step.dt * m_maxMotorForce;
        m_motorImpulse = b2Math.b2Clamp(m_motorImpulse + impulse, -maxImpulse, maxImpulse);
        impulse = m_motorImpulse - oldImpulse;

        b2Vec2 P = impulse * m_axis;
        float LA = impulse * m_a1;
        float LB = impulse * m_a2;

        vA -= mA * P;
        wA -= iA * LA;

        vB += mB * P;
        wB += iB * LB;
    }

    b2Vec2 Cdot1 = new b2Vec2();
    Cdot1.x = b2Math.b2Dot(m_perp, vB - vA) + m_s2 * wB - m_s1 * wA;
    Cdot1.y = wB - wA;

    if (m_enableLimit && m_limitState != b2LimitState.e_inactiveLimit)
    {
        // Solve prismatic and limit constraint in block form.
        float Cdot2;
        Cdot2 = b2Math.b2Dot(m_axis, vB - vA) + m_a2 * wB - m_a1 * wA;
        b2Vec3 Cdot = new b2Vec3(Cdot1.x, Cdot1.y, Cdot2);

        b2Vec3 f1 = m_impulse;
        b2Vec3 df =  m_K.Solve33(-Cdot);
        m_impulse += df;

        if (m_limitState == b2LimitState.e_atLowerLimit)
        {
            m_impulse.z = Math.Max(m_impulse.z, 0.0f);
        }
        else if (m_limitState == b2LimitState.e_atUpperLimit)
        {
            m_impulse.z = Math.Min(m_impulse.z, 0.0f);
        }

        // f2(1:2) = invK(1:2,1:2) * (-Cdot(1:2) - K(1:2,3) * (f2(3) - f1(3))) + f1(1:2)
        b2Vec2 b = -Cdot1 - (m_impulse.z - f1.z) * (new b2Vec2(m_K.ez.x, m_K.ez.y));
        b2Vec2 f2r = m_K.Solve22(b) + (new b2Vec2(f1.x, f1.y));
        m_impulse.x = f2r.x;
        m_impulse.y = f2r.y;

        df = m_impulse - f1;

        b2Vec2 P = df.x * m_perp + df.z * m_axis;
        float LA = df.x * m_s1 + df.y + df.z * m_a1;
        float LB = df.x * m_s2 + df.y + df.z * m_a2;

        vA -= mA * P;
        wA -= iA * LA;

        vB += mB * P;
        wB += iB * LB;
    }
    else
    {
        // Limit is inactive, just solve the prismatic constraint in block form.
        b2Vec2 df = m_K.Solve22(-Cdot1);
        m_impulse.x += df.x;
        m_impulse.y += df.y;

        b2Vec2 P = df.x * m_perp;
        float LA = df.x * m_s1 + df.y;
        float LB = df.x * m_s2 + df.y;

        vA -= mA * P;
        wA -= iA * LA;

        vB += mB * P;
        wB += iB * LB;

        b2Vec2 Cdot10 = Cdot1;

        Cdot1.x = b2Math.b2Dot(m_perp, vB - vA) + m_s2 * wB - m_s1 * wA;
        Cdot1.y = wB - wA;

        if (b2Math.b2Abs(Cdot1.x) > 0.01f || b2Math.b2Abs(Cdot1.y) > 0.01f)
        {
            b2Vec2 test = b2Math.b2Mul22(m_K, df);
            Cdot1.x += 0.0f;
        }
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

            float mA = InvertedMassA, mB = InvertedMassB;
            float iA = InvertedIA, iB = InvertedIB;

            // Compute fresh Jacobians
            b2Vec2 rA = b2Math.b2Mul(qA, m_localAnchorA - m_localCenterA);
            b2Vec2 rB = b2Math.b2Mul(qB, m_localAnchorB - m_localCenterB);
            b2Vec2 d = cB + rB - cA - rA;

            b2Vec2 axis = b2Math.b2Mul(qA, m_localXAxisA);
            float a1 = b2Math.b2Cross(d + rA, axis);
            float a2 = b2Math.b2Cross(rB, axis);
            b2Vec2 perp = b2Math.b2Mul(qA, m_localYAxisA);

            float s1 = b2Math.b2Cross(d + rA, perp);
            float s2 = b2Math.b2Cross(rB, perp);

            b2Vec3 impulse;
            b2Vec2 C1 = new b2Vec2();
            C1.x = b2Math.b2Dot(perp, d);
            C1.y = aB - aA - m_referenceAngle;

            float linearError = b2Math.b2Abs(C1.x);
            float angularError = b2Math.b2Abs(C1.y);

            bool active = false;
            float C2 = 0.0f;
            if (m_enableLimit)
            {
                float translation = b2Math.b2Dot(axis, d);
                if (b2Math.b2Abs(m_upperTranslation - m_lowerTranslation) < 2.0f * b2Settings.b2_linearSlop)
                {
                    // Prevent large angular corrections
                    C2 = b2Math.b2Clamp(translation, -b2Settings.b2_maxLinearCorrection, b2Settings.b2_maxLinearCorrection);
                    linearError = Math.Max(linearError, b2Math.b2Abs(translation));
                    active = true;
                }
                else if (translation <= m_lowerTranslation)
                {
                    // Prevent large linear corrections and allow some slop.
                    C2 = b2Math.b2Clamp(translation - m_lowerTranslation + b2Settings.b2_linearSlop, -b2Settings.b2_maxLinearCorrection, 0.0f);
                    linearError = Math.Max(linearError, m_lowerTranslation - translation);
                    active = true;
                }
                else if (translation >= m_upperTranslation)
                {
                    // Prevent large linear corrections and allow some slop.
                    C2 = b2Math.b2Clamp(translation - m_upperTranslation - b2Settings.b2_linearSlop, 0.0f, b2Settings.b2_maxLinearCorrection);
                    linearError = Math.Max(linearError, translation - m_upperTranslation);
                    active = true;
                }
            }

            if (active)
            {
                float k11 = mA + mB + iA * s1 * s1 + iB * s2 * s2;
                float k12 = iA * s1 + iB * s2;
                float k13 = iA * s1 * a1 + iB * s2 * a2;
                float k22 = iA + iB;
                if (k22 == 0.0f)
                {
                    // For fixed rotation
                    k22 = 1.0f;
                }
                float k23 = iA * a1 + iB * a2;
                float k33 = mA + mB + iA * a1 * a1 + iB * a2 * a2;

                b2Mat33 K = new b2Mat33(
                    new b2Vec3(k11, k12, k13),
                    new b2Vec3(k12, k22, k23),
                    new b2Vec3(k13, k23, k33));

                b2Vec3 C = new b2Vec3(C1.x, C1.y, C2);

                impulse = K.Solve33(-C);
            }
            else
            {
                float k11 = mA + mB + iA * s1 * s1 + iB * s2 * s2;
                float k12 = iA * s1 + iB * s2;
                float k22 = iA + iB;
                if (k22 == 0.0f)
                {
                    k22 = 1.0f;
                }

                b2Mat22 K = new b2Mat22();
                K.ex.Set(k11, k12);
                K.ey.Set(k12, k22);

                b2Vec2 impulse1 = K.Solve(-C1);
                impulse = new b2Vec3();
                impulse.x = impulse1.x;
                impulse.y = impulse1.y;
                impulse.z = 0.0f;
            }

            b2Vec2 P = impulse.x * perp + impulse.z * axis;
            float LA = impulse.x * s1 + impulse.y + impulse.z * a1;
            float LB = impulse.x * s2 + impulse.y + impulse.z * a2;

            cA -= mA * P;
            aA -= iA * LA;
            cB += mB * P;
            aB += iB * LB;

            data.positions[m_indexA].c = cA;
            data.positions[m_indexA].a = aA;
            data.positions[m_indexB].c = cB;
            data.positions[m_indexB].a = aB;

            return linearError <= b2Settings.b2_linearSlop && angularError <= b2Settings.b2_angularSlop;
        }

        public virtual b2Vec2 GetLocalXAxisA() {
            return (m_localXAxisA);
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
            return inv_dt * (m_impulse.x * m_perp + (m_motorImpulse + m_impulse.z) * m_axis);
        }

        public virtual float GetReactionTorque(float inv_dt)
        {
            return inv_dt * m_impulse.y;
        }

        public virtual float GetJointTranslation()
        {
            b2Vec2 pA = m_bodyA.GetWorldPoint(m_localAnchorA);
            b2Vec2 pB = m_bodyB.GetWorldPoint(m_localAnchorB);
            b2Vec2 d = pB - pA;
            b2Vec2 axis = m_bodyA.GetWorldVector(m_localXAxisA);

            float translation = b2Math.b2Dot(d, axis);
            return translation;
        }

        public virtual float GetJointSpeed()
        {
            b2Body bA = m_bodyA;
            b2Body bB = m_bodyB;

            b2Vec2 rA = b2Math.b2Mul(bA.XF.q, m_localAnchorA - bA.Sweep.localCenter);
            b2Vec2 rB = b2Math.b2Mul(bB.XF.q, m_localAnchorB - bB.Sweep.localCenter);
            b2Vec2 p1 = bA.Sweep.c + rA;
            b2Vec2 p2 = bB.Sweep.c + rB;
            b2Vec2 d = p2 - p1;
            b2Vec2 axis = b2Math.b2Mul(bA.XF.q, m_localXAxisA);

            b2Vec2 vA = bA.LinearVelocity;
            b2Vec2 vB = bB.LinearVelocity;
            float wA = bA.AngularVelocity;
            float wB = bB.AngularVelocity;

            float speed = b2Math.b2Dot(d, b2Math.b2Cross(wA, ref axis)) + b2Math.b2Dot(axis, vB + b2Math.b2Cross(wB, ref rB) - vA - b2Math.b2Cross(wA, ref rA));
            return speed;
        }

        public virtual bool IsLimitEnabled()
        {
            return m_enableLimit;
        }

        public virtual void EnableLimit(bool flag)
        {
            if (flag != m_enableLimit)
            {
                m_bodyA.SetAwake(true);
                m_bodyB.SetAwake(true);
                m_enableLimit = flag;
                m_impulse.z = 0.0f;
            }
        }

        public virtual float GetLowerLimit()
        {
            return m_lowerTranslation;
        }

        public virtual float GetUpperLimit()
        {
            return m_upperTranslation;
        }

        public virtual void SetLimits(float lower, float upper)
        {
            System.Diagnostics.Debug.Assert(lower <= upper);
            if (lower != m_lowerTranslation || upper != m_upperTranslation)
            {
                m_bodyA.SetAwake(true);
                m_bodyB.SetAwake(true);
                m_lowerTranslation = lower;
                m_upperTranslation = upper;
                m_impulse.z = 0.0f;
            }
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

        public virtual void SetMaxMotorForce(float force)
        {
            m_bodyA.SetAwake(true);
            m_bodyB.SetAwake(true);
            m_maxMotorForce = force;
        }

        public virtual float GetMotorForce(float inv_dt)
        {
            return inv_dt * m_motorImpulse;
        }

        public override void Dump()
        {
            int indexA = m_bodyA.IslandIndex;
            int indexB = m_bodyB.IslandIndex;

            b2Settings.b2Log("  b2PrismaticJointDef jd;\n");
            b2Settings.b2Log("  jd.bodyA = bodies[{0}];\n", indexA);
            b2Settings.b2Log("  jd.bodyB = bodies[{0}];\n", indexB);
            b2Settings.b2Log("  jd.collideConnected = bool(%d);\n", m_collideConnected);
            b2Settings.b2Log("  jd.localAnchorA.Set({0:F5}, {1:F5});\n", m_localAnchorA.x, m_localAnchorA.y);
            b2Settings.b2Log("  jd.localAnchorB.Set({0:F5}, {1:F5});\n", m_localAnchorB.x, m_localAnchorB.y);
            b2Settings.b2Log("  jd.localAxisA.Set({0:F5}, {1:F5});\n", m_localXAxisA.x, m_localXAxisA.y);
            b2Settings.b2Log("  jd.referenceAngle = {0:F5};\n", m_referenceAngle);
            b2Settings.b2Log("  jd.enableLimit = bool({0});\n", m_enableLimit);
            b2Settings.b2Log("  jd.lowerTranslation = {0:F5};\n", m_lowerTranslation);
            b2Settings.b2Log("  jd.upperTranslation = {0:F5};\n", m_upperTranslation);
            b2Settings.b2Log("  jd.enableMotor = bool({0});\n", m_enableMotor);
            b2Settings.b2Log("  jd.motorSpeed = {0:F5};\n", m_motorSpeed);
            b2Settings.b2Log("  jd.maxMotorForce = {0:F5};\n", m_maxMotorForce);
            b2Settings.b2Log("  joints[{0}] = m_world.CreateJoint(&jd);\n", m_index);
        }
    }
}
