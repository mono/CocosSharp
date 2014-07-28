/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.
Copyright (c) 2011-2012 openxlive.com
 
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

using System;

namespace CocosSharp
{
	public class CCWavesTiles3D : CCTiledGrid3DAction
	{
		protected internal int Waves { get; private set; }


		#region Constructors

		/// <summary>
		/// creates the action with a number of waves, the waves amplitude, the grid size and the duration
		/// </summary>
		public CCWavesTiles3D (float duration, CCGridSize gridSize, int waves = 0, float amplitude = 0)
			: base (duration, gridSize, amplitude)
		{
			Waves = waves;
		}

		#endregion Constructors


		protected internal override CCActionState StartAction(CCNode target)
		{
			return new CCWavesTiles3DState (this, target);
		}
	}


	#region Action state

	internal class CCWavesTiles3DState : CCTiledGrid3DActionState
	{
		public int Waves { get; set; }

		public CCWavesTiles3DState (CCWavesTiles3D action, CCNode target) : base (action, target)
		{
			Waves = action.Waves;
		}

		public override void Update (float time)
		{
			int i, j;

			for (i = 0; i < GridSize.X; i++)
			{
				for (j = 0; j < GridSize.Y; j++)
				{
					CCQuad3 coords = OriginalTile (i, j);
					CCVertex3F bl = coords.BottomLeft;
					CCVertex3F br = coords.BottomRight;
					CCVertex3F tl = coords.TopLeft;
					CCVertex3F tr = coords.TopRight;

					bl.Z = ((float)Math.Sin (time * (float)Math.PI * Waves * 2 + (bl.Y + bl.X) * .01f) * Amplitude * AmplitudeRate);
					br.Z = bl.Z;
					tl.Z = bl.Z;
					tr.Z = bl.Z;

					coords.BottomLeft = bl;
					coords.BottomRight = br;
					coords.TopLeft = tl;
					coords.TopRight = tr;

					SetTile (i, j, ref coords);
				}
			}
		}
	}

	#endregion Action state
}