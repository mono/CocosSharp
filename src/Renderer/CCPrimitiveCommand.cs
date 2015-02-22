using System;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    internal class CCPrimitiveCommand <T, T2> : CCRenderCommand
        where T : struct, IVertexType
        where T2 : struct
    {

        public uint MaterialID { get; set; }
        public CCTexture2D Texture { get; set; }
        public CCBlendFunc BlendType { get; set; }
        public CCPrimitive<T, T2> Primitive { get; set; }

        public CCPrimitiveCommand (float globalZOrder, CCPrimitive<T,T2> primitive, 
            CCBlendFunc blendType, CCAffineTransform modelViewTransform, int flags = 0)
            : this (globalZOrder, null, primitive, blendType, modelViewTransform, flags)
        {        }

        public CCPrimitiveCommand (float globalZOrder, CCTexture2D texture, CCPrimitive<T,T2> primitive, 
            CCBlendFunc blendType, CCAffineTransform modelViewTransform, int flags = 0)
            : base(globalZOrder, modelViewTransform, flags)
        {
            CommandType = CommandType.PRIMITIVE_COMMAND;
            Primitive = primitive;
            BlendType = blendType;
            Texture = texture;
        }

        internal override void Execute(CCDrawManager drawManager)
        {
            // Set Texture
            drawManager.BindTexture(Texture);

            // Set blend mode
            drawManager.BlendFunc(BlendType);

            // Draw the Primitives
            Primitive.Draw(drawManager);

            drawManager.DrawCount++;  // Drawn batches
            //drawManager.DrawVerticesCount = Primitive.Count; // Drawn Vertices in this batch
        }
    }
}

