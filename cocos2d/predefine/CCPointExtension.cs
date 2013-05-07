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
namespace cocos2d
{
    [Obsolete("use CCPoint and CCPoint operator overloads - Will be deleted Aug 1, 2013")]
    public class CCPointExtension
    {
        /// <summary>
        /// Helper macro that creates a CCPoint 
        /// since v0.7.2
        /// </summary>
        /// <returns>CCPoint</returns>
        [Obsolete("use CCPoint()")]
        public static CCPoint CreatePoint(float x, float y)
        {
            return new CCPoint(x, y);
        }

        /// <summary>
        /// Returns opposite of point.
        /// since v0.7.2
        /// </summary>
        /// <returns>CCPoint</returns>
        [Obsolete("use CCPoint.Negative() or the unary operator")]
        public static CCPoint Negative(CCPoint v)
        {
            return CreatePoint(-v.X, -v.Y);
        }

        /// <summary>
        /// Calculates sum of two points.
        ///@since v0.7.2
        /// <returns>CCPoint</returns>
        [Obsolete("use CCPoint.Add() or the plus operator")]
        public static CCPoint Add(CCPoint v1, CCPoint v2)
        {
            return CreatePoint(v1.X + v2.X, v1.Y + v2.Y);
        }

        /** Calculates difference of two points.
            @return CCPoint
            @since v0.7.2
        */
        [Obsolete("use CCPoint() and the minus operator")]
        public static CCPoint Subtract(CCPoint v1, CCPoint v2)
        {
            return CreatePoint(v1.X - v2.X, v1.Y - v2.Y);
        }

        /** Returns point multiplied by given factor.
            @return CCPoint
            @since v0.7.2
        */
        [Obsolete("use CCPoint() and the multiply operator")]
        public static CCPoint Multiply(CCPoint v, float s)
        {
            return CreatePoint(v.X * s, v.Y * s);
        }

        /** Calculates midpoint between two points.
            @return CCPoint
            @since v0.7.2
        */
        [Obsolete("use CCPoint.Midpoint")]
        public static CCPoint Midpoint(CCPoint v1, CCPoint v2)
        {
            return Multiply(Add(v1, v2), 0.5f);
        }

