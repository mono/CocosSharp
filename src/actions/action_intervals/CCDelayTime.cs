namespace CocosSharp
{
	public class CCDelayTime : CCFiniteTimeAction
	{
		#region Constructors

		public CCDelayTime (float d) : base (d)
		{
		}

		#endregion Constructors

		internal override CCActionState StartAction(CCNode target)
		{
			return new CCDelayTimeState (this, target);

		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCDelayTime (Duration);
		}
	}

	internal class CCDelayTimeState : CCFiniteTimeActionState
	{

		public CCDelayTimeState (CCDelayTime action, CCNode target)
			: base (action, target)
		{
		}

		public override void Update (float time)
		{
		}

	}
}