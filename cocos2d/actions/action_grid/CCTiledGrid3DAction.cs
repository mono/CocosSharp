using System.Diagnostics;

namespace Cocos2D
{
    public class CCTiledGrid3DAction : CCGridAction
    {
        private CCTiledGrid3D m_pGrid;

        public CCTiledGrid3DAction()
        {
        }

        public CCTiledGrid3DAction(float duration)
            : base(duration)
        {
        }

        public CCTiledGrid3DAction(float duration, CCGridSize gridSize)
            : base(duration, gridSize)
        {
        }

        public CCQuad3 Tile(CCGridSize pos)
        {
            return m_pGrid.Tile(pos);
        }

        public CCQuad3 OriginalTile(CCGridSize pos)
        {
            return m_pGrid.OriginalTile(pos);
        }

        public void SetTile(CCGridSize pos, ref CCQuad3 coords)
        {
            m_pGrid.SetTile(pos, ref coords);
        }

        public override CCGridBase Grid
        {
            get
            {
                m_pGrid = new CCTiledGrid3D(m_sGridSize);
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