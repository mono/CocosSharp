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

namespace CocosSharp
{
	/// <summary>
	/// @brief CCTurnOffTiles action.
	/// Turn off the files in random order
	/// </summary>
	public class CCTurnOffTiles : CCShuffleTiles
	{
    
		#region Constructors

		/// <summary>
		/// creates the action with a random seed, the grid size and the duration 
		/// </summary>
		public CCTurnOffTiles (float duration, CCGridSize gridSize, int seed = CCShuffleTiles.NoSeedSpecified)
			: base (gridSize, duration, seed)
		{
		}

		#endregion Constructors


		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCTurnOffTilesState (this, target);
		}
	}


	#region Action state

	public class CCTurnOffTilesState : CCTiledGrid3DActionState
	{
		protected int TilesCount { get; private set; }

		protected int[] TilesOrder { get; private set; }

		CCQuad3 zero;

		public CCTurnOffTilesState (CCTurnOffTiles action, CCNode target) : base (action, target)
		{
			int i;

			if (action.Seed != CCShuffleTiles.NoSeedSpecified)
			{
				CCRandom.Next (action.Seed);
			}

			CCGridSize gridSize = action.GridSize;
			TilesCount = gridSize.X * gridSize.Y;
			TilesOrder = new int[TilesCount];

			for (i = 0; i < TilesCount; ++i)
			{
				TilesOrder [i] = i;
			}

			Shuffle (TilesOrder, TilesCount);
		}

		public override void Update (float time)
		{
			int i, l, t;

			l = (int)(time * TilesCount);

			for (i = 0; i < TilesCount; i++)
			{
				t = TilesOrder [i];
				var tilePos = new CCGridSize (t / GridSize.Y, t % GridSize.Y);

				if (i < l)
				{
					TurnOffTile (tilePos);
				}
				else
				{
					TurnOnTile (tilePos);
				}
			}
		}

		#region Tile shuffling

		public void Shuffle (int[] pArray, int nLen)
		{
			int i;
			for (i = nLen - 1; i >= 0; i--)
			{
				int j = CCRandom.Next () % (i + 1);
				int v = pArray [i];
				pArray [i] = pArray [j];
				pArray [j] = v;
			}
		}

		public void TurnOnTile (CCGridSize pos)
		{
			CCQuad3 orig = OriginalTile (pos);
			SetTile (pos, ref orig);
		}

		public void TurnOffTile (CCGridSize pos)
		{
			SetTile (pos, ref zero);
		}

		#endregion Tile shuffling
	}

	#endregion Action state
}