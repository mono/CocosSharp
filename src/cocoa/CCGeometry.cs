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
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;
using System.Globalization;

namespace CocosSharp
{
#if !WINDOWS_PHONE && !NETFX_CORE
    [Serializable, StructLayout(LayoutKind.Sequential)]
#endif
    public struct CCPoint
    {
        public static readonly CCPoint Zero = new CCPoint(0, 0);
		public static readonly CCPoint NegativeInfinity = new CCPoint(float.NegativeInfinity, float.NegativeInfinity);

        public static readonly CCPoint AnchorMiddle = new CCPoint(0.5f, 0.5f);
        public static readonly CCPoint AnchorLowerLeft = new CCPoint(0f, 0f);
        public static readonly CCPoint AnchorUpperLeft = new CCPoint(0f, 1f);
        public static readonly CCPoint AnchorLowerRight = new CCPoint(1f, 0f);
        public static readonly CCPoint AnchorUpperRight = new CCPoint(1f, 1f);
        public static readonly CCPoint AnchorMiddleRight = new CCPoint(1f, .5f);
        public static readonly CCPoint AnchorMiddleLeft = new CCPoint(0f, .5f);
        public static readonly CCPoint AnchorMiddleTop = new CCPoint(.5f, 1f);
        public static readonly CCPoint AnchorMiddleBottom = new CCPoint(.5f, 0f);

        public float X;
        public float Y;


		#region Properties

		public float LengthSQ
		{
			get { return X * X + Y * Y; }
		}

		public float LengthSquare
		{
			get { return LengthSQ; }
		}

		// Computes the length of this point as if it were a vector with XY components relative to the
		// origin. This is computed each time this property is accessed, so cache the value that is
		// returned.
		public float Length
		{
			get { return (float) Math.Sqrt(X * X + Y * Y); }
		}

		public CCPoint Reverse
		{
			get { return new CCPoint(-X, -Y); }
		}

		public CCPoint InvertY
		{
			get
			{
				CCPoint pt;
				pt.X = X;
				pt.Y = -Y;
				return pt;
			}
		}

		#endregion Properties


		#region Constructors

        public CCPoint(float x, float y)
        {
            X = x;
            Y = y;
        }

        public CCPoint(CCPoint pt)
        {
            X = pt.X;
            Y = pt.Y;
        }

        public CCPoint(CCVector2 v)
        {
            X = v.X;
            Y = v.Y;
        }

		internal CCPoint(Vector2 v)
		{
			X = v.X;
			Y = v.Y;
		}

		#endregion Constructors

		#region Equality

        public static bool Equal(ref CCPoint point1, ref CCPoint point2)
        {
            return ((point1.X == point2.X) && (point1.Y == point2.Y));
        }

        public override bool Equals(object obj)
        {
            return (Equals((CCPoint) obj));
        }

        public bool Equals(CCPoint p)
        {
            return X == p.X && Y == p.Y;
        }

		public override int GetHashCode()
		{
			return X.GetHashCode() + Y.GetHashCode();
		}

		#endregion Equality


        public override string ToString()
        {
            return String.Format("CCPoint : (x={0}, y={1})", X, Y);
        }

        public float DistanceSQ(ref CCPoint v2)
        {
            return Sub(ref v2).LengthSQ;
        }

		public float Angle
		{
			get { return (float) Math.Atan2(Y,X); }
		}

        /// <summary>
        ///     Normalizes the components of this point (convert to mag 1), and returns the orignial
        ///     magnitude of the vector defined by the XY components of this point.
        /// </summary>
        /// <returns></returns>
        public float Normalize()
        {
            var mag = (float) Math.Sqrt(X * X + Y * Y);
            if (mag < float.Epsilon)
            {
                return (0f);
            }
            float l = 1f / mag;
            X *= l;
            Y *= l;
            return (mag);
        }

