namespace CocosSharp
{
    public class CCActionCamera : CCActionInterval
    {

        #region Constructors

        protected CCActionCamera (float duration) : base (duration)
        {
        }

        #endregion Constructors

        // Start the Camera operation on the given target.
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
        protected CCPoint3 CameraCenter;
        protected CCPoint3 CameraTarget;
        protected CCPoint3 CameraUpDirection;

        public CCActionCameraState (CCActionCamera action, CCNode target)
            : base (action, target)
        {       
            CameraCenter = target.FauxLocalCameraCenter;
            CameraTarget = target.FauxLocalCameraTarget;
            CameraUpDirection = target.FauxLocalCameraUpDirection;
        }

    }
}