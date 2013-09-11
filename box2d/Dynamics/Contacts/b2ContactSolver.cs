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
using System;
using System.Diagnostics;
using Box2D.Common;
using Box2D.Collision;
using Box2D.Collision.Shapes;

namespace Box2D.Dynamics.Contacts
{

    public struct b2ContactSolverDef
    {
        public b2TimeStep step;
        public b2Contact[] contacts;
        public int count;
        //public b2Body[] Bodies;
        //public b2Position[] positions;
        //public b2Velocity[] velocities;
    }

    /*
    public class b2VelocityConstraintPoint
    {
        //public static b2VelocityConstraintPoint Zero = b2VelocityConstraintPoint.Create();
  
        public void Defaults() 
        {
            rA = b2Vec2.Zero;
            rB = b2Vec2.Zero;
        }
        
        static public b2VelocityConstraintPoint Create() 
        {
            var vcp = new b2VelocityConstraintPoint();
            vcp.Defaults();
            return vcp;
        }

        public b2Vec2 rA;
        public b2Vec2 rB;
        public float normalImpulse;
        public float tangentImpulse;
        public float normalMass;
        public float tangentMass;
        public float velocityBias;
    }

    public class b2ContactVelocityConstraint
    {
       
		public void Defaults() 
        {
            points = new b2VelocityConstraintPoint[2] {new b2VelocityConstraintPoint(), new b2VelocityConstraintPoint()};
            normal = b2Vec2.Zero;

        }
        
        static public b2ContactVelocityConstraint Create() 
        {
            var cvc = new b2ContactVelocityConstraint();
            cvc.Defaults();
            return cvc;
        }

        public b2VelocityConstraintPoint[] points;
        public b2Vec2 normal;
        public b2Mat22 normalMass;
        public b2Mat22 K;
        public int indexA;
        public int indexB;
        public float invMassA, invMassB;
        public float invIA, invIB;
        public float friction;
        public float restitution;
        public int pointCount;
        public int contactIndex;
    }

    public class b2ContactPositionConstraint
    {
        //public static b2ContactPositionConstraint Default = b2ContactPositionConstraint.Create();
        
        public void Defaults() 
        {
            localPoints = new b2Vec2[] { b2Vec2.Zero, b2Vec2.Zero};
            localNormal = b2Vec2.Zero;
            localPoint = b2Vec2.Zero;

        }
        
        static public b2ContactPositionConstraint Create() 
        {
            var cvc = new b2ContactPositionConstraint();
            cvc.Defaults();
            return cvc;
        }

        public b2Vec2[] localPoints;
        public b2Vec2 localNormal;
        public b2Vec2 localPoint;
        public int indexA;
        public int indexB;
        public float invMassA, invMassB;
        public b2Vec2 localCenterA, localCenterB;
        public float invIA, invIB;
        public b2ManifoldType type;
        public float radiusA, radiusB;
        public int pointCount;
    };
    */
    public class b2ConstraintPoint
    {
        //Position
        public b2Vec2 localPoint;
        
        //Velocity
        public b2Vec2 rA;
        public b2Vec2 rB;
        public float normalImpulse;
        public float tangentImpulse;
        public float normalMass;
        public float tangentMass;
        public float velocityBias;
    }

    public class b2ContactConstraint
    {
        public b2Body BodyA;
        public b2Body BodyB;
        public float invMassA, invMassB;
        public float invIA, invIB;

        public b2ConstraintPoint[] points = new [] {new b2ConstraintPoint(), new b2ConstraintPoint()};

        //Position
        public b2Vec2 localNormal;
        public b2Vec2 localPoint;
        public b2Vec2 localCenterA, localCenterB;
        public b2ManifoldType type;
        public float radiusA, radiusB;

        public b2Vec2 c;
        public float a;

        //Velocity
        public b2Vec2 normal;
        public b2Mat22 normalMass;
        public b2Mat22 K;
        public float friction;
        public float restitution;
        
        public b2Contact contact;

        public b2Vec2 v;
        public float w;

        public int pointCount;
    };

    internal class b2ContactSolver : b2ReusedObject<b2ContactSolver>
    {
        public b2TimeStep m_step;
        //private b2Velocity[] m_velocities;
        //private b2Position[] m_positions;
        public b2ContactConstraint[] m_constraints;
        //public b2Contact[] m_contacts;
        public int m_count;

        public static b2ContactSolver Create(ref b2ContactSolverDef def)
        {
            var result = Create();
            result.Init(ref def);
            return result;
        }

