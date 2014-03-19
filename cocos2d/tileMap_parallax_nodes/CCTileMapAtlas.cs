using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public class CCTileMapAtlas : CCAtlasNode
    {
        //! numbers of tiles to render
        protected int m_nItemsToRender;
        protected Dictionary<CCGridSize, int> m_pPosToAtlasIndex;
        private CCImageTGA m_pTGAInfo;

        public CCImageTGA TGAInfo
        {
            get { return m_pTGAInfo; }
            set { m_pTGAInfo = value; }
        }


        #region Constructors

        public CCTileMapAtlas(string tile, string mapFile, int tileWidth, int tileHeight)
        {
            InitWithTileFile(tile, mapFile, tileWidth, tileHeight);
        }

        private void InitWithTileFile(string tile, string mapFile, int tileWidth, int tileHeight)
        {
            LoadTgAfile(mapFile);
            CalculateItemsToRender();

            // We can't call base constructor because we need to determine num of items to render first
            base.InitWithTileFile(tile, tileWidth, tileHeight, m_nItemsToRender);

            m_pPosToAtlasIndex = new Dictionary<CCGridSize, int>();
            UpdateAtlasValues();
            ContentSize = new CCSize(m_pTGAInfo.width * ItemWidth, m_pTGAInfo.height * ItemHeight);
        }

        #endregion Constructors 


        public Color TileAt(CCGridSize position)
        {
            Debug.Assert(m_pTGAInfo != null, "tgaInfo must not be nil");
            Debug.Assert(position.X < m_pTGAInfo.width, "Invalid position.x");
            Debug.Assert(position.Y < m_pTGAInfo.height, "Invalid position.y");

            return m_pTGAInfo.imageData[position.X + position.Y * m_pTGAInfo.width];
        }

        public void SetTile(Color tile, CCGridSize position)
        {
            Debug.Assert(m_pTGAInfo != null, "tgaInfo must not be nil");
            Debug.Assert(m_pPosToAtlasIndex != null, "posToAtlasIndex must not be nil");
            Debug.Assert(position.X < m_pTGAInfo.width, "Invalid position.x");
            Debug.Assert(position.Y < m_pTGAInfo.height, "Invalid position.x");
            Debug.Assert(tile.R != 0, "R component must be non 0");

            Color value = m_pTGAInfo.imageData[position.X + position.Y * m_pTGAInfo.width];
            if (value.R == 0)
            {
                CCLog.Log("CocosSharp: Value.r must be non 0.");
            }
            else
            {
                m_pTGAInfo.imageData[position.X + position.Y * m_pTGAInfo.width] = tile;

                // XXX: this method consumes a lot of memory
                // XXX: a tree of something like that shall be impolemented
                int num = m_pPosToAtlasIndex[position];
                UpdateAtlasValueAt(position, tile, num);
            }
        }

        public void ReleaseMap()
        {
            m_pTGAInfo = null;
            m_pPosToAtlasIndex = null;
        }

        private void LoadTgAfile(string file)
        {
            Debug.Assert(!string.IsNullOrEmpty(file), "file must be non-nil");

            m_pTGAInfo = new CCImageTGA(CCFileUtils.FullPathFromRelativePath(file));
        }

        private void CalculateItemsToRender()
        {
            Debug.Assert(m_pTGAInfo != null, "tgaInfo must be non-nil");

            m_nItemsToRender = 0;
            var data = m_pTGAInfo.imageData;
            for (int i = 0, count = m_pTGAInfo.width * m_pTGAInfo.height; i < count;  i++)
            {
                if (data[i].R != 0)
                {
                    ++m_nItemsToRender;
                }
            }
        }

        private void UpdateAtlasValueAt(CCGridSize pos, Color value, int index)
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

            var color = new CCColor4B(_displayedColor.R, _displayedColor.G, _displayedColor.B, _displayedOpacity);
            quad.TopRight.Colors = color;
            quad.TopLeft.Colors = color;
            quad.BottomRight.Colors = color;
            quad.BottomLeft.Colors = color;

            TextureAtlas.UpdateQuad(ref quad, index);
        }

        public override void UpdateAtlasValues()
        {
            Debug.Assert(m_pTGAInfo != null, "tgaInfo must be non-nil");

            int total = 0;

            for (int x = 0; x < m_pTGAInfo.width; x++)
            {
                for (int y = 0; y < m_pTGAInfo.height; y++)
                {
                    if (total < m_nItemsToRender)
                    {
                        Color value = m_pTGAInfo.imageData[x + y * m_pTGAInfo.width];

                        if (value.R != 0)
                        {
                            var pos = new CCGridSize(x, y);
                            UpdateAtlasValueAt(pos, value, total);
                            m_pPosToAtlasIndex.Add(pos, total);

                            total++;
                        }
                    }
                }
            }
        }
    }
}