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
    /// @brief CCShatteredTiles3D action
    /// </summary>
    public class CCShatteredTiles3D : CCTiledGrid3DAction
    {
        protected internal bool ShatterZ { get; private set; }
        protected internal int Range { get; private set; }


        #region Constructors

        public CCShatteredTiles3D()
        {
        }

        /// <summary>
        /// creates the action with a range, whether of not to shatter Z vertices, a grid size and duration
        /// </summary>
		public CCShatteredTiles3D(float duration, CCGridSize gridSize, int nRange = 0, bool bShatterZ = true) : base(duration, gridSize)
        {
            Range = nRange;
            ShatterZ = bShatterZ;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCShatteredTiles3DState(this, target);
        }
    }


    #region Action state

    public class CCShatteredTiles3DState : CCTiledGrid3DActionState
    {
        protected bool CachedShatterZ { get; private set; }
        protected int CachedRange { get; private set; }
        protected bool ShatterOnce { get; private set; }

        public CCShatteredTiles3DState(CCShatteredTiles3D action, CCNode target) : base(action, target)
        {
            CachedRange = action.Range;
            CachedShatterZ = action.ShatterZ;
        }

        public override void Update(float time)
        {
            int i, j;
            var doubleRange = CachedRange * 2;

            if (ShatterOnce == false)
            {
                for (i = 0; i < CachedGridSize.X; ++i)
                {
                    for (j = 0; j < CachedGridSize.Y; ++j)
                    {
                        CCQuad3 coords = OriginalTile(i, j);

                        // X
                        coords.BottomLeft.X += (CCRandom.Next() % doubleRange) - CachedRange;
                        coords.BottomRight.X += (CCRandom.Next() % doubleRange) - CachedRange;
                        coords.TopLeft.X += (CCRandom.Next() % doubleRange) - CachedRange;
                        coords.TopRight.X += (CCRandom.Next() % doubleRange) - CachedRange;

                        // Y
                        coords.BottomLeft.Y += (CCRandom.Next() % doubleRange) - CachedRange;
                        coords.BottomRight.Y += (CCRandom.Next() % doubleRange) - CachedRange;
                        coords.TopLeft.Y += (CCRandom.Next() % doubleRange) - CachedRange;
                        coords.TopRight.Y += (CCRandom.Next() % doubleRange) - CachedRange;

                        if (CachedShatterZ)
                        {
                            coords.BottomLeft.Z += (CCRandom.Next() % doubleRange) - CachedRange;
                            coords.BottomRight.Z += (CCRandom.Next() % doubleRange) - CachedRange;
                            coords.TopLeft.Z += (CCRandom.Next() % doubleRange) - CachedRange;
                            coords.TopRight.Z += (CCRandom.Next() % doubleRange) - CachedRange;
                        }

                        SetTile(i, j, ref coords);
                    }
                }

                ShatterOnce = true;
            }
        }
    }

    #endregion Action state
}