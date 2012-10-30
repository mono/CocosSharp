
using System.Diagnostics;

namespace cocos2d
{
    public class CCGridAction : CCActionInterval
    {
        protected ccGridSize m_sGridSize;

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCGridAction pCopy = null;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCGridAction) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCGridAction();
                pZone = new CCZone(pCopy);
            }

            base.CopyWithZone(pZone);

            pCopy.InitWithSize(m_sGridSize, m_fDuration);

            return pCopy;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);

            CCNode t = m_pTarget;
            CCGridBase targetGrid = t.Grid;

            if (targetGrid != null && targetGrid.ReuseGrid > 0)
            {
                Grid = targetGrid;

                if (targetGrid.Active && targetGrid.GridSize.x == m_sGridSize.x
                    && targetGrid.GridSize.y == m_sGridSize.y /*&& dynamic_cast<CCGridBase*>(targetGrid) != NULL*/)
                {
                    targetGrid.Reuse();
                }
                else
                {
                    Debug.Assert(false);
                }
            }
            else
            {
                if (targetGrid != null && targetGrid.Active)
                {
                    targetGrid.Active = false;
                }

                CCGridBase newgrid = Grid;

                t.Grid = newgrid;
                t.Grid.Active = true;
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCReverseTime.Create(this);
        }

        public virtual CCGridBase Grid
        {
            set { }
            get { return null; }
        }

        public static CCGridAction Create(ccGridSize gridSize, float duration)
        {
            var pAction = new CCGridAction();
            pAction.InitWithSize(gridSize, duration);
            return pAction;
        }

        public virtual bool InitWithSize(ccGridSize gridSize, float duration)
        {
            if (base.InitWithDuration(duration))
            {
                m_sGridSize = gridSize;
                return true;
            }
            return false;
        }
    }
}