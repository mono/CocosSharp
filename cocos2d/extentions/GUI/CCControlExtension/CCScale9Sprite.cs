using System.Diagnostics;

namespace cocos2d
{
    /// <summary>
    /// This is a special sprite container that represents a 9 point sprite region, where 8 of hte
    /// points are along the perimeter, and the 9th is the center area. This special sprite is capable of resizing
    /// itself to arbitrary scales.
    /// </summary>
    public class CCScale9Sprite : CCNode, ICCRGBAProtocol
    {
        protected CCSprite bottom;
        protected CCSprite bottomLeft;
        protected CCSprite bottomRight;
        protected CCSprite centre;
        protected CCSprite left;

        protected bool m_bIsOpacityModifyRGB;
        protected bool m_bSpriteFrameRotated;
        protected bool m_bSpritesGenerated;
        protected byte m_cOpacity;

        /** 
        * The end-cap insets. 
        * On a non-resizeable sprite, this property is set to CGRectZero; the sprite 
        * does not use end caps and the entire sprite is subject to stretching. 
        */
        private CCRect m_capInsets;
        protected CCRect m_capInsetsInternal;
        private float m_insetBottom;
        private float m_insetLeft;
        private float m_insetRight;
        private float m_insetTop;
        private CCSize m_originalSize;
        protected bool m_positionsAreDirty;
        private CCSize m_preferredSize;
        protected CCColor3B m_sColorUnmodified;
        protected CCRect m_spriteRect;
        protected CCColor3B m_tColor;
        protected CCSprite right;
        protected CCSpriteBatchNode scale9Image;
        protected CCSprite top;
        protected CCSprite topLeft;
        protected CCSprite topRight;

        public override CCSize ContentSize
        {
            get { return base.ContentSize; }
            set
            {
                base.ContentSize = value;
                m_positionsAreDirty = true;
            }
        }

        public CCSize PreferredSize
        {
            get { return m_preferredSize; }
            set
            {
                ContentSize = value;
                m_preferredSize = value;
            }
        }

        public CCRect CapInsets
        {
            get { return m_capInsets; }
            set
            {
                CCSize cs = m_tContentSize;
                UpdateWithBatchNode(scale9Image, m_spriteRect, m_bSpriteFrameRotated, value);
                ContentSize = cs;
            }
        }

        public float InsetLeft
        {
            set
            {
                m_insetLeft = value;
                UpdateCapInset();
            }
            get { return m_insetLeft; }
        }

        public float InsetTop
        {
            set
            {
                m_insetTop = value;
                UpdateCapInset();
            }
            get { return m_insetTop; }
        }

        public float InsetRight
        {
            set
            {
                m_insetRight = value;
                UpdateCapInset();
            }
            get { return m_insetRight; }
        }

        public float InsetBottom
        {
            set
            {
                m_insetBottom = value;
                UpdateCapInset();
            }
            get { return m_insetBottom; }
        }

        #region ICCRGBAProtocol Members

        public CCColor3B Color
        {
            get { return m_tColor; }
            set
            {
                m_tColor = value;
                if (scale9Image != null && scale9Image.Children != null && scale9Image.Children.count != 0)
                {
                    for (int i = 0; i < scale9Image.Children.count; i++)
                    {
                        var prot = scale9Image.Children[i] as ICCRGBAProtocol;
                        if (prot != null)
                        {
                            prot.Color = value;
                        }
                    }
                }
            }
        }

        public byte Opacity
        {
            get { return m_cOpacity; }
            set
            {
                m_cOpacity = value;
                if (scale9Image != null && scale9Image.Children != null && scale9Image.Children.count != 0)
                {
                    for (int i = 0; i < scale9Image.Children.count; i++)
                    {
                        var prot = scale9Image.Children[i] as ICCRGBAProtocol;
                        if (prot != null)
                        {
                            prot.Opacity = value;
                        }
                    }
                }
            }
        }

        public bool IsOpacityModifyRGB
        {
            get { return m_bIsOpacityModifyRGB; }
            set
            {
                m_bIsOpacityModifyRGB = value;
                if (scale9Image != null && scale9Image.Children != null && scale9Image.Children.count != 0)
                {
                    for (int i = 0; i < scale9Image.Children.count; i++)
                    {
                        var prot = scale9Image.Children[i] as ICCRGBAProtocol;
                        if (prot != null)
                        {
                            prot.IsOpacityModifyRGB = value;
                        }
                    }
                }
            }
        }

        #endregion

