using System;
using System.IO;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCSprite : CCNodeRGBA, ICCTexture
    {
        bool dirty;                     // Sprite needs to be updated
        bool flipX;
        bool flipY;
        bool hasChildren;               // optimization to check if it contain children
        bool opacityModifyRGB;
        bool recursiveDirty;            // Subchildren needs to be updated
        bool shouldBeHidden;            // should not be drawn because one of the ancestors is not visible

        CCRect textureRectInPoints;

        CCPoint unflippedOffsetPositionFromCenter;
        CCSpriteFrame initialSpriteFrame;
        CCSpriteBatchNode batchNode;    // Used batch node (weak reference)
        CCTextureAtlas textureAtlas;    // Sprite Sheet texture atlas (weak reference)
        CCTexture2D texture;
        string textureFile;
        CCAffineTransform transformToBatch;

        public CCV3F_C4B_T2F_Quad Quad;


        #region Properties

        public bool IsTextureRectRotated { get; private set; }
        public int AtlasIndex { get; set; }                     // Absolute (real) Index on the SpriteSheet
        public CCPoint OffsetPosition { get; private set; }     // Offset Position (used by Zwoptex)
        public CCBlendFunc BlendFunc { get; set; }


        public virtual bool Dirty
        {
            get { return dirty; }
            set
            {
                dirty = value;
                SetDirtyRecursively(value);
            }
        }

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

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                SetDirtyRecursively();
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
                    SetTextureRect(textureRectInPoints, IsTextureRectRotated, ContentSize);
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
                    SetTextureRect(textureRectInPoints, IsTextureRectRotated, ContentSize);
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

        public override CCColor3B Color
        {
            get { return base.Color; }
            set
            {
                base.Color = value;
                UpdateColor();
            }
        }

        public override CCPoint Position
        {
            get { return base.Position; }
            set
            {
                base.Position = value;
                SetDirtyRecursively();
            }
        }

        public CCRect TextureRect
        {
            get { return textureRectInPoints; }
            set { SetTextureRect(value, false, value.Size); }
        }

        // Rotation of the sprite, about the Z axis, in Degrees.
        public override float Rotation
        {
            set
            {
                base.Rotation = value;
                SetDirtyRecursively();
            }
        }

        // Rotation of the sprite, about the X axis, in Degrees.
        public override float RotationX
        {
            get { return base.RotationX; }
            set
            {
                base.RotationX = value;
                SetDirtyRecursively();
            }
        }

        // Rotation of the sprite, about the Y axis, in Degrees.
        public override float RotationY
        {
            get { return base.RotationY; }
            set
            {
                base.RotationY = value;
                SetDirtyRecursively();
            }
        }

        public override float SkewX
        {
            get { return base.SkewX; }
            set
            {
                base.SkewX = value;
                SetDirtyRecursively();
            }
        }

        public override float SkewY
        {
            get { return base.SkewY; }
            set
            {
                base.SkewY = value;
                SetDirtyRecursively();
            }
        }

        public virtual void ScaleTo(CCSize size)
        {
            CCSize content = ContentSize;
            float sx = size.Width / content.Width;
            float sy = size.Height / content.Height;
            base.ScaleX = sx;
            base.ScaleY = sy;
            SetDirtyRecursively();
        }

        public override float ScaleX
        {
            get { return base.ScaleX; }
            set
            {
                base.ScaleX = value;
                SetDirtyRecursively();
            }
        }

        public override float ScaleY
        {
            get { return base.ScaleY; }
            set
            {
                base.ScaleY = value;
                SetDirtyRecursively();
            }
        }

        public override float Scale
        {
            set
            {
                base.Scale = value;
                SetDirtyRecursively();
            }
        }

        public override float VertexZ
        {
            get { return base.VertexZ; }
            set
            {
                base.VertexZ = value;
                SetDirtyRecursively();
            }
        }

        public override CCPoint AnchorPoint
        {
            get { return base.AnchorPoint; }
            set
            {
                base.AnchorPoint = value;
                SetDirtyRecursively();
            }
        }

        public override bool IgnoreAnchorPointForPosition
        {
            get { return base.IgnoreAnchorPointForPosition; }
            set
            {
                Debug.Assert(batchNode == null, "ignoreAnchorPointForPosition is invalid in CCSprite");
                base.IgnoreAnchorPointForPosition = value;
                SetDirtyRecursively();
            }
        }

        public CCSpriteFrame SpriteFrame
        {
            get
            {
                return new CCSpriteFrame(
                    texture,
                    textureRectInPoints.PointsToPixels(Director.ContentScaleFactor),
                    ContentSize.PointsToPixels(Director.ContentScaleFactor),
                    IsTextureRectRotated,
                    unflippedOffsetPositionFromCenter.PointsToPixels(Director.ContentScaleFactor)
                );
            }
            set
            {
                unflippedOffsetPositionFromCenter = value.OffsetInPixels;

                CCTexture2D newTexture = value.Texture;
                // update texture before updating texture rect
                if (newTexture != texture)
                {
                    Texture = newTexture;
                }

                // update rect
                float contentScaleFactor = Director.ContentScaleFactor;
                IsTextureRectRotated = value.IsRotated;
                SetTextureRect(value.RectInPixels.PixelsToPoints(contentScaleFactor), 
                    IsTextureRectRotated, 
                    value.OriginalSizeInPixels.PixelsToPoints(contentScaleFactor));
            }
        }

        public CCSpriteBatchNode BatchNode
        {
            get { return batchNode; }
            set
            {
                batchNode = value;

                if (value == null)
                {
                    AtlasIndex = CCMacros.CCSpriteIndexNotInitialized;
                    textureAtlas = null;
                    recursiveDirty = false;
                    Dirty = false;

                    float x1 = OffsetPosition.X;
                    float y1 = OffsetPosition.Y;
                    float x2 = x1 + textureRectInPoints.Size.Width;
                    float y2 = y1 + textureRectInPoints.Size.Height;

                    Quad.BottomLeft.Vertices = new CCVertex3F(x1, y1, 0);
                    Quad.BottomRight.Vertices = new CCVertex3F(x2, y1, 0);
                    Quad.TopLeft.Vertices = new CCVertex3F(x1, y2, 0);
                    Quad.TopRight.Vertices = new CCVertex3F(x2, y2, 0);
                }
                else
                {
                    // using batch
                    transformToBatch = CCAffineTransform.Identity;
                    textureAtlas = batchNode.TextureAtlas; // weak ref

                    if(Director != null) 
                    {
                        batchNode.Director = Director;
                    }
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
                    UpdateBlendFunc();
                }
            }
        }

        public override CCDirector Director 
        { 
            get { return base.Director; }
            internal set 
            {
                base.Director = value;

                if (value != null && BatchNode != null)
                {
                    BatchNode.Director = value;
                }
            }
        }

        #endregion Properties


        #region Constructors

        public CCSprite(CCTexture2D texture=null, CCRect? rectInPoints=null, bool rotated=false)
        {
            InitWithTexture(texture, rectInPoints);
        }

        public CCSprite(CCSize size) : this((CCTexture2D)null, new CCRect(0, 0, size.Width, size.Height))
        {
        }

        public CCSprite(CCSpriteFrame spriteFrame)
        {
            InitWithSpriteFrame(spriteFrame);
        }

        public CCSprite(string fileName, CCRect? rectInPoints=null)
        {
            InitWithFile(fileName, rectInPoints);
        }

        // Used externally by non-subclasses
        internal void InitWithTexture(CCTexture2D texture, CCRect? rectInPoints=null, bool rotated=false)
        {
            IsTextureRectRotated = rotated;
            textureRectInPoints = rectInPoints ?? CCRect.Zero;

            opacityModifyRGB = true;
            BlendFunc = CCBlendFunc.AlphaBlend;

            AnchorPoint = new CCPoint(0.5f, 0.5f);
            OffsetPosition = CCPoint.Zero;

            Quad = new CCV3F_C4B_T2F_Quad();
            Quad.BottomLeft.Colors = CCColor4B.White;
            Quad.BottomRight.Colors = CCColor4B.White;
            Quad.TopLeft.Colors = CCColor4B.White;
            Quad.TopRight.Colors = CCColor4B.White;

            Texture = texture;
        }

        void InitWithSpriteFrame(CCSpriteFrame spriteFrame)
        {
            initialSpriteFrame = spriteFrame;

            opacityModifyRGB = true;
            BlendFunc = CCBlendFunc.AlphaBlend;

            AnchorPoint = new CCPoint(0.5f, 0.5f);
            OffsetPosition = CCPoint.Zero;

            Quad = new CCV3F_C4B_T2F_Quad();
            Quad.BottomLeft.Colors = CCColor4B.White;
            Quad.BottomRight.Colors = CCColor4B.White;
            Quad.TopLeft.Colors = CCColor4B.White;
            Quad.TopRight.Colors = CCColor4B.White;

            if(Director != null) 
            {
                SpriteFrame = initialSpriteFrame;
            }
        }

        void InitWithFile(string fileName, CCRect? rectInPoints=null)
        {
            Debug.Assert(!String.IsNullOrEmpty(fileName), "Invalid filename for sprite");

            textureFile = fileName;

            // Try sprite frame cache first
            CCSpriteFrame frame = CCApplication.SharedApplication.SpriteFrameCache[fileName];
            if (frame != null) 
            {
                InitWithSpriteFrame (frame);
            } 
            else 
            {
                // If frame doesn't exist, try texture cache
                CCTexture2D texture = CCApplication.SharedApplication.TextureCache.AddImage(fileName);
                if (texture != null) 
                {
                    InitWithTexture (texture, rectInPoints);
                }
            }
        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow(windowSize);

            if (initialSpriteFrame != null) 
            {
                SpriteFrame = initialSpriteFrame;
            } 
            else if (Texture != null) 
            {
                if(textureRectInPoints == CCRect.Zero) 
                {
                    textureRectInPoints.Size = Texture.ContentSize (Director.ContentScaleFactor);
                }

                SetTextureRect (textureRectInPoints, IsTextureRectRotated, textureRectInPoints.Size);
            }
        }

        #endregion Setup content


        protected override void Draw()
        {
            Debug.Assert(batchNode == null);

            CCDrawManager.BlendFunc(BlendFunc);
            CCDrawManager.BindTexture(Texture);
            CCDrawManager.DrawQuad(ref Quad);
        }

        public bool IsSpriteFrameDisplayed(CCSpriteFrame frame)
        {
            CCRect r = frame.RectInPixels.PixelsToPoints(Director.ContentScaleFactor);

            return (
                CCRect.Equal(ref r, ref textureRectInPoints) &&
                frame.Texture.Name == texture.Name &&
                frame.OffsetInPixels.Equals(unflippedOffsetPositionFromCenter)
            );
        }

        public void SetSpriteFrameWithAnimationName(string animationName, int frameIndex)
        {
            Debug.Assert(!String.IsNullOrEmpty(animationName),
                "CCSprite#setDisplayFrameWithAnimationName. animationName must not be NULL");

            CCAnimation a = CCApplication.SharedApplication.AnimationCache[animationName];

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
            CCSerialization.SerializeData(Dirty, sw);
            CCSerialization.SerializeData(IsTextureRectRotated, sw);
            CCSerialization.SerializeData(AtlasIndex, sw);
            CCSerialization.SerializeData(TextureRect, sw);
            CCSerialization.SerializeData(OffsetPosition, sw);
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
            Dirty = CCSerialization.DeSerializeBool(sr);
            IsTextureRectRotated = CCSerialization.DeSerializeBool(sr);
            AtlasIndex = CCSerialization.DeSerializeInt(sr);
            TextureRect = CCSerialization.DeSerializeRect(sr);
            OffsetPosition = CCSerialization.DeSerializePoint(sr);
        }

        #endregion Serialization


        #region Color managment

        void UpdateColor()
        {
            var color4 = new CCColor4B(DisplayedColor.R, DisplayedColor.G, DisplayedColor.B, DisplayedOpacity);

            if (opacityModifyRGB)
            {
                color4 *= (DisplayedOpacity / 255.0f);
            }

            Quad.BottomLeft.Colors = color4;
            Quad.BottomRight.Colors = color4;
            Quad.TopLeft.Colors = color4;
            Quad.TopRight.Colors = color4;

            // renders using Sprite Manager
            if (batchNode != null)
            {
                if (AtlasIndex != CCMacros.CCSpriteIndexNotInitialized)
                {
                    textureAtlas.UpdateQuad(ref Quad, AtlasIndex);
                }
                else
                {
                    // no need to set it recursively
                    // update dirty_, don't update recursiveDirty_
                    dirty = true;
                }
            }

            // self render
            // do nothing
        }

        public override void UpdateDisplayedColor(CCColor3B parentColor)
        {
            base.UpdateDisplayedColor(parentColor);
            UpdateColor();
        }

        public override void UpdateDisplayedOpacity(byte parentOpacity)
        {
            base.UpdateDisplayedOpacity(parentOpacity);
            UpdateColor();
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


        public void SetTextureRect(CCRect rectInPoints)
        {
            SetTextureRect(rectInPoints, false, rectInPoints.Size);
        }

        public void SetTextureRect(CCRect rectInPoints, bool rotated, CCSize sizeInPoints)
        {
            IsTextureRectRotated = rotated;

            ContentSize = sizeInPoints;
            SetVertexRect(rectInPoints);
            SetTextureCoords(rectInPoints);

            CCPoint relativeOffset = unflippedOffsetPositionFromCenter;

            // issue #732
            if (flipX)
            {
                relativeOffset.X = -relativeOffset.X;
            }
            if (flipY)
            {
                relativeOffset.Y = -relativeOffset.Y;
            }

            OffsetPosition = new CCPoint (
                relativeOffset.X + (ContentSize.Width - textureRectInPoints.Size.Width) / 2,
                relativeOffset.Y + (ContentSize.Height - textureRectInPoints.Size.Height) / 2);

            // rendering using batch node
            if (batchNode != null)
            {
                // update dirty_, don't update recursiveDirty_
                Dirty = true;
            }
            else
            {
                // self rendering

                // Atlas: Vertex
                float x1 = 0 + OffsetPosition.X;
                float y1 = 0 + OffsetPosition.Y;
                float x2 = x1 + textureRectInPoints.Size.Width;
                float y2 = y1 + textureRectInPoints.Size.Height;

                // Don't update Z.
                Quad.BottomLeft.Vertices = new CCVertex3F(x1, y1, 0);
                Quad.BottomRight.Vertices = new CCVertex3F(x2, y1, 0);
                Quad.TopLeft.Vertices = new CCVertex3F(x1, y2, 0);
                Quad.TopRight.Vertices = new CCVertex3F(x2, y2, 0);
            }
        }

        // override this method to generate "double scale" sprites
        protected virtual void SetVertexRect(CCRect rectInPoints)
        {
            textureRectInPoints = rectInPoints;
        }

        void SetTextureCoords(CCRect rectInPoints)
        {
            CCRect rectInPixels = rectInPoints.PointsToPixels(Director.ContentScaleFactor);

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
                left = (2 * rect.Origin.X + 1) / (2 * atlasWidth);
                right = left + (rect.Size.Height * 2 - 2) / (2 * atlasWidth);
                top = (2 * rect.Origin.Y + 1) / (2 * atlasHeight);
                bottom = top + (rect.Size.Width * 2 - 2) / (2 * atlasHeight);
                #else
                left = rectInPixels.Origin.X / atlasWidth;
                right = (rectInPixels.Origin.X + rectInPixels.Size.Height) / atlasWidth;
                top = rectInPixels.Origin.Y / atlasHeight;
                bottom = (rectInPixels.Origin.Y + rectInPixels.Size.Width) / atlasHeight;
                #endif

                if (flipX)
                {
                    CCMacros.CCSwap(ref top, ref bottom);
                }

                if (flipY)
                {
                    CCMacros.CCSwap(ref left, ref right);
                }

                Quad.BottomLeft.TexCoords.U = left;
                Quad.BottomLeft.TexCoords.V = top;
                Quad.BottomRight.TexCoords.U = left;
                Quad.BottomRight.TexCoords.V = bottom;
                Quad.TopLeft.TexCoords.U = right;
                Quad.TopLeft.TexCoords.V = top;
                Quad.TopRight.TexCoords.U = right;
                Quad.TopRight.TexCoords.V = bottom;
            }
            else
            {
                #if CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL
                left = (2 * rect.Origin.X + 1) / (2 * atlasWidth);
                right = left + (rect.Size.Width * 2 - 2) / (2 * atlasWidth);
                top = (2 * rect.Origin.Y + 1) / (2 * atlasHeight);
                bottom = top + (rect.Size.Height * 2 - 2) / (2 * atlasHeight);
                #else
                left = rectInPixels.Origin.X / atlasWidth;
                right = (rectInPixels.Origin.X + rectInPixels.Size.Width) / atlasWidth;
                top = rectInPixels.Origin.Y / atlasHeight;
                bottom = (rectInPixels.Origin.Y + rectInPixels.Size.Height) / atlasHeight;
                #endif

                if (flipX)
                {
                    CCMacros.CCSwap(ref left, ref right);
                }

                if (flipY)
                {
                    CCMacros.CCSwap(ref top, ref bottom);
                }

                Quad.BottomLeft.TexCoords.U = left;
                Quad.BottomLeft.TexCoords.V = bottom;
                Quad.BottomRight.TexCoords.U = right;
                Quad.BottomRight.TexCoords.V = bottom;
                Quad.TopLeft.TexCoords.U = left;
                Quad.TopLeft.TexCoords.V = top;
                Quad.TopRight.TexCoords.U = right;
                Quad.TopRight.TexCoords.V = top;
            }
        }

        public override void UpdateTransform()
        {
            Debug.Assert(batchNode != null,
                "updateTransform is only valid when CCSprite is being rendered using an CCSpriteBatchNode");

            // recaculate matrix only if it is dirty
            if (Dirty)
            {
                // If it is not visible, or one of its ancestors is not visible, then do nothing:
                if (!Visible ||
                    (Parent != null && Parent != batchNode && ((CCSprite)Parent).shouldBeHidden))
                {
                    Quad.BottomRight.Vertices =
                    Quad.TopLeft.Vertices = Quad.TopRight.Vertices = Quad.BottomLeft.Vertices = new CCVertex3F(0, 0, 0);
                    shouldBeHidden = true;
                }
                else
                {
                    shouldBeHidden = false;

                    if (Parent == null || Parent == batchNode)
                    {
                        transformToBatch = NodeToParentTransform();
                    }
                    else
                    {
                        Debug.Assert((Parent as CCSprite) != null,
                            "Logic error in CCSprite. Parent must be a CCSprite");
                        transformToBatch = CCAffineTransform.Concat(NodeToParentTransform(),
                            ((CCSprite)Parent).
                            transformToBatch);
                    }

                    //
                    // calculate the Quad based on the Affine Matrix
                    //

                    CCSize size = textureRectInPoints.Size;

                    float x1 = OffsetPosition.X;
                    float y1 = OffsetPosition.Y;

                    float x2 = x1 + size.Width;
                    float y2 = y1 + size.Height;
                    float x = transformToBatch.Tx;
                    float y = transformToBatch.Ty;

                    float cr = transformToBatch.A;
                    float sr = transformToBatch.B;
                    float cr2 = transformToBatch.D;
                    float sr2 = -transformToBatch.C;
                    float ax = x1 * cr - y1 * sr2 + x;
                    float ay = x1 * sr + y1 * cr2 + y;

                    float bx = x2 * cr - y1 * sr2 + x;
                    float by = x2 * sr + y1 * cr2 + y;

                    float cx = x2 * cr - y2 * sr2 + x;
                    float cy = x2 * sr + y2 * cr2 + y;

                    float dx = x1 * cr - y2 * sr2 + x;
                    float dy = x1 * sr + y2 * cr2 + y;

                    Quad.BottomLeft.Vertices = new CCVertex3F(ax, ay, VertexZ);
                    Quad.BottomRight.Vertices = new CCVertex3F(bx, by, VertexZ);
                    Quad.TopLeft.Vertices = new CCVertex3F(dx, dy, VertexZ);
                    Quad.TopRight.Vertices = new CCVertex3F(cx, cy, VertexZ);
                }

                textureAtlas.UpdateQuad(ref Quad, AtlasIndex);
                recursiveDirty = false;
                dirty = false;
            }

            // recursively iterate over children
            if (hasChildren)
            {
                CCNode[] elements = Children.Elements;
                if (batchNode != null)
                {
                    for (int i = 0, count = Children.Count; i < count; i++)
                    {
                        ((CCSprite)elements[i]).UpdateTransform();
                    }
                }
                else
                {
                    for (int i = 0, count = Children.Count; i < count; i++)
                    {
                        var sprite = elements[i] as CCSprite;
                        if (sprite != null)
                        {
                            sprite.UpdateTransform();
                        }
                    }
                }
            }
        }


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
            hasChildren = true;
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

            hasChildren = false;
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

        public virtual void SetDirtyRecursively(bool value)
        {
            dirty = recursiveDirty = value;

            // recursively set dirty
            if (hasChildren)
            {
                CCNode[] elements = Children.Elements;
                for (int i = 0, count = Children.Count; i < count; i++)
                {
                    var sprite = elements[i] as CCSprite;
                    if (sprite != null)
                    {
                        sprite.SetDirtyRecursively(true);
                    }
                }
            }
        }

        void SetDirtyRecursively()
        {
            if (batchNode != null && !recursiveDirty)
            {
                dirty = recursiveDirty = true;
                if (hasChildren)
                {
                    SetDirtyRecursively(true);
                }
            }
        }

        #endregion Child management
    }
}