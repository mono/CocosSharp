namespace CocosSharp
{
    public class CCRotateBy : CCFiniteTimeAction
    {
        public float AngleX { get; private set; }
        public float AngleY { get; private set; }


        #region Constructors

        public CCRotateBy (float duration, float deltaAngleX, float deltaAngleY) : base (duration)
        {
            AngleX = deltaAngleX;
            AngleY = deltaAngleY;
        }

        public CCRotateBy (float duration, float deltaAngle) : this (duration, deltaAngle, deltaAngle)
        {
        }

        #endregion Constructors

        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCRotateByState (this, target);

        }

        public override CCFiniteTimeAction Reverse ()
        {
            return new CCRotateBy (Duration, -AngleX, -AngleY);
        }
    }

    public class CCRotateByState : CCFiniteTimeActionState
    {

        protected float AngleX { get; set; }

        protected float AngleY { get; set; }

        protected float StartAngleX { get; set; }

        protected float StartAngleY { get; set; }

        public CCRotateByState (CCRotateBy action, CCNode target)
            : base (action, target)
        { 
            AngleX = action.AngleX;
            AngleY = action.AngleY;
            StartAngleX = target.RotationX;
            StartAngleY = target.RotationY;

        }

        public override void Update (float time)
        {
            // XXX: shall I add % 360
            if (Target != null)
            {
                Target.RotationX = StartAngleX + AngleX * time;
                Target.RotationY = StartAngleY + AngleY * time;
            }
        }

    }

}