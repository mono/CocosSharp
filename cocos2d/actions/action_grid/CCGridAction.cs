using System.Diagnostics;

namespace CocosSharp
{
    public class CCGridAction : CCActionInterval
    {
        protected internal CCGridSize GridSize { get; private set; }


        // Take me out later - See comments in CCAction
        public override bool HasState 
        { 
            get { return true; }
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
            GridSize = gridSize; 
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCGridActionState(this, target);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCReverseTime(this);
        }
    }


    #region Action state

    public class CCGridActionState : CCActionIntervalState
    {
        protected CCGridSize CachedGridSize { get; private set; }

        public virtual CCGridBase Grid 
        { 
            get { return null; } 
            protected set { } 
        }

        public CCGridActionState(CCGridAction action, CCNode target) : base(action, target)
        {
            CachedGridSize = action.GridSize;
            CCGridBase targetGrid = Target.Grid;

            if (targetGrid != null && targetGrid.ReuseGrid > 0)
            {
                Grid = targetGrid;

                if (targetGrid.Active && targetGrid.GridSize.X == CachedGridSize.X && targetGrid.GridSize.Y == CachedGridSize.Y)
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

                Target.Grid = newgrid;
                Target.Grid.Active = true;
            }
        }
    }

    #endregion Action state
}