        public virtual bool Init()
        {
            return InitWithBatchNode(null, CCRect.Zero, CCRect.Zero);
        }

        public bool InitWithBatchNode(CCSpriteBatchNode batchnode, CCRect rect, CCRect capInsets)
        {
            return InitWithBatchNode(batchnode, rect, false, capInsets);
        }

        public bool InitWithBatchNode(CCSpriteBatchNode batchnode, CCRect rect, bool rotated, CCRect capInsets)
        {
            if (batchnode != null)
            {
                UpdateWithBatchNode(batchnode, rect, rotated, capInsets);
                AnchorPoint = new CCPoint(0.5f, 0.5f);
            }
            m_positionsAreDirty = true;

            return true;
        }

        public bool UpdateWithBatchNode(CCSpriteBatchNode batchnode, CCRect rect, bool rotated, CCRect capInsets)
        {
            byte opacity = m_cOpacity;
            CCColor3B color = m_tColor;

            // Release old sprites
            RemoveAllChildrenWithCleanup(true);

            if (scale9Image != batchnode)
            {
                scale9Image = batchnode;
            }

            scale9Image.RemoveAllChildrenWithCleanup(true);

            m_capInsets = capInsets;

            // If there is no given rect
            if (rect.Equals(CCRect.Zero))
            {
                // Get the texture size as original
                CCSize textureSize = scale9Image.TextureAtlas.Texture.ContentSize;

                rect = new CCRect(0, 0, textureSize.Width, textureSize.Height);
            }

            // Set the given rect's size as original size
            m_spriteRect = rect;
            m_originalSize = rect.Size;
            m_preferredSize = m_originalSize;
            m_capInsetsInternal = capInsets;

            // Get the image edges
            float l = rect.Origin.X;
            float t = rect.Origin.Y;
            float h = rect.Size.Height;
            float w = rect.Size.Width;

            // If there is no specified center region
            if (m_capInsetsInternal.Equals(CCRect.Zero))
            {
                // Apply the 3x3 grid format
                if (rotated)
                {
                    m_capInsetsInternal = new CCRect(l + h / 3, t + w / 3, w / 3, h / 3);
                }
                else
                {
                    m_capInsetsInternal = new CCRect(l + w / 3, t + h / 3, w / 3, h / 3);
                }
            }

            //
            // Set up the image
            //
            if (rotated)
            {
                // Sprite frame is rotated

                // Centre
                centre = new CCSprite();
                centre.InitWithTexture(scale9Image.Texture, m_capInsetsInternal, true);
                scale9Image.AddChild(centre, 0, (int) Positions.pCentre);

                // Bottom
                bottom = new CCSprite();
                bottom.InitWithTexture(scale9Image.Texture, new CCRect(l,
                                                                       m_capInsetsInternal.Origin.Y,
                                                                       m_capInsetsInternal.Size.Width,
                                                                       m_capInsetsInternal.Origin.X - l),
                                       rotated
                    );
                scale9Image.AddChild(bottom, 1, (int) Positions.pBottom);

                // Top
                top = new CCSprite();
                top.InitWithTexture(scale9Image.Texture, new CCRect(m_capInsetsInternal.Origin.X + m_capInsetsInternal.Size.Height,
                                                                    m_capInsetsInternal.Origin.Y,
                                                                    m_capInsetsInternal.Size.Width,
                                                                    h - m_capInsetsInternal.Size.Height - (m_capInsetsInternal.Origin.X - l)),
                                    rotated
                    );
                scale9Image.AddChild(top, 1, (int) Positions.pTop);

                // Right
                right = new CCSprite();
                right.InitWithTexture(scale9Image.Texture, new CCRect(m_capInsetsInternal.Origin.X,
                                                                      m_capInsetsInternal.Origin.Y + m_capInsetsInternal.Size.Width,
                                                                      w - (m_capInsetsInternal.Origin.Y - t) - m_capInsetsInternal.Size.Width,
                                                                      m_capInsetsInternal.Size.Height),
                                      rotated
                    );
                scale9Image.AddChild(right, 1, (int) Positions.pRight);

                // Left
                left = new CCSprite();
                left.InitWithTexture(scale9Image.Texture, new CCRect(m_capInsetsInternal.Origin.X,
                                                                     t,
                                                                     m_capInsetsInternal.Origin.Y - t,
                                                                     m_capInsetsInternal.Size.Height),
                                     rotated
                    );
                scale9Image.AddChild(left, 1, (int) Positions.pLeft);

                // Top right
                topRight = new CCSprite();
                topRight.InitWithTexture(scale9Image.Texture, new CCRect(m_capInsetsInternal.Origin.X + m_capInsetsInternal.Size.Height,
                                                                         m_capInsetsInternal.Origin.Y + m_capInsetsInternal.Size.Width,
                                                                         w - (m_capInsetsInternal.Origin.Y - t) - m_capInsetsInternal.Size.Width,
                                                                         h - m_capInsetsInternal.Size.Height - (m_capInsetsInternal.Origin.X - l)),
                                         rotated
                    );
                scale9Image.AddChild(topRight, 2, (int) Positions.pTopRight);

                // Top left
                topLeft = new CCSprite();
                topLeft.InitWithTexture(scale9Image.Texture, new CCRect(m_capInsetsInternal.Origin.X + m_capInsetsInternal.Size.Height,
                                                                        t,
                                                                        m_capInsetsInternal.Origin.Y - t,
                                                                        h - m_capInsetsInternal.Size.Height - (m_capInsetsInternal.Origin.X - l)),
                                        rotated
                    );
                scale9Image.AddChild(topLeft, 2, (int) Positions.pTopLeft);

                // Bottom right
                bottomRight = new CCSprite();
                bottomRight.InitWithTexture(scale9Image.Texture, new CCRect(l,
                                                                            m_capInsetsInternal.Origin.Y + m_capInsetsInternal.Size.Width,
                                                                            w - (m_capInsetsInternal.Origin.Y - t) - m_capInsetsInternal.Size.Width,
                                                                            m_capInsetsInternal.Origin.X - l),
                                            rotated
                    );
                scale9Image.AddChild(bottomRight, 2, (int) Positions.pBottomRight);

                // Bottom left
                bottomLeft = new CCSprite();
                bottomLeft.InitWithTexture(scale9Image.Texture, new CCRect(l,
                                                                           t,
                                                                           m_capInsetsInternal.Origin.Y - t,
                                                                           m_capInsetsInternal.Origin.X - l),
                                           rotated
                    );
                scale9Image.AddChild(bottomLeft, 2, (int) Positions.pBottomLeft);
            }
            else
            {
                // Sprite frame is not rotated
                // Centre
                centre = new CCSprite();
                centre.InitWithTexture(scale9Image.Texture, m_capInsetsInternal, rotated);
                scale9Image.AddChild(centre, 0, (int) Positions.pCentre);

                // Top
                top = new CCSprite();
                top.InitWithTexture(scale9Image.Texture, new CCRect(m_capInsetsInternal.Origin.X,
                                                                    t,
                                                                    m_capInsetsInternal.Size.Width,
                                                                    m_capInsetsInternal.Origin.Y - t),
                                    rotated
                    );
                scale9Image.AddChild(top, 1, (int) Positions.pTop);

                // Bottom
                bottom = new CCSprite();
                bottom.InitWithTexture(scale9Image.Texture, new CCRect(m_capInsetsInternal.Origin.X,
                                                                       m_capInsetsInternal.Origin.Y + m_capInsetsInternal.Size.Height,
                                                                       m_capInsetsInternal.Size.Width,
                                                                       h - (m_capInsetsInternal.Origin.Y - t + m_capInsetsInternal.Size.Height)),
                                       rotated);
                scale9Image.AddChild(bottom, 1, (int) Positions.pBottom);

                // Left
                left = new CCSprite();
                left.InitWithTexture(scale9Image.Texture, new CCRect(
                                                              l,
                                                              m_capInsetsInternal.Origin.Y,
                                                              m_capInsetsInternal.Origin.X - l,
                                                              m_capInsetsInternal.Size.Height),
                                     rotated);
                scale9Image.AddChild(left, 1, (int) Positions.pLeft);

                // Right
                right = new CCSprite();
                right.InitWithTexture(scale9Image.Texture, new CCRect(
                                                               m_capInsetsInternal.Origin.X + m_capInsetsInternal.Size.Width,
                                                               m_capInsetsInternal.Origin.Y,
                                                               w - (m_capInsetsInternal.Origin.X - l + m_capInsetsInternal.Size.Width),
                                                               m_capInsetsInternal.Size.Height),
                                      rotated);
                scale9Image.AddChild(right, 1, (int) Positions.pRight);

                // Top left
                topLeft = new CCSprite();
                topLeft.InitWithTexture(scale9Image.Texture, new CCRect(
                                                                 l,
                                                                 t,
                                                                 m_capInsetsInternal.Origin.X - l,
                                                                 m_capInsetsInternal.Origin.Y - t),
                                        rotated);

                scale9Image.AddChild(topLeft, 2, (int) Positions.pTopLeft);

                // Top right
                topRight = new CCSprite();
                topRight.InitWithTexture(scale9Image.Texture, new CCRect(
                                                                  m_capInsetsInternal.Origin.X + m_capInsetsInternal.Size.Width,
                                                                  t,
                                                                  w - (m_capInsetsInternal.Origin.X - l + m_capInsetsInternal.Size.Width),
                                                                  m_capInsetsInternal.Origin.Y - t),
                                         rotated);

                scale9Image.AddChild(topRight, 2, (int) Positions.pTopRight);

                // Bottom left
                bottomLeft = new CCSprite();
                bottomLeft.InitWithTexture(scale9Image.Texture, new CCRect(
                                                                    l,
                                                                    m_capInsetsInternal.Origin.Y + m_capInsetsInternal.Size.Height,
                                                                    m_capInsetsInternal.Origin.X - l,
                                                                    h - (m_capInsetsInternal.Origin.Y - t + m_capInsetsInternal.Size.Height)),
                                           rotated);
                scale9Image.AddChild(bottomLeft, 2, (int) Positions.pBottomLeft);

                // Bottom right
                bottomRight = new CCSprite();
                bottomRight.InitWithTexture(scale9Image.Texture, new CCRect(
                                                                     m_capInsetsInternal.Origin.X + m_capInsetsInternal.Size.Width,
                                                                     m_capInsetsInternal.Origin.Y + m_capInsetsInternal.Size.Height,
                                                                     w - (m_capInsetsInternal.Origin.X - l + m_capInsetsInternal.Size.Width),
                                                                     h - (m_capInsetsInternal.Origin.Y - t + m_capInsetsInternal.Size.Height)),
                                            rotated);
                scale9Image.AddChild(bottomRight, 2, (int) Positions.pBottomRight);
            }

            ContentSize = rect.Size;
            AddChild(scale9Image);

            if (m_bSpritesGenerated)
            {
                // Restore color and opacity
                Opacity = opacity;
                Color = color;
            }
            m_bSpritesGenerated = true;

            return true;
        }

