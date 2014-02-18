using System;

namespace CocosSharp
{
    public class CCLiquid : CCGrid3DAction
    {
        public float Amplitude { get; private set; }
        public override float AmplitudeRate { get; protected set; }
        public int Waves { get; private set; }


        #region Constructors

		public CCLiquid(float duration, CCGridSize gridSize, int waves = 0, float amplitude = 0) : base(duration, gridSize)
        {
            Waves = waves;
            Amplitude = amplitude;
            AmplitudeRate = 1.0f;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCLiquidState(this, target);
        }
    }


    #region Action state

    public class CCLiquidState : CCGrid3DActionState
    {
        public float CachedAmplitude { get; private set; }
        public int CachedWaves { get; private set; }


        public CCLiquidState(CCLiquid action, CCNode target) : base(action, target)
        {
            CachedAmplitude = action.Amplitude;
            CachedWaves = action.Waves;
        }

        public override void Update(float time)
        {
            int i, j;

            for (i = 1; i < CachedGridSize.X; ++i)
            {
                for (j = 1; j < CachedGridSize.Y; ++j)
                {
                    CCVertex3F v = OriginalVertex(i, j);
                    v.X = (v.X +
                        ((float) Math.Sin(time * (float) Math.PI * CachedWaves * 2 + v.X * .01f) * CachedAmplitude *
                            StateAmplitudeRate));
                    v.Y = (v.Y +
                        ((float) Math.Sin(time * (float) Math.PI * CachedWaves * 2 + v.Y * .01f) * CachedAmplitude *
                            StateAmplitudeRate));
                    SetVertex(i, j, ref v);
                }
            }
        }
    }

    #endregion Action state
}