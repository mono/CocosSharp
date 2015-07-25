namespace CocosSharp
{
    /// <summary>
    /// This is a special sprite container that represents a 9 point sprite region, where 8 of the
    /// points are along the perimeter, and the 9th is the center area. This special sprite is capable of resizing
    /// itself to arbitrary scales.
    /// 
    /// Original code is from http://www.cocos2d-x.org 
    /// </summary>
    public class CCScale9Sprite : CCNode
    {

        bool positionsAreDirty;
        bool opacityModifyRGB;
        bool spriteFrameRotated;
        bool spritesGenerated;

        float insetBottom;
        float insetLeft;
        float insetRight;
        float insetTop;

        /** 
        * The end-cap insets. 
        * On a non-resizeable sprite, this property is set to CCRect.Zero the sprite 
        * does not use end caps and the entire sprite is subject to stretching. 
        */
        CCRect capInsets;
        CCRect capInsetsInternal;
        CCRect spriteRect;

        CCSize originalSize;
        CCSize preferredSize;
        CCPoint offset;

        CCSprite scale9Image;
        CCSprite topSprite;
        CCSprite topLeftSprite;
        CCSprite topRightSprite;
        CCSprite bottomSprite;
        CCSprite bottomLeftSprite;
        CCSprite bottomRightSprite;
        CCSprite centerSprite;
        CCSprite leftSprite;
        CCSprite rightSprite;

        bool isScale9Enabled = true;

        CCSize topLeftSize;
        CCSize centerSize;
        CCSize bottomRightSize;
        CCPoint centerOffset;

        bool isFlippedX;
        bool isFlippedY;

        CCBlendFunc _blendFunc;

        CCRawList<CCNode> protectedChildren = new CCRawList<CCNode>(9); ///holds the 9 sprites

        #region Properties

        public override CCSize ContentSize
        {
            get { return base.ContentSize; }
            set
            {
                base.ContentSize = value;
                positionsAreDirty = true;
            }
        }

        public CCSize PreferredSize
        {
            get { return preferredSize; }
            set
            {
                ContentSize = value;
                preferredSize = value;
            }
        }

        public CCRect CapInsets
        {
            get { return capInsets; }
            set
            {
                CCSize contentSize = ContentSize;
                UpdateWithSprite(scale9Image, spriteRect, spriteFrameRotated, value);
                ContentSize = contentSize;
            }
        }

        public float InsetLeft
        {
            set
            {
                insetLeft = value;
                UpdateCapInset();
            }
            get { return insetLeft; }
        }

        public float InsetTop
        {
            set
            {
                insetTop = value;
                UpdateCapInset();
            }
            get { return insetTop; }
        }

        public float InsetRight
        {
            set
            {
                insetRight = value;
                UpdateCapInset();
            }
            get { return insetRight; }
        }

        public float InsetBottom
        {
            set
            {
                insetBottom = value;
                UpdateCapInset();
            }
            get { return insetBottom; }
        }

        public bool IsFlippedX
        {
            get { return isFlippedX; }
            set
            {
                if (isFlippedX == value)
                    return;

                var realScale = ScaleX;
                isFlippedX = value;
                ScaleX = realScale;
            }
        }

        public override float ScaleX
        {
            get
            {
                var originalScale = base.ScaleX;
                if (isFlippedX)
                {
                    originalScale = originalScale * -1.0f;
                }
                return originalScale;
            }

            set
            {

                if (isFlippedX)
                {
                    value = value * -1;
                }

                base.ScaleX = value;
            }
        }

        public bool IsFlippedY
        {
            get { return isFlippedY; }
            set
            {
                if (isFlippedY == value)
                    return;

                var realScale = ScaleY;
                isFlippedY = value;
                ScaleY = realScale;
            }
        }

        public override float ScaleY
        {
            get
            {
                var originalScale = base.ScaleY;
                if (isFlippedY)
                {
                    originalScale = originalScale * -1.0f;
                }
                return originalScale;
            }

            set
            {

                if (isFlippedY)
                {
                    value = value * -1;
                }

                base.ScaleY = value;
            }
        }

        public bool IsScale9Enabled
        {
            get { return isScale9Enabled; }
            set
            {
                if (isScale9Enabled == value)
                    return;

                isScale9Enabled = value;

                CleanupSlicedSprites();
                protectedChildren.Clear();

                if (isScale9Enabled)
                {
                    if (scale9Image != null)
                    {
                        UpdateWithSprite(scale9Image,
                                               spriteRect,
                                               spriteFrameRotated,
                                               offset,
                                               originalSize,
                                               capInsets);
                    }
                }
                positionsAreDirty = true;
            }
        }

        public CCBlendFunc BlendFunc
        {
            get { return _blendFunc; }
            set
            {
                if (_blendFunc == value)
                    return;

                _blendFunc = value;
                ApplyBlendFunc();
            }
        }

        public CCSpriteFrame SpriteFrame
        {
            set
            {

                var spriteFrame = value;
                var sprite = new CCSprite(spriteFrame);

                if (spriteFrame != null && spriteFrame.Texture != null)
                {
                    UpdateWithSprite(sprite,
                        spriteFrame.TextureRectInPixels,
                        spriteFrame.IsRotated,
                        spriteFrame.OffsetInPixels,
                        spriteFrame.OriginalSizeInPixels,
                        capInsets);

                    // Reset insets
                    insetLeft = 0;
                    insetTop = 0;
                    insetRight = 0;
                    insetBottom = 0;
                }


            }
        }

        #endregion Properties


        #region Constructors

        public CCScale9Sprite(CCSprite sprite, CCRect rect, bool rotated, CCRect capInsets)
            : this()
        {
            if (sprite != null)
            {
                UpdateWithSprite(sprite, rect, rotated, capInsets);
            }

        }

        public CCScale9Sprite(CCSprite sprite, CCRect rect, CCRect capInsets)
            : this(sprite, rect, false, capInsets)
        {
        }

        public CCScale9Sprite()
        {
            AnchorPoint = CCPoint.AnchorMiddle;
        }

        // File

        public CCScale9Sprite(string file, CCRect rect, CCRect capInsets) 
            : this(new CCSprite(file), rect, capInsets)
        {
        }

        public CCScale9Sprite(string file, CCRect rect) : this(file, rect, CCRect.Zero)
        {
        }

        public CCScale9Sprite(string file) : this(file, CCRect.Zero)
        {
        }

        // Sprite frame

        public CCScale9Sprite(CCSpriteFrame spriteFrame, CCRect capInsets)
        {
            var sprite = new CCSprite(spriteFrame);

            if (spriteFrame != null && spriteFrame.Texture != null)
            {
                UpdateWithSprite(sprite,
                    spriteFrame.TextureRectInPixels,
                    spriteFrame.IsRotated,
                    spriteFrame.OffsetInPixels,
                    spriteFrame.OriginalSizeInPixels,
                    capInsets);

                // Reset insets
                insetLeft = 0;
                insetTop = 0;
                insetRight = 0;
                insetBottom = 0;
            }

        }

        public CCScale9Sprite(CCSpriteFrame spriteFrame) : this(spriteFrame, CCRect.Zero)
        {
        }

        // Sprite frame name

        // A constructor with argument string already exists (filename), so create this factory method instead
        public static CCScale9Sprite SpriteWithFrameName(string spriteFrameName, CCRect capInsets)
        {
            CCScale9Sprite sprite = new CCScale9Sprite();
            sprite.InitWithSpriteFrameName(spriteFrameName, capInsets);

            return sprite;
        }

        public static CCScale9Sprite SpriteWithFrameName(string spriteFrameName)
        {
            return CCScale9Sprite.SpriteWithFrameName(spriteFrameName, CCRect.Zero);
        }

        // Init calls that are called externally for objects that are already instantiated
        internal void InitWithSpriteFrame(CCSpriteFrame spriteFrame)
        {

            var sprite = new CCSprite(spriteFrame);

            if (spriteFrame != null && spriteFrame.Texture != null)
            {
                UpdateWithSprite(sprite,
                    spriteFrame.TextureRectInPixels,
                    spriteFrame.IsRotated,
                    spriteFrame.OffsetInPixels,
                    spriteFrame.OriginalSizeInPixels,
                    capInsets);

            }

        }

        internal void InitWithSpriteFrameName(string spriteFrameName)
        {
            InitWithSpriteFrameName(spriteFrameName, CCRect.Zero);
        }

        internal void InitWithSpriteFrameName(string spriteFrameName, CCRect capInsets)
        {
            CCSpriteFrame spriteFrame = CCSpriteFrameCache.SharedSpriteFrameCache[spriteFrameName];

            var sprite = new CCSprite(spriteFrame);

            if (spriteFrame != null && spriteFrame.Texture != null)
            {
                UpdateWithSprite(sprite,
                    spriteFrame.TextureRectInPixels,
                    spriteFrame.IsRotated,
                    spriteFrame.OffsetInPixels,
                    spriteFrame.OriginalSizeInPixels,
                    capInsets);
            }
        }

        #endregion Constructors


        public override void Visit(ref CCAffineTransform parentWorldTransform)
        {
            if (positionsAreDirty)
            {
                UpdatePositions();
                AdjustScale9ImagePosition();
                positionsAreDirty = false;

            }

            var localTransform = AffineLocalTransform;
            var worldTransform = CCAffineTransform.Identity;
            CCAffineTransform.Concat(ref localTransform, ref parentWorldTransform, out worldTransform);

            if (isScale9Enabled)
            {
                for (var j = 0; j < protectedChildren.Count; j++)
                {
                    var node = protectedChildren[j] as CCSprite;

                    if (node != null)
                    {
                        node.Visit(ref worldTransform);
                    }
                    else
                        break;
                }
            }
            else
            {
                if (scale9Image != null)
                {
                    scale9Image.Visit(ref worldTransform);
                }
            }

            base.Visit(ref parentWorldTransform);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            foreach (var child in protectedChildren)
            {
                child.OnEnter();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            foreach (var child in protectedChildren)
            {
                child.OnExit();
            }
        }

        public override void OnEnterTransitionDidFinish()
        {
            base.OnEnterTransitionDidFinish();
            foreach (var child in protectedChildren)
            {
                child.OnEnterTransitionDidFinish();
            }
        }

        public override void OnExitTransitionDidStart()
        {
            base.OnExitTransitionDidStart();
            foreach (var child in protectedChildren)
            {
                child.OnExitTransitionDidStart();
            }

        }

        public override void UpdateDisplayedColor(CCColor3B parentColor)
        {

            var displayedColor = CCColor3B.White;
            displayedColor.R = (byte)(RealColor.R * parentColor.R / 255.0f);
            displayedColor.G = (byte)(RealColor.G * parentColor.G / 255.0f);
            displayedColor.B = (byte)(RealColor.B * parentColor.B / 255.0f);

            UpdateColor();

            if (scale9Image != null)
            {
                scale9Image.UpdateDisplayedColor(displayedColor);
            }

            foreach (var child in protectedChildren)
            {
                child.UpdateDisplayedColor(displayedColor);
            }

            if (IsColorCascaded && Children != null)
            {
                foreach (var child in Children)
                {
                    child.UpdateDisplayedColor(displayedColor);
                }
            }
        }

        protected override void UpdateDisplayedOpacity(byte parentOpacity)
        {
            var displayedOpacity = (byte)(RealOpacity * parentOpacity / 255.0f);

            base.UpdateDisplayedOpacity(displayedOpacity);

            if (scale9Image != null)
            {
                // Due to an accessor problem to UpdateDisplayedOpacity we will need
                // implement this afterwards
                //scale9Image.UpdateDisplayedOpacity(displayedOpacity);
                scale9Image.Opacity = displayedOpacity;
            }

            foreach (var child in protectedChildren)
            {
                // Due to an accessor problem to UpdateDisplayedOpacity we will need
                // implement this afterwards
                //child.UpdateDisplayedOpacity(displayedOpacity);
                child.Opacity = displayedOpacity;
            }

            if (IsOpacityCascaded && Children != null)
            {
                foreach (var child in Children)
                {
                    // Due to an accessor problem to UpdateDisplayedOpacity we will need
                    // implement this afterwards
                    //child.UpdateDisplayedOpacity(displayedOpacity);
                    child.Opacity = displayedOpacity;
                }
            }
        }

        private void CleanupSlicedSprites()
        {
            
        }

        void AddProtectedChild(CCNode child)
        {
            positionsAreDirty = true;
            protectedChildren.Add(child);
        }

        public bool UpdateWithSprite(CCSprite sprite, CCRect rect, bool rotated, CCRect capInsets)
        {
            return UpdateWithSprite(sprite, rect, rotated, CCPoint.Zero, rect.Size, capInsets);
        }

        public bool UpdateWithSprite(CCSprite sprite, CCRect textureRect, bool rotated, CCPoint offset,
            CCSize originalSize, CCRect capInsets)
        {
            var opacity = Opacity;
            var color = Color;

            CleanupSlicedSprites();
            protectedChildren.Clear();

            UpdateBlendFunc((sprite != null) ? sprite.Texture : null);

            if (sprite != null)
            {
                if (sprite.SpriteFrame == null)
                    return false;

                if (scale9Image == null)
                {
                    scale9Image = sprite;
                    scale9Image.RemoveAllChildren(true);
                }
                else
                {
                    scale9Image.SpriteFrame = sprite.SpriteFrame;
                }

            }

            if (scale9Image == null)
                return false;

            var spriteFrame = scale9Image.SpriteFrame;

            if (spriteFrame == null)
            {
                return false;
            }

            var rect = textureRect;
            var size = originalSize;

            this.capInsets = capInsets;

            // If there is no given rect
            if (rect == CCRect.Zero)
            {
                // Get the texture size as original
                CCSize textureSize = scale9Image.Texture.ContentSizeInPixels;

                rect.Origin.X = 0;
                rect.Origin.Y = 0;
                rect.Size.Width = textureSize.Width;
                rect.Size.Height = textureSize.Height;
            }

            if (size == CCSize.Zero)
                size = rect.Size;


            // Set the given rect's size as original size
            spriteRect = rect;
            this.offset = offset;
            spriteFrameRotated = rotated;
            this.originalSize = size;
            preferredSize = size;
            capInsetsInternal = capInsets;

            if (isScale9Enabled)
                CreateSlicedSprites();

            ApplyBlendFunc();

            ContentSize = size;

            if (spritesGenerated)
            {
                // Restore color and opacity
                Opacity = opacity;
                Color = color;
            }
            spritesGenerated = true;

            return true;
        }

        void CreateSlicedSprites()
        {

            float height = originalSize.Height;
            float width = originalSize.Width;

            CCPoint offsetPosition = CCPoint.Zero;
            offsetPosition.X = (float)System.Math.Ceiling(offset.X + (originalSize.Width - spriteRect.Size.Width) / 2);
            offsetPosition.Y = (float)System.Math.Ceiling(offset.Y + (originalSize.Height - spriteRect.Size.Height) / 2);

            // If there is no specified center region
            if (capInsetsInternal == CCRect.Zero)
            {
                capInsetsInternal = new CCRect(width / 3, height / 3, width / 3, height / 3);
            }

            var originalRect = spriteRect;
            if (spriteFrameRotated)
                originalRect = new CCRect(spriteRect.Origin.X - offsetPosition.Y,
                                    spriteRect.Origin.Y - offsetPosition.X,
                                    originalSize.Width, originalSize.Height);
            else
                originalRect = new CCRect(spriteRect.Origin.X - offsetPosition.Y,
                                    spriteRect.Origin.Y - offsetPosition.Y,
                                    originalSize.Width, originalSize.Height);

            float leftWidth = capInsetsInternal.Origin.X;
            float centerWidth = capInsetsInternal.Size.Width;
            float rightWidth = originalRect.Size.Width - (leftWidth + centerWidth);

            float topHeight = capInsetsInternal.Origin.Y;
            float centerHeight = capInsetsInternal.Size.Height;
            float bottomHeight = originalRect.Size.Height - (topHeight + centerHeight);

            // calculate rects

            // ... top row
            float x = 0.0f;
            float y = 0.0f;
            //why do we need pixelRect?
            var pixelRect = new CCRect(offsetPosition.X, offsetPosition.Y,
                                  spriteRect.Size.Width, spriteRect.Size.Height);

            // top left
            var leftTopBoundsOriginal = new CCRect(x, y, leftWidth, topHeight);
            var leftTopBounds = leftTopBoundsOriginal;

            // top center
            x += leftWidth;
            var centerTopBounds = new CCRect(x, y, centerWidth, topHeight);

            // top right
            x += centerWidth;
            var rightTopBounds = new CCRect(x, y, rightWidth, topHeight);

            // ... center row
            x = 0.0f;
            y = 0.0f;
            y += topHeight;

            // center left
            var leftCenterBounds = new CCRect(x, y, leftWidth, centerHeight);

            // center center
            x += leftWidth;
            var centerBoundsOriginal = new CCRect(x, y, centerWidth, centerHeight);
            var centerBounds = centerBoundsOriginal;

            // center right
            x += centerWidth;
            var rightCenterBounds = new CCRect(x, y, rightWidth, centerHeight);

            // ... bottom row
            x = 0.0f;
            y = 0.0f;
            y += topHeight;
            y += centerHeight;

            // bottom left
            var leftBottomBounds = new CCRect(x, y, leftWidth, bottomHeight);

            // bottom center
            x += leftWidth;
            var centerBottomBounds = new CCRect(x, y, centerWidth, bottomHeight);

            // bottom right
            x += centerWidth;
            var rightBottomBoundsOriginal = new CCRect(x, y, rightWidth, bottomHeight);
            var rightBottomBounds = rightBottomBoundsOriginal;

            if ((capInsetsInternal.Origin.X + capInsetsInternal.Size.Width) <= originalSize.Width
               || (capInsetsInternal.Origin.Y + capInsetsInternal.Size.Height) <= originalSize.Height)
            //in general case it is error but for legacy support we will check it
            {
                leftTopBounds = intersectRect(leftTopBounds, pixelRect);
                centerTopBounds = intersectRect(centerTopBounds, pixelRect);
                rightTopBounds = intersectRect(rightTopBounds, pixelRect);
                leftCenterBounds = intersectRect(leftCenterBounds, pixelRect);
                centerBounds = intersectRect(centerBounds, pixelRect);
                rightCenterBounds = intersectRect(rightCenterBounds, pixelRect);
                leftBottomBounds = intersectRect(leftBottomBounds, pixelRect);
                centerBottomBounds = intersectRect(centerBottomBounds, pixelRect);
                rightBottomBounds = intersectRect(rightBottomBounds, pixelRect);
            }
            else
                //it is error but for legacy turn off clip system
                CCLog.Log("Scale9Sprite capInsetsInternal > originalSize");

            var rotatedLeftTopBoundsOriginal = leftTopBoundsOriginal;
            var rotatedCenterBoundsOriginal = centerBoundsOriginal;
            var rotatedRightBottomBoundsOriginal = rightBottomBoundsOriginal;

            var rotatedCenterBounds = centerBounds;
            var rotatedRightBottomBounds = rightBottomBounds;
            var rotatedLeftBottomBounds = leftBottomBounds;
            var rotatedRightTopBounds = rightTopBounds;
            var rotatedLeftTopBounds = leftTopBounds;
            var rotatedRightCenterBounds = rightCenterBounds;
            var rotatedLeftCenterBounds = leftCenterBounds;
            var rotatedCenterBottomBounds = centerBottomBounds;
            var rotatedCenterTopBounds = centerTopBounds;

            if (!spriteFrameRotated)
            {

                CCAffineTransform t = CCAffineTransform.Identity;
                t = CCAffineTransform.Translate(t, originalRect.Origin.X, originalRect.Origin.Y);

                rotatedLeftTopBoundsOriginal = CCAffineTransform.Transform(rotatedLeftTopBoundsOriginal, t);
                rotatedCenterBoundsOriginal = CCAffineTransform.Transform(rotatedCenterBoundsOriginal, t);
                rotatedRightBottomBoundsOriginal = CCAffineTransform.Transform(rotatedRightBottomBoundsOriginal, t);

                rotatedCenterBounds = CCAffineTransform.Transform(rotatedCenterBounds, t);
                rotatedRightBottomBounds = CCAffineTransform.Transform(rotatedRightBottomBounds, t);
                rotatedLeftBottomBounds = CCAffineTransform.Transform(rotatedLeftBottomBounds, t);
                rotatedRightTopBounds = CCAffineTransform.Transform(rotatedRightTopBounds, t);
                rotatedLeftTopBounds = CCAffineTransform.Transform(rotatedLeftTopBounds, t);
                rotatedRightCenterBounds = CCAffineTransform.Transform(rotatedRightCenterBounds, t);
                rotatedLeftCenterBounds = CCAffineTransform.Transform(rotatedLeftCenterBounds, t);
                rotatedCenterBottomBounds = CCAffineTransform.Transform(rotatedCenterBottomBounds, t);
                rotatedCenterTopBounds = CCAffineTransform.Transform(rotatedCenterTopBounds, t);

            }
            else
            {
                // set up transformation of coordinates
                // to handle the case where the sprite is stored rotated
                // in the spritesheet
                // log("rotated");

                CCAffineTransform t = CCAffineTransform.Identity;
                t = CCAffineTransform.Translate(t, originalRect.Size.Height + originalRect.Origin.X,
                    originalRect.Origin.Y);

                t = CCAffineTransform.Rotate(t, 1.57079633f);

                leftTopBoundsOriginal = CCAffineTransform.Transform(leftTopBoundsOriginal, t);
                centerBoundsOriginal = CCAffineTransform.Transform(centerBoundsOriginal, t);
                rightBottomBoundsOriginal = CCAffineTransform.Transform(rightBottomBoundsOriginal, t);

                centerBounds = CCAffineTransform.Transform(centerBounds, t);
                rightBottomBounds = CCAffineTransform.Transform(rightBottomBounds, t);
                leftBottomBounds = CCAffineTransform.Transform(leftBottomBounds, t);
                rightTopBounds = CCAffineTransform.Transform(rightTopBounds, t);
                leftTopBounds = CCAffineTransform.Transform(leftTopBounds, t);
                rightCenterBounds = CCAffineTransform.Transform(rightCenterBounds, t);
                leftCenterBounds = CCAffineTransform.Transform(leftCenterBounds, t);
                centerBottomBounds = CCAffineTransform.Transform(centerBottomBounds, t);
                centerTopBounds = CCAffineTransform.Transform(centerTopBounds, t);

                rotatedLeftTopBoundsOriginal.Origin = leftTopBoundsOriginal.Origin;
                rotatedCenterBoundsOriginal.Origin = centerBoundsOriginal.Origin;
                rotatedRightBottomBoundsOriginal.Origin = rightBottomBoundsOriginal.Origin;

                rotatedCenterBounds.Origin = centerBounds.Origin;
                rotatedRightBottomBounds.Origin = rightBottomBounds.Origin;
                rotatedLeftBottomBounds.Origin = leftBottomBounds.Origin;
                rotatedRightTopBounds.Origin = rightTopBounds.Origin;
                rotatedLeftTopBounds.Origin = leftTopBounds.Origin;
                rotatedRightCenterBounds.Origin = rightCenterBounds.Origin;
                rotatedLeftCenterBounds.Origin = leftCenterBounds.Origin;
                rotatedCenterBottomBounds.Origin = centerBottomBounds.Origin;
                rotatedCenterTopBounds.Origin = centerTopBounds.Origin;

            }

            topLeftSize = rotatedLeftTopBoundsOriginal.Size;
            centerSize = rotatedCenterBoundsOriginal.Size;
            bottomRightSize = rotatedRightBottomBoundsOriginal.Size;

            if (spriteFrameRotated)
            {
                float offsetX = (rotatedCenterBounds.Origin.X + rotatedCenterBounds.Size.Height / 2)
                    - (rotatedCenterBoundsOriginal.Origin.X + rotatedCenterBoundsOriginal.Size.Height / 2);
                float offsetY = (rotatedCenterBoundsOriginal.Origin.Y + rotatedCenterBoundsOriginal.Size.Width / 2)
                    - (rotatedCenterBounds.Origin.Y + rotatedCenterBounds.Size.Width / 2);
                centerOffset.X = -offsetY;
                centerOffset.Y = offsetX;
            }
            else
            {
                float offsetX = (rotatedCenterBounds.Origin.X + rotatedCenterBounds.Size.Width / 2)
                    - (rotatedCenterBoundsOriginal.Origin.X + rotatedCenterBoundsOriginal.Size.Width / 2);
                float offsetY = (rotatedCenterBoundsOriginal.Origin.Y + rotatedCenterBoundsOriginal.Size.Height / 2)
                    - (rotatedCenterBounds.Origin.Y + rotatedCenterBounds.Size.Height / 2);
                centerOffset.X = offsetX;
                centerOffset.Y = offsetY;
            }

            // Centre
            if (rotatedCenterBounds.Size.Width > 0 && rotatedCenterBounds.Size.Height > 0)
            {
                centerSprite = new CCSprite(scale9Image.Texture,
                                                          rotatedCenterBounds,
                                                          spriteFrameRotated);
                AddProtectedChild(centerSprite);
            }

            // Top
            if (rotatedCenterTopBounds.Size.Width > 0 && rotatedCenterTopBounds.Size.Height > 0)
            {
                topSprite = new CCSprite(scale9Image.Texture,
                                                       rotatedCenterTopBounds,
                                                       spriteFrameRotated);
                AddProtectedChild(topSprite);
            }

            // Bottom
            if (rotatedCenterBottomBounds.Size.Width > 0 && rotatedCenterBottomBounds.Size.Height > 0)
            {
                bottomSprite = new CCSprite(scale9Image.Texture,
                                                          rotatedCenterBottomBounds,
                                                          spriteFrameRotated);
                AddProtectedChild(bottomSprite);
            }

            // Left
            if (rotatedLeftCenterBounds.Size.Width > 0 && rotatedLeftCenterBounds.Size.Height > 0)
            {
                leftSprite = new CCSprite(scale9Image.Texture,
                                                        rotatedLeftCenterBounds,
                                                        spriteFrameRotated);
                AddProtectedChild(leftSprite);
            }

            // Right
            if (rotatedRightCenterBounds.Size.Width > 0 && rotatedRightCenterBounds.Size.Height > 0)
            {
                rightSprite = new CCSprite(scale9Image.Texture,
                                                         rotatedRightCenterBounds,
                                                         spriteFrameRotated);
                AddProtectedChild(rightSprite);
            }

            // Top left
            if (rotatedLeftTopBounds.Size.Width > 0 && rotatedLeftTopBounds.Size.Height > 0)
            {
                topLeftSprite = new CCSprite(scale9Image.Texture,
                                                           rotatedLeftTopBounds,
                                                           spriteFrameRotated);
                AddProtectedChild(topLeftSprite);
            }

            // Top right
            if (rotatedRightTopBounds.Size.Width > 0 && rotatedRightTopBounds.Size.Height > 0)
            {
                topRightSprite = new CCSprite(scale9Image.Texture,
                                                            rotatedRightTopBounds,
                                                            spriteFrameRotated);
                AddProtectedChild(topRightSprite);
            }

            // Bottom left
            if (rotatedLeftBottomBounds.Size.Width > 0 && rotatedLeftBottomBounds.Size.Height > 0)
            {
                bottomLeftSprite = new CCSprite(scale9Image.Texture,
                                                              rotatedLeftBottomBounds,
                                                              spriteFrameRotated);
                AddProtectedChild(bottomLeftSprite);
            }

            // Bottom right
            if (rotatedRightBottomBounds.Size.Width > 0 && rotatedRightBottomBounds.Size.Height > 0)
            {
                bottomRightSprite = new CCSprite(scale9Image.Texture,
                                                               rotatedRightBottomBounds,
                                                               spriteFrameRotated);
                AddProtectedChild(bottomRightSprite);
            }
        }

        static CCRect intersectRect(CCRect first, CCRect second)
        {
            CCRect ret;
            ret.Origin.X = (float)System.Math.Max(first.Origin.X, second.Origin.X);
            ret.Origin.Y = (float)System.Math.Max(first.Origin.Y, second.Origin.Y);

            float rightRealPoint = (float)System.Math.Min(first.Origin.X + first.Size.Width, second.Origin.X + second.Size.Width);
            float bottomRealPoint = (float)System.Math.Min(first.Origin.Y + first.Size.Height, second.Origin.Y + second.Size.Height);

            ret.Size.Width = (float)System.Math.Max(rightRealPoint - ret.Origin.X, 0.0f);
            ret.Size.Height = (float)System.Math.Max(bottomRealPoint - ret.Origin.Y, 0.0f);
            return ret;
        }

        void ApplyBlendFunc()
        {
            if (scale9Image != null)
                scale9Image.BlendFunc = _blendFunc;
            if (topLeftSprite != null)
                topLeftSprite.BlendFunc = _blendFunc;
            if (topSprite != null)
                topSprite.BlendFunc = _blendFunc;
            if (topRightSprite != null)
                topRightSprite.BlendFunc = _blendFunc;
            if (leftSprite != null)
                leftSprite.BlendFunc = _blendFunc;
            if (centerSprite != null)
                centerSprite.BlendFunc = _blendFunc;
            if (rightSprite != null)
                rightSprite.BlendFunc = _blendFunc;
            if (bottomLeftSprite != null)
                bottomLeftSprite.BlendFunc = _blendFunc;
            if (bottomSprite != null)
                bottomSprite.BlendFunc = _blendFunc;
            if (bottomRightSprite != null)
                bottomRightSprite.BlendFunc = _blendFunc;
        }

        void UpdateBlendFunc(CCTexture2D texture)
        {

            if (texture == null)
            {
                _blendFunc = CCBlendFunc.AlphaBlend;
                opacityModifyRGB = true;
            }
            else
            {
                // it is possible to have an untextured sprite
                if (!texture.HasPremultipliedAlpha)
                {
                    _blendFunc = CCBlendFunc.NonPremultiplied;
                    opacityModifyRGB = false;
                }
                else
                {
                    _blendFunc = CCBlendFunc.AlphaBlend;
                    opacityModifyRGB = true;
                }
            }
        }

        protected void UpdatePositions()
        {

            CCSize size = ContentSize;

            float sizableWidth = size.Width - topLeftSize.Width - bottomRightSize.Width;
            float sizableHeight = size.Height - topLeftSize.Height - bottomRightSize.Height;

            float horizontalScale = sizableWidth / centerSize.Width;
            float verticalScale = sizableHeight / centerSize.Height;

            if (centerSprite != null)
            {
                centerSprite.ScaleX = horizontalScale;
                centerSprite.ScaleY = verticalScale;
            }

            float rescaledWidth = centerSize.Width * horizontalScale;
            float rescaledHeight = centerSize.Height * verticalScale;

            float leftWidth = topLeftSize.Width;
            float bottomHeight = bottomRightSize.Height;

            centerOffset.X = centerOffset.X * horizontalScale;
            centerOffset.Y = centerOffset.Y * horizontalScale;

            // Position corners
            if (bottomLeftSprite != null)
            {
                bottomLeftSprite.AnchorPoint = CCPoint.AnchorUpperRight;
                bottomLeftSprite.PositionX = leftWidth;
                bottomLeftSprite.PositionY = bottomHeight;
                
            }
            if (bottomRightSprite != null)
            {
                bottomRightSprite.AnchorPoint = CCPoint.AnchorUpperLeft;
                bottomRightSprite.PositionX = leftWidth + rescaledWidth;
                bottomRightSprite.PositionY = bottomHeight;
            }
            if (topLeftSprite != null)
            {
                topLeftSprite.AnchorPoint = CCPoint.AnchorLowerRight;
                topLeftSprite.PositionX = leftWidth;
                topLeftSprite.PositionY = bottomHeight + rescaledHeight;
            }
            if (topRightSprite != null)
            {
                topRightSprite.AnchorPoint = CCPoint.AnchorLowerLeft;
                topRightSprite.PositionX = leftWidth + rescaledWidth;
                topRightSprite.PositionY = bottomHeight + rescaledHeight;
            }

            // Scale and position borders
            if (leftSprite != null)
            {
                leftSprite.AnchorPoint = CCPoint.AnchorMiddleRight;
                leftSprite.PositionX = leftWidth;
                leftSprite.PositionY = bottomHeight + rescaledHeight / 2 + centerOffset.Y;
                leftSprite.ScaleY = verticalScale;
            }
            if (rightSprite != null)
            {
                rightSprite.AnchorPoint = CCPoint.AnchorMiddleLeft;
                rightSprite.PositionX = leftWidth + rescaledWidth;
                rightSprite.PositionY = bottomHeight + rescaledHeight / 2 + centerOffset.Y;
                rightSprite.ScaleY = verticalScale;
            }
            if (topSprite != null)
            {
                topSprite.AnchorPoint = CCPoint.AnchorMiddleBottom;
                topSprite.PositionX = leftWidth + rescaledWidth / 2 + centerOffset.X;
                topSprite.PositionY = bottomHeight + rescaledHeight;
                topSprite.ScaleX = horizontalScale;
            }
            if (bottomSprite != null)
            {
                bottomSprite.AnchorPoint = CCPoint.AnchorMiddleTop;
                bottomSprite.PositionX = leftWidth + rescaledWidth / 2 + centerOffset.X;
                bottomSprite.PositionY = bottomHeight;
                bottomSprite.ScaleX = horizontalScale;
            }
            // Position centre
            if (centerSprite != null)
            {
                centerSprite.AnchorPoint = CCPoint.AnchorMiddle;
                centerSprite.PositionX = leftWidth + rescaledWidth / 2 + centerOffset.X;
                centerSprite.PositionY = bottomHeight + rescaledHeight / 2 + centerOffset.Y;
                centerSprite.ScaleX = horizontalScale;
                centerSprite.ScaleY = verticalScale;
            }

        }

        void AdjustScale9ImagePosition()
        {
            if (scale9Image != null)
            {
                scale9Image.PositionX = ContentSize.Width * scale9Image.AnchorPoint.X;
                scale9Image.PositionY = ContentSize.Height * scale9Image.AnchorPoint.Y;
            }
        }

        protected void UpdateCapInset()
        {
            CCRect insets;
            if (insetLeft == 0 && insetTop == 0 && insetRight == 0 && insetBottom == 0)
            {
                insets = CCRect.Zero;
            }
            else
            {
                insets.Origin.X = insetLeft;
                insets.Origin.Y = insetTop;
                insets.Size.Width = originalSize.Width - insetLeft - insetRight;
                insets.Size.Height = originalSize.Height - insetTop - insetBottom;
            }
            CapInsets = insets;
        }
    }
}