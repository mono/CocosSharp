using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.Common;
using Box2D.Dynamics;

namespace Box2D.Collision
{
    public struct b2Pair
    {
        public int proxyIdA;
        public int proxyIdB;
        public int next;
    };

    public class b2BroadPhase : Ib2QueryCallback, IComparer<b2Pair>
    {
        public static int e_nullProxy = -1;

        private b2DynamicTree<b2FixtureProxy> m_tree;

        private int m_proxyCount;

        private int[] m_moveBuffer;
        private int m_moveCapacity;
        private int m_moveCount;

        private b2Pair[] m_pairBuffer;
        private int m_pairCapacity;
        private int m_pairCount;

        private int m_queryProxyId;

        public b2BroadPhase()
        {
            m_proxyCount = 0;

            m_pairCapacity = 16;
            m_pairCount = 0;
            m_pairBuffer = new b2Pair[m_pairCapacity];

            m_moveCapacity = 16;
            m_moveCount = 0;
            m_moveBuffer = new int[m_moveCapacity];
            m_tree = new b2DynamicTree<b2FixtureProxy>();
        }

        public int CreateProxy(ref b2AABB aabb, ref b2FixtureProxy userData)
        {
            int proxyId = m_tree.CreateProxy(ref aabb, ref userData);
            ++m_proxyCount;
            BufferMove(proxyId);
            return proxyId;
        }

        public void DestroyProxy(int proxyId)
        {
            UnBufferMove(proxyId);
            --m_proxyCount;
            m_tree.DestroyProxy(proxyId);
        }

        public void MoveProxy(int proxyId, ref b2AABB aabb, ref b2Vec2 displacement)
        {
            bool buffer = m_tree.MoveProxy(proxyId, ref aabb, ref displacement);
            if (buffer)
            {
                BufferMove(proxyId);
            }
        }

        public void TouchProxy(int proxyId)
        {
            BufferMove(proxyId);
        }

        public void BufferMove(int proxyId)
        {
            if (m_moveCount == m_moveCapacity)
            {
                int[] oldBuffer = m_moveBuffer;
                m_moveCapacity *= 2;
                m_moveBuffer = new int[m_moveCapacity];
                oldBuffer.CopyTo(m_moveBuffer, 0);
            }

            m_moveBuffer[m_moveCount] = proxyId;
            ++m_moveCount;
        }

        public void UnBufferMove(int proxyId)
        {
            for (int i = 0; i < m_moveCount; ++i)
            {
                if (m_moveBuffer[i] == proxyId)
                {
                    m_moveBuffer[i] = e_nullProxy;
                    return;
                }
            }
        }

        // This is called from b2DynamicTreeQuery when we are gathering pairs.
        public virtual bool QueryCallback(int proxyId)
        {
            // A proxy cannot form a pair with itself.
            if (proxyId == m_queryProxyId)
            {
                return true;
            }

            // Grow the pair buffer as needed.
            if (m_pairCount == m_pairCapacity)
            {
                b2Pair[] oldBuffer = m_pairBuffer;
                m_pairCapacity *= 2;
                m_pairBuffer = new b2Pair[m_pairCapacity];
                oldBuffer.CopyTo(m_pairBuffer, 0);
            }

            m_pairBuffer[m_pairCount].proxyIdA = Math.Min(proxyId, m_queryProxyId);
            m_pairBuffer[m_pairCount].proxyIdB = Math.Max(proxyId, m_queryProxyId);
            ++m_pairCount;

            return true;
        }

        /// This is used to sort pairs.
        public bool b2PairLessThan(b2Pair pair1, b2Pair pair2)
        {
            if (pair1.proxyIdA < pair2.proxyIdA)
            {
                return true;
            }

            if (pair1.proxyIdA == pair2.proxyIdA)
            {
                return pair1.proxyIdB < pair2.proxyIdB;
            }

            return false;
        }

        public b2FixtureProxy GetUserData(int proxyId)
        {
            return m_tree.GetUserData(proxyId);
        }

        public bool TestOverlap(int proxyIdA, int proxyIdB)
        {
//            b2AABB aabbA = m_tree.GetFatAABB(proxyIdA);
//            b2AABB aabbB = m_tree.GetFatAABB(proxyIdB);
//            return b2Collision.b2TestOverlap(ref aabbA, ref aabbB);
            return b2Collision.b2TestOverlap(ref m_tree.m_nodes[proxyIdA].aabb, ref m_tree.m_nodes[proxyIdB].aabb);
        }

        public void GetFatAABB(int proxyId, out b2AABB output)
        {
            m_tree.GetFatAABB(proxyId, out output);
        }

        public int GetProxyCount()
        {
            return m_proxyCount;
        }

        public int GetTreeHeight()
        {
            return m_tree.GetHeight();
        }

        public int GetTreeBalance()
        {
            return m_tree.GetMaxBalance();
        }

        public float GetTreeQuality()
        {
            return m_tree.GetAreaRatio();
        }

        public void UpdatePairs(b2ContactManager callback)
        {
            // Reset pair buffer
            m_pairCount = 0;

            // Perform tree queries for all moving proxies.
            // Console.WriteLine("move_count={0}", m_moveCount);

            for (int i = 0; i < m_moveCount; ++i)
            {
                m_queryProxyId = m_moveBuffer[i];
                if (m_queryProxyId == e_nullProxy)
                {
                    continue;
                }

                // We have to query the tree with the fat AABB so that
                // we don't fail to create a pair that may touch later.
                b2AABB fatAABB;
                m_tree.GetFatAABB(m_queryProxyId, out fatAABB);

                // Query tree, create pairs and add them pair buffer.
                m_tree.Query(this, fatAABB);
            }

            // Reset move buffer
            m_moveCount = 0;

            // Sort the pair buffer to expose duplicates.
            // Sort starting with the m_pairCount
            Array.Sort(m_pairBuffer, 0, m_pairCount, this);
//			if (m_pairCount > 0)
//				Console.WriteLine("UpdatePairs " + m_pairCount);
            // Send the pairs back to the client.
            int i2 = 0;

            while (i2 < m_pairCount)
            {
                int i1 = i2;
                b2Pair primaryPair = m_pairBuffer[i2];
                var userDataA = m_tree.GetUserData(primaryPair.proxyIdA);
                var userDataB = m_tree.GetUserData(primaryPair.proxyIdB);

                callback.AddPair(ref userDataA, ref userDataB);
                ++i2;

                // Skip any duplicate pairs.
                while (i2 < m_pairCount)
                {
                    b2Pair pair = m_pairBuffer[i2];
                    if (pair.proxyIdA != primaryPair.proxyIdA || pair.proxyIdB != primaryPair.proxyIdB)
                    {
                        break;
                    }
                    ++i2;
                }
            }

            // Try to keep the tree balanced.
            //m_tree.Rebalance(4);
        }

        public virtual void Query(Ib2QueryCallback q, b2AABB aabb)
        {
            m_tree.Query(q, aabb);
        }

        public virtual void RayCast(b2WorldRayCastWrapper w, b2RayCastInput input)
        {
            m_tree.RayCast(w, input);
        }

        #region IComparer<b2Pair> Members

        public int Compare(b2Pair pair1, b2Pair pair2)
        {
			//return (b2PairLessThan(x, y) ? -1 : 1);
			
            if (pair1.proxyIdA < pair2.proxyIdA)
            {
                return -1;
            }

            if (pair1.proxyIdA == pair2.proxyIdA)
            {
                if (pair1.proxyIdB < pair2.proxyIdB)
                {
                    return -1;
                }
            }

            return 1;
        }

        #endregion

    }
}
