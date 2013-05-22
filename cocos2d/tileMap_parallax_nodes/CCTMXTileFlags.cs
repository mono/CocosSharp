
namespace Cocos2D
{
    public class CCTMXTileFlags
    {
        public static uint Horizontal = 0x80000000;
        public static uint Vertical = 0x40000000;
        public static uint TileDiagonal = 0x20000000;
        public static uint FlippedAll = (Horizontal | Vertical | TileDiagonal);
        public static uint FlippedMask = ~(FlippedAll);
    }
}