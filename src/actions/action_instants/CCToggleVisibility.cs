namespace CocosSharp
{
	public class CCToggleVisibility : CCActionInstant
	{
		#region Constructors

		public CCToggleVisibility ()
		{
		}

		#endregion Constructors

		/// <summary>
		/// Start the hide operation on the given target.
		/// </summary>
		/// <param name="target"></param>
		protected internal override CCActionState StartAction(CCNode target)
		{
			return new CCToggleVisibilityState (this, target);

		}

	}

	internal class CCToggleVisibilityState : CCActionInstantState
	{

		public CCToggleVisibilityState (CCToggleVisibility action, CCNode target)
			: base (action, target)
		{	
			target.Visible = !target.Visible;
		}

	}

}