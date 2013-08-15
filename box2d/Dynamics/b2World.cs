using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;
using Box2D.Collision;
using Box2D.Collision.Shapes;
using Box2D.Dynamics.Contacts;
using Box2D.Dynamics.Joints;

namespace Box2D.Dynamics
{
    /// <summary>
    /// Contact impulses for reporting. Impulses are used instead of forces because
    /// sub-step forces may approach infinity for rigid body collisions. These
    /// match up one-to-one with the contact points in b2Manifold.
    /// </summary>
    public struct b2ContactImpulse
    {
        public static b2ContactImpulse Create()
        {
            b2ContactImpulse b = new b2ContactImpulse();
            b.normalImpulses = new float[b2Settings.b2_maxManifoldPoints];
            b.tangentImpulses = new float[b2Settings.b2_maxManifoldPoints];
            b.count = 0;
            return (b);
        }
        public float[] normalImpulses;
        public float[] tangentImpulses;
        public int count;
    }

    // m_flags
    [Flags]
    public enum b2WorldFlags : short
    {
        e_newFixture = 0x1,
        e_locked = 0x2,
        e_clearForces = 0x4
    };

    public class b2World
    {

        public bool IsLocked
        {
#if XBOX
            get { return ((m_flags & b2WorldFlags.e_locked) == b2WorldFlags.e_locked); }
#else
            get { return (m_flags.HasFlag(b2WorldFlags.e_locked)); }
#endif
        }
        private b2WorldFlags m_flags;
        public b2WorldFlags Flags
        {
            get { return (m_flags); }
            set { m_flags = value; }
        }

        private b2ContactManager m_contactManager;
        public b2ContactManager ContactManager
        {
            get { return (m_contactManager); }
            set { m_contactManager = value; }
        }

        private b2Body m_bodyList;
        public b2Body BodyList
        {
            get { return (m_bodyList); }
            set { m_bodyList = value; }
        }
        private b2Joint m_jointList;
        public b2Joint JointList
        {
            get { return (m_jointList); }
            set { m_jointList = value; }
        }

        private int m_bodyCount;
        public int BodyCount
        {
            get { return (m_bodyCount); }
            set { m_bodyCount = value; }
        }

        public int ContactCount {
            get { return m_contactManager.ContactCount; }
        }

        private int m_jointCount;
        public int JointCount
        {
            get { return (m_jointCount); }
            set { m_jointCount = value; }
        }
        private b2Vec2 m_gravity;
        public b2Vec2 Gravity
        {
            get { return (m_gravity); }
            set { m_gravity = value; }
        }
        private bool m_allowSleep;
        public bool AllowSleep
        {
            get { return (m_allowSleep); }
            set { m_allowSleep = value; }
        }

        private b2DestructionListener m_destructionListener;
        private b2Draw m_debugDraw;

        // This is used to compute the time step ratio to
        // support a variable time step.
        private float m_inv_dt0;

        // These are for debugging the solver.
        private bool m_warmStarting;
        private bool m_continuousPhysics;
        private bool m_subStepping;

        private bool m_stepComplete;
#if PROFILING
        private b2Profile m_profile;
        public b2Profile Profile
        {
            get { return (m_profile); }
            set { m_profile = value; }
        }
#endif
        public b2World(b2Vec2 gravity)
        {
            m_destructionListener = null;
            m_debugDraw = null;

            m_bodyList = null;
            m_jointList = null;

            m_bodyCount = 0;
            m_jointCount = 0;

            m_warmStarting = true;
            m_continuousPhysics = true;
            m_subStepping = false;

            m_stepComplete = true;

            m_allowSleep = true;
            m_gravity = gravity;

            m_flags = b2WorldFlags.e_clearForces;

            m_inv_dt0 = 0.0f;

			// setup up our default Contact Manager 
			m_contactManager = new b2ContactManager();
        }

        /// Enable/disable sleep.
        public bool GetAllowSleeping() { return m_allowSleep; }

        /// Enable/disable warm starting. For testing.
        public void SetWarmStarting(bool flag) { m_warmStarting = flag; }
        public bool GetWarmStarting() { return m_warmStarting; }

        /// Enable/disable continuous physics. For testing.
        public void SetContinuousPhysics(bool flag) { m_continuousPhysics = flag; }
        public bool GetContinuousPhysics() { return m_continuousPhysics; }

        /// Enable/disable single stepped continuous physics. For testing.
        public void SetSubStepping(bool flag) { m_subStepping = flag; }
        public bool GetSubStepping() { return m_subStepping; }

        public void SetDestructionListener(b2DestructionListener listener)
        {
            m_destructionListener = listener;
        }

        public void SetContactFilter(b2ContactFilter filter)
        {
            m_contactManager.ContactFilter = filter;
        }

        public void SetContactListener(b2ContactListener listener)
        {
            m_contactManager.ContactListener = listener;
        }

        public void SetDebugDraw(b2Draw debugDraw)
        {
            m_debugDraw = debugDraw;
        }

        public b2Body CreateBody(b2BodyDef def)
        {
            if (IsLocked)
            {
                return null;
            }

            b2Body b = new b2Body(def, this);

            // Add to world doubly linked list.
            b.Prev = null;
            b.Next = m_bodyList;
            if (m_bodyList != null)
            {
                m_bodyList.Prev = b;
            }
            m_bodyList = b;
            ++m_bodyCount;

            return b;
        }

