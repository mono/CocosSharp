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
    public class CCShakyTiles3D : CCTiledGrid3DAction
    {
        protected internal bool ShakeZ { get; private set; }
        protected internal int Range { get; private set; }


        #region Constructors

        /// <summary>
        /// creates the action with a range, whether or not to shake Z vertices, a grid size, and duration
        /// </summary>
        public CCShakyTiles3D (float duration, CCGridSize gridSize, int range = 0, bool shakeZ = true) : base (duration, gridSize)
        {
            Range = range;
            ShakeZ = shakeZ;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCShakyTiles3DState (this, target);
        }
    }


    #region Action state

    public class CCShakyTiles3DState : CCTiledGrid3DActionState
    {
        public bool ShakeZ { get; set; }

        public int Range { get; set; }

        public CCShakyTiles3DState (CCShakyTiles3D action, CCNode target) : base (action, target)
        {
            ShakeZ = action.ShakeZ;
            Range = action.Range;
        }

        public override void Update (float time)
        {
            int i, j;

            var doubleRange = Range * 2;

            for (i = 0; i < GridSize.X; ++i) {
                for (j = 0; j < GridSize.Y; ++j) {
                    CCQuad3 coords = OriginalTile (i, j);
                    // X
                    coords.BottomLeft.X += (CCRandom.Next () % doubleRange) - Range;
                    coords.BottomRight.X += (CCRandom.Next () % doubleRange) - Range;
                    coords.TopLeft.X += (CCRandom.Next () % doubleRange) - Range;
                    coords.TopRight.X += (CCRandom.Next () % doubleRange) - Range;

                    // Y
                    coords.BottomLeft.Y += (CCRandom.Next () % doubleRange) - Range;
                    coords.BottomRight.Y += (CCRandom.Next () % doubleRange) - Range;
                    coords.TopLeft.Y += (CCRandom.Next () % doubleRange) - Range;
                    coords.TopRight.Y += (CCRandom.Next () % doubleRange) - Range;

                    if (ShakeZ) {
                        coords.BottomLeft.Z += (CCRandom.Next () % doubleRange) - Range;
                        coords.BottomRight.Z += (CCRandom.Next () % doubleRange) - Range;
                        coords.TopLeft.Z += (CCRandom.Next () % doubleRange) - Range;
                        coords.TopRight.Z += (CCRandom.Next () % doubleRange) - Range;
                    }

                    SetTile (i, j, ref coords);
                }
            }
        }
    }

    #endregion Action state
}