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
        protected CCPoint CachedPosition { get; private set; }
        protected float CachedRadius { get; private set; }

        public CCRipple3DState(CCRipple3D action, CCNode target) : base(action, target)
        {
            CachedPosition = action.Position;
            CachedRadius = action.Radius;
        }

        public override void Update(float time)
        {
            int i, j;

            for (i = 0; i < (CachedGridSize.X + 1); ++i)
            {
                for (j = 0; j < (CachedGridSize.Y + 1); ++j)
                {
                    CCVertex3F v = OriginalVertex(i, j);

                    CCPoint diff = CachedPosition- new CCPoint(v.X, v.Y);
                    float r = diff.Length;

                    if (r < CachedRadius)
                    {
                        r = CachedRadius - r;
                        float r1 = r / CachedRadius;
                        float rate = r1 * r1;
                        v.Z += ((float) Math.Sin(time * MathHelper.Pi * CachedWaves * 2 + r * 0.1f) * CachedAmplitude *
                            StateAmplitudeRate * rate);
                    }

                    SetVertex(i, j, ref v);
                }
            }
        }

    }

    #endregion Action state
}