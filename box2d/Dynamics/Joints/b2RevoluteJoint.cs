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

// Motor constraint
// Cdot = w2 - w1
// J = [0 0 -1 0 0 1]
// K = invI1 + invI2
using System;
using Box2D.Common;
using System.Diagnostics;

namespace Box2D.Dynamics.Joints
{
    /// A revolute joint constrains two bodies to share a common point while they
    /// are free to rotate about the point. The relative rotation about the shared
    /// point is the joint angle. You can limit the relative rotation with
    /// a joint limit that specifies a lower and upper angle. You can use a motor
    /// to drive the relative rotation about the shared point. A maximum motor torque
    /// is provided so that infinite forces are not generated.
    public class b2RevoluteJoint : b2Joint
    {
        // Solver shared
        protected b2Vec2 m_localAnchorA;
        protected b2Vec2 m_localAnchorB;
        protected b2Vec3 m_impulse;
        protected float m_motorImpulse;

        protected bool m_enableMotor;
        protected float m_maxMotorTorque;
        protected float m_motorSpeed;

        protected bool m_enableLimit;
        protected float m_referenceAngle;
        protected float m_lowerAngle;
        protected float m_upperAngle;

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
        protected b2Mat33 m_mass;            // effective mass for point-to-point constraint.
        protected float m_motorMass;    // effective mass for motor/limit angular constraint.
        protected b2LimitState m_limitState;

        public b2RevoluteJoint(b2RevoluteJointDef def)
            : base(def)
        {
            m_localAnchorA = def.localAnchorA;
            m_localAnchorB = def.localAnchorB;
            m_referenceAngle = def.referenceAngle;

            m_impulse.SetZero();
            m_motorImpulse = 0.0f;

            m_lowerAngle = def.lowerAngle;
            m_upperAngle = def.upperAngle;
            m_maxMotorTorque = def.maxMotorTorque;
            m_motorSpeed = def.motorSpeed;
            m_enableLimit = def.enableLimit;
            m_enableMotor = def.enableMotor;
            m_limitState = b2LimitState.e_inactiveLimit;
        }

        public virtual float GetMaxMotorTorque() { return m_maxMotorTorque; }

        /// Get the reference angle.
        public virtual float GetReferenceAngle() { return m_referenceAngle; }

        /// The local anchor point relative to bodyA's origin.
        public override b2Vec2 GetLocalAnchorA() { return m_localAnchorA; }

        /// The local anchor point relative to bodyB's origin.
        public override b2Vec2 GetLocalAnchorB() { return m_localAnchorB; }

        public override float GetMotorSpeed()
        {
            return m_motorSpeed;
        }

