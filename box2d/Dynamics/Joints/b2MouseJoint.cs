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

// p = attached point, m = mouse point
// C = p - m
// Cdot = v
//      = v + cross(w, r)
// J = [I r_skew]
// Identity used:
// w k % (rx i + ry j) = w * (-ry i + rx j)
using System;
using System.Diagnostics;
using Box2D.Common;
namespace Box2D.Dynamics.Joints
{
    /// A mouse joint is used to make a point on a body track a
    /// specified world point. This a soft constraint with a maximum
    /// force. This allows the constraint to stretch and without
    /// applying huge forces.
    /// NOTE: this joint is not documented in the manual because it was
    /// developed to be used in the testbed. If you want to learn how to
    /// use the mouse joint, look at the testbed.
    public class b2MouseJoint : b2Joint
    {
        protected b2Vec2 m_localAnchorB;
        protected b2Vec2 m_targetA;
        protected float m_frequencyHz;
        protected float m_dampingRatio;
        protected float m_beta;

        // Solver shared
        protected b2Vec2 m_impulse;
        protected float m_maxForce;
        protected float m_gamma;

        // Solver temp
        protected int m_indexA;
        protected int m_indexB;
        protected b2Vec2 m_rB;
        protected b2Vec2 m_localCenterB;
        protected float m_invMassB;
        protected float m_invIB;
        protected b2Mat22 m_mass;
        protected b2Vec2 m_C;

        public b2MouseJoint(b2MouseJointDef def)
            : base(def)
        {
            Debug.Assert(def.target.IsValid());
            Debug.Assert(b2Math.b2IsValid(def.maxForce) && def.maxForce >= 0.0f);
            Debug.Assert(b2Math.b2IsValid(def.frequencyHz) && def.frequencyHz >= 0.0f);
            Debug.Assert(b2Math.b2IsValid(def.dampingRatio) && def.dampingRatio >= 0.0f);

            m_targetA = def.target;
            m_localAnchorB = b2Math.b2MulT(m_bodyB.Transform, m_targetA);

            m_maxForce = def.maxForce;
            m_impulse.SetZero();

            m_frequencyHz = def.frequencyHz;
            m_dampingRatio = def.dampingRatio;

            m_beta = 0.0f;
            m_gamma = 0.0f;
        }

        public virtual void SetTarget(b2Vec2 target)
        {
            if (m_bodyB.IsAwake() == false)
            {
                m_bodyB.SetAwake(true);
            }
            m_targetA = target;
        }

        public virtual b2Vec2 GetTarget()
        {
            return m_targetA;
        }

        public virtual void SetMaxForce(float force)
        {
            m_maxForce = force;
        }

        public virtual float GetMaxForce()
        {
            return m_maxForce;
        }

        public virtual void SetFrequency(float hz)
        {
            m_frequencyHz = hz;
        }

        public virtual float GetFrequency()
        {
            return m_frequencyHz;
        }

        public virtual void SetDampingRatio(float ratio)
        {
            m_dampingRatio = ratio;
        }

        public virtual float GetDampingRatio()
        {
            return m_dampingRatio;
        }

        public override void InitVelocityConstraints(b2SolverData data)
        {
            m_indexB = m_bodyB.IslandIndex;
            m_localCenterB = m_bodyB.Sweep.localCenter;
            m_invMassB = m_bodyB.InvertedMass;
            m_invIB = m_bodyB.InvertedI;

            b2Vec2 cB = m_bodyB.InternalPosition.c;
            float aB = m_bodyB.InternalPosition.a;
            b2Vec2 vB = m_bodyB.InternalVelocity.v;
            float wB = m_bodyB.InternalVelocity.w;

            b2Rot qB = new b2Rot(aB);

            float mass = m_bodyB.Mass;

            // Frequency
            float omega = 2.0f * b2Settings.b2_pi * m_frequencyHz;

            // Damping coefficient
            float d = 2.0f * mass * m_dampingRatio * omega;

            // Spring stiffness
            float k = mass * (omega * omega);

            // magic formulas
            // gamma has units of inverse mass.
            // beta has units of inverse time.
            float h = data.step.dt;
            Debug.Assert(d + h * k > b2Settings.b2_epsilon);
            m_gamma = h * (d + h * k);
            if (m_gamma != 0.0f)
            {
                m_gamma = 1.0f / m_gamma;
            }
            m_beta = h * k * m_gamma;

            // Compute the effective mass matrix.
            m_rB = b2Math.b2Mul(qB, m_localAnchorB - m_localCenterB);

            // K    = [(1/m1 + 1/m2) * eye(2) - skew(r1) * invI1 * skew(r1) - skew(r2) * invI2 * skew(r2)]
            //      = [1/m1+1/m2     0    ] + invI1 * [r1.y*r1.y -r1.x*r1.y] + invI2 * [r1.y*r1.y -r1.x*r1.y]
            //        [    0     1/m1+1/m2]           [-r1.x*r1.y r1.x*r1.x]           [-r1.x*r1.y r1.x*r1.x]
            b2Mat22 K = new b2Mat22();
            K.exx = m_invMassB + m_invIB * m_rB.y * m_rB.y + m_gamma;
            K.exy = -m_invIB * m_rB.x * m_rB.y;
            K.eyx = K.ex.y;
            K.eyy = m_invMassB + m_invIB * m_rB.x * m_rB.x + m_gamma;

            m_mass = K.GetInverse();

            m_C = cB + m_rB - m_targetA;
            m_C *= m_beta;

            // Cheat with some damping
            wB *= 0.98f;

            if (data.step.warmStarting)
            {
                m_impulse *= data.step.dtRatio;
                vB += m_invMassB * m_impulse;
                wB += m_invIB * b2Math.b2Cross(m_rB, m_impulse);
            }
            else
            {
                m_impulse.SetZero();
            }

            m_bodyB.InternalVelocity.v = vB;
            m_bodyB.InternalVelocity.w = wB;
        }

        public override void SolveVelocityConstraints(b2SolverData data)
        {
            b2Vec2 vB = m_bodyB.InternalVelocity.v;
            float wB = m_bodyB.InternalVelocity.w;

            // Cdot = v + cross(w, r)
            b2Vec2 Cdot = vB + b2Math.b2Cross(wB, ref m_rB);
            b2Vec2 impulse = b2Math.b2Mul(m_mass, -(Cdot + m_C + m_gamma * m_impulse));

            b2Vec2 oldImpulse = m_impulse;
            m_impulse += impulse;
            float maxImpulse = data.step.dt * m_maxForce;
            if (m_impulse.LengthSquared > maxImpulse * maxImpulse)
            {
                m_impulse *= maxImpulse / m_impulse.Length;
            }
            impulse = m_impulse - oldImpulse;

            vB += m_invMassB * impulse;
            wB += m_invIB * b2Math.b2Cross(m_rB, impulse);

            m_bodyB.InternalVelocity.v = vB;
            m_bodyB.InternalVelocity.w = wB;
        }

        public override bool SolvePositionConstraints(b2SolverData data)
        {
            return true;
        }

        public override b2Vec2 GetAnchorA()
        {
            return m_targetA;
        }

        public override b2Vec2 GetAnchorB()
        {
            return m_bodyB.GetWorldPoint(m_localAnchorB);
        }

        public virtual b2Vec2 GetReactionForce(float inv_dt)
        {
            return inv_dt * m_impulse;
        }

        public virtual float GetReactionTorque(float inv_dt)
        {
            return inv_dt * 0.0f;
        }
    }
}