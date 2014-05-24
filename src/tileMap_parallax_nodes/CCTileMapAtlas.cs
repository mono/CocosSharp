using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public class CCTileMapAtlas : CCAtlasNode
    {
        #region Properties

        protected int NumOfItemsToRender { get; private set; }
        protected Dictionary<CCGridSize, int> PositionToAtlasIndex { get; private set; }
		internal CCImageTGA TGAInfo { get; private set; }

        #endregion Properties


        #region Constructors

        public CCTileMapAtlas(string tile, string mapFile, int tileWidth, int tileHeight)
            : this(tile, mapFile, tileWidth, tileHeight, new CCImageTGA(CCFileUtils.FullPathFromRelativePath(mapFile)))
        {
        }

        CCTileMapAtlas(string tile, string mapFile, int tileWidth, int tileHeight, CCImageTGA tgaInfo)
            : this(tile, tileWidth, tileHeight, tgaInfo, CalculateItemsToRender(tgaInfo))
        {
        }

        CCTileMapAtlas(string tile, int tileWidth, int tileHeight, CCImageTGA tgaInfo, int numOfItemsToRender)
            : base(tile, tileWidth, tileHeight, numOfItemsToRender)
        {
            TGAInfo = tgaInfo;
            NumOfItemsToRender = numOfItemsToRender;

            PositionToAtlasIndex = new Dictionary<CCGridSize, int>();
            UpdateAtlasValues();
			ContentSize = new CCSize(TGAInfo.Width * ItemWidth, TGAInfo.Height * ItemHeight);
        }

        // Just used during construction
        static int CalculateItemsToRender(CCImageTGA tgaInfo)
        {
            Debug.Assert(tgaInfo != null, "tgaInfo must be non-nil");

            int itemsToRender = 0;
            var data = tgaInfo.ImageData;
			for (int i = 0, count = tgaInfo.Width * tgaInfo.Height; i < count;  i++)
            {
                if (data[i].R != 0)
                {
                    ++itemsToRender;
                }
            }

            return itemsToRender;
        }

        #endregion Constructors 


        public void ReleaseMap()
        {
            TGAInfo = null;
            PositionToAtlasIndex = null;
        }

		public CCColor4B TileAt(CCGridSize position)
        {
            Debug.Assert(TGAInfo != null, "tgaInfo must not be nil");
			Debug.Assert(position.X < TGAInfo.Width, "Invalid position.x");
			Debug.Assert(position.Y < TGAInfo.Height, "Invalid position.y");

			return new CCColor4B(TGAInfo.ImageData[position.X + position.Y * TGAInfo.Width]);
        }

		public void SetTile(CCColor4B tile, CCGridSize position)
        {
            Debug.Assert(TGAInfo != null, "tgaInfo must not be nil");
            Debug.Assert(PositionToAtlasIndex != null, "posToAtlasIndex must not be nil");
			Debug.Assert(position.X < TGAInfo.Width, "Invalid position.x");
			Debug.Assert(position.Y < TGAInfo.Height, "Invalid position.x");
            Debug.Assert(tile.R != 0, "R component must be non 0");

			Color value = TGAInfo.ImageData[position.X + position.Y * TGAInfo.Width];
            if (value.R == 0)
            {
                CCLog.Log("CocosSharp: Value.r must be non 0.");
            }
            else
            {
				TGAInfo.ImageData[position.X + position.Y * TGAInfo.Width] = new Color(tile.R, tile.G, tile.B, tile.A);

                // XXX: this method consumes a lot of memory
                // XXX: a tree of something like that shall be impolemented
                int num = PositionToAtlasIndex[position];
				UpdateAtlasValueAt(position, tile.ToColor(), num);
            }
        }

        #region Updating atlas

        void UpdateAtlasValueAt(CCGridSize pos, Color value, int index)
        {
            int x = pos.X;
            int y = pos.Y;

            float row = (float)(value.R % ItemsPerRow);
            float col = (float)(value.R / ItemsPerRow);

            float textureWide = (TextureAtlas.Texture.PixelsWide);
            float textureHigh = (TextureAtlas.Texture.PixelsHigh);

            float itemWidthInPixels = ItemWidth * CCMacros.CCContentScaleFactor();
            float itemHeightInPixels = ItemHeight * CCMacros.CCContentScaleFactor();

#if CC_FIX_ARTIFACTS_BY_STRECHING_TEXEL
            float left        = (2 * row * itemWidthInPixels + 1) / (2 * textureWide);
            float right       = left + (itemWidthInPixels * 2 - 2) / (2 * textureWide);
            float top         = (2 * col * itemHeightInPixels + 1) / (2 * textureHigh);
            float bottom      = top + (itemHeightInPixels * 2 - 2) / (2 * textureHigh);
#else
            float left = (row * itemWidthInPixels) / textureWide;
            float right = left + itemWidthInPixels / textureWide;
            float top = (col * itemHeightInPixels) / textureHigh;
            float bottom = top + itemHeightInPixels / textureHigh;
#endif

            CCV3F_C4B_T2F_Quad quad = new CCV3F_C4B_T2F_Quad();

            quad.TopLeft.TexCoords.U = left;
            quad.TopLeft.TexCoords.V = top;
            quad.TopRight.TexCoords.U = right;
            quad.TopRight.TexCoords.V = top;
            quad.BottomLeft.TexCoords.U = left;
            quad.BottomLeft.TexCoords.V = bottom;
            quad.BottomRight.TexCoords.U = right;
            quad.BottomRight.TexCoords.V = bottom;

            quad.BottomLeft.Vertices.X = (x * ItemWidth);
            quad.BottomLeft.Vertices.Y = (y * ItemHeight);
            quad.BottomLeft.Vertices.Z = 0.0f;
            quad.BottomRight.Vertices.X = (x * ItemWidth + ItemWidth);
            quad.BottomRight.Vertices.Y = (y * ItemHeight);
            quad.BottomRight.Vertices.Z = 0.0f;
            quad.TopLeft.Vertices.X = (x * ItemWidth);
            quad.TopLeft.Vertices.Y = (y * ItemHeight + ItemHeight);
            quad.TopLeft.Vertices.Z = 0.0f;
            quad.TopRight.Vertices.X = (x * ItemWidth + ItemWidth);
            quad.TopRight.Vertices.Y = (y * ItemHeight + ItemHeight);
            quad.TopRight.Vertices.Z = 0.0f;

            var color = new CCColor4B(DisplayedColor.R, DisplayedColor.G, DisplayedColor.B, DisplayedOpacity);
            quad.TopRight.Colors = color;
            quad.TopLeft.Colors = color;
            quad.BottomRight.Colors = color;
            quad.BottomLeft.Colors = color;

            TextureAtlas.UpdateQuad(ref quad, index);
        }

        public override void UpdateAtlasValues()
        {
            Debug.Assert(TGAInfo != null, "tgaInfo must be non-nil");

            int total = 0;

			for (int x = 0; x < TGAInfo.Width; x++)
            {
				for (int y = 0; y < TGAInfo.Height; y++)
                {
                    if (total < NumOfItemsToRender)
                    {
						Color value = TGAInfo.ImageData[x + y * TGAInfo.Width];

                        if (value.R != 0)
                        {
                            var pos = new CCGridSize(x, y);
                            UpdateAtlasValueAt(pos, value, total);
                            PositionToAtlasIndex.Add(pos, total);

                            total++;
                        }
                    }
                }
            }
        }

        #endregion Updating atlas
    }
}