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

namespace cocos2d
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
        public bool InitWithRange(int nRange, bool bShatterZ, CCGridSize gridSize, float duration)
        {
            if (base.InitWithSize(gridSize, duration))
            {
                m_bOnce = false;
                m_nRandrange = nRange;
                m_bShatterZ = bShatterZ;

                return true;
            }

            return false;
        }

        public override object CopyWithZone(CCZone pZone)
        {
            CCShatteredTiles3D pCopy;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pCopy = (CCShatteredTiles3D) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCShatteredTiles3D();
                pZone = new CCZone(pCopy);
            }

            //copy super class's member
            base.CopyWithZone(pZone);

            pCopy.InitWithRange(m_nRandrange, m_bShatterZ, m_sGridSize, m_fDuration);

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
                        coords.BottomLeft.X += (Random.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.BottomRight.X += (Random.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.TopLeft.X += (Random.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.TopRight.X += (Random.Next() % (m_nRandrange * 2)) - m_nRandrange;

                        // Y
                        coords.BottomLeft.Y += (Random.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.BottomRight.Y += (Random.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.TopLeft.Y += (Random.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.TopRight.Y += (Random.Next() % (m_nRandrange * 2)) - m_nRandrange;

                        if (m_bShatterZ)
                        {
                            coords.BottomLeft.Z += (Random.Next() % (m_nRandrange * 2)) - m_nRandrange;
                            coords.BottomRight.Z += (Random.Next() % (m_nRandrange * 2)) - m_nRandrange;
                            coords.TopLeft.Z += (Random.Next() % (m_nRandrange * 2)) - m_nRandrange;
                            coords.TopRight.Z += (Random.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        }

                        SetTile(new CCGridSize(i, j), ref coords);
                    }
                }

                m_bOnce = true;
            }
        }

        /// <summary>
        /// creates the action with a range, whether of not to shatter Z vertices, a grid size and duration
        /// </summary>
        public static CCShatteredTiles3D Create(int nRange, bool bShatterZ, CCGridSize gridSize, float duration)
        {
            var pAction = new CCShatteredTiles3D();
            pAction.InitWithRange(nRange, bShatterZ, gridSize, duration);
            return pAction;
        }
    }
}