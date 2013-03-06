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
    /// @brief CCShuffleTiles action
    /// Shuffle the tiles in random order
    /// </summary>
    public class CCShuffleTiles : CCTiledGrid3DAction
    {
        protected int m_nSeed;
        protected int m_nTilesCount;
        protected Tile[] m_pTiles;
        protected int[] m_pTilesOrder;

        /// <summary>
        /// initializes the action with a random seed, the grid size and the duration
        /// </summary>
        public bool InitWithSeed(int s, ccGridSize gridSize, float duration)
        {
            if (base.InitWithSize(gridSize, duration))
            {
                m_nSeed = s;
                m_pTilesOrder = null;
                m_pTiles = null;

                return true;
            }

            return false;
        }

        public void Shuffle(ref int[] pArray, int nLen)
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

        public ccGridSize GetDelta(ccGridSize pos)
        {
            var pos2 = new CCPoint();

            int idx = pos.x * m_sGridSize.y + pos.y;

            pos2.x = (m_pTilesOrder[idx] / m_sGridSize.y);
            pos2.y = (m_pTilesOrder[idx] % m_sGridSize.y);

            return new ccGridSize((int) (pos2.x - pos.x), (int) (pos2.y - pos.y));
        }

        public void PlaceTile(ccGridSize pos, Tile t)
        {
            CCQuad3 coords = OriginalTile(pos);

            CCPoint step = m_pTarget.Grid.Step;
            coords.BottomLeft.X += (int) (t.Position.x * step.x);
            coords.BottomLeft.Y += (int) (t.Position.y * step.y);

            coords.BottomRight.X += (int) (t.Position.x * step.x);
            coords.BottomRight.Y += (int) (t.Position.y * step.y);

            coords.TopLeft.X += (int) (t.Position.x * step.x);
            coords.TopLeft.Y += (int) (t.Position.y * step.y);

            coords.TopRight.X += (int) (t.Position.x * step.x);
            coords.TopRight.Y += (int) (t.Position.y * step.y);

            SetTile(pos, ref coords);
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);

            if (m_nSeed != -1)
            {
                m_nSeed = Random.Next();
            }

            m_nTilesCount = m_sGridSize.x * m_sGridSize.y;
            m_pTilesOrder = new int[m_nTilesCount];
            int i, j;
            int k;

            /**
             * Use k to loop. Because m_nTilesCount is unsigned int,
             * and i is used later for int.
             */
            for (k = 0; k < m_nTilesCount; ++k)
            {
                m_pTilesOrder[k] = k;
            }

            Shuffle(ref m_pTilesOrder, m_nTilesCount);

            m_pTiles = new Tile[m_nTilesCount];

            int f = 0;
            for (i = 0; i < m_sGridSize.x; ++i)
            {
                for (j = 0; j < m_sGridSize.y; ++j)
                {
                    m_pTiles[f] = new Tile
                        {
                            Position = new CCPoint(i, j), 
                            StartPosition = new CCPoint(i, j), 
                            Delta = GetDelta(new ccGridSize(i, j))
                        };

                    f++;
                }
            }
        }

        public override void Update(float time)
        {
            int i, j;

            int f = 0;
            for (i = 0; i < m_sGridSize.x; ++i)
            {
                for (j = 0; j < m_sGridSize.y; ++j)
                {
                    Tile item = m_pTiles[f];
                    item.Position = new CCPoint((item.Delta.x * time), (item.Delta.y * time));
                    PlaceTile(new ccGridSize(i, j), item);

                    f++;
                }
            }
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCShuffleTiles pCopy;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                pCopy = (CCShuffleTiles) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCShuffleTiles();
                pZone = new CCZone(pCopy);
            }

            base.CopyWithZone(pZone);

            pCopy.InitWithSeed(m_nSeed, m_sGridSize, m_fDuration);

            return pCopy;
        }

        /// <summary>
        /// creates the action with a random seed, the grid size and the duration 
        /// </summary>
        public static CCShuffleTiles Create(int s, ccGridSize gridSize, float duration)
        {
            var pAction = new CCShuffleTiles();
            pAction.InitWithSeed(s, gridSize, duration);
            return pAction;
        }
    }
}