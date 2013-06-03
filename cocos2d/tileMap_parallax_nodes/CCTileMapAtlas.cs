using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Cocos2D
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

        public static CCTileMapAtlas Create(string tile, string mapFile, int tileWidth, int tileHeight)
        {
            var pRet = new CCTileMapAtlas();
            pRet.InitWithTileFile(tile, mapFile, tileWidth, tileHeight);
            return pRet;
        }

        public bool InitWithTileFile(string tile, string mapFile, int tileWidth, int tileHeight)
        {
            LoadTgAfile(mapFile);
            CalculateItemsToRender();

            if (base.InitWithTileFile(tile, tileWidth, tileHeight, m_nItemsToRender))
            {
                m_pPosToAtlasIndex = new Dictionary<CCGridSize, int>();
                UpdateAtlasValues();
                ContentSize = new CCSize(m_pTGAInfo.width * m_uItemWidth, m_pTGAInfo.height * m_uItemHeight);
                return true;
            }
            return false;
        }

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
                CCLog.Log("cocos2d: Value.r must be non 0.");
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

            float row = (float)(value.R % m_uItemsPerRow);
            float col = (float)(value.R / m_uItemsPerRow);

            float textureWide = (m_pTextureAtlas.Texture.PixelsWide);
            float textureHigh = (m_pTextureAtlas.Texture.PixelsHigh);

            float itemWidthInPixels = m_uItemWidth * CCMacros.CCContentScaleFactor();
            float itemHeightInPixels = m_uItemHeight * CCMacros.CCContentScaleFactor();

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

            CCV3F_C4B_T2F_Quad quad;

            quad.TopLeft.TexCoords.U = left;
            quad.TopLeft.TexCoords.V = top;
            quad.TopRight.TexCoords.U = right;
            quad.TopRight.TexCoords.V = top;
            quad.BottomLeft.TexCoords.U = left;
            quad.BottomLeft.TexCoords.V = bottom;
            quad.BottomRight.TexCoords.U = right;
            quad.BottomRight.TexCoords.V = bottom;

            quad.BottomLeft.Vertices.X = (x * m_uItemWidth);
            quad.BottomLeft.Vertices.Y = (y * m_uItemHeight);
            quad.BottomLeft.Vertices.Z = 0.0f;
            quad.BottomRight.Vertices.X = (x * m_uItemWidth + m_uItemWidth);
            quad.BottomRight.Vertices.Y = (y * m_uItemHeight);
            quad.BottomRight.Vertices.Z = 0.0f;
            quad.TopLeft.Vertices.X = (x * m_uItemWidth);
            quad.TopLeft.Vertices.Y = (y * m_uItemHeight + m_uItemHeight);
            quad.TopLeft.Vertices.Z = 0.0f;
            quad.TopRight.Vertices.X = (x * m_uItemWidth + m_uItemWidth);
            quad.TopRight.Vertices.Y = (y * m_uItemHeight + m_uItemHeight);
            quad.TopRight.Vertices.Z = 0.0f;

            var color = new CCColor4B(_displayedColor.R, _displayedColor.G, _displayedColor.B, _displayedOpacity);
            quad.TopRight.Colors = color;
            quad.TopLeft.Colors = color;
            quad.BottomRight.Colors = color;
            quad.BottomLeft.Colors = color;

            m_pTextureAtlas.UpdateQuad(ref quad, index);
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