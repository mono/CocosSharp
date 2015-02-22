using System;

namespace CocosSharp
{
    public class CCQuadCommand : CCRenderCommand
    {

        //internal int MaterialId { get; set; }
        internal CCTexture2D Texture { get; set; }
        internal CCBlendFunc BlendType { get; set; }
        internal CCV3F_C4B_T2F_Quad[] Quads { get; set; }
        internal int QuadCount { get; set; }

        public CCQuadCommand(float globalZOrder, CCTexture2D texture, CCBlendFunc blendType, 
            CCV3F_C4B_T2F_Quad[] quads, int quadCount, CCAffineTransform modelViewTransform, int flags = 0) 
            : base(globalZOrder, modelViewTransform, flags)
        {
            CommandType = CommandType.QUAD_COMMAND;
            Quads = quads;
            QuadCount = quadCount;
            Texture = texture;
            BlendType = blendType;

            var textureId = texture == null ? 0 : texture.TextureId;

            // +=========================================================+
            // | Material Id 24 bits                                     |
            // +============================+============================+
            // | Texture ID                 | BlendFunc (Src ^ Dest)     |
            // | 12 bits                    | 12 bits                    |
            // +============================+============================+
            MaterialId = textureId << 12 | BlendType.GetHashCode();

            System.Diagnostics.Debug.Assert(MaterialId != 0, "Material Id not set");
                
        }

        public CCQuadCommand(float globalZOrder, CCTexture2D texture, CCBlendFunc blendType, 
            CCV3F_C4B_T2F_Quad quad, CCAffineTransform modelViewTransform, int flags = 0) 
            : this(globalZOrder, texture, blendType, new CCV3F_C4B_T2F_Quad[] { quad }, 1,
                modelViewTransform, flags)
        { }


        internal void UseMaterial (CCDrawManager drawManager)
        {
            drawManager.BlendFunc(BlendType);
            drawManager.BindTexture(Texture);
        }
        internal override void Execute(CCDrawManager drawManager)
        {
//            drawManager.PushMatrix();
//            //var xnaMatrix = ModelViewTransform.XnaMatrix;
//            //drawManager.MultMatrix(ref xnaMatrix);
//            drawManager.BlendFunc(BlendType);
//            drawManager.BindTexture(Texture);
//
//            drawManager.DrawQuad(ref Quads[0]);
//
//            drawManager.PopMatrix();

        }
    }
}

