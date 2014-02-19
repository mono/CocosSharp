namespace CocosSharp
{
    public class CCActionCamera : CCActionInterval
    {

        #region Constructors

        protected CCActionCamera(float duration) : base(duration)
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

        public override CCFiniteTimeAction Reverse()
        {
			return new CCReverseTime(this);
        }
    }

	public class CCActionCameraState : CCActionIntervalState
	{

		protected float centerXOrig;
		protected float centerYOrig;
		protected float centerZOrig;

		protected float eyeXOrig;
		protected float eyeYOrig;
		protected float eyeZOrig;

		protected float upXOrig;
		protected float upYOrig;
		protected float upZOrig;

		public CCActionCameraState (CCActionCamera action, CCNode target)
			: base(action, target)
		{	
			CCCamera camera = target.Camera;

			camera.GetCenterXyz(out centerXOrig, out centerYOrig, out centerZOrig);
			camera.GetEyeXyz(out eyeXOrig, out eyeYOrig, out eyeZOrig);
			camera.GetUpXyz(out upXOrig, out upYOrig, out upZOrig);
		}

	}
}