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
        protected virtual bool InitWithDuration(float duration, CCGridSize gridSize, int seed)
        {
            if (base.InitWithDuration(duration, gridSize))
            {
                m_nSeed = seed;
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
                int j = CCRandom.Next() % (i + 1);
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

        public override object Copy(ICCCopyable pZone)
        {
            CCTurnOffTiles pCopy;
            if (pZone != null)
            {
                pCopy = (CCTurnOffTiles) (pZone);
            }
            else
            {
                pCopy = new CCTurnOffTiles();
                pZone = (pCopy);
            }

            base.Copy(pZone);

            pCopy.InitWithDuration(m_fDuration, m_sGridSize, m_nSeed);

            return pCopy;
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            int i;

            base.StartWithTarget(target);

            if (m_nSeed != -1)
            {
                CCRandom.Next(m_nSeed);
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
        public CCTurnOffTiles(float duration, CCGridSize gridSize) : base(duration)
        {
            InitWithDuration(duration, gridSize);
        }

        public CCTurnOffTiles()
        {
        }

        /// <summary>
        /// creates the action with a random seed, the grid size and the duration 
        /// </summary>
        public CCTurnOffTiles(float duration, CCGridSize gridSize, int seed) : base(duration)
        {
            InitWithDuration(duration, gridSize, seed);
        }
    }
}