        private void Init(ref b2ContactSolverDef def)
        {
            if (m_constraints == null || def.count > m_constraints.Length)
            {
                var count = def.count;
                var oldCount = 0;

                var capacity = 4;
                while (capacity < count)
                {
                    capacity *= 2;
                }

                if (m_constraints == null)
                {
                    m_constraints = new b2ContactConstraint[capacity];
                }
                else
                {
                    oldCount = m_constraints.Length;
                    Array.Resize(ref m_constraints, capacity);
                }

                for (int i = oldCount; i < capacity; i++)
                {
                    m_constraints[i] = new b2ContactConstraint();
                }
            }

            m_step = def.step;
            m_count = def.count;

            //m_positions = def.positions;
            //m_velocities = def.velocities;
            //m_contacts = def.contacts;
            var contacts = def.contacts;

            // Initialize position independent portions of the constraints.
            for (int i = 0, count = m_count; i < count; ++i)
            {
                b2Contact contact = contacts[i];

                b2Fixture fixtureA = contact.FixtureA;
                b2Fixture fixtureB = contact.FixtureB;
                b2Shape shapeA = fixtureA.Shape;
                b2Shape shapeB = fixtureB.Shape;
                float radiusA = shapeA.Radius;
                float radiusB = shapeB.Radius;
                b2Body bodyA = fixtureA.Body;
                b2Body bodyB = fixtureB.Body;
                b2Manifold manifold = contact.m_manifold;

                int pointCount = manifold.pointCount;
                Debug.Assert(pointCount > 0);

                var constraint = m_constraints[i];

                constraint.friction = contact.Friction;
                constraint.restitution = contact.Restitution;
                constraint.BodyA = bodyA;
                constraint.BodyB = bodyB;
                constraint.invMassA = bodyA.InvertedMass;
                constraint.invMassB = bodyB.InvertedMass;
                constraint.invIA = bodyA.InvertedI;
                constraint.invIB = bodyB.InvertedI;
                constraint.contact = contact;
                constraint.pointCount = pointCount;
                constraint.K = b2Mat22.Zero;
                constraint.normalMass = b2Mat22.Zero;

                //constraint.indexA = bodyA.IslandIndex;
                //constraint.indexB = bodyB.IslandIndex;
                //constraint.invMassA = bodyA.InvertedMass;
                //constraint.invMassB = bodyB.InvertedMass;
                constraint.localCenterA = bodyA.Sweep.localCenter;
                constraint.localCenterB = bodyB.Sweep.localCenter;
                //constraint.invIA = bodyA.InvertedI;
                //constraint.invIB = bodyB.InvertedI;
                constraint.localNormal = manifold.localNormal;
                constraint.localPoint = manifold.localPoint;
                //constraint.pointCount = pointCount;
                constraint.radiusA = radiusA;
                constraint.radiusB = radiusB;
                constraint.type = manifold.type;

                for (int j = 0; j < pointCount; ++j)
                {
                    b2ManifoldPoint cp = manifold.points[j];
                    var vcp = constraint.points[j];

                    if (m_step.warmStarting)
                    {
                        vcp.normalImpulse = m_step.dtRatio * cp.normalImpulse;
                        vcp.tangentImpulse = m_step.dtRatio * cp.tangentImpulse;
                    }
                    else
                    {
                        vcp.normalImpulse = 0.0f;
                        vcp.tangentImpulse = 0.0f;
                    }

                    vcp.rA = b2Vec2.Zero;
                    vcp.rB = b2Vec2.Zero;
                    vcp.normalMass = 0.0f;
                    vcp.tangentMass = 0.0f;
                    vcp.velocityBias = 0.0f;

                    constraint.points[j].localPoint = cp.localPoint;
                }
            }
        }

        //Cached WorldManifold
        private b2WorldManifold _b2WorldManifold = new b2WorldManifold();