        public void DestroyBody(b2Body b)
        {
            if (IsLocked)
            {
                return;
            }

            // Delete the attached joints.
            b2JointEdge je = b.JointList;
            while (je != null)
            {
                b2JointEdge je0 = je;
                je = je.Next;

                if (m_destructionListener != null)
                {
                    m_destructionListener.SayGoodbye(je0.Joint);
                }

                DestroyJoint(je0.Joint);

                b.JointList = je;
            }
            b.JointList = null;

            // Delete the attached contacts.
            b2ContactEdge ce = b.ContactList;
            while (ce != null)
            {
                b2ContactEdge ce0 = ce;
                ce = ce.Next;
                m_contactManager.Destroy(ce0.Contact);
            }
            b.ContactList = null;

            // Delete the attached fixtures. This destroys broad-phase proxies.
            b2Fixture f = b.FixtureList;
            while (f != null)
            {
                b2Fixture f0 = f;
                f = f.Next;

                if (m_destructionListener != null)
                {
                    m_destructionListener.SayGoodbye(f0);
                }

                f0.DestroyProxies(m_contactManager.BroadPhase);

                b.FixtureList = f;
                b.FixtureCount -= 1;
            }
            b.FixtureList = null;
            b.FixtureCount = 0;

            // Remove world body list.
            if (b.Prev != null)
            {
                b.Prev.Next = b.Next;
            }

            if (b.Next != null)
            {
                b.Next.Prev = b.Prev;
            }

            if (b == m_bodyList)
            {
                m_bodyList = b.Next;
            }

            --m_bodyCount;
        }

        public b2Joint CreateJoint(b2JointDef def)
        {
            if (IsLocked)
            {
                return null;
            }

            b2Joint j = b2Joint.Create(def);

            // Connect to the world list.
            j.Prev = null;
            j.Next = m_jointList;
            if (m_jointList != null)
            {
                m_jointList.Prev = j;
            }
            m_jointList = j;
            ++m_jointCount;

            // Connect to the bodies' doubly linked lists.
            j.m_edgeA.Joint = j;
            j.m_edgeA.Other = j.m_bodyB;
            j.m_edgeA.Prev = null;
            j.m_edgeA.Next = j.m_bodyA.JointList;
            if (j.m_bodyA.JointList != null) 
                j.m_bodyA.JointList.Prev = j.m_edgeA;
            j.m_bodyA.JointList = j.m_edgeA;

            j.m_edgeB.Joint = j;
            j.m_edgeB.Other = j.m_bodyA;
            j.m_edgeB.Prev = null;
            j.m_edgeB.Next = j.m_bodyB.JointList;
            if (j.m_bodyB.JointList != null) 
                j.m_bodyB.JointList.Prev = j.m_edgeB;
            j.m_bodyB.JointList = j.m_edgeB;

            b2Body bodyA = def.BodyA;
            b2Body bodyB = def.BodyB;

            // If the joint prevents collisions, then flag any contacts for filtering.
            if (def.CollideConnected == false)
            {
                b2ContactEdge edge = bodyB.ContactList;
                while (edge != null)
                {
                    if (edge.Other == bodyA)
                    {
                        // Flag the contact for filtering at the next time step (where either
                        // body is awake).
                        edge.Contact.FlagForFiltering();
                    }

                    edge = edge.Next;
                }
            }

            // Note: creating a joint doesn't wake the bodies.

            return j;
        }

        public void DestroyJoint(b2Joint j)
        {
            if (IsLocked)
            {
                return;
            }

            bool collideConnected = j.GetCollideConnected();

            // Remove from the doubly linked list.
            if (j.Prev != null)
            {
                j.Prev.Next = j.Next;
            }

            if (j.Next != null)
            {
                j.Next.Prev = j.Prev;
            }

            if (j == m_jointList)
            {
                m_jointList = j.Next;
            }

            // Disconnect from island graph.
            b2Body bodyA = j.m_bodyA;
            b2Body bodyB = j.m_bodyB;

            // Wake up connected bodies.
            bodyA.SetAwake(true);
            bodyB.SetAwake(true);

            // Remove from body 1.
            if (j.m_edgeA.Prev != null)
            {
                j.m_edgeA.Prev.Next = j.m_edgeA.Next;
            }

            if (j.m_edgeA.Next != null)
            {
                j.m_edgeA.Next.Prev = j.m_edgeA.Prev;
            }

            if (j.m_edgeA == bodyA.JointList)
            {
                bodyA.JointList = j.m_edgeA.Next;
            }

            j.m_edgeA.Prev = null;
            j.m_edgeA.Next = null;

            // Remove from body 2
            if (j.m_edgeB.Prev != null)
            {
                j.m_edgeB.Prev.Next = j.m_edgeB.Next;
            }

            if (j.m_edgeB.Next != null)
            {
                j.m_edgeB.Next.Prev = j.m_edgeB.Prev;
            }

            if (j.m_edgeB == bodyB.JointList)
            {
                bodyB.JointList = j.m_edgeB.Next;
            }

            j.m_edgeB.Prev = null;
            j.m_edgeB.Next = null;

            --m_jointCount;

            // If the joint prevents collisions, then flag any contacts for filtering.
            if (collideConnected == false)
            {
                b2ContactEdge edge = bodyB.ContactList;
                while (edge != null)
                {
                    if (edge.Other == bodyA)
                    {
                        // Flag the contact for filtering at the next time step (where either
                        // body is awake).
                        edge.Contact.FlagForFiltering();
                    }

                    edge = edge.Next;
                }
            }
        }

