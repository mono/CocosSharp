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

namespace cocos2d
{
    /// <summary>
    /// @brief CCFadeOutUpTiles action.
    /// Fades out the tiles in upwards direction
    /// </summary>
    public class CCFadeOutUpTiles : CCFadeOutTRTiles
    {
        public override float TestFunc(CCGridSize pos, float time)
        {
            var n = new CCPoint((m_sGridSize.X * time), (m_sGridSize.Y * time));
            if (n.Y == 0.0f)
            {
                return 1.0f;
            }

            return (float) Math.Pow(pos.Y / n.Y, 6);
        }

        public override void TransformTile(CCGridSize pos, float distance)
        {
            CCQuad3 coords = OriginalTile(pos);
            CCPoint step = m_pTarget.Grid.Step;

            coords.BottomLeft.Y += (step.Y / 2) * (1.0f - distance);
            coords.BottomRight.Y += (step.Y / 2) * (1.0f - distance);
            coords.TopLeft.Y -= (step.Y / 2) * (1.0f - distance);
            coords.TopRight.Y -= (step.Y / 2) * (1.0f - distance);

            SetTile(pos, ref coords);
        }

        /// <summary>
        /// creates the action with the grid size and the duration 
        /// </summary>
        public new static CCFadeOutUpTiles Create(CCGridSize gridSize, float time)
        {
            var pAction = new CCFadeOutUpTiles();
            pAction.InitWithSize(gridSize, time);
            return pAction;
        }
    }
}