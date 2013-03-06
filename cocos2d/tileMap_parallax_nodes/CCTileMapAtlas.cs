using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    public class CCTileMapAtlas : CCAtlasNode
    {
        //! numbers of tiles to render
        protected int m_nItemsToRender;
        protected Dictionary<ccGridSize, int> m_pPosToAtlasIndex;
        private tImageTGA m_pTGAInfo;

        public tImageTGA TGAInfo
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
                m_tColor = ccTypes.ccWHITE;
                m_pPosToAtlasIndex = new Dictionary<ccGridSize, int>();
                UpdateAtlasValues();
                ContentSize = new CCSize(m_pTGAInfo.width * m_uItemWidth, m_pTGAInfo.height * m_uItemHeight);
                return true;
            }
            return false;
        }

        public Color TileAt(ccGridSize position)
        {
            Debug.Assert(m_pTGAInfo != null, "tgaInfo must not be nil");
            Debug.Assert(position.x < m_pTGAInfo.width, "Invalid position.x");
            Debug.Assert(position.y < m_pTGAInfo.height, "Invalid position.y");

            return m_pTGAInfo.imageData[position.x + position.y * m_pTGAInfo.width];
        }

        public void SetTile(Color tile, ccGridSize position)
        {
            Debug.Assert(m_pTGAInfo != null, "tgaInfo must not be nil");
            Debug.Assert(m_pPosToAtlasIndex != null, "posToAtlasIndex must not be nil");
            Debug.Assert(position.x < m_pTGAInfo.width, "Invalid position.x");
            Debug.Assert(position.y < m_pTGAInfo.height, "Invalid position.x");
            Debug.Assert(tile.R != 0, "R component must be non 0");

            Color value = m_pTGAInfo.imageData[position.x + position.y * m_pTGAInfo.width];
            if (value.R == 0)
            {
                CCLog.Log("cocos2d: Value.r must be non 0.");
            }
            else
            {
                m_pTGAInfo.imageData[position.x + position.y * m_pTGAInfo.width] = tile;

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

            m_pTGAInfo = tImageTGA.tgaLoad(CCFileUtils.FullPathFromRelativePath(file));
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

        private void UpdateAtlasValueAt(ccGridSize pos, Color value, int index)
        {
            int x = pos.x;
            int y = pos.y;

            float row = (float)(value.R % m_uItemsPerRow);
            float col = (float)(value.R / m_uItemsPerRow);

            float textureWide = (m_pTextureAtlas.Texture.PixelsWide);
            float textureHigh = (m_pTextureAtlas.Texture.PixelsHigh);

            float itemWidthInPixels = m_uItemWidth * ccMacros.CC_CONTENT_SCALE_FACTOR();
            float itemHeightInPixels = m_uItemHeight * ccMacros.CC_CONTENT_SCALE_FACTOR();

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

            ccV3F_C4B_T2F_Quad quad;

            quad.tl.texCoords.u = left;
            quad.tl.texCoords.v = top;
            quad.tr.texCoords.u = right;
            quad.tr.texCoords.v = top;
            quad.bl.texCoords.u = left;
            quad.bl.texCoords.v = bottom;
            quad.br.texCoords.u = right;
            quad.br.texCoords.v = bottom;

            quad.bl.vertices.X = (x * m_uItemWidth);
            quad.bl.vertices.Y = (y * m_uItemHeight);
            quad.bl.vertices.Z = 0.0f;
            quad.br.vertices.X = (x * m_uItemWidth + m_uItemWidth);
            quad.br.vertices.Y = (y * m_uItemHeight);
            quad.br.vertices.Z = 0.0f;
            quad.tl.vertices.X = (x * m_uItemWidth);
            quad.tl.vertices.Y = (y * m_uItemHeight + m_uItemHeight);
            quad.tl.vertices.Z = 0.0f;
            quad.tr.vertices.X = (x * m_uItemWidth + m_uItemWidth);
            quad.tr.vertices.Y = (y * m_uItemHeight + m_uItemHeight);
            quad.tr.vertices.Z = 0.0f;

            var color = new CCColor4B(m_tColor.R, m_tColor.G, m_tColor.B, m_cOpacity);
            quad.tr.colors = color;
            quad.tl.colors = color;
            quad.br.colors = color;
            quad.bl.colors = color;

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
                            var pos = new ccGridSize(x, y);
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