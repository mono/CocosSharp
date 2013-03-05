
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace cocos2d
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
                    m_pGrid = CCGrid3D.Create(m_sGridSize, m_pTarget.ContentSize);
                }
                else
                {
                    m_pGrid = CCGrid3D.Create(m_sGridSize);
                }

                return m_pGrid;
            }
            set
            {
                Debug.Assert(value is CCGrid3D);
                m_pGrid = (CCGrid3D) value;
            }
        }

        public ccVertex3F Vertex(ccGridSize pos)
        {
            return m_pGrid.Vertex(pos);
        }

        public ccVertex3F OriginalVertex(ccGridSize pos)
        {
            return m_pGrid.OriginalVertex(pos);
        }

        public void SetVertex(ccGridSize pos, ref ccVertex3F vertex)
        {
            m_pGrid.SetVertex(pos, ref vertex);
        }
    }
}