		public CCPoint Sub(ref CCPoint v2)
		{
			CCPoint pt;
			pt.X = X - v2.X;
			pt.Y = Y - v2.Y;
			return pt;
		}

		public CCPoint Offset(float dx, float dy)
		{
			CCPoint pt;
			pt.X = X + dx;
			pt.Y = Y + dy;
			return pt;
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
            CCPoint pt;
            pt.X = del(p.X);
            pt.Y = del(p.Y);
            return pt;
        }

        /** Linear Interpolation between two points a and b
            @returns
              alpha == 0 ? a
              alpha == 1 ? b
              otherwise a value between a..B
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


        /** Multiplies a nd b components, a.X*b.X, a.y*b.y
            @returns a component-wise multiplication
            @since v0.99.1
        */

        public static CCPoint MultiplyComponents(CCPoint a, CCPoint b)
        {
            CCPoint pt;
            pt.X = a.X * b.X;
            pt.Y = a.Y * b.Y;
            return pt;
        }

        /** @returns the signed angle in radians between two vector directions
            @since v0.99.1
        */

        public static float AngleSigned(CCPoint a, CCPoint b)
        {
            CCPoint a2 = Normalize(a);
            CCPoint b2 = Normalize(b);
            var angle = (float) Math.Atan2(a2.X * b2.Y - a2.Y * b2.X, DotProduct(a2, b2));

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
            float cosa = (float) Math.Cos(angle), sina = (float) Math.Sin(angle);
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
            // P.X = A.X + *S * (B.X - A.X);
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
                CCPoint P;
                P.X = A.X + S * (B.X - A.X);
                P.Y = A.Y + S * (B.Y - A.Y);
                return P;
            }

            return Zero;
        }

        /** Converts radians to a normalized vector.
            @return CCPoint
            @since v0.7.2
        */

        public static CCPoint ForAngle(float a)
        {
            CCPoint pt;
            pt.X = (float) Math.Cos(a);
            pt.Y = (float) Math.Sin(a);
            return pt;
            //            return CreatePoint((float)Math.Cos(a), (float)Math.Sin(a));
        }

        /** Converts a vector to radians.
            @return CGFloat
            @since v0.7.2
        */

        public static float ToAngle(CCPoint v)
        {
            return (float) Math.Atan2(v.Y, v.X);
        }


        /** Clamp a value between from and to.
            @since v0.99.1
        */

        public static float Clamp(float value, float min_inclusive, float max_inclusive)
        {
            if (min_inclusive > max_inclusive)
            {
                float ftmp = min_inclusive;
                min_inclusive = max_inclusive;
                max_inclusive = ftmp;
            }

            return value < min_inclusive ? min_inclusive : value < max_inclusive ? value : max_inclusive;
        }

        /** Clamp a point between from and to.
            @since v0.99.1
        */

        public static CCPoint Clamp(CCPoint p, CCPoint from, CCPoint to)
        {
            CCPoint pt;
            pt.X = Clamp(p.X, from.X, to.X);
            pt.Y = Clamp(p.Y, from.Y, to.Y);
            return pt;
            //            return CreatePoint(Clamp(p.X, from.X, to.X), Clamp(p.Y, from.Y, to.Y));
        }

        /**
         * Allow Cast CCSize to CCPoint
         */

        public static explicit operator CCPoint(CCSize size)
        {
            CCPoint pt;
            pt.X = size.Width;
            pt.Y = size.Height;
            return pt;
        }

        public static CCPoint Perp(CCPoint p)
        {
            CCPoint pt;
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
            float x = p.X;
            float y = p.Y;
            float l = 1f / (float) Math.Sqrt(x * x + y * y);
            CCPoint pt;
            pt.X = x * l;
            pt.Y = y * l;
            return pt;
        }

        public static CCPoint Midpoint(CCPoint p1, CCPoint p2)
        {
            CCPoint pt;
            pt.X = (p1.X + p2.X) / 2f;
            pt.Y = (p1.Y + p2.Y) / 2f;
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
            CCPoint pt;
            pt.X = -v.Y;
            pt.Y = v.X;
            return pt;
        }

