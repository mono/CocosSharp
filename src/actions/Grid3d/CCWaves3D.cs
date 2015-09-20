using System;

namespace CocosSharp
{
    public class CCWaves3D : CCLiquid
    {

        #region Constructors

        public CCWaves3D (float duration, CCGridSize gridSize, int waves = 0, float amplitude = 0) : base (duration, gridSize, waves, amplitude)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCWaves3DState (this, GridNode(target));
        }
    }


    #region Action state

    public class CCWaves3DState : CCLiquidState
    {
        public CCWaves3DState (CCWaves3D action, CCNodeGrid target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            if (Target == null)
                return;
            
            int i, j;

            for (i = 0; i < GridSize.X + 1; ++i)
            {
                for (j = 0; j < GridSize.Y + 1; ++j)
                {
                    CCVertex3F v = OriginalVertex (i, j);
                    v.Z += ((float)Math.Sin ((float)Math.PI * time * Waves * 2 + (v.Y + v.X) * .01f) * Amplitude *
                        AmplitudeRate);
                    SetVertex (i, j, ref v);
                }
            }
        }

    }

    #endregion Action state
}