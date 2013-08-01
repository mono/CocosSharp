/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.
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
using Microsoft.Xna.Framework;

namespace Cocos2D
{
    /** CCLayerGradient is a subclass of CCLayerColor that draws gradients across
  the background.

  All features from CCLayerColor are valid, plus the following new features:
  - direction
  - final color
  - interpolation mode

  Color is interpolated between the startColor and endColor along the given
  vector (starting at the origin, ending at the terminus).  If no vector is
  supplied, it defaults to (0, -1) -- a fade from top to bottom.

  If 'compressedInterpolation' is disabled, you will not see either the start or end color for
  non-cardinal vectors; a smooth gradient implying both end points will be still
  be drawn, however.

  If ' compressedInterpolation' is enabled (default mode) you will see both the start and end colors of the gradient.

  @since v0.99.5
     */

    public class CCLayerGradient : CCLayerColor
    {
        private CCPoint m_AlongVector;

        /// <summary>
        /// Whether or not the interpolation will be compressed in order to display all the colors of the gradient both in canonical and non canonical vectors
        /// Default: YES
        /// </summary>
        private bool m_bCompressedInterpolation;

        private byte m_cEndOpacity;
        private byte m_cStartOpacity;

        private CCColor3B m_endColor;


        public CCLayerGradient ()
        {
        }

        public CCLayerGradient(byte startOpacity, byte endOpacity)
        {
            m_cStartOpacity = startOpacity;
            m_cEndOpacity = endOpacity;
        }
        
        /// <summary>
        /// Creates a full-screen CCLayer with a gradient between start and end.
        /// </summary>
        public CCLayerGradient (CCColor4B start, CCColor4B end)
        {
            InitWithColor(start, end);
        }
        
        /// <summary>
        /// Creates a full-screen CCLayer with a gradient between start and end in the direction of v. 
        /// </summary>
        public CCLayerGradient (CCColor4B start, CCColor4B end, CCPoint v)
        {
            InitWithColor(start, end, v);
        }

        public CCColor3B StartColor
        {
            get { return _realColor; }
            set
            {
                base.Color = value;
                UpdateColor();
            }
        }

        public CCColor3B EndColor
        {
            get { return m_endColor; }
            set
            {
                m_endColor = value;
                UpdateColor();
            }
        }

        public byte StartOpacity
        {
            get { return m_cStartOpacity; }
            set
            {
                m_cStartOpacity = value;
                UpdateColor();
            }
        }

        public byte EndOpacity
        {
            get { return m_cEndOpacity; }
            set
            {
                m_cEndOpacity = value;
                UpdateColor();
            }
        }

        public CCPoint Vector
        {
            get { return m_AlongVector; }
            set
            {
                m_AlongVector = value;
                UpdateColor();
            }
        }


        public bool IsCompressedInterpolation
        {
            get { return m_bCompressedInterpolation; }
            set
            {
                m_bCompressedInterpolation = value;
                UpdateColor();
            }
        }

        public override bool Init()
        {
            return InitWithColor(new CCColor4B(0, 0, 0, 255), new CCColor4B(0, 0, 0, 255));
        }

        /// <summary>
        /// Initializes the CCLayer with a gradient between start and end.
        /// </summary>
        protected virtual bool InitWithColor(CCColor4B start, CCColor4B end)
        {
            return InitWithColor(start, end, new CCPoint(0, -1));
        }

        /// <summary>
        /// Initializes the CCLayer with a gradient between start and end in the direction of v.
        /// </summary>
        protected virtual bool InitWithColor(CCColor4B start, CCColor4B end, CCPoint v)
        {
            m_endColor = new CCColor3B();
            m_endColor.R = end.R;
            m_endColor.G = end.G;
            m_endColor.B = end.B;

            m_cEndOpacity = end.A;
            m_cStartOpacity = start.A;
            m_AlongVector = v;

            m_bCompressedInterpolation = true;

            return base.InitWithColor(new CCColor4B(start.R, start.G, start.B, 255));
        }


        protected override void UpdateColor()
        {
            base.UpdateColor();

            float h = m_AlongVector.Length;
            if (h == 0)
                return;

            double c = Math.Sqrt(2.0);
            var u = new CCPoint(m_AlongVector.X / h, m_AlongVector.Y / h);

            // Compressed Interpolation mode
            if (m_bCompressedInterpolation)
            {
                float h2 = 1 / (Math.Abs(u.X) + Math.Abs(u.Y));
                u = u * (h2 * (float) c);
            }

            float opacityf = _displayedOpacity / 255.0f;

            var S = new CCColor4B
                {
                    R = _displayedColor.R,
                    G = _displayedColor.G,
                    B = _displayedColor.B,
                    A = (byte) (m_cStartOpacity * opacityf)
                };

            var E = new CCColor4B
                {
                    R = m_endColor.R,
                    G = m_endColor.G,
                    B = m_endColor.B,
                    A = (byte) (m_cEndOpacity * opacityf)
                };

            // (-1, -1)
            m_pSquareVertices[0].Color = new Color(
                (byte) (E.R + (S.R - E.R) * ((c + u.X + u.Y) / (2.0f * c))),
                (byte) (E.G + (S.G - E.G) * ((c + u.X + u.Y) / (2.0f * c))),
                (byte) (E.B + (S.B - E.B) * ((c + u.X + u.Y) / (2.0f * c))),
                (byte) (E.A + (S.A - E.A) * ((c + u.X + u.Y) / (2.0f * c)))
                );

            // (1, -1)
            m_pSquareVertices[1].Color = new Color(
                (byte) (E.R + (S.R - E.R) * ((c - u.X + u.Y) / (2.0f * c))),
                (byte) (E.G + (S.G - E.G) * ((c - u.X + u.Y) / (2.0f * c))),
                (byte) (E.B + (S.B - E.B) * ((c - u.X + u.Y) / (2.0f * c))),
                (byte) (E.A + (S.A - E.A) * ((c - u.X + u.Y) / (2.0f * c)))
                );

            // (-1, 1)
            m_pSquareVertices[2].Color = new Color(
                (byte) (E.R + (S.R - E.R) * ((c + u.X - u.Y) / (2.0f * c))),
                (byte) (E.G + (S.G - E.G) * ((c + u.X - u.Y) / (2.0f * c))),
                (byte) (E.B + (S.B - E.B) * ((c + u.X - u.Y) / (2.0f * c))),
                (byte) (E.A + (S.A - E.A) * ((c + u.X - u.Y) / (2.0f * c)))
                );

            // (1, 1)
            m_pSquareVertices[3].Color = new Color(
                (byte) (E.R + (S.R - E.R) * ((c - u.X - u.Y) / (2.0f * c))),
                (byte) (E.G + (S.G - E.G) * ((c - u.X - u.Y) / (2.0f * c))),
                (byte) (E.B + (S.B - E.B) * ((c - u.X - u.Y) / (2.0f * c))),
                (byte) (E.A + (S.A - E.A) * ((c - u.X - u.Y) / (2.0f * c)))
                );
        }
    }
}