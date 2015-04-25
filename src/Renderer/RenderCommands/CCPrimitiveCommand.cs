using System;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    internal class CCPrimitiveCommand <T, T2> : CCRenderCommand
        where T : struct, IVertexType
        where T2 : struct
    {
        #region Properties

        public CCTexture2D Texture { get; private set; }
        public CCBlendFunc BlendType { get; private set; }
        public CCPrimitive<T, T2> Primitive { get; private set; }

        #endregion Properties


        #region Constructors

        public CCPrimitiveCommand (float globalDepth, CCAffineTransform worldTransform, 
            CCPrimitive<T,T2> primitive, CCBlendFunc blendType)
            : this(globalDepth, worldTransform, null, primitive, blendType)
        {}

        public CCPrimitiveCommand (float globalDepth, CCAffineTransform worldTransform, CCTexture2D texture, CCPrimitive<T,T2> primitive, 
            CCBlendFunc blendType)
            : base(globalDepth, worldTransform)
        {
            Primitive = primitive;
            BlendType = blendType;
            Texture = texture;
        }

        #endregion Constructors


        internal override void RequestRenderCommand(CCRenderer renderer)
        {
            renderer.ProcessPrimitiveRenderCommand (this);
        }

        public void RenderPrimitive(CCDrawManager drawManager)
        {
            drawManager.BlendFunc(BlendType);
            drawManager.BindTexture(Texture);
            Primitive.Draw(drawManager);
        }
    }
}