        /** Calculates dot product of two points.
            @return CGFloat
            @since v0.7.2
        */
        [Obsolete("use CCPoint.DotProduct")]
        public static float DotProduct(CCPoint v1, CCPoint v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        /** Calculates cross product of two points.
            @return CGFloat
            @since v0.7.2
        */
        [Obsolete("Use CCPoint.CrossProduct")]
        public static float CrossProduct(CCPoint v1, CCPoint v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }

        /** Calculates perpendicular of v, rotated 90 degrees counter-clockwise -- cross(v, perp(v)) >= 0
            @return CCPoint
            @since v0.7.2
        */
        [Obsolete("Use CCPoint.PerpendicularCounterClockwise")]
        public static CCPoint PerpendicularCounterClockwise(CCPoint v)
        {
            return CreatePoint(-v.Y, v.X);
        }

        /** Calculates perpendicular of v, rotated 90 degrees clockwise -- cross(v, rperp(v)) <= 0
            @return CCPoint
            @since v0.7.2
        */
        [Obsolete("Use CCPoint.PerpendicularClockwise")]
        public static CCPoint PerpendicularClockwise(CCPoint v)
        {
            return CreatePoint(v.Y, -v.X);
        }

        /** Calculates the projection of v1 over v2.
            @return CCPoint
            @since v0.7.2
        */
        [Obsolete("Use CCPoint.Project")]
        public static CCPoint Project(CCPoint v1, CCPoint v2)
        {
            return Multiply(v2, DotProduct(v1, v2) / DotProduct(v2, v2));
        }

        /** Rotates two points.
            @return CCPoint
            @since v0.7.2
        */
        [Obsolete("Use CCPoint.Rotate")]
        public static CCPoint Rotate(CCPoint v1, CCPoint v2)
        {
            return CreatePoint(v1.X * v2.X - v1.Y * v2.Y, v1.X * v2.Y + v1.Y * v2.X);
        }

        /** Unrotates two points.
            @return CCPoint
            @since v0.7.2
        */
        [Obsolete("Use CCPoint.Unrotate")]
        public static CCPoint Unrotate(CCPoint v1, CCPoint v2)
        {
            return CreatePoint(v1.X * v2.X + v1.Y * v2.Y, v1.Y * v2.X - v1.X * v2.Y);
        }

        /** Calculates the square length of a CCPoint (not calling sqrt() )
            @return CGFloat
            @since v0.7.2
        */
        [Obsolete("Use CCPoint")]
        public static float LengthSquare(CCPoint v)
        {
            return DotProduct(v, v);
        }

        /** Calculates distance between point an origin
            @return CGFloat
            @since v0.7.2
        */
        [Obsolete("Use CCPoint")]
        public static float Length(CCPoint v)
        {
            return (float)Math.Sqrt(LengthSquare(v));
        }

        /** Calculates the distance between two points
            @return CGFloat
            @since v0.7.2
        */
        [Obsolete("Use CCPoint")]
        public static float Distance(CCPoint v1, CCPoint v2)
        {
            return Length(Subtract(v1, v2));
        }

        /** Returns point multiplied to a length of 1.
            @return CCPoint
            @since v0.7.2
        */
        [Obsolete("Use CCPoint")]
        public static CCPoint Normalize(CCPoint v)
        {
            return Multiply(v, 1.0f / Length(v));
        }

        /** Converts radians to a normalized vector.
            @return CCPoint
            @since v0.7.2
        */
        [Obsolete("Use CCPoint")]
        public static CCPoint ForAngle(float a)
        {
            return CreatePoint((float)Math.Cos(a), (float)Math.Sin(a));
        }

        /** Converts a vector to radians.
            @return CGFloat
            @since v0.7.2
        */
        [Obsolete("Use CCPoint")]
        public static float ToAngle(CCPoint v)
        {
            return (float)Math.Atan2(v.Y, v.X);
        }


        /** Clamp a value between from and to.
            @since v0.99.1
        */
        [Obsolete("Use CCPoint")]
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
        [Obsolete("Use CCPoint")]
        public static CCPoint Clamp(CCPoint p, CCPoint from, CCPoint to)
        {
            return CreatePoint(Clamp(p.X, from.X, to.X), Clamp(p.Y, from.Y, to.Y));
        }

        /** Quickly convert CCSize to a CCPoint
            @since v0.99.1
        */
        [Obsolete("Use CCPoint")]
        public static CCPoint FromSize(CCSize s)
        {
            return CreatePoint(s.Width, s.Height);
        }

        /** Run a math operation function on each point component
         * absf, fllorf, ceilf, roundf
         * any function that has the signature: float func(float);
         * For example: let's try to take the floor of x,y
         * ccpCompOp(p,floorf);
         @since v0.99.1
         */
        [Obsolete("Use CCPoint")]
        public delegate float ComputationOperationDelegate(float a);
        [Obsolete("Use CCPoint")]
        public static CCPoint ComputationOperation(CCPoint p, ComputationOperationDelegate del)
        {
            return CreatePoint(del(p.X), del(p.Y));
        }

        /** Linear Interpolation between two points a and b
            @returns
              alpha == 0 ? a
              alpha == 1 ? b
              otherwise a value between a..b
            @since v0.99.1
       */
        [Obsolete("Use CCPoint.Lerp")]
        public static CCPoint Lerp(CCPoint a, CCPoint b, float alpha)
        {
            return Add(Multiply(a, 1.0f - alpha), Multiply(b, alpha));
        }


        /** @returns if points have fuzzy equality which means equal with some degree of variance.
            @since v0.99.1
        */
        [Obsolete("Use CCPoint.FuzzyEqual")]
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
        [Obsolete("Use CCPoint.MultiplyComponents")]
        public static CCPoint MultiplyComponents(CCPoint a, CCPoint b)
        {
            return CreatePoint(a.X * b.X, a.Y * b.Y);
        }

        /** @returns the signed angle in radians between two vector directions
            @since v0.99.1
        */
        [Obsolete("Use CCPoint.AngleSigned")]
        public static float AngleSigned(CCPoint a, CCPoint b)
        {
            CCPoint a2 = Normalize(a);
            CCPoint b2 = Normalize(b);
            float angle = (float)Math.Atan2(a2.X * b2.Y - a2.Y * b2.X, DotProduct(a2, b2));

            if (Math.Abs(angle) < float.Epsilon)
            {
                return 0.0f;
            }

            return angle;
        }

        /** @returns the angle in radians between two vector directions
            @since v0.99.1
        */
        [Obsolete("Use CCPoint.Angle")]
        public static float Angle(CCPoint a, CCPoint b)
        {
            float angle = (float)Math.Acos(DotProduct(Normalize(a), Normalize(b)));

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
        [Obsolete("Use CCPoint.RotateByAngle")]
        public static CCPoint RotateByAngle(CCPoint v, CCPoint pivot, float angle)
        {
            CCPoint r = Subtract(v, pivot);
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
        [Obsolete("Use CCPoint.LineIntersect")]
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
        [Obsolete("Use CCPoint.SegmentIntersect")]
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
        [Obsolete("Use CCPoint.IntersectPoint")]
        public static CCPoint IntersectPoint(CCPoint A, CCPoint B, CCPoint C, CCPoint D)
        {
            float S = 0, T = 0;

            if (LineIntersect(A, B, C, D, ref S, ref T))
            {
                // Point of intersection
                CCPoint P = new CCPoint();
                P.X = A.X + S * (B.X - A.X);
                P.Y = A.Y + S * (B.Y - A.Y);
                return P;
            }

            return new CCPoint();
        }
    }
}