        // Initialize position dependent portions of the velocity constraints.
        public virtual void InitializeVelocityConstraints()
        {
#if false
            for (int i = 0; i < m_count; ++i)
            {
                b2ContactVelocityConstraint vc = m_velocityConstraints[i];
                b2ContactPositionConstraint pc = m_positionConstraints[i];

                float radiusA = pc.radiusA;
                float radiusB = pc.radiusB;
                b2Manifold manifold = m_contacts[vc.contactIndex].GetManifold();

                int indexA = vc.indexA;
                int indexB = vc.indexB;

                float mA = vc.invMassA;
                float mB = vc.invMassB;
                float iA = vc.invIA;
                float iB = vc.invIB;
                b2Vec2 localCenterA = pc.localCenterA;
                b2Vec2 localCenterB = pc.localCenterB;

                b2Vec2 cA = m_positions[indexA].c;
                float aA = m_positions[indexA].a;
                b2Vec2 vA = m_velocities[indexA].v;
                float wA = m_velocities[indexA].w;

                b2Vec2 cB = m_positions[indexB].c;
                float aB = m_positions[indexB].a;
                b2Vec2 vB = m_velocities[indexB].v;
                float wB = m_velocities[indexB].w;

                Debug.Assert(manifold.pointCount > 0);

                b2Transform xfA = b2Transform.Identity, xfB = b2Transform.Identity;
                xfA.q.Set(aA);
                xfB.q.Set(aB);
                xfA.p = cA - b2Math.b2Mul(ref xfA.q, ref localCenterA);
                xfB.p = cB - b2Math.b2Mul(ref xfB.q, ref localCenterB);

                //b2WorldManifold worldManifold = new b2WorldManifold();
                b2WorldManifold worldManifold = _b2WorldManifold;
                worldManifold.Initialize(ref manifold, xfA, radiusA, xfB, radiusB);

                vc.normal = worldManifold.normal;

                int pointCount = vc.pointCount;
                for (int j = 0; j < pointCount; ++j)
                {
                    b2VelocityConstraintPoint vcp = vc.points[j];

                    vcp.rA = worldManifold.points[j] - cA;
                    vcp.rB = worldManifold.points[j] - cB;

                    float rnA = b2Math.b2Cross(ref vcp.rA, ref vc.normal);
                    float rnB = b2Math.b2Cross(ref vcp.rB, ref vc.normal);

                    float kNormal = mA + mB + iA * rnA * rnA + iB * rnB * rnB;

                    vcp.normalMass = kNormal > 0.0f ? 1.0f / kNormal : 0.0f;

                    b2Vec2 tangent = vc.normal.UnitCross(); //  b2Math.b2Cross(vc.normal, 1.0f);

                    float rtA = b2Math.b2Cross(ref vcp.rA, ref tangent);
                    float rtB = b2Math.b2Cross(ref vcp.rB, ref tangent);

                    float kTangent = mA + mB + iA * rtA * rtA + iB * rtB * rtB;

                    vcp.tangentMass = kTangent > 0.0f ? 1.0f / kTangent : 0.0f;

                    // Setup a velocity bias for restitution.
                    vcp.velocityBias = 0.0f;
                    float vRel = b2Math.b2Dot(vc.normal, vB + b2Math.b2Cross(wB, ref vcp.rB) - vA - b2Math.b2Cross(wA, ref vcp.rA));
                    if (vRel < -b2Settings.b2_velocityThreshold)
                    {
                        vcp.velocityBias = -vc.restitution * vRel;
                    }
                }

                // If we have two points, then prepare the block solver.
                if (vc.pointCount == 2)
                {
                    b2VelocityConstraintPoint vcp1 = vc.points[0];
                    b2VelocityConstraintPoint vcp2 = vc.points[1];

                    float rn1A = b2Math.b2Cross(ref vcp1.rA, ref vc.normal);
                    float rn1B = b2Math.b2Cross(ref vcp1.rB, ref vc.normal);
                    float rn2A = b2Math.b2Cross(ref vcp2.rA, ref vc.normal);
                    float rn2B = b2Math.b2Cross(ref vcp2.rB, ref vc.normal);

                    float k11 = mA + mB + iA * rn1A * rn1A + iB * rn1B * rn1B;
                    float k22 = mA + mB + iA * rn2A * rn2A + iB * rn2B * rn2B;
                    float k12 = mA + mB + iA * rn1A * rn2A + iB * rn1B * rn2B;

                    // Ensure a reasonable condition number.
                    float k_maxConditionNumber = 1000.0f;
                    if (k11 * k11 < k_maxConditionNumber * (k11 * k22 - k12 * k12))
                    {
                        // K is safe to invert.
                        vc.K.ex.Set(k11, k12);
                        vc.K.ey.Set(k12, k22);
                        vc.normalMass = vc.K.GetInverse();
                    }
                    else
                    {
                        // The constraints are redundant, just use one.
                        // TODO_ERIN use deepest?
                        vc.pointCount = 1;
                    }
                }
            }
#else
            for (int i = 0, count = m_count; i < count; ++i)
            {
                var vc = m_constraints[i];

                float radiusA = vc.radiusA;
                float radiusB = vc.radiusB;
                b2Manifold manifold = vc.contact.m_manifold;

                var bodyA = vc.BodyA;
                var bodyB = vc.BodyB;

                float mA = vc.invMassA;
                float mB = vc.invMassB;
                float iA = vc.invIA;
                float iB = vc.invIB;
                b2Vec2 localCenterA = vc.localCenterA;
                b2Vec2 localCenterB = vc.localCenterB;

                b2Vec2 cA = bodyA.InternalPosition.c;
                float aA = bodyA.InternalPosition.a;
                b2Vec2 vA = bodyA.InternalVelocity.v;
                float wA = bodyA.InternalVelocity.w;

                b2Vec2 cB = bodyB.InternalPosition.c;
                float aB = bodyB.InternalPosition.a;
                b2Vec2 vB = bodyB.InternalVelocity.v;
                float wB = bodyB.InternalVelocity.w;

                Debug.Assert(manifold.pointCount > 0);

                b2Transform xfA, xfB;

                xfA.q.s = (float)Math.Sin(aA);
                xfA.q.c = (float)Math.Cos(aA);

                xfB.q.s = (float)Math.Sin(aB);
                xfB.q.c = (float)Math.Cos(aB);

                xfA.p.x = cA.x - (xfA.q.c * localCenterA.x - xfA.q.s * localCenterA.y);
                xfA.p.y = cA.y - (xfA.q.s * localCenterA.x + xfA.q.c * localCenterA.y);
                
                xfB.p.x = cB.x - (xfB.q.c * localCenterB.x - xfB.q.s * localCenterB.y);
                xfB.p.y = cB.y - (xfB.q.s * localCenterB.x + xfB.q.c * localCenterB.y);

                //b2WorldManifold worldManifold = new b2WorldManifold();
                b2WorldManifold worldManifold = _b2WorldManifold;
                worldManifold.Initialize(manifold, ref xfA, radiusA, ref xfB, radiusB);

                vc.normal = worldManifold.normal;

                float normalx = vc.normal.x;
                float normaly = vc.normal.y;

                float tangentx = normaly; //  b2Math.b2Cross(vc.normal, 1.0f);
                float tangenty = -normalx;

                int pointCount = vc.pointCount;
                for (int j = 0; j < pointCount; ++j)
                {
                    var vcp = vc.points[j];

                    var point = worldManifold.points[j];

                    vcp.rA.x = point.x - cA.x;
                    vcp.rA.y = point.y - cA.y;

                    vcp.rB.x = point.x - cB.x;
                    vcp.rB.y = point.y - cB.y;

                    float rnA = vcp.rA.x * normaly - vcp.rA.y * normalx;
                    float rnB = vcp.rB.x * normaly - vcp.rB.y * normalx;

                    float kNormal = mA + mB + iA * rnA * rnA + iB * rnB * rnB;

                    vcp.normalMass = kNormal > 0.0f ? 1.0f / kNormal : 0.0f;

                    float rtA = vcp.rA.x * tangenty - vcp.rA.y * tangentx;
                    float rtB = vcp.rB.x * tangenty - vcp.rB.y * tangentx;

                    float kTangent = mA + mB + iA * rtA * rtA + iB * rtB * rtB;

                    vcp.tangentMass = kTangent > 0.0f ? 1.0f / kTangent : 0.0f;

                    // Setup a velocity bias for restitution.
                    vcp.velocityBias = 0.0f;

                    float bx = -wB * vcp.rB.y;
                    float by = wB * vcp.rB.x;
                    float ax = -wA * vcp.rA.y;
                    float ay = wA * vcp.rA.x;

                    float vRel = normalx * (vB.x + bx - vA.x - ax) + normaly * (vB.y + by - vA.y - ay);
                    if (vRel < -b2Settings.b2_velocityThreshold)
                    {
                        vcp.velocityBias = -vc.restitution * vRel;
                    }
                }

                // If we have two points, then prepare the block solver.
                if (vc.pointCount == 2)
                {
                    var vcp1 = vc.points[0];
                    var vcp2 = vc.points[1];

                    float rn1A = vcp1.rA.x * normaly - vcp1.rA.y * normalx;
                    float rn1B = vcp1.rB.x * normaly - vcp1.rB.y * normalx;
                    float rn2A = vcp2.rA.x * normaly - vcp2.rA.y * normalx;
                    float rn2B = vcp2.rB.x * normaly - vcp2.rB.y * normalx;

                    float k11 = mA + mB + iA * rn1A * rn1A + iB * rn1B * rn1B;
                    float k22 = mA + mB + iA * rn2A * rn2A + iB * rn2B * rn2B;
                    float k12 = mA + mB + iA * rn1A * rn2A + iB * rn1B * rn2B;

                    // Ensure a reasonable condition number.
                    float k_maxConditionNumber = 1000.0f;
                    if (k11 * k11 < k_maxConditionNumber * (k11 * k22 - k12 * k12))
                    {
                        vc.K.ex.x = k11;
                        vc.K.ex.y = k12;
                        vc.K.ey.x = k12;
                        vc.K.ey.y = k22;
                        vc.K.GetInverse(out vc.normalMass);
                    }
                    else
                    {
                        // The constraints are redundant, just use one.
                        // TODO_ERIN use deepest?
                        vc.pointCount = 1;
                    }
                }
            }
#endif
        }

