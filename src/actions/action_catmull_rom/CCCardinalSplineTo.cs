using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCCardinalSplineTo : CCFiniteTimeAction
    {
        #region Properties

        public float Tension { get; protected set; }
        public List<CCPoint> Points { get; protected set; }

        #endregion Properties


        #region Constructors

        public CCCardinalSplineTo (float duration, List<CCPoint> points, float tension) : base (duration)
        {
            Debug.Assert (points.Count > 0, "Invalid configuration. It must at least have one control point");

            Points = points;
            Tension = tension;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCCardinalSplineToState (this, target);

        }

        public override CCFiniteTimeAction Reverse()
        {
            List<CCPoint> reverse = new List<CCPoint> (Points);
            reverse.Reverse ();

            return new CCCardinalSplineTo (Duration, reverse, Tension);
        }

    }

    internal class CCCardinalSplineToState : CCFiniteTimeActionState
    {
        protected float DeltaT { get; set; }

        protected CCPoint PreviousPosition { get; set; }

        protected CCPoint AccumulatedDiff { get; set; }

        protected List<CCPoint> Points { get; set; }

        protected float Tension { get; set; }


        public CCCardinalSplineToState (CCCardinalSplineTo action, CCNode target)
            : base (action, target)
        { 
            Points = action.Points;
            Tension = action.Tension;

            DeltaT = 1f / Points.Count;

            PreviousPosition = target.Position;
            AccumulatedDiff = CCPoint.Zero;
        }


        public override void Update (float time)
        {
            int p;
            float lt;

            // eg.
            // p..p..p..p..p..p..p
            // 1..2..3..4..5..6..7
            // want p to be 1, 2, 3, 4, 5, 6
            if (time == 1)
            {
                p = Points.Count - 1;
                lt = 1;
            }
            else
            {
                p = (int)(time / DeltaT);
                lt = (time - DeltaT * p) / DeltaT;
            }

            // Interpolate    
            var c = Points.Count - 1;
            CCPoint pp0 = Points [Math.Min (c, Math.Max (p - 1, 0))];
            CCPoint pp1 = Points [Math.Min (c, Math.Max (p + 0, 0))];
            CCPoint pp2 = Points [Math.Min (c, Math.Max (p + 1, 0))];
            CCPoint pp3 = Points [Math.Min (c, Math.Max (p + 2, 0))];

            CCPoint newPos = CCSplineMath.CCCardinalSplineAt (pp0, pp1, pp2, pp3, Tension, lt);

            // Support for stacked actions
            CCNode node = Target;
            CCPoint diff = node.Position - PreviousPosition;
            if (diff.X != 0 || diff.Y != 0)
            {
                AccumulatedDiff = AccumulatedDiff + diff;
                newPos = newPos + AccumulatedDiff;
            }

            UpdatePosition (newPos);
        }

        public virtual void UpdatePosition (CCPoint newPos)
        {
            Target.Position = newPos;
            PreviousPosition = newPos;
        }

    }



    public class CCCardinalSplineBy : CCCardinalSplineTo
    {

        #region Constructors

        public CCCardinalSplineBy (float duration, List<CCPoint> points, float tension) : base (duration, points, tension)
        {
        }

        #endregion Constructors

        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCCardinalSplineByState (this, target);

        }

        public override CCFiniteTimeAction Reverse ()
        {
            List<CCPoint> copyConfig = new List<CCPoint> (Points);

            //
            // convert "absolutes" to "diffs"
            //
            CCPoint p = copyConfig [0];

            for (int i = 1; i < copyConfig.Count; ++i)
            {
                CCPoint current = copyConfig [i];
                CCPoint diff = (current - p);
                copyConfig [i] = diff;

                p = current;
            }

            // convert to "diffs" to "reverse absolute"
            copyConfig.Reverse ();

            // 1st element (which should be 0,0) should be here too

            p = copyConfig [copyConfig.Count - 1];
            copyConfig.RemoveAt (copyConfig.Count - 1);

            p = -p;
            copyConfig.Insert (0, p);

            for (int i = 1; i < copyConfig.Count; ++i)
            {
                CCPoint current = copyConfig [i];
                current = -current;
                CCPoint abs = current + p;
                copyConfig [i] = abs;

                p = abs;
            }

            return new CCCardinalSplineBy (Duration, copyConfig, Tension);
        }

    }


    internal class CCCardinalSplineByState : CCCardinalSplineToState
    {
        protected CCPoint StartPosition { get; set; }

        public CCCardinalSplineByState (CCCardinalSplineTo action, CCNode target)
            : base (action, target)
        { 
            StartPosition = target.Position;
        }

        public override void UpdatePosition (CCPoint newPos)
        {
            Target.Position = newPos + StartPosition;
            PreviousPosition = Target.Position;
        }

    }

    public class CCCatmullRomTo : CCCardinalSplineTo
    {
        public CCCatmullRomTo (float dt, List<CCPoint> points) : base (dt, points, 0.5f)
        {
        }
    }

    public class CCCatmullRomBy : CCCardinalSplineBy
    {
        public CCCatmullRomBy (float dt, List<CCPoint> points) : base (dt, points, 0.5f)
        {
        }
    }
}