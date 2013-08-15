using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Box2D.Collision;
using Box2D.Common;
using Box2D.Dynamics;

namespace Box2D.TestBed.Tests
{
    public class DynamicTreeTest : Test, Ib2QueryCallback, Ib2RayCastCallback
    {

        public const int e_actorCount = 128;

        public DynamicTreeTest()
        {
            m_worldExtent = 15.0f;
            m_proxyExtent = 0.5f;

            Rand.Randomize(888);

            for (int i = 0; i < e_actorCount; ++i)
            {
                Actor actor = new Actor();
                m_actors[i] = actor;
                GetRandomAABB(ref actor.aabb);
                actor.proxyId = m_tree.CreateProxy(actor.aabb, actor);
            }

            m_stepCount = 0;

            float h = m_worldExtent;
            m_queryAABB.LowerBound = new b2Vec2(-3.0f, -4.0f + h);
            m_queryAABB.UpperBound = new b2Vec2(5.0f, 6.0f + h);

            m_rayCastInput.p1.Set(-5.0f, 5.0f + h);
            m_rayCastInput.p2.Set(7.0f, -4.0f + h);
            //m_rayCastInput.p1.Set(0.0f, 2.0f + h);
            //m_rayCastInput.p2.Set(0.0f, -2.0f + h);
            m_rayCastInput.maxFraction = 1.0f;

            m_automated = false;
        }

        public override void Step(Settings settings)
        {
            m_rayActor = null;
            for (int i = 0; i < e_actorCount; ++i)
            {
                m_actors[i].fraction = 1.0f;
                m_actors[i].overlap = false;
            }

            if (m_automated == true)
            {
                int actionCount = Math.Max(1, e_actorCount >> 2);

                for (int i = 0; i < actionCount; ++i)
                {
                    Action();
                }
            }

            Query();
            RayCast();

            for (int i = 0; i < e_actorCount; ++i)
            {
                Actor actor = m_actors[i];
                if (actor.proxyId == b2TreeNode.b2_nullNode)
                    continue;

                b2Color c = new b2Color(0.9f, 0.9f, 0.9f);
                if (actor == m_rayActor && actor.overlap)
                {
                    c.Set(0.9f, 0.6f, 0.6f);
                }
                else if (actor == m_rayActor)
                {
                    c.Set(0.6f, 0.9f, 0.6f);
                }
                else if (actor.overlap)
                {
                    c.Set(0.6f, 0.6f, 0.9f);
                }

                m_debugDraw.DrawAABB(actor.aabb, c);
            }

            b2Color cc = new b2Color(0.7f, 0.7f, 0.7f);
            m_debugDraw.DrawAABB(m_queryAABB, cc);

            m_debugDraw.DrawSegment(m_rayCastInput.p1, m_rayCastInput.p2, cc);

            b2Color c1 = new b2Color(0.2f, 0.9f, 0.2f);
            b2Color c2 = new b2Color(0.9f, 0.2f, 0.2f);
            m_debugDraw.DrawPoint(m_rayCastInput.p1, 6.0f, c1);
            m_debugDraw.DrawPoint(m_rayCastInput.p2, 6.0f, c2);

            if (m_rayActor != null)
            {
                b2Color cr = new b2Color(0.2f, 0.2f, 0.9f);
                b2Vec2 p = m_rayCastInput.p1 + m_rayActor.fraction * (m_rayCastInput.p2 - m_rayCastInput.p1);
                m_debugDraw.DrawPoint(p, 6.0f, cr);
            }

            {
                int height = m_tree.GetHeight();
                m_debugDraw.DrawString(5, m_textLine, "dynamic tree height = %d", height);
                m_textLine += 15;
            }

            ++m_stepCount;
        }

        public override void Keyboard(char key)
        {
            switch (key)
            {
                case 'a':
                    m_automated = !m_automated;
                    break;

                case 'c':
                    CreateProxy();
                    break;

                case 'd':
                    DestroyProxy();
                    break;

                case 'm':
                    MoveProxy();
                    break;
            }
        }

        public bool QueryCallback(int proxyId)
        {
            Actor actor = (Actor) m_tree.GetUserData(proxyId);
            actor.overlap = b2Collision.b2TestOverlap(ref m_queryAABB, ref actor.aabb);
            return true;
        }

        public float RayCastCallback(b2RayCastInput input, int proxyId)
        {
            Actor actor = (Actor) m_tree.GetUserData(proxyId);

            b2RayCastOutput output = new b2RayCastOutput();
            bool hit = actor.aabb.RayCast(out output, input);

            if (hit)
            {
                m_rayCastOutput = output;
                m_rayActor = actor;
                m_rayActor.fraction = output.fraction;
                return output.fraction;
            }

            return input.maxFraction;
        }

        private class Actor
        {
            public b2AABB aabb;
            public float fraction;
            public bool overlap;
            public int proxyId;
        };

