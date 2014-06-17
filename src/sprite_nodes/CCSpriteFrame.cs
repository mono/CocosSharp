using System;

namespace CocosSharp
{
    public class CCSpriteFrame
    {

        #region Properties

        public bool IsRotated { get; set; }
        public string TextureFilename { get; set; }

        public CCSize OriginalSizeInPixels { get; private set; }

        public CCRect RectInPixels { get; set; }
        public CCPoint OffsetInPixels { get; set; }

        public CCTexture2D Texture { get; set; }


        #endregion Properties


        #region Constructors

        public CCSpriteFrame() 
        { 
        }

        protected CCSpriteFrame(CCSpriteFrame spriteFrame) 
            : this(spriteFrame.Texture, spriteFrame.RectInPixels, spriteFrame.OriginalSizeInPixels, 
                spriteFrame.IsRotated, spriteFrame.OffsetInPixels)
        {
        }

        public CCSpriteFrame(CCTexture2D texture, CCRect rectInPxls) : this(texture, rectInPxls, rectInPxls.Size)
        {
        }

        public CCSpriteFrame(CCTexture2D texture, CCRect rectInPxls, CCSize originalSizeInPxls, bool rotated=false, CCPoint? offsetInPxls=null)
        {
            RectInPixels = rectInPxls;
            OffsetInPixels = offsetInPxls ?? CCPoint.Zero;
            OriginalSizeInPixels = originalSizeInPxls;
            IsRotated = rotated;
            Texture = texture;
        }

        #endregion Constructors

    }
}