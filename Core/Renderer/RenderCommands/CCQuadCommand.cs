using System;
using System.Diagnostics;

namespace CocosSharp
{
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    internal class CCQuadCommand : CCRenderCommand
    {
        bool disposed;
        bool materialIdDirty;
        uint materialId;

        CCBlendFunc blendType;
        CCTexture2D texture;
        CCV3F_C4B_T2F_Quad[] quads;

        public delegate void UpdateQuadCallback(ref CCV3F_C4B_T2F_Quad[] callback);


        #region Properties

        internal uint MaterialId 
        { 
            get 
            { 
                if(materialIdDirty)
                {
                    GenerateMaterialId();
                    materialIdDirty = false;
                }

                return materialId;
            }
        }

        internal CCTexture2D Texture
        { 
            get { return texture; }
            set { texture = value; materialIdDirty = true; RenderFlagsDirty = true; }
        }

        internal CCBlendFunc BlendType
        { 
            get { return blendType; }
            set { blendType = value; materialIdDirty = true; RenderFlagsDirty = true; }
        }

        internal int QuadCount { get; set; }

        internal CCV3F_C4B_T2F_Quad[] Quads 
        { 
            get { return quads; }
            set { quads = value; }
        }

        #endregion Properties


        #region Constructors

        public CCQuadCommand(int quadCount)
        {
            if(quadCount > 0) 
            {
                QuadCount = quadCount;
                quads = new CCV3F_C4B_T2F_Quad[quadCount];
                for(int i = 0; i < quadCount; ++i) 
                {
                    quads[i].BottomLeft.Colors = CCColor4B.White;
                    quads[i].BottomRight.Colors = CCColor4B.White;
                    quads[i].TopLeft.Colors = CCColor4B.White;
                    quads[i].TopRight.Colors = CCColor4B.White;
                }
            }
        }

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
        }

        protected CCQuadCommand(CCQuadCommand copy)
            : base(copy)
        {
            Quads = copy.quads;
            QuadCount = copy.QuadCount;
            Texture = copy.Texture;
            BlendType = copy.BlendType;

            materialId = copy.materialId;
            materialIdDirty = copy.materialIdDirty;
        }

        public override CCRenderCommand Copy()
        {
            return new CCQuadCommand(this);
        }

        #endregion Constructors


        #region Cleaning up

        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing) 
            {
                if (quads != null)
                {
                    quads = null;
                }
            }

            base.Dispose(disposing);

            disposed = true;
        }

        #endregion Cleaning up


        void GenerateMaterialId()
        {
            var textureId = texture == null ? 0 : texture.TextureId;
            materialId = (uint)textureId << 12 | (uint)BlendType.GetHashCode();
        }

        protected override void GenerateFlags(ref long renderFlag)
        {
            base.GenerateFlags(ref renderFlag);

            renderFlag = renderFlag
                | (long)MaterialId;
        }

        internal void RequestUpdateQuads(UpdateQuadCallback callback)
        {
            callback(ref quads);
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

        internal new string DebugDisplayString
        {
            get
            {
                return ToString();

            }
        }

        public override string ToString()
        {
            return string.Concat("[CCQuadCommand: Group ", Group.ToString(), " Depth ", GlobalDepth.ToString(),
                " QuadCount ", QuadCount,
                " MaterialId ", MaterialId.ToString(),"]");
        }
    }
}