        //
        public void SetAllowSleeping(bool flag)
        {
            if (flag == m_allowSleep)
            {
                return;
            }

            m_allowSleep = flag;
            if (m_allowSleep == false)
            {
                for (b2Body b = m_bodyList; b != null; b = b.Next)
                {
                    b.SetAwake(true);
                }
            }
        }

        private b2Island m_Island;

        // Find islands, integrate and solve raints, solve position raints
        public void Solve(b2TimeStep step)
        {
#if PROFILING
            m_profile.solveInit = 0.0f;
            m_profile.solveVelocity = 0.0f;
            m_profile.solvePosition = 0.0f;
            m_profile.computeTOI = 0f;
            m_profile.jointCount = 0;
            m_profile.contactCount = 0;
            m_profile.bodyCount = 0;
            m_profile.toiSolverIterations = 0;
            m_profile.timeInInit = 0f;
#endif
            // Size the island for the worst case.
            if (m_Island == null)
            {
                m_Island = new b2Island(m_bodyCount,
                            m_contactManager.ContactCount,
                            m_jointCount,
                            m_contactManager.ContactListener);
            }
            else
            {
                m_Island.Reset(m_bodyCount,
                            m_contactManager.ContactCount,
                            m_jointCount,
                            m_contactManager.ContactListener);
            }

            b2Island island = m_Island;

            // Clear all the island flags.
            for (b2Body b = m_bodyList; b != null; b = b.Next)
            {
#if PROFILING
                m_profile.bodyCount++;
#endif
                b.BodyFlags &= ~b2BodyFlags.e_islandFlag;
            }
            for (b2Contact c = m_contactManager.ContactList; c != null; c = c.Next)
            {
#if PROFILING
                m_profile.contactCount++;
#endif
                c.Flags &= ~b2ContactFlags.e_islandFlag;
            }
            for (b2Joint j = m_jointList; j != null; j = j.Next)
            {
#if PROFILING
                m_profile.jointCount++;
#endif
                j.m_islandFlag = false;
            }

            // Build and simulate all awake islands.
            int stackSize = m_bodyCount;
            b2Body[] stack = new b2Body[stackSize];
            for (b2Body seed = m_bodyList; seed != null; seed = seed.Next)
            {
                if (seed.BodyFlags.HasFlag(b2BodyFlags.e_islandFlag))
                {
                    continue;
                }

                if (seed.IsAwake() == false || seed.IsActive() == false)
                {
                    //System.Diagnostics.Debug.WriteLine("Body is not awake or not active, skipping");
                    continue;
                }

                // The seed can be dynamic or kinematic.
                if (seed.BodyType == b2BodyType.b2_staticBody)
                {
                    //System.Diagnostics.Debug.WriteLine("Body is a static body, skipping");
                    continue;
                }

                // Reset island and stack.
                island.Clear();
                int stackCount = 0;
                stack[stackCount++] = seed;
                seed.BodyFlags |= b2BodyFlags.e_islandFlag;

                // Perform a depth first search (DFS) on the raint graph.
                while (stackCount > 0)
                {
                    // Grab the next body off the stack and add it to the island.
                    b2Body b = stack[--stackCount];
                    island.Add(b);

                    // Make sure the body is awake.
                    b.SetAwake(true);

                    // To keep islands as small as possible, we don't
                    // propagate islands across static bodies.
                    if (b.BodyType == b2BodyType.b2_staticBody)
                    {
                        continue;
                    }

                    // Search all contacts connected to this body.
                    for (b2ContactEdge ce = b.ContactList; ce != null; ce = ce.Next)
                    {
                        b2Contact contact = ce.Contact;

                        // Has this contact already been added to an island?
                        if (contact.Flags.HasFlag(b2ContactFlags.e_islandFlag))
                        {
                            continue;
                        }

                        // Is this contact solid and touching?
                        if (contact.IsEnabled() == false ||
                            contact.IsTouching() == false)
                        {
                            continue;
                        }

                        // Skip sensors.
                        bool sensorA = contact.FixtureA.IsSensor;
                        bool sensorB = contact.FixtureB.IsSensor;
                        if (sensorA || sensorB)
                        {
                            continue;
                        }

                        island.Add(contact);
                        contact.Flags |= b2ContactFlags.e_islandFlag;

                        b2Body other = ce.Other;

                        // Was the other body already added to this island?
                        if ((other.BodyFlags & b2BodyFlags.e_islandFlag) > 0)
                        {
                            continue;
                        }

                        stack[stackCount++] = other;
                        other.BodyFlags |= b2BodyFlags.e_islandFlag;
                    }

                    // Search all joints connect to this body.
                    for (b2JointEdge je = b.JointList; je != null; je = je.Next)
                    {
#if PROFILING
                        m_profile.jointCount++;
#endif
                        if (je.Joint.m_islandFlag == true)
                        {
                            continue;
                        }

                        b2Body other = je.Other;

                        // Don't simulate joints connected to inactive bodies.
                        if (other.IsActive() == false)
                        {
                            continue;
                        }

                        island.Add(je.Joint);
                        je.Joint.m_islandFlag = true;

                        if ((other.BodyFlags & b2BodyFlags.e_islandFlag) > 0)
                        {
                            continue;
                        }

                        stack[stackCount++] = other;
                        other.BodyFlags |= b2BodyFlags.e_islandFlag;
                    }
                }
#if PROFILING
                b2Profile profile = new b2Profile();
                island.Solve(ref profile, step, m_gravity, m_allowSleep);
#else
                island.Solve(step, m_gravity, m_allowSleep);
#endif
#if PROFILING
                m_profile.solveInit += profile.solveInit;
                m_profile.solveVelocity += profile.solveVelocity;
                m_profile.solvePosition += profile.solvePosition;
#endif
                // Post solve cleanup.
                for (int i = 0; i < island.m_bodyCount; ++i)
                {
                    // Allow static bodies to participate in other islands.
                    b2Body b = island.m_bodies[i];
                    if (b.BodyType == b2BodyType.b2_staticBody)
                    {
                        b.BodyFlags &= ~b2BodyFlags.e_islandFlag;
                    }
                }
            }

            b2Timer timer = new b2Timer();
            // Synchronize fixtures, check for out of range bodies.
            for (b2Body b = m_bodyList; b != null; b = b.Next)
            {
                // If a body was not in an island then it did not move.
                if ((!b.BodyFlags.HasFlag(b2BodyFlags.e_islandFlag)))
                {
                    continue;
                }

                if (b.BodyType == b2BodyType.b2_staticBody)
                {
                    continue;
                }

                // Update fixtures (for broad-phase).
                b.SynchronizeFixtures();
            }

            // Look for new contacts.
            m_contactManager.FindNewContacts();
#if PROFILING
            m_profile.broadphase = timer.GetMilliseconds();
#endif
        }

