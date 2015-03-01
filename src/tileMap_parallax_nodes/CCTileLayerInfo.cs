using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public class CCTileLayerInfo 
    {
        const uint DefaultMinGID = 100000;

        #region Properties

        public bool Visible { get; set; }
        public byte Opacity { get; set; }
        public string Name { get; set; }

        public uint MaxGID { get; set; }
        public uint MinGID { get; set; }

        public CCTileMapCoordinates LayerDimensions { get; set; }
        public CCPoint TileCoordOffset { get; set; }

        public CCTileGidAndFlags[] TileGIDAndFlags { get; set; }

        public Dictionary<string, string> Properties { get; set; }

        public uint NumberOfTiles
        {
            get { return (uint)(LayerDimensions.Column * LayerDimensions.Row); }
        }

        #endregion Properties


        #region Constructors

        public CCTileLayerInfo(uint minGid = DefaultMinGID)
        {
            Properties = new Dictionary<string, string>(); 
            Name = String.Empty;
            MinGID = minGid;
        }

        #endregion Constructors
    }
}

