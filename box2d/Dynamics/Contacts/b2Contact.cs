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

    public enum ContactType
    {
        b2CircleContact,
        b2PolygonAndCircleContact,
        b2PolygonContact,
        b2EdgeAndCircleContact,
        b2EdgeAndPolygonContact,
        b2ChainAndCircleContact,
        b2ChainAndPolygonContact
    }

    public struct b2ContactRegister
    {
        public ContactType contactType;
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
    public class b2Contact : b2ReusedObject<b2Contact>
    {
        public b2ContactFlags Flags;

        // World pool and list pointers.
        public b2Contact Prev;
        public b2Contact Next;

        // Nodes for connecting bodies.
        public b2ContactEdge NodeA;
        public b2ContactEdge NodeB;

        public b2Fixture FixtureA;
        public b2Fixture FixtureB;

        internal int m_indexA;
        internal int m_indexB;

        internal b2Manifold m_manifold;

        public int m_toiCount;
        public float m_toi;

        public float Friction;
        public float Restitution;

        protected static b2ContactRegister[,] s_registers = new b2ContactRegister[(int)b2ShapeType.e_typeCount, (int)b2ShapeType.e_typeCount];

        private ContactType _type;

        public b2Contact()
        {
            m_manifold = new b2Manifold();
            NodeA = new b2ContactEdge();
            NodeB = new b2ContactEdge();
        }

        static b2Contact()
        {
            AddType(ContactType.b2CircleContact, b2ShapeType.e_circle, b2ShapeType.e_circle);
            AddType(ContactType.b2PolygonAndCircleContact, b2ShapeType.e_polygon, b2ShapeType.e_circle);
            AddType(ContactType.b2PolygonContact, b2ShapeType.e_polygon, b2ShapeType.e_polygon);
            AddType(ContactType.b2EdgeAndCircleContact, b2ShapeType.e_edge, b2ShapeType.e_circle);
            AddType(ContactType.b2EdgeAndPolygonContact, b2ShapeType.e_edge, b2ShapeType.e_polygon);
            AddType(ContactType.b2ChainAndCircleContact, b2ShapeType.e_chain, b2ShapeType.e_circle);
            AddType(ContactType.b2ChainAndPolygonContact, b2ShapeType.e_chain, b2ShapeType.e_polygon);
        }

        private static void AddType(ContactType createType,
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

        /// Evaluate this contact with your own manifold and transforms.
        public void Evaluate(b2Manifold manifold, ref b2Transform xfA, ref b2Transform xfB)
        {
            switch (_type)
            {
                case ContactType.b2CircleContact:
                    b2Collision.b2CollideCircles(manifold, (b2CircleShape) FixtureA.Shape, ref xfA,
                        (b2CircleShape) FixtureB.Shape, ref xfB);
                    break;

                case ContactType.b2PolygonAndCircleContact:
                    b2Collision.b2CollidePolygonAndCircle(manifold, (b2PolygonShape) FixtureA.Shape, ref xfA,
                        (b2CircleShape) FixtureB.Shape, ref xfB);
                    break;

                case ContactType.b2PolygonContact:
                    b2Collision.b2CollidePolygons(manifold, (b2PolygonShape) FixtureA.Shape, ref xfA,
                        (b2PolygonShape) FixtureB.Shape, ref xfB);
                    break;

                case ContactType.b2EdgeAndCircleContact:
                    b2Collision.b2CollideEdgeAndCircle(manifold, (b2EdgeShape) FixtureA.Shape, ref xfA,
                        (b2CircleShape) FixtureB.Shape, ref xfB);
                    break;

                case ContactType.b2EdgeAndPolygonContact:
                    b2Collision.b2CollideEdgeAndPolygon(manifold, (b2EdgeShape) FixtureA.Shape, ref xfA,
                        (b2PolygonShape) FixtureB.Shape, ref xfB);
                    break;

                case ContactType.b2ChainAndCircleContact:
                    b2ChainShape chain = (b2ChainShape) FixtureA.Shape;
                    b2EdgeShape edge;
                    edge = chain.GetChildEdge(m_indexA);
                    b2Collision.b2CollideEdgeAndCircle(manifold, edge, ref xfA, (b2CircleShape) FixtureB.Shape,
                        ref xfB);
                    break;

                case ContactType.b2ChainAndPolygonContact:
                    chain = (b2ChainShape) FixtureA.Shape;
                    edge = chain.GetChildEdge(m_indexA);
                    b2Collision.b2CollideEdgeAndPolygon(manifold, edge, ref xfA, (b2PolygonShape) FixtureB.Shape,
                        ref xfB);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public static b2Contact Create(b2Fixture fixtureA, int indexA, b2Fixture fixtureB, int indexB)
        {

            b2ShapeType type1 = fixtureA.ShapeType;
            b2ShapeType type2 = fixtureB.ShapeType;

            Debug.Assert(0 <= type1 && type1 < b2ShapeType.e_typeCount);
            Debug.Assert(0 <= type2 && type2 < b2ShapeType.e_typeCount);

            var contact = Create();

            //Type createFcn = s_registers[(int)type1, (int)type2].contactType;

            contact._type = s_registers[(int) type1, (int) type2].contactType;

            //if (createFcn != null)
            {
                if (s_registers[(int)type1, (int)type2].isPrimary)
                {
                    //return ((b2Contact)Activator.CreateInstance(createFcn, new object[] { fixtureA, indexA, fixtureB, indexB }));
                    contact.Init(fixtureA, indexA, fixtureB, indexB);
                }
                else
                {
                    //return ((b2Contact)Activator.CreateInstance(createFcn, new object[] { fixtureB, indexB, fixtureA, indexA }));
                    contact.Init(fixtureB, indexB, fixtureA, indexA);
                }
            }
            return contact;
        }

        public b2Contact(b2Fixture fA, int indexA, b2Fixture fB, int indexB)
        {
            Init(fA, indexA, fB, indexB);
        }

        protected void Init(b2Fixture fA, int indexA, b2Fixture fB, int indexB)
        {
            Flags = b2ContactFlags.e_enabledFlag;

            FixtureA = fA;
            FixtureB = fB;

            m_indexA = indexA;
            m_indexB = indexB;

            m_manifold.pointCount = 0;

            Prev = null;
            Next = null;

            NodeA.Contact = null;
            NodeA.hasPrev = false;
            NodeA.hasNext = false;
            NodeA.Other = null;

            NodeB.Contact = null;
            NodeB.hasPrev = false;
            NodeB.hasNext = false;
            NodeB.Other = null;

            m_toiCount = 0;

            Friction = b2Math.b2MixFriction(FixtureA.Friction, FixtureB.Friction);
            Restitution = b2Math.b2MixRestitution(FixtureA.Restitution, FixtureB.Restitution);
        }

        //Memory save
        private static b2Manifold oldManifold = new b2Manifold();

        // Update the contact manifold and touching status.
        // Note: do not assume the fixture AABBs are overlapping or are valid.
        public virtual void Update(b2ContactListener listener)
        {
            oldManifold.CopyFrom(m_manifold);

            // Re-enable this contact.
            Flags |= b2ContactFlags.e_enabledFlag;

            bool touching = false;
            bool wasTouching = (Flags & b2ContactFlags.e_touchingFlag) == b2ContactFlags.e_touchingFlag;

            bool sensor = FixtureA.m_isSensor || FixtureB.m_isSensor;

            b2Body bodyA = FixtureA.Body;
            b2Body bodyB = FixtureB.Body;
            b2Transform xfA = bodyA.Transform;
            b2Transform xfB = bodyB.Transform;

            // Is this contact a sensor?
            if (sensor)
            {
                b2Shape shapeA = FixtureA.Shape;
                b2Shape shapeB = FixtureB.Shape;
                touching = b2Collision.b2TestOverlap(shapeA, m_indexA, shapeB, m_indexB, ref xfA, ref xfB);

                // Sensors don't generate manifolds.
                m_manifold.pointCount = 0;
            }
            else
            {
                Evaluate(m_manifold, ref xfA, ref xfB);
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
                }

                if (touching != wasTouching)
                {
                    bodyA.SetAwake(true);
                    bodyB.SetAwake(true);
                }
            }

            if (touching)
            {
                Flags |= b2ContactFlags.e_touchingFlag;
            }
            else
            {
                Flags &= ~b2ContactFlags.e_touchingFlag;
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
                listener.PreSolve(this, oldManifold);
            }
        }
        public virtual b2Manifold GetManifold()
        {
            return m_manifold;
        }
        /*
        public virtual void SetManifold(ref b2Manifold m)
        {
            m_manifold = m;
            m_manifold.CopyPointsFrom(ref m);
        }
        */
        public virtual void GetWorldManifold(ref b2WorldManifold worldManifold)
        {
            b2Body bodyA = FixtureA.Body;
            b2Body bodyB = FixtureB.Body;
            b2Shape shapeA = FixtureA.Shape;
            b2Shape shapeB = FixtureB.Shape;

            worldManifold.Initialize(m_manifold, ref bodyA.Transform, shapeA.Radius, ref bodyB.Transform, shapeB.Radius);
        }

        public virtual void SetEnabled(bool flag)
        {
            if (flag)
            {
                Flags |= b2ContactFlags.e_enabledFlag;
            }
            else
            {
                Flags &= ~b2ContactFlags.e_enabledFlag;
            }
        }

        public virtual bool IsEnabled()
        {
            return (Flags & b2ContactFlags.e_enabledFlag) != 0;
        }

        public virtual bool IsTouching()
        {
            return (Flags & b2ContactFlags.e_touchingFlag) != 0;
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
            return FixtureA;
        }

        public virtual b2Fixture GetFixtureB()
        {
            return FixtureB;
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
            Flags |= b2ContactFlags.e_filterFlag;
        }

        public virtual void SetFriction(float friction)
        {
            Friction = friction;
        }

        public virtual float GetFriction()
        {
            return Friction;
        }

        public virtual void ResetFriction()
        {
            Friction = b2MixFriction(FixtureA.Friction, FixtureB.Friction);
        }

        public virtual void SetRestitution(float restitution)
        {
            Restitution = restitution;
        }

        public virtual float GetRestitution()
        {
            return Restitution;
        }

        public virtual void ResetRestitution()
        {
            Restitution = b2MixRestitution(FixtureA.Restitution, FixtureB.Restitution);
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