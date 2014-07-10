using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
	public class CCPrimitiveBatch : IDisposable
    {
		const int DefaultBufferSize = 500;

        // a basic effect, which contains the shaders that we will use to draw our
        // primitives.
        readonly BasicEffect basicEffect;

        // the device that we will issue draw calls to.
        readonly GraphicsDevice device;
        readonly VertexPositionColor[] lineVertices;
        readonly VertexPositionColor[] triangleVertices;

        // hasBegun is flipped to true once Begin is called, and is used to make
        // sure users don't call End before Begin is called.
        bool hasBegun;
        bool isDisposed;

        int lineVertsCount;
        int triangleVertsCount;

        #region Properties

        public CCDrawManager DrawManager { get; set; }

        #endregion Properties


		#region Constructors

        public CCPrimitiveBatch(CCDrawManager drawManager, int bufferSize=DefaultBufferSize)
        {
            DrawManager = drawManager;

            if (drawManager.XnaGraphicsDevice == null)
            {
                throw new ArgumentNullException("graphicsDevice");
            }
            device = drawManager.XnaGraphicsDevice;

            triangleVertices = new VertexPositionColor[bufferSize - bufferSize % 3];
            lineVertices = new VertexPositionColor[bufferSize - bufferSize % 2];

            // set up a new basic effect, and enable vertex colors.
            basicEffect = new BasicEffect(drawManager.XnaGraphicsDevice);
            basicEffect.VertexColorEnabled = true;
        }

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
				if (basicEffect != null)
					basicEffect.Dispose();

				isDisposed = true;
			}
		}

		#endregion Cleaning up


		public void SetProjection(ref Matrix projection)
        {
            basicEffect.Projection = projection;
        }
			
        // Begin is called to tell the PrimitiveBatch what kind of primitives will be
        // drawn, and to prepare the graphics card to render those primitives.
        public void Begin()
        {
            if (hasBegun)
            {
                throw new InvalidOperationException("End must be called before Begin can be called again.");
            }

            //tell our basic effect to begin.
            UpdateMatrix();

            // flip the error checking boolean. It's now ok to call AddVertex, Flush,
            // and End.
            hasBegun = true;
        }

        public void UpdateMatrix()
        {
            basicEffect.Projection = DrawManager.ProjectionMatrix; ;
            basicEffect.View = DrawManager.ViewMatrix;
            basicEffect.World = DrawManager.WorldMatrix;
            basicEffect.CurrentTechnique.Passes[0].Apply();
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
				triangleVertices[triangleVertsCount].Position = new Vector3(vertex.ToVector2(), -0.1f);
				triangleVertices[triangleVertsCount].Color = color.ToColor();
                triangleVertsCount++;
            }

            if (primitiveType == PrimitiveType.LineList)
            {
                if (lineVertsCount >= lineVertices.Length)
                {
                    FlushLines();
                }
				lineVertices[lineVertsCount].Position = new Vector3(vertex.ToVector2(), 0f);
				lineVertices[lineVertsCount].Color = color.ToColor();
                lineVertsCount++;
            }
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
                // submit the draw call to the graphics card
#if NETFX_CORE
                device.SamplerStates[0] = SamplerState.LinearClamp;
#else
                device.SamplerStates[0] = SamplerState.AnisotropicClamp;
#endif
                device.DrawUserPrimitives(PrimitiveType.TriangleList, triangleVertices, 0, primitiveCount);
                triangleVertsCount -= primitiveCount * 3;

                DrawManager.DrawCount++;
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
                // submit the draw call to the graphics card
#if NETFX_CORE
                device.SamplerStates[0] = SamplerState.LinearClamp;
#else
                device.SamplerStates[0] = SamplerState.AnisotropicClamp;
#endif
                device.DrawUserPrimitives(PrimitiveType.LineList, lineVertices, 0, primitiveCount);
                lineVertsCount -= primitiveCount * 2;

                DrawManager.DrawCount++;
            }
        }
    }
}