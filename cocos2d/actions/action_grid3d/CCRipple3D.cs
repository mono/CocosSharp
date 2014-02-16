using System;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public class CCRipple3D : CCLiquid
    {
        public CCPoint Position { get; private set; }
        public float Radius { get; private set; }


        #region Constructors

		public CCRipple3D(float duration, CCGridSize gridSize) 
			: this(duration, gridSize, CCPoint.Zero, 0, 0, 0)
		{
		}

		public CCRipple3D(float duration, CCGridSize gridSize, CCPoint position, float radius, int waves, float amplitude) 
            : base(duration, gridSize, waves, amplitude)
        {
            Position = position;
            Radius = radius;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCRipple3DState(this, target);
        }
    }


    #region Action state

    public class CCRipple3DState : CCLiquidState
    {
        protected CCRipple3D Ripple3DAction
        { 
            get { return Action as CCRipple3D; } 
        }

        public CCRipple3DState(CCRipple3D action, CCNode target) : base(action, target)
        {
        }

        public override void Update(float time)
        {
            int i, j;

            CCRipple3D ripple3DAction = Ripple3DAction;
            CCGridSize gridSize = ripple3DAction.GridSize;
            int waves = ripple3DAction.Waves;
            float amplitude = ripple3DAction.Amplitude;
            float radius = ripple3DAction.Radius;

            for (i = 0; i < (gridSize.X + 1); ++i)
            {
                for (j = 0; j < (gridSize.Y + 1); ++j)
                {
                    CCVertex3F v = OriginalVertex(i, j);

                    CCPoint diff = ripple3DAction.Position - new CCPoint(v.X, v.Y);
                    float r = diff.Length;

                    if (r < radius)
                    {
                        r = radius - r;
                        float r1 = r / radius;
                        float rate = r1 * r1;
                        v.Z += ((float) Math.Sin(time * MathHelper.Pi * waves * 2 + r * 0.1f) * amplitude *
                            StateAmplitudeRate * rate);
                    }

                    SetVertex(i, j, ref v);
                }
            }
        }

    }

    #endregion Action state
}