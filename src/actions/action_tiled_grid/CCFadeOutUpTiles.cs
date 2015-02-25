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
    /// <summary>
    /// @brief CCFadeOutUpTiles action.
    /// Fades out the tiles in upwards direction
    /// </summary>
    public class CCFadeOutUpTiles : CCFadeOutTRTiles
    {
        #region Constructors

        public CCFadeOutUpTiles (float duration, CCGridSize gridSize) : base (duration, gridSize)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCFadeOutUpTilesState (this, target);
        }
    }


    #region Action state

    public class CCFadeOutUpTilesState : CCFadeOutTRTilesState
    {
        public CCFadeOutUpTilesState (CCFadeOutUpTiles action, CCNode target) : base (action, target)
        {
        }


        #region Tile transform

        public override float TestFunc (CCGridSize pos, float time)
        {
            float fy = GridSize.Y * time;
            if (fy == 0f)
            {
                return (1f);
            }
            return (float)Math.Pow (pos.Y / fy, 6);
        }

        public override void TransformTile (CCGridSize pos, float distance)
        {
            CCQuad3 coords = OriginalTile (pos);

            var step = (Target is CCNodeGrid) ?  ((CCNodeGrid)Target).Grid.Step : Target.Grid.Step;

            float dy = (step.Y / 2) * (1.0f - distance);

            coords.BottomLeft.Y += dy; // (step.Y / 2) * (1.0f - distance);
            coords.BottomRight.Y += dy; //  (step.Y / 2) * (1.0f - distance);
            coords.TopLeft.Y -= dy; //  (step.Y / 2) * (1.0f - distance);
            coords.TopRight.Y -= dy; // (step.Y / 2) * (1.0f - distance);

            SetTile (pos, ref coords);
        }

        #endregion Tile transform
    }

    #endregion Action state
}