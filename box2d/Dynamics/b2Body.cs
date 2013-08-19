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


    public class b2Body : IComparable
    {
        public b2BodyType BodyType;

        public b2BodyFlags BodyFlags;

        public int IslandIndex;

        public b2Transform Transform = b2Transform.Identity;        // the body origin transform
        
        public b2Transform XF
        {
            get { return (Transform); }
            set { Transform = value; }
        }

        public b2Vec2 Position
        {
            get { return (Transform.p); }
        }

        public b2Sweep Sweep = b2Sweep.Zero;        // the swept motion for CCD

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

        internal b2Vec2 m_linearVelocity = b2Vec2.Zero;
        public b2Vec2 LinearVelocity
        {
            get { return (m_linearVelocity); }
            set
            {
                if (BodyType == b2BodyType.b2_staticBody)
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
                if (BodyType == b2BodyType.b2_staticBody)
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

        public b2Vec2 Force = new b2Vec2();
        public float Torque;

        public b2World World;
        public b2Body Prev;
        public b2Body Next;

        /// <summary>
        /// A linked list of fixtures.
        /// </summary>
        public b2Fixture FixtureList;

        public int FixtureCount;

        public b2JointEdge JointList;
        public b2ContactEdge ContactList;

        public float Mass, InvertedMass;

        // Rotational inertia about the center of mass.
        public float m_I, InvertedI;
        public float Inertia
        {
            get
            {
                return m_I + Mass * Sweep.localCenter.LengthSquared;
            }
        }


        public float LinearDamping;
        public float AngularDamping;
        public float GravityScale;

        public float SleepTime;

        public object UserData;

        #region IComparable Members

        public int CompareTo(object obj)
        {
            b2Body b2 = obj as b2Body;
            if (b2 == null)
            {
                return (-1);
            }
            if (BodyType != b2.BodyType)
            {
                return (BodyType.CompareTo(b2.BodyType));
            }
            return (obj == this ? 0 : -1);
        }

        #endregion

        public b2Body(b2BodyDef bd, b2World world)
        {
            BodyFlags = 0;

            if (bd.bullet)
            {
                BodyFlags |= b2BodyFlags.e_bulletFlag;
            }
            if (bd.fixedRotation)
            {
                BodyFlags |= b2BodyFlags.e_fixedRotationFlag;
            }
            if (bd.allowSleep)
            {
                BodyFlags |= b2BodyFlags.e_autoSleepFlag;
            }
            if (bd.awake)
            {
                BodyFlags |= b2BodyFlags.e_awakeFlag;
            }
            if (bd.active)
            {
                BodyFlags |= b2BodyFlags.e_activeFlag;
            }

            World = world;

            Transform.p = bd.position;
            Transform.q.Set(bd.angle);

            Sweep.localCenter.SetZero();
            Sweep.c0 = Transform.p;
            Sweep.c = Transform.p;
            Sweep.a0 = bd.angle;
            Sweep.a = bd.angle;
            Sweep.alpha0 = 0.0f;

            JointList = null;
            ContactList = null;
            Prev = null;
            Next = null;

            m_linearVelocity = bd.linearVelocity;
            m_angularVelocity = bd.angularVelocity;

            LinearDamping = bd.linearDamping;
            AngularDamping = bd.angularDamping;
            GravityScale = bd.gravityScale;

            Force.SetZero();
            Torque = 0.0f;

            SleepTime = 0.0f;

            BodyType = bd.type;

            if (BodyType == b2BodyType.b2_dynamicBody)
            {
                Mass = 1.0f;
                InvertedMass = 1.0f;
            }
            else
            {
                Mass = 0.0f;
                InvertedMass = 0.0f;
            }

            m_I = 0.0f;
            InvertedI = 0.0f;

            UserData = bd.userData;

            FixtureList = null;
            FixtureCount = 0;
        }

        public virtual b2MassData GetMassData()
        {
            b2MassData data;
            data.mass = Mass;
            data.I = m_I + Mass * Sweep.localCenter.LengthSquared; //  b2Math.b2Dot(Sweep.localCenter, Sweep.localCenter);
            data.center = Sweep.localCenter;
            return (data);
        }
        public virtual b2Vec2 GetWorldPoint(b2Vec2 localPoint)
        {
            return b2Math.b2Mul(ref Transform, ref localPoint);
        }

        public virtual b2Vec2 GetWorldVector(b2Vec2 localVector)
        {
            return b2Math.b2Mul(ref Transform.q, ref localVector);
        }

        public virtual b2Vec2 GetLocalPoint(b2Vec2 worldPoint)
        {
            return b2Math.b2MulT(ref Transform, ref worldPoint);
        }

        public virtual b2Vec2 GetLocalVector(b2Vec2 worldVector)
        {
            return b2Math.b2MulT(ref Transform.q, ref worldVector);
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
                BodyFlags |= b2BodyFlags.e_bulletFlag;
            }
            else
            {
                BodyFlags &= ~b2BodyFlags.e_bulletFlag;
            }
        }

        public virtual bool IsBullet()
        {
            return (BodyFlags & b2BodyFlags.e_bulletFlag) != 0;
        }

        public virtual void SetAwake(bool flag)
        {
            if (flag)
            {
               if ((BodyFlags & b2BodyFlags.e_awakeFlag) == 0)
                {
                    BodyFlags |= b2BodyFlags.e_awakeFlag;
                    SleepTime = 0.0f;
                }
            }
            else
            {
                if ((BodyFlags & b2BodyFlags.e_awakeFlag) != 0)
                {
                    BodyFlags &= ~b2BodyFlags.e_awakeFlag;
                    SleepTime = 0.0f;
                    m_linearVelocity = b2Vec2.Zero;
                    m_angularVelocity = 0.0f;
                    Force = b2Vec2.Zero;
                    Torque = 0.0f;
                }
            }
        }

        public virtual bool IsAwake()
        {
            return (BodyFlags & b2BodyFlags.e_awakeFlag) != 0;
        }

        public virtual bool IsActive()
        {
            return (BodyFlags & b2BodyFlags.e_activeFlag) != 0;
        }

        public virtual void SetFixedRotation(bool flag)
        {
            if (flag)
            {
                if ((BodyFlags & b2BodyFlags.e_fixedRotationFlag) != 0)
                {
                    return;
                }
                BodyFlags |= b2BodyFlags.e_fixedRotationFlag;
            }
            else
            {
                if ((BodyFlags & b2BodyFlags.e_fixedRotationFlag) == 0)
                {
                    return;
                }
                BodyFlags &= ~b2BodyFlags.e_fixedRotationFlag;
            }

            ResetMassData();
        }

        public virtual bool IsFixedRotation()
        {
            return (BodyFlags & b2BodyFlags.e_fixedRotationFlag) == b2BodyFlags.e_fixedRotationFlag;
        }

        public virtual void SetSleepingAllowed(bool flag)
        {
            if (flag)
            {
                BodyFlags |= b2BodyFlags.e_autoSleepFlag;
            }
            else
            {
                BodyFlags &= ~b2BodyFlags.e_autoSleepFlag;
                SetAwake(true);
            }
        }

        public virtual bool IsSleepingAllowed()
        {
            return (BodyFlags & b2BodyFlags.e_autoSleepFlag) == b2BodyFlags.e_autoSleepFlag;
        }
        public virtual void ApplyForce(b2Vec2 force, b2Vec2 point)
        {
            if (BodyType != b2BodyType.b2_dynamicBody)
            {
                return;
            }

            if ((BodyFlags & b2BodyFlags.e_awakeFlag) == 0)
            {
                SetAwake(true);
            }

            Force += force;
            Torque += b2Math.b2Cross(point - Sweep.c, force);
        }

        public virtual void ApplyForceToCenter(b2Vec2 force)
        {
            if (BodyType != b2BodyType.b2_dynamicBody)
            {
                return;
            }

            if ((BodyFlags & b2BodyFlags.e_awakeFlag) == 0)
            {
                SetAwake(true);
            }

            Force += force;
        }

        public virtual void ApplyTorque(float torque)
        {
            if (BodyType != b2BodyType.b2_dynamicBody)
            {
                return;
            }

            if ((BodyFlags & b2BodyFlags.e_awakeFlag) == 0)
            {
                SetAwake(true);
            }

            Torque += torque;
        }

        public virtual void ApplyLinearImpulse(b2Vec2 impulse, b2Vec2 point)
        {
            if (BodyType != b2BodyType.b2_dynamicBody)
            {
                return;
            }

            if ((BodyFlags & b2BodyFlags.e_awakeFlag) == 0)
            {
                SetAwake(true);
            }
            m_linearVelocity += InvertedMass * impulse;
            m_angularVelocity += InvertedI * b2Math.b2Cross(point - Sweep.c, impulse);
        }

        public virtual void ApplyAngularImpulse(float impulse)
        {
            if (BodyType != b2BodyType.b2_dynamicBody)
            {
                return;
            }

            if ((BodyFlags & b2BodyFlags.e_awakeFlag) == 0)
            {
                SetAwake(true);
            }
            m_angularVelocity += InvertedI * impulse;
        }

        public virtual void SynchronizeTransform()
        {
            Transform.q.Set(Sweep.a);
            Transform.p = Sweep.c - b2Math.b2Mul(Transform.q, Sweep.localCenter);
        }

        public virtual void Advance(float alpha)
        {
            // Advance to the new safe time. This doesn't sync the broad-phase.
            Sweep.Advance(alpha);
            Sweep.c = Sweep.c0;
            Sweep.a = Sweep.a0;
            Transform.q.Set(Sweep.a);
            Transform.p = Sweep.c - b2Math.b2Mul(Transform.q, Sweep.localCenter);
        }

        public virtual void SetType(b2BodyType type)
        {
            if (World.IsLocked == true)
            {
                return;
            }

            if (BodyType == type)
            {
                return;
            }

            BodyType = type;

            ResetMassData();

            if (BodyType == b2BodyType.b2_staticBody)
            {
                m_linearVelocity.SetZero();
                m_angularVelocity = 0.0f;
                Sweep.a0 = Sweep.a;
                Sweep.c0 = Sweep.c;
                SynchronizeFixtures();
            }

            SetAwake(true);

            Force.SetZero();
            Torque = 0.0f;

            // Since the body type changed, we need to flag contacts for filtering.
            for (b2Fixture f = FixtureList; f != null; f = f.Next)
            {
                f.Refilter();
            }
        }

        public virtual b2Fixture CreateFixture(b2FixtureDef def)
        {
            if (World.IsLocked == true)
            {
                return null;
            }

            b2Fixture fixture = new b2Fixture();
            fixture.Create(this, def);

            if ((BodyFlags & b2BodyFlags.e_activeFlag) != 0)
            {
                b2BroadPhase broadPhase = World.ContactManager.BroadPhase;
                fixture.CreateProxies(broadPhase, Transform);
            }

            fixture.Next = FixtureList;
            FixtureList = fixture;
            ++FixtureCount;

            fixture.Body = this;

            // Adjust mass properties if needed.
            if (fixture.Density > 0.0f)
            {
                ResetMassData();
            }

            // Let the world know we have a new fixture. This will cause new contacts
            // to be created at the beginning of the next time step.
            World.Flags |= b2WorldFlags.e_newFixture;

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
            Debug.Assert(World.IsLocked == false);
            if (World.IsLocked)
            {
                return;
            }

            Debug.Assert(fixture.Body == this);

            // Remove the fixture from this body's singly linked list.
            Debug.Assert(FixtureCount > 0);
            b2Fixture node = FixtureList;
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
            b2ContactEdge edge = ContactList;
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
                    World.ContactManager.Destroy(c);
                }
            }


            if ((BodyFlags & b2BodyFlags.e_activeFlag) != 0)
            {
                b2BroadPhase broadPhase = World.ContactManager.BroadPhase;
                fixture.DestroyProxies(broadPhase);
            }

            fixture.Body = null;
            fixture.Next = null;

            --FixtureCount;

            // Reset the mass data.
            ResetMassData();
        }

        public virtual void ResetMassData()
        {
            // Compute mass data from shapes. Each shape has its own density.
            Mass = 0.0f;
            InvertedMass = 0.0f;
            m_I = 0.0f;
            InvertedI = 0.0f;
            Sweep.localCenter.SetZero();

            // Static and kinematic bodies have zero mass.
            if (BodyType == b2BodyType.b2_staticBody || BodyType == b2BodyType.b2_kinematicBody)
            {
                Sweep.c0 = Transform.p;
                Sweep.c = Transform.p;
                Sweep.a0 = Sweep.a;
                return;
            }

            Debug.Assert(BodyType == b2BodyType.b2_dynamicBody);

            // Accumulate mass over all fixtures.
            b2Vec2 localCenter = b2Math.b2Vec2_zero;
            for (b2Fixture f = FixtureList; f != null; f = f.Next)
            {
                if (f.Density == 0.0f)
                {
                    continue;
                }

                b2MassData massData;
                massData = f.GetMassData();
                Mass += massData.mass;
                localCenter += massData.mass * massData.center;
                m_I += massData.I;
            }

            // Compute center of mass.
            if (Mass > 0.0f)
            {
                InvertedMass = 1.0f / Mass;
                localCenter *= InvertedMass;
            }
            else
            {
                // Force all dynamic bodies to have a positive mass.
                Mass = 1.0f;
                InvertedMass = 1.0f;
            }

            if (m_I > 0.0f && (BodyFlags & b2BodyFlags.e_fixedRotationFlag) == 0)
            {
                // Center the inertia about the center of mass.
                m_I -= Mass * b2Math.b2Dot(ref localCenter, ref localCenter);
                Debug.Assert(m_I > 0.0f);
                InvertedI = 1.0f / m_I;

            }
            else
            {
                m_I = 0.0f;
                InvertedI = 0.0f;
            }

            // Move center of mass.
            b2Vec2 oldCenter = Sweep.c;
            Sweep.localCenter = localCenter;
            Sweep.c0 = Sweep.c = b2Math.b2Mul(ref Transform, ref Sweep.localCenter);

            // Update center of mass velocity.
            b2Vec2 diff = Sweep.c - oldCenter;
            m_linearVelocity += b2Math.b2Cross(m_angularVelocity, ref diff);
        }

        public virtual void SetMassData(b2MassData massData)
        {
            Debug.Assert(World.IsLocked == false);
            if (World.IsLocked == true)
            {
                return;
            }

            if (BodyType != b2BodyType.b2_dynamicBody)
            {
                return;
            }

            InvertedMass = 0.0f;
            m_I = 0.0f;
            InvertedI = 0.0f;

            Mass = massData.mass;
            if (Mass <= 0.0f)
            {
                Mass = 1.0f;
            }

            InvertedMass = 1.0f / Mass;

            if (massData.I > 0.0f && (BodyFlags & b2BodyFlags.e_fixedRotationFlag) == 0)
            {
                m_I = massData.I - Mass * b2Math.b2Dot(ref massData.center, ref massData.center);
                Debug.Assert(m_I > 0.0f);
                InvertedI = 1.0f / m_I;
            }

            // Move center of mass.
            b2Vec2 oldCenter = Sweep.c;
            Sweep.localCenter = massData.center;
            Sweep.c0 = Sweep.c = b2Math.b2Mul(ref Transform, ref Sweep.localCenter);

            // Update center of mass velocity.
            b2Vec2 diff = Sweep.c - oldCenter;
            m_linearVelocity += b2Math.b2Cross(m_angularVelocity, ref diff);
        }

        public virtual bool ShouldCollide(b2Body other)
        {
            // At least one body should be dynamic.
            if (BodyType != b2BodyType.b2_dynamicBody && other.BodyType != b2BodyType.b2_dynamicBody)
            {
                return false;
            }

            // Does a joint prevent collision?
            for (b2JointEdge jn = JointList; jn != null; jn = jn.Next)
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
            if (World.IsLocked == true)
            {
                return;
            }

            Transform.q.Set(angle);
            Transform.p = position;

            Sweep.c = b2Math.b2Mul(ref Transform, ref Sweep.localCenter);
            Sweep.a = angle;

            Sweep.c0 = Sweep.c;
            Sweep.a0 = angle;

            b2BroadPhase broadPhase = World.ContactManager.BroadPhase;
            for (b2Fixture f = FixtureList; f != null; f = f.Next)
            {
                f.Synchronize(broadPhase, ref Transform, ref Transform);
            }

            World.ContactManager.FindNewContacts();
        }

        public virtual void SynchronizeFixtures()
        {
            b2Transform xf1 = b2Transform.Identity;
            xf1.q.Set(Sweep.a0);
            xf1.p = Sweep.c0 - b2Math.b2Mul(xf1.q, Sweep.localCenter);

            b2BroadPhase broadPhase = World.ContactManager.BroadPhase;
            for (b2Fixture f = FixtureList; f != null; f = f.Next)
            {
                f.Synchronize(broadPhase, ref xf1, ref Transform);
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
                BodyFlags |= b2BodyFlags.e_activeFlag;

                // Create all proxies.
                b2BroadPhase broadPhase = World.ContactManager.BroadPhase;
                for (b2Fixture f = FixtureList; f != null; f = f.Next)
                {
                    f.CreateProxies(broadPhase, Transform);
                }

                // Contacts are created the next time step.
            }
            else
            {
                BodyFlags &= ~b2BodyFlags.e_activeFlag;

                // Destroy all proxies.
                b2BroadPhase broadPhase = World.ContactManager.BroadPhase;
                for (b2Fixture f = FixtureList; f != null; f = f.Next)
                {
                    f.DestroyProxies(broadPhase);
                }

                // Destroy the attached contacts.
                b2ContactEdge ce = ContactList;
                while (ce != null)
                {
                    b2ContactEdge ce0 = ce;
                    ce = ce.Next;
                    World.ContactManager.Destroy(ce0.Contact);
                }
                ContactList = null;
            }
        }

        public virtual void Dump()
        {
            int bodyIndex = IslandIndex;

            System.Diagnostics.Debug.WriteLine("{");
            System.Diagnostics.Debug.WriteLine("  b2BodyDef bd;");
            System.Diagnostics.Debug.WriteLine("  bd.type = {0};", BodyType);
            System.Diagnostics.Debug.WriteLine("  bd.position.Set({0:N5}, {1:N5});", Transform.p.x, Transform.p.y);
            System.Diagnostics.Debug.WriteLine("  bd.angle = {0:N5};", Sweep.a);
            System.Diagnostics.Debug.WriteLine("  bd.linearVelocity.Set({0:N5}, {1:N5});", m_linearVelocity.x, m_linearVelocity.y);
            System.Diagnostics.Debug.WriteLine("  bd.angularVelocity = {0:N5};", m_angularVelocity);
            System.Diagnostics.Debug.WriteLine("  bd.linearDamping = {0:N5};", LinearDamping);
            System.Diagnostics.Debug.WriteLine("  bd.angularDamping = {0:N5};", AngularDamping);
            System.Diagnostics.Debug.WriteLine("  bd.allowSleep = {0};", BodyFlags.HasFlag(b2BodyFlags.e_autoSleepFlag));
            System.Diagnostics.Debug.WriteLine("  bd.awake = {0};", BodyFlags.HasFlag(b2BodyFlags.e_awakeFlag));
            System.Diagnostics.Debug.WriteLine("  bd.fixedRotation = {0};", BodyFlags.HasFlag(b2BodyFlags.e_fixedRotationFlag));
            System.Diagnostics.Debug.WriteLine("  bd.bullet = {0};", BodyFlags.HasFlag(b2BodyFlags.e_bulletFlag));
            System.Diagnostics.Debug.WriteLine("  bd.active = {0};", BodyFlags.HasFlag(b2BodyFlags.e_activeFlag));
            System.Diagnostics.Debug.WriteLine("  bd.gravityScale = {0:N5};", GravityScale);
            System.Diagnostics.Debug.WriteLine("  bodies[{0}] = m_world.CreateBody(bd);", IslandIndex);
            System.Diagnostics.Debug.WriteLine("");
            for (b2Fixture f = FixtureList; f != null; f = f.Next)
            {
                System.Diagnostics.Debug.WriteLine("  {");
                f.Dump(bodyIndex);
                System.Diagnostics.Debug.WriteLine("  }");
            }
            System.Diagnostics.Debug.WriteLine("}");
        }

    }
}
