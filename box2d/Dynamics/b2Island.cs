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


/*
Position Correction Notes
=========================
I tried the several algorithms for position correction of the 2D revolute joint.
I looked at these systems:
- simple pendulum (1m diameter sphere on massless 5m stick) with initial angular velocity of 100 rad/s.
- suspension bridge with 30 1m long planks of length 1m.
- multi-link chain with 30 1m long links.

Here are the algorithms:

Baumgarte - A fraction of the position error is added to the velocity error. There is no
separate position solver.

Pseudo Velocities - After the velocity solver and position integration,
the position error, Jacobian, and effective mass are recomputed. Then
the velocity constraints are solved with pseudo velocities and a fraction
of the position error is added to the pseudo velocity error. The pseudo
velocities are initialized to zero and there is no warm-starting. After
the position solver, the pseudo velocities are added to the positions.
This is also called the First Order World method or the Position LCP method.

Modified Nonlinear Gauss-Seidel (NGS) - Like Pseudo Velocities except the
position error is re-computed for each constraint and the positions are updated
after the constraint is solved. The radius vectors (aka Jacobians) are
re-computed too (otherwise the algorithm has horrible instability). The pseudo
velocity states are not needed because they are effectively zero at the beginning
of each iteration. Since we have the current position error, we allow the
iterations to terminate early if the error becomes smaller than b2_linearSlop.

Full NGS or just NGS - Like Modified NGS except the effective mass are re-computed
each time a constraint is solved.

Here are the results:
Baumgarte - this is the cheapest algorithm but it has some stability problems,
especially with the bridge. The chain links separate easily close to the root
and they jitter as they struggle to pull together. This is one of the most common
methods in the field. The big drawback is that the position correction artificially
affects the momentum, thus leading to instabilities and false bounce. I used a
bias factor of 0.2. A larger bias factor makes the bridge less stable, a smaller
factor makes joints and contacts more spongy.

Pseudo Velocities - the is more stable than the Baumgarte method. The bridge is
stable. However, joints still separate with large angular velocities. Drag the
simple pendulum in a circle quickly and the joint will separate. The chain separates
easily and does not recover. I used a bias factor of 0.2. A larger value lead to
the bridge collapsing when a heavy cube drops on it.

Modified NGS - this algorithm is better in some ways than Baumgarte and Pseudo
Velocities, but in other ways it is worse. The bridge and chain are much more
stable, but the simple pendulum goes unstable at high angular velocities.

Full NGS - stable in all tests. The joints display good stiffness. The bridge
still sags, but this is better than infinite forces.

Recommendations
Pseudo Velocities are not really worthwhile because the bridge and chain cannot
recover from joint separation. In other cases the benefit over Baumgarte is small.

Modified NGS is not a robust method for the revolute joint due to the violent
instability seen in the simple pendulum. Perhaps it is viable with other constraint
types, especially scalar constraints where the effective mass is a scalar.

This leaves Baumgarte and Full NGS. Baumgarte has small, but manageable instabilities
and is very fast. I don't think we can escape Baumgarte, especially in highly
demanding cases where high constraint fidelity is not needed.

Full NGS is robust and easy on the eyes. I recommend this as an option for
higher fidelity simulation and certainly for suspension bridges and long chains.
Full NGS might be a good choice for ragdolls, especially motorized ragdolls where
joint separation can be problematic. The number of NGS iterations can be reduced
for better performance without harming robustness much.

Each joint in a can be handled differently in the position solver. So I recommend
a system where the user can select the algorithm on a per joint basis. I would
probably default to the slower Full NGS and let the user select the faster
Baumgarte method in performance critical scenarios.
*/

/*
Cache Performance

The Box2D solvers are dominated by cache misses. Data structures are designed
to increase the number of cache hits. Much of misses are due to random access
to body data. The constraint structures are iterated over linearly, which leads
to few cache misses.

The bodies are not accessed during iteration. Instead read only data, such as
the mass values are stored with the constraints. The mutable data are the constraint
impulses and the bodies velocities/positions. The impulses are held inside the
constraint structures. The body velocities/positions are held in compact, temporary
arrays to increase the number of cache hits. Linear and angular velocity are
stored in a single array since multiple arrays lead to multiple misses.
*/

/*
2D Rotation

R = [cos(theta) -sin(theta)]
    [sin(theta) cos(theta) ]

thetaDot = omega

Let q1 = cos(theta), q2 = sin(theta).
R = [q1 -q2]
    [q2  q1]

q1Dot = -thetaDot * q2
q2Dot = thetaDot * q1

q1_new = q1_old - dt * w * q2
q2_new = q2_old + dt * w * q1
then normalize.

This might be faster than computing sin+cos.
However, we can compute sin+cos of the same angle fast.
*/
using System;
using System.Diagnostics;
using Box2D.Common;
using Box2D.Collision;
using Box2D.Dynamics.Contacts;
using Box2D.Collision.Shapes;
using Box2D.Dynamics.Joints;

