using System.Diagnostics;
using System.Collections.Generic;

namespace CocosSharp
{
    public class CCParticleBatchNode : CCNode, ICCTexture
    {
        public const int kCCParticleDefaultCapacity = 500;
        /// <summary>
        /// The number of children that will trigger a binary search
        /// </summary>
        private const int kBinarySearchTrigger = 50;
        /// <summary>
        /// The number of children in the search range that will switch back to linear searching
        /// </summary>
        private const int kLinearSearchTrigger = 10;

        public readonly CCTextureAtlas TextureAtlas = new CCTextureAtlas();
        private CCBlendFunc m_tBlendFunc;

        #region ICCTextureProtocol Members

        public CCTexture2D Texture
        {
            get { return TextureAtlas.Texture; }
            set
            {
                TextureAtlas.Texture = value;

                // If the new texture has No premultiplied alpha, AND the blendFunc hasn't been changed, then update it
                if (value != null && !value.HasPremultipliedAlpha && m_tBlendFunc == CCBlendFunc.AlphaBlend)
                {
                    m_tBlendFunc = CCBlendFunc.NonPremultiplied;
                }
            }
        }

        public CCBlendFunc BlendFunc
        {
            get { return m_tBlendFunc; }
            set { m_tBlendFunc = value; }
        }

        #endregion

        /*
         * creation with CCTexture2D
         */

        public CCParticleBatchNode (CCTexture2D tex) : this(tex, kCCParticleDefaultCapacity)
        { }

        public CCParticleBatchNode (CCTexture2D tex, int capacity /* = kCCParticleDefaultCapacity*/)
        {
            InitWithTexture(tex, capacity);
        }

        /*
         * creation with File Image
         */

        public CCParticleBatchNode (string imageFile, int capacity /* = kCCParticleDefaultCapacity*/)
        {
            InitWithFile(imageFile, capacity);
        }

        /*
         * init with CCTexture2D
         */

        public bool InitWithTexture(CCTexture2D tex, int capacity)
        {
            TextureAtlas.InitWithTexture(tex, capacity);

            // no lazy alloc in this node
            m_pChildren = new CCRawList<CCNode>(capacity);

            m_tBlendFunc = CCBlendFunc.AlphaBlend;

            return true;
        }

        /*
         * init with FileImage
         */

        public bool InitWithFile(string fileImage, int capacity)
        {
            CCTexture2D tex = CCTextureCache.SharedTextureCache.AddImage(fileImage);
            return InitWithTexture(tex, capacity);
        }

        // CCParticleBatchNode - composition

        // override visit.
        // Don't call visit on it's children
        public override void Visit()
        {
            // CAREFUL:
            // This visit is almost identical to CCNode#visit
            // with the exception that it doesn't call visit on it's children
            //
            // The alternative is to have a void CCSprite#visit, but
            // although this is less mantainable, is faster
            //
            if (!m_bVisible)
            {
                return;
            }

            //kmGLPushMatrix();
            CCDrawManager.PushMatrix();

            if (m_pGrid != null && m_pGrid.Active)
            {
                m_pGrid.BeforeDraw();
                TransformAncestors();
            }

            Transform();

            Draw();

            if (m_pGrid != null && m_pGrid.Active)
            {
                m_pGrid.AfterDraw(this);
            }

            //kmGLPopMatrix();
            CCDrawManager.PopMatrix();
        }

        // override addChild:
        public override void AddChild(CCNode child, int zOrder, int tag)
        {
            Debug.Assert(child != null, "Argument must be non-null");
            Debug.Assert(child is CCParticleSystem, "CCParticleBatchNode only supports CCQuadParticleSystems as children");
            var pChild = (CCParticleSystem) child;
            Debug.Assert(pChild.Texture.Name == TextureAtlas.Texture.Name, "CCParticleSystem is not using the same texture id");

            // If this is the 1st children, then copy blending function
            if (m_pChildren.Count == 0)
            {
                BlendFunc = pChild.BlendFunc;
            }

            Debug.Assert(m_tBlendFunc.Source == pChild.BlendFunc.Source && m_tBlendFunc.Destination == pChild.BlendFunc.Destination,
                         "Can't add a PaticleSystem that uses a differnt blending function");

            //no lazy sorting, so don't call super addChild, call helper instead
            int pos = AddChildHelper(pChild, zOrder, tag);

            //get new atlasIndex
            int atlasIndex;

            if (pos != 0)
            {
                var p = (CCParticleSystem) m_pChildren[pos - 1];
                atlasIndex = p.AtlasIndex + p.TotalParticles;
            }
            else
            {
                atlasIndex = 0;
            }

            InsertChild(pChild, atlasIndex, tag);

            // update quad info
            pChild.BatchNode = this;
        }

