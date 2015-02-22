using System;

namespace CocosSharp
{

    public class CCBatchCommand : CCRenderCommand
    {
        CCTextureAtlas Texture { get; set; }
        public CCBlendFunc BlendType { get; set; }

        public CCBatchCommand(float globalZOrder, CCBlendFunc blendType, CCTextureAtlas texture, CCAffineTransform modelViewTransform, int flags) 
            : base(globalZOrder, modelViewTransform, flags)
        {
            CommandType = CommandType.BATCH_COMMAND;
            Texture = texture;
            BlendType = blendType;
        }

        public CCBatchCommand(float globalZOrder, CCBlendFunc blendType, CCTextureAtlas texture) 
            : this(globalZOrder, blendType, texture, CCAffineTransform.Identity, 0)
        {


        }

        internal override void Execute(CCDrawManager drawManager)
        {
            drawManager.BindTexture(Texture.Texture);
            drawManager.BlendFunc(BlendType);
            Texture.DrawQuads();
        }
    }
}

