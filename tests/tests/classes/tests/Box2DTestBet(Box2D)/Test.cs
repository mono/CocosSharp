/*
* Farseer Physics Engine based on Box2D.XNA port:
* Copyright (c) 2010 Ian Qvist
* 
* Box2D.XNA port of Box2D:
* Copyright (c) 2009 Brandon Furtwangler, Nathan Furtwangler
*
* Original source Box2D:
* Copyright (c) 2006-2009 Erin Catto http://www.gphysics.com 
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
using Box2D.Collision;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using Box2D.Dynamics.Contacts;
using Box2D.Dynamics.Joints;
using CocosSharp;
using Random = System.Random;

namespace Box2D.TestBed
{
    public static class Rand
    {
        public static Random Random = new Random(0x2eed2eed);

        public static void Randomize(int seed)
        {
            Random = new Random(seed);
        }

        /// <summary>
        /// Random number in range [-1,1]
        /// </summary>
        /// <returns></returns>
        public static float RandomFloat()
        {
            return (float)(Random.NextDouble() * 2.0 - 1.0);
        }

        /// <summary>
        /// Random floating point number in range [lo, hi]
        /// </summary>
        /// <param name="lo">The lo.</param>
        /// <param name="hi">The hi.</param>
        /// <returns></returns>
        public static float RandomFloat(float lo, float hi)
        {
            float r = (float)Random.NextDouble();
            r = (hi - lo) * r + lo;
            return r;
        }
    }

    public struct TestEntry
    {
        public Func<Test> CreateFcn;
        public string Name;
    }

    public class DestructionListener : b2DestructionListener
    {
        public override void SayGoodbye(b2Fixture fixture)
        {
        }

        public override void SayGoodbye(b2Joint joint)
        {
            if (test.m_mouseJoint == joint)
            {
                test.m_mouseJoint = null;
            }
            else
            {
                test.JointDestroyed(joint);
            }
        }

        public Test test;
    };

    public class Settings
    {

        public b2Vec2 viewCenter = new b2Vec2(0.0f, 20.0f);
        public float hz = 60.0f;
        public int velocityIterations = 8;
        public int positionIterations = 3;
        public bool drawShapes = true;
        public bool drawJoints = true;
        public bool drawAABBs = false;
        public bool drawPairs = false;
        public bool drawContactPoints = false;
        public int drawContactNormals = 0;
        public int drawContactForces = 0;
        public int drawFrictionForces = 0;
        public bool drawCOMs = false;
        public bool drawStats = true;
        public bool drawProfile = true;
        public int enableWarmStarting = 1;
        public int enableContinuous = 1;
        public int enableSubStepping = 0;
        public bool pause = false;
        public bool singleStep = false;
    }


    public class ContactPoint
    {
        public b2Fixture fixtureA;
        public b2Fixture fixtureB;
        public b2Vec2 normal;
        public b2Vec2 position;
        public b2PointState state;
    };

    public class QueryCallback : b2QueryCallback
    {
        public QueryCallback(b2Vec2 point)
        {
            m_point = point;
            m_fixture = null;
        }

        public override bool ReportFixture(b2Fixture fixture)
        {
            b2Body body = fixture.Body;
            if (body.BodyType == b2BodyType.b2_dynamicBody)
            {
                bool inside = fixture.TestPoint(m_point);
                if (inside)
                {
                    m_fixture = fixture;

                    // We are done, terminate the query.
                    return false;
                }
            }

            // Continue the query.
            return true;
        }

        public b2Vec2 m_point;
        public b2Fixture m_fixture;
    }

    public class Test : b2ContactListener
    {
        public const int k_maxContactPoints = 2048;
        public b2Body m_groundBody;
        public b2AABB m_worldAABB;
        public ContactPoint[] m_points = new ContactPoint[k_maxContactPoints];
        public int m_pointCount;
        public DestructionListener m_destructionListener;
        public CCBox2dDraw m_debugDraw;
        public int m_textLine;
        public b2World m_world;
        public b2Body m_bomb;
        public b2MouseJoint m_mouseJoint;
        public b2Vec2 m_bombSpawnPoint;
        public bool m_bombSpawning;
        public b2Vec2 m_mouseWorld;
        public int m_stepCount;

#if PROFILING
        public b2Profile m_maxProfile;
        public b2Profile m_totalProfile;
#endif        

        public Test()
        {
            m_destructionListener = new DestructionListener();
            m_debugDraw = new CCBox2dDraw("fonts/arial-12", 1);

            b2Vec2 gravity = new b2Vec2();
            gravity.Set(0.0f, -10.0f);
            m_world = new b2World(gravity);
            m_bomb = null;
            m_textLine = 30;
            m_mouseJoint = null;
            m_pointCount = 0;

            m_destructionListener.test = this;
            m_world.SetDestructionListener(m_destructionListener);
            m_world.SetContactListener(this);
            m_world.SetDebugDraw(m_debugDraw);
            m_world.SetContinuousPhysics(true);
            m_world.SetWarmStarting(true);

            m_bombSpawning = false;

            m_stepCount = 0;

            b2BodyDef bodyDef = new b2BodyDef();
            m_groundBody = m_world.CreateBody(bodyDef);
        }

        public virtual void JointDestroyed(b2Joint joint)
        {
        }

        public void DrawTitle(int x, int y, string title)
        {
            m_debugDraw.DrawString(x, y, title);
        }


        public virtual bool MouseDown(b2Vec2 p)
        {
            m_mouseWorld = p;

            if (m_mouseJoint != null)
            {
                return false;
            }

            // Make a small box.
            b2AABB aabb = new b2AABB();
            b2Vec2 d = new b2Vec2();
            d.Set(0.001f, 0.001f);
            aabb.LowerBound = p - d;
            aabb.UpperBound = p + d;

            // Query the world for overlapping shapes.
            QueryCallback callback = new QueryCallback(p);
            m_world.QueryAABB(callback, aabb);

            if (callback.m_fixture != null)
            {
                b2Body body = callback.m_fixture.Body;
                b2MouseJointDef md = new b2MouseJointDef();
                md.BodyA = m_groundBody;
                md.BodyB = body;
                md.target = p;
                md.maxForce = 1000.0f * body.Mass;
                m_mouseJoint = (b2MouseJoint) m_world.CreateJoint(md);
                body.SetAwake(true);
                return true;
            }
            return false;
        }

        public void SpawnBomb(b2Vec2 worldPt)
        {
            m_bombSpawnPoint = worldPt;
            m_bombSpawning = true;
        }

        public void CompleteBombSpawn(b2Vec2 p)
        {
            if (m_bombSpawning == false)
            {
                return;
            }

            const float multiplier = 30.0f;
            b2Vec2 vel = m_bombSpawnPoint - p;
            vel *= multiplier;
            LaunchBomb(m_bombSpawnPoint, vel);
            m_bombSpawning = false;
        }

        public void ShiftMouseDown(b2Vec2 p)
        {
            m_mouseWorld = p;

            if (m_mouseJoint != null)
            {
                return;
            }

            SpawnBomb(p);
        }


        public virtual void MouseUp(b2Vec2 p)
        {
            if (m_mouseJoint != null)
            {
                m_world.DestroyJoint(m_mouseJoint);
                m_mouseJoint = null;
            }

            if (m_bombSpawning)
            {
                CompleteBombSpawn(p);
            }
        }

        public void MouseMove(b2Vec2 p)
        {
            m_mouseWorld = p;

            if (m_mouseJoint != null)
            {
                m_mouseJoint.SetTarget(p);
            }
        }

        public void LaunchBomb()
        {
            b2Vec2 p = new b2Vec2(Rand.RandomFloat(-15.0f, 15.0f), 30.0f);
            b2Vec2 v = -5.0f * p;
            LaunchBomb(p, v);
        }

        public void LaunchBomb(b2Vec2 position, b2Vec2 velocity)
        {
            if (m_bomb != null)
            {
                m_world.DestroyBody(m_bomb);
                m_bomb = null;
            }

            b2BodyDef bd = new b2BodyDef();
            bd.type = b2BodyType.b2_dynamicBody;
            bd.position = position;
            bd.bullet = true;
            m_bomb = m_world.CreateBody(bd);
            m_bomb.LinearVelocity = velocity;

            b2CircleShape circle = new b2CircleShape();
            circle.Radius = 0.3f;

            b2FixtureDef fd = new b2FixtureDef();
            fd.shape = circle;
            fd.density = 20.0f;
            fd.restitution = 0.0f;

            b2Vec2 minV = position - new b2Vec2(0.3f, 0.3f);
            b2Vec2 maxV = position + new b2Vec2(0.3f, 0.3f);

            b2AABB aabb = new b2AABB();
            aabb.LowerBound = minV;
            aabb.UpperBound = maxV;

            m_bomb.CreateFixture(fd);
        }

        public void InternalDraw(Settings settings)
        {
            m_textLine = 30;

            m_debugDraw.Begin();

            Draw(settings);

            m_debugDraw.End();
        }

        protected virtual void Draw(Settings settings)
        {
            m_world.DrawDebugData();

            if (settings.drawStats)
            {
                int bodyCount = m_world.BodyCount;
                int contactCount = m_world.ContactCount;
                int jointCount = m_world.JointCount;
                m_debugDraw.DrawString(5, m_textLine, "bodies/contacts/joints = {0}/{1}/{2}", bodyCount, contactCount,
                                       jointCount);
                m_textLine += 15;

                int proxyCount = m_world.GetProxyCount();
                int height = m_world.GetTreeHeight();
                int balance = m_world.GetTreeBalance();
                float quality = m_world.GetTreeQuality();
                m_debugDraw.DrawString(5, m_textLine, "proxies/height/balance/quality = {0}/{1}/{2}/{3}", proxyCount,
                                       height, balance, quality);
                m_textLine += 15;
            }
#if PROFILING
            // Track maximum profile times
            {
                b2Profile p = m_world.Profile;
                m_maxProfile.step = Math.Max(m_maxProfile.step, p.step);
                m_maxProfile.collide = Math.Max(m_maxProfile.collide, p.collide);
                m_maxProfile.solve = Math.Max(m_maxProfile.solve, p.solve);
                m_maxProfile.solveInit = Math.Max(m_maxProfile.solveInit, p.solveInit);
                m_maxProfile.solveVelocity = Math.Max(m_maxProfile.solveVelocity, p.solveVelocity);
                m_maxProfile.solvePosition = Math.Max(m_maxProfile.solvePosition, p.solvePosition);
                m_maxProfile.solveTOI = Math.Max(m_maxProfile.solveTOI, p.solveTOI);
                m_maxProfile.broadphase = Math.Max(m_maxProfile.broadphase, p.broadphase);

                m_totalProfile.step += p.step;
                m_totalProfile.collide += p.collide;
                m_totalProfile.solve += p.solve;
                m_totalProfile.solveInit += p.solveInit;
                m_totalProfile.solveVelocity += p.solveVelocity;
                m_totalProfile.solvePosition += p.solvePosition;
                m_totalProfile.solveTOI += p.solveTOI;
                m_totalProfile.broadphase += p.broadphase;
            }

            if (settings.drawProfile)
            {
                b2Profile p = m_world.Profile;

                b2Profile aveProfile = new b2Profile();
                if (m_stepCount > 0)
                {
                    float scale = 1.0f / m_stepCount;
                    aveProfile.step = scale * m_totalProfile.step;
                    aveProfile.collide = scale * m_totalProfile.collide;
                    aveProfile.solve = scale * m_totalProfile.solve;
                    aveProfile.solveInit = scale * m_totalProfile.solveInit;
                    aveProfile.solveVelocity = scale * m_totalProfile.solveVelocity;
                    aveProfile.solvePosition = scale * m_totalProfile.solvePosition;
                    aveProfile.solveTOI = scale * m_totalProfile.solveTOI;
                    aveProfile.broadphase = scale * m_totalProfile.broadphase;
                }

                m_debugDraw.DrawString(5, m_textLine, "step [ave] (max) = {0:00000.00} [{1:000000.00}] ({2:000000.00})", p.step,
                                       aveProfile.step, m_maxProfile.step);
                m_textLine += 15;
                m_debugDraw.DrawString(5, m_textLine, "collide [ave] (max) = {0:00000.00} [{1:000000.00}] ({2:000000.00})", p.collide,
                                       aveProfile.collide, m_maxProfile.collide);
                m_textLine += 15;
                m_debugDraw.DrawString(5, m_textLine, "solve [ave] (max) = {0:00000.00} [{1:000000.00}] ({2:000000.00})", p.solve,
                                       aveProfile.solve, m_maxProfile.solve);
                m_textLine += 15;
                m_debugDraw.DrawString(5, m_textLine, "solve init [ave] (max) = {0:00000.00} [{1:000000.00}] ({2:000000.00})", p.solveInit,
                                       aveProfile.solveInit, m_maxProfile.solveInit);
                m_textLine += 15;
                m_debugDraw.DrawString(5, m_textLine, "solve velocity [ave] (max) = {0:00000.00} [{1:000000.00}] ({2:000000.00})",
                                       p.solveVelocity, aveProfile.solveVelocity, m_maxProfile.solveVelocity);
                m_textLine += 15;
                m_debugDraw.DrawString(5, m_textLine, "solve position [ave] (max) = {0:00000.00} [{1:000000.00}] ({2:000000.00})",
                                       p.solvePosition, aveProfile.solvePosition, m_maxProfile.solvePosition);
                m_textLine += 15;
                m_debugDraw.DrawString(5, m_textLine, "solveTOI [ave] (max) = {0:00000.00} [{1:000000.00}] ({2:000000.00})", p.solveTOI,
                                       aveProfile.solveTOI, m_maxProfile.solveTOI);
                m_textLine += 15;
                m_debugDraw.DrawString(5, m_textLine, "broad-phase [ave] (max) = {0:00000.00} [{1:000000.00}] ({2:000000.00})", p.broadphase,
                                       aveProfile.broadphase, m_maxProfile.broadphase);
                m_textLine += 15;
            }
#endif
            if (m_mouseJoint != null)
            {
                b2Vec2 p1 = m_mouseJoint.GetAnchorB();
                b2Vec2 p2 = m_mouseJoint.GetTarget();

                b2Color c = new b2Color();
                c.Set(0.0f, 1.0f, 0.0f);
                m_debugDraw.DrawPoint(p1, 4.0f, c);
                m_debugDraw.DrawPoint(p2, 4.0f, c);

                c.Set(0.8f, 0.8f, 0.8f);
                m_debugDraw.DrawSegment(p1, p2, c);
            }

            if (m_bombSpawning)
            {
                b2Color c = new b2Color();
                c.Set(0.0f, 0.0f, 1.0f);
                m_debugDraw.DrawPoint(m_bombSpawnPoint, 4.0f, c);

                c.Set(0.8f, 0.8f, 0.8f);
                m_debugDraw.DrawSegment(m_mouseWorld, m_bombSpawnPoint, c);
            }

            if (settings.drawContactPoints)
            {
                //const float32 k_impulseScale = 0.1f;
                float k_axisScale = 0.3f;

                for (int i = 0; i < m_pointCount; ++i)
                {
                    ContactPoint point = m_points[i];

                    if (point.state == b2PointState.b2_addState)
                    {
                        // Add
                        m_debugDraw.DrawPoint(point.position, 10.0f, new b2Color(0.3f, 0.95f, 0.3f));
                    }
                    else if (point.state == b2PointState.b2_persistState)
                    {
                        // Persist
                        m_debugDraw.DrawPoint(point.position, 5.0f, new b2Color(0.3f, 0.3f, 0.95f));
                    }

                    if (settings.drawContactNormals == 1)
                    {
                        b2Vec2 p1 = point.position;
                        b2Vec2 p2 = p1 + k_axisScale * point.normal;
                        m_debugDraw.DrawSegment(p1, p2, new b2Color(0.9f, 0.9f, 0.9f));
                    }
                    else if (settings.drawContactForces == 1)
                    {
                        //b2Vec2 p1 = point->position;
                        //b2Vec2 p2 = p1 + k_forceScale * point->normalForce * point->normal;
                        //DrawSegment(p1, p2, b2Color(0.9f, 0.9f, 0.3f));
                    }

                    if (settings.drawFrictionForces == 1)
                    {
                        //b2Vec2 tangent = b2Cross(point->normal, 1.0f);
                        //b2Vec2 p1 = point->position;
                        //b2Vec2 p2 = p1 + k_forceScale * point->tangentForce * tangent;
                        //DrawSegment(p1, p2, b2Color(0.9f, 0.9f, 0.3f));
                    }
                }
            }

        }

        public virtual void Step(Settings settings)
        {
            float timeStep = settings.hz > 0.0f ? 1.0f / settings.hz : 0.0f;

            if (settings.pause)
            {
                if (settings.singleStep)
                {
                    settings.singleStep = false;
                }
                else
                {
                    timeStep = 0.0f;
                }

                m_debugDraw.DrawString(5, m_textLine, "****PAUSED****");
                m_textLine += 15;
            }

            b2DrawFlags flags = 0;
            if (settings.drawShapes) flags |= b2DrawFlags.e_shapeBit;
            if (settings.drawJoints) flags |= b2DrawFlags.e_jointBit;
            if (settings.drawAABBs) flags |= b2DrawFlags.e_aabbBit;
            if (settings.drawPairs) flags |= b2DrawFlags.e_pairBit;
            if (settings.drawCOMs) flags |= b2DrawFlags.e_centerOfMassBit;
            m_debugDraw.SetFlags(flags);

            m_world.SetWarmStarting(settings.enableWarmStarting > 0);
            m_world.SetContinuousPhysics(settings.enableContinuous > 0);
            m_world.SetSubStepping(settings.enableSubStepping > 0);

            m_pointCount = 0;

            m_world.Step(timeStep, settings.velocityIterations, settings.positionIterations);

            if (timeStep > 0.0f)
            {
                ++m_stepCount;
            }
        }

        public virtual void Keyboard(char key)
        {
        }

        public virtual void KeyboardUp(char key)
        {
        }

        b2PointState[] state1 = new b2PointState[b2Settings.b2_maxManifoldPoints];
        b2PointState[] state2 = new b2PointState[b2Settings.b2_maxManifoldPoints];
        b2WorldManifold worldManifold = new b2WorldManifold();

        public override void PreSolve(b2Contact contact, b2Manifold oldManifold)
        {
            b2Manifold manifold = contact.GetManifold();

            if (manifold.pointCount == 0)
            {
                return;
            }

            b2Fixture fixtureA = contact.GetFixtureA();
            b2Fixture fixtureB = contact.GetFixtureB();

            b2Collision.b2GetPointStates(state1, state2, oldManifold, manifold);

            contact.GetWorldManifold(ref worldManifold);

            for (int i = 0; i < manifold.pointCount && m_pointCount < k_maxContactPoints; ++i)
            {
                ContactPoint cp = m_points[m_pointCount];
                if (cp == null)
                {
                    cp = new ContactPoint();
                    m_points[m_pointCount] = cp;
                }
                cp.fixtureA = fixtureA;
                cp.fixtureB = fixtureB;
                cp.position = worldManifold.points[i];
                cp.normal = worldManifold.normal;
                cp.state = state2[i];
                ++m_pointCount;
            }
        }

        public override void PostSolve(b2Contact contact, ref b2ContactImpulse impulse)
        {
        }

        public override void BeginContact(b2Contact contact) {}
        public override void EndContact(b2Contact contact)  {}
    }
}