        protected override void InitVelocityConstraints(b2SolverData data)
        {
            m_indexA = m_bodyA.IslandIndex;
            m_indexB = m_bodyB.IslandIndex;
            m_localCenterA = m_bodyA.Sweep.localCenter;
            m_localCenterB = m_bodyB.Sweep.localCenter;
            m_invMassA = m_bodyA.InvertedMass;
            m_invMassB = m_bodyB.InvertedMass;
            m_invIA = m_bodyA.InvertedI;
            m_invIB = m_bodyB.InvertedI;

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

            // J = [-I -r1_skew I r2_skew]
            //     [ 0       -1 0       1]
            // r_skew = [-ry; rx]

            // Matlab
            // K = [ mA+r1y^2*iA+mB+r2y^2*iB,  -r1y*iA*r1x-r2y*iB*r2x,          -r1y*iA-r2y*iB]
            //     [  -r1y*iA*r1x-r2y*iB*r2x, mA+r1x^2*iA+mB+r2x^2*iB,           r1x*iA+r2x*iB]
            //     [          -r1y*iA-r2y*iB,           r1x*iA+r2x*iB,                   iA+iB]

            float mA = m_invMassA, mB = m_invMassB;
            float iA = m_invIA, iB = m_invIB;

            bool fixedRotation = (iA + iB == 0.0f);
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
            m_mass = new b2Mat33(ex, ey, ez);

            m_motorMass = iA + iB;
            if (m_motorMass > 0.0f)
            {
                m_motorMass = 1.0f / m_motorMass;
            }

            if (m_enableMotor == false || fixedRotation)
            {
                m_motorImpulse = 0.0f;
            }

            if (m_enableLimit && fixedRotation == false)
            {
                float jointAngle = aB - aA - m_referenceAngle;
                if (b2Math.b2Abs(m_upperAngle - m_lowerAngle) < 2.0f * b2Settings.b2_angularSlop)
                {
                    m_limitState = b2LimitState.e_equalLimits;
                }
                else if (jointAngle <= m_lowerAngle)
                {
                    if (m_limitState != b2LimitState.e_atLowerLimit)
                    {
                        m_impulse.z = 0.0f;
                    }
                    m_limitState = b2LimitState.e_atLowerLimit;
                }
                else if (jointAngle >= m_upperAngle)
                {
                    if (m_limitState != b2LimitState.e_atUpperLimit)
                    {
                        m_impulse.z = 0.0f;
                    }
                    m_limitState = b2LimitState.e_atUpperLimit;
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
            }

            if (data.step.warmStarting)
            {
                // Scale impulses to support a variable time step.
                m_impulse *= data.step.dtRatio;
                m_motorImpulse *= data.step.dtRatio;

                b2Vec2 P = new b2Vec2(m_impulse.x, m_impulse.y);

                vA -= mA * P;
                wA -= iA * (b2Math.b2Cross(m_rA, P) + m_motorImpulse + m_impulse.z);

                vB += mB * P;
                wB += iB * (b2Math.b2Cross(m_rB, P) + m_motorImpulse + m_impulse.z);
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

        protected override void SolveVelocityConstraints(b2SolverData data)
        {
            b2Vec2 vA = data.velocities[m_indexA].v;
            float wA = data.velocities[m_indexA].w;
            b2Vec2 vB = data.velocities[m_indexB].v;
            float wB = data.velocities[m_indexB].w;

            float mA = m_invMassA, mB = m_invMassB;
            float iA = m_invIA, iB = m_invIB;

            bool fixedRotation = (iA + iB == 0.0f);

            // Solve motor constraint.
            if (m_enableMotor && m_limitState != b2LimitState.e_equalLimits && fixedRotation == false)
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

            // Solve limit constraint.
            if (m_enableLimit && m_limitState != b2LimitState.e_inactiveLimit && fixedRotation == false)
            {
                b2Vec2 Cdot1 = vB + b2Math.b2Cross(wB, m_rB) - vA - b2Math.b2Cross(wA, m_rA);
                float Cdot2 = wB - wA;
                b2Vec3 Cdot = new b2Vec3(Cdot1.x, Cdot1.y, Cdot2);

                b2Vec3 impulse = -m_mass.Solve33(Cdot);

                if (m_limitState == b2LimitState.e_equalLimits)
                {
                    m_impulse += impulse;
                }
                else if (m_limitState == b2LimitState.e_atLowerLimit)
                {
                    float newImpulse = m_impulse.z + impulse.z;
                    if (newImpulse < 0.0f)
                    {
                        b2Vec2 rhs = -Cdot1 + m_impulse.z * (new b2Vec2(m_mass.ez.x, m_mass.ez.y));
                        b2Vec2 reduced = m_mass.Solve22(rhs);
                        impulse.x = reduced.x;
                        impulse.y = reduced.y;
                        impulse.z = -m_impulse.z;
                        m_impulse.x += reduced.x;
                        m_impulse.y += reduced.y;
                        m_impulse.z = 0.0f;
                    }
                    else
                    {
                        m_impulse += impulse;
                    }
                }
                else if (m_limitState == b2LimitState.e_atUpperLimit)
                {
                    float newImpulse = m_impulse.z + impulse.z;
                    if (newImpulse > 0.0f)
                    {
                        b2Vec2 rhs = -Cdot1 + m_impulse.z * (new b2Vec2(m_mass.ez.x, m_mass.ez.y));
                        b2Vec2 reduced = m_mass.Solve22(rhs);
                        impulse.x = reduced.x;
                        impulse.y = reduced.y;
                        impulse.z = -m_impulse.z;
                        m_impulse.x += reduced.x;
                        m_impulse.y += reduced.y;
                        m_impulse.z = 0.0f;
                    }
                    else
                    {
                        m_impulse += impulse;
                    }
                }

                b2Vec2 P = new b2Vec2(impulse.x, impulse.y);

                vA -= mA * P;
                wA -= iA * (b2Math.b2Cross(m_rA, P) + impulse.z);

                vB += mB * P;
                wB += iB * (b2Math.b2Cross(m_rB, P) + impulse.z);
            }
            else
            {
                // Solve point-to-point constraint
                b2Vec2 Cdot = vB + b2Math.b2Cross(wB, m_rB) - vA - b2Math.b2Cross(wA, m_rA);
                b2Vec2 impulse = m_mass.Solve22(-Cdot);

                m_impulse.x += impulse.x;
                m_impulse.y += impulse.y;

                vA -= mA * impulse;
                wA -= iA * b2Math.b2Cross(m_rA, impulse);

                vB += mB * impulse;
                wB += iB * b2Math.b2Cross(m_rB, impulse);
            }

            data.velocities[m_indexA].v = vA;
            data.velocities[m_indexA].w = wA;
            data.velocities[m_indexB].v = vB;
            data.velocities[m_indexB].w = wB;
        }

        protected override bool SolvePositionConstraints(b2SolverData data)
        {
            b2Vec2 cA = data.positions[m_indexA].c;
            float aA = data.positions[m_indexA].a;
            b2Vec2 cB = data.positions[m_indexB].c;
            float aB = data.positions[m_indexB].a;

            b2Rot qA = new b2Rot(aA);
            b2Rot qB = new b2Rot(aB);

            float angularError = 0.0f;
            float positionError = 0.0f;

            bool fixedRotation = (m_invIA + m_invIB == 0.0f);

            // Solve angular limit constraint.
            if (m_enableLimit && m_limitState != b2LimitState.e_inactiveLimit && fixedRotation == false)
            {
                float angle = aB - aA - m_referenceAngle;
                float limitImpulse = 0.0f;

                if (m_limitState == b2LimitState.e_equalLimits)
                {
                    // Prevent large angular corrections
                    float C = b2Math.b2Clamp(angle - m_lowerAngle, -b2Settings.b2_maxAngularCorrection, b2Settings.b2_maxAngularCorrection);
                    limitImpulse = -m_motorMass * C;
                    angularError = b2Math.b2Abs(C);
                }
                else if (m_limitState == b2LimitState.e_atLowerLimit)
                {
                    float C = angle - m_lowerAngle;
                    angularError = -C;

                    // Prevent large angular corrections and allow some slop.
                    C = b2Math.b2Clamp(C + b2Settings.b2_angularSlop, -b2Settings.b2_maxAngularCorrection, 0.0f);
                    limitImpulse = -m_motorMass * C;
                }
                else if (m_limitState == b2LimitState.e_atUpperLimit)
                {
                    float C = angle - m_upperAngle;
                    angularError = C;

                    // Prevent large angular corrections and allow some slop.
                    C = b2Math.b2Clamp(C - b2Settings.b2_angularSlop, 0.0f, b2Settings.b2_maxAngularCorrection);
                    limitImpulse = -m_motorMass * C;
                }

                aA -= m_invIA * limitImpulse;
                aB += m_invIB * limitImpulse;
            }

            // Solve point-to-point constraint.
            {
                qA.Set(aA);
                qB.Set(aB);
                b2Vec2 rA = b2Math.b2Mul(qA, m_localAnchorA - m_localCenterA);
                b2Vec2 rB = b2Math.b2Mul(qB, m_localAnchorB - m_localCenterB);

                b2Vec2 C = cB + rB - cA - rA;
                positionError = C.Length();

                float mA = m_invMassA, mB = m_invMassB;
                float iA = m_invIA, iB = m_invIB;

                b2Mat22 K = new b2Mat22();
                K.exx = mA + mB + iA * rA.y * rA.y + iB * rB.y * rB.y;
                K.exy = -iA * rA.x * rA.y - iB * rB.x * rB.y;
                K.eyx = K.ex.y;
                K.eyy = mA + mB + iA * rA.x * rA.x + iB * rB.x * rB.x;

                b2Vec2 impulse = -K.Solve(C);

                cA -= mA * impulse;
                aA -= iA * b2Math.b2Cross(rA, impulse);

                cB += mB * impulse;
                aB += iB * b2Math.b2Cross(rB, impulse);
            }

            data.positions[m_indexA].c = cA;
            data.positions[m_indexA].a = aA;
            data.positions[m_indexB].c = cB;
            data.positions[m_indexB].a = aB;

            return positionError <= b2Settings.b2_linearSlop && angularError <= b2Settings.b2_angularSlop;
        }

        public override b2Vec2 GetAnchorA()
        {
            return m_bodyA.GetWorldPoint(m_localAnchorA);
        }

        public override b2Vec2 GetAnchorB()
        {
            return m_bodyB.GetWorldPoint(m_localAnchorB);
        }

        public override b2Vec2 GetReactionForce(float inv_dt)
        {
            b2Vec2 P = new b2Vec2(m_impulse.x, m_impulse.y);
            return inv_dt * P;
        }

        public override float GetReactionTorque(float inv_dt)
        {
            return inv_dt * m_impulse.z;
        }

        public override float GetJointAngle()
        {
            b2Body bA = m_bodyA;
            b2Body bB = m_bodyB;
            return bB.Sweep.a - bA.Sweep.a - m_referenceAngle;
        }

        public override float GetJointSpeed()
        {
            b2Body bA = m_bodyA;
            b2Body bB = m_bodyB;
            return bB.AngularVelocity - bA.AngularVelocity;
        }

        public override bool IsMotorEnabled()
        {
            return m_enableMotor;
        }

        public override void EnableMotor(bool flag)
        {
            m_bodyA.SetAwake(true);
            m_bodyB.SetAwake(true);
            m_enableMotor = flag;
        }

        public override float GetMotorTorque(float inv_dt)
        {
            return inv_dt * m_motorImpulse;
        }

        public override void SetMotorSpeed(float speed)
        {
            m_bodyA.SetAwake(true);
            m_bodyB.SetAwake(true);
            m_motorSpeed = speed;
        }

        public override void SetMaxMotorTorque(float torque)
        {
            m_bodyA.SetAwake(true);
            m_bodyB.SetAwake(true);
            m_maxMotorTorque = torque;
        }

        public override bool IsLimitEnabled()
        {
            return m_enableLimit;
        }

        public override void EnableLimit(bool flag)
        {
            if (flag != m_enableLimit)
            {
                m_bodyA.SetAwake(true);
                m_bodyB.SetAwake(true);
                m_enableLimit = flag;
                m_impulse.z = 0.0f;
            }
        }

        public override float GetLowerLimit()
        {
            return m_lowerAngle;
        }

        public override float GetUpperLimit()
        {
            return m_upperAngle;
        }

        public override void SetLimits(float lower, float upper)
        {
            Debug.Assert(lower <= upper);

            if (lower != m_lowerAngle || upper != m_upperAngle)
            {
                m_bodyA.SetAwake(true);
                m_bodyB.SetAwake(true);
                m_impulse.z = 0.0f;
                m_lowerAngle = lower;
                m_upperAngle = upper;
            }
        }

        public override void Dump()
        {
            int indexA = m_bodyA.IslandIndex;
            int indexB = m_bodyB.IslandIndex;

            b2Settings.b2Log("  b2RevoluteJointDef jd;\n");
            b2Settings.b2Log("  jd.bodyA = bodies[{0}];\n", indexA);
            b2Settings.b2Log("  jd.bodyB = bodies[{0}];\n", indexB);
            b2Settings.b2Log("  jd.collideConnected = bool({0});\n", m_collideConnected);
            b2Settings.b2Log("  jd.localAnchorA.Set({0:F5}, {1:F5});\n", m_localAnchorA.x, m_localAnchorA.y);
            b2Settings.b2Log("  jd.localAnchorB.Set({0:F5}, {1:F5};\n", m_localAnchorB.x, m_localAnchorB.y);
            b2Settings.b2Log("  jd.referenceAngle = {0:F5};\n", m_referenceAngle);
            b2Settings.b2Log("  jd.enableLimit = bool({0});\n", m_enableLimit);
            b2Settings.b2Log("  jd.lowerAngle = {0:F5};\n", m_lowerAngle);
            b2Settings.b2Log("  jd.upperAngle = {0:F5};\n", m_upperAngle);
            b2Settings.b2Log("  jd.enableMotor = bool({0});\n", m_enableMotor);
            b2Settings.b2Log("  jd.motorSpeed = {0:F5};\n", m_motorSpeed);
            b2Settings.b2Log("  jd.maxMotorTorque = {0:F5};\n", m_maxMotorTorque);
            b2Settings.b2Log("  joints[%d] = m_world.CreateJoint(&jd);\n", m_index);
        }
    }
}
