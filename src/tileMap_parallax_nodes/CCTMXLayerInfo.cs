using System.Collections.Generic;

namespace CocosSharp
{
    public class CCTMXLayerInfo 
    {
        #region Properties

        public bool OwnTiles { get; set; }
        public bool Visible { get; set; }
        public byte Opacity { get; set; }
        public string Name { get; set; }

        public uint MaxGID { get; set; }
        public uint MinGID { get; set; }

        public CCSize LayerSize { get; set; }
        public CCPoint Offset { get; set; }

        public uint[] Tiles { get; set; }

        public Dictionary<string, string> Properties { get; set; }

        #endregion Properties


        #region Constructors

        public CCTMXLayerInfo()
        {
            Properties = new Dictionary<string, string>(); 
            Name = "";
            MinGID = 100000;
        }

        #endregion Constructors
    }
}