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
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace cocos2d
{

#if !WINDOWS_PHONE
	[Serializable, StructLayout(LayoutKind.Sequential), TypeConverter(typeof(CCPointConverter))]
#endif
    public struct CCPoint
    {
        public static readonly CCPoint Zero = new CCPoint(0, 0);

        public float X;
        public float Y;

        public CCPoint(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public CCPoint(CCPoint pt)
        {
            this.X = pt.X;
            this.Y = pt.Y;
        }

        public CCPoint(Point pt)
        {
            X = pt.X;
            Y = pt.Y;
        }

        public CCPoint(Vector2 v)
        {
            X = v.X;
            Y = v.Y;
        }

        public static bool CCPointEqualToPoint(CCPoint point1, CCPoint point2)
        {
            return ((point1.X == point2.X) && (point1.Y == point2.Y));
        }

        public CCPoint Offset(float dx, float dy)
        {
            return (new CCPoint(X + dx, Y + dy));
        }
        public CCPoint Reverse
        {
            get
            {
                return (new CCPoint(-X, -Y));
            }
        }

		public override int GetHashCode ()
		{
			return this.X.GetHashCode() + this.Y.GetHashCode();
		}
        public override bool Equals(object obj)
        {
            return (Equals((CCPoint)obj));
        }
        public bool Equals(CCPoint p)
        {
            return X == p.X && Y == p.Y;
        }

        public override string ToString()
        {
            return String.Format("CCPoint : (x={0}, y={1})", X, Y);
        }

        public float DistanceSQ(ref CCPoint v2)
        {
            return Sub(ref v2).LengthSQ;
        }

        public CCPoint Sub(ref CCPoint v2)
        {
            return new CCPoint(X - v2.X, Y - v2.Y);
        }

        public float LengthSQ
        {
            get { return X * X + Y * Y; }
        }

        public float Length
        {
            get { return (float)Math.Sqrt(X * X + Y * Y); }
        }

        /// <summary>
        /// Inverts the direction or location of the Y component.
        /// </summary>
        public CCPoint InvertY
        {
            get
            {
                return (new CCPoint(X, -Y));
            }
        }

        public static CCPoint Perp(CCPoint p)
        {
            return new CCPoint(-p.Y, p.X);
        }

        public static float Dot(CCPoint p1, CCPoint p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y;
        }

        public void Normalize()
        {
            var l = 1f / (float)Math.Sqrt(X * X + Y * Y);
            X *= l;
            Y *= l;
        }

        public static CCPoint Normalize(CCPoint p)
        {
            var x = p.X;
            var y = p.Y;
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
            return new CCPoint(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static CCPoint operator +(CCPoint p1, CCPoint p2)
        {
            return new CCPoint(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static CCPoint operator *(CCPoint p, float value)
        {
            return new CCPoint(p.X * value, p.Y * value);
        }

		public static CCPoint Parse(string s) 
		{
#if !WINDOWS_PHONE
			return (CCPoint)TypeDescriptor.GetConverter(typeof(CCPoint)).ConvertFromString (s);
#else

            throw(new NotImplementedException("Serialization is not supported on the WindowsPhone device."));
#endif
		}
    }

#if !WINDOWS_PHONE
	[Serializable, StructLayout(LayoutKind.Sequential), TypeConverter(typeof(CCSizeConverter))]
#endif
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

		public override int GetHashCode()
		{
			return (this.Width.GetHashCode() + this.Height.GetHashCode());
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

		public static CCSize Parse(string s) 
		{
#if !WINDOWS_PHONE
			return (CCSize)TypeDescriptor.GetConverter(typeof(CCSize)).ConvertFromString (s);
#else
            throw(new NotImplementedException("Serialization is not supported on the WindowsPhone device."));
#endif
		}
    }

#if !WINDOWS_PHONE
	[Serializable, StructLayout(LayoutKind.Sequential), TypeConverter(typeof(CCRectConverter))]
#endif
    public struct CCRect
    {
        public static readonly CCRect Zero = new CCRect(0, 0, 0, 0);

        public CCPoint Origin;
        public CCSize Size;

        /// <summary>
        /// Creates the rectangle at (x,y) -> (width,height)
        /// </summary>
        /// <param name="x">Lower Left corner X</param>
        /// <param name="y">Lower left corner Y</param>
        /// <param name="width">width of the rectangle</param>
        /// <param name="height">height of the rectangle</param>
        public CCRect(float x, float y, float width, float height)
        {
            Origin = new CCPoint();
            Size = new CCSize();

            // Only support that, the width and height > 0
            Debug.Assert(width >= 0 && height >= 0);

            Origin.X = x;
            Origin.Y = y;

            Size.Width = width;
            Size.Height = height;
        }

        /// <summary>
        /// Returns the inversion of this rect's size, which is the height and width swapped, while the origin stays unchanged.
        /// </summary>
        public CCRect InvertedSize
        {
            get
            {
                CCRect c = new CCRect(Origin.X, Origin.Y, Size.Height, Size.Width);
                return (c);
            }
        }
        public float MaxX
        {
            get { return (float) (Origin.X + Size.Width); }
        }

        public float MidX
        {
            get { return (float) (Origin.X + Size.Width / 2.0); }
        }

        public float MinX
        {
            get { return Origin.X; }
        }

        public float MaxY
        {
            get { return Origin.Y + Size.Height; }
        }

        public float MidY
        {
            get { return (float) (Origin.Y + Size.Height / 2.0); }
        }

        public float MinY
        {
            get { return Origin.Y; }
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
            return point.X >= MinX && point.X <= MaxX && point.Y >= MinY && point.Y <= MaxY;
        }

        public bool ContainsPoint(float x, float y)
        {
            return x >= MinX && x <= MaxX && y >= MinY && y <= MaxY;
        }

        // return the leftmost x-value of 'rect'
        public static float CCRectGetMinX(CCRect rect)
        {
            return rect.Origin.X;
        }

        // return the rightmost x-value of 'rect'
        public static float CCRectGetMaxX(CCRect rect)
        {
            return rect.Origin.X + rect.Size.Width;
        }

        // return the midpoint x-value of 'rect'
        public static float CCRectGetMidX(CCRect rect)
        {
            return (rect.Origin.X + rect.Size.Width / 2.0f);
        }

        // Return the bottommost y-value of 'rect'
        public static float CCRectGetMinY(CCRect rect)
        {
            return rect.Origin.Y;
        }

        // Return the topmost y-value of 'rect'
        public static float CCRectGetMaxY(CCRect rect)
        {
            return rect.Origin.Y + rect.Size.Height;
        }

        // Return the midpoint y-value of 'rect'
        public static float CCRectGetMidY(CCRect rect)
        {
            return (rect.Origin.Y + rect.Size.Height / 2.0f);
        }

        public static bool CCRectEqualToRect(CCRect rect1, CCRect rect2)
        {
            return (rect1.Origin.Equals(rect2.Origin)) && (rect1.Size.Equals(rect2.Size));
        }

        public static bool CCRectContainsPoint(CCRect rect, CCPoint point)
        {
            bool bRet = false;

            if (float.IsNaN(point.X))
            {
                point.X = 0;
            }

            if (float.IsNaN(point.Y))
            {
                point.Y = 0;
            }

            if (point.X >= CCRectGetMinX(rect) && point.X <= CCRectGetMaxX(rect) && point.Y >= CCRectGetMinY(rect) && point.Y <= CCRectGetMaxY(rect))
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

		public override int GetHashCode()
		{
			//return (this.Origin.X.GetHashCode() + this.Origin.Y.GetHashCode() + this.Size.Width.GetHashCode() + this.Size.Height.GetHashCode());
			return this.Origin.GetHashCode() + this.Size.GetHashCode();
		}

        public override bool Equals(object obj)
        {
            return (Equals((CCRect)obj));
        }

        public bool Equals(CCRect rect)
        {
            return (Origin.Equals(rect.Origin)) && (Size.Equals(rect.Size));
        }

        public override string ToString()
        {
            return String.Format("CCRect : (x={0}, y={1}, width={2}, height={3})", Origin.X, Origin.Y, Size.Width, Size.Height);
        }

		public static CCRect Parse(string s) 
		{
#if !WINDOWS_PHONE
			return (CCRect)TypeDescriptor.GetConverter(typeof(CCRect)).ConvertFromString (s);
#else
            throw(new NotImplementedException("Serialization is not supported on the WindowsPhone device."));
#endif
        }
    }
}
