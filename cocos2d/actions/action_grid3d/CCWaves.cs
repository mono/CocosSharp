using System;

namespace CocosSharp
{
    public class CCWaves : CCLiquid
    {
        protected internal bool Vertical { get; private set; }
        protected internal bool Horizontal { get; private set; }


		#region Constructors

		public CCWaves(float duration, CCGridSize gridSize, int waves = 0, float amplitude = 0, bool horizontal = true, bool vertical = true)
            : base(duration, gridSize, waves, amplitude)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCWavesState(this, target);
        }
    }


    #region Action state

    public class CCWavesState : CCLiquidState
    {
        protected CCWaves WavesAction
        { 
            get { return Action as CCWaves; } 
        }

        public CCWavesState(CCWaves action, CCNode target) : base(action, target)
        {
        }

        public override void Update(float time)
        {
            int i, j;
            CCWaves wavesAction = WavesAction;
            CCGridSize gridSize = wavesAction.GridSize;
            int waves = wavesAction.Waves;
            float amplitude = wavesAction.Amplitude;
            float ampRate = StateAmplitudeRate;

            for (i = 0; i < gridSize.X + 1; ++i)
            {
                for (j = 0; j < gridSize.Y + 1; ++j)
                {
                    CCVertex3F v = OriginalVertex(i, j);

                    if (wavesAction.Vertical)
                    {
                        v.X = (v.X +
                            ((float) Math.Sin(time * (float) Math.PI * waves * 2 + v.Y * .01f) * amplitude * ampRate));
                    }

                    if (wavesAction.Horizontal)
                    {
                        v.Y = (v.Y +
                            ((float) Math.Sin(time * (float) Math.PI * waves * 2 + v.X * .01f) * amplitude * ampRate));
                    }

                    SetVertex(i, j, ref v);
                }
            }
        }

    }

    #endregion Action state
}