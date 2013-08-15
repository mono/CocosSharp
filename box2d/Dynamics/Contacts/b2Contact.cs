/*
* Copyright (c) 2006-2009 Erin Catto http://www.box2d.org
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
using Box2D.Collision.Shapes;
using Box2D.Collision;

namespace Box2D.Dynamics.Contacts
{

    public struct b2ContactRegister
    {
        public Type contactType;
        public bool isPrimary;
    }

    /// <summary>
    /// A contact edge is used to connect bodies and contacts together
    /// in a contact graph where each body is a node and each contact
    /// is an edge. A contact edge belongs to a doubly linked list
    /// maintained in each attached body. Each contact has two contact
    /// nodes, one for each attached body.
    /// </summary>
    public class b2ContactEdge
    {
        public b2Body Other;            //< provides quick access to the other body attached.
        public b2Contact Contact;        //< the contact
        public bool hasPrev;
        public b2ContactEdge Prev;    //< the previous contact edge in the body's contact list
        public bool hasNext;
        public b2ContactEdge Next;    //< the next contact edge in the body's contact list
    }

    [Flags]
    public enum b2ContactFlags : uint
    {
        // Flags stored in m_flags
        // Used when crawling contact graph when forming islands.
        e_islandFlag = 0x0001,

        // Set when the shapes are touching.
        e_touchingFlag = 0x0002,

        // This contact can be disabled (by user)
        e_enabledFlag = 0x0004,

        // This contact needs filtering because a fixture filter was changed.
        e_filterFlag = 0x0008,

        // This bullet contact had a TOI event
        e_bulletHitFlag = 0x0010,

        // This contact has a valid TOI in m_toi
        e_toiFlag = 0x0020
    }
    public abstract class b2Contact
    {

        protected b2ContactFlags m_flags;

        public b2ContactFlags Flags
        {
            get { return (m_flags); }
            set { m_flags = value; }
        }

        // World pool and list pointers.
        protected b2Contact m_Prev;
        public b2Contact Prev { 
            get { return (m_Prev); }
            set { m_Prev = value; }
        }
        protected b2Contact m_Next;
        public b2Contact Next {
            get { return (m_Next); }
            set { m_Next = value; }
        }

        // Nodes for connecting bodies.
        protected b2ContactEdge m_NodeA;
        public b2ContactEdge NodeA {
            get { return (m_NodeA); }
            set { m_NodeA = value; }
        }
        protected b2ContactEdge m_nodeB;
        public b2ContactEdge NodeB { 
            get { return (m_nodeB); }
            set { m_nodeB = value; }
        }

        protected b2Fixture m_fixtureA;
        public b2Fixture FixtureA { get { return (m_fixtureA); }
            set { m_fixtureA = value; }
        }
        protected b2Fixture m_fixtureB;
        public b2Fixture FixtureB { get { return (m_fixtureB); }
            set { m_fixtureB = value; }
        }

        protected int m_indexA;
        protected int m_indexB;

        protected b2Manifold m_manifold;

        public int m_toiCount;
        public float m_toi;

        protected float m_friction;
        public float Friction { get { return (m_friction); } set { m_friction = value; } }
        protected float m_restitution;
        public float Restitution { get { return (m_restitution); } set { m_restitution = value; } }

        /// Evaluate this contact with your own manifold and transforms.
        public abstract void Evaluate(ref b2Manifold manifold, ref b2Transform xfA, ref b2Transform xfB);

        protected static b2ContactRegister[,] s_registers = new b2ContactRegister[(int)b2ShapeType.e_typeCount, (int)b2ShapeType.e_typeCount];

        static b2Contact()
        {
            AddType(typeof(b2CircleContact), b2ShapeType.e_circle, b2ShapeType.e_circle);
            AddType(typeof(b2PolygonAndCircleContact), b2ShapeType.e_polygon, b2ShapeType.e_circle);
            AddType(typeof(b2PolygonContact), b2ShapeType.e_polygon, b2ShapeType.e_polygon);
            AddType(typeof(b2EdgeAndCircleContact), b2ShapeType.e_edge, b2ShapeType.e_circle);
            AddType(typeof(b2EdgeAndPolygonContact), b2ShapeType.e_edge, b2ShapeType.e_polygon);
            AddType(typeof(b2ChainAndCircleContact), b2ShapeType.e_chain, b2ShapeType.e_circle);
            AddType(typeof(b2ChainAndPolygonContact), b2ShapeType.e_chain, b2ShapeType.e_polygon);
        }

        private static void AddType(Type createType,
                                b2ShapeType type1, b2ShapeType type2)
        {
            Debug.Assert(0 <= type1 && type1 < b2ShapeType.e_typeCount);
            Debug.Assert(0 <= type2 && type2 < b2ShapeType.e_typeCount);

            s_registers[(int)type1, (int)type2].contactType = createType;
            s_registers[(int)type1, (int)type2].isPrimary = true;

            if (type1 != type2)
            {
                s_registers[(int)type2, (int)type1].contactType = createType;
                s_registers[(int)type2, (int)type1].isPrimary = false;
            }
        }

        public static b2Contact Create(b2Fixture fixtureA, int indexA, b2Fixture fixtureB, int indexB)
        {

            b2ShapeType type1 = fixtureA.ShapeType;
            b2ShapeType type2 = fixtureB.ShapeType;

            Debug.Assert(0 <= type1 && type1 < b2ShapeType.e_typeCount);
            Debug.Assert(0 <= type2 && type2 < b2ShapeType.e_typeCount);

            Type createFcn = s_registers[(int)type1, (int)type2].contactType;


            if (createFcn != null)
            {
                if (s_registers[(int)type1, (int)type2].isPrimary)
                {
                    return ((b2Contact)Activator.CreateInstance(createFcn, new object[] { fixtureA, indexA, fixtureB, indexB }));
                }
                else
                {
                    return ((b2Contact)Activator.CreateInstance(createFcn, new object[] { fixtureB, indexB, fixtureA, indexA }));
                }
            }
            return (null);
        }

        public b2Contact(b2Fixture fA, int indexA, b2Fixture fB, int indexB)
        {
            m_flags = b2ContactFlags.e_enabledFlag;

            m_fixtureA = fA;
            m_fixtureB = fB;

            m_indexA = indexA;
            m_indexB = indexB;

			m_manifold = b2Manifold.Create();
            m_manifold.pointCount = 0;

            Prev = null;
            Next = null;

			m_NodeA = new b2ContactEdge();
			m_NodeA.Contact = null;
			m_NodeA.hasPrev = false;
			m_NodeA.hasNext = false;
			m_NodeA.Other = null;

			m_nodeB = new b2ContactEdge();
            m_nodeB.Contact = null;
            m_nodeB.hasPrev = false;
            m_nodeB.hasNext = false;
            m_nodeB.Other = null;

            m_toiCount = 0;

            m_friction = b2Math.b2MixFriction(m_fixtureA.Friction, m_fixtureB.Friction);
            m_restitution = b2Math.b2MixRestitution(m_fixtureA.Restitution, m_fixtureB.Restitution);
        }

        // Update the contact manifold and touching status.
        // Note: do not assume the fixture AABBs are overlapping or are valid.
        public virtual void Update(b2ContactListener listener)
        {
            b2Manifold oldManifold = m_manifold;
            oldManifold.points = m_manifold.CopyPoints();


            // Re-enable this contact.
            m_flags |= b2ContactFlags.e_enabledFlag;

            bool touching = false;
            bool wasTouching = (m_flags & b2ContactFlags.e_touchingFlag) == b2ContactFlags.e_touchingFlag;

            bool sensorA = m_fixtureA.IsSensor;
            bool sensorB = m_fixtureB.IsSensor;
            bool sensor = sensorA || sensorB;

            b2Body bodyA = m_fixtureA.Body;
            b2Body bodyB = m_fixtureB.Body;
            b2Transform xfA = bodyA.Transform;
            b2Transform xfB = bodyB.Transform;

            // Is this contact a sensor?
            if (sensor)
            {
                b2Shape shapeA = m_fixtureA.Shape;
                b2Shape shapeB = m_fixtureB.Shape;
                touching = b2Collision.b2TestOverlap(shapeA, m_indexA, shapeB, m_indexB, ref xfA, ref xfB);

                // Sensors don't generate manifolds.
                m_manifold.pointCount = 0;
            }
            else
            {
                Evaluate(ref m_manifold, ref xfA, ref xfB);
                touching = m_manifold.pointCount > 0;

                // Match old contact ids to new contact ids and copy the
                // stored impulses to warm start the solver.
                for (int i = 0; i < m_manifold.pointCount; ++i)
                {
                    b2ManifoldPoint mp2 = m_manifold.points[i];
                    mp2.normalImpulse = 0.0f;
                    mp2.tangentImpulse = 0.0f;
                    b2ContactFeature id2 = mp2.id;

                    for (int j = 0; j < oldManifold.pointCount; ++j)
                    {
                        b2ManifoldPoint mp1 = oldManifold.points[j];

                        if (mp1.id.key == id2.key)
                        {
                            mp2.normalImpulse = mp1.normalImpulse;
                            mp2.tangentImpulse = mp1.tangentImpulse;
                            break;
                        }
                    }
                    m_manifold.points[i] = mp2;
                }

                if (touching != wasTouching)
                {
                    bodyA.SetAwake(true);
                    bodyB.SetAwake(true);
                }
            }

            if (touching)
            {
                m_flags |= b2ContactFlags.e_touchingFlag;
            }
            else
            {
                m_flags &= ~b2ContactFlags.e_touchingFlag;
            }

            if (wasTouching == false && touching == true && listener != null)
            {
                listener.BeginContact(this);
            }

            if (wasTouching == true && touching == false && listener != null)
            {
                listener.EndContact(this);
            }

            if (sensor == false && touching && listener != null)
            {
                listener.PreSolve(this, ref oldManifold);
            }
        }
        public virtual b2Manifold GetManifold()
        {
            return m_manifold;
        }
        public virtual void SetManifold(b2Manifold m)
        {
            m_manifold = m;
        }

        public virtual void GetWorldManifold(ref b2WorldManifold worldManifold)
        {
            b2Body bodyA = m_fixtureA.Body;
            b2Body bodyB = m_fixtureB.Body;
            b2Shape shapeA = m_fixtureA.Shape;
            b2Shape shapeB = m_fixtureB.Shape;

            worldManifold.Initialize(ref m_manifold, bodyA.Transform, shapeA.Radius, bodyB.Transform, shapeB.Radius);
        }

        public virtual void SetEnabled(bool flag)
        {
            if (flag)
            {
                m_flags |= b2ContactFlags.e_enabledFlag;
            }
            else
            {
                m_flags &= ~b2ContactFlags.e_enabledFlag;
            }
        }

        public virtual bool IsEnabled()
        {
            return (m_flags.HasFlag(b2ContactFlags.e_enabledFlag));
        }

        public virtual bool IsTouching()
        {
            return (m_flags.HasFlag(b2ContactFlags.e_touchingFlag));
        }

        public virtual b2Contact GetNext()
        {
            return Next;
        }

        public virtual b2Contact GetPrev()
        {
            return Prev;
        }

        public virtual b2Fixture GetFixtureA()
        {
            return m_fixtureA;
        }

        public virtual b2Fixture GetFixtureB()
        {
            return m_fixtureB;
        }

        public virtual int GetChildIndexA()
        {
            return m_indexA;
        }

        public virtual int GetChildIndexB()
        {
            return m_indexB;
        }

        public virtual void FlagForFiltering()
        {
            m_flags |= b2ContactFlags.e_filterFlag;
        }

        public virtual void SetFriction(float friction)
        {
            m_friction = friction;
        }

        public virtual float GetFriction()
        {
            return m_friction;
        }

        public virtual void ResetFriction()
        {
            m_friction = b2MixFriction(m_fixtureA.Friction, m_fixtureB.Friction);
        }

        public virtual void SetRestitution(float restitution)
        {
            m_restitution = restitution;
        }

        public virtual float GetRestitution()
        {
            return m_restitution;
        }

        public virtual void ResetRestitution()
        {
            m_restitution = b2MixRestitution(m_fixtureA.Restitution, m_fixtureB.Restitution);
        }

        protected virtual float b2MixFriction(float friction1, float friction2)
        {
            return (float)Math.Sqrt(friction1 * friction2);
        }

        /// Restitution mixing law. The idea is allow for anything to bounce off an inelastic surface.
        /// For example, a superball bounces on anything.
        protected float b2MixRestitution(float restitution1, float restitution2)
        {
            return restitution1 > restitution2 ? restitution1 : restitution2;
        }
    }
}