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


        public CCVertex3F Vertex(CCGridSize pos)
        {
            return m_pGrid.Vertex(pos);
        }

        public CCVertex3F OriginalVertex(CCGridSize pos)
        {
            return m_pGrid.OriginalVertex(pos);
        }

        public void SetVertex(CCGridSize pos, ref CCVertex3F vertex)
        {
            m_pGrid.SetVertex(pos, ref vertex);
        }
    }
}