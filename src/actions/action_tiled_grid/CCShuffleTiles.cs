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
    /// @brief CCShuffleTiles action
    /// Shuffle the tiles in random order
    /// </summary>
    public class CCShuffleTiles : CCTiledGrid3DAction
    {
        protected internal const int NoSeedSpecified = -1;

        protected internal int Seed { get; private set; }


        #region Constructors

        /// <summary>
        /// creates the action with a random seed, the grid size and the duration 
        /// </summary>
        public CCShuffleTiles (CCGridSize gridSize, float duration, int seed = NoSeedSpecified)
            : base (duration, gridSize)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCShuffleTilesState (this, target);
        }
    }


    #region Action state

    public class CCShuffleTilesState : CCTiledGrid3DActionState
    {
        protected int TilesCount { get; private set; }

        protected CCTile[] Tiles { get; private set; }

        protected int[] TilesOrder { get; private set; }


        public CCShuffleTilesState (CCShuffleTiles action, CCNode target) : base (action, target)
        {
            CCGridSize gridSize = action.GridSize;
            TilesCount = gridSize.X * gridSize.Y;
            int[] shuffledTilesOrder = new int[TilesCount];
            int i, j, f = 0;

            for (i = 0; i < TilesCount; i++) {
                shuffledTilesOrder [i] = i;
            }

            if (action.Seed != CCShuffleTiles.NoSeedSpecified) {
                CCRandom.Next (action.Seed);
            }


            Shuffle (ref shuffledTilesOrder, TilesCount);
            TilesOrder = shuffledTilesOrder;

            Tiles = new CCTile[TilesCount];

            for (i = 0; i < gridSize.X; ++i) {
                for (j = 0; j < gridSize.Y; ++j) {
                    Tiles [f] = new CCTile {
                        Position = new CCPoint (i, j),
                        StartPosition = new CCPoint (i, j),
                        Delta = GetDelta (i, j)
                    };

                    f++;
                }
            }
        }

        public override void Update (float time)
        {
            int i, j, f = 0;

            for (i = 0; i < GridSize.X; ++i) {
                for (j = 0; j < GridSize.Y; ++j) {
                    CCTile item = Tiles [f];
                    item.Position = new CCPoint ((item.Delta.X * time), (item.Delta.Y * time));
                    PlaceTile (i, j, item);

                    f++;
                }
            }
        }


        #region Tile Shuffling

        public void Shuffle (ref int[] pArray, int nLen)
        {
            int i;
            for (i = nLen - 1; i >= 0; i--) {
                int j = CCRandom.Next () % (i + 1);
                int v = pArray [i];
                pArray [i] = pArray [j];
                pArray [j] = v;
            }
        }

        protected CCGridSize GetDelta (CCGridSize pos)
        {
            var pos2 = CCPoint.Zero;

            int idx = pos.X * GridSize.Y + pos.Y;
            int tileOrder = TilesOrder [idx];

            pos2.X = (tileOrder / GridSize.Y);
            pos2.Y = (tileOrder % GridSize.Y);

            return new CCGridSize ((int)(pos2.X - pos.X), (int)(pos2.Y - pos.Y));
        }

        protected CCGridSize GetDelta (int x, int y)
        {
            var pos2 = CCPoint.Zero;

            int idx = x * GridSize.Y + y;
            int tileOrder = TilesOrder [idx];

            pos2.X = (tileOrder / GridSize.Y);
            pos2.Y = (tileOrder % GridSize.Y);

            return new CCGridSize ((int)(pos2.X - x), (int)(pos2.Y - y));
        }

        protected void PlaceTile (CCGridSize pos, CCTile tile)
        {
            CCQuad3 coords = OriginalTile (pos);

            CCPoint step = Target.Grid.Step;
            coords.BottomLeft.X += (int)(tile.Position.X * step.X);
            coords.BottomLeft.Y += (int)(tile.Position.Y * step.Y);

            coords.BottomRight.X += (int)(tile.Position.X * step.X);
            coords.BottomRight.Y += (int)(tile.Position.Y * step.Y);

            coords.TopLeft.X += (int)(tile.Position.X * step.X);
            coords.TopLeft.Y += (int)(tile.Position.Y * step.Y);

            coords.TopRight.X += (int)(tile.Position.X * step.X);
            coords.TopRight.Y += (int)(tile.Position.Y * step.Y);

            SetTile (pos, ref coords);
        }

        protected void PlaceTile (int x, int y, CCTile tile)
        {
            CCQuad3 coords = OriginalTile (x, y);

            CCPoint step = Target.Grid.Step;
            coords.BottomLeft.X += (int)(tile.Position.X * step.X);
            coords.BottomLeft.Y += (int)(tile.Position.Y * step.Y);

            coords.BottomRight.X += (int)(tile.Position.X * step.X);
            coords.BottomRight.Y += (int)(tile.Position.Y * step.Y);

            coords.TopLeft.X += (int)(tile.Position.X * step.X);
            coords.TopLeft.Y += (int)(tile.Position.Y * step.Y);

            coords.TopRight.X += (int)(tile.Position.X * step.X);
            coords.TopRight.Y += (int)(tile.Position.Y * step.Y);

            SetTile (x, y, ref coords);
        }

        #endregion Tile Shuffling
    }

    #endregion Action state
}