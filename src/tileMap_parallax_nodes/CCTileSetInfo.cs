using System;

namespace CocosSharp
{
    public class CCTileSetInfo 
    {
        #region Properties

        public int BorderWidth { get; set; }   
        public int TileSpacing { get; set; } 
        public short FirstGid { get; set; }

        public string Name { get; set; }
        public string TilesheetFilename { get; set; }

        public CCSize TilesheetSize { get; set; }
        public CCSize TileTexelSize { get; set; }

        #endregion Properties


        public CCRect TextureRectForGID(short gid)
        {
            CCRect rect = new CCRect();

            if (gid != 0)
            {
                // Rect offset relative to first gid
                gid -= FirstGid;
                rect.Size = TileTexelSize;
                var max_x = (int)((TilesheetSize.Width - BorderWidth * 2 + TileSpacing) / (TileTexelSize.Width + TileSpacing));
                rect.Origin.X = (gid % max_x) * (TileTexelSize.Width + TileSpacing) + BorderWidth;
                rect.Origin.Y = (gid / max_x) * (TileTexelSize.Height + TileSpacing) + BorderWidth;
            }
            return rect;
        }
    }
}

