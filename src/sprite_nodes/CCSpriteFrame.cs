using System;

namespace CocosSharp
{
    public class CCSpriteFrame
    {
        // ivars
        CCPoint offset;
        CCPoint offsetInPixels;
        CCRect rect;
        CCRect rectInPixels;


        #region Properties

        public bool IsRotated { get; set; }

        public CCSize OriginalSizeInPixels { get; private set; }
        public CCSize OriginalSize { get; private set; }

        public string TextureFilename { get; set; }
        public CCTexture2D Texture { get; set; }

        public CCRect Rect
        {
            get { return rect; }
            set
            {
                rect = value;
                rectInPixels = rect.PointsToPixels();
            }
        }

        public CCRect RectInPixels
        {
            get { return rectInPixels; }
            set
            {
                rectInPixels = value;
				rect = rectInPixels.PixelsToPoints();
            }
        }

        public CCPoint Offset
        {
            get { return offset; }
            set
            {
                offset = value;
				offsetInPixels = offset.PointsToPixels();
            }
        }

        public CCPoint OffsetInPixels
        {
            get { return offsetInPixels; }
            set
            {
                offsetInPixels = value;
				offset = offsetInPixels.PixelsToPoints();
            }
        }

        #endregion Properties


        #region Constructors

        public CCSpriteFrame() 
        { 
        }

        protected CCSpriteFrame(CCSpriteFrame spriteFrame) 
            : this(spriteFrame.Texture, spriteFrame.rectInPixels, spriteFrame.OriginalSizeInPixels, 
                spriteFrame.IsRotated, spriteFrame.offsetInPixels)
        {
        }

        public CCSpriteFrame(CCTexture2D pobTexture, CCRect rect) : this(pobTexture, rect.PointsToPixels(), rect.PointsToPixels().Size)
        {
        }

        public CCSpriteFrame(CCTexture2D pobTexture, CCRect rectIn, CCSize originalSize, bool rotated=false, CCPoint? offsetIn=null)
        {
            Texture = pobTexture;

            rectInPixels = rectIn;
            rect = rectIn.PixelsToPoints();

            offsetInPixels = offsetIn ?? CCPoint.Zero;

			offset = offsetInPixels.PixelsToPoints();
            OriginalSizeInPixels = originalSize;
            OriginalSize = OriginalSizeInPixels.PixelsToPoints();
            IsRotated = rotated;
        }

        #endregion Constructors

    }
}