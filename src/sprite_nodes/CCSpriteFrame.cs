using System;

namespace CocosSharp
{
    public class CCSpriteFrame
    {

        #region Properties

        public bool IsRotated { get; set; }
        public string TextureFilename { get; set; }

        public CCSize ContentSize { get; private set; }

        public CCRect TextureRectInPixels { get; set; }

        public CCTexture2D Texture { get; set; }


        #endregion Properties


        #region Constructors

        public CCSpriteFrame() 
        { 
        }

        protected CCSpriteFrame(CCSpriteFrame spriteFrame) 
            : this(spriteFrame.ContentSize, spriteFrame.Texture, spriteFrame.TextureRectInPixels, 
                spriteFrame.IsRotated)
        {
        }

        public CCSpriteFrame(CCTexture2D texture, CCRect rectInPxls) : this(rectInPxls.Size, texture, rectInPxls)
        {
        }

        public CCSpriteFrame(CCSize contentSize, CCTexture2D texture, CCRect textureRectInPxls, bool rotated=false)
        {
            TextureRectInPixels = textureRectInPxls;
            ContentSize = contentSize;
            IsRotated = rotated;
            Texture = texture;
        }

        #endregion Constructors

    }
}