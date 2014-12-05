using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public class CCSpriteBatchNode : CCNode, ICCTexture
    {
        const int defaultSpriteBatchCapacity = 29;


        #region Properties

        public CCTextureAtlas TextureAtlas { get ; private set; }
        public CCRawList<CCSprite> Descendants { get; private set; }
        public CCBlendFunc BlendFunc { get; set; }

        public bool IsAntialiased
        {
            get { return Texture.IsAntialiased; }
            set { Texture.IsAntialiased = value; }
        }

        public virtual CCTexture2D Texture
        {
            get { return TextureAtlas.Texture; }
            set
            {
                TextureAtlas.Texture = value;
                UpdateBlendFunc();
            }
        }

//        // Size of batch node in world space makes no sense
//        public override CCSize ContentSize
//        {
//            get { return CCSize.Zero; }
//            set
//            {
//            }
//        }
//
//        public override CCAffineTransform AffineLocalTransform
//        {
//            get { return CCAffineTransform.Identity; }
//        }
//
//        protected internal override Matrix XnaLocalMatrix 
//        { 
//            get { return Matrix.Identity; }
//            protected set 
//            {
//            }
//        }
//
        #endregion Properties


        #region Constructors

        // We need this constructor for all the subclasses that initialise by directly calling InitCCSpriteBatchNode
        public CCSpriteBatchNode()
        {
        }

        public CCSpriteBatchNode(CCTexture2D tex, int capacity=defaultSpriteBatchCapacity)
        {
            InitCCSpriteBatchNode(tex, capacity);
        }

        public CCSpriteBatchNode(string fileImage, int capacity=defaultSpriteBatchCapacity) 
            : this(CCTextureCache.SharedTextureCache.AddImage(fileImage), capacity)
        {
        }

        protected void InitCCSpriteBatchNode(CCTexture2D tex, int capacity=defaultSpriteBatchCapacity)
        {
            BlendFunc = CCBlendFunc.AlphaBlend;

            if (capacity == 0)
            {
                capacity = defaultSpriteBatchCapacity;
            }

            TextureAtlas = new CCTextureAtlas(tex, capacity);

            UpdateBlendFunc();

            // no lazy alloc in this node
            Children = new CCRawList<CCNode>(capacity);
            Descendants = new CCRawList<CCSprite>(capacity);
        }

        #endregion Constructors

        protected override void AddedToScene ()
        {
            base.AddedToScene ();

            if(ContentSize == CCSize.Zero)
                ContentSize = Layer.VisibleBoundsWorldspace.Size;
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
            if (!Visible)
            {
                return;
            }

            Window.DrawManager.PushMatrix();

            if (Grid != null && Grid.Active)
            {
                Grid.BeforeDraw();
                TransformAncestors();
            }

            SortAllChildren();
            CCDrawManager.SharedDrawManager.SetIdentityMatrix();

            Draw();

            if (Grid != null && Grid.Active)
            {
                Grid.AfterDraw(this);
            }

            Window.DrawManager.PopMatrix();
        }

        protected override void Draw()
        {
            // Optimization: Fast Dispatch  
            if (TextureAtlas.TotalQuads == 0)
            {
                return;
            }

            Window.DrawManager.BlendFunc(BlendFunc);

            TextureAtlas.DrawQuads();
        }


        #region Child management

        public override void AddChild(CCNode child, int zOrder = 0, int tag = CCNode.TagInvalid)
        {
            Debug.Assert(child != null, "child should not be null");

            Debug.Assert(child is CCSprite, "CCSpriteBatchNode only supports CCSprites as children");

            var pSprite = (CCSprite) child;

            // check CCSprite is using the same texture id
            Debug.Assert(pSprite.Texture.Name == TextureAtlas.Texture.Name, "CCSprite is not using the same texture id");

            base.AddChild(child, zOrder, tag);

            AppendChild(pSprite);
        }

        public override void ReorderChild(CCNode child, int zOrder)
        {
            Debug.Assert(child != null, "the child should not be null");
            Debug.Assert(Children.Contains(child), "Child doesn't belong to Sprite");

            if (zOrder == child.ZOrder)
            {
                return;
            }

            //set the z-order and sort later
            base.ReorderChild(child, zOrder);
        }

        public override void RemoveChild(CCNode child, bool cleanup)
        {
            var pSprite = (CCSprite) child;

            Debug.Assert(Children.Contains(pSprite), "sprite batch node should contain the child");

            // cleanup before removing
            RemoveSpriteFromAtlas(pSprite);

            base.RemoveChild(pSprite, cleanup);
        }

        public void RemoveChildAtIndex(int index, bool doCleanup)
        {
            RemoveChild((Children[index]), doCleanup);
        }

        public override void RemoveAllChildren(bool cleanup)
        {
            // Invalidate atlas index. issue #569
            // useSelfRender should be performed on all descendants. issue #1216
            CCSprite[] elements = Descendants.Elements;
            for (int i = 0, count = Descendants.Count; i < count; i++)
            {
                elements[i].BatchNode = null;
            }

            base.RemoveAllChildren(cleanup);

            Descendants.Clear();
            TextureAtlas.RemoveAllQuads();
        }

        public override void SortAllChildren()
        {
            if (IsReorderChildDirty)
            {
                int count = Children.Count;
                CCNode[] elements = Children.Elements;

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

                IsReorderChildDirty = false;
            }
        }

        public void InsertChild(CCSprite sprite, int uIndex)
        {
            if (TextureAtlas.TotalQuads == TextureAtlas.Capacity)
            {
                IncreaseAtlasCapacity();
            }

            TextureAtlas.InsertQuad(ref sprite.transformedQuad, uIndex);
            sprite.BatchNode = this;
            sprite.AtlasIndex = uIndex;

            Descendants.Insert(uIndex, sprite);

            // update indices
            CCSprite[] delements = Descendants.Elements;
            for (int i = uIndex + 1, count = Descendants.Count; i < count; i++)
            {
                delements[i].AtlasIndex++;
            }

            // add children recursively
            CCRawList<CCNode> children = sprite.Children;

            if (children != null && children.Count > 0)
            {
                CCNode[] elements = children.Elements;
                for (int j = 0, count = children.Count; j < count; j++)
                {
                    var child = (CCSprite) elements[j];
                    uIndex = AtlasIndexForChild(child, child.ZOrder);
                    InsertChild(child, uIndex);
                }
            }
        }

        // addChild helper, faster than insertChild
        public void AppendChild(CCSprite sprite)
        {
            IsReorderChildDirty = true;

            if (TextureAtlas.TotalQuads == TextureAtlas.Capacity)
            {
                IncreaseAtlasCapacity();
            }

            Descendants.Add(sprite);

            int index = Descendants.Count - 1;

            sprite.AtlasIndex = index;
            sprite.BatchNode = this;

            TextureAtlas.UpdateQuad(ref sprite.transformedQuad, index);

            // add children recursively
            CCRawList<CCNode> children = sprite.Children;
            if (children != null && children.Count > 0)
            {
                CCNode[] elements = children.Elements;
                int count = children.Count;
                for (int i = 0; i < count; i++)
                {
                    AppendChild((CCSprite) elements[i]);
                }
            }
        }

        #endregion Child management


        void UpdateAtlasIndex(CCSprite sprite, ref int curIndex)
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
                sprite.OrderOfArrival = 0;
                if (oldIndex != curIndex)
                {
                    Swap(oldIndex, curIndex);
                }
                curIndex++;
            }
            else
            {
                bool needNewIndex = true;

                if (pArray.Elements[0].ZOrder >= 0)
                {
                    //all children are in front of the parent
                    oldIndex = sprite.AtlasIndex;
                    sprite.AtlasIndex = curIndex;
                    sprite.OrderOfArrival = 0;
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
                    if (needNewIndex && child.ZOrder >= 0)
                    {
                        oldIndex = sprite.AtlasIndex;
                        sprite.AtlasIndex = curIndex;
                        sprite.OrderOfArrival = 0;
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
                    sprite.OrderOfArrival = 0;
                    if (oldIndex != curIndex)
                    {
                        Swap(oldIndex, curIndex);
                    }
                    curIndex++;
                }
            }
        }

        void Swap(int oldIndex, int newIndex)
        {
            CCSprite[] sprites = Descendants.Elements;
            CCRawList<CCV3F_C4B_T2F_Quad> quads = TextureAtlas.Quads;

            TextureAtlas.Dirty = true;

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
            IsReorderChildDirty = reorder;
        }

        public void IncreaseAtlasCapacity()
        {
            // if we're going beyond the current TextureAtlas's capacity,
            // all the previously initialized sprites will need to redo their texture coords
            // this is likely computationally expensive
            int quantity = (TextureAtlas.Capacity + 1) * 4 / 3;

            CCLog.Log(string.Format(
                "CocosSharp: CCSpriteBatchNode: resizing TextureAtlas capacity from [{0}] to [{1}].",
                TextureAtlas.Capacity, quantity));

            TextureAtlas.ResizeCapacity(quantity);
        }

        public int RebuildIndexInOrder(CCSprite pobParent, int uIndex)
        {
            CCRawList<CCNode> pChildren = pobParent.Children;

            if (pChildren != null && pChildren.Count > 0)
            {
                CCNode[] elements = pChildren.Elements;
                for (int i = 0, count = pChildren.Count; i < count; i++)
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

            if (pChildren != null && pChildren.Count > 0)
            {
                CCNode[] elements = pChildren.Elements;
                for (int i = 0, count = pChildren.Count; i < count; i++)
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

            if (pChildren == null || pChildren.Count == 0)
            {
                return pSprite.AtlasIndex;
            }
            else
            {
                return HighestAtlasIndexInChild((CCSprite) pChildren.Elements[pChildren.Count - 1]);
            }
        }

        public int LowestAtlasIndexInChild(CCSprite pSprite)
        {
            CCRawList<CCNode> pChildren = pSprite.Children;

            if (pChildren == null || pChildren.Count == 0)
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

        public void RemoveSpriteFromAtlas(CCSprite sprite)
        {
            // remove from TextureAtlas
            TextureAtlas.RemoveQuadAtIndex(sprite.AtlasIndex);

            // Cleanup sprite. It might be reused (issue #569)
            sprite.BatchNode = null;

            int uIndex = Descendants.IndexOf(sprite);

            if (uIndex >= 0)
            {
                Descendants.RemoveAt(uIndex);

                // update all sprites beyond this one
                int count = Descendants.Count;
                CCSprite[] elements = Descendants.Elements;

                for (; uIndex < count; ++uIndex)
                {
                    elements[uIndex].AtlasIndex--;
                }
            }

            // remove children recursively
            CCRawList<CCNode> pChildren = sprite.Children;

            if (pChildren != null && pChildren.Count > 0)
            {
                CCNode[] elements = pChildren.Elements;
                for (int i = 0, count = pChildren.Count; i < count; i++)
                {
                    RemoveSpriteFromAtlas((CCSprite) elements[i]);
                }
            }
        }

        void UpdateBlendFunc()
        {
            if (!TextureAtlas.Texture.HasPremultipliedAlpha)
            {
                BlendFunc = CCBlendFunc.NonPremultiplied;
            }
        }

        //CCSpriteSheet Extension
        //implementation CCSpriteSheet (TMXTiledMapExtension)
        protected void InsertQuadFromSprite(CCSprite sprite, int index)
        {
            Debug.Assert(sprite != null, "Argument must be non-NULL");

            while (index >= TextureAtlas.Capacity || TextureAtlas.Capacity == TextureAtlas.TotalQuads)
            {
                IncreaseAtlasCapacity();
            }
            //
            // update the quad directly. Don't add the sprite to the scene graph
            //
            sprite.BatchNode = this;
            sprite.AtlasIndex = index;

            TextureAtlas.InsertQuad(ref sprite.transformedQuad, index);

            // XXX: updateTransform will update the textureAtlas too using updateQuad.
            // XXX: so, it should be AFTER the insertQuad
            sprite.UpdateTransformedSpriteTextureQuads();
        }

        protected void UpdateQuadFromSprite(CCSprite sprite, int index)
        {
            Debug.Assert(sprite != null, "Argument must be non-NULL");

            while (index >= TextureAtlas.Capacity || TextureAtlas.Capacity == TextureAtlas.TotalQuads)
            {
                IncreaseAtlasCapacity();
            }
            //
            // update the quad directly. Don't add the sprite to the scene graph
            //
            sprite.BatchNode = this;
            sprite.AtlasIndex = index;

            // UpdateTransform updates the textureAtlas quad
            sprite.UpdateTransformedSpriteTextureQuads();
        }

        protected CCSpriteBatchNode AddSpriteWithoutQuad(CCSprite child, int z, int aTag)
        {
            Debug.Assert(child != null, "Argument must be non-NULL");

            // quad index is Z
            child.AtlasIndex = z;

            // XXX: optimize with a binary search
            int i = 0;

            if (Descendants.Count > 0)
            {
                CCSprite[] elements = Descendants.Elements;
                for (int j = 0, count = Descendants.Count; j < count; j++)
                {
                    if (elements[i].AtlasIndex >= z)
                    {
                        ++i;
                    }
                }
            }

            Descendants.Insert(i, child);

            base.AddChild(child, z, aTag);

            //#issue 1262 don't use lazy sorting, tiles are added as quads not as sprites, so sprites need to be added in order
            ReorderBatch(false);

            return this;
        }
    }
}