using Microsoft.Xna.Framework.Graphics;

namespace cocos2d
{
    public struct ccV3F_T2F : IVertexType
    {
        public static readonly VertexDeclaration VertexDeclaration;

        /// <summary>
        /// vertices (3F)
        /// </summary>
        public ccVertex3F vertices; // 12 bytes

        /// <summary>
        /// tex coords (2F)
        /// </summary>
        public ccTex2F texCoords; // 8 byts

        static ccV3F_T2F()
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
        private IndexBuffer m_pIndexBuffer;
        protected ushort[] m_pIndices;
        protected ccVertex3F[] m_pOriginalVertices;
        private VertexBuffer m_pVertexBuffer;
        protected ccV3F_T2F[] m_pVertices;

        //protected CCPoint[] m_pTexCoordinates;
        //protected ccVertex3F[] m_pVertices;

        /// <summary>
        /// returns the vertex at a given position
        /// </summary>
        public ccVertex3F Vertex(ccGridSize pos)
        {
            return m_pVertices[pos.x * (m_sGridSize.y + 1) + pos.y].vertices;
        }

        /// <summary>
        /// returns the original (non-transformed) vertex at a given position
        /// </summary>
        public ccVertex3F OriginalVertex(ccGridSize pos)
        {
            return m_pOriginalVertices[pos.x * (m_sGridSize.y + 1) + pos.y];
        }

        /// <summary>
        /// sets a new vertex at a given position
        /// </summary>
        public void SetVertex(ccGridSize pos, ref ccVertex3F vertex)
        {
            m_pVertices[pos.x * (m_sGridSize.y + 1) + pos.y].vertices = vertex;
            m_bDirty = true;
        }

        public override void Blit()
        {
            if (m_pVertexBuffer == null || m_pVertexBuffer.VertexCount < m_pVertices.Length)
            {
                m_pVertexBuffer = new VertexBuffer(DrawManager.graphicsDevice, typeof(ccV3F_T2F), m_pVertices.Length, BufferUsage.WriteOnly);
            }

            if (m_pIndexBuffer == null || m_pIndexBuffer.IndexCount < m_pIndices.Length)
            {
                m_pIndexBuffer = new IndexBuffer(DrawManager.graphicsDevice, typeof(ushort), m_pIndices.Length, BufferUsage.WriteOnly);
                m_pIndexBuffer.SetData(m_pIndices, 0, m_pIndices.Length);
            }

            if (m_bDirty)
            {
                m_pVertexBuffer.SetData(m_pVertices, 0, m_pVertices.Length);
            }

            bool save = DrawManager.VertexColorEnabled;

            DrawManager.VertexColorEnabled = false;
            DrawManager.DrawBuffer(m_pVertexBuffer, m_pIndexBuffer, 0, m_pIndices.Length / 3);
            DrawManager.VertexColorEnabled = save;
        }

        public override void Reuse()
        {
            if (m_nReuseGrid > 0)
            {
                for (int i = 0, count = (m_sGridSize.x + 1) * (m_sGridSize.y + 1); i < count; i++)
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

            int numOfPoints = (m_sGridSize.x + 1) * (m_sGridSize.y + 1);

            m_pVertices = new ccV3F_T2F[numOfPoints];
            m_pOriginalVertices = new ccVertex3F[numOfPoints];
            //m_pTexCoordinates = new CCPoint[numOfPoints];
            m_pIndices = new ushort[m_sGridSize.x * m_sGridSize.y * 6];

            ccV3F_T2F[] vertArray = m_pVertices;
            //var texArray = m_pTexCoordinates;
            ushort[] idxArray = m_pIndices;

            var l1 = new int[4];
            var l2 = new ccVertex3F[4];
            var tex1 = new int[4];
            var tex2 = new CCPoint[4];

            //int idx = -1;
            for (int x = 0; x < m_sGridSize.x; ++x)
            {
                for (int y = 0; y < m_sGridSize.y; ++y)
                {
                    float x1 = x * m_obStep.x;
                    float x2 = x1 + m_obStep.x;
                    float y1 = y * m_obStep.y;
                    float y2 = y1 + m_obStep.y;

                    var a = (short) (x * (m_sGridSize.y + 1) + y);
                    var b = (short) ((x + 1) * (m_sGridSize.y + 1) + y);
                    var c = (short) ((x + 1) * (m_sGridSize.y + 1) + (y + 1));
                    var d = (short) (x * (m_sGridSize.y + 1) + (y + 1));

                    int idx = ((y * m_sGridSize.x) + x) * 6;

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

                    l2[0] = new ccVertex3F(x1, y1, 0);
                    l2[1] = new ccVertex3F(x2, y1, 0);
                    l2[2] = new ccVertex3F(x2, y2, 0);
                    l2[3] = new ccVertex3F(x1, y2, 0);

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

                        vertArray[tex1[i]].texCoords.u = tex2[i].x / width;

                        if (m_bIsTextureFlipped)
                        {
                            vertArray[tex1[i]].texCoords.v = tex2[i].y / height;
                        }
                        else
                        {
                            vertArray[tex1[i]].texCoords.v = (imageH - tex2[i].y) / height;
                        }
                    }
                }
            }

            int n = (m_sGridSize.x + 1) * (m_sGridSize.y + 1);
            for (int i = 0; i < n; i++)
            {
                m_pOriginalVertices[i] = m_pVertices[i].vertices;
            }

            m_bDirty = true;
        }

        public static CCGrid3D Create(ccGridSize gridSize, CCTexture2D pTexture, bool bFlipped)
        {
            var pRet = new CCGrid3D();
            if (pRet.InitWithSize(gridSize, pTexture, bFlipped))
            {
                return pRet;
            }
            return null;
        }

        public static CCGrid3D Create(ccGridSize gridSize)
        {
            var pRet = new CCGrid3D();
            if (pRet.InitWithSize(gridSize))
            {
                return pRet;
            }
            return null;
        }

        public static CCGrid3D Create(ccGridSize gridSize, CCSize size)
        {
            var pRet = new CCGrid3D();
            if (pRet.InitWithSize(gridSize, size))
            {
                return pRet;
            }
            return null;
        }
    }
}