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
        public float Amplitude { get; set; }
        public int Waves { get; set; }


        public CCLiquidState(CCLiquid action, CCNode target) : base(action, target)
        {
            Amplitude = action.Amplitude;
            Waves = action.Waves;
        }

        public override void Update(float time)
        {
            int i, j;

            for (i = 1; i < GridSize.X; ++i)
            {
                for (j = 1; j < GridSize.Y; ++j)
                {
                    CCVertex3F v = OriginalVertex(i, j);
                    v.X = (v.X +
                        ((float) Math.Sin(time * (float) Math.PI * Waves * 2 + v.X * .01f) * Amplitude *
                            StateAmplitudeRate));
                    v.Y = (v.Y +
                        ((float) Math.Sin(time * (float) Math.PI * Waves * 2 + v.Y * .01f) * Amplitude *
                            StateAmplitudeRate));
                    SetVertex(i, j, ref v);
                }
            }
        }
    }

    #endregion Action state
}