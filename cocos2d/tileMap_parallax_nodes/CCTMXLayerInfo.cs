using System.Collections.Generic;

namespace cocos2d
{
    public class CCTMXLayerInfo : CCObject
    {
        public Dictionary<string, string> Properties = new Dictionary<string, string>();
        public bool m_bOwnTiles = true;
        public bool m_bVisible;
        public byte m_cOpacity;
        public uint[] m_pTiles;
        public string m_sName = "";
        public CCSize m_tLayerSize;
        public CCPoint m_tOffset;
        public uint m_uMaxGID;
        public uint m_uMinGID = 100000;
    }
}