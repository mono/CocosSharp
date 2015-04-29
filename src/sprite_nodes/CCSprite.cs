using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;

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

        CCQuadCommand quadCommand;


        #region Properties

        // Static properties

        public static float DefaultTexelToContentSizeRatio
        {
            set { DefaultTexelToContentSizeRatios = new CCSize(value, value); }
        }

        public static CCSize DefaultTexelToContentSizeRatios { get; set; }


        // Instance properties

        public int AtlasIndex { get ; internal set; }
        protected internal CCV3F_C4B_T2F_Quad Quad { get { return quadCommand.Quads[0]; } }
        internal CCTextureAtlas TextureAtlas { get; set; }

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

        public override byte Opacity
        {
            get { return base.Opacity; }
            set
            {
                base.Opacity = value;
                UpdateColor();
            }
        }

        public CCBlendFunc BlendFunc 
        { 
            get { return quadCommand.BlendType; }
            set { quadCommand.BlendType = value; }
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
                    if(ContentSize == CCSize.Zero)
                        ContentSize = textureRectInPixels.Size / DefaultTexelToContentSizeRatios;

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
                    Texture,
                    TextureRectInPixels,
                    UntrimmedSizeInPixels,
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
                    if (newTexture != Texture) 
                    {
                        Texture = newTexture;
                    }

                    // update rect
                    IsTextureRectRotated = value.IsRotated;
                    textureRectInPixels = value.TextureRectInPixels;
                    unflippedOffsetPositionFromCenter = value.OffsetInPixels;
                    UntrimmedSizeInPixels = value.OriginalSizeInPixels;

                    ContentSize = UntrimmedSizeInPixels / DefaultTexelToContentSizeRatios;

                    UpdateSpriteTextureQuads();
                }
            }
        }

        public virtual CCTexture2D Texture
        {
            get { return quadCommand.Texture; }
            set
            {
                if (Texture != value)
                {
                    quadCommand.Texture = value;

                    if(TextureRectInPixels == CCRect.Zero) 
                    {
                        CCSize texSize = value.ContentSizeInPixels;
                        TextureRectInPixels = new CCRect(0.0f, 0.0f, texSize.Width, texSize.Height);
                    }

                    if(ContentSize == CCSize.Zero)
                        ContentSize = textureRectInPixels.Size / DefaultTexelToContentSizeRatios;

                    UpdateBlendFunc();
                }
            }
        }

        protected internal CCSize UntrimmedSizeInPixels
        {
            get
            {
                if(untrimmedSizeInPixels == CCSize.Zero)
                    return TextureRectInPixels.Size;
                return untrimmedSizeInPixels;
            }

            set { untrimmedSizeInPixels = value; }
        }

        #endregion Properties


        #region Constructors

        static CCSprite()
        {
            DefaultTexelToContentSizeRatios = CCSize.One;
        }

        public CCSprite()
        {
            IsTextureRectRotated = false;

            opacityModifyRGB = true;
            BlendFunc = CCBlendFunc.AlphaBlend;

            UpdateSpriteTextureQuads();
        }

        public CCSprite(CCTexture2D texture=null, CCRect? texRectInPixels=null, bool rotated=false) : this()
        {
            InitWithTexture(texture, texRectInPixels, rotated);
        }

        public CCSprite(CCSpriteFrame spriteFrame) : this(spriteFrame.ContentSize, spriteFrame)
        {
        }

        public CCSprite(CCSize contentSize, CCSpriteFrame spriteFrame) : this()
        {
            ContentSize = contentSize;
            InitWithSpriteFrame(spriteFrame);
        }

        public CCSprite(string fileName, CCRect? texRectInPixels=null) : this()
        {
            InitWithFile(fileName, texRectInPixels);
        }
            
        void InitWithTexture(CCTexture2D texture, CCRect? texRectInPixels=null, bool rotated=false)
        {
            IsTextureRectRotated = rotated;
            CCSize texSize = (texture != null) ? texture.ContentSizeInPixels : CCSize.Zero;

            textureRectInPixels = texRectInPixels ?? new CCRect(0.0f, 0.0f, texSize.Width, texSize.Height);

            opacityModifyRGB = true;
            BlendFunc = CCBlendFunc.AlphaBlend;

            AnchorPoint = CCPoint.AnchorMiddle;

            Texture = texture;

            // If content size not initialized, assume worldspace dimensions match texture dimensions
            if(ContentSize == CCSize.Zero)
                ContentSize = textureRectInPixels.Size / CCSprite.DefaultTexelToContentSizeRatios;

            UpdateSpriteTextureQuads();
        }

        void InitWithSpriteFrame(CCSpriteFrame spriteFrame)
        {
            opacityModifyRGB = true;
            BlendFunc = CCBlendFunc.AlphaBlend;

            AnchorPoint = CCPoint.AnchorMiddle;

            SpriteFrame = spriteFrame;
        }

        void InitWithFile(string fileName, CCRect? rectInPoints=null)
        {
            Debug.Assert(!String.IsNullOrEmpty(fileName), "Invalid filename for sprite");

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

        protected override void InitialiseRenderCommand()
        {
            quadCommand = new CCQuadCommand(1);
        }

        #endregion Constructors


        internal override void VisitRenderer()
        {
            quadCommand.GlobalDepth = VertexZ;
            quadCommand.WorldTransform = AffineWorldTransform;
            Renderer.AddCommand(quadCommand);
        }

        protected override void Draw()
        {
            base.Draw();

            quadCommand.RequestUpdateQuads(DrawCallback);
        }

        void DrawCallback(ref CCV3F_C4B_T2F_Quad[] Quads)
        {
            CCDrawManager drawManager = DrawManager;

            drawManager.BlendFunc(BlendFunc);
            drawManager.BindTexture(Texture);
            drawManager.DrawQuad(ref Quads[0]);
        }

        public bool IsSpriteFrameDisplayed(CCSpriteFrame frame)
        {
            CCRect r = frame.TextureRectInPixels;

            return (
                CCRect.Equal(ref r, ref textureRectInPixels) &&
                frame.Texture.Name == Texture.Name
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

        #region Color managment


		public override void UpdateColor()
        {
            quadCommand.RequestUpdateQuads(UpdateColorCallback);
        }

        void UpdateColorCallback(ref CCV3F_C4B_T2F_Quad[] quads)
        {
            var color4 = new CCColor4B(DisplayedColor.R, DisplayedColor.G, DisplayedColor.B, DisplayedOpacity);

            if(opacityModifyRGB)
            {
                color4.R = (byte)(color4.R * DisplayedOpacity / 255.0f);
                color4.G = (byte)(color4.G * DisplayedOpacity / 255.0f);
                color4.B = (byte)(color4.B * DisplayedOpacity / 255.0f);
            }

            quads[0].BottomLeft.Colors = color4;
            quads[0].BottomRight.Colors = color4;
            quads[0].TopLeft.Colors = color4;
            quads[0].TopRight.Colors = color4;
        }

        protected void UpdateBlendFunc()
        {
            // it's possible to have an untextured sprite
            if (Texture == null || !Texture.HasPremultipliedAlpha)
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
            quadCommand.RequestUpdateQuads(UpdateSpriteTextureQuadsCallback);
        }

        void UpdateSpriteTextureQuadsCallback(ref CCV3F_C4B_T2F_Quad[] quads)
        {
            if (!Visible)
            {
                quads[0].BottomRight.Vertices = quads[0].TopLeft.Vertices 
                    = quads[0].TopRight.Vertices = quads[0].BottomLeft.Vertices = CCVertex3F.Zero;
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

                CCPoint centerPoint = UntrimmedSizeInPixels.Center + relativeOffset;
                CCPoint subRectOrigin;
                subRectOrigin.X = centerPoint.X - textureRectInPixels.Size.Width / 2.0f;
                subRectOrigin.Y = centerPoint.Y - textureRectInPixels.Size.Height / 2.0f;

                CCRect subRectRatio = CCRect.Zero;

                if (UntrimmedSizeInPixels.Width > 0 && UntrimmedSizeInPixels.Height > 0)
                {
                    subRectRatio = new CCRect(
                        subRectOrigin.X / UntrimmedSizeInPixels.Width, 
                        subRectOrigin.Y / UntrimmedSizeInPixels.Height,
                        textureRectInPixels.Size.Width / UntrimmedSizeInPixels.Width,
                        textureRectInPixels.Size.Height / UntrimmedSizeInPixels.Height);
                }

                // Atlas: Vertex
                float x1 = subRectRatio.Origin.X * ContentSize.Width;
                float y1 = subRectRatio.Origin.Y * ContentSize.Height;
                float x2 = x1 + (subRectRatio.Size.Width * ContentSize.Width);
                float y2 = y1 + (subRectRatio.Size.Height * ContentSize.Height);

                // Don't set z-value: The node's transform will be set to include z offset
                quads[0].BottomLeft.Vertices = new CCVertex3F(x1, y1, 0);
                quads[0].BottomRight.Vertices = new CCVertex3F(x2, y1, 0);
                quads[0].TopLeft.Vertices = new CCVertex3F(x1, y2, 0);
                quads[0].TopRight.Vertices = new CCVertex3F(x2, y2, 0);

                if (Texture == null)
                {
                    return;
                }

                float atlasWidth = Texture.PixelsWide;
                float atlasHeight = Texture.PixelsHigh;

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

                    quads[0].BottomLeft.TexCoords.U = left;
                    quads[0].BottomLeft.TexCoords.V = top;
                    quads[0].BottomRight.TexCoords.U = left;
                    quads[0].BottomRight.TexCoords.V = bottom;
                    quads[0].TopLeft.TexCoords.U = right;
                    quads[0].TopLeft.TexCoords.V = top;
                    quads[0].TopRight.TexCoords.U = right;
                    quads[0].TopRight.TexCoords.V = bottom;
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

                    quads[0].BottomLeft.TexCoords.U = left;
                    quads[0].BottomLeft.TexCoords.V = bottom;
                    quads[0].BottomRight.TexCoords.U = right;
                    quads[0].BottomRight.TexCoords.V = bottom;
                    quads[0].TopLeft.TexCoords.U = left;
                    quads[0].TopLeft.TexCoords.V = top;
                    quads[0].TopRight.TexCoords.U = right;
                    quads[0].TopRight.TexCoords.V = top;
                }
            }
        }


        public void UpdateLocalTransformedSpriteTextureQuads()
        {
            quadCommand.RequestUpdateQuads(UpdateLocalTransformedSpriteTextureQuadsCallback);
        }

        void UpdateLocalTransformedSpriteTextureQuadsCallback(ref CCV3F_C4B_T2F_Quad[] quads)
        {
            if(AtlasIndex == CCMacros.CCSpriteIndexNotInitialized)
                return;

            var transformedQuad = quads[0];

            // We can't use the AffineLocalTransform because that's the 2d projection of a 3d transformation
            // i.e. The Z coords of our quad would remain unaltered which in general incorrect
            // Instead, we need use the XnaLocalTransform which incorporates any potential z-transforms
            Matrix worldMatrix = XnaLocalMatrix;
            Vector3 topLeft = quads[0].TopLeft.Vertices.XnaVector;
            Vector3 topRight = quads[0].TopRight.Vertices.XnaVector;
            Vector3 bottomLeft = quads[0].BottomLeft.Vertices.XnaVector;
            Vector3 bottomRight = quads[0].BottomRight.Vertices.XnaVector;

            topLeft = Vector3.Transform(topLeft, worldMatrix);
            topRight = Vector3.Transform(topRight, worldMatrix);
            bottomLeft = Vector3.Transform(bottomLeft, worldMatrix);
            bottomRight = Vector3.Transform(bottomRight, worldMatrix);

            transformedQuad.TopLeft.Vertices = new CCVertex3F(topLeft.X, topLeft.Y, topLeft.Z);
            transformedQuad.TopRight.Vertices = new CCVertex3F(topRight.X, topRight.Y, topRight.Z);
            transformedQuad.BottomLeft.Vertices = new CCVertex3F(bottomLeft.X, bottomLeft.Y, bottomLeft.Z);
            transformedQuad.BottomRight.Vertices = new CCVertex3F(bottomRight.X, bottomRight.Y, bottomRight.Z);

            if(TextureAtlas != null && TextureAtlas.TotalQuads > AtlasIndex)
                TextureAtlas.UpdateQuad(ref transformedQuad, AtlasIndex);
        }

        #endregion Updating texture quads


        #region Child management

        public override void ReorderChild(CCNode child, int zOrder)
        {
            Debug.Assert(child != null);
            Debug.Assert(Children.Contains(child));

            if (zOrder == child.ZOrder)
            {
                return;
            }

            base.ReorderChild(child, zOrder);
        }

        public override void SortAllChildren()
        {
            if (IsReorderChildDirty)
            {
                var elements = Children.Elements;
                int count = Children.Count;

                Array.Sort(elements, 0, count, this);

                IsReorderChildDirty = false;
            }
        }

        #endregion Child management
    }
}