        /** Calculates perpendicular of v, rotated 90 degrees clockwise -- cross(v, rperp(v)) <= 0
            @return CCPoint
            @since v0.7.2
        */

        public static CCPoint PerpendicularClockwise(CCPoint v)
        {
            CCPoint pt;
            pt.X = v.Y;
            pt.Y = -v.X;
            return pt;
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
            CCPoint pt;
            pt.X = v2.X * f;
            pt.Y = v2.Y * f;
            return pt;
            // return Multiply(v2, DotProduct(v1, v2) / DotProduct(v2, v2));
        }

        /** Rotates two points.
            @return CCPoint
            @since v0.7.2
        */

        public static CCPoint Rotate(CCPoint v1, CCPoint v2)
        {
            CCPoint pt;
            pt.X = v1.X * v2.X - v1.Y * v2.Y;
            pt.Y = v1.X * v2.Y + v1.Y * v2.X;
            return pt;
        }

        /** Unrotates two points.
            @return CCPoint
            @since v0.7.2
        */

        public static CCPoint Unrotate(CCPoint v1, CCPoint v2)
        {
            CCPoint pt;
            pt.X = v1.X * v2.X + v1.Y * v2.Y;
            pt.Y = v1.Y * v2.X - v1.X * v2.Y;
            return pt;
        }

        #endregion

        #region Operator Overloads

        public static bool operator ==(CCPoint p1, CCPoint p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator !=(CCPoint p1, CCPoint p2)
        {
            return p1.X != p2.X || p1.Y != p2.Y;
        }

        public static CCPoint operator -(CCPoint p1, CCPoint p2)
        {
            CCPoint pt;
            pt.X = p1.X - p2.X;
            pt.Y = p1.Y - p2.Y;
            return pt;
        }

        public static CCPoint operator -(CCPoint p1)
        {
            CCPoint pt;
            pt.X = -p1.X;
            pt.Y = -p1.Y;
            return pt;
        }

        public static CCPoint operator +(CCPoint p1, CCPoint p2)
        {
            CCPoint pt;
            pt.X = p1.X + p2.X;
            pt.Y = p1.Y + p2.Y;
            return pt;
        }

        public static CCPoint operator +(CCPoint p1)
        {
            CCPoint pt;
            pt.X = +p1.X;
            pt.Y = +p1.Y;
            return pt;
        }

        public static CCPoint operator *(CCPoint p, float value)
        {
            CCPoint pt;
            pt.X = p.X * value;
            pt.Y = p.Y * value;
            return pt;
        }

        public static CCPoint operator *(CCPoint p1, CCPoint p2)
        {
            return new CCPoint(p1.X * p2.X, p1.Y * p2.Y);
        }

        public static CCPoint operator /(CCPoint p, float value)
        {
            CCPoint pt;
            pt.X = p.X / value;
            pt.Y = p.Y / value;
            return pt;
        }

        #endregion


        public static CCPoint Parse(string s)
        {
            return (CCPointConverter.CCPointFromString(s));
        }

        public static implicit operator CCPoint(CCVector2 point)
        {
             return new CCPoint(point.X, point.Y);
        }

        public static implicit operator CCVector2(CCPoint point)
        {
            return new CCVector2(point.X, point.Y);
        }

    }

#if !WINDOWS_PHONE && !NETFX_CORE
    [Serializable, StructLayout(LayoutKind.Sequential)]
#endif
    public struct CCSize
    {
        public static readonly CCSize Zero = new CCSize(0, 0);

        public float Width;
        public float Height;


		#region Properties

		public CCPoint Center
		{
			get { return new CCPoint(Width / 2f, Height / 2f); }
		}

		public CCSize Inverted
		{
			get { return new CCSize(Height, Width); }
		}

		#endregion Properties


		#region Constructors

