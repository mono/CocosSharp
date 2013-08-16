using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Box2D.Common;
using Box2D.Collision;
using Box2D.Collision.Shapes;
using Box2D.Dynamics.Joints;
using Box2D.Dynamics.Contacts;
using Box2D;

namespace Box2D.Dynamics
{

    public enum b2BodyType
    {
        b2_staticBody = 0,
        b2_kinematicBody,
        b2_dynamicBody

        //b2_bulletBody,
    }

    [Flags]
    public enum b2BodyFlags
    {
        e_islandFlag = 0x0001,
        e_awakeFlag = 0x0002,
        e_autoSleepFlag = 0x0004,
        e_bulletFlag = 0x0008,
        e_fixedRotationFlag = 0x0010,
        e_activeFlag = 0x0020,
        e_toiFlag = 0x0040
    }


    public class b2Body
    {
        protected b2BodyType m_type;
        public b2BodyType BodyType
        {
            get { return (m_type); }
            set { m_type = value; }
        }

        protected b2BodyFlags m_flags;
        public b2BodyFlags BodyFlags
        {
            get { return (m_flags); }
            set { m_flags = value; }
        }

        protected int m_islandIndex;
        public int IslandIndex
        {
            get { return (m_islandIndex); }
            set { m_islandIndex = value; }
        }

        protected b2Transform m_xf = b2Transform.Identity;        // the body origin transform
        public b2Transform XF
        {
            get { return (m_xf); }
            set { m_xf = value; }
        }
        
        public b2Transform Transform
        {
            get { return m_xf; }
            set { m_xf = value; }
        }
        public b2Vec2 Position
        {
            get { return (m_xf.p); }
        }

        public b2Sweep Sweep = b2Sweep.Zero;        // the swept motion for CCD
/*        public b2Sweep Sweep
        {
            get { return (m_sweep); }
            set { m_sweep = value; }
        }
 */
        public float Angle
        {
            get { return (Sweep.a); }
        }
        public b2Vec2 WorldCenter
        {
            get { return (Sweep.c); }
        }
        public b2Vec2 LocalCenter
        {
            get { return (Sweep.localCenter); }
        }

        protected b2Vec2 m_linearVelocity = b2Vec2.Zero;
        public b2Vec2 LinearVelocity
        {
            get { return (m_linearVelocity); }
            set
            {
                if (m_type == b2BodyType.b2_staticBody)
                {
                    return;
                }
                float l = value.LengthSquared; // same as dot with itself!
                if (l > 0.0f)
                {
                    SetAwake(true);
                }
                m_linearVelocity = value;
            }
        }
        protected float m_angularVelocity;
        public float AngularVelocity
        {
            get { return (m_angularVelocity); }
            set
            {
                if (m_type == b2BodyType.b2_staticBody)
                {
                    return;
                }

                if (value * value > 0.0f)
                {
                    SetAwake(true);
                }
                m_angularVelocity = value;
            }
        }

        protected b2Vec2 m_force = new b2Vec2();
        public b2Vec2 Force
        {
            get { return (m_force); }
            set { m_force = value; }
        }
        protected float m_torque;
        public float Torque
        {
            get { return (m_torque); }
            set { m_torque = value; }
        }

        protected b2World m_world;
        public b2World World
        {
            get { return (m_world); }
            set { m_world = value; }
        }
        protected b2Body m_Prev;
        public b2Body Prev
        {
            get { return (m_Prev); }
            set { m_Prev = value; }
        }
        protected b2Body m_Next;
        public b2Body Next
        {
            get { return (m_Next); }
            set { m_Next = value; }
        }

        /// <summary>
        /// A linked list of fixtures.
        /// </summary>
        protected b2Fixture m_fixtureList;
        public b2Fixture FixtureList
        {
            get { return (m_fixtureList); }
            set { m_fixtureList = value; }
        }
        protected int m_fixtureCount;
        public int FixtureCount
        {
            get { return (m_fixtureCount); }
            set { m_fixtureCount = value; }
        }

        protected b2JointEdge m_jointList;
        public b2JointEdge JointList
        {
            get { return (m_jointList); }
            set { m_jointList = value; }
        }
        protected b2ContactEdge m_contactList;
        public b2ContactEdge ContactList
        {
            get { return (m_contactList); }
            set { m_contactList = value; }
        }

