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

        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCReuseGridState (this, target);

        }

    }

    internal class CCReuseGridState : CCActionInstantState
    {
        public CCReuseGridState (CCReuseGrid action, CCNode target)
            : base (action, target)
        {   
            CCGridBase grid = Target.Grid;
            if (grid != null && grid.Active)
            {
                grid.ReuseGrid += action.Times;
            }
        }

    }
}