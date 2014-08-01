namespace CocosSharp
{
    public class CCMoveBy : CCFiniteTimeAction
    {
        #region Constructors

        public CCMoveBy (float duration, CCPoint position) : base (duration)
        {
            PositionDelta = position;
        }

        #endregion Constructors

        public CCPoint PositionDelta { get; private set; }

        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCMoveByState (this, target);
        }

        public override CCFiniteTimeAction Reverse ()
        {
            return new CCMoveBy (Duration, new CCPoint (-PositionDelta.X, -PositionDelta.Y));
        }
    }

    internal class CCMoveByState : CCFiniteTimeActionState
    {
        protected CCPoint PositionDelta;
        protected CCPoint EndPosition;
        protected CCPoint StartPosition;
        protected CCPoint PreviousPosition;

        public CCMoveByState (CCMoveBy action, CCNode target)
            : base (action, target)
        { 
            PositionDelta = action.PositionDelta;
            PreviousPosition = StartPosition = target.Position;
        }

        public override void Update (float time)
        {
            if (Target == null)
                return;

            var currentPos = Target.Position;
            var diff = currentPos - PreviousPosition;
            StartPosition = StartPosition + diff;
            CCPoint newPos = StartPosition + PositionDelta * time;
            Target.Position = newPos;
            PreviousPosition = newPos;
        }
    }

}