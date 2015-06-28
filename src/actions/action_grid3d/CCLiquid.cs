using System;

namespace CocosSharp
{
    public class CCLiquid : CCGrid3DAction
    {
        public int Waves { get; private set; }


        #region Constructors

        public CCLiquid (float duration, CCGridSize gridSize, int waves = 0, float amplitude = 0)
            : base (duration, gridSize, amplitude)
        {
            Waves = waves;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCLiquidState (this, GridNode(target));
        }
    }


    #region Action state

    public class CCLiquidState : CCGrid3DActionState
    {
        public int Waves { get; set; }

        public CCLiquidState (CCLiquid action, CCNodeGrid target) : base (action, target)
        {
            Waves = action.Waves;
        }

        public override void Update (float time)
        {
            if (Target == null)
                return;
            
            int i, j;

            for (i = 1; i < GridSize.X; ++i)
            {
                for (j = 1; j < GridSize.Y; ++j)
                {
                    CCVertex3F v = OriginalVertex (i, j);
                    v.X = (v.X +
                        ((float)Math.Sin (time * (float)Math.PI * Waves * 2 + v.X * .01f) * Amplitude *
                            AmplitudeRate));
                    v.Y = (v.Y +
                        ((float)Math.Sin (time * (float)Math.PI * Waves * 2 + v.Y * .01f) * Amplitude *
                            AmplitudeRate));
                    SetVertex (i, j, ref v);
                }
            }
        }
    }

    #endregion Action state
}