        protected float m_mass, m_invMass;
        public float Mass
        {
            get { return (m_mass); }
            set { m_mass = value; }
        }
        public float InvertedMass
        {
            get { return (m_invMass); }
            set { m_invMass = value; }
        }

        // Rotational inertia about the center of mass.
        protected float m_I, m_invI;
        public float I
        {
            get { return (m_I); }
            set { m_I = value; }
        }
        public float Inertia
        {
            get
            {
                return m_I + m_mass * b2Math.b2Dot(Sweep.localCenter, Sweep.localCenter);
            }
        }
        public float InvertedI
        {
            get { return (m_invI); }
            set { m_invI = value; }
        }


        protected float m_linearDamping;
        public float LinearDamping
        {
            get { return (m_linearDamping); }
            set { m_linearDamping = value; }
        }
        protected float m_angularDamping;
        public float AngularDamping
        {
            get { return (m_angularDamping); }
            set { m_angularDamping = value; }
        }
        protected float m_gravityScale;
        public float GravityScale
        {
            get { return (m_gravityScale); }
            set { m_gravityScale = value; }
        }

        protected float m_sleepTime;
        public float SleepTime
        {
            get { return (m_sleepTime); }
            set { m_sleepTime = value; }
        }

        protected object m_userData;
        public object UserData
        {
            get { return (m_userData); }
            set { m_userData = value; }
        }

        public b2Body(b2BodyDef bd, b2World world)
        {
            m_flags = 0;

            if (bd.bullet)
            {
                m_flags |= b2BodyFlags.e_bulletFlag;
            }
            if (bd.fixedRotation)
            {
                m_flags |= b2BodyFlags.e_fixedRotationFlag;
            }
            if (bd.allowSleep)
            {
                m_flags |= b2BodyFlags.e_autoSleepFlag;
            }
            if (bd.awake)
            {
                m_flags |= b2BodyFlags.e_awakeFlag;
            }
            if (bd.active)
            {
                m_flags |= b2BodyFlags.e_activeFlag;
            }

            m_world = world;

            m_xf.p = bd.position;
            m_xf.q.Set(bd.angle);

            Sweep.localCenter.SetZero();
            Sweep.c0 = m_xf.p;
            Sweep.c = m_xf.p;
            Sweep.a0 = bd.angle;
            Sweep.a = bd.angle;
            Sweep.alpha0 = 0.0f;

            m_jointList = null;
            m_contactList = null;
            Prev = null;
            Next = null;

            m_linearVelocity = bd.linearVelocity;
            m_angularVelocity = bd.angularVelocity;

            m_linearDamping = bd.linearDamping;
            m_angularDamping = bd.angularDamping;
            m_gravityScale = bd.gravityScale;

            m_force.SetZero();
            m_torque = 0.0f;

            m_sleepTime = 0.0f;

            m_type = bd.type;

            if (m_type == b2BodyType.b2_dynamicBody)
            {
                m_mass = 1.0f;
                m_invMass = 1.0f;
            }
            else
            {
                m_mass = 0.0f;
                m_invMass = 0.0f;
            }

            m_I = 0.0f;
            m_invI = 0.0f;

            m_userData = bd.userData;

            m_fixtureList = null;
            m_fixtureCount = 0;
        }

        public virtual b2MassData GetMassData()
        {
            b2MassData data;
            data.mass = m_mass;
            data.I = m_I + m_mass * Sweep.localCenter.LengthSquared; //  b2Math.b2Dot(Sweep.localCenter, Sweep.localCenter);
            data.center = Sweep.localCenter;
            return (data);
        }
        public virtual b2Vec2 GetWorldPoint(b2Vec2 localPoint)
        {
            return b2Math.b2Mul(m_xf, localPoint);
        }

        public virtual b2Vec2 GetWorldVector(b2Vec2 localVector)
        {
            return b2Math.b2Mul(m_xf.q, localVector);
        }

        public virtual b2Vec2 GetLocalPoint(b2Vec2 worldPoint)
        {
            return b2Math.b2MulT(m_xf, worldPoint);
        }

        public virtual b2Vec2 GetLocalVector(b2Vec2 worldVector)
        {
            return b2Math.b2MulT(m_xf.q, worldVector);
        }

        public virtual b2Vec2 GetLinearVelocityFromWorldPoint(b2Vec2 worldPoint)
        {
            b2Vec2 diff = worldPoint - Sweep.c;
            return m_linearVelocity + b2Math.b2Cross(m_angularVelocity, ref diff);
        }

