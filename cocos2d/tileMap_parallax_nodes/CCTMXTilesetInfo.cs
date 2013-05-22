
namespace Cocos2D
{
    public class CCTMXTilesetInfo 
    {
        /// <summary>
        /// The name of this tileset.
        /// </summary>
        public string m_sName;
        /// <summary>
        ///filename containing the tiles (should be spritesheet / texture atlas) 
        /// </summary>
        public string m_sSourceImage;
        /// <summary>
        /// size in pixels of the image
        /// </summary>
        public CCSize m_tImageSize;
        /// <summary>
        /// The size of this tile in unscaled pixels
        /// </summary>
        public CCSize m_tTileSize;
        public uint m_uFirstGid;
        /// <summary>
        /// the margin/border around the tilesheer
        /// </summary>
        public int m_uMargin;
        /// <summary>
        /// Spacing between the tiles in the tilesheet
        /// </summary>
        public int m_uSpacing;

        public CCRect RectForGID(uint gid)
        {
            CCRect rect = new CCRect();
            rect.Size = m_tTileSize;
            gid &= CCTMXTileFlags.FlippedMask;
            gid = gid - m_uFirstGid;
            var max_x = (int) ((m_tImageSize.Width - m_uMargin * 2 + m_uSpacing) / (m_tTileSize.Width + m_uSpacing));
            //	int max_y = (imageSize.height - margin*2 + spacing) / (tileSize.height + spacing);
            rect.Origin.X = (gid % max_x) * (m_tTileSize.Width + m_uSpacing) + m_uMargin;
            rect.Origin.Y = (gid / max_x) * (m_tTileSize.Height + m_uSpacing) + m_uMargin;
            return rect;
        }
    }
}