        // Find TOI contacts and solve them.

        // TODO: Make this faster, it's very slow.

        private b2Island m_TOIIsland;

        public void SolveTOI(b2TimeStep step)
        {
            if (m_TOIIsland == null)
            {
                m_TOIIsland = new b2Island(2 * b2Settings.b2_maxTOIContacts, b2Settings.b2_maxTOIContacts, 0, m_contactManager.ContactListener);
            }
            else
            {
                m_TOIIsland.Reset(2 * b2Settings.b2_maxTOIContacts, b2Settings.b2_maxTOIContacts, 0, m_contactManager.ContactListener);
            }
            b2Island island = m_TOIIsland;

            if (m_stepComplete)
            {
                for (b2Body b = m_bodyList; b != null; b = b.Next)
                {
                    b.BodyFlags &= ~b2BodyFlags.e_islandFlag;
                    b.Sweep.alpha0 = 0.0f;
                }

                for (b2Contact c = m_contactManager.ContactList; c != null; c = c.Next)
                {
                    // Invalidate TOI
                    c.Flags &= ~(b2ContactFlags.e_toiFlag | b2ContactFlags.e_islandFlag);
                    c.m_toiCount = 0;
                    c.m_toi = 1.0f;
                }
            }

            // Find TOI events and solve them.
            b2Body[] bodies = new b2Body[2];
            b2Timer computeTimer = new b2Timer();
            for (; ; )
            {
#if PROFILING
                m_profile.toiSolverIterations++;
#endif
                // Find the first TOI.
                b2Contact minContact = null;
                float minAlpha = 1.0f;

                for (b2Contact c = m_contactManager.ContactList; c != null; c = c.Next)
                {
                    // Is this contact disabled?
                    if (c.IsEnabled() == false)
                    {
                        continue;
                    }

                    // Prevent excessive sub-stepping.
                    if (c.m_toiCount > b2Settings.b2_maxSubSteps)
                    {
                        continue;
                    }

                    float alpha = 1.0f;
                    if (c.Flags.HasFlag(b2ContactFlags.e_toiFlag))
                    {
                        // This contact has a valid cached TOI.
                        alpha = c.m_toi;
                    }
                    else
                    {
                        b2Fixture fA = c.GetFixtureA();
                        b2Fixture fB = c.GetFixtureB();

                        // Is there a sensor?
                        if (fA.IsSensor || fB.IsSensor)
                        {
                            continue;
                        }

                        b2Body bA = fA.Body;
                        b2Body bB = fB.Body;

                        b2BodyType typeA = bA.BodyType;
                        b2BodyType typeB = bB.BodyType;

                        bool activeA = bA.IsAwake() && typeA != b2BodyType.b2_staticBody;
                        bool activeB = bB.IsAwake() && typeB != b2BodyType.b2_staticBody;

                        // Is at least one body active (awake and dynamic or kinematic)?
                        if (activeA == false && activeB == false)
                        {
                            continue;
                        }

                        bool collideA = bA.IsBullet() || typeA != b2BodyType.b2_dynamicBody;
                        bool collideB = bB.IsBullet() || typeB != b2BodyType.b2_dynamicBody;

                        // Are these two non-bullet dynamic bodies?
                        if (collideA == false && collideB == false)
                        {
                            continue;
                        }

                        // Compute the TOI for this contact.
                        // Put the sweeps onto the same time interval.
                        float alpha0 = bA.Sweep.alpha0;

                        if (bA.Sweep.alpha0 < bB.Sweep.alpha0)
                        {
                            alpha0 = bB.Sweep.alpha0;
                            bA.Sweep.Advance(alpha0);
                        }
                        else if (bB.Sweep.alpha0 < bA.Sweep.alpha0)
                        {
                            alpha0 = bA.Sweep.alpha0;
                            bB.Sweep.Advance(alpha0);
                        }

                        int indexA = c.GetChildIndexA();
                        int indexB = c.GetChildIndexB();

                        // Compute the time of impact in interval [0, minTOI]
                        b2TOIInput input = b2TOIInput.Zero;
                        input.proxyA.Set(fA.Shape, indexA);
                        input.proxyB.Set(fB.Shape, indexB);
                        input.sweepA = bA.Sweep;
                        input.sweepB = bB.Sweep;
                        input.tMax = 1.0f;

                        computeTimer.Reset();
                        b2TOIOutput output = b2TimeOfImpact.Compute(input);

                        // Console.WriteLine("TOI Output={0}, t={1}", output.state, output.t);

#if PROFILING
                        m_profile.computeTOI += computeTimer.GetMilliseconds();
#endif
                        // Console.WriteLine("b2TimeOfImpact.compute tool {0:F4} ms", m_profile.computeTOI);

                        // Beta is the fraction of the remaining portion of the .
                        float beta = output.t;
                        if (output.state == b2ImpactState.e_touching)
                        {
                            alpha = Math.Min(alpha0 + (1.0f - alpha0) * beta, 1.0f);
                        }
                        else
                        {
                            alpha = 1.0f;
                        }

                        c.m_toi = alpha;
                        c.Flags |= b2ContactFlags.e_toiFlag;
                    }

                    if (alpha < minAlpha)
                    {
                        // This is the minimum TOI found so far.
                        minContact = c;
                        minAlpha = alpha;
                    }
                }

                if (minContact == null || b2Settings.b2_alphaEpsilon < minAlpha)
                {
                    // No more TOI events. Done!
                    m_stepComplete = true;
                    break;
                }
                {
                    b2Timer bt = new b2Timer();
                    // Advance the bodies to the TOI.
                    b2Fixture fA = minContact.GetFixtureA();
                    b2Fixture fB = minContact.GetFixtureB();
                    b2Body bA = fA.Body;
                    b2Body bB = fB.Body;

                    b2Sweep backup1 = bA.Sweep;
                    b2Sweep backup2 = bB.Sweep;

                    bA.Advance(minAlpha);
                    bB.Advance(minAlpha);

                    // The TOI contact likely has some new contact points.
                    minContact.Update(m_contactManager.ContactListener);
                    minContact.Flags &= ~b2ContactFlags.e_toiFlag;
                    ++minContact.m_toiCount;

                    // Is the contact solid?
                    if (minContact.IsEnabled() == false || minContact.IsTouching() == false)
                    {
                        // Restore the sweeps.
                        minContact.SetEnabled(false);
                        bA.Sweep = backup1;
                        bB.Sweep = backup2;
                        bA.SynchronizeTransform();
                        bB.SynchronizeTransform();
                        continue;
                    }

                    bA.SetAwake(true);
                    bB.SetAwake(true);

                    // Build the island
                    island.Clear();
                    island.Add(bA);
                    island.Add(bB);
                    island.Add(minContact);

                    bA.BodyFlags |= b2BodyFlags.e_islandFlag;
                    bB.BodyFlags |= b2BodyFlags.e_islandFlag;
                    minContact.Flags |= b2ContactFlags.e_islandFlag;

                    // Get contacts on bodyA and bodyB.
                    bodies[0] = bA;
                    bodies[1] = bB;
                    for (int i = 0; i < 2; ++i)
                    {
                        b2Body body = bodies[i];
                        if (body.BodyType == b2BodyType.b2_dynamicBody)
                        {
                            for (b2ContactEdge ce = body.ContactList; ce != null; ce = ce.Next)
                            {
                                if (island.m_bodyCount == island.m_bodyCapacity)
                                {
                                    break;
                                }

                                if (island.m_contactCount == island.m_contactCapacity)
                                {
                                    break;
                                }

                                b2Contact contact = ce.Contact;

                                // Has this contact already been added to the island?
                                if (contact.Flags.HasFlag(b2ContactFlags.e_islandFlag))
                                {
                                    continue;
                                }

                                // Only add static, kinematic, or bullet bodies.
                                b2Body other = ce.Other;
                                if (other.BodyType == b2BodyType.b2_dynamicBody &&
                                    body.IsBullet() == false && other.IsBullet() == false)
                                {
                                    continue;
                                }

                                // Skip sensors.
                                bool sensorA = contact.FixtureA.IsSensor;
                                bool sensorB = contact.FixtureB.IsSensor;
                                if (sensorA || sensorB)
                                {
                                    continue;
                                }

                                // Tentatively advance the body to the TOI.
                                b2Sweep backup = other.Sweep;
                                if (other.BodyFlags.HasFlag(b2BodyFlags.e_islandFlag))
                                {
                                    other.Advance(minAlpha);
                                }

                                // Update the contact points
                                contact.Update(m_contactManager.ContactListener);

                                // Was the contact disabled by the user?
                                if (contact.IsEnabled() == false)
                                {
                                    other.Sweep = backup;
                                    other.SynchronizeTransform();
                                    continue;
                                }

                                // Are there contact points?
                                if (contact.IsTouching() == false)
                                {
                                    other.Sweep = backup;
                                    other.SynchronizeTransform();
                                    continue;
                                }

                                // Add the contact to the island
                                contact.Flags |= b2ContactFlags.e_islandFlag;
                                island.Add(contact);

                                // Has the other body already been added to the island?
                                if (other.BodyFlags.HasFlag(b2BodyFlags.e_islandFlag))
                                {
                                    continue;
                                }

                                // Add the other body to the island.
                                other.BodyFlags |= b2BodyFlags.e_islandFlag;

                                if (other.BodyType != b2BodyType.b2_staticBody)
                                {
                                    other.SetAwake(true);
                                }

                                island.Add(other);
                            }
                        }
                    }

                    b2TimeStep subStep;
                    subStep.dt = (1.0f - minAlpha) * step.dt;
                    subStep.inv_dt = 1.0f / subStep.dt;
                    subStep.dtRatio = 1.0f;
                    subStep.positionIterations = 20;
                    subStep.velocityIterations = step.velocityIterations;
                    subStep.warmStarting = false;
                    island.SolveTOI(subStep, bA.IslandIndex, bB.IslandIndex);

                    // Reset island flags and synchronize broad-phase proxies.
                    for (int i = 0; i < island.m_bodyCount; ++i)
                    {
                        b2Body body = island.m_bodies[i];
                        body.BodyFlags &= ~b2BodyFlags.e_islandFlag;

                        if (body.BodyType != b2BodyType.b2_dynamicBody)
                        {
                            continue;
                        }

                        body.SynchronizeFixtures();

                        // Invalidate all contact TOIs on this displaced body.
                        for (b2ContactEdge ce = body.ContactList; ce != null; ce = ce.Next)
                        {
                            ce.Contact.Flags &= ~(b2ContactFlags.e_toiFlag | b2ContactFlags.e_islandFlag);
                        }
                    }

                    // Commit fixture proxy movements to the broad-phase so that new contacts are created.
                    // Also, some contacts can be destroyed.
                    m_contactManager.FindNewContacts();

                    if (m_subStepping)
                    {
                        m_stepComplete = false;
                        break;
                    }
#if PROFILING
                    m_profile.solveTOIAdvance += bt.GetMilliseconds();
#endif
                }
            }
        }