        protected void UpdatePositions()
        {
            CCSize size = m_tContentSize;

            float sizableWidth = size.Width - topLeft.ContentSize.Width - topRight.ContentSize.Width;
            float sizableHeight = size.Height - topLeft.ContentSize.Height - bottomRight.ContentSize.Height;

            float horizontalScale = sizableWidth / centre.ContentSize.Width;
            float verticalScale = sizableHeight / centre.ContentSize.Height;

            centre.ScaleX = horizontalScale;
            centre.ScaleY = verticalScale;

            float rescaledWidth = centre.ContentSize.Width * horizontalScale;
            float rescaledHeight = centre.ContentSize.Height * verticalScale;

            float leftWidth = bottomLeft.ContentSize.Width;
            float bottomHeight = bottomLeft.ContentSize.Height;

            bottomLeft.AnchorPoint = CCPoint.Zero;
            bottomRight.AnchorPoint = CCPoint.Zero;
            topLeft.AnchorPoint = CCPoint.Zero;
            topRight.AnchorPoint = CCPoint.Zero;
            left.AnchorPoint = CCPoint.Zero;
            right.AnchorPoint = CCPoint.Zero;
            top.AnchorPoint = CCPoint.Zero;
            bottom.AnchorPoint = CCPoint.Zero;
            centre.AnchorPoint = CCPoint.Zero;

            // Position corners
            bottomLeft.Position = CCPoint.Zero;
            bottomRight.Position = new CCPoint(leftWidth + rescaledWidth, 0);
            topLeft.Position = new CCPoint(0, bottomHeight + rescaledHeight);
            topRight.Position = new CCPoint(leftWidth + rescaledWidth, bottomHeight + rescaledHeight);

            // Scale and position borders
            left.Position = new CCPoint(0, bottomHeight);
            left.ScaleY = verticalScale;
            right.Position = new CCPoint(leftWidth + rescaledWidth, bottomHeight);
            right.ScaleY = verticalScale;
            bottom.Position = new CCPoint(leftWidth, 0);
            bottom.ScaleX = horizontalScale;
            top.Position = new CCPoint(leftWidth, bottomHeight + rescaledHeight);
            top.ScaleX = horizontalScale;

            // Position centre
            centre.Position = new CCPoint(leftWidth, bottomHeight);
        }

