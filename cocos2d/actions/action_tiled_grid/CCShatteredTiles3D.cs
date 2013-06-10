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

namespace Cocos2D
{
    /// <summary>
    /// @brief CCShatteredTiles3D action
    /// </summary>
    public class CCShatteredTiles3D : CCTiledGrid3DAction
    {
        protected bool m_bOnce;
        protected bool m_bShatterZ;
        protected int m_nRandrange;

        /// <summary>
        /// initializes the action with a range, whether or not to shatter Z vertices, a grid size and duration
        /// </summary>
        protected virtual bool InitWithDuration(float duration, CCGridSize gridSize, int nRange, bool bShatterZ)
        {
            if (base.InitWithDuration(duration, gridSize))
            {
                m_bOnce = false;
                m_nRandrange = nRange;
                m_bShatterZ = bShatterZ;

                return true;
            }

            return false;
        }

        public override object Copy(ICCCopyable pZone)
        {
            CCShatteredTiles3D pCopy;
            if (pZone != null)
            {
                pCopy = (CCShatteredTiles3D) (pZone);
            }
            else
            {
                pCopy = new CCShatteredTiles3D();
                pZone = (pCopy);
            }

            //copy super class's member
            base.Copy(pZone);

            pCopy.InitWithDuration(m_fDuration, m_sGridSize, m_nRandrange, m_bShatterZ);

            return pCopy;
        }

        public override void Update(float time)
        {
            int i, j;

            if (m_bOnce == false)
            {
                for (i = 0; i < m_sGridSize.X; ++i)
                {
                    for (j = 0; j < m_sGridSize.Y; ++j)
                    {
                        CCQuad3 coords = OriginalTile(new CCGridSize(i, j));

                        // X
                        coords.BottomLeft.X += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.BottomRight.X += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.TopLeft.X += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.TopRight.X += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;

                        // Y
                        coords.BottomLeft.Y += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.BottomRight.Y += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.TopLeft.Y += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.TopRight.Y += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;

                        if (m_bShatterZ)
                        {
                            coords.BottomLeft.Z += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                            coords.BottomRight.Z += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                            coords.TopLeft.Z += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                            coords.TopRight.Z += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        }

                        SetTile(new CCGridSize(i, j), ref coords);
                    }
                }

                m_bOnce = true;
            }
        }

        public CCShatteredTiles3D()
        {
        }

        /// <summary>
        /// creates the action with a range, whether of not to shatter Z vertices, a grid size and duration
        /// </summary>
        public CCShatteredTiles3D(float duration, CCGridSize gridSize, int nRange, bool bShatterZ) : base(duration)
        {
            InitWithDuration(duration, gridSize, nRange, bShatterZ);
        }
    }
}