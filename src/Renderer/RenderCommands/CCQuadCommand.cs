using System;

namespace CocosSharp
{
    internal class CCQuadCommand : CCRenderCommand
    {
        #region Properties

        internal CCTexture2D Texture { get; private set; }
        internal CCBlendFunc BlendType { get; private set; }
        internal CCV3F_C4B_T2F_Quad[] Quads { get; private set; }
        internal int QuadCount { get; private set; }
        internal uint MaterialId { get; private set; }

        #endregion Properties


        #region Constructors

        public CCQuadCommand(float globalDepth, CCAffineTransform worldTransform, 
            CCTexture2D texture, CCBlendFunc blendType, 
            params CCV3F_C4B_T2F_Quad[] quads) 
            : this(globalDepth, worldTransform, texture, blendType, quads.Length, quads)
        {  }

        public CCQuadCommand(float globalDepth, CCAffineTransform worldTransform, 
            CCTexture2D texture, CCBlendFunc blendType, int quadCount,
            params CCV3F_C4B_T2F_Quad[] quads) 
            : base(globalDepth, worldTransform)
        {
            Quads = quads;
            QuadCount = quadCount;
            Texture = texture;
            BlendType = blendType;

            var textureId = texture == null ? 0 : texture.TextureId;

            // Material id should be 24 bits
            // First 12 bits blend func. hash code (Src ^ Dest)
            // Last 12 bits texture id
            MaterialId = (uint)textureId << 12 | (uint)BlendType.GetHashCode();

            System.Diagnostics.Debug.Assert(MaterialId != 0, "Material Id not set");
        }

        #endregion Constructors

        protected override void GenerateId(ref long renderId)
        {
            // 64 - 57 : Group id (byte)
            // 56 - 25 : Global depth (float)
            // 24 - 1 : Material id (24 bit)
            base.GenerateId(ref renderId);

            renderId = renderId
                | (long)MaterialId;
        }

        internal override void RequestRenderCommand(CCRenderer renderer)
        {
            renderer.ProcessQuadRenderCommand(this);
        }

        internal void UseMaterial(CCDrawManager drawManager)
        {
            drawManager.BlendFunc(BlendType);
            drawManager.BindTexture(Texture);
        }
    }
}

