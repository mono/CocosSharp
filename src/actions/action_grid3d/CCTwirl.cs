using System;

namespace CocosSharp
{
    public class CCTwirl : CCGrid3DAction
    {
        public int Twirls { get; private set; }

        public CCPoint Position { get; private set; }


        #region Constructors

        public CCTwirl (float duration, CCGridSize gridSize)
            : this (duration, gridSize, CCPoint.Zero)
        {
        }

        public CCTwirl (float duration, CCGridSize gridSize, CCPoint position, int twirls = 0, float amplitude = 0)
            : base (duration, gridSize, amplitude)
        {
            Position = position;
            Twirls = twirls;
        }

        #endregion Constructors


        internal override CCActionState StartAction(CCNode target)
        {
            return new CCTwirlState (this, target);
        }
    }


    #region Action state

    internal class CCTwirlState : CCGrid3DActionState
    {
        public int Twirls { get; set; }

        public CCPoint PositionInPixels { get; set; }

        public CCTwirlState (CCTwirl action, CCNode target) : base (action, target)
        {
            Twirls = action.Twirls;
            PositionInPixels = action.Position;
        }

        public override void Update (float time)
        {
            int i, j;
            CCPoint avg = CCPoint.Zero;
            CCPoint c = PositionInPixels;
            int twirls = Twirls;

            for (i = 0; i < (GridSize.X + 1); ++i)
            {
                for (j = 0; j < (GridSize.Y + 1); ++j)
                {
                    CCVertex3F v = OriginalVertex (i, j);

                    avg.X = i - (GridSize.X / 2.0f);
                    avg.Y = j - (GridSize.Y / 2.0f);

                    var r = (float)Math.Sqrt ((avg.X * avg.X + avg.Y * avg.Y));

                    float amp = 0.1f * Amplitude * AmplitudeRate;
                    float a = r * (float)Math.Cos ((float)Math.PI / 2.0f + time * (float)Math.PI * twirls * 2) * amp;

                    float dx = (float)Math.Sin (a) * (v.Y - c.Y) + (float)Math.Cos (a) * (v.X - c.X);
                    float dy = (float)Math.Cos (a) * (v.Y - c.Y) - (float)Math.Sin (a) * (v.X - c.X);

                    v.X = c.X + dx;
                    v.Y = c.Y + dy;

                    SetVertex (i, j, ref v);
                }
            }
        }

    }

    #endregion Action state
}