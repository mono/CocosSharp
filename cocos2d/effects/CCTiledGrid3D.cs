/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.
Copyright (c) 2011-2012 openxlive.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

using Microsoft.Xna.Framework.Graphics;

namespace Cocos2D
{
    public class CCTiledGrid3D : CCGridBase
    {
        private bool m_bDirty;
        private CCIndexBuffer<short> m_pIndexBuffer;
        protected short[] m_pIndices;
        protected CCQuad3[] m_pOriginalVertices;
        private CCVertexBuffer<CCV3F_T2F> m_pVertexBuffer;
        internal CCV3F_T2F[] m_pVertices;

        /// <summary>
        ///  returns the tile at the given position
        /// </summary>
        public CCQuad3 Tile(CCGridSize pos)
        {
            int idx = (m_sGridSize.Y * pos.X + pos.Y) * 4;

            CCV3F_T2F[] vertArray = m_pVertices;

            return new CCQuad3
                {
                    BottomLeft = vertArray[idx + 0].vertices,
                    BottomRight = vertArray[idx + 1].vertices,
                    TopLeft = vertArray[idx + 2].vertices,
                    TopRight = vertArray[idx + 3].vertices
                };
        }

        /// <summary>
        /// returns the original tile (untransformed) at the given position
        /// </summary>
        public CCQuad3 OriginalTile(CCGridSize pos)
        {
            int idx = (m_sGridSize.Y * pos.X + pos.Y);
            return m_pOriginalVertices[idx];
        }

        /// <summary>
        /// sets a new tile
        /// </summary>
        public void SetTile(CCGridSize pos, ref CCQuad3 coords)
        {
            int idx = (m_sGridSize.Y * pos.X + pos.Y) * 4;

            CCV3F_T2F[] vertArray = m_pVertices;

            vertArray[idx + 0].vertices = coords.BottomLeft;
            vertArray[idx + 1].vertices = coords.BottomRight;
            vertArray[idx + 2].vertices = coords.TopLeft;
            vertArray[idx + 3].vertices = coords.TopRight;

            m_bDirty = true;
        }

        public override void Blit()
        {
            if (m_bDirty)
            {
                m_pVertexBuffer.UpdateBuffer();
            }

            bool save = CCDrawManager.VertexColorEnabled;
            CCDrawManager.VertexColorEnabled = false;
            CCDrawManager.DrawBuffer(m_pVertexBuffer, m_pIndexBuffer, 0, m_pIndices.Length / 3);
            CCDrawManager.VertexColorEnabled = save;
        }

        public override void Reuse()
        {
            if (m_nReuseGrid > 0)
            {
                int numQuads = m_sGridSize.X * m_sGridSize.Y;

                CCQuad3[] orig = m_pOriginalVertices;
                CCV3F_T2F[] verts = m_pVertices;

                for (int i = 0; i < numQuads; i++)
                {
                    int i4 = i * 4;
                    orig[i].BottomLeft = verts[i4 + 0].vertices;
                    orig[i].BottomRight = verts[i4 + 1].vertices;
                    orig[i].TopLeft = verts[i4 + 2].vertices;
                    orig[i].TopRight = verts[i4 + 3].vertices;
                }

                --m_nReuseGrid;
            }
        }

        public override void CalculateVertexPoints()
        {
            float width = m_pTexture.PixelsWide;
            float height = m_pTexture.PixelsHigh;
            float imageH = m_pTexture.ContentSizeInPixels.Height;

            int numQuads = m_sGridSize.X * m_sGridSize.Y;

            m_pVertexBuffer = new CCVertexBuffer<CCV3F_T2F>(numQuads * 4, BufferUsage.WriteOnly);
            m_pVertexBuffer.Count = numQuads * 4;
            m_pIndexBuffer = new CCIndexBuffer<short>(numQuads * 6, BufferUsage.WriteOnly);
            m_pIndexBuffer.Count = numQuads * 6;

            m_pVertices = m_pVertexBuffer.Data.Elements;
            m_pIndices = m_pIndexBuffer.Data.Elements;

            m_pOriginalVertices = new CCQuad3[numQuads];

            CCV3F_T2F[] vertArray = m_pVertices;
            short[] idxArray = m_pIndices;


            int index = 0;

            for (int x = 0; x < m_sGridSize.X; x++)
            {
                for (int y = 0; y < m_sGridSize.Y; y++)
                {
                    float x1 = x * m_obStep.X;
                    float x2 = x1 + m_obStep.X;
                    float y1 = y * m_obStep.Y;
                    float y2 = y1 + m_obStep.Y;

                    vertArray[index + 0].vertices = new CCVertex3F(x1, y1, 0);
                    vertArray[index + 1].vertices = new CCVertex3F(x2, y1, 0);
                    vertArray[index + 2].vertices = new CCVertex3F(x1, y2, 0);
                    vertArray[index + 3].vertices = new CCVertex3F(x2, y2, 0);

                    float newY1 = y1;
                    float newY2 = y2;

                    if (!m_bIsTextureFlipped)
                    {
                        newY1 = imageH - y1;
                        newY2 = imageH - y2;
                    }

                    vertArray[index + 0].texCoords = new CCTex2F(x1 / width, newY1 / height);
                    vertArray[index + 1].texCoords = new CCTex2F(x2 / width, newY1 / height);
                    vertArray[index + 2].texCoords = new CCTex2F(x1 / width, newY2 / height);
                    vertArray[index + 3].texCoords = new CCTex2F(x2 / width, newY2 / height);

                    index += 4;
                }
            }

            for (int x = 0; x < numQuads; x++)
            {
                int i6 = x * 6;
                int i4 = x * 4;
                idxArray[i6 + 0] = (short) (i4 + 0);
                idxArray[i6 + 1] = (short) (i4 + 2);
                idxArray[i6 + 2] = (short) (i4 + 1);

                idxArray[i6 + 3] = (short) (i4 + 1);
                idxArray[i6 + 4] = (short) (i4 + 2);
                idxArray[i6 + 5] = (short) (i4 + 3);
            }

            m_pIndexBuffer.UpdateBuffer();

            for (int i = 0; i < numQuads; i++)
            {
                int i4 = i * 4;
                m_pOriginalVertices[i].BottomLeft = vertArray[i4 + 0].vertices;
                m_pOriginalVertices[i].BottomRight = vertArray[i4 + 1].vertices;
                m_pOriginalVertices[i].TopLeft = vertArray[i4 + 2].vertices;
                m_pOriginalVertices[i].TopRight = vertArray[i4 + 3].vertices;
            }
        }

        public CCTiledGrid3D(CCGridSize gridSize, CCTexture2D pTexture, bool bFlipped)
        {
            InitWithSize(gridSize, pTexture, bFlipped);
        }

        public CCTiledGrid3D(CCGridSize gridSize)
        {
            InitWithSize(gridSize);
        }
    }
}