namespace CocosSharp
{
    public class CCActionCamera : CCFiniteTimeAction
    {

        #region Constructors

        protected CCActionCamera (float duration) : base (duration)
        {
        }

        #endregion Constructors

        // Start the Camera operation on the given target.
        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCActionCameraState (this, target);

        }

        public override CCFiniteTimeAction Reverse ()
        {
            return new CCReverseTime (this);
        }
    }

    internal class CCActionCameraState : CCFiniteTimeActionState
    {
        protected CCPoint3 CameraCenter;
        protected CCPoint3 CameraTarget;
        protected CCPoint3 CameraUpDirection;

        public CCActionCameraState (CCActionCamera action, CCNode target)
            : base (action, target)
        {
            target.FauxLocalCameraCenter = new CCPoint3(target.AnchorPointInPoints, 0.0f);
            target.FauxLocalCameraTarget = new CCPoint3(target.AnchorPointInPoints, 0.0f);

            CameraCenter = target.FauxLocalCameraCenter;
            CameraTarget = target.FauxLocalCameraTarget;
            CameraUpDirection = target.FauxLocalCameraUpDirection;
        }

    }
}