        public virtual b2Vec2 GetLinearVelocityFromLocalPoint(b2Vec2 localPoint)
        {
            return GetLinearVelocityFromWorldPoint(GetWorldPoint(localPoint));
        }
        public virtual void SetBullet(bool flag)
        {
            if (flag)
            {
                m_flags |= b2BodyFlags.e_bulletFlag;
            }
            else
            {
                m_flags &= ~b2BodyFlags.e_bulletFlag;
            }
        }

        public virtual bool IsBullet()
        {
            return (m_flags.HasFlag(b2BodyFlags.e_bulletFlag));
        }

        public virtual void SetAwake(bool flag)
        {
            if (flag)
            {
               if (!m_flags.HasFlag(b2BodyFlags.e_awakeFlag))
                {
                    m_flags |= b2BodyFlags.e_awakeFlag;
                    m_sleepTime = 0.0f;
                }
            }
            else
            {
                if (m_flags.HasFlag(b2BodyFlags.e_awakeFlag))
                {
                    m_flags &= ~b2BodyFlags.e_awakeFlag;
                    m_sleepTime = 0.0f;
                    m_linearVelocity.SetZero();
                    m_angularVelocity = 0.0f;
                    m_force.SetZero();
                    m_torque = 0.0f;
                }
            }
        }

        public virtual bool IsAwake()
        {
            return (m_flags.HasFlag(b2BodyFlags.e_awakeFlag));
        }

        public virtual bool IsActive()
        {
            return (m_flags.HasFlag(b2BodyFlags.e_activeFlag));
        }

        public virtual void SetFixedRotation(bool flag)
        {
            if (flag)
            {
                if (m_flags.HasFlag(b2BodyFlags.e_fixedRotationFlag))
                {
                    return;
                }
                m_flags |= b2BodyFlags.e_fixedRotationFlag;
            }
            else
            {
                if (!m_flags.HasFlag(b2BodyFlags.e_fixedRotationFlag))
                {
                    return;
                }
                m_flags &= ~b2BodyFlags.e_fixedRotationFlag;
            }

            ResetMassData();
        }

        public virtual bool IsFixedRotation()
        {
            return (m_flags & b2BodyFlags.e_fixedRotationFlag) == b2BodyFlags.e_fixedRotationFlag;
        }

        public virtual void SetSleepingAllowed(bool flag)
        {
            if (flag)
            {
                m_flags |= b2BodyFlags.e_autoSleepFlag;
            }
            else
            {
                m_flags &= ~b2BodyFlags.e_autoSleepFlag;
                SetAwake(true);
            }
        }

        public virtual bool IsSleepingAllowed()
        {
            return (m_flags & b2BodyFlags.e_autoSleepFlag) == b2BodyFlags.e_autoSleepFlag;
        }
        public virtual void ApplyForce(b2Vec2 force, b2Vec2 point)
        {
            if (m_type != b2BodyType.b2_dynamicBody)
            {
                return;
            }

            if (IsAwake() == false)
            {
                SetAwake(true);
            }

            m_force += force;
            m_torque += b2Math.b2Cross(point - Sweep.c, force);
        }

        public virtual void ApplyForceToCenter(b2Vec2 force)
        {
            if (m_type != b2BodyType.b2_dynamicBody)
            {
                return;
            }

            if (IsAwake() == false)
            {
                SetAwake(true);
            }

            m_force += force;
        }

        public virtual void ApplyTorque(float torque)
        {
            if (m_type != b2BodyType.b2_dynamicBody)
            {
                return;
            }

            if (IsAwake() == false)
            {
                SetAwake(true);
            }

            m_torque += torque;
        }

        public virtual void ApplyLinearImpulse(b2Vec2 impulse, b2Vec2 point)
        {
            if (m_type != b2BodyType.b2_dynamicBody)
            {
                return;
            }

            if (IsAwake() == false)
            {
                SetAwake(true);
            }
            m_linearVelocity += m_invMass * impulse;
            m_angularVelocity += m_invI * b2Math.b2Cross(point - Sweep.c, impulse);
        }

        public virtual void ApplyAngularImpulse(float impulse)
        {
            if (m_type != b2BodyType.b2_dynamicBody)
            {
                return;
            }

            if (IsAwake() == false)
            {
                SetAwake(true);
            }
            m_angularVelocity += m_invI * impulse;
        }

