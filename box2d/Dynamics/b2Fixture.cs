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
        private float m_density;
        public float Density
        {
            get { return (m_density); }
            set { m_density = value; }
        }
        private b2Fixture m_Next;
        public b2Fixture Next
        {
            get { return (m_Next); }
            set { m_Next = value; }
        }
        private b2Body m_body;
        public b2Body Body
        {
            get { return (m_body); }
            set { m_body = value; }
        }
        private b2Shape m_shape;
        public b2Shape Shape
        {
            get { return (m_shape); }
            set { m_shape = value; }
        }
        public b2ShapeType ShapeType
        {
            get { return (m_shape.GetShapeType()); }
        }
        private float m_friction;
        public float Friction
        {
            get { return (m_friction); }
            set { m_friction = value; }
        }
        private float m_restitution;
        public float Restitution
        {
            get { return (m_restitution); }
            set { m_restitution = value; }
        }

        private List<b2FixtureProxy> m_proxies = new List<b2FixtureProxy>();
        public IList<b2FixtureProxy> Proxies
        {
            get
            {
                return (m_proxies);
            }
        }
        private int m_proxyCount;
        public int ProxyCount
        {
            get { return (m_proxies.Count); }
        }
        private b2Filter m_filter = b2Filter.Default;
        public b2Filter Filter
        {
            get { return (m_filter); }
            set { m_filter = value; Refilter(); }
        }
        private bool m_isSensor;
        public bool IsSensor
        {
            get { return (m_isSensor); }
            set
            {
                if (value != m_isSensor)
                {
                    m_body.SetAwake(true);
                    m_isSensor = value;
                }
            }
        }

        private object m_userData;
        public object UserData
        {
            get { return (m_userData); }
            set { m_userData = value; }
        }
        public virtual bool TestPoint(b2Vec2 p)
        {
            return m_shape.TestPoint(m_body.Transform, p);
        }

        public virtual bool RayCast(out b2RayCastOutput output, b2RayCastInput input, int childIndex)
        {
            return m_shape.RayCast(out output, input, m_body.Transform, childIndex);
        }

        public virtual b2MassData GetMassData()
        {
            b2MassData data;
            data = m_shape.ComputeMass(m_density);
            return (data);
        }

        public virtual b2AABB GetAABB(int childIndex)
        {
            return m_proxies[childIndex].aabb;
        }

        public void Create(b2Body body, b2FixtureDef def)
        {
            m_userData = def.userData;
            m_friction = def.friction;
            m_restitution = def.restitution;

            m_body = body;
            Next = null;

            m_filter = def.filter;

            m_isSensor = def.isSensor;

            m_shape = def.shape.Clone();

            // Reserve proxy space
            int childCount = m_shape.GetChildCount();
            for (int i = 0; i < childCount; ++i)
            {
                b2FixtureProxy proxy = new b2FixtureProxy();
                proxy.fixture = null;
                proxy.proxyId = b2BroadPhase.e_nullProxy;
                m_proxies.Add(proxy);
            }
            m_proxyCount = 0;

            m_density = def.density;
        }

        public virtual void Destroy()
        {
            m_proxies = null;
            m_shape = null;
        }

        public virtual void CreateProxies(b2BroadPhase broadPhase, b2Transform xf)
        {
            // Create proxies in the broad-phase.
            m_proxyCount = m_shape.GetChildCount();

            for (int i = 0; i < m_proxyCount; ++i)
            {
                b2FixtureProxy proxy = m_proxies[i];
                proxy.aabb = m_shape.ComputeAABB(xf, i);
                proxy.fixture = this;
                proxy.childIndex = i;
                proxy.proxyId = broadPhase.CreateProxy(proxy.aabb, ref proxy);
                m_proxies[i] = proxy;
            }
        }
        public virtual void DestroyProxies(b2BroadPhase broadPhase)
        {
            // Destroy proxies in the broad-phase.
            for (int i = 0; i < m_proxyCount; ++i)
            {
                b2FixtureProxy proxy = m_proxies[i];
                broadPhase.DestroyProxy(proxy.proxyId);
                proxy.proxyId = b2BroadPhase.e_nullProxy;
                m_proxies[i] = proxy;
            }
            m_proxies.Clear();
            m_proxyCount = 0;
        }
        public virtual void Synchronize(b2BroadPhase broadPhase, b2Transform transform1, b2Transform transform2)
        {
            if (m_proxyCount == 0)
            {
                return;
            }

            for (int i = 0; i < m_proxyCount; ++i)
            {
                b2FixtureProxy proxy = m_proxies[i];

                // Compute an AABB that covers the swept shape (may miss some rotation effect).
                b2AABB aabb1, aabb2;
                aabb1 = m_shape.ComputeAABB(transform1, proxy.childIndex);
                aabb2 = m_shape.ComputeAABB(transform2, proxy.childIndex);

                proxy.aabb.Combine(ref aabb1, ref aabb2);

                b2Vec2 displacement = transform2.p - transform1.p;

                broadPhase.MoveProxy(proxy.proxyId, proxy.aabb, displacement);
            }
        }
        public virtual void SetFilterData(b2Filter filter)
        {
            m_filter = filter;

            Refilter();
        }
        public virtual void Refilter()
        {
            if (m_body == null)
            {
                return;
            }

            // Flag associated contacts for filtering.
            b2ContactEdge edge = m_body.ContactList;
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

            b2World world = m_body.World;

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
            System.Diagnostics.Debug.WriteLine("    fd.friction = {0:N5};", m_friction);
            System.Diagnostics.Debug.WriteLine("    fd.restitution = {0:N5};", m_restitution);
            System.Diagnostics.Debug.WriteLine("    fd.density = {0:N5};", m_density);
            System.Diagnostics.Debug.WriteLine("    fd.isSensor = {0};", m_isSensor);
            System.Diagnostics.Debug.WriteLine("    fd.filter.categoryBits = {0};", m_filter.categoryBits);
            System.Diagnostics.Debug.WriteLine("    fd.filter.maskBits = {0};", m_filter.maskBits);
            System.Diagnostics.Debug.WriteLine("    fd.filter.groupIndex = {0};", m_filter.groupIndex);

            switch (m_shape.ShapeType)
            {
                case b2ShapeType.e_circle:
                    {
                        b2CircleShape s = (b2CircleShape)m_shape;
                        System.Diagnostics.Debug.WriteLine("    b2CircleShape shape;");
                        System.Diagnostics.Debug.WriteLine("    shape.m_radius = {0:N5};", s.Radius);
                        System.Diagnostics.Debug.WriteLine("    shape.m_p.Set({0:N5}, {0:N5});", s.Position.x, s.Position.y);
                    }
                    break;

                case b2ShapeType.e_edge:
                    {
                        b2EdgeShape s = (b2EdgeShape)m_shape;
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
                        b2PolygonShape s = (b2PolygonShape)m_shape;
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
                        b2ChainShape s = (b2ChainShape)m_shape;
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
