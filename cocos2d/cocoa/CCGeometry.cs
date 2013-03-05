/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org


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
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    public struct CCPoint
    {
        public static readonly CCPoint Zero = new CCPoint(0, 0);

        public float x;
        public float y;

        public CCPoint(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public CCPoint(CCPoint pt)
        {
            this.x = pt.x;
            this.y = pt.y;
        }

        public CCPoint(Point pt)
        {
            x = pt.X;
            y = pt.Y;
        }

        public CCPoint(Vector2 v)
        {
            x = v.X;
            y = v.Y;
        }

        public static bool CCPointEqualToPoint(CCPoint point1, CCPoint point2)
        {
            return ((point1.x == point2.x) && (point1.y == point2.y));
        }

        public CCPoint Offset(float dx, float dy)
        {
            return (new CCPoint(x + dx, y + dy));
        }
        public CCPoint Reverse
        {
            get
            {
                return (new CCPoint(-x, -y));
            }
        }

        public override bool Equals(object obj)
        {
            return (Equals((CCPoint)obj));
        }
        public bool Equals(CCPoint p)
        {
            return x == p.x && y == p.y;
        }

        public override string ToString()
        {
            return String.Format("CCPoint : (x={0}, y={1})", x, y);
        }

        public float DistanceSQ(ref CCPoint v2)
        {
            return Sub(ref v2).LengthSQ;
        }

        public CCPoint Sub(ref CCPoint v2)
        {
            return new CCPoint(x - v2.x, y - v2.y);
        }

        public float LengthSQ
        {
            get { return x * x + y * y; }
        }

        public float Length
        {
            get { return (float)Math.Sqrt(x * x + y * y); }
        }

        /// <summary>
        /// Inverts the direction or location of the Y component.
        /// </summary>
        public CCPoint InvertY
        {
            get
            {
                return (new CCPoint(x, -y));
            }
        }

        public static CCPoint Perp(CCPoint p)
        {
            return new CCPoint(-p.y, p.x);
        }

        public static float Dot(CCPoint p1, CCPoint p2)
        {
            return p1.x * p2.x + p1.y * p2.y;
        }

        public void Normalize()
        {
            var l = 1f / (float)Math.Sqrt(x * x + y * y);
            x *= l;
            y *= l;
        }

        public static CCPoint Normalize(CCPoint p)
        {
            var x = p.x;
            var y = p.y;
            var l = 1f / (float)Math.Sqrt(x * x + y * y);
            return new CCPoint(x * l, y * l);
        }

        public static CCPoint Midpoint(CCPoint p1, CCPoint p2)
        {
            return (p1 + p2) * 0.5f;
        }

        public static bool operator ==(CCPoint p1, CCPoint p2)
        {
            return (p1.Equals(p2));
        }
        public static bool operator !=(CCPoint p1, CCPoint p2)
        {
            return (!p1.Equals(p2));
        }
        public static CCPoint operator -(CCPoint p1, CCPoint p2)
        {
            return new CCPoint(p1.x - p2.x, p1.y - p2.y);
        }

        public static CCPoint operator +(CCPoint p1, CCPoint p2)
        {
            return new CCPoint(p1.x + p2.x, p1.y + p2.y);
        }

        public static CCPoint operator *(CCPoint p, float value)
        {
            return new CCPoint(p.x * value, p.y * value);
        }
    }

    public struct CCSize
    {
        public static readonly CCSize Zero = new CCSize(0, 0);
 
        public float Width;
        public float Height;

        public CCSize(float width, float height)
        {
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Returns the inversion of this size, which is the height and width swapped.
        /// </summary>
        public CCSize Inverted
        {
            get
            {
                CCSize c = new CCSize(Height, Width);
                return (c);
            }
        }
        public static bool CCSizeEqualToSize(CCSize size1, CCSize size2)
        {
            return ((size1.Width == size2.Width) && (size1.Height == size2.Height));
        }

        public bool Equals(CCSize s)
        {
            return Width == s.Width && Height == s.Height;
        }

        public override bool Equals(object obj)
        {
            return (Equals((CCSize)obj));
        }

        public CCPoint Center
        {
            get { return new CCPoint(Width / 2f, Height / 2f); }
        }

        public override string ToString()
        {
            return String.Format("{0} x {1}", Width, Height);
        }
        public static bool operator ==(CCSize p1, CCSize p2)
        {
            return (p1.Equals(p2));
        }
        public static bool operator !=(CCSize p1, CCSize p2)
        {
            return (!p1.Equals(p2));
        }
        public static CCSize operator *(CCSize p, float f)
        {
            return(new CCSize(p.Width*f, p.Height*f));
        }
        public static CCSize operator /(CCSize p, float f)
        {
            return (new CCSize(p.Width / f, p.Height / f));
        }
        public static CCSize operator +(CCSize p, float f)
        {
            return (new CCSize(p.Width + f, p.Height + f));
        }
        public static CCSize operator -(CCSize p, float f)
        {
            return (new CCSize(p.Width - f, p.Height - f));
        }
    }

    public struct CCRect
    {
        public static readonly CCRect Zero = new CCRect(0, 0, 0, 0);

        public CCPoint origin;
        public CCSize size;

        /// <summary>
        /// Creates the rectangle at (x,y) -> (width,height)
        /// </summary>
        /// <param name="x">Lower Left corner X</param>
        /// <param name="y">Lower left corner Y</param>
        /// <param name="width">width of the rectangle</param>
        /// <param name="height">height of the rectangle</param>
        public CCRect(float x, float y, float width, float height)
        {
            origin = new CCPoint();
            size = new CCSize();

            // Only support that, the width and height > 0
            Debug.Assert(width >= 0 && height >= 0);

            origin.x = x;
            origin.y = y;

            size.Width = width;
            size.Height = height;
        }

        /// <summary>
        /// Returns the inversion of this rect's size, which is the height and width swapped, while the origin stays unchanged.
        /// </summary>
        public CCRect InvertedSize
        {
            get
            {
                CCRect c = new CCRect(origin.x, origin.y, size.Height, size.Width);
                return (c);
            }
        }
        public float MaxX
        {
            get { return (float) (origin.x + size.Width); }
        }

        public float MidX
        {
            get { return (float) (origin.x + size.Width / 2.0); }
        }

        public float MinX
        {
            get { return origin.x; }
        }

        public float MaxY
        {
            get { return origin.y + size.Height; }
        }

        public float MidY
        {
            get { return (float) (origin.y + size.Height / 2.0); }
        }

        public float MinY
        {
            get { return origin.y; }
        }

        public CCRect Intersection(CCRect rect)
        {
            if (!IntersectsRect(rect))
            {
                return (CCRect.Zero);
            }
            CCRect r;
            /*       +-------------+
             *       |             |
             *       |         +---+-----+
             * +-----+---+     |   |     |
             * |     |   |     |   |     |
             * |     |   |     +---+-----+
             * |     |   |         |
             * |     |   |         |
             * +-----+---+         |
             *       |             |
             *       +-------------+
             */
            float minx=0, miny=0, maxx=0, maxy=0;
            // X
            if (rect.MinX < MinX)
            {
                minx = MinX;
            }
            else if (rect.MinX < MaxX)
            {
                minx = rect.MinX;
            }
            if (rect.MaxX < MaxX)
            {
                maxx = rect.MaxX;
            }
            else if (rect.MaxX > MaxX)
            {
                maxx = MaxX;
            }
            //  Y
            if (rect.MinY < MinY)
            {
                miny = MinY;
            }
            else if (rect.MinY < MaxY)
            {
                miny = rect.MinY;
            }
            if (rect.MaxY < MaxY)
            {
                maxy = rect.MaxY;
            }
            else if (rect.MaxY > MaxY)
            {
                maxy = MaxY;
            }
            return (new CCRect(minx, miny, maxx - minx, maxy - miny));
        }
        public bool IntersectsRect(CCRect rect)
        {
            return !(MaxX < rect.MinX || rect.MaxX < MinX || MaxY < rect.MinY || rect.MaxY < MinY);
        }

        public bool IntersectsRect(ref CCRect rect)
        {
            return !(MaxX < rect.MinX || rect.MaxX < MinX || MaxY < rect.MinY || rect.MaxY < MinY);
        }

        public bool ContainsPoint(CCPoint point)
        {
            return point.x >= MinX && point.x <= MaxX && point.y >= MinY && point.y <= MaxY;
        }

        public bool ContainsPoint(float x, float y)
        {
            return x >= MinX && x <= MaxX && y >= MinY && y <= MaxY;
        }

        // return the leftmost x-value of 'rect'
        public static float CCRectGetMinX(CCRect rect)
        {
            return rect.origin.x;
        }

        // return the rightmost x-value of 'rect'
        public static float CCRectGetMaxX(CCRect rect)
        {
            return rect.origin.x + rect.size.Width;
        }

        // return the midpoint x-value of 'rect'
        public static float CCRectGetMidX(CCRect rect)
        {
            return (rect.origin.x + rect.size.Width / 2.0f);
        }

        // Return the bottommost y-value of 'rect'
        public static float CCRectGetMinY(CCRect rect)
        {
            return rect.origin.y;
        }

        // Return the topmost y-value of 'rect'
        public static float CCRectGetMaxY(CCRect rect)
        {
            return rect.origin.y + rect.size.Height;
        }

        // Return the midpoint y-value of 'rect'
        public static float CCRectGetMidY(CCRect rect)
        {
            return (rect.origin.y + rect.size.Height / 2.0f);
        }

        public static bool CCRectEqualToRect(CCRect rect1, CCRect rect2)
        {
            return (rect1.origin.Equals(rect2.origin)) && (rect1.size.Equals(rect2.size));
        }

        public static bool CCRectContainsPoint(CCRect rect, CCPoint point)
        {
            bool bRet = false;

            if (float.IsNaN(point.x))
            {
                point.x = 0;
            }

            if (float.IsNaN(point.y))
            {
                point.y = 0;
            }

            if (point.x >= CCRectGetMinX(rect) && point.x <= CCRectGetMaxX(rect) && point.y >= CCRectGetMinY(rect) && point.y <= CCRectGetMaxY(rect))
            {
                bRet = true;
            }

            return bRet;
        }

        public static bool CCRectIntersetsRect(CCRect rectA, CCRect rectB)
        {
            return !(CCRectGetMaxX(rectA) < CCRectGetMinX(rectB)
                     || CCRectGetMaxX(rectB) < CCRectGetMinX(rectA)
                     || CCRectGetMaxY(rectA) < CCRectGetMinY(rectB)
                     || CCRectGetMaxY(rectB) < CCRectGetMinY(rectA));
        }
        public static bool operator ==(CCRect p1, CCRect p2)
        {
            return (p1.Equals(p2));
        }
        public static bool operator !=(CCRect p1, CCRect p2)
        {
            return (!p1.Equals(p2));
        }

        public override bool Equals(object obj)
        {
            return (Equals((CCRect)obj));
        }

        public bool Equals(CCRect rect)
        {
            return (origin.Equals(rect.origin)) && (size.Equals(rect.size));
        }

        public override string ToString()
        {
            return String.Format("CCRect : (x={0}, y={1}, width={2}, height={3})", origin.x, origin.y, size.Width, size.Height);
        }
    }
}
