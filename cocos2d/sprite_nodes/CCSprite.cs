using System;
using System.Drawing;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Cocos2D
{
    public class CCSprite : CCNodeRGBA, ICCTextureProtocol
    {
        protected bool m_bDirty; // Sprite needs to be updated
        protected bool m_bFlipX;
        protected bool m_bFlipY;
        protected bool m_bHasChildren; // optimization to check if it contain children
        protected bool m_bOpacityModifyRGB;
        protected bool m_bRectRotated;
        protected bool m_bRecursiveDirty; // Subchildren needs to be updated
        protected bool m_bShouldBeHidden; // should not be drawn because one of the ancestors is not visible

        // Offset Position (used by Zwoptex)
        protected CCPoint m_obOffsetPosition;

        protected CCRect m_obRect;
        protected CCPoint m_obUnflippedOffsetPositionFromCenter;
        protected CCSpriteBatchNode m_pobBatchNode; // Used batch node (weak reference)
        protected CCTexture2D m_pobTexture; // Texture used to render the sprite
        protected CCTextureAtlas m_pobTextureAtlas; // Sprite Sheet texture atlas (weak reference)
        protected CCBlendFunc m_sBlendFunc; // Needed for the texture protocol

        private string m_TextureFile;

        internal CCV3F_C4B_T2F_Quad m_sQuad;
        protected CCAffineTransform m_transformToBatch; //
        protected int m_uAtlasIndex; // Absolute (real) Index on the SpriteSheet

        public override void Serialize(System.IO.Stream stream)
        {
            base.Serialize(stream);
            StreamWriter sw = new StreamWriter(stream);
            CCSerialization.SerializeData(Dirty, sw);
            CCSerialization.SerializeData(IsTextureRectRotated, sw);
            CCSerialization.SerializeData(AtlasIndex, sw);
            CCSerialization.SerializeData(TextureRect, sw);
            CCSerialization.SerializeData(OffsetPosition, sw);
            sw.WriteLine(m_TextureFile == null ? "null" : m_TextureFile);
        }

        public override void Deserialize(System.IO.Stream stream)
        {
            base.Deserialize(stream);
            StreamReader sr = new StreamReader(stream);
            m_TextureFile = sr.ReadLine();
            if (m_TextureFile == "null")
            {
                m_TextureFile = null;
            }
            else
            {
                CCLog.Log("CCSprite - deserialized with texture file " + m_TextureFile);
                InitWithFile(m_TextureFile);
            }
            Dirty = CCSerialization.DeSerializeBool(sr);
            IsTextureRectRotated = CCSerialization.DeSerializeBool(sr);
            AtlasIndex = CCSerialization.DeSerializeInt(sr);
            TextureRect = CCSerialization.DeSerializeRect(sr);
            OffsetPosition = CCSerialization.DeSerializePoint(sr);
        }

        public virtual bool Dirty
        {
            get { return m_bDirty; }
            set
            {
                m_bDirty = value;
                SetDirtyRecursively(value);
            }
        }

        public CCV3F_C4B_T2F_Quad Quad
        {
            // read only
            get { return m_sQuad; }
        }


        public bool IsTextureRectRotated
        {
            get { return m_bRectRotated; }
            private set { m_bRectRotated = value; }
        }

        public int AtlasIndex
        {
            get { return m_uAtlasIndex; }
            set { m_uAtlasIndex = value; }
        }

        public CCRect TextureRect
        {
            get { return m_obRect; }
            set { SetTextureRect(value, false, value.Size); }
        }

        public CCPoint OffsetPosition
        {
            // read only
            get { return m_obOffsetPosition; }
            private set { m_obOffsetPosition = value; }
        }

        public override CCPoint Position
        {
            get { return base.Position; }
            set
            {
                base.Position = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float Rotation
        {
            get { return base.Rotation; }
            set
            {
                base.Rotation = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float RotationX
        {
            get { return base.RotationX; }
            set
            {
                base.RotationX = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float RotationY
        {
            get { return base.RotationY; }
            set
            {
                base.RotationY = value;
                SET_DIRTY_RECURSIVELY();
            }
        }


        public override float SkewX
        {
            get { return base.SkewX; }
            set
            {
                base.SkewX = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float SkewY
        {
            get { return base.SkewY; }
            set
            {
                base.SkewY = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float ScaleX
        {
            get { return base.ScaleX; }
            set
            {
                base.ScaleX = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float ScaleY
        {
            get { return base.ScaleY; }
            set
            {
                base.ScaleY = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float Scale
        {
            get { return base.Scale; }
            set
            {
                base.Scale = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override float VertexZ
        {
            get { return base.VertexZ; }
            set
            {
                base.VertexZ = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override CCPoint AnchorPoint
        {
            get { return base.AnchorPoint; }
            set
            {
                base.AnchorPoint = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override bool IgnoreAnchorPointForPosition
        {
            get { return base.IgnoreAnchorPointForPosition; }
            set
            {
                Debug.Assert(m_pobBatchNode == null, "ignoreAnchorPointForPosition is invalid in CCSprite");
                base.IgnoreAnchorPointForPosition = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                base.Visible = value;
                SET_DIRTY_RECURSIVELY();
            }
        }

        public bool FlipX
        {
            get { return m_bFlipX; }
            set
            {
                if (m_bFlipX != value)
                {
                    m_bFlipX = value;
                    SetTextureRect(m_obRect, m_bRectRotated, m_obContentSize);
                }
            }
        }

        public bool FlipY
        {
            get { return m_bFlipY; }
            set
            {
                if (m_bFlipY != value)
                {
                    m_bFlipY = value;
                    SetTextureRect(m_obRect, m_bRectRotated, m_obContentSize);
                }
            }
        }

        public CCSpriteFrame DisplayFrame
        {
            get
            {
                return new CCSpriteFrame(
                    m_pobTexture,
                    m_obRect.PointsToPixels(),
                    m_bRectRotated,
                    m_obUnflippedOffsetPositionFromCenter.PointsToPixels(),
                    m_obContentSize.PointsToPixels()
                    );
            }
            set
            {
                m_obUnflippedOffsetPositionFromCenter = value.Offset;

                CCTexture2D pNewTexture = value.Texture;
                // update texture before updating texture rect
                if (pNewTexture != m_pobTexture)
                {
                    Texture = pNewTexture;
                }

                // update rect
                m_bRectRotated = value.IsRotated;
                SetTextureRect(value.Rect, m_bRectRotated, value.OriginalSize);
            }
        }

        public CCSpriteBatchNode BatchNode
        {
            get { return m_pobBatchNode; }
            set
            {
                m_pobBatchNode = value;

                if (value == null)
                {
                    m_uAtlasIndex = CCMacros.CCSpriteIndexNotInitialized;
                    m_pobTextureAtlas = null;
                    m_bRecursiveDirty = false;
                    Dirty = false;

                    float x1 = m_obOffsetPosition.X;
                    float y1 = m_obOffsetPosition.Y;
                    float x2 = x1 + m_obRect.Size.Width;
                    float y2 = y1 + m_obRect.Size.Height;

                    m_sQuad.BottomLeft.Vertices = new CCVertex3F(x1, y1, 0);
                    m_sQuad.BottomRight.Vertices = new CCVertex3F(x2, y1, 0);
                    m_sQuad.TopLeft.Vertices = new CCVertex3F(x1, y2, 0);
                    m_sQuad.TopRight.Vertices = new CCVertex3F(x2, y2, 0);
                }
                else
                {
                    // using batch
                    m_transformToBatch = CCAffineTransform.Identity;
                    m_pobTextureAtlas = m_pobBatchNode.TextureAtlas; // weak ref
                }
            }
        }

        public bool IsAntialiased
        {
            get { return Texture.IsAntialiased; }

            set { Texture.IsAntialiased = value; }
        }

        #region RGBA protocol

        private void UpdateColor()
        {
            var color4 = new CCColor4B(_displayedColor.R, _displayedColor.G, _displayedColor.B, _displayedOpacity);

            if (m_bOpacityModifyRGB)
            {
                color4.R = (byte)(color4.R * _displayedOpacity / 255.0f);
                color4.G = (byte)(color4.G * _displayedOpacity / 255.0f);
                color4.B = (byte)(color4.B * _displayedOpacity / 255.0f);
            }

            m_sQuad.BottomLeft.Colors = color4;
            m_sQuad.BottomRight.Colors = color4;
            m_sQuad.TopLeft.Colors = color4;
            m_sQuad.TopRight.Colors = color4;

            // renders using Sprite Manager
            if (m_pobBatchNode != null)
            {
                if (m_uAtlasIndex != CCMacros.CCSpriteIndexNotInitialized)
                {
                    m_pobTextureAtlas.UpdateQuad(ref m_sQuad, m_uAtlasIndex);
                }
                else
                {
                    // no need to set it recursively
                    // update dirty_, don't update recursiveDirty_
                    m_bDirty = true;
                }
            }

            // self render
            // do nothing
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

        public override bool IsOpacityModifyRGB
        {
            get { return m_bOpacityModifyRGB; }
            set
            {
                if (m_bOpacityModifyRGB != value)
                {
                    m_bOpacityModifyRGB = value;
                    UpdateColor();
                }
            }
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

        #endregion

        #region ICCTextureProtocol Members

        public CCBlendFunc BlendFunc
        {
            get { return m_sBlendFunc; }
            set { m_sBlendFunc = value; }
        }

        public virtual CCTexture2D Texture
        {
            get { return m_pobTexture; }
            set
            {
                // If batchnode, then texture id should be the same
                Debug.Assert(m_pobBatchNode == null || value.Name == m_pobBatchNode.Texture.Name,
                             "CCSprite: Batched sprites should use the same texture as the batchnode");

                if (m_pobBatchNode == null && m_pobTexture != value)
                {
                    m_pobTexture = value;
                    UpdateBlendFunc();
                }
            }
        }

        #endregion

        #region Constructors

        public CCSprite()
        {
            if (!Init())
            {
                CCLog.Log("CCSprite (): Problems initializing class");
            }
        }

        public CCSprite(CCSize size)
        {
            if (!InitWithTexture(null, new CCRect(0, 0, size.Width, size.Height)))
            {
                CCLog.Log("CCSprite (CCSize size): Problems initializing class");
            }
        }

        public CCSprite(CCRect rect)
        {
            if (!InitWithTexture(null, rect))
            {
                CCLog.Log("CCSprite (CCRect rect): Problems initializing class");
            }
        }

        public CCSprite(CCTexture2D texture)
        {
            if (!InitWithTexture(texture))
            {
				CCLog.Log("CCSprite (CCTexture2D texture): Problems initializing class"); 
            }
        }

        public CCSprite (CCTexture2D texture, CCRect rect)
        {
            if (!InitWithTexture(texture, rect))
            {
				CCLog.Log("CCSprite (CCTexture2D texture, CCRect rect): Problems initializing class"); 

            }
        }

        public CCSprite (string fileName)
        {
            if (!InitWithFile(fileName))
            {
				CCLog.Log("CCSprite (string fileName): Problems initializing class"); 
			}
        }

        public CCSprite (string fileName, CCRect rect)
        {
            if (!InitWithFile(fileName, rect))
            {
				CCLog.Log("CCSprite (string fileName, CCRect rect): Problems initializing class"); 
			}
        }

        public CCSprite (CCSpriteFrame pSpriteFrame)
        {
            if (!InitWithSpriteFrame(pSpriteFrame))
            {
				CCLog.Log("CCSprite (CCSpriteFrame pSpriteFrame): Problems initializing class"); 
			}
        }

        #endregion

        public override bool Init()
        {
            return InitWithTexture(null, new CCRect());
        }

        public bool InitWithTexture(CCTexture2D pTexture, CCRect rect, bool rotated)
        {
            base.Init();

            m_pobBatchNode = null;

            // shader program
            //setShaderProgram(CCShaderCache::sharedShaderCache()->programForKey(kCCShader_PositionTextureColor));

            m_bRecursiveDirty = false;
            Dirty = false;

            m_bOpacityModifyRGB = true;
            m_sBlendFunc = CCBlendFunc.AlphaBlend;

            m_bFlipX = m_bFlipY = false;

            // default transform anchor: center
            AnchorPoint = new CCPoint(0.5f, 0.5f);

            // zwoptex default values
            m_obOffsetPosition = CCPoint.Zero;

            m_bHasChildren = false;

            // clean the Quad
            m_sQuad = new CCV3F_C4B_T2F_Quad();

            // Atlas: Color
            var tmpColor = new CCColor4B(255, 255, 255, 255);
            m_sQuad.BottomLeft.Colors = tmpColor;
            m_sQuad.BottomRight.Colors = tmpColor;
            m_sQuad.TopLeft.Colors = tmpColor;
            m_sQuad.TopRight.Colors = tmpColor;

            // update texture (calls updateBlendFunc)
            Texture = pTexture;
            SetTextureRect(rect, rotated, rect.Size);

            // by default use "Self Render".
            // if the sprite is added to a batchnode, then it will automatically switch to "batchnode Render"
            BatchNode = null;

            return true;
        }

        public virtual bool InitWithTexture(CCTexture2D texture, CCRect rect)
        {
            return InitWithTexture(texture, rect, false);
        }

        public virtual bool InitWithTexture(CCTexture2D texture)
        {
            Debug.Assert(texture != null, "Invalid texture for sprite");

            var rect = new CCRect();
            rect.Size = texture.ContentSize;

            return InitWithTexture(texture, rect);
        }

        public virtual bool InitWithFile(string fileName)
        {
            Debug.Assert(!String.IsNullOrEmpty(fileName), "Invalid filename for sprite");

            m_TextureFile = fileName;
            CCSpriteFrame pFrame = CCSpriteFrameCache.SharedSpriteFrameCache.SpriteFrameByName(fileName);
            if (pFrame != null)
            {
                return InitWithSpriteFrame(pFrame);
            }

            CCTexture2D pTexture = CCTextureCache.SharedTextureCache.AddImage(fileName);

            if (null != pTexture)
            {
                var rect = new CCRect();
                rect.Size = pTexture.ContentSize;
                return InitWithTexture(pTexture, rect);
            }

            return false;
        }

        protected virtual bool InitWithFile(string fileName, CCRect rect)
        {
            Debug.Assert(!String.IsNullOrEmpty(fileName), "Invalid filename for sprite");

            m_TextureFile = fileName;
            CCTexture2D pTexture = CCTextureCache.SharedTextureCache.AddImage(fileName);
            if (pTexture != null)
            {
                return InitWithTexture(pTexture, rect);
            }

            return false;
        }

        protected virtual bool InitWithSpriteFrame(CCSpriteFrame pSpriteFrame)
        {
            Debug.Assert(pSpriteFrame != null);

            bool bRet = InitWithTexture(pSpriteFrame.Texture, pSpriteFrame.Rect);
            DisplayFrame = pSpriteFrame;

            return bRet;
        }

        public void SetTextureRect(CCRect rect)
        {
            SetTextureRect(rect, false, rect.Size);
        }

        public void SetTextureRect(CCRect value, bool rotated, CCSize untrimmedSize)
        {
            m_bRectRotated = rotated;

            ContentSize = untrimmedSize;
            SetVertexRect(value);
            SetTextureCoords(value);

            CCPoint relativeOffset = m_obUnflippedOffsetPositionFromCenter;

            // issue #732
            if (m_bFlipX)
            {
                relativeOffset.X = -relativeOffset.X;
            }
            if (m_bFlipY)
            {
                relativeOffset.Y = -relativeOffset.Y;
            }

            m_obOffsetPosition.X = relativeOffset.X + (m_obContentSize.Width - m_obRect.Size.Width) / 2;
            m_obOffsetPosition.Y = relativeOffset.Y + (m_obContentSize.Height - m_obRect.Size.Height) / 2;

            // rendering using batch node
            if (m_pobBatchNode != null)
            {
                // update dirty_, don't update recursiveDirty_
                Dirty = true;
            }
            else
            {
                // self rendering

                // Atlas: Vertex
                float x1 = 0 + m_obOffsetPosition.X;
                float y1 = 0 + m_obOffsetPosition.Y;
                float x2 = x1 + m_obRect.Size.Width;
                float y2 = y1 + m_obRect.Size.Height;

                // Don't update Z.
                m_sQuad.BottomLeft.Vertices = CCTypes.Vertex3(x1, y1, 0);
                m_sQuad.BottomRight.Vertices = CCTypes.Vertex3(x2, y1, 0);
                m_sQuad.TopLeft.Vertices = CCTypes.Vertex3(x1, y2, 0);
                m_sQuad.TopRight.Vertices = CCTypes.Vertex3(x2, y2, 0);
            }
        }

        // override this method to generate "double scale" sprites
        protected virtual void SetVertexRect(CCRect rect)
        {
            m_obRect = rect;
        }

        private void SetTextureCoords(CCRect rect)
        {
            rect = rect.PointsToPixels();

            CCTexture2D tex = m_pobBatchNode != null ? m_pobTextureAtlas.Texture : m_pobTexture;
            if (tex == null)
            {
                return;
            }

            float atlasWidth = tex.PixelsWide;
            float atlasHeight = tex.PixelsHigh;

            float left, right, top, bottom;

            if (m_bRectRotated)
            {
#if CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL
                left    = (2*rect.origin.x+1)/(2*atlasWidth);
                right    = left+(rect.size.height*2-2)/(2*atlasWidth);
                top        = (2*rect.origin.y+1)/(2*atlasHeight);
                bottom    = top+(rect.size.width*2-2)/(2*atlasHeight);
#else
                left = rect.Origin.X / atlasWidth;
                right = (rect.Origin.X + rect.Size.Height) / atlasWidth;
                top = rect.Origin.Y / atlasHeight;
                bottom = (rect.Origin.Y + rect.Size.Width) / atlasHeight;
#endif
                // CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL

                if (m_bFlipX)
                {
                    CCMacros.CCSwap(ref top, ref bottom);
                }

                if (m_bFlipY)
                {
                    CCMacros.CCSwap(ref left, ref right);
                }

                m_sQuad.BottomLeft.TexCoords.U = left;
                m_sQuad.BottomLeft.TexCoords.V = top;
                m_sQuad.BottomRight.TexCoords.U = left;
                m_sQuad.BottomRight.TexCoords.V = bottom;
                m_sQuad.TopLeft.TexCoords.U = right;
                m_sQuad.TopLeft.TexCoords.V = top;
                m_sQuad.TopRight.TexCoords.U = right;
                m_sQuad.TopRight.TexCoords.V = bottom;
            }
            else
            {
#if CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL
                left    = (2*rect.origin.x+1)/(2*atlasWidth);
                right    = left + (rect.size.width*2-2)/(2*atlasWidth);
                top        = (2*rect.origin.y+1)/(2*atlasHeight);
                bottom    = top + (rect.size.height*2-2)/(2*atlasHeight);
#else
                left = rect.Origin.X / atlasWidth;
                right = (rect.Origin.X + rect.Size.Width) / atlasWidth;
                top = rect.Origin.Y / atlasHeight;
                bottom = (rect.Origin.Y + rect.Size.Height) / atlasHeight;
#endif
                // ! CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL

                if (m_bFlipX)
                {
                    CCMacros.CCSwap(ref left, ref right);
                }

                if (m_bFlipY)
                {
                    CCMacros.CCSwap(ref top, ref bottom);
                }

                m_sQuad.BottomLeft.TexCoords.U = left;
                m_sQuad.BottomLeft.TexCoords.V = bottom;
                m_sQuad.BottomRight.TexCoords.U = right;
                m_sQuad.BottomRight.TexCoords.V = bottom;
                m_sQuad.TopLeft.TexCoords.U = left;
                m_sQuad.TopLeft.TexCoords.V = top;
                m_sQuad.TopRight.TexCoords.U = right;
                m_sQuad.TopRight.TexCoords.V = top;
            }
        }

        public override void UpdateTransform()
        {
            Debug.Assert(m_pobBatchNode != null,
                         "updateTransform is only valid when CCSprite is being rendered using an CCSpriteBatchNode");

            // recaculate matrix only if it is dirty
            if (Dirty)
            {
                // If it is not visible, or one of its ancestors is not visible, then do nothing:
                if (!m_bVisible ||
                    (m_pParent != null && m_pParent != m_pobBatchNode && ((CCSprite)m_pParent).m_bShouldBeHidden))
                {
                    m_sQuad.BottomRight.Vertices =
                        m_sQuad.TopLeft.Vertices = m_sQuad.TopRight.Vertices = m_sQuad.BottomLeft.Vertices = new CCVertex3F(0, 0, 0);
                    m_bShouldBeHidden = true;
                }
                else
                {
                    m_bShouldBeHidden = false;

                    if (m_pParent == null || m_pParent == m_pobBatchNode)
                    {
                        m_transformToBatch = NodeToParentTransform();
                    }
                    else
                    {
                        Debug.Assert((m_pParent as CCSprite) != null,
                                     "Logic error in CCSprite. Parent must be a CCSprite");
                        m_transformToBatch = CCAffineTransform.Concat(NodeToParentTransform(),
                                                                                       ((CCSprite)m_pParent).
                                                                                           m_transformToBatch);
                    }

                    //
                    // calculate the Quad based on the Affine Matrix
                    //

                    CCSize size = m_obRect.Size;

                    float x1 = m_obOffsetPosition.X;
                    float y1 = m_obOffsetPosition.Y;

                    float x2 = x1 + size.Width;
                    float y2 = y1 + size.Height;
                    float x = m_transformToBatch.tx;
                    float y = m_transformToBatch.ty;

                    float cr = m_transformToBatch.a;
                    float sr = m_transformToBatch.b;
                    float cr2 = m_transformToBatch.d;
                    float sr2 = -m_transformToBatch.c;
                    float ax = x1 * cr - y1 * sr2 + x;
                    float ay = x1 * sr + y1 * cr2 + y;

                    float bx = x2 * cr - y1 * sr2 + x;
                    float by = x2 * sr + y1 * cr2 + y;

                    float cx = x2 * cr - y2 * sr2 + x;
                    float cy = x2 * sr + y2 * cr2 + y;

                    float dx = x1 * cr - y2 * sr2 + x;
                    float dy = x1 * sr + y2 * cr2 + y;

                    m_sQuad.BottomLeft.Vertices = new CCVertex3F(ax, ay, m_fVertexZ);
                    m_sQuad.BottomRight.Vertices = new CCVertex3F(bx, by, m_fVertexZ);
                    m_sQuad.TopLeft.Vertices = new CCVertex3F(dx, dy, m_fVertexZ);
                    m_sQuad.TopRight.Vertices = new CCVertex3F(cx, cy, m_fVertexZ);
                }

                m_pobTextureAtlas.UpdateQuad(ref m_sQuad, m_uAtlasIndex);
                m_bRecursiveDirty = false;
                m_bDirty = false;
            }

            // recursively iterate over children
            if (m_bHasChildren)
            {
                CCNode[] elements = m_pChildren.Elements;
                if (m_pobBatchNode != null)
                {
                    for (int i = 0, count = m_pChildren.count; i < count; i++)
                    {
                        ((CCSprite)elements[i]).UpdateTransform();
                    }
                }
                else
                {
                    for (int i = 0, count = m_pChildren.count; i < count; i++)
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

        public override void Draw()
        {
            Debug.Assert(m_pobBatchNode == null);

            CCDrawManager.BlendFunc(m_sBlendFunc);
            CCDrawManager.BindTexture(Texture);
            CCDrawManager.DrawQuad(ref m_sQuad);
        }

        public override void AddChild(CCNode child, int zOrder, int tag)
        {
            Debug.Assert(child != null, "Argument must be non-NULL");

            if (m_pobBatchNode != null)
            {
                var sprite = child as CCSprite;

                Debug.Assert(sprite != null, "CCSprite only supports CCSprites as children when using CCSpriteBatchNode");
                Debug.Assert(sprite.Texture.Name == m_pobTextureAtlas.Texture.Name);

                m_pobBatchNode.AppendChild(sprite);

                if (!m_bReorderChildDirty)
                {
                    SetReorderChildDirtyRecursively();
                }
            }

            base.AddChild(child, zOrder, tag);
            m_bHasChildren = true;
        }

        public override void ReorderChild(CCNode child, int zOrder)
        {
            Debug.Assert(child != null);
            Debug.Assert(m_pChildren.Contains(child));

            if (zOrder == child.ZOrder)
            {
                return;
            }

            if (m_pobBatchNode != null && !m_bReorderChildDirty)
            {
                SetReorderChildDirtyRecursively();
                m_pobBatchNode.ReorderBatch(true);
            }

            base.ReorderChild(child, zOrder);
        }

        public override void RemoveChild(CCNode child, bool cleanup)
        {
            if (m_pobBatchNode != null)
            {
                m_pobBatchNode.RemoveSpriteFromAtlas((CCSprite)(child));
            }

            base.RemoveChild(child, cleanup);
        }

        public override void RemoveAllChildrenWithCleanup(bool cleanup)
        {
            if (m_pobBatchNode != null)
            {
                CCSpriteBatchNode batch = m_pobBatchNode;
                CCNode[] elements = m_pChildren.Elements;
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    batch.RemoveSpriteFromAtlas((CCSprite)elements[i]);
                }
            }

            base.RemoveAllChildrenWithCleanup(cleanup);

            m_bHasChildren = false;
        }

        public override void SortAllChildren()
        {
            if (m_bReorderChildDirty)
            {
                int i, j, length = m_pChildren.count;
                CCNode[] x = m_pChildren.Elements;
                CCNode tempItem;

                // insertion sort
                for (i = 1; i < length; i++)
                {
                    tempItem = x[i];
                    j = i - 1;

                    //continue moving element downwards while zOrder is smaller or when zOrder is the same but orderOfArrival is smaller
                    while (j >= 0 &&
                           (tempItem.m_nZOrder < x[j].m_nZOrder ||
                            (tempItem.m_nZOrder == x[j].m_nZOrder && tempItem.m_uOrderOfArrival < x[j].m_uOrderOfArrival)))
                    {
                        x[j + 1] = x[j];
                        j = j - 1;
                    }
                    x[j + 1] = tempItem;
                }

                if (m_pobBatchNode != null)
                {
                    foreach (CCNode node in m_pChildren)
                    {
                        (node).SortAllChildren();
                    }
                }

                m_bReorderChildDirty = false;
            }
        }

        public virtual void SetReorderChildDirtyRecursively()
        {
            //only set parents flag the first time
            if (!m_bReorderChildDirty)
            {
                m_bReorderChildDirty = true;
                CCNode node = m_pParent;
                while (node != null && node != m_pobBatchNode)
                {
                    ((CCSprite)node).SetReorderChildDirtyRecursively();
                    node = node.Parent;
                }
            }
        }

        public virtual void SetDirtyRecursively(bool bValue)
        {
            m_bDirty = m_bRecursiveDirty = bValue;

            // recursively set dirty
            if (m_bHasChildren)
            {
                CCNode[] elements = m_pChildren.Elements;
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    var sprite = elements[i] as CCSprite;
                    if (sprite != null)
                    {
                        sprite.SetDirtyRecursively(true);
                    }
                }
            }
        }

        private void SET_DIRTY_RECURSIVELY()
        {
            if (m_pobBatchNode != null && !m_bRecursiveDirty)
            {
                m_bDirty = m_bRecursiveDirty = true;
                if (m_bHasChildren)
                {
                    SetDirtyRecursively(true);
                }
            }
		}

        public void SetDisplayFrameWithAnimationName(string animationName, int frameIndex)
        {
            Debug.Assert(!String.IsNullOrEmpty(animationName),
                         "CCSprite#setDisplayFrameWithAnimationName. animationName must not be NULL");

            CCAnimation a = CCAnimationCache.SharedAnimationCache.AnimationByName(animationName);

            Debug.Assert(a != null, "CCSprite#setDisplayFrameWithAnimationName: Frame not found");

            var frame = (CCAnimationFrame)a.Frames[frameIndex];

            Debug.Assert(frame != null, "CCSprite#setDisplayFrame. Invalid frame");

            DisplayFrame = frame.SpriteFrame;
        }

        public bool IsFrameDisplayed(CCSpriteFrame pFrame)
        {
            CCRect r = pFrame.Rect;

            return (
                       CCRect.Equal(ref r, ref m_obRect) &&
                       pFrame.Texture.Name == m_pobTexture.Name &&
                       pFrame.Offset.Equals(m_obUnflippedOffsetPositionFromCenter)
                   );
        }

        protected void UpdateBlendFunc()
        {
            Debug.Assert(m_pobBatchNode == null,
                         "CCSprite: updateBlendFunc doesn't work when the sprite is rendered using a CCSpriteSheet");

            // it's possible to have an untextured sprite
            if (m_pobTexture == null || !m_pobTexture.HasPremultipliedAlpha)
            {
                m_sBlendFunc = CCBlendFunc.NonPremultiplied;
                IsOpacityModifyRGB = false;
            }
            else
            {
                m_sBlendFunc = CCBlendFunc.AlphaBlend;
                IsOpacityModifyRGB = true;
            }
        }
    }
}