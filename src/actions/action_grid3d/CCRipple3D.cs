using System;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
	public class CCRipple3D : CCGrid3DAction
	{
		public CCPoint Position { get; private set; }

		public float Radius { get; private set; }

		public int Waves { get; private set; }


		#region Constructors

		public CCRipple3D (float duration, CCGridSize gridSize)
			: this (duration, gridSize, CCPoint.Zero, 0, 0, 0)
		{
		}

		public CCRipple3D (float duration, CCGridSize gridSize, CCPoint position, float radius, int waves, float amplitude)
			: base (duration, gridSize, amplitude)
		{
			Position = position;
			Radius = radius;
			Waves = waves;
		}

		#endregion Constructors


		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCRipple3DState (this, target);
		}
	}


	#region Action state

	public class CCRipple3DState : CCGrid3DActionState
	{
		public CCPoint Position { get; set; }

		public float Radius { get; set; }

		public int Waves { get; set; }


		public CCRipple3DState (CCRipple3D action, CCNode target) : base (action, target)
		{
			Position = action.Position;
			Radius = action.Radius;
			Waves = action.Waves;
		}

		public override void Update (float time)
		{
			int i, j;

			for (i = 0; i < (GridSize.X + 1); ++i)
			{
				for (j = 0; j < (GridSize.Y + 1); ++j)
				{
					CCVertex3F v = OriginalVertex (i, j);

					CCPoint diff = Position - new CCPoint (v.X, v.Y);
					float r = diff.Length;

					if (r < Radius)
					{
						r = Radius - r;
						float r1 = r / Radius;
						float rate = r1 * r1;
						v.Z += ((float)Math.Sin (time * MathHelper.Pi * Waves * 2 + r * 0.1f) * Amplitude *
						AmplitudeRate * rate);
					}

					SetVertex (i, j, ref v);
				}
			}
		}

	}

	#endregion Action state
}