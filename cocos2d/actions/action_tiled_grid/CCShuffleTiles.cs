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
    /// @brief CCShuffleTiles action
    /// Shuffle the tiles in random order
    /// </summary>
    public class CCShuffleTiles : CCTiledGrid3DAction
    {
        protected int m_nSeed;
        protected int m_nTilesCount;
        protected CCTile[] m_pTiles;
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
                int j = CCRandom.Next() % (i + 1);
                int v = pArray[i];
                pArray[i] = pArray[j];
                pArray[j] = v;
            }
        }

        public CCGridSize GetDelta(CCGridSize pos)
        {
            var pos2 = new CCPoint();

            int idx = pos.X * m_sGridSize.Y + pos.Y;

            pos2.X = (m_pTilesOrder[idx] / m_sGridSize.Y);
            pos2.Y = (m_pTilesOrder[idx] % m_sGridSize.Y);

            return new CCGridSize((int) (pos2.X - pos.X), (int) (pos2.Y - pos.Y));
        }

        public void PlaceTile(CCGridSize pos, CCTile t)
        {
            CCQuad3 coords = OriginalTile(pos);

            CCPoint step = m_pTarget.Grid.Step;
            coords.BottomLeft.X += (int) (t.Position.X * step.X);
            coords.BottomLeft.Y += (int) (t.Position.Y * step.Y);

            coords.BottomRight.X += (int) (t.Position.X * step.X);
            coords.BottomRight.Y += (int) (t.Position.Y * step.Y);

            coords.TopLeft.X += (int) (t.Position.X * step.X);
            coords.TopLeft.Y += (int) (t.Position.Y * step.Y);

            coords.TopRight.X += (int) (t.Position.X * step.X);
            coords.TopRight.Y += (int) (t.Position.Y * step.Y);

            SetTile(pos, ref coords);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);

            if (m_nSeed != -1)
            {
                m_nSeed = CCRandom.Next();
            }

            m_nTilesCount = m_sGridSize.X * m_sGridSize.Y;
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

            m_pTiles = new CCTile[m_nTilesCount];

            int f = 0;
            for (i = 0; i < m_sGridSize.X; ++i)
            {
                for (j = 0; j < m_sGridSize.Y; ++j)
                {
                    m_pTiles[f] = new CCTile
                        {
                            Position = new CCPoint(i, j),
                            StartPosition = new CCPoint(i, j),
                            Delta = GetDelta(new CCGridSize(i, j))
                        };

                    f++;
                }
            }
        }

        public override void Update(float time)
        {
            int i, j;

            int f = 0;
            for (i = 0; i < m_sGridSize.X; ++i)
            {
                for (j = 0; j < m_sGridSize.Y; ++j)
                {
                    CCTile item = m_pTiles[f];
                    item.Position = new CCPoint((item.Delta.X * time), (item.Delta.Y * time));
                    PlaceTile(new CCGridSize(i, j), item);

                    f++;
                }
            }
        }

        public override object Copy(ICCCopyable pZone)
        {
            CCShuffleTiles pCopy;
            if (pZone != null)
            {
                pCopy = (CCShuffleTiles) (pZone);
            }
            else
            {
                pCopy = new CCShuffleTiles();
                pZone = (pCopy);
            }

            base.Copy(pZone);

            pCopy.InitWithDuration(m_fDuration, m_sGridSize, m_nSeed);

            return pCopy;
        }

        public CCShuffleTiles()
        {
        }

        /// <summary>
        /// creates the action with a random seed, the grid size and the duration 
        /// </summary>
        public CCShuffleTiles(CCGridSize gridSize, float duration, int seed) : base(duration)
        {
            InitWithDuration(duration, gridSize, seed);
        }
    }
}