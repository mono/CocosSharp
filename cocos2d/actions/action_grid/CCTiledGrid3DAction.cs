using System.Diagnostics;

namespace CocosSharp
{
    public class CCTiledGrid3DAction : CCGridAction
    {
        private CCTiledGrid3D m_pGrid;

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


        #region Constructors

        public CCTiledGrid3DAction()
        {
        }

        public CCTiledGrid3DAction(float duration) : base(duration)
        {
        }

        public CCTiledGrid3DAction(float duration, CCGridSize gridSize) : base(duration, gridSize)
        {
        }

        public CCTiledGrid3DAction(CCTiledGrid3DAction gridAction) : base(gridAction)
        {
        }

        #endregion Constructors

		/// <summary>
		/// returns the tile quad at a given position
		/// </summary>
        public CCQuad3 Tile(CCGridSize pos)
        {
            return m_pGrid.Tile(pos);
        }

		/// <summary>
		/// returns the tile quad at a given position
		/// </summary>
		public CCQuad3 Tile(int x, int y)
		{
			return m_pGrid.Tile(x, y);
		}

		/// <summary>
		/// returns the original (non-transformed) tile quad at a given position
		/// </summary>
        public CCQuad3 OriginalTile(CCGridSize pos)
        {
            return m_pGrid.OriginalTile(pos);
        }

		/// <summary>
		/// returns the original (non-transformed) tile quad at a given position
		/// </summary>
		public CCQuad3 OriginalTile(int x, int y)
		{
			return m_pGrid.OriginalTile(x, y);
		}

		/// <summary>
		/// sets a new tile quad at a given position
		/// </summary>
        public void SetTile(CCGridSize pos, ref CCQuad3 coords)
        {
            m_pGrid.SetTile(pos, ref coords);
        }

		/// <summary>
		/// sets a new tile quad at a given position
		/// </summary>
		public void SetTile(int x, int y, ref CCQuad3 coords)
		{
			m_pGrid.SetTile(x, y, ref coords);
		}

    }
}