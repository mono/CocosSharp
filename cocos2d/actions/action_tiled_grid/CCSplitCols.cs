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
    public class CCSplitCols : CCTiledGrid3DAction
    {
        protected int m_nCols;
        protected CCSize m_winSize;

        /// <summary>
        ///  initializes the action with the number of columns to split and the duration 
        /// </summary>
        public bool InitWithCols(int nCols, float duration)
        {
            m_nCols = nCols;
            return base.InitWithSize(new CCGridSize(nCols, 1), duration);
        }

        public override object Copy(ICopyable pZone)
        {
            CCSplitCols pCopy;
            if (pZone != null)
            {
                pCopy = (CCSplitCols) (pZone);
            }
            else
            {
                pCopy = new CCSplitCols();
                pZone =  (pCopy);
            }

            base.Copy(pZone);
            pCopy.InitWithCols(m_nCols, m_fDuration);

            return pCopy;
        }

        public override void Update(float time)
        {
            int i;

            for (i = 0; i < m_sGridSize.X; ++i)
            {
                CCQuad3 coords = OriginalTile(new CCGridSize(i, 0));
                float direction = 1;

                if ((i % 2) == 0)
                {
                    direction = -1;
                }

                coords.BottomLeft.Y += direction * m_winSize.Height * time;
                coords.BottomRight.Y += direction * m_winSize.Height * time;
                coords.TopLeft.Y += direction * m_winSize.Height * time;
                coords.TopRight.Y += direction * m_winSize.Height * time;

                SetTile(new CCGridSize(i, 0), ref coords);
            }
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_winSize = CCDirector.SharedDirector.WinSizeInPixels;
        }

        /// <summary>
        /// creates the action with the number of columns to split and the duration
        /// </summary>
        public static CCSplitCols Create(int nCols, float duration)
        {
            var pAction = new CCSplitCols();
            pAction.InitWithCols(nCols, duration);
            return pAction;
        }
    }
}