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

#if !WINDOWS_PHONE && !XBOX && !NETFX_CORE
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
            CCPoint pt = CCPoint.Zero;
            pt.X = X + dx;
            pt.Y = Y + dy;
            return pt;
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
            CCPoint pt = CCPoint.Zero;
            pt.X = X - v2.X;
            pt.Y = Y - v2.Y;
            return pt;
        }

        public float LengthSQ
        {
            get { return X * X + Y * Y; }
        }

        public float LengthSquare
        {
            get { return LengthSQ; }
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
                CCPoint pt = CCPoint.Zero;
                pt.X = X;
                pt.Y = -Y;
                return pt;
            }
        }

        public void Normalize()
        {
            var l = 1f / (float)Math.Sqrt(X * X + Y * Y);
            X *= l;
            Y *= l;
        }

        #region Static Methods

        /** Run a math operation function on each point component
         * absf, fllorf, ceilf, roundf
         * any function that has the signature: float func(float);
         * For example: let's try to take the floor of x,y
         * ccpCompOp(p,floorf);
         @since v0.99.1
         */
        public delegate float ComputationOperationDelegate(float a);
        public static CCPoint ComputationOperation(CCPoint p, ComputationOperationDelegate del)
        {
            CCPoint pt = CCPoint.Zero;
            pt.X = del(p.X);
            pt.Y = del(p.Y);
            return (pt);
        }

        /** Linear Interpolation between two points a and b
            @returns
              alpha == 0 ? a
              alpha == 1 ? b
              otherwise a value between a..b
            @since v0.99.1
       */
        public static CCPoint Lerp(CCPoint a, CCPoint b, float alpha)
        {
            return (a * (1f - alpha) + b * alpha);
        }


        /** @returns if points have fuzzy equality which means equal with some degree of variance.
            @since v0.99.1
        */
        public static bool FuzzyEqual(CCPoint a, CCPoint b, float variance)
        {
            if (a.X - variance <= b.X && b.X <= a.X + variance)
                if (a.Y - variance <= b.Y && b.Y <= a.Y + variance)
                    return true;

            return false;
        }


        /** Multiplies a nd b components, a.x*b.x, a.y*b.y
            @returns a component-wise multiplication
            @since v0.99.1
        */
        public static CCPoint MultiplyComponents(CCPoint a, CCPoint b)
        {
            CCPoint pt = CCPoint.Zero;
            pt.X = a.X * b.X;
            pt.Y = a.Y * b.Y;
            return pt;
        }

        /** @returns the signed angle in radians between two vector directions
            @since v0.99.1
        */
        public static float AngleSigned(CCPoint a, CCPoint b)
        {
            CCPoint a2 = CCPoint.Normalize(a);
            CCPoint b2 = CCPoint.Normalize(b);
            float angle = (float)Math.Atan2(a2.X * b2.Y - a2.Y * b2.X, DotProduct(a2, b2));

            if (Math.Abs(angle) < float.Epsilon)
            {
                return 0.0f;
            }

            return angle;
        }

        /** Rotates a point counter clockwise by the angle around a pivot
            @param v is the point to rotate
            @param pivot is the pivot, naturally
            @param angle is the angle of rotation cw in radians
            @returns the rotated point
            @since v0.99.1
        */
        public static CCPoint RotateByAngle(CCPoint v, CCPoint pivot, float angle)
        {
            CCPoint r = v - pivot;
            float cosa = (float)Math.Cos(angle), sina = (float)Math.Sin(angle);
            float t = r.X;

            r.X = t * cosa - r.Y * sina + pivot.X;
            r.Y = t * sina + r.Y * cosa + pivot.Y;

            return r;
        }

        /** A general line-line intersection test
         @param p1 
            is the startpoint for the first line P1 = (p1 - p2)
         @param p2 
            is the endpoint for the first line P1 = (p1 - p2)
         @param p3 
            is the startpoint for the second line P2 = (p3 - p4)
         @param p4 
            is the endpoint for the second line P2 = (p3 - p4)
         @param s 
            is the range for a hitpoint in P1 (pa = p1 + s*(p2 - p1))
         @param t
            is the range for a hitpoint in P3 (pa = p2 + t*(p4 - p3))
         @return bool 
            indicating successful intersection of a line
            note that to truly test intersection for segments we have to make 
            sure that s & t lie within [0..1] and for rays, make sure s & t > 0
            the hit point is		p3 + t * (p4 - p3);
            the hit point also is	p1 + s * (p2 - p1);
         @since v0.99.1
         */
        public static bool LineIntersect(CCPoint A, CCPoint B, CCPoint C, CCPoint D, ref float S, ref float T)
        {
            // FAIL: Line undefined
            if ((A.X == B.X && A.Y == B.Y) || (C.X == D.X && C.Y == D.Y))
            {
                return false;
            }

            float BAx = B.X - A.X;
            float BAy = B.Y - A.Y;
            float DCx = D.X - C.X;
            float DCy = D.Y - C.Y;
            float ACx = A.X - C.X;
            float ACy = A.Y - C.Y;

            float denom = DCy * BAx - DCx * BAy;

            S = DCx * ACy - DCy * ACx;
            T = BAx * ACy - BAy * ACx;

            if (denom == 0)
            {
                if (S == 0 || T == 0)
                {
                    // Lines incident
                    return true;
                }
                // Lines parallel and not incident
                return false;
            }

            S = S / denom;
            T = T / denom;

            // Point of intersection
            // CGPoint P;
            // P.x = A.x + *S * (B.x - A.x);
            // P.y = A.y + *S * (B.y - A.y);

            return true;
        }

        /*
        ccpSegmentIntersect returns YES if Segment A-B intersects with segment C-D
        @since v1.0.0
        */
        public static bool SegmentIntersect(CCPoint A, CCPoint B, CCPoint C, CCPoint D)
        {
            float S = 0, T = 0;

            if (LineIntersect(A, B, C, D, ref S, ref T)
                && (S >= 0.0f && S <= 1.0f && T >= 0.0f && T <= 1.0f))
            {
                return true;
            }

            return false;
        }

        /*
        ccpIntersectPoint returns the intersection point of line A-B, C-D
        @since v1.0.0
        */
        public static CCPoint IntersectPoint(CCPoint A, CCPoint B, CCPoint C, CCPoint D)
        {
            float S = 0, T = 0;

            if (LineIntersect(A, B, C, D, ref S, ref T))
            {
                // Point of intersection
                CCPoint P = CCPoint.Zero;
                P.X = A.X + S * (B.X - A.X);
                P.Y = A.Y + S * (B.Y - A.Y);
                return P;
            }

            return new CCPoint();
        }
        /** Converts radians to a normalized vector.
            @return CCPoint
            @since v0.7.2
        */
        public static CCPoint ForAngle(float a)
        {
            CCPoint pt = CCPoint.Zero;
            pt.X = (float)Math.Cos(a);
            pt.Y = (float)Math.Sin(a);
            return (pt);
//            return CreatePoint((float)Math.Cos(a), (float)Math.Sin(a));
        }

        /** Converts a vector to radians.
            @return CGFloat
            @since v0.7.2
        */
        public static float ToAngle(CCPoint v)
        {
            return (float)Math.Atan2(v.Y, v.X);
        }


        /** Clamp a value between from and to.
            @since v0.99.1
        */
        public static float Clamp(float value, float min_inclusive, float max_inclusive)
        {
            if (min_inclusive > max_inclusive)
            {
                float ftmp;
                ftmp = min_inclusive;
                min_inclusive = max_inclusive;
                max_inclusive = min_inclusive;
            }

            return value < min_inclusive ? min_inclusive : value < max_inclusive ? value : max_inclusive;
        }

        /** Clamp a point between from and to.
            @since v0.99.1
        */
        public static CCPoint Clamp(CCPoint p, CCPoint from, CCPoint to)
        {
            CCPoint pt = CCPoint.Zero;
            pt.X = Clamp(p.X, from.X, to.X);
            pt.Y = Clamp(p.Y, from.Y, to.Y);
            return pt;
//            return CreatePoint(Clamp(p.X, from.X, to.X), Clamp(p.Y, from.Y, to.Y));
        }

        /** Quickly convert CCSize to a CCPoint
            @since v0.99.1
        */
        public static CCPoint FromSize(CCSize s)
        {
            CCPoint pt = CCPoint.Zero;
            pt.X = s.Width;
            pt.Y = s.Height;
            return pt;
        }

        public static CCPoint Perp(CCPoint p)
        {
            CCPoint pt = CCPoint.Zero;
            pt.X = -p.Y;
            pt.Y = p.X;
            return pt;
        }

        public static float Dot(CCPoint p1, CCPoint p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y;
        }

        public static float Distance(CCPoint v1, CCPoint v2)
        {
            return (v1 - v2).Length;
        }
        public static CCPoint Normalize(CCPoint p)
        {
            var x = p.X;
            var y = p.Y;
            var l = 1f / (float)Math.Sqrt(x * x + y * y);
            CCPoint pt = CCPoint.Zero;
            pt.X = x*l;
            pt.Y = y*l;
            return pt;
        }

        public static CCPoint Midpoint(CCPoint p1, CCPoint p2)
        {
            CCPoint pt = CCPoint.Zero;
            pt.X = (p1.X + p2.X)/2f;
            pt.Y = (p1.Y + p2.Y)/2f;
            return pt;
        }
        public static float DotProduct(CCPoint v1, CCPoint v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        /** Calculates cross product of two points.
            @return CGFloat
            @since v0.7.2
        */
        public static float CrossProduct(CCPoint v1, CCPoint v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }

        /** Calculates perpendicular of v, rotated 90 degrees counter-clockwise -- cross(v, perp(v)) >= 0
            @return CCPoint
            @since v0.7.2
        */
        public static CCPoint PerpendicularCounterClockwise(CCPoint v)
        {
            CCPoint pt = CCPoint.Zero;
            pt.X = -v.Y;
            pt.Y = v.X;
            return (pt);
        }

        /** Calculates perpendicular of v, rotated 90 degrees clockwise -- cross(v, rperp(v)) <= 0
            @return CCPoint
            @since v0.7.2
        */
        public static CCPoint PerpendicularClockwise(CCPoint v)
        {
            CCPoint pt = CCPoint.Zero;
            pt.X = v.Y;
            pt.Y = -v.X;
            return (pt);
        }

        /** Calculates the projection of v1 over v2.
            @return CCPoint
            @since v0.7.2
        */
        public static CCPoint Project(CCPoint v1, CCPoint v2)
        {
            float dp1 = v1.X * v2.X + v1.Y * v2.Y;
            float dp2 = v2.LengthSQ;
            float f = dp1 / dp2;
            CCPoint pt = CCPoint.Zero;
            pt.X = v2.X * f;
            pt.Y = v2.Y * f;
            return (pt);
            // return Multiply(v2, DotProduct(v1, v2) / DotProduct(v2, v2));
        }

        /** Rotates two points.
            @return CCPoint
            @since v0.7.2
        */
        public static CCPoint Rotate(CCPoint v1, CCPoint v2)
        {
            CCPoint pt = CCPoint.Zero;
            pt.X = v1.X * v2.X - v1.Y * v2.Y;
            pt.Y = v1.X * v2.Y + v1.Y * v2.X;
            return(pt);
        }

        /** Unrotates two points.
            @return CCPoint
            @since v0.7.2
        */
        public static CCPoint Unrotate(CCPoint v1, CCPoint v2)
        {
            CCPoint pt = CCPoint.Zero;
            pt.X = v1.X * v2.X + v1.Y * v2.Y;
            pt.Y = v1.Y * v2.X - v1.X * v2.Y;
            return(pt);
        }

        #endregion

        #region Operator Overloads

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
            CCPoint pt = CCPoint.Zero;
            pt.X = p1.X - p2.X;
            pt.Y = p1.Y - p2.Y;
            return pt;
        }

        public static CCPoint operator -(CCPoint p1)
        {
            CCPoint pt = CCPoint.Zero;
            pt.X = -p1.X;
            pt.Y = -p1.Y;
            return pt;
        }

        public static CCPoint operator +(CCPoint p1, CCPoint p2)
        {
            CCPoint pt = CCPoint.Zero;
            pt.X = p1.X + p2.X;
            pt.Y = p1.Y + p2.Y;
            return pt;
        }

        public static CCPoint operator +(CCPoint p1)
        {
            CCPoint pt = CCPoint.Zero;
            pt.X = +p1.X;
            pt.Y = +p1.Y;
            return pt;
        }
        
        public static CCPoint operator *(CCPoint p, float value)
        {
            CCPoint pt = CCPoint.Zero;
            pt.X = p.X * value;
            pt.Y = p.Y * value;
            return pt;
        }
        #endregion

        public static CCPoint Parse(string s)
        {
#if !WINDOWS_PHONE && !XBOX && !NETFX_CORE
			return (CCPoint)TypeDescriptor.GetConverter(typeof(CCPoint)).ConvertFromString (s);
#else
            return (CCPointConverter.CCPointFromString(s));
#endif
		}
    }

#if !WINDOWS_PHONE && !XBOX && !NETFX_CORE
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
#if !WINDOWS_PHONE && !XBOX && !NETFX_CORE
			return (CCSize)TypeDescriptor.GetConverter(typeof(CCSize)).ConvertFromString (s);
#else
            return (CCSizeConverter.CCSizeFromString(s));
#endif
		}
    }

#if !WINDOWS_PHONE && !XBOX && !NETFX_CORE
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
#if !WINDOWS_PHONE && !XBOX && !NETFX_CORE
			return (CCRect)TypeDescriptor.GetConverter(typeof(CCRect)).ConvertFromString (s);
#else
            return (CCRectConverter.CCRectFromString(s));
#endif
        }
    }
}
