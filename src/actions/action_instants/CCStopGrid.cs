namespace CocosSharp
{
	public class CCStopGrid : CCActionInstant
	{
		public CCStopGrid ()
		{
		}

		/// <summary>
		/// Start the stop grid operation on the given target.
		/// </summary>
		/// <param name="target"></param>
		internal override CCActionState StartAction(CCNode target)
		{
			return new CCStopGridState (this, target);

		}
	}

	internal class CCStopGridState : CCActionInstantState
	{

		public CCStopGridState (CCStopGrid action, CCNode target)
			: base (action, target)
		{	
			CCGridBase pGrid = Target.Grid;
			if (pGrid != null && pGrid.Active)
			{
				pGrid.Active = false;
			}
		}

	}
}