        public virtual void WarmStart()
        {
            // Warm start.
            for (int i = 0; i < m_count; ++i)
            {
                var vc = m_constraints[i];

                var bodyA = vc.BodyA;
                var bodyB = vc.BodyB;
                float mA = vc.invMassA;
                float iA = vc.invIA;
                float mB = vc.invMassB;
                float iB = vc.invIB;
                int pointCount = vc.pointCount;

                b2Vec2 vA = bodyA.InternalVelocity.v;
                float wA = bodyA.InternalVelocity.w;
                b2Vec2 vB = bodyB.InternalVelocity.v;
                float wB = bodyB.InternalVelocity.w;

                b2Vec2 normal = vc.normal;
                b2Vec2 tangent = normal.UnitCross(); //  b2Math.b2Cross(normal, 1.0f);

                for (int j = 0; j < pointCount; ++j)
                {
                    var vcp = vc.points[j];

                    b2Vec2 P;
                    P.x = vcp.normalImpulse * normal.x + vcp.tangentImpulse * tangent.x;
                    P.y = vcp.normalImpulse * normal.y + vcp.tangentImpulse * tangent.y;

                    wA -= iA * b2Math.b2Cross(ref vcp.rA, ref P);

                    vA.x -= mA * P.x;
                    vA.y -= mA * P.y;
                    
                    wB += iB * b2Math.b2Cross(ref vcp.rB, ref P);

                    vB.x += mB * P.x;
                    vB.y += mB * P.y;
                }

                bodyA.InternalVelocity.v = vA;
                bodyA.InternalVelocity.w = wA;
                bodyB.InternalVelocity.v = vB;
                bodyB.InternalVelocity.w = wB;
            }
        }

