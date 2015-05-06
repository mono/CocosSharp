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

        public CCSize TilesheetSize { get { return Texture.ContentSizeInPixels; } }
        public CCSize TileTexelSize { get; set; }

        public CCTexture2D Texture
        {
            get
            {
                if (texture == null)
                {
                    texture = string.IsNullOrEmpty(this.TilesheetFilename) ? null : CCTextureCache.SharedTextureCache.AddImage(this.TilesheetFilename);

                    if (texture.ContentSizeInPixels == CCSize.Zero)
                        CCLog.Log("Tilemap Layer Texture {0} not loaded", this.TilesheetFilename);
                }
                return texture;
            }
        }

        public int LastGid
        {
            get
            {
                if (lastGid == -1)
                {
                    var x = (int)((TilesheetSize.Width - BorderWidth * 2 + TileSpacing) / (TileTexelSize.Width + TileSpacing));
                    var y = (int)((TilesheetSize.Height - BorderWidth * 2 + TileSpacing) / (TileTexelSize.Height + TileSpacing));
                    lastGid = x * y + FirstGid - 1;
                }
                return lastGid;
            }
        }

        private CCTexture2D texture;
        private int lastGid = -1;
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

