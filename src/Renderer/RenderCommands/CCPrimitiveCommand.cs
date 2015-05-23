using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    [DebuggerDisplay("{DebugDisplayString,nq}")]
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

        protected CCPrimitiveCommand(CCPrimitiveCommand<T,T2> copy)
        {
            Primitive = copy.Primitive;
            BlendType = copy.BlendType;
            Texture = copy.Texture;
        }

        public override CCRenderCommand Copy()
        {
            return new CCPrimitiveCommand<T, T2>(this);
        }
            
        #endregion Constructors


        internal override void RequestRenderCommand(CCRenderer renderer)
        {
            renderer.ProcessPrimitiveRenderCommand (this);
        }

        public void RenderPrimitive(CCDrawManager drawManager)
        {
            drawManager.PushMatrix();
            drawManager.SetIdentityMatrix();
            var worldTrans = WorldTransform.XnaMatrix;
            drawManager.MultMatrix(ref worldTrans);

            drawManager.BlendFunc(BlendType);
            drawManager.BindTexture(Texture);

            Primitive.Draw(drawManager);

            drawManager.PopMatrix();
        }

        internal new string DebugDisplayString
        {
            get
            {
                return ToString();
            }
        }

        public override string ToString()
        {
            return string.Concat("[CCPrimitiveCommand: Group ", Group.ToString(), " Depth ", GlobalDepth.ToString(),"]");
        }
    }
}