        public virtual void SolveVelocityConstraints()
        {
            for (int i = 0; i < m_count; ++i)
            {
                var vc = m_constraints[i];

                var bodyA = vc.BodyA;
                var bodyB = vc.BodyB;

                float mA = vc.invMassA;
                float iA = vc.invIA;
                float mB = vc.invMassB;
                float iB = vc.invIB;
                int pointCount = vc.pointCount;

                b2Vec2 vA = bodyA.InternalVelocity.v;
                float wA = bodyA.InternalVelocity.w;
                b2Vec2 vB = bodyB.InternalVelocity.v;
                float wB = bodyB.InternalVelocity.w;

                float normalx = vc.normal.x;
                float normaly = vc.normal.y;

                float tangentx = normaly; // b2Math.b2Cross(normal, 1.0f);
                float tangenty = -normalx;
                float friction = vc.friction;


                Debug.Assert(pointCount == 1 || pointCount == 2);

                // Solve tangent constraints first because non-penetration is more important
                // than friction.
                for (int j = 0; j < pointCount; ++j)
                {
                    var vcp = vc.points[j];

                    // Relative velocity at contact
                    /*
                        b.m_x = -s * a.m_y;
                        b.m_y = s * a.m_x;
                     */

                    // b2Vec2 dv = vB + b2Math.b2Cross(wB, ref vcp.rB) - vA - b2Math.b2Cross(wA, ref vcp.rA);
                    float dvx = vB.x + (-wB * vcp.rB.y) - vA.x - (-wA * vcp.rA.y);
                    float dvy = vB.y + (wB * vcp.rB.x) - vA.y - (wA * vcp.rA.x);

                    // Compute tangent force
                    float vt = dvx * tangentx + dvy * tangenty; // b2Math.b2Dot(dv, tangent);
                    float lambda = vcp.tangentMass * (-vt);

                    // b2Math.b2Clamp the accumulated force
                    float maxFriction = friction * vcp.normalImpulse;
                    float newImpulse = vcp.tangentImpulse + lambda;
                    if (newImpulse < -maxFriction)
                    {
                        newImpulse = -maxFriction;
                    }
                    else if (newImpulse > maxFriction)
                    {
                        newImpulse = maxFriction;
                    }

                    lambda = newImpulse - vcp.tangentImpulse;
                    vcp.tangentImpulse = newImpulse;

                    // Apply contact impulse
                    // P = lambda * tangent;
                    float Px = lambda * tangentx;
                    float Py = lambda * tangenty;

                    // vA -= mA * P;
                    vA.x -= mA * Px;
                    vA.y -= mA * Py;

                   // wA -= iA * b2Math.b2Cross(vcp.rA, P);
                    wA -= iA * (vcp.rA.x * Py - vcp.rA.y * Px);

                    // vB += mB * P;
                    vB.x += mB * Px;
                    vB.y += mB * Py; 

                    // wB += iB * b2Math.b2Cross(vcp.rB, P);
                    wB += iB * (vcp.rB.x * Py - vcp.rB.y * Px);

                    //vc.points[j] = vcp;
                }

                // Solve normal constraints
                if (vc.pointCount == 1)
                {
                    var vcp = vc.points[0];

                    // Relative velocity at contact
                    // b2Vec2 dv = vB + b2Math.b2Cross(wB, ref vcp.rB) - vA - b2Math.b2Cross(wA, ref vcp.rA);
                    float dvx = vB.x + (-wB * vcp.rB.y) - vA.x - (-wA * vcp.rA.y);
                    float dvy = vB.y + (wB * vcp.rB.x) - vA.y - (wA * vcp.rA.x);

                    // Compute normal impulse
                    float vn = dvx * normalx + dvy * normaly; //b2Math.b2Dot(ref dv, ref normal);
                    float lambda = -vcp.normalMass * (vn - vcp.velocityBias);

                    // b2Math.b2Clamp the accumulated impulse
                    float newImpulse = vcp.normalImpulse + lambda;
                    if (newImpulse < 0f)
                    {
                        newImpulse = 0f;
                    }
                    lambda = newImpulse - vcp.normalImpulse;
                    vcp.normalImpulse = newImpulse;

                    // Apply contact impulse
                    //b2Vec2 P = lambda * normal;
                    float Px = lambda * normalx;
                    float Py = lambda * normaly;

                    // vA -= mA * P;
                    vA.x -= mA * Px;
                    vA.y -= mA * Py;

                    // wA -= iA * b2Math.b2Cross(vcp.rA, P);
                    wA -= iA * (vcp.rA.x * Py - vcp.rA.y * Px);

                    // vB += mB * P;
                    vB.x += mB * Px;
                    vB.y += mB * Py;

                    // wB += iB * b2Math.b2Cross(vcp.rB, P);
                    wB += iB * (vcp.rB.x * Py - vcp.rB.y * Px);
                    
                    //vc.points[0] = vcp;
                }
                else
                {
                    // Block solver developed in collaboration with Dirk Gregorius (back in 01/07 on Box2D_Lite).
                    // Build the mini LCP for this contact patch
                    //
                    // vn = A * x + b, vn >= 0, , vn >= 0, x >= 0 and vn_i * x_i = 0 with i = 1..2
                    //
                    // A = J * W * JT and J = ( -n, -r1 x n, n, r2 x n )
                    // b = vn0 - velocityBias
                    //
                    // The system is solved using the "Total enumeration method" (s. Murty). The complementary constraint vn_i * x_i
                    // implies that we must have in any solution either vn_i = 0 or x_i = 0. So for the 2D contact problem the cases
                    // vn1 = 0 and vn2 = 0, x1 = 0 and x2 = 0, x1 = 0 and vn2 = 0, x2 = 0 and vn1 = 0 need to be tested. The first valid
                    // solution that satisfies the problem is chosen.
                    // 
                    // In order to account of the accumulated impulse 'a' (because of the iterative nature of the solver which only requires
                    // that the accumulated impulse is clamped and not the incremental impulse) we change the impulse variable (x_i).
                    //
                    // Substitute:
                    // 
                    // x = a + d
                    // 
                    // a := old total impulse
                    // x := new total impulse
                    // d := incremental impulse 
                    //
                    // For the current iteration we extend the formula for the incremental impulse
                    // to compute the new total impulse:
                    //
                    // vn = A * d + b
                    //    = A * (x - a) + b
                    //    = A * x + b - A * a
                    //    = A * x + b'
                    // b' = b - A * a;

                    var cp1 = vc.points[0];
                    var cp2 = vc.points[1];

                    float ax = cp1.normalImpulse;
                    float ay = cp2.normalImpulse;
                    Debug.Assert(ax >= 0.0f && ay >= 0.0f);

                    // Relative velocity at contact
                    // vB + b2Math.b2Cross(wB, ref cp1.rB) - vA - b2Math.b2Cross(wA, ref cp1.rA);
                    float dv1x = vB.x + (-wB * cp1.rB.y) - vA.x - (-wA * cp1.rA.y);
                    float dv1y = vB.y + (wB * cp1.rB.x) - vA.y - (wA * cp1.rA.x);

                    // vB + b2Math.b2Cross(wB, ref cp2.rB) - vA - b2Math.b2Cross(wA, ref cp2.rA);
                    float dv2x = vB.x + (-wB * cp2.rB.y) - vA.x - (-wA * cp2.rA.y);
                    float dv2y = vB.y + (wB * cp2.rB.x) - vA.y - (wA * cp2.rA.x);

                    // Compute normal velocity
                    float vn1 = dv1x * normalx + dv1y * normaly;// b2Math.b2Dot(ref dv1, ref normal);
                    float vn2 = dv2x * normalx + dv2y * normaly;// b2Math.b2Dot(ref dv2, ref normal);

                    float bx = vn1 - cp1.velocityBias;
                    float by = vn2 - cp2.velocityBias;

                    // Compute b'
                    // (A.ex.x * v.x + A.ey.x * v.y, A.ex.y * v.x + A.ey.y * v.y)
                    bx -= (vc.K.ex.x * ax + vc.K.ey.x * ay);
                    by -= (vc.K.ex.y * ax + vc.K.ey.y * ay);
                    // b -= b2Math.b2Mul(vc.K, a);

                    //            float k_errorTol = 1e-3f;
                    #region Iteration 
                    while (true)
                    {
                        //
                        // Case 1: vn = 0
                        //
                        // 0 = A * x + b'
                        //
                        // Solve for x:
                        //
                        // x = - inv(A) * b'
                        //
                        float xx = -(vc.normalMass.ex.x * bx + vc.normalMass.ey.x * by);
                        float xy = -(vc.normalMass.ex.y * bx + vc.normalMass.ey.y * by);

                        if (xx >= 0.0f && xy >= 0.0f)
                        {
                            // Get the incremental impulse
                            float dx = xx - ax;
                            float dy = xy - ay;

                            // Apply incremental impulse
                            float P1x = dx * normalx;
                            float P1y = dx * normaly;

                            float P2x = dy * normalx;
                            float P2y = dy * normaly;

                            float P12x = P1x + P2x;
                            float P12y = P1y + P2y;

                            vA.x -= mA * P12x;
                            vA.y -= mA * P12y;
                            wA -= iA * (cp1.rA.x * P1y - cp1.rA.y * P1x + (cp2.rA.x * P2y - cp2.rA.y * P2x));

                            vB.x += mB * P12x;
                            vB.y += mB * P12y;
                            wB += iB * (cp1.rB.x * P1y - cp1.rB.y * P1x + (cp2.rB.x * P2y - cp2.rB.y * P2x));

                            // Accumulate
                            cp1.normalImpulse = xx;
                            cp2.normalImpulse = xy;

#if B2_DEBUG_SOLVER
                    // Postconditions
                    dv1 = vB + b2Math.b2Cross(wB, cp1.rB) - vA - b2Math.b2Cross(wA, cp1.rA);
                    dv2 = vB + b2Math.b2Cross(wB, cp2.rB) - vA - b2Math.b2Cross(wA, cp2.rA);

                    // Compute normal velocity
                    vn1 = b2Math.b2Dot(dv1, normal);
                    vn2 = b2Math.b2Dot(dv2, normal);

                    Debug.Assert(b2Abs(vn1 - cp1.velocityBias) < k_errorTol);
                    Debug.Assert(b2Abs(vn2 - cp2.velocityBias) < k_errorTol);
#endif
                            break;
                        }

                        //
                        // Case 2: vn1 = 0 and x2 = 0
                        //
                        //   0 = a11 * x1 + a12 * 0 + b1' 
                        // vn2 = a21 * x1 + a22 * 0 + b2'
                        //
                        xx = -cp1.normalMass * bx;
                        xy = 0.0f;
                        vn1 = 0.0f;
                        vn2 = vc.K.ex.y * xx + by;

                        if (xx >= 0.0f && vn2 >= 0.0f)
                        {
                            // Get the incremental impulse
                            float dx = xx - ax;
                            float dy = xy - ay;

                            // Apply incremental impulse
                            float P1x = dx * normalx;
                            float P1y = dx * normaly;

                            float P2x = dy * normalx;
                            float P2y = dy * normaly;

                            float P12x = P1x + P2x;
                            float P12y = P1y + P2y;

                            vA.x -= mA * P12x;
                            vA.y -= mA * P12y;
                            wA -= iA * (cp1.rA.x * P1y - cp1.rA.y * P1x + (cp2.rA.x * P2y - cp2.rA.y * P2x));

                            vB.x += mB * P12x;
                            vB.y += mB * P12y;
                            wB += iB * (cp1.rB.x * P1y - cp1.rB.y * P1x + (cp2.rB.x * P2y - cp2.rB.y * P2x));

                            // Accumulate
                            cp1.normalImpulse = xx;
                            cp2.normalImpulse = xy;

#if B2_DEBUG_SOLVER
                    // Postconditions
                    dv1 = vB + b2Math.b2Cross(wB, cp1.rB) - vA - b2Math.b2Cross(wA, cp1.rA);

                    // Compute normal velocity
                    vn1 = b2Math.b2Dot(dv1, normal);

                    Debug.Assert(b2Abs(vn1 - cp1.velocityBias) < k_errorTol);
#endif
                            break;
                        }


                        //
                        // Case 3: vn2 = 0 and x1 = 0
                        //
                        // vn1 = a11 * 0 + a12 * x2 + b1' 
                        //   0 = a21 * 0 + a22 * x2 + b2'
                        //
                        xx = 0.0f;
                        xy = -cp2.normalMass * by;
                        vn1 = vc.K.ey.x * xy + bx;
                        vn2 = 0.0f;

                        if (xy >= 0.0f && vn1 >= 0.0f)
                        {
                            // Resubstitute for the incremental impulse
                            float dx = xx - ax;
                            float dy = xy - ay;

                            // Apply incremental impulse
                            float P1x = dx * normalx;
                            float P1y = dx * normaly;

                            float P2x = dy * normalx;
                            float P2y = dy * normaly;

                            float P12x = P1x + P2x;
                            float P12y = P1y + P2y;

                            vA.x -= mA * P12x;
                            vA.y -= mA * P12y;
                            wA -= iA * (cp1.rA.x * P1y - cp1.rA.y * P1x + (cp2.rA.x * P2y - cp2.rA.y * P2x));

                            vB.x += mB * P12x;
                            vB.y += mB * P12y;
                            wB += iB * (cp1.rB.x * P1y - cp1.rB.y * P1x + (cp2.rB.x * P2y - cp2.rB.y * P2x));

                            // Accumulate
                            cp1.normalImpulse = xx;
                            cp2.normalImpulse = xy;

#if B2_DEBUG_SOLVER
                    // Postconditions
                    dv2 = vB + b2Math.b2Cross(wB, cp2.rB) - vA - b2Math.b2Cross(wA, cp2.rA);

                    // Compute normal velocity
                    vn2 = b2Math.b2Dot(dv2, normal);

                    Debug.Assert(b2Abs(vn2 - cp2.velocityBias) < k_errorTol);
#endif
                            break;
                        }

                        //
                        // Case 4: x1 = 0 and x2 = 0
                        // 
                        // vn1 = b1
                        // vn2 = b2;
                        xx = 0.0f;
                        xy = 0.0f;
                        vn1 = bx;
                        vn2 = by;

                        if (vn1 >= 0.0f && vn2 >= 0.0f)
                        {
                            // Resubstitute for the incremental impulse
                            float dx = xx - ax;
                            float dy = xy - ay;

                            // Apply incremental impulse
                            float P1x = dx * normalx;
                            float P1y = dx * normaly;

                            float P2x = dy * normalx;
                            float P2y = dy * normaly;

                            float P12x = P1x + P2x;
                            float P12y = P1y + P2y;

                            vA.x -= mA * P12x;
                            vA.y -= mA * P12y;
                            wA -= iA * (cp1.rA.x * P1y - cp1.rA.y * P1x + (cp2.rA.x * P2y - cp2.rA.y * P2x));

                            vB.x += mB * P12x;
                            vB.y += mB * P12y;
                            wB += iB * (cp1.rB.x * P1y - cp1.rB.y * P1x + (cp2.rB.x * P2y - cp2.rB.y * P2x));

                            // Accumulate
                            cp1.normalImpulse = xx;
                            cp2.normalImpulse = xy;

                            break;
                        }


                        // No solution, give up. This is hit sometimes, but it doesn't seem to matter.
                        break;
                    }
                    #endregion
                }

                bodyA.InternalVelocity.v = vA;
                bodyA.InternalVelocity.w = wA;
                bodyB.InternalVelocity.v = vB;
                bodyB.InternalVelocity.w = wB;
            }
        }

