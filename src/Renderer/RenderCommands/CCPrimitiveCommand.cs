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

        public CCPrimitiveCommand (float globalDepth, CCAffineTransform worldTransform, CCPrimitive<T,T2> primitive, 
            CCBlendFunc blendType)
            : this (globalDepth, worldTransform, null, primitive, blendType)
        {        }

        public CCPrimitiveCommand (float globalDepth, CCAffineTransform worldTransform, CCTexture2D texture, CCPrimitive<T,T2> primitive, 
            CCBlendFunc blendType)
            : base(globalDepth, worldTransform)
        {
            Primitive = primitive;
            BlendType = blendType;
            Texture = texture;
        }

        internal override void RequestRenderCommand(CCRenderer renderer)
        {
            renderer.ProcessPrimitiveRenderCommand (this);
        }

        internal void UseMaterial(CCDrawManager drawManager)
        {
            drawManager.BlendFunc(BlendType);
            drawManager.BindTexture(Texture);
        }

    }
}