        #region File Init Methods
        internal virtual bool InitWithFile(string file, CCRect rect, CCRect capInsets)
        {
            Debug.Assert(!string.IsNullOrEmpty(file), "Invalid file for sprite");

            CCSpriteBatchNode batchnode = new CCSpriteBatchNode(file, 9);
            bool pReturn = InitWithBatchNode(batchnode, rect, capInsets);
            return pReturn;
        }

        internal virtual bool InitWithFile(string file, CCRect rect)
        {
            Debug.Assert(!string.IsNullOrEmpty(file), "Invalid file for sprite");
            bool pReturn = InitWithFile(file, rect, CCRect.Zero);
            return pReturn;
        }

        internal virtual bool InitWithFile(CCRect capInsets, string file)
        {
            bool pReturn = InitWithFile(file, CCRect.Zero, capInsets);
            return pReturn;
        }

        internal virtual bool InitWithFile(string file)
        {
            bool pReturn = InitWithFile(file, CCRect.Zero);
            return pReturn;
        }
        #endregion

        #region SpriteFrame Methods

        internal virtual bool InitWithSpriteFrame(CCSpriteFrame spriteFrame, CCRect capInsets)
        {
            Debug.Assert(spriteFrame != null, "Sprite frame must be not nil");

            CCSpriteBatchNode batchnode = new CCSpriteBatchNode(spriteFrame.Texture, 9);
            bool pReturn = InitWithBatchNode(batchnode, spriteFrame.Rect, spriteFrame.IsRotated, capInsets);
            return pReturn;
        }

