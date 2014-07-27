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
	public class CCSplitRows : CCTiledGrid3DAction
	{
		protected internal int Rows { get; private set; }


		#region Constructors

		/// <summary>
		///  creates the action with the number of rows to split and the duration 
		/// </summary>
		public CCSplitRows (float duration, int nRows) : base (duration, new CCGridSize (1, nRows))
		{
			Rows = nRows;
		}

		#endregion Constructors


		internal override CCActionState StartAction(CCNode target)
		{
			return new CCSplitRowsState (this, target);
		}
	}


	#region Action state

	internal class CCSplitRowsState : CCTiledGrid3DActionState
	{
		protected CCRect VisibleBounds { get; private set; }

		public CCSplitRowsState (CCSplitRows action, CCNode target) : base (action, target)
		{
            VisibleBounds = Layer.VisibleBoundsWorldspace;
		}

		public override void Update (float time)
		{
			int j;

			for (j = 0; j < GridSize.Y; ++j) {
				CCQuad3 coords = OriginalTile (0, j);
				float direction = 1;

				if ((j % 2) == 0) {
					direction = -1;
				}

                coords.BottomLeft.X += direction * VisibleBounds.Size.Width * time;
                coords.BottomRight.X += direction * VisibleBounds.Size.Width * time;
                coords.TopLeft.X += direction * VisibleBounds.Size.Width * time;
                coords.TopRight.X += direction * VisibleBounds.Size.Width * time;

				SetTile (0, j, ref coords);
			}
		}

	}

	#endregion Action state
}