        public virtual void StoreImpulses()
        {
            for (int i = 0, count = m_count; i < count; ++i)
            {
                var vc = m_constraints[i];
                b2Manifold manifold = vc.contact.m_manifold;

                for (int j = 0; j < vc.pointCount; ++j)
                {
                    manifold.points[j].normalImpulse = vc.points[j].normalImpulse;
                    manifold.points[j].tangentImpulse = vc.points[j].tangentImpulse;
                }
                //m_contacts[vc.contactIndex].SetManifold(ref manifold);
            }
        }

        public struct b2PositionSolverManifold
        {
            public b2PositionSolverManifold(b2ContactConstraint pc, ref b2Transform xfA, ref b2Transform xfB, int index)
            {
                Debug.Assert(pc.pointCount > 0);

                switch (pc.type)
                {
                    case b2ManifoldType.e_circles:
                        {
                            b2Vec2 pointA;
                            pointA.x = (xfA.q.c * pc.localPoint.x - xfA.q.s * pc.localPoint.y) + xfA.p.x;
                            pointA.y = (xfA.q.s * pc.localPoint.x + xfA.q.c * pc.localPoint.y) + xfA.p.y;

                            var lc = pc.points[0].localPoint;
                            b2Vec2 pointB;
                            pointB.x = (xfB.q.c * lc.x - xfB.q.s * lc.y) + xfB.p.x;
                            pointB.y = (xfB.q.s * lc.x + xfB.q.c * lc.y) + xfB.p.y;

                            normal = pointB - pointA;
                            normal.Normalize();
                            point = 0.5f * (pointA + pointB);
                            b2Vec2 a = pointB - pointA;
                            separation = a.x * normal.x + a.y * normal.y - pc.radiusA - pc.radiusB;
                        }
                        break;

                    case b2ManifoldType.e_faceA:
                        {
                            normal.x = xfA.q.c * pc.localNormal.x - xfA.q.s * pc.localNormal.y;
                            normal.y = xfA.q.s * pc.localNormal.x + xfA.q.c * pc.localNormal.y;

                            b2Vec2 planePoint;
                            planePoint.x = (xfA.q.c * pc.localPoint.x - xfA.q.s * pc.localPoint.y) + xfA.p.x;
                            planePoint.y = (xfA.q.s * pc.localPoint.x + xfA.q.c * pc.localPoint.y) + xfA.p.y;

                            var lc = pc.points[index].localPoint;
                            b2Vec2 clipPoint;
                            clipPoint.x = (xfB.q.c * lc.x - xfB.q.s * lc.y) + xfB.p.x;
                            clipPoint.y = (xfB.q.s * lc.x + xfB.q.c * lc.y) + xfB.p.y;

                            b2Vec2 rCP = clipPoint - planePoint;
                            separation = rCP.x * normal.x + rCP.y * normal.y - pc.radiusA - pc.radiusB;
                            point = clipPoint;
                        }
                        break;

                    case b2ManifoldType.e_faceB:
                        {
                            normal.x = xfB.q.c * pc.localNormal.x - xfB.q.s * pc.localNormal.y;
                            normal.y = xfB.q.s * pc.localNormal.x + xfB.q.c * pc.localNormal.y;
                            
                            b2Vec2 planePoint;
                            planePoint.x = (xfB.q.c * pc.localPoint.x - xfB.q.s * pc.localPoint.y) + xfB.p.x;
                            planePoint.y = (xfB.q.s * pc.localPoint.x + xfB.q.c * pc.localPoint.y) + xfB.p.y;

                            var lc = pc.points[index].localPoint;
                            b2Vec2 clipPoint;
                            clipPoint.x = (xfA.q.c * lc.x - xfA.q.s * lc.y) + xfA.p.x;
                            clipPoint.y = (xfA.q.s * lc.x + xfA.q.c * lc.y) + xfA.p.y;

                            b2Vec2 rCP = clipPoint - planePoint;
                            separation = rCP.x * normal.x + rCP.y * normal.y - pc.radiusA - pc.radiusB;
                            point = clipPoint;

                            // Ensure normal points from A to B
                            normal = -normal;
                        }
                        break;

                    default:
                        throw new ArgumentException();
                }
            }

