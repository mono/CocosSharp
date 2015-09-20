namespace CocosSharp
{
    public class CCStopGrid : CCActionInstant
    {
        public CCStopGrid ()
        {
        }

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
            return new CCStopGridState (this, GridNode(target));
        }
    }

    public class CCStopGridState : CCActionInstantState
    {

        public CCStopGridState (CCStopGrid action, CCNodeGrid target)
            : base (action, target)
        {   
            CCGridBase pGrid = target.Grid;
            if (pGrid != null && pGrid.Active)
            {
                pGrid.Active = false;
            }
        }

    }
}