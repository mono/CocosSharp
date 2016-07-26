using System;

namespace CocosSharp
{
    public class CCBezierBy : CCFiniteTimeAction
    {
        public CCBezierConfig BezierConfig { get; private set; }


        #region Constructors

        public CCBezierBy (float t, CCBezierConfig config) : base (t)
        {
            BezierConfig = config;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCBezierByState (this, target);

        }

        public override CCFiniteTimeAction Reverse ()
        {
            CCBezierConfig r;

            r.EndPosition = -BezierConfig.EndPosition;
            r.ControlPoint1 = BezierConfig.ControlPoint2 + -BezierConfig.EndPosition;
            r.ControlPoint2 = BezierConfig.ControlPoint1 + -BezierConfig.EndPosition;

            var action = new CCBezierBy (Duration, r);
            return action;
        }
    }

    public class CCBezierByState : CCFiniteTimeActionState
    {
        protected CCBezierConfig BezierConfig { get; set; }

        protected CCPoint StartPosition { get; set; }

        protected CCPoint PreviousPosition { get; set; }


        public CCBezierByState (CCBezierBy action, CCNode target)
            : base (action, target)
        { 
            BezierConfig = action.BezierConfig;
            PreviousPosition = StartPosition = target.Position;
        }

        public override void Update (float time)
        {
            if (Target != null)
            {
                float xa = 0;
                float xb = BezierConfig.ControlPoint1.X;
                float xc = BezierConfig.ControlPoint2.X;
                float xd = BezierConfig.EndPosition.X;

                float ya = 0;
                float yb = BezierConfig.ControlPoint1.Y;
                float yc = BezierConfig.ControlPoint2.Y;
                float yd = BezierConfig.EndPosition.Y;

                float x = CCSplineMath.CubicBezier (xa, xb, xc, xd, time);
                float y = CCSplineMath.CubicBezier (ya, yb, yc, yd, time);

                CCPoint currentPos = Target.Position;
                CCPoint diff = currentPos - PreviousPosition;
                StartPosition = StartPosition + diff;

                CCPoint newPos = StartPosition + new CCPoint (x, y);
                Target.Position = newPos;

                PreviousPosition = newPos;
            }
        }

    }

    public struct CCBezierConfig
    {
        public CCPoint ControlPoint1;
        public CCPoint ControlPoint2;
        public CCPoint EndPosition;
    }
}