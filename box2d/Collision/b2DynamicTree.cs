/*
* Copyright (c) 2009 Erin Catto http://www.box2d.org
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
using System.Collections.Generic;
using System.Diagnostics;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;

namespace Box2D.Collision
{

    /// A node in the dynamic tree. The client does not interact with this directly.
    public class b2TreeNode
    {
        public const int b2_nullNode = -1;

        public bool IsLeaf()
        {
            return child1 == b2TreeNode.b2_nullNode;
        }

        /// Enlarged AABB
        public b2AABB aabb;

        public object userData;
        public int parentOrNext;
//        public int parent;
//        public int next;

        public int child1;
        public int child2;

        // leaf = 0, free node = -1
        public int height;
    };

    /// A dynamic AABB tree broad-phase, inspired by Nathanael Presson's btDbvt.
    /// A dynamic tree arranges data in a binary tree to accelerate
    /// queries such as volume queries and ray casts. Leafs are proxies
    /// with an AABB. In the tree we expand the proxy AABB by b2_fatAABBFactor
    /// so that the proxy AABB is bigger than the client object. This allows the client
    /// object to move by small amounts without triggering a tree update.
    ///
    /// Nodes are pooled and relocatable, so we use node indices rather than pointers.
    public class b2DynamicTree
    {
        private int m_root;

        private b2TreeNode[]  m_nodes;
        private int m_nodeCount;
        private int m_nodeCapacity;

        private int m_freeList;

        /// This is used to incrementally traverse the tree for re-balancing.
//        private uint m_path;

        private int m_insertionCount;

        public b2DynamicTree()
        {
            m_root = b2TreeNode.b2_nullNode;

            m_nodeCapacity = 16;
            m_nodeCount = 0;
            m_nodes = new b2TreeNode[m_nodeCapacity];

            // Build a linked list for the free list.
            for (int i = 0; i < m_nodeCapacity - 1; ++i)
            {
				if (m_nodes[i] == null)
					m_nodes[i] = new b2TreeNode();

                m_nodes[i].parentOrNext = i + 1;
                m_nodes[i].height = -1;
            }
			if (m_nodes[m_nodeCapacity - 1] == null)
				m_nodes[m_nodeCapacity - 1] = new b2TreeNode();

            m_nodes[m_nodeCapacity - 1].parentOrNext = b2TreeNode.b2_nullNode;
            m_nodes[m_nodeCapacity - 1].height = -1;
            m_freeList = 0;

//            m_path = 0;

            m_insertionCount = 0;
        }

        // Allocate a node from the pool. Grow the pool if necessary.
        public int AllocateNode()
        {
            // Expand the node pool as needed.
            if (m_freeList == b2TreeNode.b2_nullNode)
            {
                Debug.Assert(m_nodeCount == m_nodeCapacity);

                // The free list is empty. Rebuild a bigger pool.
                b2TreeNode[]  oldNodes = m_nodes;
                m_nodeCapacity *= 2;
                m_nodes = new b2TreeNode[m_nodeCapacity];

				// initialize new b2TreeNode
                oldNodes.CopyTo(m_nodes, 0);

                // Build a linked list for the free list. The parent
                // pointer becomes the "next" pointer.
                for (int i = m_nodeCount; i < m_nodeCapacity - 1; ++i)
                {
					if (m_nodes[i] == null)
						m_nodes[i] = new b2TreeNode();

                    m_nodes[i].parentOrNext = i + 1;
                    m_nodes[i].height = -1;
                }

				if (m_nodes[m_nodeCapacity - 1] == null)
					m_nodes[m_nodeCapacity - 1] = new b2TreeNode();

                m_nodes[m_nodeCapacity - 1].parentOrNext = b2TreeNode.b2_nullNode;
                m_nodes[m_nodeCapacity - 1].height = -1;
                m_freeList = m_nodeCount;
            }

            // Peel a node off the free list.
            int nodeId = m_freeList;
            m_freeList = m_nodes[nodeId].parentOrNext;
            m_nodes[nodeId].parentOrNext = b2TreeNode.b2_nullNode;
            m_nodes[nodeId].child1 = b2TreeNode.b2_nullNode;
            m_nodes[nodeId].child2 = b2TreeNode.b2_nullNode;
            m_nodes[nodeId].height = 0;
            m_nodes[nodeId].userData = null;
            ++m_nodeCount;
            return nodeId;
        }

        // Return a node to the pool.
        void FreeNode(int nodeId)
        {
            Debug.Assert(0 <= nodeId && nodeId < m_nodeCapacity);
            Debug.Assert(0 < m_nodeCount);
            m_nodes[nodeId].parentOrNext = m_freeList;
            m_nodes[nodeId].height = -1;
            m_freeList = nodeId;
            --m_nodeCount;
        }

        // Create a proxy in the tree as a leaf node. We return the index
        // of the node instead of a pointer so that we can grow
        // the node pool.
        public int CreateProxy(b2AABB aabb, object userData)
        {
            int proxyId = AllocateNode();

            // Fatten the aabb.
            m_nodes[proxyId].aabb = aabb;
            m_nodes[proxyId].aabb.Fatten();
            m_nodes[proxyId].userData = userData;
            m_nodes[proxyId].height = 0;

            InsertLeaf(proxyId);

            return proxyId;
        }

        public void DestroyProxy(int proxyId)
        {
            Debug.Assert(0 <= proxyId && proxyId < m_nodeCapacity);
            Debug.Assert(m_nodes[proxyId].IsLeaf());

            RemoveLeaf(proxyId);
            FreeNode(proxyId);
        }

        public bool MoveProxy(int proxyId, b2AABB aabb, b2Vec2 displacement)
        {
            Debug.Assert(0 <= proxyId && proxyId < m_nodeCapacity);

            Debug.Assert(m_nodes[proxyId].IsLeaf());

            if (m_nodes[proxyId].aabb.Contains(ref aabb))
            {
                return false;
            }

            RemoveLeaf(proxyId);

            // Extend AABB.
            b2AABB b = aabb;
            b.Fatten();

            // Predict AABB displacement.
            b2Vec2 d = b2Settings.b2_aabbMultiplier * displacement;

            if (d.x < 0.0f)
            {
                b.LowerBoundX += d.x;
            }
            else
            {
                b.UpperBoundX += d.x;
            }

            if (d.y < 0.0f)
            {
                b.LowerBoundY += d.y;
            }
            else
            {
                b.UpperBoundY += d.y;
            }

            b.UpdateAttributes();
            m_nodes[proxyId].aabb = b;

            InsertLeaf(proxyId);
            return true;
        }

        public void InsertLeaf(int leaf)
        {
            ++m_insertionCount;

            if (m_root == b2TreeNode.b2_nullNode)
            {
                m_root = leaf;
                m_nodes[m_root].parentOrNext = b2TreeNode.b2_nullNode;
                return;
            }

            // Find the best sibling for this node
            b2AABB leafAABB = m_nodes[leaf].aabb;
            int index = m_root;
            while (m_nodes[index].IsLeaf() == false)
            {
                int child1 = m_nodes[index].child1;
                int child2 = m_nodes[index].child2;

                float area = m_nodes[index].aabb.Perimeter;

                b2AABB combinedAABB = b2AABB.Default;
                combinedAABB.Combine(ref m_nodes[index].aabb, ref leafAABB);
                float combinedArea = combinedAABB.Perimeter;

                // Cost of creating a new parent for this node and the new leaf
                float cost = 2.0f * combinedArea;

                // Minimum cost of pushing the leaf further down the tree
                float inheritanceCost = 2.0f * (combinedArea - area);

                // Cost of descending into child1
                float cost1;
                if (m_nodes[child1].IsLeaf())
                {
                    b2AABB aabb = b2AABB.Default;
                    aabb.Combine(ref leafAABB, ref m_nodes[child1].aabb);
                    cost1 = aabb.Perimeter + inheritanceCost;
                }
                else
                {
                    b2AABB aabb = b2AABB.Default;
                    aabb.Combine(ref leafAABB, ref m_nodes[child1].aabb);
                    float oldArea = m_nodes[child1].aabb.Perimeter;
                    float newArea = aabb.Perimeter;
                    cost1 = (newArea - oldArea) + inheritanceCost;
                }

                // Cost of descending into child2
                float cost2;
                if (m_nodes[child2].IsLeaf())
                {
                    b2AABB aabb = b2AABB.Default;
                    aabb.Combine(ref leafAABB, ref m_nodes[child2].aabb);
                    cost2 = aabb.Perimeter + inheritanceCost;
                }
                else
                {
                    b2AABB aabb = b2AABB.Default;
                    aabb.Combine(ref leafAABB, ref m_nodes[child2].aabb);
                    float oldArea = m_nodes[child2].aabb.Perimeter;
                    float newArea = aabb.Perimeter;
                    cost2 = newArea - oldArea + inheritanceCost;
                }

                // Descend according to the minimum cost.
                if (cost < cost1 && cost < cost2)
                {
                    break;
                }

                // Descend
                if (cost1 < cost2)
                {
                    index = child1;
                }
                else
                {
                    index = child2;
                }
            }

            int sibling = index;


            // Create a new parent.
            int oldParent = m_nodes[sibling].parentOrNext;
            int newParent = AllocateNode();
            m_nodes[newParent].parentOrNext = oldParent;
            m_nodes[newParent].userData = null;
            m_nodes[newParent].aabb.Combine(ref leafAABB, ref m_nodes[sibling].aabb);
            m_nodes[newParent].height = m_nodes[sibling].height + 1;

            if (oldParent != b2TreeNode.b2_nullNode)
            {
                // The sibling was not the root.
                if (m_nodes[oldParent].child1 == sibling)
                {
                    m_nodes[oldParent].child1 = newParent;
                }
                else
                {
                    m_nodes[oldParent].child2 = newParent;
                }

                m_nodes[newParent].child1 = sibling;
                m_nodes[newParent].child2 = leaf;
                m_nodes[sibling].parentOrNext = newParent;
                m_nodes[leaf].parentOrNext = newParent;
            }
            else
            {
                // The sibling was the root.
                m_nodes[newParent].child1 = sibling;
                m_nodes[newParent].child2 = leaf;
                m_nodes[sibling].parentOrNext = newParent;
                m_nodes[leaf].parentOrNext = newParent;
                m_root = newParent;
            }

            // Walk back up the tree fixing heights and AABBs
            index = m_nodes[leaf].parentOrNext;
            while (index != b2TreeNode.b2_nullNode)
            {
                index = Balance(index);

                int child1 = m_nodes[index].child1;
                int child2 = m_nodes[index].child2;

                Debug.Assert(child1 != b2TreeNode.b2_nullNode);
                Debug.Assert(child2 != b2TreeNode.b2_nullNode);

                m_nodes[index].height = 1 + Math.Max(m_nodes[child1].height, m_nodes[child2].height);
                m_nodes[index].aabb.Combine(ref m_nodes[child1].aabb, ref m_nodes[child2].aabb);

                index = m_nodes[index].parentOrNext;
            }

            //Validate();
        }

        void RemoveLeaf(int leaf)
        {
            if (leaf == m_root)
            {
                m_root = b2TreeNode.b2_nullNode;
                return;
            }

            int parent = m_nodes[leaf].parentOrNext;
            int grandParent = m_nodes[parent].parentOrNext;
            int sibling;
            if (m_nodes[parent].child1 == leaf)
            {
                sibling = m_nodes[parent].child2;
            }
            else
            {
                sibling = m_nodes[parent].child1;
            }

            if (grandParent != b2TreeNode.b2_nullNode)
            {
                // Destroy parent and connect sibling to grandParent.
                if (m_nodes[grandParent].child1 == parent)
                {
                    m_nodes[grandParent].child1 = sibling;
                }
                else
                {
                    m_nodes[grandParent].child2 = sibling;
                }
                m_nodes[sibling].parentOrNext = grandParent;
                FreeNode(parent);

                // Adjust ancestor bounds.
                int index = grandParent;
                while (index != b2TreeNode.b2_nullNode)
                {
                    index = Balance(index);

                    int child1 = m_nodes[index].child1;
                    int child2 = m_nodes[index].child2;

                    m_nodes[index].aabb.Combine(ref m_nodes[child1].aabb, ref m_nodes[child2].aabb);
                    m_nodes[index].height = 1 + Math.Max(m_nodes[child1].height, m_nodes[child2].height);

                    index = m_nodes[index].parentOrNext;
                }
            }
            else
            {
                m_root = sibling;
                m_nodes[sibling].parentOrNext = b2TreeNode.b2_nullNode;
                FreeNode(parent);
            }

            //Validate();
        }

        // Perform a left or right rotation if node A is imbalanced.
        // Returns the new root index.
        int Balance(int iA)
        {
            Debug.Assert(iA != b2TreeNode.b2_nullNode);

            b2TreeNode A = m_nodes[iA];
            if (A.IsLeaf() || A.height < 2)
            {
                return iA;
            }

            int iB = A.child1;
            int iC = A.child2;
            Debug.Assert(0 <= iB && iB < m_nodeCapacity);
            Debug.Assert(0 <= iC && iC < m_nodeCapacity);

            b2TreeNode B = m_nodes[iB];
            b2TreeNode C = m_nodes[iC];

            int balance = C.height - B.height;

            // Rotate C up
            if (balance > 1)
            {
                int iF = C.child1;
                int iG = C.child2;
                b2TreeNode F = m_nodes[iF];
                b2TreeNode G = m_nodes[iG];
                Debug.Assert(0 <= iF && iF < m_nodeCapacity);
                Debug.Assert(0 <= iG && iG < m_nodeCapacity);

                // Swap A and C
                C.child1 = iA;
                C.parentOrNext = A.parentOrNext;
                A.parentOrNext = iC;

                // A's old parent should point to C
                if (C.parentOrNext != b2TreeNode.b2_nullNode)
                {
                    if (m_nodes[C.parentOrNext].child1 == iA)
                    {
                        m_nodes[C.parentOrNext].child1 = iC;
                    }
                    else
                    {
                        Debug.Assert(m_nodes[C.parentOrNext].child2 == iA);
                        m_nodes[C.parentOrNext].child2 = iC;
                    }
                }
                else
                {
                    m_root = iC;
                }

                // Rotate
                if (F.height > G.height)
                {
                    C.child2 = iF;
                    A.child2 = iG;
                    G.parentOrNext = iA;
                    A.aabb.Combine(ref B.aabb, ref G.aabb);
                    C.aabb.Combine(ref A.aabb, ref F.aabb);

                    A.height = 1 + Math.Max(B.height, G.height);
                    C.height = 1 + Math.Max(A.height, F.height);
                }
                else
                {
                    C.child2 = iG;
                    A.child2 = iF;
                    F.parentOrNext = iA;
                    A.aabb.Combine(ref B.aabb, ref F.aabb);
                    C.aabb.Combine(ref A.aabb, ref G.aabb);

                    A.height = 1 + Math.Max(B.height, F.height);
                    C.height = 1 + Math.Max(A.height, G.height);
                }

                return iC;
            }

            // Rotate B up
            if (balance < -1)
            {
                int iD = B.child1;
                int iE = B.child2;
                b2TreeNode D = m_nodes[iD];
                b2TreeNode E = m_nodes[iE];
                Debug.Assert(0 <= iD && iD < m_nodeCapacity);
                Debug.Assert(0 <= iE && iE < m_nodeCapacity);

                // Swap A and B
                B.child1 = iA;
                B.parentOrNext = A.parentOrNext;
                A.parentOrNext = iB;

                // A's old parent should point to B
                if (B.parentOrNext != b2TreeNode.b2_nullNode)
                {
                    if (m_nodes[B.parentOrNext].child1 == iA)
                    {
                        m_nodes[B.parentOrNext].child1 = iB;
                    }
                    else
                    {
                        Debug.Assert(m_nodes[B.parentOrNext].child2 == iA);
                        m_nodes[B.parentOrNext].child2 = iB;
                    }
                }
                else
                {
                    m_root = iB;
                }

                // Rotate
                if (D.height > E.height)
                {
                    B.child2 = iD;
                    A.child1 = iE;
                    E.parentOrNext = iA;
                    A.aabb.Combine(ref C.aabb, ref E.aabb);
                    B.aabb.Combine(ref A.aabb, ref D.aabb);

                    A.height = 1 + Math.Max(C.height, E.height);
                    B.height = 1 + Math.Max(A.height, D.height);
                }
                else
                {
                    B.child2 = iE;
                    A.child1 = iD;
                    D.parentOrNext = iA;
                    A.aabb.Combine(ref C.aabb, ref D.aabb);
                    B.aabb.Combine(ref A.aabb, ref E.aabb);

                    A.height = 1 + Math.Max(C.height, D.height);
                    B.height = 1 + Math.Max(A.height, E.height);
                }

                return iB;
            }

            return iA;
        }

        public int GetHeight()
        {
            if (m_root == b2TreeNode.b2_nullNode)
            {
                return 0;
            }

            return m_nodes[m_root].height;
        }

        //
        public float GetAreaRatio()
        {
            if (m_root == b2TreeNode.b2_nullNode)
            {
                return 0.0f;
            }

            b2TreeNode root = m_nodes[m_root];
            float rootArea = root.aabb.Perimeter;

            float totalArea = 0.0f;
            for (int i = 0; i < m_nodeCapacity; ++i)
            {
                b2TreeNode node = m_nodes[i];
                if (node.height < 0)
                {
                    // Free node in pool
                    continue;
                }

                totalArea += node.aabb.Perimeter;
            }

            return totalArea / rootArea;
        }

        // Compute the height of a sub-tree.
        public int ComputeHeight(int nodeId)
        {
            Debug.Assert(0 <= nodeId && nodeId < m_nodeCapacity);
            b2TreeNode node = m_nodes[nodeId];

            if (node.IsLeaf())
            {
                return 0;
            }

            int height1 = ComputeHeight(node.child1);
            int height2 = ComputeHeight(node.child2);
            return 1 + Math.Max(height1, height2);
        }

        public int ComputeHeight()
        {
            int height = ComputeHeight(m_root);
            return height;
        }

        public void ValidateStructure(int index)
        {
            if (index == b2TreeNode.b2_nullNode)
            {
                return;
            }

            if (index == m_root)
            {
                Debug.Assert(m_nodes[index].parentOrNext == b2TreeNode.b2_nullNode);
            }

            b2TreeNode node = m_nodes[index];

            int child1 = node.child1;
            int child2 = node.child2;

            if (node.IsLeaf())
            {
                Debug.Assert(child1 == b2TreeNode.b2_nullNode);
                Debug.Assert(child2 == b2TreeNode.b2_nullNode);
                Debug.Assert(node.height == 0);
                return;
            }

            Debug.Assert(0 <= child1 && child1 < m_nodeCapacity);
            Debug.Assert(0 <= child2 && child2 < m_nodeCapacity);

            Debug.Assert(m_nodes[child1].parentOrNext == index);
            Debug.Assert(m_nodes[child2].parentOrNext == index);

            ValidateStructure(child1);
            ValidateStructure(child2);
        }

        public void ValidateMetrics(int index)
        {
            if (index == b2TreeNode.b2_nullNode)
            {
                return;
            }

            b2TreeNode node = m_nodes[index];

            int child1 = node.child1;
            int child2 = node.child2;

            if (node.IsLeaf())
            {
                Debug.Assert(child1 == b2TreeNode.b2_nullNode);
                Debug.Assert(child2 == b2TreeNode.b2_nullNode);
                Debug.Assert(node.height == 0);
                return;
            }

            Debug.Assert(0 <= child1 && child1 < m_nodeCapacity);
            Debug.Assert(0 <= child2 && child2 < m_nodeCapacity);

            int height1 = m_nodes[child1].height;
            int height2 = m_nodes[child2].height;
            int height;
            height = 1 + Math.Max(height1, height2);
            Debug.Assert(node.height == height);

            b2AABB aabb = b2AABB.Default;
            aabb.Combine(ref m_nodes[child1].aabb, ref m_nodes[child2].aabb);

            Debug.Assert(aabb.LowerBound == node.aabb.LowerBound);
            Debug.Assert(aabb.UpperBound == node.aabb.UpperBound);

            ValidateMetrics(child1);
            ValidateMetrics(child2);
        }

        public void Validate()
        {
            ValidateStructure(m_root);
            ValidateMetrics(m_root);

            int freeCount = 0;
            int freeIndex = m_freeList;
            while (freeIndex != b2TreeNode.b2_nullNode)
            {
                Debug.Assert(0 <= freeIndex && freeIndex < m_nodeCapacity);
                freeIndex = m_nodes[freeIndex].parentOrNext;
                ++freeCount;
            }

            Debug.Assert(GetHeight() == ComputeHeight());

            Debug.Assert(m_nodeCount + freeCount == m_nodeCapacity);
        }

        public int GetMaxBalance()
        {
            int maxBalance = 0;
            for (int i = 0; i < m_nodeCapacity; ++i)
            {
                b2TreeNode node = m_nodes[i];
                if (node.height <= 1)
                {
                    continue;
                }

                Debug.Assert(node.IsLeaf() == false);

                int child1 = node.child1;
                int child2 = node.child2;
                int balance = Math.Abs(m_nodes[child2].height - m_nodes[child1].height);
                maxBalance = Math.Max(maxBalance, balance);
            }

            return maxBalance;
        }

        public void RebuildBottomUp()
        {
            int[] nodes = new int[m_nodeCount];
            int count = 0;

            // Build array of leaves. Free the rest.
            for (int i = 0; i < m_nodeCapacity; ++i)
            {
                if (m_nodes[i].height < 0)
                {
                    // free node in pool
                    continue;
                }

                if (m_nodes[i].IsLeaf())
                {
                    m_nodes[i].parentOrNext = b2TreeNode.b2_nullNode;
                    nodes[count] = i;
                    ++count;
                }
                else
                {
                    FreeNode(i);
                }
            }

            while (count > 1)
            {
                float minCost = b2Settings.b2_maxFloat;
                int iMin = -1, jMin = -1;
                for (int i = 0; i < count; ++i)
                {
                    b2AABB aabbi = m_nodes[nodes[i]].aabb;

                    for (int j = i + 1; j < count; ++j)
                    {
                        b2AABB aabbj = m_nodes[nodes[j]].aabb;
                        b2AABB b = b2AABB.Default;
                        b.Combine(ref aabbi, ref aabbj);
                        float cost = b.Perimeter;
                        if (cost < minCost)
                        {
                            iMin = i;
                            jMin = j;
                            minCost = cost;
                        }
                    }
                }

                int index1 = nodes[iMin];
                int index2 = nodes[jMin];
                b2TreeNode child1 = m_nodes[index1];
                b2TreeNode child2 = m_nodes[index2];

                int parentIndex = AllocateNode();
                b2TreeNode parent = m_nodes[parentIndex];
                parent.child1 = index1;
                parent.child2 = index2;
                parent.height = 1 + Math.Max(child1.height, child2.height);
                parent.aabb.Combine(ref child1.aabb, ref child2.aabb);
                parent.parentOrNext = b2TreeNode.b2_nullNode;

                child1.parentOrNext = parentIndex;
                child2.parentOrNext = parentIndex;

                nodes[jMin] = nodes[count - 1];
                nodes[iMin] = parentIndex;
                --count;
            }

            m_root = nodes[0];

            Validate();
        }

        public object GetUserData(int proxyId)
        {
            Debug.Assert(0 <= proxyId && proxyId < m_nodeCapacity);
            return m_nodes[proxyId].userData;
        }

        public b2AABB GetFatAABB(int proxyId)
        {
            Debug.Assert(0 <= proxyId && proxyId < m_nodeCapacity);
            return m_nodes[proxyId].aabb;
        }


        public void Query(Ib2QueryCallback w, b2AABB aabb)
        {
            Stack<int> stack = new Stack<int>();
            stack.Push(m_root);

            while (stack.Count > 0)
            {
                int nodeId = stack.Pop();
                if (nodeId == b2TreeNode.b2_nullNode)
                {
                    continue;
                }

                b2TreeNode node = m_nodes[nodeId];

                if (b2Collision.b2TestOverlap(ref node.aabb, ref aabb))
                {
                    if (node.IsLeaf())
                    {
                        bool proceed = w.QueryCallback(nodeId);
                        if (proceed == false)
                        {
                            return;
                        }
                    }
                    else
                    {
                        if(node.child1 != b2TreeNode.b2_nullNode)
                            stack.Push(node.child1);
                        if (node.child2 != b2TreeNode.b2_nullNode)
                            stack.Push(node.child2);
                    }
                }
            }
        }

        public void RayCast(Ib2RayCastCallback callback, b2RayCastInput input)
        {
            b2Vec2 p1 = input.p1;
            b2Vec2 p2 = input.p2;
            b2Vec2 r = p2 - p1;
            Debug.Assert(r.LengthSquared > 0.0f);
            r.Normalize();

            // v is perpendicular to the segment.
            b2Vec2 v = r.NegUnitCross(); // b2Math.b2Cross(1.0f, r);
            b2Vec2 abs_v = b2Math.b2Abs(v);

            // Separating axis for segment (Gino, p80).
            // |dot(v, p1 - c)| > dot(|v|, h)

            float maxFraction = input.maxFraction;

            // Build a bounding box for the segment.
            b2AABB segmentAABB = b2AABB.Default;
            {
                b2Vec2 t = p1 + maxFraction * (p2 - p1);
                segmentAABB.Set(b2Math.b2Min(p1, t), b2Math.b2Max(p1, t));
            }

            Stack<int> stack = new Stack<int>();
            stack.Push(m_root);

            while (stack.Count > 0)
            {
                int nodeId = stack.Pop();
                if (nodeId == b2TreeNode.b2_nullNode)
                {
                    continue;
                }

                b2TreeNode node = m_nodes[nodeId];

                if (b2Collision.b2TestOverlap(ref node.aabb, ref segmentAABB) == false)
                {
                    continue;
                }

                // Separating axis for segment (Gino, p80).
                // |dot(v, p1 - c)| > dot(|v|, h)
                b2Vec2 c = node.aabb.Center;
                b2Vec2 h = node.aabb.Extents;
                float separation = b2Math.b2Abs(b2Math.b2Dot(v, p1 - c)) - b2Math.b2Dot(abs_v, h);
                if (separation > 0.0f)
                {
                    continue;
                }

                if (node.IsLeaf())
                {
                    b2RayCastInput subInput = new b2RayCastInput();
                    subInput.p1 = input.p1;
                    subInput.p2 = input.p2;
                    subInput.maxFraction = maxFraction;

                    float value = callback.RayCastCallback(subInput, nodeId);

                    if (value == 0.0f)
                    {
                        // The client has terminated the ray cast.
                        return;
                    }

                    if (value > 0.0f)
                    {
                        // Update segment bounding box.
                        maxFraction = value;
                        b2Vec2 t = p1 + maxFraction * (p2 - p1);
                        segmentAABB.Set(b2Math.b2Min(p1, t),b2Math.b2Max(p1, t));
                    }
                }
                else
                {
                    stack.Push(node.child1);
                    stack.Push(node.child2);
                }
            }
        }
    }
}