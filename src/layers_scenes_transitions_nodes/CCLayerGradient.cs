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

namespace CocosSharp
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
        // Whether or not the interpolation will be compressed in order to display all the colors of the gradient both in canonical and non canonical vectors
        bool compressedInterpolation; 

        byte endOpacity;
        byte startOpacity;

        CCPoint alongVector;
        CCColor3B endColor;


        #region Properties

        public CCColor3B StartColor
        {
            get { return RealColor; }
            set
            {
                base.Color = value;
                UpdateColor();
            }
        }

        public CCColor3B EndColor
        {
            get { return endColor; }
            set
            {
                endColor = value;
                UpdateColor();
            }
        }

        public byte StartOpacity
        {
            get { return startOpacity; }
            set
            {
                startOpacity = value;
                UpdateColor();
            }
        }

        public byte EndOpacity
        {
            get { return endOpacity; }
            set
            {
                endOpacity = value;
                UpdateColor();
            }
        }

        public CCPoint Vector
        {
            get { return alongVector; }
            set
            {
                alongVector = value;
                UpdateColor();
            }
        }


        public bool IsCompressedInterpolation
        {
            get { return compressedInterpolation; }
            set
            {
                compressedInterpolation = value;
                UpdateColor();
            }
        }

        #endregion Properties


        #region Constructors

        /// <summary>
        /// Creates a full-screen CCLayer with a gradient between start and end.
        /// </summary>
        public CCLayerGradient (CCColor4B start, CCColor4B end) : this(start, end, new CCPoint(0, -1))
        {
        }

        public CCLayerGradient() : this(new CCColor4B(0, 0, 0, 255), new CCColor4B(0, 0, 0, 255))
        {
        }

        public CCLayerGradient(byte startOpacity, byte endOpacity) : base()
        {
            StartOpacity = startOpacity;
            EndOpacity = endOpacity;
        }

        /// <summary>
        /// Creates a full-screen CCLayer with a gradient between start and end in the direction of v. 
        /// </summary>
        public CCLayerGradient (CCColor4B start, CCColor4B end, CCPoint gradientDirection) 
            : base(new CCColor4B(start.R, start.G, start.B, 255))
        {
            EndColor = new CCColor3B(end.R, end.G, end.B);
            StartOpacity = start.A;
            EndOpacity = end.A;
            IsCompressedInterpolation = true;

            alongVector = gradientDirection;
        }

        #endregion Constructors


        public override void UpdateColor()
        {
            base.UpdateColor();

            float h = alongVector.Length;
            if (h == 0)
                return;

            double c = Math.Sqrt(2.0);
            var u = new CCPoint(alongVector.X / h, alongVector.Y / h);

            // Compressed Interpolation mode
            if (IsCompressedInterpolation)
            {
                float h2 = 1 / (Math.Abs(u.X) + Math.Abs(u.Y));
                u = u * (h2 * (float) c);
            }

            float opacityf = DisplayedOpacity / 255.0f;

            var S = new CCColor4B
                {
                    R = DisplayedColor.R,
                    G = DisplayedColor.G,
                    B = DisplayedColor.B,
                    A = (byte) (StartOpacity * opacityf)
                };

            var E = new CCColor4B
                {
                    R = EndColor.R,
                    G = EndColor.G,
                    B = EndColor.B,
                    A = (byte) (EndOpacity * opacityf)
                };

            // (-1, -1)
            SquareVertices[0].Color = new Color(
                (byte) (E.R + (S.R - E.R) * ((c + u.X + u.Y) / (2.0f * c))),
                (byte) (E.G + (S.G - E.G) * ((c + u.X + u.Y) / (2.0f * c))),
                (byte) (E.B + (S.B - E.B) * ((c + u.X + u.Y) / (2.0f * c))),
                (byte) (E.A + (S.A - E.A) * ((c + u.X + u.Y) / (2.0f * c)))
                );

            // (1, -1)
            SquareVertices[1].Color = new Color(
                (byte) (E.R + (S.R - E.R) * ((c - u.X + u.Y) / (2.0f * c))),
                (byte) (E.G + (S.G - E.G) * ((c - u.X + u.Y) / (2.0f * c))),
                (byte) (E.B + (S.B - E.B) * ((c - u.X + u.Y) / (2.0f * c))),
                (byte) (E.A + (S.A - E.A) * ((c - u.X + u.Y) / (2.0f * c)))
                );

            // (-1, 1)
            SquareVertices[2].Color = new Color(
                (byte) (E.R + (S.R - E.R) * ((c + u.X - u.Y) / (2.0f * c))),
                (byte) (E.G + (S.G - E.G) * ((c + u.X - u.Y) / (2.0f * c))),
                (byte) (E.B + (S.B - E.B) * ((c + u.X - u.Y) / (2.0f * c))),
                (byte) (E.A + (S.A - E.A) * ((c + u.X - u.Y) / (2.0f * c)))
                );

            // (1, 1)
            SquareVertices[3].Color = new Color(
                (byte) (E.R + (S.R - E.R) * ((c - u.X - u.Y) / (2.0f * c))),
                (byte) (E.G + (S.G - E.G) * ((c - u.X - u.Y) / (2.0f * c))),
                (byte) (E.B + (S.B - E.B) * ((c - u.X - u.Y) / (2.0f * c))),
                (byte) (E.A + (S.A - E.A) * ((c - u.X - u.Y) / (2.0f * c)))
                );
        }
    }
}