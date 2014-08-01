namespace CocosSharp
{
    public class CCScaleTo : CCFiniteTimeAction
    {
        public float EndScaleX { get; private set; }
        public float EndScaleY { get; private set; }


        #region Constructors

        public CCScaleTo (float duration, float scale) : this (duration, scale, scale)
        {
        }

        public CCScaleTo (float duration, float scaleX, float scaleY) : base (duration)
        {
            EndScaleX = scaleX;
            EndScaleY = scaleY;
        }

        #endregion Constructors

        public override CCFiniteTimeAction Reverse()
        {
            throw new System.NotImplementedException ();
        }

        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCScaleToState (this, target);
        }
    }

    internal class CCScaleToState : CCFiniteTimeActionState
    {
        protected float DeltaX;
        protected float DeltaY;
        protected float EndScaleX;
        protected float EndScaleY;
        protected float StartScaleX;
        protected float StartScaleY;

        public CCScaleToState (CCScaleTo action, CCNode target)
            : base (action, target)
        { 
            StartScaleX = target.ScaleX;
            StartScaleY = target.ScaleY;
            EndScaleX = action.EndScaleX;
            EndScaleY = action.EndScaleY;
            DeltaX = EndScaleX - StartScaleX;
            DeltaY = EndScaleY - StartScaleY;
        }

        public override void Update (float time)
        {
            if (Target != null)
            {
                Target.ScaleX = StartScaleX + DeltaX * time;
                Target.ScaleY = StartScaleY + DeltaY * time;
            }
        }
    }
}