namespace Box2D.Dynamics
{
    public class b2Island
    {

        public b2ContactListener m_listener;

        public b2Body[] m_bodies;
        public b2Contact[] m_contacts;
        public b2Joint[] m_joints;

        public b2Position[] m_positions;
        public b2Velocity[] m_velocities;

        public int m_bodyCount;
        public int m_jointCount;
        public int m_contactCount;

        public int m_bodyCapacity;
        public int m_contactCapacity;
        public int m_jointCapacity;

        public void Clear()
        {
            m_bodyCount = 0;
            m_contactCount = 0;
            m_jointCount = 0;
        }

        public void Add(b2Body body)
        {
            Debug.Assert(m_bodyCount < m_bodyCapacity);
            body.IslandIndex = m_bodyCount;
            m_bodies[m_bodyCount] = body;
            ++m_bodyCount;
        }

        public void Add(b2Contact contact)
        {
            Debug.Assert(m_contactCount < m_contactCapacity);
            m_contacts[m_contactCount++] = contact;
        }

        public void Add(b2Joint joint)
        {
            Debug.Assert(m_jointCount < m_jointCapacity);
            m_joints[m_jointCount++] = joint;
        }


        public b2Island(
            int bodyCapacity,
            int contactCapacity,
            int jointCapacity,
            b2ContactListener listener)
        {
            m_bodyCapacity = bodyCapacity;
            m_contactCapacity = contactCapacity;
            m_jointCapacity = jointCapacity;
            m_bodyCount = 0;
            m_contactCount = 0;
            m_jointCount = 0;

            m_listener = listener;

            m_bodies = new b2Body[bodyCapacity];
            m_contacts = new b2Contact[contactCapacity];
            m_joints = new b2Joint[jointCapacity];

            m_velocities = new b2Velocity[m_bodyCapacity];
            m_positions = new b2Position[m_bodyCapacity];
        }


