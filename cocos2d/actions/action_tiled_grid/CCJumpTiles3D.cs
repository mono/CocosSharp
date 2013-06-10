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
    public class CCJumpTiles3D : CCTiledGrid3DAction
    {
        protected float m_fAmplitude;
        protected float m_fAmplitudeRate;
        protected int m_nJumps;

        /// <summary>
        /// amplitude of the sin
        /// </summary>
        public float Amplitude
        {
            get { return m_fAmplitude; }
            set { m_fAmplitude = value; }
        }

        /// <summary>
        ///  amplitude rate 
        /// </summary>
        public override float AmplitudeRate
        {
            get { return m_fAmplitudeRate; }
            set { m_fAmplitudeRate = value; }
        }

        /// <summary>
        /// initializes the action with the number of jumps, the sin amplitude, the grid size and the duration 
        /// </summary>
        protected virtual bool InitWithDuration(float duration, CCGridSize gridSize, int numberOfJumps, float amplitude)
        {
            if (base.InitWithDuration(duration, gridSize))
            {
                m_nJumps = numberOfJumps;
                m_fAmplitude = amplitude;
                m_fAmplitudeRate = 1.0f;

                return true;
            }

            return false;
        }

        public override object Copy(ICCCopyable pZone)
        {
            CCJumpTiles3D pCopy;
            if (pZone != null)
            {
                pCopy = (CCJumpTiles3D) (pZone);
            }
            else
            {
                pCopy = new CCJumpTiles3D();
                pZone = (pCopy);
            }

            base.Copy(pZone);

            pCopy.InitWithDuration(m_fDuration, m_sGridSize, m_nJumps, m_fAmplitude);

            return pCopy;
        }

        public override void Update(float time)
        {
            int i, j;

            float sinz = ((float) Math.Sin((float) Math.PI * time * m_nJumps * 2) * m_fAmplitude * m_fAmplitudeRate);
            var sinz2 =
                (float) (Math.Sin((float) Math.PI * (time * m_nJumps * 2 + 1)) * m_fAmplitude * m_fAmplitudeRate);

            for (i = 0; i < m_sGridSize.X; i++)
            {
                for (j = 0; j < m_sGridSize.Y; j++)
                {
                    CCQuad3 coords = OriginalTile(new CCGridSize(i, j));

                    if (((i + j) % 2) == 0)
                    {
                        coords.BottomLeft.Z += sinz;
                        coords.BottomRight.Z += sinz;
                        coords.TopLeft.Z += sinz;
                        coords.TopRight.Z += sinz;
                    }
                    else
                    {
                        coords.BottomLeft.Z += sinz2;
                        coords.BottomRight.Z += sinz2;
                        coords.TopLeft.Z += sinz2;
                        coords.TopRight.Z += sinz2;
                    }

                    SetTile(new CCGridSize(i, j), ref coords);
                }
            }
        }

        public CCJumpTiles3D()
        {
        }

        public CCJumpTiles3D(float duration, CCGridSize gridSize, int numberOfJumps, float amplitude) : base(duration)
        {
            InitWithDuration(duration, gridSize, numberOfJumps, amplitude);
        }
    }
}