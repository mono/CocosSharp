using System;
using System.Diagnostics;

namespace CocosSharp
{
	public class CCGridAction : CCAmplitudeAction
	{
		protected internal CCGridSize GridSize { get; private set; }


		#region Constructors

		public CCGridAction (float duration) : base (duration)
		{
		}

		public CCGridAction (float duration, CCGridSize gridSize) : this (duration, gridSize, 0)
		{
		}

		protected CCGridAction (float duration, CCGridSize gridSize, float amplitude) : base (duration, amplitude)
		{
			GridSize = gridSize;
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
            return new CCGridActionState (this, GridNode(target));
		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCReverseTime (this);
		}
	}


	#region Action state

    public class CCGridActionState : CCAmplitudeActionState
	{
		protected CCGridSize GridSize { get; private set; }

		public virtual CCGridBase Grid { 
			get { return null; } 
			protected set { } 
		}

        public CCGridActionState (CCGridAction action, CCNodeGrid target) : base (action, target)
		{
			GridSize = action.GridSize;


            CCGridBase targetGrid = target.Grid;

			if (targetGrid != null && targetGrid.ReuseGrid > 0)
			{
				Grid = targetGrid;

				if (targetGrid.Active && targetGrid.GridSize.X == GridSize.X && targetGrid.GridSize.Y == GridSize.Y)
				{
					targetGrid.Reuse ();
				}
				else
				{
					Debug.Assert (false);
				}
			}
			else
			{
				if (targetGrid != null && targetGrid.Active)
				{
					targetGrid.Active = false;
				}

				CCGridBase newgrid = Grid;
                if (target != null)
                {
                    target.Grid = newgrid;
                    target.Grid.Active = true;
                }
			}
		}
	}

	#endregion Action state
}