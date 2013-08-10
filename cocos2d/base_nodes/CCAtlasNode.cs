
using System.Diagnostics;

namespace Cocos2D
{
    public class CCAtlasNode : CCNodeRGBA, ICCTextureProtocol
    {
        protected bool m_bIsOpacityModifyRGB;

        protected CCTextureAtlas m_pTextureAtlas;
        protected CCBlendFunc m_tBlendFunc;

        protected CCColor3B m_tColorUnmodified;
        protected int m_uItemHeight;
        protected int m_uItemWidth;
        protected int m_uItemsPerColumn;
        protected int m_uItemsPerRow;

        // color uniform
        protected int m_nUniformColor;

        // quads to draw
        protected int m_uQuadsToDraw;

        // This varible is only used for CCLabelAtlas FPS display. So plz don't modify its value.
        protected bool m_bIgnoreContentScaleFactor;

        public CCTextureAtlas TextureAtlas
        {
            get { return m_pTextureAtlas; }
            set { m_pTextureAtlas = value; }
        }

        public int QuadsToDraw
        {
            get { return m_uQuadsToDraw; }
            set { m_uQuadsToDraw = value; }
        }

		public bool IsAntialiased
		{
			get { return Texture.IsAntialiased; }

			set { Texture.IsAntialiased = value; }
		}

        #region ICCRGBAProtocol Members

        public override bool IsOpacityModifyRGB
        {
            get { return m_bIsOpacityModifyRGB; }
            set
            {
                CCColor3B oldColor = Color;
                m_bIsOpacityModifyRGB = value;
                Color = oldColor;
            }
        }

        public override byte Opacity
        {
            get { return base.Opacity; }
            set
            {
                base.Opacity = value;

                // special opacity for premultiplied textures
                if (m_bIsOpacityModifyRGB)
                {
                    Color = m_tColorUnmodified;
                }
                else
                {
                    UpdateAtlasValues();
                }
            }
        }

        public override CCColor3B Color
        {
            get
            {
                if (m_bIsOpacityModifyRGB)
                {
                    return m_tColorUnmodified;
                }
                return base.Color;
            }
            set
            {
                var tmp = value;
                m_tColorUnmodified = value;

                if (m_bIsOpacityModifyRGB)
                {
                    tmp.R = (byte) (value.R * _displayedOpacity / 255);
                    tmp.G = (byte) (value.G * _displayedOpacity / 255);
                    tmp.B = (byte) (value.B * _displayedOpacity / 255);
                }
                base.Color = tmp;
                UpdateAtlasValues();
            }
        }

        private void UpdateOpacityModifyRgb()
        {
            m_bIsOpacityModifyRGB = m_pTextureAtlas.Texture.HasPremultipliedAlpha;
        }

        #endregion

        #region ICCTextureProtocol Members

        public CCBlendFunc BlendFunc
        {
            get { return m_tBlendFunc; }
            set { m_tBlendFunc = value; }
        }

        public virtual CCTexture2D Texture
        {
            get { return m_pTextureAtlas.Texture; }
            set
            {
                m_pTextureAtlas.Texture = value;
                UpdateBlendFunc();
                UpdateOpacityModifyRgb();
            }
        }

        private void UpdateBlendFunc()
        {
            if (!m_pTextureAtlas.Texture.HasPremultipliedAlpha)
            {
                m_tBlendFunc = CCBlendFunc.NonPremultiplied;
            }
        }

        #endregion

        internal CCAtlasNode()
        {
        }

        public CCAtlasNode(string tile, int tileWidth, int tileHeight, int itemsToRender)
        {
            InitWithTileFile(tile, tileWidth, tileHeight, itemsToRender);
        }

        public CCAtlasNode(CCTexture2D texture, int tileWidth, int tileHeight, int itemsToRender)
        {
            InitWithTexture(texture, tileWidth, tileHeight, itemsToRender);
        }

        public bool InitWithTileFile(string tile, int tileWidth, int tileHeight, int itemsToRender)
        {
            Debug.Assert(tile != null, "title should not be null");
            var texture = CCTextureCache.SharedTextureCache.AddImage(tile);
            return InitWithTexture(texture, tileWidth, tileHeight, itemsToRender);
        }

        public bool InitWithTexture(CCTexture2D texture, int tileWidth, int tileHeight, int itemsToRender)
        {
            m_uItemWidth = tileWidth;
            m_uItemHeight = tileHeight;

            m_tColorUnmodified = CCTypes.CCWhite;
            m_bIsOpacityModifyRGB = true;

            m_tBlendFunc = CCBlendFunc.AlphaBlend; 

            m_pTextureAtlas = new CCTextureAtlas();
            m_pTextureAtlas.InitWithTexture(texture, itemsToRender);

            UpdateBlendFunc();
            UpdateOpacityModifyRgb();

            CalculateMaxItems();

            m_uQuadsToDraw = itemsToRender;

            return true;
        }

        private void CalculateMaxItems()
        {
            CCSize s = m_pTextureAtlas.Texture.ContentSize;

            if (m_bIgnoreContentScaleFactor)
            {
                s = m_pTextureAtlas.Texture.ContentSizeInPixels;
            }

            m_uItemsPerColumn = (int) (s.Height / m_uItemHeight);
            m_uItemsPerRow = (int) (s.Width / m_uItemWidth);
        }

        public virtual void UpdateAtlasValues()
        {
            Debug.Assert(false, "CCAtlasNode:Abstract updateAtlasValue not overridden");
        }

        public override void Draw()
        {
            CCDrawManager.BlendFunc(m_tBlendFunc);

            m_pTextureAtlas.DrawNumberOfQuads(m_uQuadsToDraw, 0);
        }

        public void SetIgnoreContentScaleFactor(bool bIgnoreContentScaleFactor)
        {
            m_bIgnoreContentScaleFactor = bIgnoreContentScaleFactor;
        }
    }
}