        public void Step(float dt, int velocityIterations, int positionIterations)
        {
            b2Timer stepTimer = new b2Timer();

            // If new fixtures were added, we need to find the new contacts.
            if (m_flags.HasFlag(b2WorldFlags.e_newFixture))
            {
                m_contactManager.FindNewContacts();
                m_flags &= ~b2WorldFlags.e_newFixture;
            }

            m_flags |= b2WorldFlags.e_locked;

            b2TimeStep step = new b2TimeStep();
            step.dt = dt;
            step.velocityIterations = velocityIterations;
            step.positionIterations = positionIterations;
            if (dt > 0.0f)
            {
                step.inv_dt = 1.0f / dt;
            }
            else
            {
                step.inv_dt = 0.0f;
            }

            step.dtRatio = m_inv_dt0 * dt;

            step.warmStarting = m_warmStarting;

            b2Timer timer = new b2Timer();
            // Update contacts. This is where some contacts are destroyed.
            {
                m_contactManager.Collide();
#if PROFILING
                m_profile.collide = timer.GetMilliseconds();
#endif
            }

            // Integrate velocities, solve velocityraints, and integrate positions.
            if (m_stepComplete && step.dt > 0.0f)
            {
                timer.Reset();
                Solve(step);
#if PROFILING
                m_profile.solve = timer.GetMilliseconds();
#endif
            }

            // Handle TOI events.
            if (m_continuousPhysics && step.dt > 0.0f)
            {
                timer.Reset();
                SolveTOI(step);
#if PROFILING
                m_profile.solveTOI = timer.GetMilliseconds();
#endif
            }

            if (step.dt > 0.0f)
            {
                m_inv_dt0 = step.inv_dt;
            }

            if (m_flags.HasFlag(b2WorldFlags.e_clearForces))
            {
                ClearForces();
            }

            m_flags &= ~b2WorldFlags.e_locked;
#if PROFILING
            m_profile.step = stepTimer.GetMilliseconds();
#endif
        }

