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
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public class CCTiledGrid3D : CCGridBase
    {
        bool dirty;

        CCIndexBuffer<short> indexBuffer;
        CCVertexBuffer<CCV3F_T2F> vertexBuffer;


        #region Properties

        protected short[] Indices { get; private set; }
        protected CCQuad3[] OriginalVertices { get; private set; }
        internal CCV3F_T2F[] Vertices { get; private set; }

        #endregion Properties


        #region Constructors

        public CCTiledGrid3D(CCGridSize gridSize, CCTexture2D texture, bool flipped=false) : base(gridSize, texture, flipped)
        {
        }

        #endregion Constructors


        #region Tile Indexers and accessors

        public CCQuad3 this[CCGridSize pos]
        {
            get { return this[pos.X, pos.Y]; }
            set {
                this[pos.X, pos.Y] = value;
            }
        }

        public CCQuad3 this[int x, int y]
        {
            get 
            { 
                int idx = (GridSize.Y * x + y) * 4;

                CCV3F_T2F[] vertArray = Vertices;

                return new CCQuad3
                {
                    BottomLeft = vertArray[idx + 0].Vertices,
                    BottomRight = vertArray[idx + 1].Vertices,
                    TopLeft = vertArray[idx + 2].Vertices,
                    TopRight = vertArray[idx + 3].Vertices
                };
            }
            set 
            {
                int idx = (GridSize.Y * x + y) * 4;

                CCV3F_T2F[] vertArray = Vertices;

                vertArray[idx + 0].Vertices = value.BottomLeft;
                vertArray[idx + 1].Vertices = value.BottomRight;
                vertArray[idx + 2].Vertices = value.TopLeft;
                vertArray[idx + 3].Vertices = value.TopRight;

                dirty = true;
            }
        }


        // returns the original tile (untransformed) at the given position
        public CCQuad3 OriginalTile(CCGridSize pos)
        {
            return OriginalTile(pos.X, pos.Y);
        }

        // returns the original tile (untransformed) at the given position
        public CCQuad3 OriginalTile(int x, int y)
        {
            int idx = (GridSize.Y * x + y);
            return OriginalVertices[idx];
        }

        #endregion Tile Indexers and accessors


        public override void Blit()
        {
            if (dirty)
            {
                vertexBuffer.UpdateBuffer();
            }

            base.Blit();

            CCDrawManager drawManager = Scene.Window.DrawManager;
            bool save = drawManager.VertexColorEnabled;

            drawManager.VertexColorEnabled = false;
            drawManager.DrawBuffer(vertexBuffer, indexBuffer, 0, Indices.Length / 3);
            drawManager.VertexColorEnabled = save;
        }

        public override void Reuse()
        {
            if (ReuseGrid > 0)
            {
                int numQuads = GridSize.X * GridSize.Y;

                CCQuad3[] orig = OriginalVertices;
                CCV3F_T2F[] verts = Vertices;

                for (int i = 0; i < numQuads; i++)
                {
                    int i4 = i * 4;
                    orig[i].BottomLeft = verts[i4 + 0].Vertices;
                    orig[i].BottomRight = verts[i4 + 1].Vertices;
                    orig[i].TopLeft = verts[i4 + 2].Vertices;
                    orig[i].TopRight = verts[i4 + 3].Vertices;
                }

                --ReuseGrid;
            }
        }

        public override void CalculateVertexPoints()
        {
            float width = Texture.PixelsWide;
            float height = Texture.PixelsHigh;
            float imageH = Texture.ContentSizeInPixels.Height;

            int numQuads = GridSize.X * GridSize.Y;

            vertexBuffer = new CCVertexBuffer<CCV3F_T2F>(numQuads * 4, CCBufferUsage.WriteOnly);
            vertexBuffer.Count = numQuads * 4;
            indexBuffer = new CCIndexBuffer<short>(numQuads * 6, BufferUsage.WriteOnly);
            indexBuffer.Count = numQuads * 6;

            Vertices = vertexBuffer.Data.Elements;
            Indices = indexBuffer.Data.Elements;

            OriginalVertices = new CCQuad3[numQuads];

            CCV3F_T2F[] vertArray = Vertices;
            short[] idxArray = Indices;


            int index = 0;

            for (int x = 0; x < GridSize.X; x++)
            {
                for (int y = 0; y < GridSize.Y; y++)
                {
                    float x1 = x * Step.X;
                    float x2 = x1 + Step.X;
                    float y1 = y * Step.Y;
                    float y2 = y1 + Step.Y;

                    vertArray[index + 0].Vertices = new CCVertex3F(x1, y1, 0);
                    vertArray[index + 1].Vertices = new CCVertex3F(x2, y1, 0);
                    vertArray[index + 2].Vertices = new CCVertex3F(x1, y2, 0);
                    vertArray[index + 3].Vertices = new CCVertex3F(x2, y2, 0);

                    float newY1 = y1;
                    float newY2 = y2;

                    if (!TextureFlipped)
                    {
                        newY1 = imageH - y1;
                        newY2 = imageH - y2;
                    }

                    vertArray[index + 0].TexCoords = new CCTex2F(x1 / width, newY1 / height);
                    vertArray[index + 1].TexCoords = new CCTex2F(x2 / width, newY1 / height);
                    vertArray[index + 2].TexCoords = new CCTex2F(x1 / width, newY2 / height);
                    vertArray[index + 3].TexCoords = new CCTex2F(x2 / width, newY2 / height);

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

            indexBuffer.UpdateBuffer();

            for (int i = 0; i < numQuads; i++)
            {
                int i4 = i * 4;
                OriginalVertices[i].BottomLeft = vertArray[i4 + 0].Vertices;
                OriginalVertices[i].BottomRight = vertArray[i4 + 1].Vertices;
                OriginalVertices[i].TopLeft = vertArray[i4 + 2].Vertices;
                OriginalVertices[i].TopRight = vertArray[i4 + 3].Vertices;
            }
        }

    }
}