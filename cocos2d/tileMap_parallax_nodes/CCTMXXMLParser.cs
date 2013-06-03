
namespace Cocos2D
{
    public enum CCTMXOrientation
    {
        /// <summary>
        /// Orthogonal orientation
        /// </summary>
        CCTMXOrientationOrtho = 0,

        /// <summary>
        /// Hexagonal orientation
        /// </summary>
        CCTMXOrientationHex = 1,

        /// <summary>
        ///  Isometric orientation
        /// </summary>
        CCTMXOrientationIso = 2,
    };

    public enum CCTMXLayerAttrib
    {
        None = 1 << 0,
        Base64 = 1 << 1,
        Gzip = 1 << 2,
        Zlib = 1 << 3,
    };

    public enum CCTMXProperty
    {
        None,
        Map,
        Layer,
        ObjectGroup,
        Object,
        Tile
    };
}