
using System.Diagnostics;

namespace CocosSharp
{
    public class CCAtlasNode : CCNode, ICCTexture
    {
    
        #region Properties

        protected bool IsOpacityModifyRGB { get; set; }

        protected int ItemHeight { get; set; }
        protected int ItemWidth { get; set; }
        protected int ItemsPerColumn { get; set; }
        protected int ItemsPerRow { get; set; }

        protected int UniformColor { get; set; }
        protected int QuadsToDraw { get; set; }
        public CCBlendFunc BlendFunc { get; set; }
        protected CCColor3B ColorUnmodified { get; set; }
         
        public CCTextureAtlas TextureAtlas { get; set; }

        public bool IsAntialiased
        {
            get { return Texture.IsAntialiased; }
            set { Texture.IsAntialiased = value; }
        }

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
                    tmp.R = (byte) (value.R * DisplayedOpacity / 255);
                    tmp.G = (byte) (value.G * DisplayedOpacity / 255);
                    tmp.B = (byte) (value.B * DisplayedOpacity / 255);
                }
                base.Color = tmp;
                UpdateAtlasValues();
            }
        }

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

        #endregion Properties


        #region Constructors

        internal CCAtlasNode()
        {
        }

        public CCAtlasNode(string tile, int tileWidth, int tileHeight, int itemsToRender) 
            : this(CCTextureCache.SharedTextureCache.AddImage(tile), tileWidth, tileHeight, itemsToRender)
        {
        }

        public CCAtlasNode(CCTexture2D texture, int tileWidth, int tileHeight, int itemsToRender)
        {
            ItemWidth = tileWidth;
            ItemHeight = tileHeight;

            ColorUnmodified = CCColor3B.White;
            IsOpacityModifyRGB = false;
            BlendFunc = CCBlendFunc.AlphaBlend; 

            TextureAtlas = new CCTextureAtlas(texture, itemsToRender);

            UpdateBlendFunc();
            UpdateOpacityModifyRgb();

            CalculateMaxItems();
            QuadsToDraw = itemsToRender;
        }

        #endregion Constructors

        protected override void VisitRenderer(ref CCAffineTransform worldTransform)
        {
            var quadsCommand = new CCQuadCommand(worldTransform.Tz, worldTransform, Texture, BlendFunc, QuadsToDraw, TextureAtlas.Quads.Elements);
            Renderer.AddCommand(quadsCommand);
        }

        void CalculateMaxItems()
        {
            CCSize s = TextureAtlas.Texture.ContentSizeInPixels;

            ItemsPerColumn = (int) (s.Height / ItemHeight);
            ItemsPerRow = (int) (s.Width / ItemWidth);
        }


        #region Updating atlas

        public virtual void UpdateAtlasValues()
        {
            Debug.Assert(false, "CCAtlasNode:Abstract updateAtlasValue not overridden");
        }

        void UpdateOpacityModifyRgb()
        {
            IsOpacityModifyRGB = TextureAtlas.Texture.HasPremultipliedAlpha;
        }

        void UpdateBlendFunc()
        {
            if (!TextureAtlas.Texture.HasPremultipliedAlpha)
            {
                BlendFunc = CCBlendFunc.NonPremultiplied;
            }
        }

        #endregion Updating atlas

    }
}