        // don't use lazy sorting, reordering the particle systems quads afterwards would be too complex
        // XXX research whether lazy sorting + freeing current quads and calloc a new block with size of capacity would be faster
        // XXX or possibly using vertexZ for reordering, that would be fastest
        // this helper is almost equivalent to CCNode's addChild, but doesn't make use of the lazy sorting
        private int AddChildHelper(CCParticleSystem child, int z, int aTag)
        {
            Debug.Assert(child != null, "Argument must be non-nil");
            Debug.Assert(child.Parent == null, "child already added. It can't be added again");

            if (m_pChildren == null)
            {
                m_pChildren = new CCRawList<CCNode>(4);
            }

            //don't use a lazy insert
            int pos = SearchNewPositionInChildrenForZ(z);

            m_pChildren.Insert(pos, child);

            child.Parent = this;

            child.Tag = aTag;
            child.m_nZOrder = z;

            if (m_bRunning)
            {
                child.OnEnter();
                child.OnEnterTransitionDidFinish();
            }
            return pos;
        }

        // Reorder will be done in this function, no "lazy" reorder to particles
        public override void ReorderChild(CCNode child, int zOrder)
        {
            Debug.Assert(child != null, "Child must be non-null");
            Debug.Assert(child is CCParticleSystem,
                         "CCParticleBatchNode only supports CCQuadParticleSystems as children");
            Debug.Assert(m_pChildren.Contains(child), "Child doesn't belong to batch");

            var pChild = (CCParticleSystem) (child);

            if (zOrder == child.ZOrder)
            {
                return;
            }

            // no reordering if only 1 child
            if (m_pChildren.Count > 1)
            {
                int newIndex = 0, oldIndex = 0;

                GetCurrentIndex(ref oldIndex, ref newIndex, pChild, zOrder);

                if (oldIndex != newIndex)
                {
                    // reorder m_pChildren.array
                    m_pChildren.RemoveAt(oldIndex);
                    m_pChildren.Insert(newIndex, pChild);

                    // save old altasIndex
                    int oldAtlasIndex = pChild.AtlasIndex;

                    // update atlas index
                    UpdateAllAtlasIndexes();

                    // Find new AtlasIndex
                    int newAtlasIndex = 0;
                    for (int i = 0; i < m_pChildren.count; i++)
                    {
                        var node = (CCParticleSystem) m_pChildren.Elements[i];
                        if (node == pChild)
                        {
                            newAtlasIndex = pChild.AtlasIndex;
                            break;
                        }
                    }

                    // reorder textureAtlas quads
                    TextureAtlas.MoveQuadsFromIndex(oldAtlasIndex, pChild.TotalParticles, newAtlasIndex);

                    pChild.UpdateWithNoTime();
                }
            }

            pChild.m_nZOrder = zOrder;
        }

        private void GetCurrentIndex(ref int oldIndex, ref int newIndex, CCNode child, int z)
        {
            bool foundCurrentIdx = false;
            bool foundNewIdx = false;

            int minusOne = 0;
            int count = m_pChildren.count;

            for (int i = 0; i < count; i++)
            {
                CCNode node = m_pChildren.Elements[i];

                // new index
                if (node.m_nZOrder > z && ! foundNewIdx)
                {
                    newIndex = i;
                    foundNewIdx = true;

                    if (foundCurrentIdx && foundNewIdx)
                    {
                        break;
                    }
                }

                // current index
                if (child == node)
                {
                    oldIndex = i;
                    foundCurrentIdx = true;

                    if (! foundNewIdx)
                    {
                        minusOne = -1;
                    }

                    if (foundCurrentIdx && foundNewIdx)
                    {
                        break;
                    }
                }
            }

            if (! foundNewIdx)
            {
                newIndex = count;
            }

            newIndex += minusOne;
        }

        private int BinarySearchNewPositionInChildrenForZ(int start, int end, int z)
        {
            // Partition in half
            int count = end - start;
            if (count < kLinearSearchTrigger)
            {
                return (SearchNewPositionInChildrenForZ(start, end, z));
            }
            int mid = (start + end) / 2;
            CCNode child = m_pChildren.Elements[mid];
            if (child.m_nZOrder > z)
            {
                return BinarySearchNewPositionInChildrenForZ(start, mid, z);
            }
            return (BinarySearchNewPositionInChildrenForZ(mid, end, z));
        }

        /// <summary>
        /// Do a binary search if the number of children is larger than a set limit.
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        private int SearchNewPositionInChildrenForZ(int z)
        {
            int count = m_pChildren.count;
            if (count > kBinarySearchTrigger)
            {
                return (BinarySearchNewPositionInChildrenForZ(0, count, z));
            }
            return (SearchNewPositionInChildrenForZ(0, count, z));
        }

        /// <summary>
        /// Linearly search from start to end, exclusive of end, to find a position
        /// in [start,end) where the given z < z[index+1].
        /// </summary>
        /// <param name="start">The start of the search range</param>
        /// <param name="end">The end of the search range</param>
        /// <param name="z">The z for comparison</param>
        /// <returns>The index on [start,end)</returns>
        private int SearchNewPositionInChildrenForZ(int start, int end, int z)
        {
            int count = m_pChildren.count;

            for (int i = 0; i < count; i++)
            {
                CCNode child = m_pChildren.Elements[i];
                if (child.m_nZOrder > z)
                {
                    return i;
                }
            }
            return count;
        }

