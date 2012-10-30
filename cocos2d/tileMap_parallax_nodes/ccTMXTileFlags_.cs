
namespace cocos2d
{
    public class ccTMXTileFlags
    {
        public static uint kCCTMXTileHorizontalFlag = 0x80000000;
        public static uint kCCTMXTileVerticalFlag = 0x40000000;
        public static uint kCCTMXTileDiagonalFlag = 0x20000000;
        public static uint kCCFlipedAll = (kCCTMXTileHorizontalFlag | kCCTMXTileVerticalFlag | kCCTMXTileDiagonalFlag);
        public static uint kCCFlippedMask = ~(kCCFlipedAll);
    }
}