            public b2Vec2 normal;
            public b2Vec2 point;
            public float separation;
        }

        // Sequential solver.
        public bool SolvePositionConstraints()
        {
            float minSeparation = 0.0f;

            var constraints = m_constraints;

            for (int i = 0, count = m_count; i < count; ++i)
            {
                var pc = constraints[i];

                var bodyA = pc.BodyA;
                var bodyB = pc.BodyB;
                b2Vec2 localCenterA = pc.localCenterA;
                float mA = pc.invMassA;
                float iA = pc.invIA;
                b2Vec2 localCenterB = pc.localCenterB;
                float mB = pc.invMassB;
                float iB = pc.invIB;
                int pointCount = pc.pointCount;

                b2Vec2 cA = bodyA.InternalPosition.c;
                float aA = bodyA.InternalPosition.a;

                b2Vec2 cB = bodyB.InternalPosition.c;
                float aB = bodyB.InternalPosition.a;

                b2Transform xfA, xfB;

                // Solve normal constraints
                for (int j = 0; j < pointCount; ++j)
                {
                    xfA.q.s = (float)Math.Sin(aA);
                    xfA.q.c = (float)Math.Cos(aA);
                    xfB.q.s = (float)Math.Sin(aB);
                    xfB.q.c = (float)Math.Cos(aB);

                    float bx = xfA.q.c * localCenterA.x - xfA.q.s * localCenterA.y;
                    float by = xfA.q.s * localCenterA.x + xfA.q.c * localCenterA.y;
                    xfA.p.x = cA.x - bx;
                    xfA.p.y = cA.y - by;

                    bx = xfB.q.c * localCenterB.x - xfB.q.s * localCenterB.y;
                    by = xfB.q.s * localCenterB.x + xfB.q.c * localCenterB.y;
                    xfB.p.x = cB.x - bx;
                    xfB.p.y = cB.y - by;

                    b2PositionSolverManifold psm = new b2PositionSolverManifold(pc, ref xfA, ref xfB, j);
                    
                    float normalx = psm.normal.x;
                    float normaly = psm.normal.y;

                    b2Vec2 point = psm.point;
                    float separation = psm.separation;

                    float rAx = point.x - cA.x;
                    float rAy = point.y - cA.y;

                    float rBx = point.x - cB.x;
                    float rBy = point.y - cB.y;

                    // Track max constraint error.
                    minSeparation = Math.Min(minSeparation, separation);

                    // Prevent large corrections and allow slop.
                    float C = b2Settings.b2_baumgarte * (separation + b2Settings.b2_linearSlop);
                    if (C < -b2Settings.b2_maxLinearCorrection)
                    {
                        C = -b2Settings.b2_maxLinearCorrection;
                    }
                    else if (C > 0)
                    {
                        C = 0;
                    }

                    // Compute the effective mass.
                    float rnA = rAx * normaly - rAy * normalx;
                    float rnB = rBx * normaly - rBy * normalx;
                    float K = mA + mB + iA * rnA * rnA + iB * rnB * rnB;

                    // Compute normal impulse
                    float impulse = K > 0.0f ? -C / K : 0.0f;

                    float Px = impulse * normalx;
                    float Py = impulse * normaly;

                    cA.x -= mA * Px;
                    cA.y -= mA * Py;
                    aA -= iA * (rAx * Py - rAy * Px);

                    cB.x += mB * Px;
                    cB.y += mB * Py;
                    aB += iB * (rBx * Py - rBy * Px);
                }

                bodyA.InternalPosition.c = cA;
                bodyA.InternalPosition.a = aA;

                bodyB.InternalPosition.c = cB;
                bodyB.InternalPosition.a = aB;
            }

            // We can't expect minSpeparation >= -b2_linearSlop because we don't
            // push the separation above -b2_linearSlop.
            return minSeparation >= -3.0f * b2Settings.b2_linearSlop;
        }

