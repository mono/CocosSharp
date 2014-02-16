// GEN OK

using System;

namespace CocosSharp
{
    public partial class CCSpriteFrame : ICCCopyable<CCSpriteFrame>
    {
        #region properties

        protected bool m_bRotated;
        protected CCPoint m_obOffset;
        protected CCPoint m_obOffsetInPixels;
        protected CCSize m_obOriginalSize;
        protected CCSize m_obOriginalSizeInPixels;
        protected CCRect m_obRect;
        protected CCRect m_obRectInPixels;
        protected CCTexture2D m_pobTexture;
        protected string m_strTextureFilename;

        /// <summary>
        /// get or set rect of the frame
        /// </summary>
        public CCRect Rect
        {
            get { return m_obRect; }
            set
            {
                m_obRect = value;
                m_obRectInPixels = m_obRect.PointsToPixels();
            }
        }

        public CCRect RectInPixels
        {
            get { return m_obRectInPixels; }
            set
            {
                m_obRectInPixels = value;
                m_obRect = m_obRectInPixels.PixelsToPoints();
            }
        }

        public CCPoint Offset
        {
            get { return m_obOffset; }
            set
            {
                m_obOffset = value;
                m_obOffsetInPixels = m_obOffset.PointsToPixels();
            }
        }

        public CCPoint OffsetInPixels
        {
            get { return m_obOffsetInPixels; }
            set
            {
                m_obOffsetInPixels = value;
                m_obOffset = m_obOffsetInPixels.PixelsToPoints();
            }
        }

        public bool IsRotated
        {
            get { return m_bRotated; }
            set { m_bRotated = value; }
        }


        public CCSize OriginalSizeInPixels
        {
            get { return m_obOriginalSizeInPixels; }
            set { m_obOriginalSizeInPixels = value; }
        }

        public CCSize OriginalSize
        {
            get { return m_obOriginalSize; }
            set { m_obOriginalSize = value; }
        }

        /// <summary>
        /// get or set texture of the frame
        /// </summary>
        public CCTexture2D Texture
        {
            get { return m_pobTexture; }
            set { m_pobTexture = value; }
        }

        /// <summary>
        /// Gets or sets the texture filename.
        /// </summary>
        /// <value>The texture filename.</value>
        public string TextureFilename
        {
            get { return m_strTextureFilename; }
            set { m_strTextureFilename = value; }
        }

        #endregion


        #region Constructors

        public CCSpriteFrame() 
        { 
        }

        protected CCSpriteFrame(CCSpriteFrame spriteFrame) 
            : this(spriteFrame.m_pobTexture, spriteFrame.m_obRectInPixels, spriteFrame.m_bRotated, 
            spriteFrame.m_obOffsetInPixels, spriteFrame.m_obOriginalSizeInPixels)
		{
		}

        public CCSpriteFrame(CCTexture2D pobTexture, CCRect rect, CCSize originalSize)
        {
            InitCCSpriteFrame(pobTexture, rect, originalSize);
        }

        public CCSpriteFrame(CCTexture2D pobTexture, CCRect rect) : this(pobTexture, rect, rect.Size)
        {
        }

        public CCSpriteFrame(CCTexture2D pobTexture, CCRect rect, bool rotated, CCPoint offset, CCSize originalSize)
        {
            InitCCSpriteFrame(pobTexture, rect, originalSize, rotated, offset);
        }

        private void InitCCSpriteFrame(CCTexture2D pobTexture, CCRect rect, CCSize originalSize, bool rotated=false, CCPoint offset=default(CCPoint))
        {
            m_pobTexture = pobTexture;

            m_obRectInPixels = rect;
            m_obRect = rect.PixelsToPoints();
            m_obOffsetInPixels = offset;
            m_obOffset = m_obOffsetInPixels.PixelsToPoints();
            m_obOriginalSizeInPixels = originalSize;
            m_obOriginalSize = m_obOriginalSizeInPixels.PixelsToPoints();
            m_bRotated = rotated;
        }

        #endregion Constructors


        public CCSpriteFrame DeepCopy()
        {
            return new CCSpriteFrame(this);
        }
    }
}