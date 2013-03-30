using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision;
using Box2D.Dynamics.Contacts;

namespace Box2D.Dynamics
{
    public class b2ContactManager
    {
        private b2BroadPhase m_broadPhase;
        private b2Contact m_contactList;
        private int m_contactCount;
        private b2ContactFilter m_contactFilter;
        private b2ContactListener m_contactListener;

        public b2ContactManager()
        {
            m_contactList = null;
            m_contactCount = 0;
            m_contactFilter = b2ContactFilter.b2_defaultFilter;
            m_contactListener = b2ContactListener.b2_defaultListener;
        }

        public int ContactCount
        {
            get { return (m_contactCount); }
            set { m_contactCount = value; }
        }

        public b2BroadPhase BroadPhase { 
            get { return (m_broadPhase); } 
            set { m_broadPhase = value; } 
        }

        public b2ContactListener ContactListener
        {
            set { m_contactListener = value; }
            get { return (m_contactListener); }
        }

        public b2ContactFilter ContactFilter
        {
            set { m_contactFilter = value; }
            get { return (m_contactFilter); }
        }

        public void Destroy(b2Contact c)
        {
            b2Fixture fixtureA = c.GetFixtureA();
            b2Fixture fixtureB = c.GetFixtureB();
            b2Body bodyA = fixtureA.GetBody();
            b2Body bodyB = fixtureB.GetBody();

            if (m_contactListener && c.IsTouching())
            {
                m_contactListener.EndContact(c);
            }

            // Remove from the world.
            if (c.m_prev)
            {
                c.m_prev.m_next = c.m_next;
            }

            if (c.m_next)
            {
                c.m_next.m_prev = c.m_prev;
            }

            if (c == m_contactList)
            {
                m_contactList = c.m_next;
            }

            // Remove from body 1
            if (c.m_nodeA.prev)
            {
                c.m_nodeA.prev.next = c.m_nodeA.next;
            }

            if (c.m_nodeA.next)
            {
                c.m_nodeA.next.prev = c.m_nodeA.prev;
            }

            if (c.m_nodeA == bodyA.m_contactList)
            {
                bodyA.m_contactList = c.m_nodeA.next;
            }

            // Remove from body 2
            if (c.m_nodeB.prev)
            {
                c.m_nodeB.prev.next = c.m_nodeB.next;
            }

            if (c.m_nodeB.next)
            {
                c.m_nodeB.next.prev = c.m_nodeB.prev;
            }

            if (c.m_nodeB == bodyB.m_contactList)
            {
                bodyB.m_contactList = c.m_nodeB.next;
            }

            // Call the factory.
            b2Contact.Destroy(c);
            --m_contactCount;
        }

        // This is the top level collision call for the time step. Here
        // all the narrow phase collision is processed for the world
        // contact list.
        public void Collide()
        {
            // Update awake contacts.
            b2Contact c = m_contactList;
            while (c)
            {
                b2Fixture fixtureA = c.GetFixtureA();
                b2Fixture fixtureB = c.GetFixtureB();
                int indexA = c.GetChildIndexA();
                int indexB = c.GetChildIndexB();
                b2Body bodyA = fixtureA.GetBody();
                b2Body bodyB = fixtureB.GetBody();

                // Is this contact flagged for filtering?
                if (c.m_flags & b2Contact.e_filterFlag)
                {
                    // Should these bodies collide?
                    if (bodyB.ShouldCollide(bodyA) == false)
                    {
                        b2Contact cNuke = c;
                        c = cNuke.GetNext();
                        Destroy(cNuke);
                        continue;
                    }

                    // Check user filtering.
                    if (m_contactFilter && m_contactFilter.ShouldCollide(fixtureA, fixtureB) == false)
                    {
                        b2Contact cNuke = c;
                        c = cNuke.GetNext();
                        Destroy(cNuke);
                        continue;
                    }

                    // Clear the filtering flag.
                    c.m_flags &= ~b2Contact.e_filterFlag;
                }

                bool activeA = bodyA.IsAwake() && bodyA.m_type != b2BodyType.b2_staticBody;
                bool activeB = bodyB.IsAwake() && bodyB.m_type != b2BodyType.b2_staticBody;

                // At least one body must be awake and it must be dynamic or kinematic.
                if (activeA == false && activeB == false)
                {
                    c = c.GetNext();
                    continue;
                }

                int proxyIdA = fixtureA.m_proxies[indexA].proxyId;
                int proxyIdB = fixtureB.m_proxies[indexB].proxyId;
                bool overlap = m_broadPhase.TestOverlap(proxyIdA, proxyIdB);

                // Here we destroy contacts that cease to overlap in the broad-phase.
                if (overlap == false)
                {
                    b2Contact cNuke = c;
                    c = cNuke.GetNext();
                    Destroy(cNuke);
                    continue;
                }

                // The contact persists.
                c.Update(m_contactListener);
                c = c.GetNext();
            }
        }

