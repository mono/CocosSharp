using System.Diagnostics;
using System.Collections.Generic;

namespace CocosSharp
{
    public class CCParticleBatchNode : CCNode, ICCTexture
    {
        public const int ParticleDefaultCapacity = 500;
        const int BinarySearchTrigger = 50;             // The number of children that will trigger a binary search
        const int LinearSearchTrigger = 10;             // The number of children in the search range that will switch back to linear searching


        #region Properties

        public CCTextureAtlas TextureAtlas { get; private set; }
        public CCBlendFunc BlendFunc { get; set; }

        public CCTexture2D Texture
        {
            get { return TextureAtlas.Texture; }
            set
            {
                TextureAtlas.Texture = value;

                // If the new texture has No premultiplied alpha, AND the blendFunc hasn't been changed, then update it
                if (value != null && !value.HasPremultipliedAlpha && BlendFunc == CCBlendFunc.AlphaBlend)
                {
                    BlendFunc = CCBlendFunc.NonPremultiplied;
                }
            }
        }

        #endregion Properties


        #region Constructors

        public CCParticleBatchNode (string imageFile, int capacity = ParticleDefaultCapacity) 
            : this(CCTextureCache.Instance.AddImage(imageFile), capacity)
        {
        }

        public CCParticleBatchNode(CCTexture2D tex, int capacity = ParticleDefaultCapacity)
        {
            BlendFunc = CCBlendFunc.AlphaBlend;
            TextureAtlas = new CCTextureAtlas(tex, capacity);
            Children = new CCRawList<CCNode>(capacity);
        }

        #endregion Constructors


        public override void Visit()
        {
            // CAREFUL:
            // This visit is almost identical to CCNode#visit
            // with the exception that it doesn't call visit on it's children
            //
            // The alternative is to have a void CCSprite#visit, but
            // although this is less mantainable, is faster
            //
            if (!Visible)
            {
                return;
            }

            CCDrawManager.PushMatrix();

            if (Grid != null && Grid.Active)
            {
                Grid.BeforeDraw();
                TransformAncestors();
            }

            Transform();

            Draw();

            if (Grid != null && Grid.Active)
            {
                Grid.AfterDraw(this);
            }

            CCDrawManager.PopMatrix();
        }

        protected override void Draw()
        {
            if (TextureAtlas.TotalQuads == 0)
            {
                return;
            }

            CCDrawManager.BlendFunc(BlendFunc);

            TextureAtlas.DrawQuads();
        }

        void UpdateBlendFunc()
        {
            if (!TextureAtlas.Texture.HasPremultipliedAlpha)
            {
                BlendFunc = CCBlendFunc.NonPremultiplied;
            }
        }

        void UpdateAllAtlasIndexes()
        {
            int index = 0;

            for (int i = 0; i < Children.count; i++)
            {
                var child = (CCParticleSystem) Children.Elements[i];
                child.AtlasIndex = index;
                index += child.TotalParticles;
            }
        }

        void IncreaseAtlasCapacityTo(int quantity)
        {
            CCLog.Log("CocosSharp: CCParticleBatchNode: resizing TextureAtlas capacity from [{0}] to [{1}].",
                TextureAtlas.Capacity,
                quantity);

            TextureAtlas.ResizeCapacity(quantity);

        }

        //sets a 0'd quad into the quads array
        public void DisableParticle(int particleIndex)
        {
            CCV3F_C4B_T2F_Quad[] quads = TextureAtlas.Quads.Elements;
            TextureAtlas.Dirty = true;

            quads[particleIndex].BottomRight.Vertices = CCVertex3F.Zero;
            quads[particleIndex].TopRight.Vertices = CCVertex3F.Zero;
            quads[particleIndex].TopLeft.Vertices = CCVertex3F.Zero;
            quads[particleIndex].BottomLeft.Vertices = CCVertex3F.Zero;
        }


        #region Child management

        public override void AddChild(CCNode child, int zOrder, int tag)
        {
            Debug.Assert(child != null, "Argument must be non-null");
            Debug.Assert(child is CCParticleSystem, "CCParticleBatchNode only supports CCQuadParticleSystems as children");

            CCParticleSystem pChild = (CCParticleSystem) child;
            Debug.Assert(pChild.Texture.Name == TextureAtlas.Texture.Name, "CCParticleSystem is not using the same texture id");

            // If this is the 1st child, then copy blending function
            if (Children.Count == 0)
            {
                BlendFunc = pChild.BlendFunc;
            }

            Debug.Assert(BlendFunc.Source == pChild.BlendFunc.Source && BlendFunc.Destination == pChild.BlendFunc.Destination, 
                "Can't add a ParticleSystem that uses a differnt blending function");

            // No lazy sorting, so don't call base AddChild, call helper instead
            int pos = AddChildHelper(pChild, zOrder, tag);

            int atlasIndex;

            if (pos != 0)
            {
                CCParticleSystem p = (CCParticleSystem) Children[pos - 1];
                atlasIndex = p.AtlasIndex + p.TotalParticles;
            }
            else
            {
                atlasIndex = 0;
            }

            InsertChild(pChild, atlasIndex, tag);

            pChild.BatchNode = this;
        }