        public CCSize(float width, float height)
        {
            Width = width;
            Height = height;
        }

		#endregion Constructors


		#region Equality

        public static bool Equal(ref CCSize size1, ref CCSize size2)
        {
            return ((size1.Width == size2.Width) && (size1.Height == size2.Height));
        }

        public bool Equals(CCSize s)
        {
            return Width == s.Width && Height == s.Height;
        }

        public override bool Equals(object obj)
        {
            return (Equals((CCSize) obj));
        }

		public override int GetHashCode()
		{
			return (Width.GetHashCode() + Height.GetHashCode());
		}
			
		#endregion Equality


        public override string ToString()
        {
            return String.Format("{0} x {1}", Width, Height);
        }


		public static CCSize Parse(string s)
		{
			return (CCSizeConverter.CCSizeFromString(s));
		}

		// Allow Cast CCPoint to CCSize
		public static explicit operator CCSize(CCPoint point)
		{
			CCSize size;
			size.Width = point.X;
			size.Height = point.Y;
			return size;
		}

		#region Operators

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
            return (new CCSize(p.Width * f, p.Height * f));
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

		#endregion Operators
    }

#if !WINDOWS_PHONE && !NETFX_CORE
    [Serializable, StructLayout(LayoutKind.Sequential)]
#endif
    public struct CCRect
    {
        public static readonly CCRect Zero = new CCRect(0, 0, 0, 0);

        public CCPoint Origin;
        public CCSize Size;


		#region Properties

		public float MinX { get { return Origin.X; } }
		public float MaxX { get { return Origin.X + Size.Width; } }
		public float MidX { get { return Origin.X + Size.Width / 2.0f; } }

		public float MinY { get { return Origin.Y; } }
		public float MaxY { get { return Origin.Y + Size.Height; } }
		public float MidY { get { return Origin.Y + Size.Height / 2.0f; } }

		public CCPoint Center
		{
			get
			{
				CCPoint pt;
				pt.X = MidX;
				pt.Y = MidY;
				return pt;
			}
		}

		public CCPoint UpperRight
		{
			get
			{
				CCPoint pt;
				pt.X = MaxX;
				pt.Y = MaxY;
				return (pt);
			}
		}

		public CCPoint LowerLeft
		{
			get
			{
				CCPoint pt;
				pt.X = MinX;
				pt.Y = MinY;
				return (pt);
			}
		}

		public CCRect InvertedSize
		{
			get { return new CCRect(Origin.X, Origin.Y, Size.Height, Size.Width); }
		}

		#endregion Properties


		#region Constructors

        public CCRect(float x, float y, float width, float height)
        {
            // Only support that, the width and height > 0
            Debug.Assert(width >= 0 && height >= 0);

            Origin.X = x;
            Origin.Y = y;

            Size.Width = width;
            Size.Height = height;
		}

		#endregion Constructors


		#region Equality

		public static bool Equal(ref CCRect rect1, ref CCRect rect2)
		{
			return rect1.Origin.Equals(rect2.Origin) && rect1.Size.Equals(rect2.Size);
		}

		public override bool Equals(object obj)
		{
			return (Equals((CCRect) obj));
		}

		public bool Equals(CCRect rect)
		{
			return Origin.Equals(rect.Origin) && Size.Equals(rect.Size);
		}

		public override int GetHashCode()
		{
			return Origin.GetHashCode() + Size.GetHashCode();
		}

		#endregion Equality


        public CCRect Intersection(CCRect rect)
        {
            if (!IntersectsRect(rect))
            {
                return (Zero);
            }

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
            float minx = 0, miny = 0, maxx = 0, maxy = 0;
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
            return new CCRect(minx, miny, maxx - minx, maxy - miny);
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


        public static bool ContainsPoint(ref CCRect rect, ref CCPoint point)
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

            if (point.X >= rect.MinX && point.X <= rect.MaxX && point.Y >= rect.MinY &&
                point.Y <= rect.MaxY)
            {
                bRet = true;
            }

            return bRet;
        }