        public virtual void SynchronizeTransform()
        {
            m_xf.q.Set(Sweep.a);
            m_xf.p = Sweep.c - b2Math.b2Mul(m_xf.q, Sweep.localCenter);
        }

        public virtual void Advance(float alpha)
        {
            // Advance to the new safe time. This doesn't sync the broad-phase.
            Sweep.Advance(alpha);
            Sweep.c = Sweep.c0;
            Sweep.a = Sweep.a0;
            m_xf.q.Set(Sweep.a);
            m_xf.p = Sweep.c - b2Math.b2Mul(m_xf.q, Sweep.localCenter);
        }

        public virtual void SetType(b2BodyType type)
        {
            if (m_world.IsLocked == true)
            {
                return;
            }

            if (m_type == type)
            {
                return;
            }

            m_type = type;

            ResetMassData();

            if (m_type == b2BodyType.b2_staticBody)
            {
                m_linearVelocity.SetZero();
                m_angularVelocity = 0.0f;
                Sweep.a0 = Sweep.a;
                Sweep.c0 = Sweep.c;
                SynchronizeFixtures();
            }

            SetAwake(true);

            m_force.SetZero();
            m_torque = 0.0f;

            // Since the body type changed, we need to flag contacts for filtering.
            for (b2Fixture f = m_fixtureList; f != null; f = f.Next)
            {
                f.Refilter();
            }
        }

        public virtual b2Fixture CreateFixture(b2FixtureDef def)
        {
            if (m_world.IsLocked == true)
            {
                return null;
            }

            b2Fixture fixture = new b2Fixture();
            fixture.Create(this, def);

            if (m_flags.HasFlag(b2BodyFlags.e_activeFlag))
            {
                b2BroadPhase broadPhase = m_world.ContactManager.BroadPhase;
                fixture.CreateProxies(broadPhase, m_xf);
            }

            fixture.Next = m_fixtureList;
            m_fixtureList = fixture;
            ++m_fixtureCount;

            fixture.Body = this;

            // Adjust mass properties if needed.
            if (fixture.Density > 0.0f)
            {
                ResetMassData();
            }

            // Let the world know we have a new fixture. This will cause new contacts
            // to be created at the beginning of the next time step.
            m_world.Flags |= b2WorldFlags.e_newFixture;

            return fixture;
        }

        public virtual b2Fixture CreateFixture(b2Shape shape, float density)
        {
            b2FixtureDef def = new b2FixtureDef();
            def.shape = shape;
            def.density = density;
            return CreateFixture(def);
        }

        public virtual void DestroyFixture(b2Fixture fixture)
        {
            Debug.Assert(m_world.IsLocked == false);
            if (m_world.IsLocked)
            {
                return;
            }

            Debug.Assert(fixture.Body == this);

            // Remove the fixture from this body's singly linked list.
            Debug.Assert(m_fixtureCount > 0);
            b2Fixture node = m_fixtureList;
            bool found = false;
            while (node != null)
            {
                if (node == fixture)
                {
                    node = fixture.Next;
                    found = true;
                    break;
                }

                node = node.Next;
            }

            // You tried to remove a shape that is not attached to this body.
            Debug.Assert(found);

            // Destroy any contacts associated with the fixture.
            b2ContactEdge edge = m_contactList;
            while (edge != null)
            {
                b2Contact c = edge.Contact;
                edge = edge.Next;

                b2Fixture fixtureA = c.FixtureA;
                b2Fixture fixtureB = c.FixtureB;

                if (fixture == fixtureA || fixture == fixtureB)
                {
                    // This destroys the contact and removes it from
                    // this body's contact list.
                    m_world.ContactManager.Destroy(c);
                }
            }


            if (m_flags.HasFlag(b2BodyFlags.e_activeFlag))
            {
                b2BroadPhase broadPhase = m_world.ContactManager.BroadPhase;
                fixture.DestroyProxies(broadPhase);
            }

            fixture.Body = null;
            fixture.Next = null;

            --m_fixtureCount;

            // Reset the mass data.
            ResetMassData();
        }

