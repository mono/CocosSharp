namespace CocosSharp
{
    public class CCReuseGrid : CCActionInstant
    {
        public int Times { get; private set; }

        #region Constructors

        public CCReuseGrid ()
        {
        }

        public CCReuseGrid (int times)
        {
            Times = times;
        }

        #endregion Constructors

        protected CCNodeGrid GridNode(CCNode target)
        {
            var gridNodeTarget = target as CCNodeGrid;
            if (gridNodeTarget == null)
            {
                CCLog.Log("Grid Actions can only target CCNodeGrids.");
                return null;
            }

            return gridNodeTarget;
        }

        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCReuseGridState (this, GridNode(target));
        }

    }

    public class CCReuseGridState : CCActionInstantState
    {
        public CCReuseGridState (CCReuseGrid action, CCNodeGrid target)
            : base (action, target)
        {   
            CCGridBase grid = target.Grid;
            if (grid != null && grid.Active)
            {
                grid.ReuseGrid += action.Times;
            }
        }

    }
}