        public void FindNewContacts()
        {
            m_broadPhase.UpdatePairs(this);
        }

        public void AddPair(object proxyUserDataA, object proxyUserDataB)
        {
            b2FixtureProxy proxyA = (b2FixtureProxy)proxyUserDataA;
            b2FixtureProxy proxyB = (b2FixtureProxy)proxyUserDataB;

            b2Fixture fixtureA = proxyA.fixture;
            b2Fixture fixtureB = proxyB.fixture;

            int indexA = proxyA.childIndex;
            int indexB = proxyB.childIndex;

            b2Body bodyA = fixtureA.GetBody();
            b2Body bodyB = fixtureB.GetBody();

            // Are the fixtures on the same body?
            if (bodyA == bodyB)
            {
                return;
            }

            // TODO_ERIN use a hash table to remove a potential bottleneck when both
            // bodies have a lot of contacts.
            // Does a contact already exist?
            b2ContactEdge edge = bodyB.GetContactList();
            while (edge)
            {
                if (edge.other == bodyA)
                {
                    b2Fixture fA = edge.contact.GetFixtureA();
                    b2Fixture fB = edge.contact.GetFixtureB();
                    int iA = edge.contact.GetChildIndexA();
                    int iB = edge.contact.GetChildIndexB();

                    if (fA == fixtureA && fB == fixtureB && iA == indexA && iB == indexB)
                    {
                        // A contact already exists.
                        return;
                    }

                    if (fA == fixtureB && fB == fixtureA && iA == indexB && iB == indexA)
                    {
                        // A contact already exists.
                        return;
                    }
                }

                edge = edge.next;
            }

            // Does a joint override collision? Is at least one body dynamic?
            if (bodyB.ShouldCollide(bodyA) == false)
            {
                return;
            }

            // Check user filtering.
            if (m_contactFilter && m_contactFilter.ShouldCollide(fixtureA, fixtureB) == false)
            {
                return;
            }

            // Call the factory.
            b2Contact c = b2Contact.Create(fixtureA, indexA, fixtureB, indexB, m_allocator);
            if (c == null)
            {
                return;
            }

            // Contact creation may swap fixtures.
            fixtureA = c.GetFixtureA();
            fixtureB = c.GetFixtureB();
            indexA = c.GetChildIndexA();
            indexB = c.GetChildIndexB();
            bodyA = fixtureA.GetBody();
            bodyB = fixtureB.GetBody();

            // Insert into the world.
            c.m_prev = null;
            c.m_next = m_contactList;
            if (m_contactList != null)
            {
                m_contactList.m_prev = c;
            }
            m_contactList = c;

            // Connect to island graph.

            // Connect to body A
            c.m_nodeA.contact = c;
            c.m_nodeA.other = bodyB;

            c.m_nodeA.prev = null;
            c.m_nodeA.next = bodyA.m_contactList;
            if (bodyA.m_contactList != null)
            {
                bodyA.m_contactList.prev = &c.m_nodeA;
            }
            bodyA.m_contactList = &c.m_nodeA;

            // Connect to body B
            c.m_nodeB.contact = c;
            c.m_nodeB.other = bodyA;

            c.m_nodeB.prev = null;
            c.m_nodeB.next = bodyB.m_contactList;
            if (bodyB.m_contactList != null)
            {
                bodyB.m_contactList.prev = &c.m_nodeB;
            }
            bodyB.m_contactList = &c.m_nodeB;

            // Wake up the bodies
            bodyA.SetAwake(true);
            bodyB.SetAwake(true);

            ++m_contactCount;
        }
    }
}
