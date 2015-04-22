using System;

namespace CocosSharp
{

    internal class CCBatchCommand : CCRenderCommand
    {
        CCTextureAtlas Texture { get; set; }
        public CCBlendFunc BlendType { get; set; }

        public CCBatchCommand(float globalZOrder, CCBlendFunc blendType, CCTextureAtlas texture, CCAffineTransform modelViewTransform) 
            : base(globalZOrder, modelViewTransform)
        {
            CommandType = CCRenderer.CCCommandType.Batch;
            Texture = texture;
            BlendType = blendType;
        }

        public CCBatchCommand(float globalZOrder, CCBlendFunc blendType, CCTextureAtlas texture) 
            : this(globalZOrder, blendType, texture, CCAffineTransform.Identity)
        {  }

        internal override void RequestRenderCommand(CCRenderer renderer)
        {
            var drawManager = renderer.DrawManager;
            drawManager.BindTexture(Texture.Texture);
            drawManager.BlendFunc(BlendType);
            Texture.DrawQuads();
        }
    }
}

