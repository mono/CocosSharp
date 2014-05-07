using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
	#region Structs

	internal struct CCV3F_T2F : IVertexType
    {
        internal static readonly VertexDeclaration VertexDeclaration;

		internal CCVertex3F Vertices; // 12 bytes
		internal CCTex2F TexCoords; // 8 byts

		VertexDeclaration IVertexType.VertexDeclaration
		{
			get { return VertexDeclaration; }
		}

        static CCV3F_T2F()
        {
            var elements = new[]
                {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
                };
            VertexDeclaration = new VertexDeclaration(elements);
        }
    }

	#endregion Structs


    // CCGrid3D is a 3D grid implementation. Each vertex has 3 dimensions: x,y,z
    public class CCGrid3D : CCGridBase
    {
		bool dirty;

		CCIndexBuffer<ushort> indexBuffer;
		CCVertexBuffer<CCV3F_T2F> vertexBuffer;


		#region Properties

		protected ushort[] Indices { get; private set; }
		protected CCVertex3F[] OriginalVertices { get; private set; }
		internal CCV3F_T2F[] Vertices { get; private set; }


		#endregion Properties


		#region Constructors

		public CCGrid3D(CCGridSize gridSize, CCTexture2D texture, bool flipped) : base(gridSize, texture, flipped)
		{
		}

		public CCGrid3D(CCGridSize gridSize) : base(gridSize)
		{
		}

		public CCGrid3D(CCGridSize gridSize, CCSize size) : base(gridSize, size)
		{
		}

		#endregion Constructors


		#region Vertex Indexers

		public CCVertex3F this[CCGridSize pos]
		{
			get { return this[pos.X, pos.Y]; }
			set { this[pos.X, pos.Y] = value; }
		}

		public CCVertex3F this[int x, int y]
		{
			get { return Vertices[x * (GridSize.Y + 1) + y].Vertices; }
			set 
			{
				Vertices[x * (GridSize.Y + 1) + y].Vertices = value;
				dirty = true;
			}
		}

        // returns the original (non-transformed) vertex at a given position
        public CCVertex3F OriginalVertex(CCGridSize pos)
        {
            return OriginalVertices[pos.X * (GridSize.Y + 1) + pos.Y];
        }
			
		// returns the original (non-transformed) vertex at a given position
		public CCVertex3F OriginalVertex(int x, int y)
		{
			return OriginalVertices[x * (GridSize.Y + 1) + y];
		}

		#endregion Vertex Indexers


        public override void Blit()
        {
            if (dirty)
            {
                vertexBuffer.UpdateBuffer();
            }

            bool save = CCDrawManager.VertexColorEnabled;

            CCDrawManager.VertexColorEnabled = false;
            CCDrawManager.DrawBuffer(vertexBuffer, indexBuffer, 0, Indices.Length / 3);
            CCDrawManager.VertexColorEnabled = save;
        }

        public override void Reuse()
        {
            if (ReuseGrid > 0)
            {
                for (int i = 0, count = (GridSize.X + 1) * (GridSize.Y + 1); i < count; i++)
                {
                    OriginalVertices[i] = Vertices[i].Vertices;
                }
                --ReuseGrid;
            }
        }

        public override void CalculateVertexPoints()
        {
            float width = Texture.PixelsWide;
            float height = Texture.PixelsHigh;
            float imageH = Texture.ContentSizeInPixels.Height;

            int numOfPoints = (GridSize.X + 1) * (GridSize.Y + 1);

			vertexBuffer = new CCVertexBuffer<CCV3F_T2F>(numOfPoints, CCBufferUsage.WriteOnly);
            vertexBuffer.Count = numOfPoints;
            indexBuffer = new CCIndexBuffer<ushort>(GridSize.X * GridSize.Y * 6, BufferUsage.WriteOnly);
            indexBuffer.Count = GridSize.X * GridSize.Y * 6;

            Vertices = vertexBuffer.Data.Elements;
            Indices = indexBuffer.Data.Elements;

            OriginalVertices = new CCVertex3F[numOfPoints];

            CCV3F_T2F[] vertArray = Vertices;
            ushort[] idxArray = Indices;

            var l1 = new int[4];
            var l2 = new CCVertex3F[4];
            var tex1 = new int[4];
            var tex2 = new CCPoint[4];

            //int idx = -1;
            for (int x = 0; x < GridSize.X; ++x)
            {
                for (int y = 0; y < GridSize.Y; ++y)
                {
                    float x1 = x * Step.X;
                    float x2 = x1 + Step.X;
                    float y1 = y * Step.Y;
                    float y2 = y1 + Step.Y;

                    var a = (short) (x * (GridSize.Y + 1) + y);
                    var b = (short) ((x + 1) * (GridSize.Y + 1) + y);
                    var c = (short) ((x + 1) * (GridSize.Y + 1) + (y + 1));
                    var d = (short) (x * (GridSize.Y + 1) + (y + 1));

                    int idx = ((y * GridSize.X) + x) * 6;

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
                        vertArray[l1[i]].Vertices = l2[i];

                        vertArray[tex1[i]].TexCoords.U = tex2[i].X / width;

						if (TextureFlipped)
                        {
                            vertArray[tex1[i]].TexCoords.V = tex2[i].Y / height;
                        }
                        else
                        {
                            vertArray[tex1[i]].TexCoords.V = (imageH - tex2[i].Y) / height;
                        }
                    }
                }
            }

            int n = (GridSize.X + 1) * (GridSize.Y + 1);
            for (int i = 0; i < n; i++)
            {
                OriginalVertices[i] = Vertices[i].Vertices;
            }

            indexBuffer.UpdateBuffer();

            dirty = true;
        }
    }
}