using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
	#region Drawing buffer structures

	/// <summary>
	/// a Point with a vertex point, a tex coord point and a color 4B
	/// </summary>
	//TODO: Use VertexPositionColorTexture
	public struct CCV3F_C4B_T2F : IVertexType
	{
		public static readonly VertexDeclaration VertexDeclaration;

		public CCVertex3F Vertices;
		public CCColor4B Colors;
		public CCTex2F TexCoords;

		VertexDeclaration IVertexType.VertexDeclaration
		{
			get { return VertexDeclaration; }
		}

		static CCV3F_C4B_T2F()
		{
			var elements = new VertexElement[]
			{
				new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
				new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0),
				new VertexElement(0x10, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
			};
			VertexDeclaration = new VertexDeclaration(elements);
		}
	}

	/// <summary>
	/// 4 ccVertex2FTex2FColor4B Quad
	/// </summary>
	public class CCV2F_C4B_T2F_Quad
	{
		public CCV2F_C4B_T2F BottomLeft;
		public CCV2F_C4B_T2F BottomRight;
		public CCV2F_C4B_T2F TopLeft;
		public CCV2F_C4B_T2F TopRight;

		public CCV2F_C4B_T2F_Quad()
		{
			BottomLeft = new CCV2F_C4B_T2F();
			BottomRight = new CCV2F_C4B_T2F();
			TopLeft = new CCV2F_C4B_T2F();
			TopRight = new CCV2F_C4B_T2F();
		}
	}

	/// <summary>
	/// 4 ccVertex3FTex2FColor4B
	/// </summary>
	public struct CCV3F_C4B_T2F_Quad : IVertexType
	{
		public static readonly VertexDeclaration VertexDeclaration;

		public CCV3F_C4B_T2F TopLeft;
		public CCV3F_C4B_T2F BottomLeft;
		public CCV3F_C4B_T2F TopRight;
		public CCV3F_C4B_T2F BottomRight;

		VertexDeclaration IVertexType.VertexDeclaration
		{
			get { return VertexDeclaration; }
		}

		static CCV3F_C4B_T2F_Quad()
		{
			var elements = new VertexElement[]
			{
				new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
				new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0),
				new VertexElement(0x10, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
			};
			VertexDeclaration = new VertexDeclaration(elements);
		}
	}

	/// <summary>
	/// 4 ccVertex2FTex2FColor4F Quad
	/// </summary>
	public class CCV2F_C4F_T2F_Quad
	{
		public CCV2F_C4F_T2F BottomLeft;
		public CCV2F_C4F_T2F BottomRight;
		public CCV2F_C4F_T2F TopLeft;
		public CCV2F_C4F_T2F TopRight;


		public CCV2F_C4F_T2F_Quad()
		{
			TopLeft = new CCV2F_C4F_T2F();
			BottomLeft = new CCV2F_C4F_T2F();
			TopRight = new CCV2F_C4F_T2F();
			BottomRight = new CCV2F_C4F_T2F();
		}
	}

	#endregion Drawing buffer structures
}