        // don't use lazy sorting, reordering the particle systems quads afterwards would be too complex
        // XXX research whether lazy sorting + freeing current quads and calloc a new block with size of capacity would be faster
        // XXX or possibly using vertexZ for reordering, that would be fastest
        // this helper is almost equivalent to CCNode's addChild, but doesn't make use of the lazy sorting
        int AddChildHelper(CCParticleSystem child, int z, int aTag)
        {
            Debug.Assert(child != null, "Argument must be non-nil");
            Debug.Assert(child.Parent == null, "child already added. It can't be added again");

            if (Children == null)
            {
                Children = new CCRawList<CCNode>(4);
            }

            // Don't use a lazy insert
            int pos = SearchNewPositionInChildrenForZ(z);

            Children.Insert(pos, child);

            child.Parent = this;

            child.Tag = aTag;
            child.ZOrder = z;

            if (IsRunning)
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
            Debug.Assert(Children.Contains(child), "Child doesn't belong to batch");

            var pChild = (CCParticleSystem) (child);

            if (zOrder == child.ZOrder)
            {
                return;
            }

            // no reordering if only 1 child
            if (Children.Count > 1)
            {
                int newIndex = 0, oldIndex = 0;

                GetCurrentIndex(ref oldIndex, ref newIndex, pChild, zOrder);

                if (oldIndex != newIndex)
                {
                    // reorder Children array
                    Children.RemoveAt(oldIndex);
                    Children.Insert(newIndex, pChild);

                    // save old altasIndex
                    int oldAtlasIndex = pChild.AtlasIndex;

                    // update atlas index
                    UpdateAllAtlasIndexes();

                    // Find new AtlasIndex
                    int newAtlasIndex = 0;
                    for (int i = 0; i < Children.count; i++)
                    {
                        var node = (CCParticleSystem) Children.Elements[i];
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

            pChild.ZOrder = zOrder;
        }

        void GetCurrentIndex(ref int oldIndex, ref int newIndex, CCNode child, int z)
        {
            bool foundCurrentIdx = false;
            bool foundNewIdx = false;

            int minusOne = 0;
            int count = Children.Count;

            for (int i = 0; i < count; i++)
            {
                CCNode node = Children.Elements[i];

                // new index
                if (node.ZOrder > z && ! foundNewIdx)
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

        int BinarySearchNewPositionInChildrenForZ(int start, int end, int z)
        {
            // Partition in half
            int count = end - start;
            if (count < LinearSearchTrigger)
            {
                return (SearchNewPositionInChildrenForZ(start, end, z));
            }
            int mid = (start + end) / 2;
            CCNode child = Children.Elements[mid];
            if (child.ZOrder > z)
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
        int SearchNewPositionInChildrenForZ(int z)
        {
            int count = Children.count;
            if (count > BinarySearchTrigger)
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
        int SearchNewPositionInChildrenForZ(int start, int end, int z)
        {
            int count = Children.count;

            for (int i = 0; i < count; i++)
            {
                CCNode child = Children.Elements[i];
                if (child.ZOrder > z)
                {
                    return i;
                }
            }
            return count;
        }

        public override void RemoveChild(CCNode child, bool cleanup)
        {
            if (child == null)
            {
                return;
            }

            Debug.Assert(child is CCParticleSystem,
                         "CCParticleBatchNode only supports CCQuadParticleSystems as children");
            Debug.Assert(Children.Contains(child), "CCParticleBatchNode doesn't contain the sprite. Can't remove it");

            CCParticleSystem pChild = (CCParticleSystem) child;

            // remove child helper
            TextureAtlas.RemoveQuadsAtIndex(pChild.AtlasIndex, pChild.TotalParticles);

            // after memmove of data, empty the quads at the end of array
            TextureAtlas.FillWithEmptyQuadsFromIndex(TextureAtlas.TotalQuads, pChild.TotalParticles);

            // particle could be reused for self rendering
            pChild.BatchNode = null;

            // Need to remove child from list of children before we update atlas indices
            base.RemoveChild(pChild, cleanup);

            UpdateAllAtlasIndexes();
        }

        public void RemoveChildAtIndex(int index, bool doCleanup)
        {
            RemoveChild(Children[index], doCleanup);
        }

        public override void RemoveAllChildrenWithCleanup(bool doCleanup)
        {
            for (int i = 0; i < Children.count; i++)
            {
                ((CCParticleSystem) Children.Elements[i]).BatchNode = null;
            }

            base.RemoveAllChildrenWithCleanup(doCleanup);

            TextureAtlas.RemoveAllQuads();
        }

        void InsertChild(CCParticleSystem pSystem, int index, int tag)
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

        #endregion Child management
    }
}