        public static bool IntersetsRect(ref CCRect rectA, ref CCRect rectB)
        {
            return
                !(rectA.MaxX < rectB.MinX || rectB.MaxX < rectA.MinX || rectA.MaxY < rectB.MinY ||
                  rectB.MaxY < rectA.MinY);
        }

        public static bool operator ==(CCRect p1, CCRect p2)
        {
            return (p1.Equals(p2));
        }

        public static bool operator !=(CCRect p1, CCRect p2)
        {
            return (!p1.Equals(p2));
        }


        public override string ToString()
        {
            return String.Format("CCRect : (x={0}, y={1}, width={2}, height={3})", Origin.X, Origin.Y, Size.Width,
                                 Size.Height);
        }

        public static CCRect Parse(string s)
        {
            return (CCRectConverter.CCRectFromString(s));
        }
    }

	[DataContract]
	public struct CCVector2 : IEquatable<CCVector2>
	{
		static CCVector2 zeroVector = new CCVector2(0f, 0f);
		static CCVector2 unitVector = new CCVector2(1f, 1f);
		static CCVector2 unitXVector = new CCVector2(1f, 0f);
		static CCVector2 unitYVector = new CCVector2(0f, 1f);

		[DataMember]
		public float X;

		[DataMember]
		public float Y;


		#region Properties

		public static CCVector2 Zero
		{
			get { return zeroVector; }
		}

		public static CCVector2 One
		{
			get { return unitVector; }
		}

		public static CCVector2 UnitX
		{
			get { return unitXVector; }
		}

		public static CCVector2 UnitY
		{
			get { return unitYVector; }
		}

		#endregion Properties


		#region Constructors

		public CCVector2(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}

		public CCVector2(float value)
		{
			this.X = value;
			this.Y = value;
		}

		#endregion Constructors


		#region Public Methods

		public static CCVector2 Add(CCVector2 value1, CCVector2 value2)
		{
			value1.X += value2.X;
			value1.Y += value2.Y;
			return value1;
		}

		public static void Add(ref CCVector2 value1, ref CCVector2 value2, out CCVector2 result)
		{
			result.X = value1.X + value2.X;
			result.Y = value1.Y + value2.Y;
		}

