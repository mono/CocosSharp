using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
	public class CCPrimitiveBatch : IDisposable
    {
		const int DefaultBufferSize = 500;
  
        readonly CCV3F_C4B[] lineVertices;
        readonly CCV3F_C4B[] triangleVertices;

        // hasBegun is flipped to true once Begin is called, and is used to make
        // sure users don't call End before Begin is called.
        bool hasBegun;
        bool isDisposed;

        int lineVertsCount;
        int triangleVertsCount;

        CCRawList<CCV3F_C4B[]> triangleVerts;
        CCRawList<CCV3F_C4B[]> lineVerts;

        #region Properties

        internal CCDrawManager DrawManager { get; set; }

        #endregion Properties


		#region Constructors

        internal CCPrimitiveBatch(CCDrawManager drawManager, int bufferSize=DefaultBufferSize)
        {
            DrawManager = drawManager;

            if (drawManager.XnaGraphicsDevice == null)
            {
                throw new ArgumentNullException("graphicsDevice");
            }
  
            triangleVertices = new CCV3F_C4B[bufferSize - bufferSize % 3];
            lineVertices = new CCV3F_C4B[bufferSize - bufferSize % 2];

            triangleVerts = new CCRawList<CCV3F_C4B[]>(100, true);
            lineVerts = new CCRawList<CCV3F_C4B[]>(100, true);

        }

        public CCPrimitiveBatch(int bufferSize=DefaultBufferSize)
            : this (CCDrawManager.SharedDrawManager, bufferSize)
        {  }

        #endregion Constructors


		#region Cleaning up

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !isDisposed)
			{
				isDisposed = true;
			}
		}

		#endregion Cleaning up


        // Begin is called to tell the PrimitiveBatch what kind of primitives will be
        // drawn, and to prepare the graphics card to render those primitives.
        public void Begin()
        {
            if (hasBegun)
            {
                throw new InvalidOperationException("End must be called before Begin can be called again.");
            }

            // flip the error checking boolean. It's now ok to call AddVertex, Flush,
            // and End.
            hasBegun = true;
        }

        public bool IsReady()
        {
            return hasBegun;
        }

		public void AddVertex(CCVector2 vertex, CCColor4B color, PrimitiveType primitiveType)
        {
			AddVertex(ref vertex, color, primitiveType);
        }

		public void AddVertex(ref CCVector2 vertex, CCColor4B color, PrimitiveType primitiveType)
        {
            if (!hasBegun)
            {
                throw new InvalidOperationException("Begin must be called before AddVertex can be called.");
            }

            if (primitiveType == PrimitiveType.LineStrip || primitiveType == PrimitiveType.TriangleStrip)
            {
                throw new NotSupportedException("The specified primitiveType is not supported by PrimitiveBatch.");
            }

            if (primitiveType == PrimitiveType.TriangleList)
            {
                if (triangleVertsCount >= triangleVertices.Length)
                {
                    FlushTriangles();
                }
                triangleVertices[triangleVertsCount].Vertices = new CCVertex3F(vertex.X, vertex.Y, -0.1f);
				triangleVertices[triangleVertsCount].Colors = color;
                triangleVertsCount++;
            }

            if (primitiveType == PrimitiveType.LineList)
            {
                if (lineVertsCount >= lineVertices.Length)
                {
                    FlushLines();
                }
                lineVertices[lineVertsCount].Vertices = new CCVertex3F(vertex.X, vertex.Y, 0.0f);
				lineVertices[lineVertsCount].Colors = color;
                lineVertsCount++;
            }
        }

        public void AddVertex(CCVertex3F vertex, CCColor4B color, PrimitiveType primitiveType)
        {
            AddVertex(new CCVector2(vertex.X, vertex.Y), color, primitiveType);
        }

        // End is called once all the primitives have been drawn using AddVertex.
        // it will call Flush to actually submit the draw call to the graphics card, and
        // then tell the basic effect to end.
        public void End()
        {
            if (!hasBegun)
            {
                throw new InvalidOperationException("Begin must be called before End can be called.");
            }

            // Draw whatever the user wanted us to draw
            FlushTriangles();
            FlushLines();

            DrawManager.Renderer.AddCommand(
                new CCCustomCommand(0, new CCAffineTransform(DrawManager.WorldMatrix), Flush));

            hasBegun = false;
        }

        void Flush()
        {

            // Make sure that TextureEnabled is false (not enabled) or primitives will not show up.
            DrawManager.TextureEnabled = false;
            // Make sure that VertexColor is true (enabled) or primitives will not show up.
            DrawManager.VertexColorEnabled = true;

            while (triangleVerts.Count > 0)
            {
                var triangle = triangleVerts.Pop();
                DrawManager.DrawPrimitives(PrimitiveType.TriangleList, triangle, 0, triangle.Length / 3);

            }

            while (lineVerts.Count > 0)
            {
                var line = lineVerts.Pop();
                DrawManager.DrawPrimitives(PrimitiveType.LineList, line, 0, line.Length / 2);
            }


            hasBegun = false;
        }


        void FlushTriangles()
        {
            if (!hasBegun)
            {
                throw new InvalidOperationException("Begin must be called before Flush can be called.");
            }

            if (triangleVertsCount >= 3)
            {
                int primitiveCount = triangleVertsCount / 3;

                var triangles = new CCV3F_C4B[triangleVertsCount];
                Array.Copy(triangleVertices, triangles, triangleVertsCount);
                // add the Triangle List to our triangles list vertices for later rendering from the Renderer
                triangleVerts.Add(triangles);

                triangleVertsCount -= primitiveCount * 3;

            }
        }

        void FlushLines()
        {
            if (!hasBegun)
            {
                throw new InvalidOperationException("Begin must be called before Flush can be called.");
            }

            if (lineVertsCount >= 2)
            {
                int primitiveCount = lineVertsCount / 2;

                var lines = new CCV3F_C4B[lineVertsCount];
                Array.Copy(lineVertices, lines, triangleVertsCount);
                // add the Line Lists to our line list vertices for later rendering from the Renderer
                lineVerts.Add(lines);

                lineVertsCount -= primitiveCount * 2;

            }
        }
    }
}