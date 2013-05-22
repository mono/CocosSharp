using System.Collections.Generic;

namespace Cocos2D
{
    public class CCTMXLayerInfo 
    {
        public Dictionary<string, string> Properties = new Dictionary<string, string>();
        private bool m_bOwnTiles = true;
        private bool m_bVisible;
        private byte m_cOpacity;
        private uint[] m_pTiles;
        private string m_sName = "";
        private CCSize m_tLayerSize;
        private CCPoint m_tOffset;
        private uint m_uMaxGID;
        private uint m_uMinGID = 100000;

        public bool OwnTiles 
        {
            get { 
                return m_bOwnTiles; 
            }
            set { 
                m_bOwnTiles = value; 
            }
        }

        public bool Visible
        {
            get { 
                return m_bVisible; 
            }
            set { 
                m_bVisible = value; 
            }
        }

        public byte Opacity
        {
            get { 
                return m_cOpacity; 
            }
            set { 
                m_cOpacity = value; 
            }
        }

        public uint[] Tiles {
            get {
                return m_pTiles;
            }
            set {
                m_pTiles = value;
            }
        }

        public string Name {
            get {
                return m_sName;
            }
            set {
                m_sName = value;
            }
        }

        public CCSize LayerSize {
            get {
                return m_tLayerSize;
            }
            set {
                m_tLayerSize = value;
            }
        }

        public CCPoint Offset {
            get {
                return m_tOffset;
            }
            set {
                m_tOffset = value;
            }
        }

        public uint MaxGID {
            get {
                return m_uMaxGID;
            }
            set {
                m_uMaxGID = value;
            }
        }

        public uint MinGID {
            get {
                return m_uMinGID;
            }
            set {
                m_uMinGID = value;
            }
        }
    }
}