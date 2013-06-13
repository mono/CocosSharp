using Microsoft.Xna.Framework.Graphics;

namespace Cocos2D
{
    internal struct CCV3F_T2F : IVertexType
    {
        internal static readonly VertexDeclaration VertexDeclaration;

        /// <summary>
        /// vertices (3F)
        /// </summary>
        internal CCVertex3F vertices; // 12 bytes

        /// <summary>
        /// tex coords (2F)
        /// </summary>
        internal CCTex2F texCoords; // 8 byts

        static CCV3F_T2F()
        {
            var elements = new[]
                {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
                };
            VertexDeclaration = new VertexDeclaration(elements);
        }

        #region IVertexType Members

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }

        #endregion
    }

    /// <summary> 
    /// CCGrid3D is a 3D grid implementation. Each vertex has 3 dimensions: x,y,z
    /// </summary>
    public class CCGrid3D : CCGridBase
    {
        private bool m_bDirty;
        private CCIndexBuffer<ushort> m_pIndexBuffer;
        protected ushort[] m_pIndices;
        protected CCVertex3F[] m_pOriginalVertices;
        private CCVertexBuffer<CCV3F_T2F> m_pVertexBuffer;
        internal CCV3F_T2F[] m_pVertices;

        //protected CCPoint[] m_pTexCoordinates;
        //protected ccVertex3F[] m_pVertices;

        /// <summary>
        /// returns the vertex at a given position
        /// </summary>
        public CCVertex3F Vertex(CCGridSize pos)
        {
            return m_pVertices[pos.X * (m_sGridSize.Y + 1) + pos.Y].vertices;
        }

        /// <summary>
        /// returns the original (non-transformed) vertex at a given position
        /// </summary>
        public CCVertex3F OriginalVertex(CCGridSize pos)
        {
            return m_pOriginalVertices[pos.X * (m_sGridSize.Y + 1) + pos.Y];
        }

        /// <summary>
        /// sets a new vertex at a given position
        /// </summary>
        public void SetVertex(CCGridSize pos, ref CCVertex3F vertex)
        {
            m_pVertices[pos.X * (m_sGridSize.Y + 1) + pos.Y].vertices = vertex;
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
                for (int i = 0, count = (m_sGridSize.X + 1) * (m_sGridSize.Y + 1); i < count; i++)
                {
                    m_pOriginalVertices[i] = m_pVertices[i].vertices;
                }
                --m_nReuseGrid;
            }
        }

        public override void CalculateVertexPoints()
        {
            float width = m_pTexture.PixelsWide;
            float height = m_pTexture.PixelsHigh;
            float imageH = m_pTexture.ContentSizeInPixels.Height;

            int numOfPoints = (m_sGridSize.X + 1) * (m_sGridSize.Y + 1);

            m_pVertexBuffer = new CCVertexBuffer<CCV3F_T2F>(numOfPoints, BufferUsage.WriteOnly);
            m_pVertexBuffer.Count = numOfPoints;
            m_pIndexBuffer = new CCIndexBuffer<ushort>(m_sGridSize.X * m_sGridSize.Y * 6, BufferUsage.WriteOnly);
            m_pIndexBuffer.Count = m_sGridSize.X * m_sGridSize.Y * 6;

            m_pVertices = m_pVertexBuffer.Data.Elements;
            m_pIndices = m_pIndexBuffer.Data.Elements;

            m_pOriginalVertices = new CCVertex3F[numOfPoints];

            CCV3F_T2F[] vertArray = m_pVertices;
            ushort[] idxArray = m_pIndices;

            var l1 = new int[4];
            var l2 = new CCVertex3F[4];
            var tex1 = new int[4];
            var tex2 = new CCPoint[4];

            //int idx = -1;
            for (int x = 0; x < m_sGridSize.X; ++x)
            {
                for (int y = 0; y < m_sGridSize.Y; ++y)
                {
                    float x1 = x * m_obStep.X;
                    float x2 = x1 + m_obStep.X;
                    float y1 = y * m_obStep.Y;
                    float y2 = y1 + m_obStep.Y;

                    var a = (short) (x * (m_sGridSize.Y + 1) + y);
                    var b = (short) ((x + 1) * (m_sGridSize.Y + 1) + y);
                    var c = (short) ((x + 1) * (m_sGridSize.Y + 1) + (y + 1));
                    var d = (short) (x * (m_sGridSize.Y + 1) + (y + 1));

                    int idx = ((y * m_sGridSize.X) + x) * 6;

                    idxArray[idx + 0] = (ushort) a;
                    idxArray[idx + 1] = (ushort) b;
                    idxArray[idx + 2] = (ushort) d;
                    idxArray[idx + 3] = (ushort) b;
                    idxArray[idx + 4] = (ushort) c;
                    idxArray[idx + 5] = (ushort) d;

                    //var tempidx = new short[6] {a, d, b, b, d, c};
                    //Array.Copy(tempidx, 0, idxArray, 6 * idx, tempidx.Length);

                    l1[0] = a;
                    l1[1] = b;
                    l1[2] = c;
                    l1[3] = d;

                    //var e = new Vector3(x1, y1, 0);
                    //var f = new Vector3(x2, y1, 0);
                    //var g = new Vector3(x2, y2, 0);
                    //var h = new Vector3(x1, y2, 0);

                    l2[0] = new CCVertex3F(x1, y1, 0);
                    l2[1] = new CCVertex3F(x2, y1, 0);
                    l2[2] = new CCVertex3F(x2, y2, 0);
                    l2[3] = new CCVertex3F(x1, y2, 0);

                    tex1[0] = a;
                    tex1[1] = b;
                    tex1[2] = c;
                    tex1[3] = d;

                    tex2[0] = new CCPoint(x1, y1);
                    tex2[1] = new CCPoint(x2, y1);
                    tex2[2] = new CCPoint(x2, y2);
                    tex2[3] = new CCPoint(x1, y2);

                    for (int i = 0; i < 4; ++i)
                    {
                        vertArray[l1[i]].vertices = l2[i];

                        vertArray[tex1[i]].texCoords.U = tex2[i].X / width;

                        if (m_bIsTextureFlipped)
                        {
                            vertArray[tex1[i]].texCoords.V = tex2[i].Y / height;
                        }
                        else
                        {
                            vertArray[tex1[i]].texCoords.V = (imageH - tex2[i].Y) / height;
                        }
                    }
                }
            }

            int n = (m_sGridSize.X + 1) * (m_sGridSize.Y + 1);
            for (int i = 0; i < n; i++)
            {
                m_pOriginalVertices[i] = m_pVertices[i].vertices;
            }

            m_pIndexBuffer.UpdateBuffer();

            m_bDirty = true;
        }

        public CCGrid3D(CCGridSize gridSize, CCTexture2D pTexture, bool bFlipped)
        {
            InitWithSize(gridSize, pTexture, bFlipped);
        }

        public CCGrid3D(CCGridSize gridSize)
        {
            InitWithSize(gridSize);
        }

        public CCGrid3D(CCGridSize gridSize, CCSize size)
        {
            InitWithSize(gridSize, size);
        }
    }
}