
using System.Diagnostics;

namespace CocosSharp
{
    public class CCAtlasNode : CCNodeRGBA, ICCTexture
    {
        protected bool IsOpacityModifyRGB;

        public CCTextureAtlas TextureAtlas;
        public CCBlendFunc BlendFunc { get; set; }

        protected CCColor3B ColorUnmodified;
        protected int ItemHeight;
        protected int ItemWidth;
        protected int ItemsPerColumn;
        protected int ItemsPerRow;

        // color uniform
        protected int UniformColor;

        // quads to draw
        public int QuadsToDraw;

        // This varible is only used for CCLabelAtlas FPS display. So please don't modify its value.
        public bool IgnoreContentScaleFactor;


		public bool IsAntialiased
		{
			get { return Texture.IsAntialiased; }
			set { Texture.IsAntialiased = value; }
		}

        #region ICCRGBAProtocol Members

        public override bool IsColorModifiedByOpacity
        {
            get { return IsOpacityModifyRGB; }
            set
            {
                CCColor3B oldColor = Color;
                IsOpacityModifyRGB = value;
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
                if (IsOpacityModifyRGB)
                {
                    Color = ColorUnmodified;
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
                if (IsOpacityModifyRGB)
                {
                    return ColorUnmodified;
                }
                return base.Color;
            }
            set
            {
                var tmp = value;
                ColorUnmodified = value;

                if (IsOpacityModifyRGB)
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
            IsOpacityModifyRGB = TextureAtlas.Texture.HasPremultipliedAlpha;
        }

        #endregion

        #region ICCTextureProtocol Members

        public virtual CCTexture2D Texture
        {
            get { return TextureAtlas.Texture; }
            set
            {
                TextureAtlas.Texture = value;
                UpdateBlendFunc();
                UpdateOpacityModifyRgb();
            }
        }

        private void UpdateBlendFunc()
        {
            if (!TextureAtlas.Texture.HasPremultipliedAlpha)
            {
                BlendFunc = CCBlendFunc.NonPremultiplied;
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

        protected void InitWithTileFile(string tile, int tileWidth, int tileHeight, int itemsToRender)
        {
            Debug.Assert(tile != null, "title should not be null");
            var texture = CCTextureCache.SharedTextureCache.AddImage(tile);
            InitWithTexture(texture, tileWidth, tileHeight, itemsToRender);
        }

        private void InitWithTexture(CCTexture2D texture, int tileWidth, int tileHeight, int itemsToRender)
        {
            ItemWidth = tileWidth;
            ItemHeight = tileHeight;

            ColorUnmodified = CCTypes.CCWhite;
            IsOpacityModifyRGB = true;

            BlendFunc = CCBlendFunc.AlphaBlend; 

            TextureAtlas = new CCTextureAtlas(texture, itemsToRender);

            UpdateBlendFunc();
            UpdateOpacityModifyRgb();

            CalculateMaxItems();

            QuadsToDraw = itemsToRender;
        }

        private void CalculateMaxItems()
        {
            CCSize s = TextureAtlas.Texture.ContentSize;

            if (IgnoreContentScaleFactor)
            {
                s = TextureAtlas.Texture.ContentSizeInPixels;
            }

            ItemsPerColumn = (int) (s.Height / ItemHeight);
            ItemsPerRow = (int) (s.Width / ItemWidth);
        }

        public virtual void UpdateAtlasValues()
        {
            Debug.Assert(false, "CCAtlasNode:Abstract updateAtlasValue not overridden");
        }

        protected override void Draw()
        {
            CCDrawManager.BlendFunc(BlendFunc);

            TextureAtlas.DrawNumberOfQuads(QuadsToDraw, 0);
        }
    }
}