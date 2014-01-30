using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public class CCGrid3DAction : CCGridAction
    {
        private CCGrid3D m_pGrid;

        public override CCGridBase Grid
        {
            get
            {
                if (m_pTarget != null && !m_pTarget.ContentSize.Equals(CCSize.Zero))
                {
                    m_pGrid = new CCGrid3D(m_sGridSize, m_pTarget.ContentSize.PointsToPixels());
                }
                else
                {
                    m_pGrid = new CCGrid3D(m_sGridSize);
                }

                return m_pGrid;
            }
            set
            {
                Debug.Assert(value is CCGrid3D);
                m_pGrid = (CCGrid3D) value;
            }
        }


        #region Constructors

        protected CCGrid3DAction()
        {
        }

        protected CCGrid3DAction(float duration) : base(duration)
        {
        }

        protected CCGrid3DAction(float duration, CCGridSize gridSize) : base(duration, gridSize)
        {
        }

        // Perform a deep copy of CCGrid3DAction
        protected CCGrid3DAction(CCGrid3DAction action) : base(action)
        {
        }

        #endregion Constructors

		/// <summary>
		/// returns the vertex at a given position
		/// </summary>
        public CCVertex3F Vertex(CCGridSize pos)
        {
            return m_pGrid.Vertex(pos);
        }

		/// <summary>
		/// returns the vertex at a given position
		/// </summary>
		public CCVertex3F Vertex(int x, int y)
		{
			return m_pGrid.Vertex(x,y);
		}

		/// <summary>
		/// returns the original (non-transformed) vertex at a given position
		/// </summary>
        public CCVertex3F OriginalVertex(CCGridSize pos)
        {
            return m_pGrid.OriginalVertex(pos);
        }

		/// <summary>
		/// returns the original (non-transformed) vertex at a given position
		/// </summary>
		public CCVertex3F OriginalVertex(int x, int y)
		{
			return m_pGrid.OriginalVertex(x,y);
		}

		/// <summary>
		/// sets a new vertex at a given position
		/// </summary>
        public void SetVertex(CCGridSize pos, ref CCVertex3F vertex)
        {
            m_pGrid.SetVertex(pos, ref vertex);
        }

		/// <summary>
		/// sets a new vertex at a given position
		/// </summary>
		public void SetVertex(int x, int y, ref CCVertex3F vertex)
		{
			m_pGrid.SetVertex(x,y, ref vertex);
		}

    }
}