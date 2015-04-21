using System;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    internal class CCPrimitive <T, T2> 
        where T : struct, IVertexType
        where T2 : struct
    {
        #region Properties

        CCVertexBuffer<T> VertexBuffer { get; set; }
        CCIndexBuffer<T2> IndexBuffer { get; set; }
        PrimitiveType PrimitiveType { get; set; }

        #endregion Properties


        #region Constructors

        public CCPrimitive (CCVertexBuffer<T> vertexBuffer, 
            CCIndexBuffer<T2> indexBuffer = null, 
            PrimitiveType primitiveType = PrimitiveType.TriangleList)
        {
            VertexBuffer = vertexBuffer;
            IndexBuffer = indexBuffer;
            PrimitiveType = primitiveType;
        }

        #endregion Constructors


        public void Draw (CCDrawManager drawManager)
        {
            if (VertexBuffer == null || VertexBuffer.Count == 0)
                return;

            if (IndexBuffer != null)
            {
                VertexBuffer.UpdateBuffer();
                IndexBuffer.UpdateBuffer();

                drawManager.DrawBuffer(VertexBuffer, IndexBuffer, 0, IndexBuffer.Count / 3);
            }
            else
                drawManager.DrawPrimitives(PrimitiveType, VertexBuffer.Data.Elements, 0, VertexBuffer.Count);

        }
    }
}

