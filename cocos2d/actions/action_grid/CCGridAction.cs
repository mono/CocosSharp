using System.Diagnostics;

namespace Cocos2D
{
    public class CCGridAction : CCActionInterval
    {
        protected CCGridSize m_sGridSize;

        public CCGridAction()
        {
        }

        public CCGridAction(float duration)
            : base(duration)
        {
        }

        public CCGridAction(float duration, CCGridSize gridSize) : base(duration)
        {
            InitWithDuration(duration, gridSize);
        }

        protected virtual bool InitWithDuration(float duration, CCGridSize gridSize)
        {
            if (base.InitWithDuration(duration))
            {
                m_sGridSize = gridSize;
                return true;
            }
            return false;
        }

        public CCGridAction(CCGridAction gridAction) : this(gridAction.m_fDuration, gridAction.m_sGridSize)
        {
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = (CCGridAction) (pZone);
                base.Copy(pZone);

                pCopy.InitWithDuration(m_fDuration, m_sGridSize);

                return pCopy;
            }
            else
            {
                return new CCGridAction(this);
            }
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);

            CCNode t = m_pTarget;
            CCGridBase targetGrid = t.Grid;

            if (targetGrid != null && targetGrid.ReuseGrid > 0)
            {
                Grid = targetGrid;

                if (targetGrid.Active && targetGrid.GridSize.X == m_sGridSize.X
                    && targetGrid.GridSize.Y == m_sGridSize.Y /*&& dynamic_cast<CCGridBase*>(targetGrid) != NULL*/)
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
            return new CCReverseTime(this);
        }

        public virtual CCGridBase Grid
        {
            set { }
            get { return null; }
        }
    }
}