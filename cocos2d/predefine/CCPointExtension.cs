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
    public class CCPointExtension
    {
        /// <summary>
        /// Helper macro that creates a CCPoint 
        /// since v0.7.2
        /// </summary>
        /// <returns>CCPoint</returns>
        public static CCPoint ccp(float x, float y)
        {
            return new CCPoint(x, y);
        }

        /// <summary>
        /// Returns opposite of point.
        /// since v0.7.2
        /// </summary>
        /// <returns>CCPoint</returns>
        public static CCPoint ccpNeg(CCPoint v)
        {
            return ccp(-v.x, -v.y);
        }

        /// <summary>
        /// Calculates sum of two points.
        ///@since v0.7.2
        /// <returns>CCPoint</returns>
        public static CCPoint ccpAdd(CCPoint v1, CCPoint v2)
        {
            return ccp(v1.x + v2.x, v1.y + v2.y);
        }

        /** Calculates difference of two points.
            @return CCPoint
            @since v0.7.2
        */
        public static CCPoint ccpSub(CCPoint v1, CCPoint v2)
        {
            return ccp(v1.x - v2.x, v1.y - v2.y);
        }

        /** Returns point multiplied by given factor.
            @return CCPoint
            @since v0.7.2
        */
        public static CCPoint ccpMult(CCPoint v, float s)
        {
            return ccp(v.x * s, v.y * s);
        }

        /** Calculates midpoint between two points.
            @return CCPoint
            @since v0.7.2
        */
        public static CCPoint ccpMidpoint(CCPoint v1, CCPoint v2)
        {
            return ccpMult(ccpAdd(v1, v2), 0.5f);
        }

        /** Calculates dot product of two points.
            @return CGFloat
            @since v0.7.2
        */
        public static float ccpDot(CCPoint v1, CCPoint v2)
        {
            return v1.x * v2.x + v1.y * v2.y;
        }

        /** Calculates cross product of two points.
            @return CGFloat
            @since v0.7.2
        */
        public static float ccpCross(CCPoint v1, CCPoint v2)
        {
            return v1.x * v2.y - v1.y * v2.x;
        }

        /** Calculates perpendicular of v, rotated 90 degrees counter-clockwise -- cross(v, perp(v)) >= 0
            @return CCPoint
            @since v0.7.2
        */
        public static CCPoint ccpPerp(CCPoint v)
        {
            return ccp(-v.y, v.x);
        }

        /** Calculates perpendicular of v, rotated 90 degrees clockwise -- cross(v, rperp(v)) <= 0
            @return CCPoint
            @since v0.7.2
        */
        public static CCPoint ccpRPerp(CCPoint v)
        {
            return ccp(v.y, -v.x);
        }

        /** Calculates the projection of v1 over v2.
            @return CCPoint
            @since v0.7.2
        */
        public static CCPoint ccpProject(CCPoint v1, CCPoint v2)
        {
            return ccpMult(v2, ccpDot(v1, v2) / ccpDot(v2, v2));
        }

        /** Rotates two points.
            @return CCPoint
            @since v0.7.2
        */
        public static CCPoint ccpRotate(CCPoint v1, CCPoint v2)
        {
            return ccp(v1.x * v2.x - v1.y * v2.y, v1.x * v2.y + v1.y * v2.x);
        }

        /** Unrotates two points.
            @return CCPoint
            @since v0.7.2
        */
        public static CCPoint ccpUnrotate(CCPoint v1, CCPoint v2)
        {
            return ccp(v1.x * v2.x + v1.y * v2.y, v1.y * v2.x - v1.x * v2.y);
        }

        /** Calculates the square length of a CCPoint (not calling sqrt() )
            @return CGFloat
            @since v0.7.2
        */
        public static float ccpLengthSQ(CCPoint v)
        {
            return ccpDot(v, v);
        }

        /** Calculates distance between point an origin
            @return CGFloat
            @since v0.7.2
        */
        public static float ccpLength(CCPoint v)
        {
            return (float)Math.Sqrt(ccpLengthSQ(v));
        }

        /** Calculates the distance between two points
            @return CGFloat
            @since v0.7.2
        */
        public static float ccpDistance(CCPoint v1, CCPoint v2)
        {
            return ccpLength(ccpSub(v1, v2));
        }

        /** Returns point multiplied to a length of 1.
            @return CCPoint
            @since v0.7.2
        */
        public static CCPoint ccpNormalize(CCPoint v)
        {
            return ccpMult(v, 1.0f / ccpLength(v));
        }

        /** Converts radians to a normalized vector.
            @return CCPoint
            @since v0.7.2
        */
        public static CCPoint ccpForAngle(float a)
        {
            return ccp((float)Math.Cos(a), (float)Math.Sin(a));
        }

        /** Converts a vector to radians.
            @return CGFloat
            @since v0.7.2
        */
        public static float ccpToAngle(CCPoint v)
        {
            return (float)Math.Atan2(v.y, v.x);
        }


        /** Clamp a value between from and to.
            @since v0.99.1
        */
        public static float clampf(float value, float min_inclusive, float max_inclusive)
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
        public static CCPoint ccpClamp(CCPoint p, CCPoint from, CCPoint to)
        {
            return ccp(clampf(p.x, from.x, to.x), clampf(p.y, from.y, to.y));
        }

        /** Quickly convert CCSize to a CCPoint
            @since v0.99.1
        */
        public static CCPoint ccpFromSize(CCSize s)
        {
            return ccp(s.width, s.height);
        }

        /** Run a math operation function on each point component
         * absf, fllorf, ceilf, roundf
         * any function that has the signature: float func(float);
         * For example: let's try to take the floor of x,y
         * ccpCompOp(p,floorf);
         @since v0.99.1
         */
        public delegate float ccpCompOpDelegate(float a);
        public static CCPoint ccpCompOp(CCPoint p, ccpCompOpDelegate del)
        {
            return ccp(del(p.x), del(p.y));
        }

        /** Linear Interpolation between two points a and b
            @returns
              alpha == 0 ? a
              alpha == 1 ? b
              otherwise a value between a..b
            @since v0.99.1
       */
        public static CCPoint ccpLerp(CCPoint a, CCPoint b, float alpha)
        {
            return ccpAdd(ccpMult(a, 1.0f - alpha), ccpMult(b, alpha));
        }


        /** @returns if points have fuzzy equality which means equal with some degree of variance.
            @since v0.99.1
        */
        public static bool ccpFuzzyEqual(CCPoint a, CCPoint b, float variance)
        {
            if (a.x - variance <= b.x && b.x <= a.x + variance)
                if (a.y - variance <= b.y && b.y <= a.y + variance)
                    return true;

            return false;
        }


        /** Multiplies a nd b components, a.x*b.x, a.y*b.y
            @returns a component-wise multiplication
            @since v0.99.1
        */
        public static CCPoint ccpCompMult(CCPoint a, CCPoint b)
        {
            return ccp(a.x * b.x, a.y * b.y);
        }

        /** @returns the signed angle in radians between two vector directions
            @since v0.99.1
        */
        public static float ccpAngleSigned(CCPoint a, CCPoint b)
        {
            CCPoint a2 = ccpNormalize(a);
            CCPoint b2 = ccpNormalize(b);
            float angle = (float)Math.Atan2(a2.x * b2.y - a2.y * b2.x, ccpDot(a2, b2));

            if (Math.Abs(angle) < ccMacros.FLT_EPSILON)
            {
                return 0.0f;
            }

            return angle;
        }

        /** @returns the angle in radians between two vector directions
            @since v0.99.1
        */
        public static float ccpAngle(CCPoint a, CCPoint b)
        {
            float angle = (float)Math.Acos(ccpDot(ccpNormalize(a), ccpNormalize(b)));

            if (Math.Abs(angle) < ccMacros.FLT_EPSILON)
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
        public static CCPoint ccpRotateByAngle(CCPoint v, CCPoint pivot, float angle)
        {
            CCPoint r = ccpSub(v, pivot);
            float cosa = (float)Math.Cos(angle), sina = (float)Math.Sin(angle);
            float t = r.x;

            r.x = t * cosa - r.y * sina + pivot.x;
            r.y = t * sina + r.y * cosa + pivot.y;

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
        public static bool ccpLineIntersect(CCPoint A, CCPoint B, CCPoint C, CCPoint D, ref float S, ref float T)
        {
            // FAIL: Line undefined
            if ((A.x == B.x && A.y == B.y) || (C.x == D.x && C.y == D.y))
            {
                return false;
            }

            float BAx = B.x - A.x;
            float BAy = B.y - A.y;
            float DCx = D.x - C.x;
            float DCy = D.y - C.y;
            float ACx = A.x - C.x;
            float ACy = A.y - C.y;

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
        public static bool ccpSegmentIntersect(CCPoint A, CCPoint B, CCPoint C, CCPoint D)
        {
            float S = 0, T = 0;

            if (ccpLineIntersect(A, B, C, D, ref S, ref T)
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
        public static CCPoint ccpIntersectPoint(CCPoint A, CCPoint B, CCPoint C, CCPoint D)
        {
            float S = 0, T = 0;

            if (ccpLineIntersect(A, B, C, D, ref S, ref T))
            {
                // Point of intersection
                CCPoint P = new CCPoint();
                P.x = A.x + S * (B.x - A.x);
                P.y = A.y + S * (B.y - A.y);
                return P;
            }

            return new CCPoint();
        }
    }
}
