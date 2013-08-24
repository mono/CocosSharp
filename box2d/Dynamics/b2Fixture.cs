using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Collision.Shapes;
using Box2D.Collision;
using Box2D.Dynamics.Contacts;
using Box2D.Common;

namespace Box2D.Dynamics
{
    /// This holds contact filtering data.
    public struct b2Filter
    {
        public b2Filter(ushort cat, ushort mask, int group)
        {
            categoryBits = cat;
            maskBits = mask;
            groupIndex = group;
        }

        /// The collision category bits. Normally you would just set one bit.
        public ushort categoryBits;

        /// The collision mask bits. This states the categories that this
        /// shape would accept for collision.
        public ushort maskBits;

        /// Collision groups allow a certain group of objects to never collide (negative)
        /// or always collide (positive). Zero means no collision group. Non-zero group
        /// filtering always wins against the mask bits.
        public int groupIndex;

        public static b2Filter Default = new b2Filter()
        {
            categoryBits = 0x0001,
            maskBits = 0xFFFF,
            groupIndex = 0
        };
    }

    public class b2Fixture
    {
        public float Density;
        public b2Fixture Next;
        public b2Body Body;
        public b2Shape Shape;

        public b2ShapeType ShapeType
        {
            get { return (Shape.GetShapeType()); }
        }
        public float Friction;
        public float Restitution;

        internal b2FixtureProxy[] m_proxies;
        public b2FixtureProxy[] Proxies
        {
            get
            {
                return (m_proxies);
            }
        }
        private int m_proxyCount;
        public int ProxyCount
        {
            get { return (m_proxyCount); }
        }
        private b2Filter m_filter = b2Filter.Default;
        public b2Filter Filter
        {
            get { return (m_filter); }
            set { m_filter = value; Refilter(); }
        }
        
        internal bool m_isSensor;
        public bool IsSensor
        {
            get { return (m_isSensor); }
            set
            {
                if (value != m_isSensor)
                {
                    Body.SetAwake(true);
                    m_isSensor = value;
                }
            }
        }

        public object UserData;

        public virtual bool TestPoint(b2Vec2 p)
        {
            return Shape.TestPoint(ref Body.Transform, p);
        }

        public virtual bool RayCast(out b2RayCastOutput output, b2RayCastInput input, int childIndex)
        {
            return Shape.RayCast(out output, input, ref Body.Transform, childIndex);
        }

        public virtual b2MassData GetMassData()
        {
            b2MassData data;
            data = Shape.ComputeMass(Density);
            return (data);
        }

        public virtual b2AABB GetAABB(int childIndex)
        {
            return m_proxies[childIndex].aabb;
        }

        public void Create(b2Body body, b2FixtureDef def)
        {
            UserData = def.userData;
            Friction = def.friction;
            Restitution = def.restitution;

            Body = body;
            Next = null;

            m_filter = def.filter;

            m_isSensor = def.isSensor;

            Shape = def.shape.Clone();

            // Reserve proxy space
            int childCount = Shape.GetChildCount();
            m_proxies = b2ArrayPool<b2FixtureProxy>.Create(childCount, true);
            for (int i = 0; i < childCount; ++i)
            {
                m_proxies[i].fixture = null;
                m_proxies[i].proxyId = b2BroadPhase.e_nullProxy;
            }
            m_proxyCount = 0;

            Density = def.density;
        }

        public virtual void Destroy()
        {
            b2ArrayPool<b2FixtureProxy>.Free(m_proxies);
            m_proxies = null;
            Shape = null;
        }