        public virtual void ResetMassData()
        {
            // Compute mass data from shapes. Each shape has its own density.
            m_mass = 0.0f;
            m_invMass = 0.0f;
            m_I = 0.0f;
            m_invI = 0.0f;
            Sweep.localCenter.SetZero();

            // Static and kinematic bodies have zero mass.
            if (m_type == b2BodyType.b2_staticBody || m_type == b2BodyType.b2_kinematicBody)
            {
                Sweep.c0 = m_xf.p;
                Sweep.c = m_xf.p;
                Sweep.a0 = Sweep.a;
                return;
            }

            Debug.Assert(m_type == b2BodyType.b2_dynamicBody);

            // Accumulate mass over all fixtures.
            b2Vec2 localCenter = b2Math.b2Vec2_zero;
            for (b2Fixture f = m_fixtureList; f != null; f = f.Next)
            {
                if (f.Density == 0.0f)
                {
                    continue;
                }

                b2MassData massData;
                massData = f.GetMassData();
                m_mass += massData.mass;
                localCenter += massData.mass * massData.center;
                m_I += massData.I;
            }

            // Compute center of mass.
            if (m_mass > 0.0f)
            {
                m_invMass = 1.0f / m_mass;
                localCenter *= m_invMass;
            }
            else
            {
                // Force all dynamic bodies to have a positive mass.
                m_mass = 1.0f;
                m_invMass = 1.0f;
            }

            if (m_I > 0.0f && (!m_flags.HasFlag(b2BodyFlags.e_fixedRotationFlag)))
            {
                // Center the inertia about the center of mass.
                m_I -= m_mass * b2Math.b2Dot(localCenter, localCenter);
                Debug.Assert(m_I > 0.0f);
                m_invI = 1.0f / m_I;

            }
            else
            {
                m_I = 0.0f;
                m_invI = 0.0f;
            }

            // Move center of mass.
            b2Vec2 oldCenter = Sweep.c;
            Sweep.localCenter = localCenter;
            Sweep.c0 = Sweep.c = b2Math.b2Mul(m_xf, Sweep.localCenter);

            // Update center of mass velocity.
            b2Vec2 diff = Sweep.c - oldCenter;
            m_linearVelocity += b2Math.b2Cross(m_angularVelocity, ref diff);
        }

        public virtual void SetMassData(b2MassData massData)
        {
            Debug.Assert(m_world.IsLocked == false);
            if (m_world.IsLocked == true)
            {
                return;
            }

            if (m_type != b2BodyType.b2_dynamicBody)
            {
                return;
            }

            m_invMass = 0.0f;
            m_I = 0.0f;
            m_invI = 0.0f;

            m_mass = massData.mass;
            if (m_mass <= 0.0f)
            {
                m_mass = 1.0f;
            }

            m_invMass = 1.0f / m_mass;

            if (massData.I > 0.0f && (!m_flags.HasFlag(b2BodyFlags.e_fixedRotationFlag)))
            {
                m_I = massData.I - m_mass * b2Math.b2Dot(massData.center, massData.center);
                Debug.Assert(m_I > 0.0f);
                m_invI = 1.0f / m_I;
            }

            // Move center of mass.
            b2Vec2 oldCenter = Sweep.c;
            Sweep.localCenter = massData.center;
            Sweep.c0 = Sweep.c = b2Math.b2Mul(m_xf, Sweep.localCenter);

            // Update center of mass velocity.
            b2Vec2 diff = Sweep.c - oldCenter;
            m_linearVelocity += b2Math.b2Cross(m_angularVelocity, ref diff);
        }