        private void GetRandomAABB(ref b2AABB aabb)
        {
            b2Vec2 w = new b2Vec2();
            w.Set(2.0f * m_proxyExtent, 2.0f * m_proxyExtent);
            //aabb->lowerBound.x = -m_proxyExtent;
            //aabb->lowerBound.y = -m_proxyExtent + m_worldExtent;
            aabb.LowerBoundX = Rand.RandomFloat(-m_worldExtent, m_worldExtent);
            aabb.LowerBoundY = Rand.RandomFloat(0.0f, 2.0f * m_worldExtent);
            aabb.UpperBound = aabb.LowerBound + w;
        }

        private void MoveAABB(b2AABB aabb)
        {
            b2Vec2 d = new b2Vec2();
            d.x = Rand.RandomFloat(-0.5f, 0.5f);
            d.y = Rand.RandomFloat(-0.5f, 0.5f);
            //d.x = 2.0f;
            //d.y = 0.0f;
            aabb.LowerBound += d;
            aabb.UpperBound += d;

            b2Vec2 c0 = 0.5f * (aabb.LowerBound + aabb.UpperBound);
            b2Vec2 min = new b2Vec2();
            min.Set(-m_worldExtent, 0.0f);
            b2Vec2 max = new b2Vec2();
            max.Set(m_worldExtent, 2.0f * m_worldExtent);
            b2Vec2 c = b2Math.b2Clamp(c0, min, max);

            aabb.LowerBound += c - c0;
            aabb.UpperBound += c - c0;
        }

        private void CreateProxy()
        {
            for (int i = 0; i < e_actorCount; ++i)
            {
                int j = Rand.Random.Next() % e_actorCount;
                Actor actor = m_actors[j];
                if (actor.proxyId == b2TreeNode.b2_nullNode)
                {
                    GetRandomAABB(ref actor.aabb);
                    actor.proxyId = m_tree.CreateProxy(actor.aabb, actor);
                    return;
                }
            }
        }

        private void DestroyProxy()
        {
            for (int i = 0; i < e_actorCount; ++i)
            {
                int j = Rand.Random.Next() % e_actorCount;
                Actor actor = m_actors[j];
                if (actor.proxyId != b2TreeNode.b2_nullNode)
                {
                    m_tree.DestroyProxy(actor.proxyId);
                    actor.proxyId = b2TreeNode.b2_nullNode;
                    return;
                }
            }
        }

        private void MoveProxy()
        {
            for (int i = 0; i < e_actorCount; ++i)
            {
                int j = Rand.Random.Next() % e_actorCount;
                Actor actor = m_actors[j];
                if (actor.proxyId == b2TreeNode.b2_nullNode)
                {
                    continue;
                }

                b2AABB aabb0 = actor.aabb;
                MoveAABB(actor.aabb);
                b2Vec2 displacement = actor.aabb.Center - aabb0.Center;
                m_tree.MoveProxy(actor.proxyId, actor.aabb, displacement);
                return;
            }
        }

        private void Action()
        {
            int choice = Rand.Random.Next() % 20;

            switch (choice)
            {
                case 0:
                    CreateProxy();
                    break;

                case 1:
                    DestroyProxy();
                    break;

                default:
                    MoveProxy();
                    break;
            }
        }

        private void Query()
        {
            m_tree.Query(this, m_queryAABB);

            for (int i = 0; i < e_actorCount; ++i)
            {
                if (m_actors[i].proxyId == b2TreeNode.b2_nullNode)
                {
                    continue;
                }

                bool overlap = b2Collision.b2TestOverlap(ref m_queryAABB, ref m_actors[i].aabb);
                Debug.Assert(overlap == m_actors[i].overlap);
            }
        }

        private void RayCast()
        {
            m_rayActor = null;

            b2RayCastInput input = m_rayCastInput;

            // Ray cast against the dynamic tree.
            m_tree.RayCast(this, input);

            // Brute force ray cast.
            Actor bruteActor = null;
            b2RayCastOutput bruteOutput = new b2RayCastOutput();
            for (int i = 0; i < e_actorCount; ++i)
            {
                if (m_actors[i].proxyId == b2TreeNode.b2_nullNode)
                {
                    continue;
                }

                b2RayCastOutput output;
                bool hit = m_actors[i].aabb.RayCast(out output, input);
                if (hit)
                {
                    bruteActor = m_actors[i];
                    bruteOutput = output;
                    input.maxFraction = output.fraction;
                }
            }

            if (bruteActor != null)
            {
                Debug.Assert(bruteOutput.fraction == m_rayCastOutput.fraction);
            }
        }

        private float m_worldExtent;
        private float m_proxyExtent;

        private b2DynamicTree m_tree = new b2DynamicTree();
        private b2AABB m_queryAABB;
        private b2RayCastInput m_rayCastInput;
        private b2RayCastOutput m_rayCastOutput;
        private Actor m_rayActor = new Actor();
        private Actor[] m_actors = new Actor[e_actorCount];
        private int m_stepCount;
        private bool m_automated;
    }
}
