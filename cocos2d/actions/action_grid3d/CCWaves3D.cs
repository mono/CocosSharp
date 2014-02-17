using System;

namespace CocosSharp
{
    public class CCWaves3D : CCLiquid
    {
    
        #region Constructors

        public CCWaves3D(float duration, CCGridSize gridSize, int waves = 0, float amplitude = 0) : base(duration, gridSize, waves, amplitude)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCWaves3DState(this, target);
        }
    }


    #region Action state

    public class CCWaves3DState : CCLiquidState
    {

        protected CCWaves3D Waves3DAction
        { 
            get { return Action as CCWaves3D; } 
        }

        public CCWaves3DState(CCWaves3D action, CCNode target) : base(action, target)
        {
        }

        public override void Update(float time)
        {
            int i, j;
            CCWaves3D waves3DAction = Waves3DAction;
            CCGridSize gridSize = waves3DAction.GridSize;
            int waves = waves3DAction.Waves;
			float amplitude = waves3DAction.Amplitude;

			for (i = 0; i < gridSize.X + 1; ++i)
            {
                for (j = 0; j < gridSize.Y + 1; ++j)
                {
                    CCVertex3F v = OriginalVertex(i, j);
                    v.Z += ((float) Math.Sin((float) Math.PI * time * waves * 2 + (v.Y + v.X) * .01f) * amplitude *
                        StateAmplitudeRate);
                    SetVertex(i, j, ref v);
                }
            }
        }

    }

    #endregion Action state
}