        public void ClearForces()
        {
            for (b2Body body = m_bodyList; body != null; body = body.Next)
            {
                body.Force.SetZero(); // = b2Vec2.Zero;
                body.Torque = 0.0f;
            }
        }

        public void QueryAABB(b2QueryCallback callback, b2AABB aabb)
        {
            b2WorldQueryWrapper wrapper = new b2WorldQueryWrapper();
            wrapper.BroadPhase = m_contactManager.BroadPhase;
            wrapper.Callback = callback;
            m_contactManager.BroadPhase.Query(wrapper, aabb);
        }

        public void RayCast(b2RayCastCallback callback, b2Vec2 point1, b2Vec2 point2)
        {
            b2WorldRayCastWrapper wrapper = new b2WorldRayCastWrapper();
            wrapper.BroadPhase = m_contactManager.BroadPhase;
            wrapper.Callback = callback;
            b2RayCastInput input;
            input.maxFraction = 1.0f;
            input.p1 = point1;
            input.p2 = point2;
            m_contactManager.BroadPhase.RayCast(wrapper, input);
        }

        public void DrawShape(b2Fixture fixture, b2Transform xf, b2Color color)
        {
            switch (fixture.ShapeType)
            {
                case b2ShapeType.e_circle:
                    {
                        b2CircleShape circle = (b2CircleShape)fixture.Shape;

                        b2Vec2 center = b2Math.b2Mul(xf, circle.Position);
                        float radius = circle.Radius;
                        b2Vec2 axis = b2Math.b2Mul(xf.q, new b2Vec2(1.0f, 0.0f));

                        m_debugDraw.DrawSolidCircle(center, radius, axis, color);
                    }
                    break;

                case b2ShapeType.e_edge:
                    {
                        b2EdgeShape edge = (b2EdgeShape)fixture.Shape;
                        b2Vec2 v1 = b2Math.b2Mul(xf, edge.Vertex1);
                        b2Vec2 v2 = b2Math.b2Mul(xf, edge.Vertex2);
                        m_debugDraw.DrawSegment(v1, v2, color);
                    }
                    break;

                case b2ShapeType.e_chain:
                    {
                        b2ChainShape chain = (b2ChainShape)fixture.Shape;
                        int count = chain.Count;
                        b2Vec2[] vertices = chain.Vertices;

                        b2Vec2 v1 = b2Math.b2Mul(xf, vertices[0]);
                        for (int i = 1; i < count; ++i)
                        {
                            b2Vec2 v2 = b2Math.b2Mul(xf, vertices[i]);
                            m_debugDraw.DrawSegment(v1, v2, color);
                            m_debugDraw.DrawCircle(v1, 0.05f, color);
                            v1 = v2;
                        }
                    }
                    break;

                case b2ShapeType.e_polygon:
                    {
                        b2PolygonShape poly = (b2PolygonShape)fixture.Shape;
                        int vertexCount = poly.VertexCount;
                        b2Vec2[] vertices = new b2Vec2[b2Settings.b2_maxPolygonVertices];

                        for (int i = 0; i < vertexCount; ++i)
                        {
                            vertices[i] = b2Math.b2Mul(xf, poly.Vertices[i]);
                        }

                        m_debugDraw.DrawSolidPolygon(vertices, vertexCount, color);
                    }
                    break;

                default:
                    break;
            }
        }

