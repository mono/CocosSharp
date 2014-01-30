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
    public class CCShakyTiles3D : CCTiledGrid3DAction
    {
        protected bool m_bShakeZ;
        protected int m_nRandrange;

		protected bool ShakeZ
		{
			get { return m_bShakeZ; }
			set { m_bShakeZ = value; }
		}

		protected int Range
		{
			get { return m_nRandrange; }
			set { m_nRandrange = value; }
		}

        #region Constructors

        public CCShakyTiles3D()
        {
        }

        /// <summary>
        /// creates the action with a range, whether or not to shake Z vertices, a grid size, and duration
        /// </summary>
		public CCShakyTiles3D(float duration, CCGridSize gridSize, int nRange = 0, bool bShakeZ = true) : base(duration, gridSize)
        {
            InitCCShakyTiles3D(nRange, bShakeZ);
        }

        // Perform deep copy of CCShakyTiles3D
        public CCShakyTiles3D(CCShakyTiles3D shakyTiles) : base(shakyTiles)
        {
            InitCCShakyTiles3D(shakyTiles.m_nRandrange, shakyTiles.m_bShakeZ);
        }

        private void InitCCShakyTiles3D(int nRange, bool bShakeZ)
        {
            m_nRandrange = nRange;
            m_bShakeZ = bShakeZ;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable pZone)
        {
            return new CCShakyTiles3D(this);
        }

        public override void Update(float time)
        {
            int i, j;

            for (i = 0; i < m_sGridSize.X; ++i)
            {
                for (j = 0; j < m_sGridSize.Y; ++j)
                {
                    CCQuad3 coords = OriginalTile(i, j);
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

                    if (m_bShakeZ)
                    {
                        coords.BottomLeft.Z += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.BottomRight.Z += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.TopLeft.Z += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                        coords.TopRight.Z += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    }

                    SetTile(i, j, ref coords);
                }
            }
        }
    }
}