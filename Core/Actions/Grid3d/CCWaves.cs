using System;

namespace CocosSharp
{
    public class CCWaves : CCLiquid
    {
        protected internal bool Vertical { get; private set; }
        protected internal bool Horizontal { get; private set; }


        #region Constructors

        public CCWaves (float duration, CCGridSize gridSize, int waves = 0, float amplitude = 0, bool horizontal = true, bool vertical = true)
            : base (duration, gridSize, waves, amplitude)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCWavesState (this, GridNode(target));
        }
    }


    #region Action state

    public class CCWavesState : CCLiquidState
    {
        public bool Vertical { get; set; }

        public bool Horizontal { get; set; }

        public CCWavesState (CCWaves action, CCNodeGrid target) : base (action, target)
        {
            Vertical = action.Vertical;
            Horizontal = action.Horizontal;
        }

        public override void Update (float time)
        {
            if (Target == null)
                return;
            
            int i, j;
            float ampRate = AmplitudeRate;

            for (i = 0; i < GridSize.X + 1; ++i)
            {
                for (j = 0; j < GridSize.Y + 1; ++j)
                {
                    CCVertex3F v = OriginalVertex (i, j);

                    if (Vertical)
                    {
                        v.X = (v.X +
                            ((float)Math.Sin (time * (float)Math.PI * Waves * 2 + v.Y * .01f) * Amplitude * ampRate));
                    }

                    if (Horizontal)
                    {
                        v.Y = (v.Y +
                            ((float)Math.Sin (time * (float)Math.PI * Waves * 2 + v.X * .01f) * Amplitude * ampRate));
                    }

                    SetVertex (i, j, ref v);
                }
            }
        }

    }

    #endregion Action state
}