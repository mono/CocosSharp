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
        protected CCLiquid LiquidAction 
        { 
            get { return Action as CCLiquid; } 
        }

        public CCLiquidState(CCLiquid action, CCNode target) : base(action, target)
        {
        }

        public override void Update(float time)
        {
            int i, j;

            CCLiquid liquidAction = LiquidAction;
            CCGridSize gridSize = liquidAction.GridSize;

            for (i = 1; i < gridSize.X; ++i)
            {
                for (j = 1; j < gridSize.Y; ++j)
                {
                    int waves = liquidAction.Waves;
                    float amplitude = liquidAction.Amplitude;
                    float ampRate = StateAmplitudeRate;
                    CCVertex3F v = OriginalVertex(i, j);
                    v.X = (v.X +
                        ((float) Math.Sin(time * (float) Math.PI * waves * 2 + v.X * .01f) * amplitude *
                            ampRate));
                    v.Y = (v.Y +
                        ((float) Math.Sin(time * (float) Math.PI * waves * 2 + v.Y * .01f) * amplitude *
                            ampRate));
                    SetVertex(i, j, ref v);
                }
            }
        }
    }

    #endregion Action state
}