        public virtual bool ShouldCollide(b2Body other)
        {
            // At least one body should be dynamic.
            if (m_type != b2BodyType.b2_dynamicBody && other.m_type != b2BodyType.b2_dynamicBody)
            {
                return false;
            }

            // Does a joint prevent collision?
            for (b2JointEdge jn = m_jointList; jn != null; jn = jn.Next)
            {
                if (jn.Other == other)
                {
                    if (jn.Joint.GetCollideConnected() == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public virtual void SetTransform(b2Vec2 position, float angle)
        {
            if (m_world.IsLocked == true)
            {
                return;
            }

            m_xf.q.Set(angle);
            m_xf.p = position;

            Sweep.c = b2Math.b2Mul(m_xf, Sweep.localCenter);
            Sweep.a = angle;

            Sweep.c0 = Sweep.c;
            Sweep.a0 = angle;

            b2BroadPhase broadPhase = m_world.ContactManager.BroadPhase;
            for (b2Fixture f = m_fixtureList; f != null; f = f.Next)
            {
                f.Synchronize(broadPhase, m_xf, m_xf);
            }

            m_world.ContactManager.FindNewContacts();
        }

        public virtual void SynchronizeFixtures()
        {
            b2Transform xf1 = b2Transform.Identity;
            xf1.q.Set(Sweep.a0);
            xf1.p = Sweep.c0 - b2Math.b2Mul(xf1.q, Sweep.localCenter);

            b2BroadPhase broadPhase = m_world.ContactManager.BroadPhase;
            for (b2Fixture f = m_fixtureList; f != null; f = f.Next)
            {
                f.Synchronize(broadPhase, xf1, m_xf);
            }
        }

        public virtual void SetActive(bool flag)
        {

            if (flag == IsActive())
            {
                return;
            }

            if (flag)
            {
                m_flags |= b2BodyFlags.e_activeFlag;

                // Create all proxies.
                b2BroadPhase broadPhase = m_world.ContactManager.BroadPhase;
                for (b2Fixture f = m_fixtureList; f != null; f = f.Next)
                {
                    f.CreateProxies(broadPhase, m_xf);
                }

                // Contacts are created the next time step.
            }
            else
            {
                m_flags &= ~b2BodyFlags.e_activeFlag;

                // Destroy all proxies.
                b2BroadPhase broadPhase = m_world.ContactManager.BroadPhase;
                for (b2Fixture f = m_fixtureList; f != null; f = f.Next)
                {
                    f.DestroyProxies(broadPhase);
                }

                // Destroy the attached contacts.
                b2ContactEdge ce = m_contactList;
                while (ce != null)
                {
                    b2ContactEdge ce0 = ce;
                    ce = ce.Next;
                    m_world.ContactManager.Destroy(ce0.Contact);
                }
                m_contactList = null;
            }
        }

        public virtual void Dump()
        {
            int bodyIndex = m_islandIndex;

            System.Diagnostics.Debug.WriteLine("{");
            System.Diagnostics.Debug.WriteLine("  b2BodyDef bd;");
            System.Diagnostics.Debug.WriteLine("  bd.type = {0};", m_type);
            System.Diagnostics.Debug.WriteLine("  bd.position.Set({0:N5}, {1:N5});", m_xf.p.x, m_xf.p.y);
            System.Diagnostics.Debug.WriteLine("  bd.angle = {0:N5};", Sweep.a);
            System.Diagnostics.Debug.WriteLine("  bd.linearVelocity.Set({0:N5}, {1:N5});", m_linearVelocity.x, m_linearVelocity.y);
            System.Diagnostics.Debug.WriteLine("  bd.angularVelocity = {0:N5};", m_angularVelocity);
            System.Diagnostics.Debug.WriteLine("  bd.linearDamping = {0:N5};", m_linearDamping);
            System.Diagnostics.Debug.WriteLine("  bd.angularDamping = {0:N5};", m_angularDamping);
            System.Diagnostics.Debug.WriteLine("  bd.allowSleep = {0};", m_flags.HasFlag(b2BodyFlags.e_autoSleepFlag));
            System.Diagnostics.Debug.WriteLine("  bd.awake = {0};", m_flags.HasFlag(b2BodyFlags.e_awakeFlag));
            System.Diagnostics.Debug.WriteLine("  bd.fixedRotation = {0};", m_flags.HasFlag(b2BodyFlags.e_fixedRotationFlag));
            System.Diagnostics.Debug.WriteLine("  bd.bullet = {0};", m_flags.HasFlag(b2BodyFlags.e_bulletFlag));
            System.Diagnostics.Debug.WriteLine("  bd.active = {0};", m_flags.HasFlag(b2BodyFlags.e_activeFlag));
            System.Diagnostics.Debug.WriteLine("  bd.gravityScale = {0:N5};", m_gravityScale);
            System.Diagnostics.Debug.WriteLine("  bodies[{0}] = m_world.CreateBody(bd);", m_islandIndex);
            System.Diagnostics.Debug.WriteLine("");
            for (b2Fixture f = m_fixtureList; f != null; f = f.Next)
            {
                System.Diagnostics.Debug.WriteLine("  {");
                f.Dump(bodyIndex);
                System.Diagnostics.Debug.WriteLine("  }");
            }
            System.Diagnostics.Debug.WriteLine("}");
        }
    }
}
