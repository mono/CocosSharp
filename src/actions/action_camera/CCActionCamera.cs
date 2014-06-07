namespace CocosSharp
{
	public class CCActionCamera : CCActionInterval
	{

		#region Constructors

		protected CCActionCamera (float duration) : base (duration)
		{
		}

		#endregion Constructors

		/// <summary>
		/// Start the Camera operation on the given target.
		/// </summary>
		/// <param name="target"></param>
		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCActionCameraState (this, target);

		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCReverseTime (this);
		}
	}

	public class CCActionCameraState : CCActionIntervalState
	{
		protected float CenterXOrig;
		protected float CenterYOrig;
		protected float CenterZOrig;

		protected float EyeXOrig;
		protected float EyeYOrig;
		protected float EyeZOrig;

		protected float UpXOrig;
		protected float UpYOrig;
		protected float UpZOrig;

		public CCActionCameraState (CCActionCamera action, CCNode target)
			: base (action, target)
		{	
			CCCamera camera = target.Camera;

			camera.GetCenterXyz (out CenterXOrig, out CenterYOrig, out CenterZOrig);
			camera.GetEyeXyz (out EyeXOrig, out EyeYOrig, out EyeZOrig);
			camera.GetUpXyz (out UpXOrig, out UpYOrig, out UpZOrig);
		}

	}
}