		public static CCVector2 Barycentric(CCVector2 value1, CCVector2 value2, CCVector2 value3, float amount1, float amount2)
		{
			return new CCVector2(
				MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2),
				MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2));
		}

		public static void Barycentric(ref CCVector2 value1, ref CCVector2 value2, ref CCVector2 value3, float amount1, float amount2, out CCVector2 result)
		{
			result = new CCVector2(
				MathHelper.Barycentric(value1.X, value2.X, value3.X, amount1, amount2),
				MathHelper.Barycentric(value1.Y, value2.Y, value3.Y, amount1, amount2));
		}

		public static CCVector2 CatmullRom(CCVector2 value1, CCVector2 value2, CCVector2 value3, CCVector2 value4, float amount)
		{
			return new CCVector2(
				MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
				MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount));
		}

		public static void CatmullRom(ref CCVector2 value1, ref CCVector2 value2, ref CCVector2 value3, ref CCVector2 value4, float amount, out CCVector2 result)
		{
			result = new CCVector2(
				MathHelper.CatmullRom(value1.X, value2.X, value3.X, value4.X, amount),
				MathHelper.CatmullRom(value1.Y, value2.Y, value3.Y, value4.Y, amount));
		}

		public static CCVector2 Clamp(CCVector2 value1, CCVector2 min, CCVector2 max)
		{
			return new CCVector2(
				MathHelper.Clamp(value1.X, min.X, max.X),
				MathHelper.Clamp(value1.Y, min.Y, max.Y));
		}

		public static void Clamp(ref CCVector2 value1, ref CCVector2 min, ref CCVector2 max, out CCVector2 result)
		{
			result = new CCVector2(
				MathHelper.Clamp(value1.X, min.X, max.X),
				MathHelper.Clamp(value1.Y, min.Y, max.Y));
		}

		public static float Distance(CCVector2 value1, CCVector2 value2)
		{
			float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
			return (float)Math.Sqrt((v1 * v1) + (v2 * v2));
		}

		public static void Distance(ref CCVector2 value1, ref CCVector2 value2, out float result)
		{
			float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
			result = (float)Math.Sqrt((v1 * v1) + (v2 * v2));
		}

		public static float DistanceSquared(CCVector2 value1, CCVector2 value2)
		{
			float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
			return (v1 * v1) + (v2 * v2);
		}

		public static void DistanceSquared(ref CCVector2 value1, ref CCVector2 value2, out float result)
		{
			float v1 = value1.X - value2.X, v2 = value1.Y - value2.Y;
			result = (v1 * v1) + (v2 * v2);
		}

		public static CCVector2 Divide(CCVector2 value1, CCVector2 value2)
		{
			value1.X /= value2.X;
			value1.Y /= value2.Y;
			return value1;
		}

		public static void Divide(ref CCVector2 value1, ref CCVector2 value2, out CCVector2 result)
		{
			result.X = value1.X / value2.X;
			result.Y = value1.Y / value2.Y;
		}

		public static CCVector2 Divide(CCVector2 value1, float divider)
		{
			float factor = 1 / divider;
			value1.X *= factor;
			value1.Y *= factor;
			return value1;
		}

		public static void Divide(ref CCVector2 value1, float divider, out CCVector2 result)
		{
			float factor = 1 / divider;
			result.X = value1.X * factor;
			result.Y = value1.Y * factor;
		}

		public static float Dot(CCVector2 value1, CCVector2 value2)
		{
			return (value1.X * value2.X) + (value1.Y * value2.Y);
		}

		public static void Dot(ref CCVector2 value1, ref CCVector2 value2, out float result)
		{
			result = (value1.X * value2.X) + (value1.Y * value2.Y);
		}

		public override bool Equals(object obj)
		{
			if(obj is CCVector2)
			{
				return Equals((CCVector2)obj);
			}

			return false;
		}

		public bool Equals(CCVector2 other)
		{
			return (X == other.X) && (Y == other.Y);
		}

		public static CCVector2 Reflect(CCVector2 vector, CCVector2 normal)
		{
			CCVector2 result;
			float val = 2.0f * ((vector.X * normal.X) + (vector.Y * normal.Y));
			result.X = vector.X - (normal.X * val);
			result.Y = vector.Y - (normal.Y * val);
			return result;
		}

		public static void Reflect(ref CCVector2 vector, ref CCVector2 normal, out CCVector2 result)
		{
			float val = 2.0f * ((vector.X * normal.X) + (vector.Y * normal.Y));
			result.X = vector.X - (normal.X * val);
			result.Y = vector.Y - (normal.Y * val);
		}

		public override int GetHashCode()
		{
			return X.GetHashCode() + Y.GetHashCode();
		}

		public static CCVector2 Hermite(CCVector2 value1, CCVector2 tangent1, CCVector2 value2, CCVector2 tangent2, float amount)
		{
			CCVector2 result = new CCVector2();
			Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
			return result;
		}

		public static void Hermite(ref CCVector2 value1, ref CCVector2 tangent1, ref CCVector2 value2, ref CCVector2 tangent2, float amount, out CCVector2 result)
		{
			result.X = MathHelper.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
			result.Y = MathHelper.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
		}

		public float Length()
		{
			return (float)Math.Sqrt((X * X) + (Y * Y));
		}

		public float LengthSquared()
		{
			return (X * X) + (Y * Y);
		}

		public static CCVector2 Lerp(CCVector2 value1, CCVector2 value2, float amount)
		{
			return new CCVector2(
				MathHelper.Lerp(value1.X, value2.X, amount),
				MathHelper.Lerp(value1.Y, value2.Y, amount));
		}

		public static void Lerp(ref CCVector2 value1, ref CCVector2 value2, float amount, out CCVector2 result)
		{
			result = new CCVector2(
				MathHelper.Lerp(value1.X, value2.X, amount),
				MathHelper.Lerp(value1.Y, value2.Y, amount));
		}

		public static CCVector2 Max(CCVector2 value1, CCVector2 value2)
		{
			return new CCVector2(value1.X > value2.X ? value1.X : value2.X, 
				value1.Y > value2.Y ? value1.Y : value2.Y);
		}

		public static void Max(ref CCVector2 value1, ref CCVector2 value2, out CCVector2 result)
		{
			result.X = value1.X > value2.X ? value1.X : value2.X;
			result.Y = value1.Y > value2.Y ? value1.Y : value2.Y;
		}

		public static CCVector2 Min(CCVector2 value1, CCVector2 value2)
		{
			return new CCVector2(value1.X < value2.X ? value1.X : value2.X, 
				value1.Y < value2.Y ? value1.Y : value2.Y); 
		}

		public static void Min(ref CCVector2 value1, ref CCVector2 value2, out CCVector2 result)
		{
			result.X = value1.X < value2.X ? value1.X : value2.X;
			result.Y = value1.Y < value2.Y ? value1.Y : value2.Y;
		}

		public static CCVector2 Multiply(CCVector2 value1, CCVector2 value2)
		{
			value1.X *= value2.X;
			value1.Y *= value2.Y;
			return value1;
		}

		public static CCVector2 Multiply(CCVector2 value1, float scaleFactor)
		{
			value1.X *= scaleFactor;
			value1.Y *= scaleFactor;
			return value1;
		}

		public static void Multiply(ref CCVector2 value1, float scaleFactor, out CCVector2 result)
		{
			result.X = value1.X * scaleFactor;
			result.Y = value1.Y * scaleFactor;
		}

		public static void Multiply(ref CCVector2 value1, ref CCVector2 value2, out CCVector2 result)
		{
			result.X = value1.X * value2.X;
			result.Y = value1.Y * value2.Y;
		}

		public static CCVector2 Negate(CCVector2 value)
		{
			value.X = -value.X;
			value.Y = -value.Y;
			return value;
		}

		public static void Negate(ref CCVector2 value, out CCVector2 result)
		{
			result.X = -value.X;
			result.Y = -value.Y;
		}

		public void Normalize()
		{
			float val = 1.0f / (float)Math.Sqrt((X * X) + (Y * Y));
			X *= val;
			Y *= val;
		}

		public static CCVector2 Normalize(CCVector2 value)
		{
			float val = 1.0f / (float)Math.Sqrt((value.X * value.X) + (value.Y * value.Y));
			value.X *= val;
			value.Y *= val;
			return value;
		}

		public static void Normalize(ref CCVector2 value, out CCVector2 result)
		{
			float val = 1.0f / (float)Math.Sqrt((value.X * value.X) + (value.Y * value.Y));
			result.X = value.X * val;
			result.Y = value.Y * val;
		}

		public static CCVector2 SmoothStep(CCVector2 value1, CCVector2 value2, float amount)
		{
			return new CCVector2(
				MathHelper.SmoothStep(value1.X, value2.X, amount),
				MathHelper.SmoothStep(value1.Y, value2.Y, amount));
		}

		public static void SmoothStep(ref CCVector2 value1, ref CCVector2 value2, float amount, out CCVector2 result)
		{
			result = new CCVector2(
				MathHelper.SmoothStep(value1.X, value2.X, amount),
				MathHelper.SmoothStep(value1.Y, value2.Y, amount));
		}

		public static CCVector2 Subtract(CCVector2 value1, CCVector2 value2)
		{
			value1.X -= value2.X;
			value1.Y -= value2.Y;
			return value1;
		}

		public static void Subtract(ref CCVector2 value1, ref CCVector2 value2, out CCVector2 result)
		{
			result.X = value1.X - value2.X;
			result.Y = value1.Y - value2.Y;
		}

		public static CCVector2 Transform(CCVector2 position, CCAffineTransform matrix)
		{
			Transform(ref position, ref matrix, out position);
			return position;
		}

		public static void Transform(ref CCVector2 position, ref CCAffineTransform affineTransform, out CCVector2 result)
		{
			result = new CCVector2((position.X * affineTransform.A) + (position.Y * affineTransform.C) + affineTransform.Tx,
				(position.X * affineTransform.B) + (position.Y * affineTransform.D) + affineTransform.Ty);
		}

		public static void Transform (
			CCVector2[] sourceArray,
			ref CCAffineTransform affineTransform,
			CCVector2[] destinationArray)
		{
			Transform(sourceArray, 0, ref affineTransform, destinationArray, 0, sourceArray.Length);
		}

		public static void Transform (
			CCVector2[] sourceArray,
			int sourceIndex,
			ref CCAffineTransform matrix,
			CCVector2[] destinationArray,
			int destinationIndex,
			int length)
		{
			for (int x = 0; x < length; x++) {
				var position = sourceArray[sourceIndex + x];
				var destination = destinationArray[destinationIndex + x];
				destination.X = (position.X * matrix.A) + (position.Y * matrix.C) + matrix.Tx;
				destination.Y = (position.X * matrix.B) + (position.Y * matrix.D) + matrix.Ty;
				destinationArray[destinationIndex + x] = destination;
			}
		}

		public static void TransformNormal(ref CCVector2 normal, ref CCAffineTransform affineTransform, out CCVector2 result)
		{
			result = new CCVector2((normal.X * affineTransform.A) + (normal.Y * affineTransform.C),
				(normal.X * affineTransform.B) + (normal.Y * affineTransform.D));
		}

		public override string ToString()
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			return string.Format(currentCulture, "{{X:{0} Y:{1}}}", new object[] { 
				this.X.ToString(currentCulture), this.Y.ToString(currentCulture) });
		}

		#endregion Public Methods


		#region Operators

		public static CCVector2 operator -(CCVector2 value)
		{
			value.X = -value.X;
			value.Y = -value.Y;
			return value;
		}


		public static bool operator ==(CCVector2 value1, CCVector2 value2)
		{
			return value1.X == value2.X && value1.Y == value2.Y;
		}


		public static bool operator !=(CCVector2 value1, CCVector2 value2)
		{
			return value1.X != value2.X || value1.Y != value2.Y;
		}


		public static CCVector2 operator +(CCVector2 value1, CCVector2 value2)
		{
			value1.X += value2.X;
			value1.Y += value2.Y;
			return value1;
		}


		public static CCVector2 operator -(CCVector2 value1, CCVector2 value2)
		{
			value1.X -= value2.X;
			value1.Y -= value2.Y;
			return value1;
		}


		public static CCVector2 operator *(CCVector2 value1, CCVector2 value2)
		{
			value1.X *= value2.X;
			value1.Y *= value2.Y;
			return value1;
		}


		public static CCVector2 operator *(CCVector2 value, float scaleFactor)
		{
			value.X *= scaleFactor;
			value.Y *= scaleFactor;
			return value;
		}


		public static CCVector2 operator *(float scaleFactor, CCVector2 value)
		{
			value.X *= scaleFactor;
			value.Y *= scaleFactor;
			return value;
		}


		public static CCVector2 operator /(CCVector2 value1, CCVector2 value2)
		{
			value1.X /= value2.X;
			value1.Y /= value2.Y;
			return value1;
		}


		public static CCVector2 operator /(CCVector2 value1, float divider)
		{
			float factor = 1 / divider;
			value1.X *= factor;
			value1.Y *= factor;
			return value1;
		}

		#endregion Operators
	}

}