        public virtual void CreateProxies(b2BroadPhase broadPhase, b2Transform xf)
        {
            // Create proxies in the broad-phase.
            m_proxyCount = Shape.GetChildCount();

            for (int i = 0; i < m_proxyCount; ++i)
            {
                b2FixtureProxy proxy = m_proxies[i];
                Shape.ComputeAABB(out proxy.aabb, ref xf, i);
                proxy.fixture = this;
                proxy.childIndex = i;
                proxy.proxyId = broadPhase.CreateProxy(ref proxy.aabb, ref proxy);
                m_proxies[i] = proxy;
            }
        }
        public virtual void DestroyProxies(b2BroadPhase broadPhase)
        {
            // Destroy proxies in the broad-phase.
            for (int i = 0; i < m_proxyCount; ++i)
            {
                broadPhase.DestroyProxy(m_proxies[i].proxyId);
                m_proxies[i].proxyId = b2BroadPhase.e_nullProxy;
            }
            m_proxyCount = 0;
        }
        public virtual void Synchronize(b2BroadPhase broadPhase, ref b2Transform transform1, ref b2Transform transform2)
        {
            if (m_proxyCount == 0)
            {
                return;
            }

            for (int i = 0, count = m_proxyCount; i < count; ++i)
            {
                b2FixtureProxy proxy = m_proxies[i];

                // Compute an AABB that covers the swept shape (may miss some rotation effect).
                b2AABB aabb1, aabb2;
                Shape.ComputeAABB(out aabb1, ref transform1, proxy.childIndex);
                Shape.ComputeAABB(out aabb2, ref transform2, proxy.childIndex);

                proxy.aabb.Combine(ref aabb1, ref aabb2);

                b2Vec2 displacement;
                displacement.x = transform2.p.x - transform1.p.x;
                displacement.y = transform2.p.y - transform1.p.y;

                broadPhase.MoveProxy(proxy.proxyId, ref proxy.aabb, ref displacement);
            }
        }
        public virtual void SetFilterData(b2Filter filter)
        {
            m_filter = filter;

            Refilter();
        }
        public virtual void Refilter()
        {
            if (Body == null)
            {
                return;
            }

            // Flag associated contacts for filtering.
            b2ContactEdge edge = Body.ContactList;
            while (edge != null)
            {
                b2Contact contact = edge.Contact;
                b2Fixture fixtureA = contact.GetFixtureA();
                b2Fixture fixtureB = contact.GetFixtureB();
                if (fixtureA == this || fixtureB == this)
                {
                    contact.FlagForFiltering();
                }

                edge = edge.Next;
            }

            b2World world = Body.World;

            if (world == null)
            {
                return;
            }

            // Touch each proxy so that new pairs may be created
            b2BroadPhase broadPhase = world.ContactManager.BroadPhase;
            for (int i = 0; i < m_proxyCount; ++i)
            {
                broadPhase.TouchProxy(m_proxies[i].proxyId);
            }
        }
        public virtual void Dump(int bodyIndex)
        {
            System.Diagnostics.Debug.WriteLine("    b2FixtureDef fd;");
            System.Diagnostics.Debug.WriteLine("    fd.friction = {0:N5};", Friction);
            System.Diagnostics.Debug.WriteLine("    fd.restitution = {0:N5};", Restitution);
            System.Diagnostics.Debug.WriteLine("    fd.density = {0:N5};", Density);
            System.Diagnostics.Debug.WriteLine("    fd.isSensor = {0};", m_isSensor);
            System.Diagnostics.Debug.WriteLine("    fd.filter.categoryBits = {0};", m_filter.categoryBits);
            System.Diagnostics.Debug.WriteLine("    fd.filter.maskBits = {0};", m_filter.maskBits);
            System.Diagnostics.Debug.WriteLine("    fd.filter.groupIndex = {0};", m_filter.groupIndex);

            switch (Shape.ShapeType)
            {
                case b2ShapeType.e_circle:
                    {
                        b2CircleShape s = (b2CircleShape)Shape;
                        System.Diagnostics.Debug.WriteLine("    b2CircleShape shape;");
                        System.Diagnostics.Debug.WriteLine("    shape.m_radius = {0:N5};", s.Radius);
                        System.Diagnostics.Debug.WriteLine("    shape.m_p.Set({0:N5}, {0:N5});", s.Position.x, s.Position.y);
                    }
                    break;

                case b2ShapeType.e_edge:
                    {
                        b2EdgeShape s = (b2EdgeShape)Shape;
                        System.Diagnostics.Debug.WriteLine("    b2EdgeShape shape;");
                        System.Diagnostics.Debug.WriteLine("    shape.m_radius = {0:N5};", s.Radius);
                        System.Diagnostics.Debug.WriteLine("    shape.m_vertex0.Set({0:N5}, {0:N5});", s.Vertex0.x, s.Vertex0.y);
                        System.Diagnostics.Debug.WriteLine("    shape.m_vertex1.Set({0:N5}, {0:N5});", s.Vertex1.x, s.Vertex1.y);
                        System.Diagnostics.Debug.WriteLine("    shape.m_vertex2.Set({0:N5}, {0:N5});", s.Vertex2.x, s.Vertex2.y);
                        System.Diagnostics.Debug.WriteLine("    shape.m_vertex3.Set({0:N5}, {0:N5});", s.Vertex3.x, s.Vertex3.y);
                        System.Diagnostics.Debug.WriteLine("    shape.m_hasVertex0 = {0};", s.HasVertex0);
                        System.Diagnostics.Debug.WriteLine("    shape.m_hasVertex3 = {0};", s.HasVertex3);
                    }
                    break;

                case b2ShapeType.e_polygon:
                    {
                        b2PolygonShape s = (b2PolygonShape)Shape;
                        System.Diagnostics.Debug.WriteLine("    b2PolygonShape shape;");
                        System.Diagnostics.Debug.WriteLine("    b2Vec2 vs[{0}];", b2Settings.b2_maxPolygonVertices);
                        for (int i = 0; i < s.VertexCount; ++i)
                        {
                            System.Diagnostics.Debug.WriteLine("    vs[{0}].Set({0:N5}, {0:N5});", i, s.Vertices[i].x, s.Vertices[i].y);
                        }
                        System.Diagnostics.Debug.WriteLine("    shape.Set(vs, {0});", s.VertexCount);
                    }
                    break;

                case b2ShapeType.e_chain:
                    {
                        b2ChainShape s = (b2ChainShape)Shape;
                        System.Diagnostics.Debug.WriteLine("    b2ChainShape shape;");
                        System.Diagnostics.Debug.WriteLine("    b2Vec2 vs[{0}];", s.Count);
                        for (int i = 0; i < s.Count; ++i)
                        {
                            System.Diagnostics.Debug.WriteLine("    vs[{0}].Set({0:N5}, {0:N5});", i, s.Vertices[i].x, s.Vertices[i].y);
                        }
                        System.Diagnostics.Debug.WriteLine("    shape.CreateChain(vs, {0});", s.Count);
                        System.Diagnostics.Debug.WriteLine("    shape.PrevVertex.Set({0:N5}, {0:N5});", s.PrevVertex.x, s.PrevVertex.y);
                        System.Diagnostics.Debug.WriteLine("    shape.NextVertex.Set({0:N5}, {0:N5});", s.NextVertex.x, s.NextVertex.y);
                        System.Diagnostics.Debug.WriteLine("    shape.m_hasPrevVertex = {0};", s.HasPrevVertex);
                        System.Diagnostics.Debug.WriteLine("    shape.m_hasNextVertex = {0};", s.HasNextVertex);
                    }
                    break;

                default:
                    return;
            }

            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.WriteLine("    fd.shape = shape;");
            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.WriteLine("    bodies[{0}].CreateFixture(fd);", bodyIndex);
        }
    }
}
