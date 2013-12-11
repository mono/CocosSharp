using System;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCSpriteBatchNode : CCNode, ICCTextureProtocol
    {
        private const int kDefaultSpriteBatchCapacity = 29;

        protected CCBlendFunc m_blendFunc;
        protected CCRawList<CCSprite> m_pobDescendants;
        protected CCTextureAtlas m_pobTextureAtlas;

        public CCTextureAtlas TextureAtlas
        {
            get { return m_pobTextureAtlas; }
            set { m_pobTextureAtlas = value; }
        }

        public CCRawList<CCSprite> Descendants
        {
            get { return m_pobDescendants; }
        }

        #region ICCTextureProtocol Members

        public CCBlendFunc BlendFunc
        {
            get { return m_blendFunc; }
            set { m_blendFunc = value; }
        }

        public virtual CCTexture2D Texture
        {
            get { return m_pobTextureAtlas.Texture; }
            set
            {
                m_pobTextureAtlas.Texture = value;
                UpdateBlendFunc();
                if (value != null)
                {
                    m_obContentSize = value.ContentSize;
            	}
        	}
        }

		public bool IsAntialiased
		{
			get { return Texture.IsAntialiased; }

			set { Texture.IsAntialiased = value; }
		}

        #endregion

        public CCSpriteBatchNode()
        {
        }

        public CCSpriteBatchNode(CCTexture2D tex)
        {
            InitWithTexture(tex, kDefaultSpriteBatchCapacity);
        }

        public CCSpriteBatchNode(CCTexture2D tex, int capacity)
        {
            InitWithTexture(tex, capacity);
        }

        public CCSpriteBatchNode(string fileImage)
        {
            InitWithFile(fileImage, kDefaultSpriteBatchCapacity);
        }

        public CCSpriteBatchNode(string fileImage, int capacity)
        {
            InitWithFile(fileImage, capacity);
        }

        protected virtual bool InitWithFile(string fileImage, int capacity)
        {
            CCTexture2D pTexture2D = CCTextureCache.SharedTextureCache.AddImage(fileImage);
            return InitWithTexture(pTexture2D, capacity);
        }

        protected virtual bool InitWithTexture(CCTexture2D tex, int capacity)
        {
            m_blendFunc = CCBlendFunc.AlphaBlend;

            m_pobTextureAtlas = new CCTextureAtlas();

            if (capacity == 0)
            {
                capacity = kDefaultSpriteBatchCapacity;
            }

            ContentSize= tex.ContentSize; // @@ TotallyEvil - contentSize should return the size of the sprite sheet
            m_pobTextureAtlas.InitWithTexture(tex, capacity);

            UpdateBlendFunc();

            // no lazy alloc in this node
            m_pChildren = new CCRawList<CCNode>(capacity);
            m_pobDescendants = new CCRawList<CCSprite>(capacity);

            return true;
        }

        public override bool Init()
        {
            var texture = new CCTexture2D();
            return InitWithTexture(texture, 0);
        }

        public override void Visit()
        {
            // CAREFUL:
            // This visit is almost identical to CocosNode#visit
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

            SortAllChildren();
            Transform();

            Draw();

            if (m_pGrid != null && m_pGrid.Active)
            {
                m_pGrid.AfterDraw(this);
            }

            //kmGLPopMatrix();
            CCDrawManager.PopMatrix();

            //m_uOrderOfArrival = 0;
        }

        public override void AddChild(CCNode child, int zOrder, int tag)
        {
            Debug.Assert(child != null, "child should not be null");

            Debug.Assert(child is CCSprite, "CCSpriteBatchNode only supports CCSprites as children");

            var pSprite = (CCSprite) child;

            // check CCSprite is using the same texture id
            Debug.Assert(pSprite.Texture.Name == m_pobTextureAtlas.Texture.Name, "CCSprite is not using the same texture id");

            base.AddChild(child, zOrder, tag);

            AppendChild(pSprite);
        }

        public override void ReorderChild(CCNode child, int zOrder)
        {
            Debug.Assert(child != null, "the child should not be null");
            Debug.Assert(m_pChildren.Contains(child), "Child doesn't belong to Sprite");

            if (zOrder == child.m_nZOrder)
            {
                return;
            }

            //set the z-order and sort later
            base.ReorderChild(child, zOrder);
        }

        public override void RemoveChild(CCNode child, bool cleanup)
        {
            var pSprite = (CCSprite) child;

            Debug.Assert(m_pChildren.Contains(pSprite), "sprite batch node should contain the child");

            // cleanup before removing
            RemoveSpriteFromAtlas(pSprite);

            base.RemoveChild(pSprite, cleanup);
        }

        public void RemoveChildAtIndex(int index, bool doCleanup)
        {
            RemoveChild((m_pChildren[index]), doCleanup);
        }

        public override void RemoveAllChildrenWithCleanup(bool cleanup)
        {
            // Invalidate atlas index. issue #569
            // useSelfRender should be performed on all descendants. issue #1216
            CCSprite[] elements = m_pobDescendants.Elements;
            for (int i = 0, count = m_pobDescendants.count; i < count; i++)
            {
                elements[i].BatchNode = null;
            }

            base.RemoveAllChildrenWithCleanup(cleanup);

            m_pobDescendants.Clear();
            m_pobTextureAtlas.RemoveAllQuads();
        }

        //override sortAllChildren
        public override void SortAllChildren()
        {
            if (m_bReorderChildDirty)
            {
                int j = 0, count = m_pChildren.count;
                CCNode[] elements = m_pChildren.Elements;

                Array.Sort(elements, 0, count, this);

                //sorted now check all children
                if (count > 0)
                {
                    //first sort all children recursively based on zOrder
                    for (int i = 0; i < count; i++)
                    {
                        elements[i].SortAllChildren();
                    }

                    int index = 0;

                    //fast dispatch, give every child a new atlasIndex based on their relative zOrder (keep parent -> child relations intact)
                    // and at the same time reorder descedants and the quads to the right index
                    for (int i = 0; i < count; i++)
                    {
                        UpdateAtlasIndex((CCSprite) elements[i], ref index);
                    }
                }

                m_bReorderChildDirty = false;
            }
        }

        private void UpdateAtlasIndex(CCSprite sprite, ref int curIndex)
        {
            int count = 0;
            CCRawList<CCNode> pArray = sprite.Children;

            if (pArray != null)
            {
                count = pArray.Count;
            }

            int oldIndex = 0;

            if (count == 0)
            {
                oldIndex = sprite.AtlasIndex;
                sprite.AtlasIndex = curIndex;
                sprite.m_uOrderOfArrival = 0;
                if (oldIndex != curIndex)
                {
                    Swap(oldIndex, curIndex);
                }
                curIndex++;
            }
            else
            {
                bool needNewIndex = true;

                if (pArray.Elements[0].m_nZOrder >= 0)
                {
                    //all children are in front of the parent
                    oldIndex = sprite.AtlasIndex;
                    sprite.AtlasIndex = curIndex;
                    sprite.m_uOrderOfArrival = 0;
                    if (oldIndex != curIndex)
                    {
                        Swap(oldIndex, curIndex);
                    }
                    curIndex++;

                    needNewIndex = false;
                }

                for (int i = 0; i < count; i++)
                {
                    var child = (CCSprite) pArray.Elements[i];
                    if (needNewIndex && child.m_nZOrder >= 0)
                    {
                        oldIndex = sprite.AtlasIndex;
                        sprite.AtlasIndex = curIndex;
                        sprite.m_uOrderOfArrival = 0;
                        if (oldIndex != curIndex)
                        {
                            Swap(oldIndex, curIndex);
                        }
                        curIndex++;
                        needNewIndex = false;
                    }

                    UpdateAtlasIndex(child, ref curIndex);
                }

                if (needNewIndex)
                {
                    //all children have a zOrder < 0)
                    oldIndex = sprite.AtlasIndex;
                    sprite.AtlasIndex = curIndex;
                    sprite.m_uOrderOfArrival = 0;
                    if (oldIndex != curIndex)
                    {
                        Swap(oldIndex, curIndex);
                    }
                    curIndex++;
                }
            }
        }

        private void Swap(int oldIndex, int newIndex)
        {
            CCSprite[] sprites = m_pobDescendants.Elements;
            CCRawList<CCV3F_C4B_T2F_Quad> quads = m_pobTextureAtlas.m_pQuads;

            m_pobTextureAtlas.Dirty = true;

            CCSprite tempItem = sprites[oldIndex];
            CCV3F_C4B_T2F_Quad tempItemQuad = quads[oldIndex];

            //update the index of other swapped item
            sprites[newIndex].AtlasIndex = oldIndex;

            sprites[oldIndex] = sprites[newIndex];
            quads[oldIndex] = quads[newIndex];
            sprites[newIndex] = tempItem;
            quads[newIndex] = tempItemQuad;
        }

        public void ReorderBatch(bool reorder)
        {
            m_bReorderChildDirty = reorder;
        }

        public override void Draw()
        {
            // Optimization: Fast Dispatch	
            if (m_pobTextureAtlas.TotalQuads == 0)
            {
                return;
            }

            if (m_pChildren != null && m_pChildren.count > 0)
            {
                CCNode[] elements = m_pChildren.Elements;
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    ((CCSprite) elements[i]).UpdateTransform();
                }
            }

            CCDrawManager.BlendFunc(m_blendFunc);

            m_pobTextureAtlas.DrawQuads();
        }

        public void IncreaseAtlasCapacity()
        {
            // if we're going beyond the current TextureAtlas's capacity,
            // all the previously initialized sprites will need to redo their texture coords
            // this is likely computationally expensive
            int quantity = (m_pobTextureAtlas.Capacity + 1) * 4 / 3;

            CCLog.Log(string.Format(
                "cocos2d: CCSpriteBatchNode: resizing TextureAtlas capacity from [{0}] to [{1}].",
                m_pobTextureAtlas.Capacity, quantity));

            if (!m_pobTextureAtlas.ResizeCapacity(quantity))
            {
                // serious problems
                CCLog.Log("cocos2d: WARNING: Not enough memory to resize the atlas");
                Debug.Assert(false, "Not enough memory to resize the atla");
            }
        }

        public int RebuildIndexInOrder(CCSprite pobParent, int uIndex)
        {
            CCRawList<CCNode> pChildren = pobParent.Children;

            if (pChildren != null && pChildren.count > 0)
            {
                CCNode[] elements = pChildren.Elements;
                for (int i = 0, count = pChildren.count; i < count; i++)
                {
                    if (elements[i].ZOrder < 0)
                    {
                        uIndex = RebuildIndexInOrder((CCSprite) pChildren[i], uIndex);
                    }
                }
            }

            // ignore self (batch node)
            if (!pobParent.Equals(this))
            {
                pobParent.AtlasIndex = uIndex;
                uIndex++;
            }

            if (pChildren != null && pChildren.count > 0)
            {
                CCNode[] elements = pChildren.Elements;
                for (int i = 0, count = pChildren.count; i < count; i++)
                {
                    if (elements[i].ZOrder >= 0)
                    {
                        uIndex = RebuildIndexInOrder((CCSprite) elements[i], uIndex);
                    }
                }
            }

            return uIndex;
        }

        public int HighestAtlasIndexInChild(CCSprite pSprite)
        {
            CCRawList<CCNode> pChildren = pSprite.Children;

            if (pChildren == null || pChildren.count == 0)
            {
                return pSprite.AtlasIndex;
            }
            else
            {
                return HighestAtlasIndexInChild((CCSprite) pChildren.Elements[pChildren.count - 1]);
            }
        }

        public int LowestAtlasIndexInChild(CCSprite pSprite)
        {
            CCRawList<CCNode> pChildren = pSprite.Children;

            if (pChildren == null || pChildren.count == 0)
            {
                return pSprite.AtlasIndex;
            }
            else
            {
                return LowestAtlasIndexInChild((CCSprite) pChildren.Elements[0]);
            }
        }

        public int AtlasIndexForChild(CCSprite pobSprite, int nZ)
        {
            CCRawList<CCNode> pBrothers = pobSprite.Parent.Children;

            int uChildIndex = pBrothers.IndexOf(pobSprite);

            // ignore parent Z if parent is spriteSheet
            bool bIgnoreParent = (pobSprite.Parent == this);

            CCSprite pPrevious = null;

            if (uChildIndex > 0)
            {
                pPrevious = (CCSprite) pBrothers[uChildIndex - 1];
            }

            // first child of the sprite sheet
            if (bIgnoreParent)
            {
                if (uChildIndex == 0)
                {
                    return 0;
                }

                return HighestAtlasIndexInChild(pPrevious) + 1;
            }

            // parent is a CCSprite, so, it must be taken into account

            // first child of an CCSprite ?
            if (uChildIndex == 0)
            {
                var p = (CCSprite) pobSprite.Parent;

                // less than parent and brothers
                if (nZ < 0)
                {
                    return p.AtlasIndex;
                }
                else
                {
                    return p.AtlasIndex + 1;
                }
            }
            else
            {
                // previous & sprite belong to the same branch
                if ((pPrevious.ZOrder < 0 && nZ < 0) || (pPrevious.ZOrder >= 0 && nZ >= 0))
                {
                    return HighestAtlasIndexInChild(pPrevious) + 1;
                }

                // else (previous < 0 and sprite >= 0 )
                var p = (CCSprite) pobSprite.Parent;
                return p.AtlasIndex + 1;
            }
        }

        public void InsertChild(CCSprite pobSprite, int uIndex)
        {
            pobSprite.BatchNode = this;
            pobSprite.AtlasIndex = uIndex;
            pobSprite.Dirty = true;

            if (m_pobTextureAtlas.TotalQuads == m_pobTextureAtlas.Capacity)
            {
                IncreaseAtlasCapacity();
            }

            m_pobTextureAtlas.InsertQuad(ref pobSprite.m_sQuad, uIndex);

            m_pobDescendants.Insert(uIndex, pobSprite);

            // update indices
            CCSprite[] delements = m_pobDescendants.Elements;
            for (int i = uIndex + 1, count = m_pobDescendants.count; i < count; i++)
            {
                delements[i].AtlasIndex++;
            }

            // add children recursively
            CCRawList<CCNode> pChildren = pobSprite.Children;

            if (pChildren != null && pChildren.count > 0)
            {
                CCNode[] elements = pChildren.Elements;
                for (int j = 0, count = pChildren.count; j < count; j++)
                {
                    var pChild = (CCSprite) elements[j];
                    uIndex = AtlasIndexForChild(pChild, pChild.ZOrder);
                    InsertChild(pChild, uIndex);
                }
            }
        }

        // addChild helper, faster than insertChild
        public void AppendChild(CCSprite sprite)
        {
            m_bReorderChildDirty = true;
            sprite.BatchNode = this;
            sprite.Dirty = true;

            if (m_pobTextureAtlas.TotalQuads == m_pobTextureAtlas.Capacity)
            {
                IncreaseAtlasCapacity();
            }

            m_pobDescendants.Add(sprite);

            int index = m_pobDescendants.Count - 1;

            sprite.AtlasIndex = index;

            m_pobTextureAtlas.InsertQuad(ref sprite.m_sQuad, index);

            // add children recursively
            CCRawList<CCNode> children = sprite.Children;
            if (children != null && children.count > 0)
            {
                CCNode[] elements = children.Elements;
                int count = children.count;
                for (int i = 0; i < count; i++)
                {
                    AppendChild((CCSprite) elements[i]);
                }
            }
        }

        public void RemoveSpriteFromAtlas(CCSprite pobSprite)
        {
            // remove from TextureAtlas
            m_pobTextureAtlas.RemoveQuadAtIndex(pobSprite.AtlasIndex);

            // Cleanup sprite. It might be reused (issue #569)
            pobSprite.BatchNode = null;

            int uIndex = m_pobDescendants.IndexOf(pobSprite);

            if (uIndex >= 0)
            {
                m_pobDescendants.RemoveAt(uIndex);

                // update all sprites beyond this one
                int count = m_pobDescendants.count;
                CCSprite[] elements = m_pobDescendants.Elements;

                for (; uIndex < count; ++uIndex)
                {
                    elements[uIndex].AtlasIndex--;
                }
            }

            // remove children recursively
            CCRawList<CCNode> pChildren = pobSprite.Children;

            if (pChildren != null && pChildren.count > 0)
            {
                CCNode[] elements = pChildren.Elements;
                for (int i = 0, count = pChildren.count; i < count; i++)
                {
                    RemoveSpriteFromAtlas((CCSprite) elements[i]);
                }
            }
        }

        private void UpdateBlendFunc()
        {
            if (!m_pobTextureAtlas.Texture.HasPremultipliedAlpha)
            {
                m_blendFunc = CCBlendFunc.NonPremultiplied;
            }
        }

        //CCSpriteSheet Extension
        //implementation CCSpriteSheet (TMXTiledMapExtension)
        protected void InsertQuadFromSprite(CCSprite sprite, int index)
        {
            Debug.Assert(sprite != null, "Argument must be non-NULL");

            while (index >= m_pobTextureAtlas.Capacity || m_pobTextureAtlas.Capacity == m_pobTextureAtlas.TotalQuads)
            {
                IncreaseAtlasCapacity();
            }
            //
            // update the quad directly. Don't add the sprite to the scene graph
            //
            sprite.BatchNode = this;
            sprite.AtlasIndex = index;

            m_pobTextureAtlas.InsertQuad(ref sprite.m_sQuad, index);

            // XXX: updateTransform will update the textureAtlas too using updateQuad.
            // XXX: so, it should be AFTER the insertQuad
            sprite.Dirty = true;
            sprite.UpdateTransform();
        }

        protected void UpdateQuadFromSprite(CCSprite sprite, int index)
        {
            Debug.Assert(sprite != null, "Argument must be non-NULL");

            while (index >= m_pobTextureAtlas.Capacity || m_pobTextureAtlas.Capacity == m_pobTextureAtlas.TotalQuads)
            {
                IncreaseAtlasCapacity();
            }
            //
            // update the quad directly. Don't add the sprite to the scene graph
            //
            sprite.BatchNode = this;
            sprite.AtlasIndex = index;

            sprite.Dirty = true;

            // UpdateTransform updates the textureAtlas quad
            sprite.UpdateTransform();
        }

        protected CCSpriteBatchNode AddSpriteWithoutQuad(CCSprite child, int z, int aTag)
        {
            Debug.Assert(child != null, "Argument must be non-NULL");

            // quad index is Z
            child.AtlasIndex = z;

            // XXX: optimize with a binary search
            int i = 0;

            if (m_pobDescendants.count > 0)
            {
                CCSprite[] elements = m_pobDescendants.Elements;
                for (int j = 0, count = m_pobDescendants.count; j < count; j++)
                {
                    if (elements[i].AtlasIndex >= z)
                    {
                        ++i;
                    }
                }
            }

            m_pobDescendants.Insert(i, child);

            // I  MPORTANT: Call super, and not self. Avoid adding it to the texture atlas array
            base.AddChild(child, z, aTag);
            //#issue 1262 don't use lazy sorting, tiles are added as quads not as sprites, so sprites need to be added in order
            ReorderBatch(false);

            return this;
        }
    }
}