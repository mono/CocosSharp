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
    /// @brief CCTurnOffTiles action.
    /// Turn off the files in random order
    /// </summary>
    public class CCTurnOffTiles : CCTiledGrid3DAction
    {
        private CCQuad3 m_pZero;
        protected int m_nSeed;
        protected int m_nTilesCount;
        protected int[] m_pTilesOrder;

        /// <summary>
        /// initializes the action with a random seed, the grid size and the duration 
        /// </summary>
        public bool InitWithSeed(int s, CCGridSize gridSize, float duration)
        {
            if (base.InitWithSize(gridSize, duration))
            {
                m_nSeed = s;
                m_pTilesOrder = null;

                return true;
            }

            return false;
        }

        public void Shuffle(int[] pArray, int nLen)
        {
            int i;
            for (i = nLen - 1; i >= 0; i--)
            {
                int j = Random.Next() % (i + 1);
                int v = pArray[i];
                pArray[i] = pArray[j];
                pArray[j] = v;
            }
        }

        public void TurnOnTile(CCGridSize pos)
        {
            CCQuad3 orig = OriginalTile(pos);
            SetTile(pos, ref orig);
        }

        public void TurnOffTile(CCGridSize pos)
        {
            SetTile(pos, ref m_pZero);
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCTurnOffTiles pCopy;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pCopy = (CCTurnOffTiles) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCTurnOffTiles();
                pZone = new CCZone(pCopy);
            }

            base.CopyWithZone(pZone);

            pCopy.InitWithSeed(m_nSeed, m_sGridSize, m_fDuration);

            return pCopy;
        }

        public override void StartWithTarget(CCNode target)
        {
            int i;

            base.StartWithTarget(target);

            if (m_nSeed != -1)
            {
                Random.Next(m_nSeed);
            }

            m_nTilesCount = m_sGridSize.X * m_sGridSize.Y;
            m_pTilesOrder = new int[m_nTilesCount];

            for (i = 0; i < m_nTilesCount; ++i)
            {
                m_pTilesOrder[i] = i;
            }

            Shuffle(m_pTilesOrder, m_nTilesCount);
        }

        public override void Update(float time)
        {
            int i, l, t;

            l = (int) (time * m_nTilesCount);

            for (i = 0; i < m_nTilesCount; i++)
            {
                t = m_pTilesOrder[i];
                var tilePos = new CCGridSize(t / m_sGridSize.Y, t % m_sGridSize.Y);

                if (i < l)
                {
                    TurnOffTile(tilePos);
                }
                else
                {
                    TurnOnTile(tilePos);
                }
            }
        }

        /// <summary>
        /// creates the action with the grid size and the duration
        /// </summary>
        public new static CCTurnOffTiles Create(CCGridSize size, float d)
        {
            var pAction = new CCTurnOffTiles();
            pAction.InitWithSize(size, d);
            return pAction;
        }

        /// <summary>
        /// creates the action with a random seed, the grid size and the duration 
        /// </summary>
        public static CCTurnOffTiles Create(int s, CCGridSize gridSize, float duration)
        {
            var pAction = new CCTurnOffTiles();
            pAction.InitWithSeed(s, gridSize, duration);
            return pAction;
        }
    }
}