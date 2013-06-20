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
    public class CCSplitRows : CCTiledGrid3DAction
    {
        protected int m_nRows;
        protected CCSize m_winSize;

        /// <summary>
        /// initializes the action with the number of rows to split and the duration
        /// </summary>
        protected virtual bool InitWithDuration(float duration, int nRows)
        {
            m_nRows = nRows;

            return base.InitWithDuration(duration, new CCGridSize(1, nRows));
        }

        public override object Copy(ICCCopyable pZone)
        {
            CCSplitRows pCopy;
            if (pZone != null)
            {
                pCopy = (CCSplitRows) (pZone);
            }
            else
            {
                pCopy = new CCSplitRows();
                pZone = (pCopy);
            }

            base.Copy(pZone);

            pCopy.InitWithDuration(m_fDuration, m_nRows);

            return pCopy;
        }

        public override void Update(float time)
        {
            int j;

            for (j = 0; j < m_sGridSize.Y; ++j)
            {
                CCQuad3 coords = OriginalTile(new CCGridSize(0, j));
                float direction = 1;

                if ((j % 2) == 0)
                {
                    direction = -1;
                }

                coords.BottomLeft.X += direction * m_winSize.Width * time;
                coords.BottomRight.X += direction * m_winSize.Width * time;
                coords.TopLeft.X += direction * m_winSize.Width * time;
                coords.TopRight.X += direction * m_winSize.Width * time;

                SetTile(new CCGridSize(0, j), ref coords);
            }
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_winSize = CCDirector.SharedDirector.WinSizeInPixels;
        }

        public CCSplitRows()
        {
        }

        /// <summary>
        ///  creates the action with the number of rows to split and the duration 
        /// </summary>
        public CCSplitRows(float duration, int nRows) : base(duration)
        {
            InitWithDuration(duration, nRows);
        }
    }
}