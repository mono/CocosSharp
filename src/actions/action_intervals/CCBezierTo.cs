namespace CocosSharp
{
	public class CCBezierTo : CCBezierBy
	{
		#region Constructors

		public CCBezierTo (float t, CCBezierConfig c)
			: base (t, c)
		{
		}

		#endregion Constructors


		protected internal override CCActionState StartAction(CCNode target)
		{
			return new CCBezierToState (this, target);

		}

	}

	internal class CCBezierToState : CCBezierByState
	{

		public CCBezierToState (CCBezierBy action, CCNode target)
			: base (action, target)
		{ 
			var config = BezierConfig;

			config.ControlPoint1 -= StartPosition;
			config.ControlPoint2 -= StartPosition;
			config.EndPosition -= StartPosition;

			BezierConfig = config;
		}

	}

}