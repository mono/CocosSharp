namespace CocosSharp
{
	public class CCHide : CCActionInstant
	{
		#region Constructors

		public CCHide ()
		{
		}

		#endregion Constructors

		/// <summary>
		/// Start the hide operation on the given target.
		/// </summary>
		/// <param name="target"></param>
		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCHideState (this, target);

		}

		public override CCFiniteTimeAction Reverse ()
		{
			return (new CCShow ());
		}

	}

	public class CCHideState : CCActionInstantState
	{

		public CCHideState (CCHide action, CCNode target)
			: base (action, target)
		{	
			target.Visible = false;
		}

	}

}