        public void DrawJoint(b2Joint joint)
        {
            b2Body bodyA = joint.GetBodyA();
            b2Body bodyB = joint.GetBodyB();
            b2Transform xf1 = bodyA.Transform;
            b2Transform xf2 = bodyB.Transform;
            b2Vec2 x1 = xf1.p;
            b2Vec2 x2 = xf2.p;
            b2Vec2 p1 = joint.GetAnchorA();
            b2Vec2 p2 = joint.GetAnchorB();

            b2Color color = new b2Color(0.5f, 0.8f, 0.8f);

            switch (joint.GetJointType())
            {
                case b2JointType.e_distanceJoint:
                    m_debugDraw.DrawSegment(p1, p2, color);
                    break;

                case b2JointType.e_pulleyJoint:
                    {
                        b2PulleyJoint pulley = (b2PulleyJoint)joint;
                        b2Vec2 s1 = pulley.GetGroundAnchorA();
                        b2Vec2 s2 = pulley.GetGroundAnchorB();
                        m_debugDraw.DrawSegment(s1, p1, color);
                        m_debugDraw.DrawSegment(s2, p2, color);
                        m_debugDraw.DrawSegment(s1, s2, color);
                    }
                    break;

                case b2JointType.e_mouseJoint:
                    // don't draw this
                    break;

                default:
                    m_debugDraw.DrawSegment(x1, p1, color);
                    m_debugDraw.DrawSegment(p1, p2, color);
                    m_debugDraw.DrawSegment(x2, p2, color);
                    break;
            }
        }

