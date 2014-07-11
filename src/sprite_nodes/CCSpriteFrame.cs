using System;

namespace CocosSharp
{
    public class CCSpriteFrame
    {

        #region Properties

        public bool IsRotated { get; set; }
        public string TextureFilename { get; set; }

        public CCPoint OffsetInPixels { get; private set; }

        public CCSize ContentSize { get; private set; }
        public CCSize OriginalSizeInPixels { get; private set; }    // The dimensions of the sprite before trimming

        public CCRect TextureRectInPixels { get; set; }

        public CCTexture2D Texture { get; set; }

        #endregion Properties


        #region Constructors

        public CCSpriteFrame() 
        { 
        }

        protected CCSpriteFrame(CCSpriteFrame spriteFrame) 
            : this(spriteFrame.ContentSize, spriteFrame.Texture, spriteFrame.TextureRectInPixels, spriteFrame.OriginalSizeInPixels,
                spriteFrame.IsRotated, spriteFrame.OffsetInPixels)
        {
        }

        public CCSpriteFrame(CCTexture2D texture, CCRect rectInPxls) 
            : this(rectInPxls.Size, texture, rectInPxls, rectInPxls.Size)
        {
        }

        public CCSpriteFrame(CCSize contentSize, CCTexture2D texture, CCRect textureRectInPxls, bool rotated=false, CCPoint? offsetInPxls=null)
            : this(contentSize, texture, textureRectInPxls, textureRectInPxls.Size, rotated, offsetInPxls)
        {
        }

        public CCSpriteFrame(CCSize contentSize, CCTexture2D texture, CCRect textureRectInPxls, 
            CCSize originalSizeInPxls, bool rotated=false, CCPoint? offsetInPxls=null)
        {
            TextureRectInPixels = textureRectInPxls;
            ContentSize = contentSize;
            OffsetInPixels = offsetInPxls ?? CCPoint.Zero;
            OriginalSizeInPixels = originalSizeInPxls;
            IsRotated = rotated;
            Texture = texture;
        }

        #endregion Constructors

    }
}