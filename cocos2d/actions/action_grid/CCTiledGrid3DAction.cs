
using System.Diagnostics;

namespace cocos2d
{
    public class CCTiledGrid3DAction : CCGridAction
    {
        private CCTiledGrid3D m_pGrid;

        public CCQuad3 Tile(ccGridSize pos)
        {
            return m_pGrid.Tile(pos);
        }

        public CCQuad3 OriginalTile(ccGridSize pos)
        {
            return m_pGrid.OriginalTile(pos);
        }

        public void SetTile(ccGridSize pos, ref CCQuad3 coords)
        {
            m_pGrid.SetTile(pos, ref coords);
        }

        public override CCGridBase Grid
        {
            get
            {
                m_pGrid = CCTiledGrid3D.Create(m_sGridSize);
                return m_pGrid;
            }
            set
            {
                Debug.Assert(value is CCTiledGrid3D);
                m_pGrid = (CCTiledGrid3D) value;
            }
        }
    }
}