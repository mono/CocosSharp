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
	public class CCJumpTiles3D : CCTiledGrid3DAction
	{
		/// <summary>
		/// amplitude of the sin
		/// </summary>
		protected internal int NumberOfJumps { get; private set; }


		#region Constructors

		public CCJumpTiles3D (float duration, CCGridSize gridSize, int numberOfJumps = 0, float amplitude = 0)
			: base (duration, gridSize, amplitude)
		{
			NumberOfJumps = numberOfJumps;
		}

		#endregion Constructors


		protected internal override CCActionState StartAction(CCNode target)
		{
			return new CCJumpTiles3DState (this, target);
		}
	}


	#region Action state

	internal class CCJumpTiles3DState : CCTiledGrid3DActionState
	{
		protected int NumberOfJumps { get; set; }


		public CCJumpTiles3DState (CCJumpTiles3D action, CCNode target) : base (action, target)
		{
			NumberOfJumps = action.NumberOfJumps;
		}

		public override void Update (float time)
		{
			int i, j;

			float sinz = ((float)Math.Sin ((float)Math.PI * time * NumberOfJumps * 2) * Amplitude * AmplitudeRate);
			float sinz2 = (float)(Math.Sin ((float)Math.PI * (time * NumberOfJumps * 2 + 1)) * Amplitude * AmplitudeRate);

			for (i = 0; i < GridSize.X; i++)
			{
				for (j = 0; j < GridSize.Y; j++)
				{
					CCQuad3 coords = OriginalTile (i, j);

					if (((i + j) % 2) == 0)
					{
						coords.BottomLeft.Z += sinz;
						coords.BottomRight.Z += sinz;
						coords.TopLeft.Z += sinz;
						coords.TopRight.Z += sinz;
					}
					else
					{
						coords.BottomLeft.Z += sinz2;
						coords.BottomRight.Z += sinz2;
						coords.TopLeft.Z += sinz2;
						coords.TopRight.Z += sinz2;
					}

					SetTile (i, j, ref coords);
				}
			}
		}
	}

	#endregion Action state
}