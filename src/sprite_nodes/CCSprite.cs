using System;
using System.IO;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCSprite : CCNode, ICCTexture
    {
        bool flipX;
        bool flipY;
        bool isTextureRectRotated;
        bool opacityModifyRGB;

        CCPoint unflippedOffsetPositionFromCenter;
        CCSize untrimmedSizeInPixels;

        CCRect textureRectInPixels;

        CCSpriteBatchNode batchNode;    // Used batch node (weak reference)
        CCTextureAtlas textureAtlas;    // Sprite Sheet texture atlas (weak reference)
        CCTexture2D texture;
        string textureFile;

        protected CCV3F_C4B_T2F_Quad quad;
        internal CCV3F_C4B_T2F_Quad transformedQuad;

        #region Properties

        public int AtlasIndex { get ; internal set; }
        public CCBlendFunc BlendFunc { get; set; }

        protected internal CCV3F_C4B_T2F_Quad Quad { get { return quad; } }
        protected internal CCV3F_C4B_T2F_Quad TransformedQuad { get { return transformedQuad; } } 

        public bool IsAntialiased
        {
            get { return Texture.IsAntialiased; }
            set { Texture.IsAntialiased = value; }
        }

        public override bool IsColorModifiedByOpacity
        {
            get { return opacityModifyRGB; }
            set
            {
                if (opacityModifyRGB != value)
                {
                    opacityModifyRGB = value;
                    UpdateColor();
                }
            }
        }

        public bool IsTextureRectRotated 
        { 
            get { return isTextureRectRotated; }
            set 
            {
                if (isTextureRectRotated != value) 
                {
                    isTextureRectRotated = value;
                    UpdateSpriteTextureQuads();
                }
            }
        }

        public bool FlipX
        {
            get { return flipX; }
            set
            {
                if (flipX != value)
                {
                    flipX = value;
                    UpdateSpriteTextureQuads();
                }
            }
        }

        public bool FlipY
        {
            get { return flipY; }
            set
            {
                if (flipY != value)
                {
                    flipY = value;
                    UpdateSpriteTextureQuads();
                }
            }
        }

        public override byte Opacity
        {
            get { return base.Opacity; }
            set
            {
                base.Opacity = value;
                UpdateColor();
            }
        }

        public override float Rotation
        {
            set
            {
                base.Rotation = value;
                UpdateSpriteTextureQuads();
            }
        }

        // Rotation of the sprite, about the X axis, in Degrees.
        public override float RotationX
        {
            get { return base.RotationX; }
            set
            {
                base.RotationX = value;
                UpdateSpriteTextureQuads();
            }
        }

        // Rotation of the sprite, about the Y axis, in Degrees.
        public override float RotationY
        {
            get { return base.RotationY; }
            set
            {
                base.RotationY = value;
                UpdateSpriteTextureQuads();
            }
        }

        public override float SkewX
        {
            get { return base.SkewX; }
            set
            {
                base.SkewX = value;
                UpdateSpriteTextureQuads();
            }
        }

        public override float SkewY
        {
            get { return base.SkewY; }
            set
            {
                base.SkewY = value;
                UpdateSpriteTextureQuads();
            }
        }

        public virtual void ScaleTo(CCSize size)
        {
            CCSize content = ContentSize;
            float sx = size.Width / content.Width;
            float sy = size.Height / content.Height;
            base.ScaleX = sx;
            base.ScaleY = sy;
            UpdateSpriteTextureQuads();
        }

        public override float ScaleX
        {
            get { return base.ScaleX; }
            set
            {
                base.ScaleX = value;
                UpdateSpriteTextureQuads();
            }
        }

        public override float ScaleY
        {
            get { return base.ScaleY; }
            set
            {
                base.ScaleY = value;
                UpdateSpriteTextureQuads();
            }
        }

        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
                UpdateSpriteTextureQuads();
            }
        }

        public override CCColor3B Color
        {
            get { return base.Color; }
            set
            {
                base.Color = value;
                UpdateColor();
            }
        }

        public override CCSize ContentSize
        {
            get { return base.ContentSize; }
            set
            {
                base.ContentSize = value;

//                if(untrimmedSizeInPixels == CCSize.Zero)
                    untrimmedSizeInPixels = value;

                UpdateSpriteTextureQuads();
            }
        }

        public override CCPoint Position
        {
            get { return base.Position; }
            set
            {
                base.Position = value;
                UpdateSpriteTextureQuads();
            }
        }

        public CCRect TextureRectInPixels
        {
            get { return textureRectInPixels; }
            set 
            { 
                if(textureRectInPixels != value) 
                {
                    textureRectInPixels = value;
                    UpdateSpriteTextureQuads();
                }
            }
        }

        public CCSpriteFrame SpriteFrame
        {
            get
            {
                return new CCSpriteFrame(                    
                    ContentSize,
                    texture,
                    textureRectInPixels,
                    untrimmedSizeInPixels,
                    IsTextureRectRotated,
                    unflippedOffsetPositionFromCenter
                );
            }
            set
            {
                if (value != null) 
                {
                    CCTexture2D newTexture = value.Texture;

                    // update texture before updating texture rect
                    if (newTexture != texture) 
                    {
                        Texture = newTexture;
                    }

                    // update rect
                    IsTextureRectRotated = value.IsRotated;
                    textureRectInPixels = value.TextureRectInPixels;
                    unflippedOffsetPositionFromCenter = value.OffsetInPixels;
                    untrimmedSizeInPixels = value.OriginalSizeInPixels;

                    ContentSize = untrimmedSizeInPixels;

                    UpdateSpriteTextureQuads();
                }
            }
        }

        public CCSpriteBatchNode BatchNode
        {
            get { return batchNode; }
            set
            {
                if (value != batchNode) 
                {

                    batchNode = value;

                    if (value == null) 
                    {
                        AtlasIndex = CCMacros.CCSpriteIndexNotInitialized;
                        textureAtlas = null;

                        float x1 = 0.0f;
                        float y1 = 0.0f;
                        float x2 = x1 + ContentSize.Width;
                        float y2 = y1 + ContentSize.Height;

                        quad.BottomLeft.Vertices = new CCVertex3F (x1, y1, 0);
                        quad.BottomRight.Vertices = new CCVertex3F (x2, y1, 0);
                        quad.TopLeft.Vertices = new CCVertex3F (x1, y2, 0);
                        quad.TopRight.Vertices = new CCVertex3F (x2, y2, 0);
                    } 
                    else 
                    {
                        textureAtlas = batchNode.TextureAtlas;

                        if (Layer != null && batchNode.Layer != Layer) {
                            batchNode.Layer = Layer;
                        }
                    }

                    UpdateSpriteTextureQuads();
                }
            }
        }

        public virtual CCTexture2D Texture
        {
            get { return texture; }
            set
            {
                // If batchnode, then texture id should be the same
                Debug.Assert(batchNode == null || value.Name == batchNode.Texture.Name,
                    "CCSprite: Batched sprites should use the same texture as the batchnode");

                if (batchNode == null && texture != value)
                {
                    texture = value;

                    if(TextureRectInPixels == CCRect.Zero) 
                    {
                        CCSize texSize = texture.ContentSizeInPixels;
                        TextureRectInPixels = new CCRect(0.0f, 0.0f, texSize.Width, texSize.Height);
                    }

                    if(ContentSize == CCSize.Zero)
                        ContentSize = textureRectInPixels.Size;

                    UpdateBlendFunc();
                }
            }
        }

        #endregion Properties


        #region Constructors

        public CCSprite()
        {
        }

        public CCSprite(CCTexture2D texture=null, CCRect? texRectInPixels=null, bool rotated=false)
        {
            InitWithTexture(texture, texRectInPixels, rotated);
        }

        public CCSprite(CCSpriteFrame spriteFrame) : this(spriteFrame.ContentSize, spriteFrame)
        {
        }

        public CCSprite(CCSize contentSize, CCSpriteFrame spriteFrame)
        {
            ContentSize = contentSize;
            InitWithSpriteFrame(spriteFrame);
        }

        public CCSprite(string fileName, CCRect? texRectInPixels=null)
        {
            InitWithFile(fileName, texRectInPixels);
        }

        // Used externally by non-subclasses
        internal void InitWithTexture(CCTexture2D texture, CCRect? texRectInPixels=null, bool rotated=false)
        {
            // do not remove this
            // This sets up the atlas index correctly.  If not set correctly lot of weird sprite artifacts start showing up.
            BatchNode = null;

            IsTextureRectRotated = rotated;
            CCSize texSize = texture.ContentSizeInPixels;
            textureRectInPixels = texRectInPixels ?? new CCRect(0.0f, 0.0f, texSize.Width, texSize.Height);

            opacityModifyRGB = true;
            BlendFunc = CCBlendFunc.AlphaBlend;

            AnchorPoint = new CCPoint(0.5f, 0.5f);

            quad = new CCV3F_C4B_T2F_Quad();
            quad.BottomLeft.Colors = CCColor4B.White;
            quad.BottomRight.Colors = CCColor4B.White;
            quad.TopLeft.Colors = CCColor4B.White;
            quad.TopRight.Colors = CCColor4B.White;

            Texture = texture;

            // If content size not initialized, assume worldspace dimensions match texture dimensions
            if(ContentSize == CCSize.Zero)
                ContentSize = textureRectInPixels.Size;


            UpdateSpriteTextureQuads();
        }

        void InitWithSpriteFrame(CCSpriteFrame spriteFrame)
        {
            opacityModifyRGB = true;
            BlendFunc = CCBlendFunc.AlphaBlend;

            AnchorPoint = new CCPoint(0.5f, 0.5f);

            quad = new CCV3F_C4B_T2F_Quad();
            quad.BottomLeft.Colors = CCColor4B.White;
            quad.BottomRight.Colors = CCColor4B.White;
            quad.TopLeft.Colors = CCColor4B.White;
            quad.TopRight.Colors = CCColor4B.White;

            SpriteFrame = spriteFrame;
        }

        void InitWithFile(string fileName, CCRect? rectInPoints=null)
        {
            Debug.Assert(!String.IsNullOrEmpty(fileName), "Invalid filename for sprite");

            textureFile = fileName;

            // Try sprite frame cache first
            CCSpriteFrame frame = CCSpriteFrameCache.SharedSpriteFrameCache[fileName];
            if (frame != null) 
            {
                InitWithSpriteFrame(frame);
            } 
            else 
            {
                // If frame doesn't exist, try texture cache
                CCTexture2D texture = CCTextureCache.SharedTextureCache.AddImage(fileName);
                if (texture != null) 
                {
                    InitWithTexture(texture, rectInPoints);
                }
            }
        }

        #endregion Constructors


        protected override void Draw()
        {
            base.Draw();

            Debug.Assert(batchNode == null);

            CCDrawManager drawManager = Window.DrawManager;

            drawManager.BlendFunc(BlendFunc);
            drawManager.BindTexture(Texture);
            drawManager.DrawQuad(ref quad);
        }

        public bool IsSpriteFrameDisplayed(CCSpriteFrame frame)
        {
            CCRect r = frame.TextureRectInPixels;

            return (
                CCRect.Equal(ref r, ref textureRectInPixels) &&
                frame.Texture.Name == texture.Name
            );
        }

        public void SetSpriteFrameWithAnimationName(string animationName, int frameIndex)
        {
            Debug.Assert(!String.IsNullOrEmpty(animationName),
                "CCSprite#setDisplayFrameWithAnimationName. animationName must not be NULL");

            CCAnimation a = CCAnimationCache.SharedAnimationCache[animationName];

            Debug.Assert(a != null, "CCSprite#setDisplayFrameWithAnimationName: Frame not found");

            var frame = (CCAnimationFrame)a.Frames[frameIndex];

            Debug.Assert(frame != null, "CCSprite#setDisplayFrame. Invalid frame");

            SpriteFrame = frame.SpriteFrame;
        }


        #region Serialization

        public override void Serialize(System.IO.Stream stream)
        {
            base.Serialize(stream);
            StreamWriter sw = new StreamWriter(stream);
            CCSerialization.SerializeData(IsTextureRectRotated, sw);
            CCSerialization.SerializeData(AtlasIndex, sw);
            CCSerialization.SerializeData(TextureRectInPixels, sw);
            sw.WriteLine(textureFile == null ? "null" : textureFile);
        }

        public override void Deserialize(System.IO.Stream stream)
        {
            base.Deserialize(stream);
            StreamReader sr = new StreamReader(stream);
            textureFile = sr.ReadLine();
            if (textureFile == "null")
                textureFile = null;
            else {
                CCLog.Log("CCSprite - deserialized with texture file " + textureFile);
                InitWithFile(textureFile);
            }

            IsTextureRectRotated = CCSerialization.DeSerializeBool(sr);
            AtlasIndex = CCSerialization.DeSerializeInt(sr);
            TextureRectInPixels = CCSerialization.DeSerializeRect(sr);
        }

        #endregion Serialization


        #region Color managment


		public override void UpdateColor()
        {
            var color4 = new CCColor4B(DisplayedColor.R, DisplayedColor.G, DisplayedColor.B, DisplayedOpacity);
			//opacityModifyRGB = true;
            if (opacityModifyRGB)
            {
                color4.R = (byte)(color4.R * DisplayedOpacity / 255.0f);
                color4.G = (byte)(color4.G * DisplayedOpacity / 255.0f);
                color4.B = (byte)(color4.B * DisplayedOpacity / 255.0f);
            }

            quad.BottomLeft.Colors = color4;
            quad.BottomRight.Colors = color4;
            quad.TopLeft.Colors = color4;
            quad.TopRight.Colors = color4;

            UpdateTransformedSpriteTextureQuads();
        }

        protected void UpdateBlendFunc()
        {
            Debug.Assert(batchNode == null,
                "CCSprite: updateBlendFunc doesn't work when the sprite is rendered using a CCSpriteSheet");

            // it's possible to have an untextured sprite
            if (texture == null || !texture.HasPremultipliedAlpha)
            {
                BlendFunc = CCBlendFunc.NonPremultiplied;
                IsColorModifiedByOpacity = false;
            }
            else
            {
                BlendFunc = CCBlendFunc.AlphaBlend;
                IsColorModifiedByOpacity = true;
            }
        }

        #endregion Color managment


        #region Updating quads

        void UpdateSpriteTextureQuads()
        {

            if (!Visible)
            {
                quad.BottomRight.Vertices =
                    quad.TopLeft.Vertices = quad.TopRight.Vertices = quad.BottomLeft.Vertices = new CCVertex3F(0, 0, 0);
            }
            else
            {
                CCPoint relativeOffset = unflippedOffsetPositionFromCenter;

                if (flipX)
                {
                    relativeOffset.X = -relativeOffset.X;
                }
                if (flipY)
                {
                    relativeOffset.Y = -relativeOffset.Y;
                }

                CCPoint centerPoint = untrimmedSizeInPixels.Center + relativeOffset;
                CCPoint subRectOrigin;
                subRectOrigin.X = centerPoint.X - textureRectInPixels.Size.Width / 2.0f;
                subRectOrigin.Y = centerPoint.Y - textureRectInPixels.Size.Height / 2.0f;

                CCRect subRectRatio = CCRect.Zero;

                if (untrimmedSizeInPixels.Width > 0 && untrimmedSizeInPixels.Height > 0)
                {
                    subRectRatio = new CCRect(
                        subRectOrigin.X / untrimmedSizeInPixels.Width, 
                        subRectOrigin.Y / untrimmedSizeInPixels.Height,
                        textureRectInPixels.Size.Width / untrimmedSizeInPixels.Width,
                        textureRectInPixels.Size.Height / untrimmedSizeInPixels.Height);
                }

                // Atlas: Vertex
                float x1 = subRectRatio.Origin.X * ContentSize.Width;
                float y1 = subRectRatio.Origin.Y * ContentSize.Height;
                float x2 = x1 + (subRectRatio.Size.Width * ContentSize.Width);
                float y2 = y1 + (subRectRatio.Size.Height * ContentSize.Height);

                // Don't update Z.
                quad.BottomLeft.Vertices = new CCVertex3F(x1, y1, 0);
                quad.BottomRight.Vertices = new CCVertex3F(x2, y1, 0);
                quad.TopLeft.Vertices = new CCVertex3F(x1, y2, 0);
                quad.TopRight.Vertices = new CCVertex3F(x2, y2, 0);

                CCTexture2D tex = batchNode != null ? textureAtlas.Texture : texture;
                if (tex == null)
                {
                    return;
                }

                float atlasWidth = tex.PixelsWide;
                float atlasHeight = tex.PixelsHigh;

                float left, right, top, bottom;

                if (IsTextureRectRotated)
                {
                    #if CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL
                    left = (2 * textureRectInPixels.Origin.X + 1) / (2 * atlasWidth);
                    right = left + (textureRectInPixels.Size.Height * 2 - 2) / (2 * atlasWidth);
                    top = (2 * textureRectInPixels.Origin.Y + 1) / (2 * atlasHeight);
                    bottom = top + (textureRectInPixels.Size.Width * 2 - 2) / (2 * atlasHeight);
                    #else
                    left = textureRectInPixels.Origin.X / atlasWidth;
                    right = (textureRectInPixels.Origin.X + textureRectInPixels.Size.Height) / atlasWidth;
                    top = textureRectInPixels.Origin.Y / atlasHeight;
                    bottom = (textureRectInPixels.Origin.Y + textureRectInPixels.Size.Width) / atlasHeight;
                    #endif

                    if (flipX)
                    {
                        CCMacros.CCSwap(ref top, ref bottom);
                    }

                    if (flipY)
                    {
                        CCMacros.CCSwap(ref left, ref right);
                    }

                    quad.BottomLeft.TexCoords.U = left;
                    quad.BottomLeft.TexCoords.V = top;
                    quad.BottomRight.TexCoords.U = left;
                    quad.BottomRight.TexCoords.V = bottom;
                    quad.TopLeft.TexCoords.U = right;
                    quad.TopLeft.TexCoords.V = top;
                    quad.TopRight.TexCoords.U = right;
                    quad.TopRight.TexCoords.V = bottom;
                }
                else
                {
                    #if CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL
                    left = (2 * textureRectInPixels.Origin.X + 1) / (2 * atlasWidth);
                    right = left + (textureRectInPixels.Size.Width * 2 - 2) / (2 * atlasWidth);
                    top = (2 * textureRectInPixels.Origin.Y + 1) / (2 * atlasHeight);
                    bottom = top + (textureRectInPixels.Size.Height * 2 - 2) / (2 * atlasHeight);
                    #else
                    left = textureRectInPixels.Origin.X / atlasWidth;
                    right = (textureRectInPixels.Origin.X + textureRectInPixels.Size.Width) / atlasWidth;
                    top = textureRectInPixels.Origin.Y / atlasHeight;
                    bottom = (textureRectInPixels.Origin.Y + textureRectInPixels.Size.Height) / atlasHeight;
                    #endif

                    if (flipX)
                    {
                        CCMacros.CCSwap(ref left, ref right);
                    }

                    if (flipY)
                    {
                        CCMacros.CCSwap(ref top, ref bottom);
                    }

                    quad.BottomLeft.TexCoords.U = left;
                    quad.BottomLeft.TexCoords.V = bottom;
                    quad.BottomRight.TexCoords.U = right;
                    quad.BottomRight.TexCoords.V = bottom;
                    quad.TopLeft.TexCoords.U = left;
                    quad.TopLeft.TexCoords.V = top;
                    quad.TopRight.TexCoords.U = right;
                    quad.TopRight.TexCoords.V = top;
                }
            }

            UpdateTransformedSpriteTextureQuads();
        }

        // For when using a batch node
        // In this instance, drawing will not make use of node's local matrix
        public void UpdateTransformedSpriteTextureQuads()
        {
            if(batchNode == null || AtlasIndex == CCMacros.CCSpriteIndexNotInitialized)
                return;

            transformedQuad = 
                AffineWorldTransform.Transform(quad);

            if(textureAtlas != null && textureAtlas.TotalQuads > AtlasIndex)
                textureAtlas.UpdateQuad(ref transformedQuad, AtlasIndex);
        }

        protected override void ParentUpdatedTransform()
        {
            base.ParentUpdatedTransform();

            UpdateTransformedSpriteTextureQuads();
        }

        #endregion Updating texture quads


        #region Child management

        public override void AddChild(CCNode child, int zOrder, int tag)
        {
            Debug.Assert(child != null, "Argument must be non-NULL");

            if (batchNode != null)
            {
                var sprite = child as CCSprite;

                Debug.Assert(sprite != null, "CCSprite only supports CCSprites as children when using CCSpriteBatchNode");
                Debug.Assert(sprite.Texture.Name == textureAtlas.Texture.Name);

                batchNode.AppendChild(sprite);

                if (!IsReorderChildDirty)
                {
                    SetReorderChildDirtyRecursively();
                }
            }

            base.AddChild(child, zOrder, tag);
        }

        public override void ReorderChild(CCNode child, int zOrder)
        {
            Debug.Assert(child != null);
            Debug.Assert(Children.Contains(child));

            if (zOrder == child.ZOrder)
            {
                return;
            }

            if (batchNode != null && !IsReorderChildDirty)
            {
                SetReorderChildDirtyRecursively();
                batchNode.ReorderBatch(true);
            }

            base.ReorderChild(child, zOrder);
        }

        public override void RemoveChild(CCNode child, bool cleanup)
        {
            if (batchNode != null)
            {
                batchNode.RemoveSpriteFromAtlas((CCSprite)(child));
            }

            base.RemoveChild(child, cleanup);
        }

        public override void RemoveAllChildren(bool cleanup)
        {
            if (batchNode != null)
            {
                CCSpriteBatchNode batch = batchNode;
                CCNode[] elements = Children.Elements;
                for (int i = 0, count = Children.Count; i < count; i++)
                {
                    batch.RemoveSpriteFromAtlas((CCSprite)elements[i]);
                }
            }

            base.RemoveAllChildren(cleanup);
        }

        public override void SortAllChildren()
        {
            if (IsReorderChildDirty)
            {
                var elements = Children.Elements;
                int count = Children.Count;

                Array.Sort(elements, 0, count, this);

                if (batchNode != null)
                {
                    for (int i = 0; i < count; i++)
                    {
                        elements[i].SortAllChildren();
                    }
                }

                IsReorderChildDirty = false;
            }
        }

        public virtual void SetReorderChildDirtyRecursively()
        {
            //only set parents flag the first time
            if (!IsReorderChildDirty)
            {
                IsReorderChildDirty = true;
                CCNode node = Parent;
                while (node != null && node != batchNode)
                {
                    ((CCSprite)node).SetReorderChildDirtyRecursively();
                    node = node.Parent;
                }
            }
        }

        #endregion Child management
    }
}