        public void Solve(ref b2Profile profile, b2TimeStep step, b2Vec2 gravity, bool allowSleep)
        {
            b2Timer timer = new b2Timer();

            float h = step.dt;

            // Integrate velocities and apply damping. Initialize the body state.
            for (int i = 0; i < m_bodyCount; ++i)
            {
                b2Body b = m_bodies[i];

                b2Vec2 c = b.Sweep.c;
                float a = b.Sweep.a;
                b2Vec2 v = b.LinearVelocity;
                float w = b.AngularVelocity;

                // Store positions for continuous collision.
                b.Sweep.c0 = b.Sweep.c;
                b.Sweep.a0 = b.Sweep.a;

                if (b.BodyType == b2BodyType.b2_dynamicBody)
                {
                    // Integrate velocities.
                    v += h * (b.GravityScale * gravity + b.InvertedMass * b.Force);
                    w += h * b.InvertedI * b.Torque;

                    // Apply damping.
                    // ODE: dv/dt + c * v = 0
                    // Solution: v(t) = v0 * exp(-c * t)
                    // Time step: v(t + dt) = v0 * exp(-c * (t + dt)) = v0 * exp(-c * t) * exp(-c * dt) = v * exp(-c * dt)
                    // v2 = exp(-c * dt) * v1
                    // Taylor expansion:
                    // v2 = (1.0f - c * dt) * v1
                    v *= b2Math.b2Clamp(1.0f - h * b.LinearDamping, 0.0f, 1.0f);
                    w *= b2Math.b2Clamp(1.0f - h * b.AngularDamping, 0.0f, 1.0f);
                }

                m_positions[i].c = c;
                m_positions[i].a = a;
                m_velocities[i].v = v;
                m_velocities[i].w = w;
            }

            timer.Reset();

            // Solver data
            b2SolverData solverData = new b2SolverData();
            solverData.step = step;
            solverData.positions = m_positions;
            solverData.velocities = m_velocities;

            // Initialize velocity constraints.
            b2ContactSolverDef contactSolverDef = new b2ContactSolverDef();
            contactSolverDef.step = step;
            contactSolverDef.contacts = m_contacts;
            contactSolverDef.count = m_contactCount;
            contactSolverDef.positions = m_positions;
            contactSolverDef.velocities = m_velocities;

            b2ContactSolver contactSolver = new b2ContactSolver(contactSolverDef);
            contactSolver.InitializeVelocityConstraints();

            if (step.warmStarting)
            {
                contactSolver.WarmStart();
            }

            for (int i = 0; i < m_jointCount; ++i)
            {
                m_joints[i].InitVelocityConstraints(solverData);
            }

            profile.solveInit = timer.GetMilliseconds();

            // Solve velocity constraints
            timer.Reset();
            for (int i = 0; i < step.velocityIterations; ++i)
            {
                for (int j = 0; j < m_jointCount; ++j)
                {
                    m_joints[j].SolveVelocityConstraints(solverData);
                }

                contactSolver.SolveVelocityConstraints();
            }

            // Store impulses for warm starting
            contactSolver.StoreImpulses();
            profile.solveVelocity = timer.GetMilliseconds();

            // Integrate positions
            for (int i = 0; i < m_bodyCount; ++i)
            {
                b2Vec2 c = m_positions[i].c;
                float a = m_positions[i].a;
                b2Vec2 v = m_velocities[i].v;
                float w = m_velocities[i].w;

                // Check for large velocities
                b2Vec2 translation = h * v;
                if (b2Math.b2Dot(translation, translation) > b2Settings.b2_maxTranslationSquared)
                {
                    float ratio = b2Settings.b2_maxTranslation / translation.Length();
                    v *= ratio;
                }

                float rotation = h * w;
                if (rotation * rotation > b2Settings.b2_maxRotationSquared)
                {
                    float ratio = b2Settings.b2_maxRotation / Math.Abs(rotation);
                    w *= ratio;
                }

                // Integrate
                c += h * v;
                a += h * w;

                m_positions[i].c = c;
                m_positions[i].a = a;
                m_velocities[i].v = v;
                m_velocities[i].w = w;
            }

            // Solve position constraints
            timer.Reset();
            bool positionSolved = false;
            for (int i = 0; i < step.positionIterations; ++i)
            {
                bool contactsOkay = contactSolver.SolvePositionConstraints();

                bool jointsOkay = true;
                for (int i2 = 0; i2 < m_jointCount; ++i2)
                {
                    bool jointOkay = m_joints[i2].SolvePositionConstraints(solverData);
                    jointsOkay = jointsOkay && jointOkay;
                }

                if (contactsOkay && jointsOkay)
                {
                    // Exit early if the position errors are small.
                    positionSolved = true;
                    break;
                }
            }

            // Copy state buffers back to the bodies
            for (int i = 0; i < m_bodyCount; ++i)
            {
                b2Body body = m_bodies[i];
                body.Sweep.c = m_positions[i].c;
                body.Sweep.a = m_positions[i].a;
                body.LinearVelocity = m_velocities[i].v;
                body.AngularVelocity = m_velocities[i].w;
                body.SynchronizeTransform();
            }

            profile.solvePosition = timer.GetMilliseconds();

            Report(contactSolver.m_velocityConstraints);

            if (allowSleep)
            {
                float minSleepTime = b2Settings.b2_maxFloat;

                float linTolSqr = b2Settings.b2_linearSleepTolerance * b2Settings.b2_linearSleepTolerance;
                float angTolSqr = b2Settings.b2_angularSleepTolerance * b2Settings.b2_angularSleepTolerance;

                for (int i = 0; i < m_bodyCount; ++i)
                {
                    b2Body b = m_bodies[i];
                    if (b.BodyType == b2BodyType.b2_staticBody)
                    {
                        continue;
                    }

                    if (!(b.BodyFlags.HasFlag(b2BodyFlags.e_autoSleepFlag)) ||
                        b.AngularVelocity * b.AngularVelocity > angTolSqr ||
                        b2Math.b2Dot(b.LinearVelocity, b.LinearVelocity) > linTolSqr)
                    {
                        b.SleepTime = 0.0f;
                        minSleepTime = 0.0f;
                    }
                    else
                    {
                        b.SleepTime += h;
                        minSleepTime = Math.Min(minSleepTime, b.SleepTime);
                    }
                }

                if (minSleepTime >= b2Settings.b2_timeToSleep && positionSolved)
                {
                    for (int i = 0; i < m_bodyCount; ++i)
                    {
                        b2Body b = m_bodies[i];
                        b.SetAwake(false);
                    }
                }
            }
        }