        // Sequential position solver for position constraints.
        public bool SolveTOIPositionConstraints(int toiIndexA, int toiIndexB)
        {
            float minSeparation = 0.0f;

            for (int i = 0; i < m_count; ++i)
            {
                var pc = m_constraints[i];

                var bodyA = pc.BodyA;
                var bodyB = pc.BodyB;

                b2Vec2 localCenterA = pc.localCenterA;
                b2Vec2 localCenterB = pc.localCenterB;
                int pointCount = pc.pointCount;

                float mA = 0.0f;
                float iA = 0.0f;
                if (bodyA.IslandIndex == toiIndexA || bodyA.IslandIndex == toiIndexB)
                {
                    mA = pc.invMassA;
                    iA = pc.invIA;
                }

                float mB = pc.invMassB;
                float iB = pc.invIB;
                if (bodyB.IslandIndex == toiIndexA || bodyB.IslandIndex == toiIndexB)
                {
                    mB = pc.invMassB;
                    iB = pc.invIB;
                }

                b2Vec2 cA = bodyA.InternalPosition.c;
                float aA = bodyA.InternalPosition.a;

                b2Vec2 cB = bodyB.InternalPosition.c;
                float aB = bodyB.InternalPosition.a;

                // Solve normal constraints
                for (int j = 0; j < pointCount; ++j)
                {
                    b2Transform xfA, xfB;
                    xfA.q.s = (float)Math.Sin(aA);
                    xfA.q.c = (float)Math.Cos(aA);
                    xfB.q.s = (float)Math.Sin(aB);
                    xfB.q.c = (float)Math.Cos(aB);

                    xfA.p.x = cA.x - (xfA.q.c * localCenterA.x - xfA.q.s * localCenterA.y);
                    xfA.p.y = cA.y - (xfA.q.s * localCenterA.x + xfA.q.c * localCenterA.y);

                    xfB.p.x = cB.x - (xfB.q.c * localCenterB.x - xfB.q.s * localCenterB.y);
                    xfB.p.y = cB.y - (xfB.q.s * localCenterB.x + xfB.q.c * localCenterB.y);

                    b2PositionSolverManifold psm = new b2PositionSolverManifold(pc, ref xfA, ref xfB, j);
                    b2Vec2 normal = psm.normal;

                    b2Vec2 point = psm.point;
                    float separation = psm.separation;

                    float rAx = point.x - cA.x;
                    float rAy = point.y - cA.y;

                    float rBx = point.x - cB.x;
                    float rBy = point.y - cB.y;

                    // Track max constraint error.
                    minSeparation = Math.Min(minSeparation, separation);

                    // Prevent large corrections and allow slop.
                    float C = b2Math.b2Clamp(b2Settings.b2_toiBaugarte * (separation + b2Settings.b2_linearSlop), -b2Settings.b2_maxLinearCorrection, 0.0f);

                    // Compute the effective mass.
                    float rnA = rAx * normal.y - rAy * normal.x;
                    float rnB = rBx * normal.y - rBy * normal.x;
                    
                    float K = mA + mB + iA * rnA * rnA + iB * rnB * rnB;

                    // Compute normal impulse
                    float impulse = K > 0.0f ? -C / K : 0.0f;

                    float Px = impulse * normal.x;
                    float Py = impulse * normal.y;

                    cA.x -= mA * Px;
                    cA.y -= mA * Py;

                    aA -= iA * (rAx * Py - rAy * Px);

                    cB.x += mB * Px;
                    cB.y += mB * Py;

                    aB += iB * (rBx * Py - rBy * Px);
                }

                bodyA.InternalPosition.c = cA;
                bodyA.InternalPosition.a = aA;

                bodyB.InternalPosition.c = cB;
                bodyB.InternalPosition.a = aB;
            }

            // We can't expect minSpeparation >= -b2_linearSlop because we don't
            // push the separation above -b2_linearSlop.
            return minSeparation >= -1.5f * b2Settings.b2_linearSlop;
        }
    }
}