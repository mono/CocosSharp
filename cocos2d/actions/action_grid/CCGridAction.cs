using System.Diagnostics;

namespace CocosSharp
{
    public class CCGridAction : CCActionInterval
    {
        protected CCGridSize m_sGridSize;

        public virtual CCGridBase Grid
        {
            set { }
            get { return null; }
        }


        #region Constructors

        public CCGridAction()
        {
        }

        public CCGridAction(float duration) : base(duration)
        {
        }

        public CCGridAction(float duration, CCGridSize gridSize) : this(duration)
        {
            InitCCGridAction(gridSize);
        }

        // Perform deep copy of CCGridAction
        public CCGridAction(CCGridAction gridAction) : base(gridAction)
        {
            InitCCGridAction(gridAction.m_sGridSize);
        }

        private void InitCCGridAction(CCGridSize gridSize)
        {
            m_sGridSize = gridSize; 
        }

        #endregion Constructors


        public override object Copy(ICCCopyable pZone)
        {
            return new CCGridAction(this);
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
    }
}