        public void SolveTOI(b2TimeStep subStep, int toiIndexA, int toiIndexB)
        {
            Debug.Assert(toiIndexA < m_bodyCount);
            Debug.Assert(toiIndexB < m_bodyCount);

            // Initialize the body state.
            for (int i = 0; i < m_bodyCount; ++i)
            {
                b2Body b = m_bodies[i];
                m_positions[i].c = b.Sweep.c;
                m_positions[i].a = b.Sweep.a;
                m_velocities[i].v = b.LinearVelocity;
                m_velocities[i].w = b.AngularVelocity;
            }

            b2ContactSolverDef contactSolverDef;
            contactSolverDef.contacts = m_contacts;
            contactSolverDef.count = m_contactCount;
            contactSolverDef.step = subStep;
            contactSolverDef.positions = m_positions;
            contactSolverDef.velocities = m_velocities;
            b2ContactSolver contactSolver = new b2ContactSolver(contactSolverDef);

            // Solve position constraints.
            for (int i = 0; i < subStep.positionIterations; ++i)
            {
                bool contactsOkay = contactSolver.SolveTOIPositionConstraints(toiIndexA, toiIndexB);
                if (contactsOkay)
                {
                    break;
                }
            }

#if false
    // Is the new position really safe?
    for (int i = 0; i < m_contactCount; ++i)
    {
        b2Contact c = m_contacts[i];
        b2Fixture fA = c.GetFixtureA();
        b2Fixture fB = c.GetFixtureB();

        b2Body bA = fA.Body;
        b2Body bB = fB.Body;

        int indexA = c.GetChildIndexA();
        int indexB = c.GetChildIndexB();

        b2DistanceInput input = new b2DistanceInput();
        input.proxyA.Set(fA.Shape, indexA);
        input.proxyB.Set(fB.Shape, indexB);
        input.transformA = bA.Transform;
        input.transformB = bB.Transform;
        input.useRadii = false;

        b2DistanceOutput output;
        b2SimplexCache cache = new b2SimplexCache();
        cache.count = 0;
        output = b2Distance(cache, input);

        if (output.distance == 0 || cache.count == 3)
        {
            cache.count += 0;
        }
    }
#endif

            // Leap of faith to new safe state.
            m_bodies[toiIndexA].Sweep.c0 = m_positions[toiIndexA].c;
            m_bodies[toiIndexA].Sweep.a0 = m_positions[toiIndexA].a;
            m_bodies[toiIndexB].Sweep.c0 = m_positions[toiIndexB].c;
            m_bodies[toiIndexB].Sweep.a0 = m_positions[toiIndexB].a;

            // No warm starting is needed for TOI events because warm
            // starting impulses were applied in the discrete solver.
            contactSolver.InitializeVelocityConstraints();

            // Solve velocity constraints.
            for (int i = 0; i < subStep.velocityIterations; ++i)
            {
                contactSolver.SolveVelocityConstraints();
            }

            // Don't store the TOI contact forces for warm starting
            // because they can be quite large.

            float h = subStep.dt;

            // Integrate positions
            for (int i = 0; i < m_bodyCount; ++i)
            {
                b2Vec2 c = m_positions[i].c;
                float a = m_positions[i].a;
                b2Vec2 v = m_velocities[i].v;
                float w = m_velocities[i].w;

                // Check for large velocities
                b2Vec2 translation = h * v;
                if (b2Math.b2Dot(translation, translation) > b2Settings.b2_maxTranslationSquared)
                {
                    float ratio = b2Settings.b2_maxTranslation / translation.Length();
                    v *= ratio;
                }

                float rotation = h * w;
                if (rotation * rotation > b2Settings.b2_maxRotationSquared)
                {
                    float ratio = b2Settings.b2_maxRotation / Math.Abs(rotation);
                    w *= ratio;
                }

                // Integrate
                c += h * v;
                a += h * w;

                m_positions[i].c = c;
                m_positions[i].a = a;
                m_velocities[i].v = v;
                m_velocities[i].w = w;

                // Sync bodies
                b2Body body = m_bodies[i];
                body.Sweep.c = c;
                body.Sweep.a = a;
                body.LinearVelocity = v;
                body.AngularVelocity = w;
                body.SynchronizeTransform();
            }

            Report(contactSolver.m_velocityConstraints);
        }

        public void Report(b2ContactVelocityConstraint[] constraints)
        {
            if (m_listener == null)
            {
                return;
            }

            b2ContactImpulse impulse = new b2ContactImpulse();
            for (int i = 0; i < m_contactCount; ++i)
            {
                b2Contact c = m_contacts[i];

                b2ContactVelocityConstraint vc = constraints[i];

                impulse.count = vc.pointCount;
                for (int j = 0; j < vc.pointCount; ++j)
                {
                    impulse.normalImpulses[j] = vc.points[j].normalImpulse;
                    impulse.tangentImpulses[j] = vc.points[j].tangentImpulse;
                }

                m_listener.PostSolve(c, impulse);
            }
        }
    }
}