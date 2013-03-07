// GEN OK

using System;

namespace cocos2d
{
    public class CCSpriteFrame : CCObject
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
                m_obRectInPixels = CCMacros.CCRectanglePointsToPixels(m_obRect);
            }
        }

        public CCRect RectInPixels
        {
            get { return m_obRectInPixels; }
            set
            {
                m_obRectInPixels = value;
                m_obRect = CCMacros.CCRectanglePixelsToPoints(m_obRectInPixels);
            }
        }

        public CCPoint Offset
        {
            get { return m_obOffset; }
            set
            {
                m_obOffset = value;
                m_obOffsetInPixels = CCMacros.CCPointPointsToPixels(m_obOffset);
            }
        }

        public CCPoint OffsetInPixels
        {
            get { return m_obOffsetInPixels; }
            set
            {
                m_obOffsetInPixels = value;
                m_obOffset = CCMacros.CCPointPixelsToPoints(m_obOffsetInPixels);
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

        public override CCObject CopyWithZone(CCZone pZone)
        {
            var pCopy = new CCSpriteFrame();
            pCopy.InitWithTexture(m_pobTexture, m_obRectInPixels, m_bRotated, m_obOffsetInPixels, m_obOriginalSizeInPixels);
            return pCopy;
        }

        public static CCSpriteFrame Create(CCTexture2D pobTexture, CCRect rect)
        {
            var pSpriteFrame = new CCSpriteFrame();
            pSpriteFrame.InitWithTexture(pobTexture, rect);
            return pSpriteFrame;
        }

        public static CCSpriteFrame Create(CCTexture2D pobTexture, CCRect rect, bool rotated, CCPoint offset,
                                           CCSize originalSize)
        {
            var pSpriteFrame = new CCSpriteFrame();
            pSpriteFrame.InitWithTexture(pobTexture, rect, rotated, offset, originalSize);

            return pSpriteFrame;
        }

        public bool InitWithTexture(CCTexture2D pobTexture, CCRect rect)
        {
            CCRect rectInPixels = CCMacros.CCRectanglePointsToPixels(rect);
            return InitWithTexture(pobTexture, rectInPixels, false, new CCPoint(0, 0), rectInPixels.size);
        }

        public bool InitWithTexture(CCTexture2D pobTexture, CCRect rect, bool rotated, CCPoint offset,
                                    CCSize originalSize)
        {
            m_pobTexture = pobTexture;

            m_obRectInPixels = rect;
            m_obRect = CCMacros.CCRectanglePixelsToPoints(rect);
            m_obOffsetInPixels = offset;
            m_obOffset = CCMacros.CCPointPixelsToPoints(m_obOffsetInPixels);
            m_obOriginalSizeInPixels = originalSize;
            m_obOriginalSize = CCMacros.CCSizePixelsToPoints(m_obOriginalSizeInPixels);
            m_bRotated = rotated;

            return true;
        }

        public bool InitWithTextureFilename(String filename, CCRect rect)
        {
            return InitWithTextureFilename(filename, rect, false, CCPoint.Zero, rect.size);
        }

        public bool InitWithTextureFilename(String filename, CCRect rect, bool rotated, CCPoint offset,
                                            CCSize originalSize)
        {
            m_pobTexture = null;
            m_strTextureFilename = filename;
            m_obRectInPixels = rect;
            m_obRect = CCMacros.CCRectanglePixelsToPoints(rect);
            m_obOffsetInPixels = offset;
            m_obOffset = CCMacros.CCPointPixelsToPoints(m_obOffsetInPixels);
            m_obOriginalSizeInPixels = originalSize;
            m_obOriginalSize = CCMacros.CCSizePixelsToPoints(m_obOriginalSizeInPixels);
            m_bRotated = rotated;

            return true;
        }
    }
}