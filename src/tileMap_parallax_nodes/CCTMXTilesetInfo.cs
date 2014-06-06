
namespace CocosSharp
{
    public class CCTMXTilesetInfo 
    {
        #region Properties

        public int Margin { get; set; }                 // The margin/border around the tilesheer
        public int Spacing { get; set; }                // Spacing between the tiles in the tilesheet
        public uint FirstGid { get; set; }

        public string Name { get; set; }
        public string SourceImage { get; set; }         // Filename containing the tiles (should be spritesheet / texture atlas)

        public CCSize ImageSize { get; set; }           // Size in pixels of the image
        public CCSize TileSize { get; set; }            // The size of this tile in unscaled pixels

        #endregion Properties

        public CCRect RectForGID(uint gid)
        {
            CCRect rect = new CCRect();
            rect.Size = TileSize;
            gid &= CCTMXTileFlags.FlippedMask;
            gid = gid - FirstGid;
            var max_x = (int) ((ImageSize.Width - Margin * 2 + Spacing) / (TileSize.Width + Spacing));
            rect.Origin.X = (gid % max_x) * (TileSize.Width + Spacing) + Margin;
            rect.Origin.Y = (gid / max_x) * (TileSize.Height + Spacing) + Margin;
            return rect;
        }
    }
}