        internal virtual bool InitWithSpriteFrame(CCSpriteFrame spriteFrame)
        {
            Debug.Assert(spriteFrame != null, "Invalid spriteFrame for sprite");
            bool pReturn = InitWithSpriteFrame(spriteFrame, CCRect.Zero);
            return pReturn;
        }

        internal virtual bool InitWithSpriteFrameName(string spriteFrameName, CCRect capInsets)
        {
            Debug.Assert(spriteFrameName != null, "Invalid spriteFrameName for sprite");

            CCSpriteFrame frame = CCSpriteFrameCache.SharedSpriteFrameCache.SpriteFrameByName(spriteFrameName);
            bool pReturn = InitWithSpriteFrame(frame, capInsets);
            return pReturn;
        }

        internal virtual bool InitWithSpriteFrameName(string spriteFrameName)
        {
            bool pReturn = InitWithSpriteFrameName(spriteFrameName, CCRect.Zero);
            return pReturn;
        }

        #endregion

        public CCScale9Sprite(CCRect capInsets)
        {
            InitWithBatchNode(scale9Image, m_spriteRect, capInsets);
        }

        public CCScale9Sprite()
        {
            Init();
        }

        protected void UpdateCapInset()
        {
            CCRect insets;
            if (m_insetLeft == 0 && m_insetTop == 0 && m_insetRight == 0 && m_insetBottom == 0)
            {
                insets = CCRect.Zero;
            }
            else
            {
                if (m_bSpriteFrameRotated)
                {
                    insets = new CCRect(m_spriteRect.Origin.X + m_insetBottom,
                                        m_spriteRect.Origin.Y + m_insetLeft,
                                        m_spriteRect.Size.Width - m_insetRight - m_insetLeft,
                                        m_spriteRect.Size.Height - m_insetTop - m_insetBottom);
                }
                else
                {
                    insets = new CCRect(m_spriteRect.Origin.X + m_insetLeft,
                                        m_spriteRect.Origin.Y + m_insetTop,
                                        m_spriteRect.Size.Width - m_insetLeft - m_insetRight,
                                        m_spriteRect.Size.Height - m_insetTop - m_insetBottom);
                }
            }
            CapInsets = insets;
        }


        public void SetSpriteFrame(CCSpriteFrame spriteFrame)
        {
            CCSpriteBatchNode batchnode = new CCSpriteBatchNode(spriteFrame.Texture, 9);
            UpdateWithBatchNode(batchnode, spriteFrame.Rect, spriteFrame.IsRotated, CCRect.Zero);

            // Reset insets
            m_insetLeft = 0;
            m_insetTop = 0;
            m_insetRight = 0;
            m_insetBottom = 0;
        }

        public override void Visit()
        {
            if (m_positionsAreDirty)
            {
                UpdatePositions();
                m_positionsAreDirty = false;
            }
            base.Visit();
        }

        #region Nested type: Positions

        private enum Positions
        {
            pCentre = 0,
            pTop,
            pLeft,
            pRight,
            pBottom,
            pTopRight,
            pTopLeft,
            pBottomRight,
            pBottomLeft
        };

        #endregion
    }
}