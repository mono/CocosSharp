// GEN OK

using System;

namespace Cocos2D
{
    public partial class CCSpriteFrame : ICCCopyable
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

        #endregion

        public CCSpriteFrame() { }

		public CCSpriteFrame Copy()
		{
			return (CCSpriteFrame)Copy(null);
		}

        public object Copy(ICCCopyable pZone)
        {
            var pCopy = new CCSpriteFrame();
            pCopy.InitWithTexture(m_pobTexture, m_obRectInPixels, m_bRotated, m_obOffsetInPixels, m_obOriginalSizeInPixels);
            return pCopy;
        }

        public CCSpriteFrame(CCTexture2D pobTexture, CCRect rect)
        {
            InitWithTexture(pobTexture, rect);
        }

        public CCSpriteFrame(CCTexture2D pobTexture, CCRect rect, bool rotated, CCPoint offset,
                                           CCSize originalSize)
        {
            InitWithTexture(pobTexture, rect, rotated, offset, originalSize);
        }

        protected virtual bool InitWithTexture(CCTexture2D pobTexture, CCRect rect)
        {
            CCRect rectInPixels = rect.PointsToPixels();
            return InitWithTexture(pobTexture, rectInPixels, false, new CCPoint(0, 0), rectInPixels.Size);
        }

        protected virtual bool InitWithTexture(CCTexture2D pobTexture, CCRect rect, bool rotated, CCPoint offset,
                                    CCSize originalSize)
        {
            m_pobTexture = pobTexture;

            m_obRectInPixels = rect;
            m_obRect = rect.PixelsToPoints();
            m_obOffsetInPixels = offset;
            m_obOffset = m_obOffsetInPixels.PixelsToPoints();
            m_obOriginalSizeInPixels = originalSize;
            m_obOriginalSize = m_obOriginalSizeInPixels.PixelsToPoints();
            m_bRotated = rotated;

            return true;
        }

        protected virtual bool InitWithTextureFilename(String filename, CCRect rect)
        {
            return InitWithTextureFilename(filename, rect, false, CCPoint.Zero, rect.Size);
        }

        protected virtual bool InitWithTextureFilename(String filename, CCRect rect, bool rotated, CCPoint offset,
                                            CCSize originalSize)
        {
            m_pobTexture = null;
            m_strTextureFilename = filename;
            m_obRectInPixels = rect;
            m_obRect = rect.PixelsToPoints();
            m_obOffsetInPixels = offset;
            m_obOffset = m_obOffsetInPixels.PixelsToPoints();
            m_obOriginalSizeInPixels = originalSize;
            m_obOriginalSize = m_obOriginalSizeInPixels.PixelsToPoints();
            m_bRotated = rotated;

            return true;
        }
    }
}