        public void DrawDebugData()
        {
            if (m_debugDraw == null)
            {
                return;
            }

            b2DrawFlags flags = m_debugDraw.Flags;

            if (flags.HasFlag(b2DrawFlags.e_shapeBit))
            {
                for (b2Body b = m_bodyList; b != null; b = b.Next)
                {
                    b2Transform xf = b.Transform;
                    for (b2Fixture f = b.FixtureList; f != null; f = f.Next)
                    {
                        if (b.IsActive() == false)
                        {
                            DrawShape(f, xf, new b2Color(0.5f, 0.5f, 0.3f));
                        }
                        else if (b.BodyType == b2BodyType.b2_staticBody)
                        {
                            DrawShape(f, xf, new b2Color(0.5f, 0.9f, 0.5f));
                        }
                        else if (b.BodyType == b2BodyType.b2_kinematicBody)
                        {
                            DrawShape(f, xf, new b2Color(0.5f, 0.5f, 0.9f));
                        }
                        else if (b.IsAwake() == false)
                        {
                            DrawShape(f, xf, new b2Color(0.6f, 0.6f, 0.6f));
                        }
                        else
                        {
                            DrawShape(f, xf, new b2Color(0.9f, 0.7f, 0.7f));
                        }
                    }
                }
            }

            if (flags.HasFlag(b2DrawFlags.e_jointBit))
            {
                for (b2Joint j = m_jointList; j != null; j = j.GetNext())
                {
                    DrawJoint(j);
                }
            }

            if (flags.HasFlag(b2DrawFlags.e_pairBit))
            {
                b2Color color = new b2Color(0.3f, 0.9f, 0.9f);
                for (b2Contact c = m_contactManager.ContactList; c != null; c = c.Next)
                {
                    //b2Fixture fixtureA = c.GetFixtureA();
                    //b2Fixture fixtureB = c.GetFixtureB();

                    //b2Vec2 cA = fixtureA.GetAABB().Center;
                    //b2Vec2 cB = fixtureB.GetAABB().Center;

                    //m_debugDraw.DrawSegment(cA, cB, color);
                }
            }

            if (flags.HasFlag(b2DrawFlags.e_aabbBit))
            {
                b2Color color = new b2Color(0.9f, 0.3f, 0.9f);
                b2BroadPhase bp = m_contactManager.BroadPhase;

                for (b2Body b = m_bodyList; b != null; b = b.Next)
                {
                    if (b.IsActive() == false)
                    {
                        continue;
                    }

                    for (b2Fixture f = b.FixtureList; f != null; f = f.Next)
                    {
                        for (int i = 0; i < f.ProxyCount; ++i)
                        {
                            b2FixtureProxy proxy = f.Proxies[i];
                            b2AABB aabb = bp.GetFatAABB(proxy.proxyId);
                            b2Vec2[] vs = new b2Vec2[4];
                            vs[0].Set(aabb.LowerBound.x, aabb.LowerBound.y);
                            vs[1].Set(aabb.UpperBound.x, aabb.LowerBound.y);
                            vs[2].Set(aabb.UpperBound.x, aabb.UpperBound.y);
                            vs[3].Set(aabb.LowerBound.x, aabb.UpperBound.y);

                            m_debugDraw.DrawPolygon(vs, 4, color);
                        }
                    }
                }
            }

            if (flags.HasFlag(b2DrawFlags.e_centerOfMassBit))
            {
                for (b2Body b = m_bodyList; b != null; b = b.Next)
                {
                    b2Transform xf = b.Transform;
                    xf.p = b.WorldCenter;
                    m_debugDraw.DrawTransform(xf);
                }
            }
        }

        public int GetProxyCount()
        {
            return m_contactManager.BroadPhase.GetProxyCount();
        }

        public int GetTreeHeight()
        {
            return m_contactManager.BroadPhase.GetTreeHeight();
        }

        public int GetTreeBalance()
        {
            return m_contactManager.BroadPhase.GetTreeBalance();
        }

        public float GetTreeQuality()
        {
            return m_contactManager.BroadPhase.GetTreeQuality();
        }

        public void Dump()
        {
            if ((m_flags & b2WorldFlags.e_locked) == b2WorldFlags.e_locked)
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine("b2Vec2 g = new b2Vec2({0:N5}, {1:N5});", m_gravity.x, m_gravity.y);
            System.Diagnostics.Debug.WriteLine("m_world.SetGravity(g);");

            System.Diagnostics.Debug.WriteLine("b2Body bodies = new b2Body[{0}];", m_bodyCount);
            System.Diagnostics.Debug.WriteLine("b2Joint joints = new b2Joint[{0}];", m_jointCount);
            int i = 0;
            for (b2Body b = m_bodyList; b != null; b = b.Next, i++)
            {
                b.IslandIndex = i;
                b.Dump();
            }

            i = 0;
            for (b2Joint j = m_jointList; j != null; j = j.Next, i++)
            {
                j.Index = i;
            }

            // First pass on joints, only gear joints.
            for (b2Joint j = m_jointList; j != null; j = j.Next)
            {
                if (j.GetJointType() == b2JointType.e_gearJoint)
                {
                    continue;
                }

                System.Diagnostics.Debug.WriteLine("{");
                j.Dump();
                System.Diagnostics.Debug.WriteLine("}");
            }

            // Second pass on joints, skip gear joints.
            for (b2Joint j = m_jointList; j != null; j = j.Next)
            {
                if (j.GetJointType() != b2JointType.e_gearJoint)
                {
                    continue;
                }

                System.Diagnostics.Debug.WriteLine("{");
                j.Dump();
                System.Diagnostics.Debug.WriteLine("}");
            }

        }
    }
}
