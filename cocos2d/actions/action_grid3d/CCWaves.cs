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
        protected bool CachedVertical { get; private set; }
        protected bool CachedHorizontal { get; private set; }

        public CCWavesState(CCWaves action, CCNode target) : base(action, target)
        {
            CachedVertical = action.Vertical;
            CachedHorizontal = action.Horizontal;
        }

        public override void Update(float time)
        {
            int i, j;
            float ampRate = StateAmplitudeRate;

            for (i = 0; i < CachedGridSize.X + 1; ++i)
            {
                for (j = 0; j < CachedGridSize.Y + 1; ++j)
                {
                    CCVertex3F v = OriginalVertex(i, j);

                    if (CachedVertical)
                    {
                        v.X = (v.X +
                            ((float) Math.Sin(time * (float) Math.PI * CachedWaves * 2 + v.Y * .01f) * CachedAmplitude * ampRate));
                    }

                    if (CachedHorizontal)
                    {
                        v.Y = (v.Y +
                            ((float) Math.Sin(time * (float) Math.PI * CachedWaves * 2 + v.X * .01f) * CachedAmplitude * ampRate));
                    }

                    SetVertex(i, j, ref v);
                }
            }
        }

    }

    #endregion Action state
}