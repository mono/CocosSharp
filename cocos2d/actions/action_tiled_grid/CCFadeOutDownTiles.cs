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

namespace Cocos2D
{
    /// <summary>
    /// @brief CCFadeOutDownTiles action.
    /// Fades out the tiles in downwards direction
    /// </summary>
    public class CCFadeOutDownTiles : CCFadeOutUpTiles
    {
        public override float TestFunc(CCGridSize pos, float time)
        {
            var n = new CCPoint((m_sGridSize.X * (1.0f - time)), (m_sGridSize.Y * (1.0f - time)));
            if (pos.Y == 0)
            {
                return 1.0f;
            }

            return (float) Math.Pow(n.Y / pos.Y, 6);
        }

        public CCFadeOutDownTiles()
        {
        }

        /// <summary>
        ///  creates the action with the grid size and the duration 
        /// </summary>
        public CCFadeOutDownTiles(float duration, CCGridSize gridSize) : base(duration, gridSize)
        {
            InitWithDuration(duration, gridSize);
        }
    }
}