        // override removeChild:
        public override void RemoveChild(CCNode child, bool cleanup)
        {
            // explicit nil handling
            if (child == null)
            {
                return;
            }

            Debug.Assert(child is CCParticleSystem,
                         "CCParticleBatchNode only supports CCQuadParticleSystems as children");
            Debug.Assert(m_pChildren.Contains(child), "CCParticleBatchNode doesn't contain the sprite. Can't remove it");

            var pChild = (CCParticleSystem) child;
            base.RemoveChild(pChild, cleanup);

            // remove child helper
            TextureAtlas.RemoveQuadsAtIndex(pChild.AtlasIndex, pChild.TotalParticles);

            // after memmove of data, empty the quads at the end of array
            TextureAtlas.FillWithEmptyQuadsFromIndex(TextureAtlas.TotalQuads, pChild.TotalParticles);

            // paticle could be reused for self rendering
            pChild.BatchNode = null;

            UpdateAllAtlasIndexes();
        }

        public void RemoveChildAtIndex(int index, bool doCleanup)
        {
            RemoveChild(m_pChildren[index], doCleanup);
        }

        public override void RemoveAllChildrenWithCleanup(bool doCleanup)
        {
            for (int i = 0; i < m_pChildren.count; i++)
            {
                ((CCParticleSystem) m_pChildren.Elements[i]).BatchNode = null;
            }

            base.RemoveAllChildrenWithCleanup(doCleanup);

            TextureAtlas.RemoveAllQuads();
        }

        public override void Draw()
        {
            if (TextureAtlas.TotalQuads == 0)
            {
                return;
            }

            CCDrawManager.BlendFunc(m_tBlendFunc);

            TextureAtlas.DrawQuads();
        }


        private void IncreaseAtlasCapacityTo(int quantity)
        {
            CCLog.Log("cocos2d: CCParticleBatchNode: resizing TextureAtlas capacity from [{0}] to [{1}].",
                      TextureAtlas.Capacity,
                      quantity);

            if (!TextureAtlas.ResizeCapacity(quantity))
            {
                // serious problems
                CCLog.Log("cocos2d: WARNING: Not enough memory to resize the atlas");
                Debug.Assert(false, "XXX: CCParticleBatchNode #increaseAtlasCapacity SHALL handle this assert");
            }
        }

        //sets a 0'd quad into the quads array
        public void DisableParticle(int particleIndex)
        {
            CCV3F_C4B_T2F_Quad[] quads = TextureAtlas.m_pQuads.Elements;
            TextureAtlas.Dirty = true;

            quads[particleIndex].BottomRight.Vertices = CCVertex3F.Zero;
            quads[particleIndex].TopRight.Vertices = CCVertex3F.Zero;
            quads[particleIndex].TopLeft.Vertices = CCVertex3F.Zero;
            quads[particleIndex].BottomLeft.Vertices = CCVertex3F.Zero;
        }

        // CCParticleBatchNode - add / remove / reorder helper methods

        // add child helper
        private void InsertChild(CCParticleSystem pSystem, int index, int tag)
        {
            pSystem.AtlasIndex = index;

            if (TextureAtlas.TotalQuads + pSystem.TotalParticles > TextureAtlas.Capacity)
            {
                IncreaseAtlasCapacityTo(TextureAtlas.TotalQuads + pSystem.TotalParticles);

                // after a realloc empty quads of textureAtlas can be filled with gibberish (realloc doesn't perform calloc), insert empty quads to prevent it
                TextureAtlas.FillWithEmptyQuadsFromIndex(TextureAtlas.Capacity - pSystem.TotalParticles,
                                                         pSystem.TotalParticles);
            }

            // make room for quads, not necessary for last child
            if (pSystem.AtlasIndex + pSystem.TotalParticles != TextureAtlas.TotalQuads)
            {
                TextureAtlas.MoveQuadsFromIndex(index, index + pSystem.TotalParticles);
            }

            // increase totalParticles here for new particles, update method of particlesystem will fill the quads
            TextureAtlas.IncreaseTotalQuadsWith(pSystem.TotalParticles);

            UpdateAllAtlasIndexes();
        }

        //rebuild atlas indexes
        private void UpdateAllAtlasIndexes()
        {
            int index = 0;

            for (int i = 0; i < m_pChildren.count; i++)
            {
                var child = (CCParticleSystem) m_pChildren.Elements[i];
                child.AtlasIndex = index;
                index += child.TotalParticles;
            }
        }

        // CCParticleBatchNode - CocosNodeTexture protocol

        private void UpdateBlendFunc()
        {
            if (!TextureAtlas.Texture.HasPremultipliedAlpha)
            {
                m_tBlendFunc = CCBlendFunc.NonPremultiplied;
            }
        }
    }
}