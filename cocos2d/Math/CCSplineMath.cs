namespace CocosSharp
{
    public static class CCSplineMath
    {
        // CatmullRom Spline formula:
        /// <summary>
        /// See http://en.wikipedia.org/wiki/Cubic_Hermite_spline#Cardinal_spline
        /// </summary>
        /// <param name="p0">Control point 1</param>
        /// <param name="p1">Control point 2</param>
        /// <param name="p2">Control point 3</param>
        /// <param name="p3">Control point 4</param>
        /// <param name="tension"> The parameter c is a tension parameter that must be in the interval (0,1). In some sense, this can be interpreted as the "length" of the tangent. c=1 will yield all zero tangents, and c=0 yields a Catmull–Rom spline.</param>
        /// <param name="t">Time along the spline</param>
        /// <returns>The point along the spline for the given time (t)</returns>
        public static CCPoint CCCardinalSplineAt(CCPoint p0, CCPoint p1, CCPoint p2, CCPoint p3, float tension, float t)
        {
            if (tension < 0f)
            {
                tension = 0f;
            }
            if (tension > 1f)
            {
                tension = 1f;
            }
            float t2 = t * t;
            float t3 = t2 * t;

            /*
             * Formula: s(-ttt + 2tt - t)P1 + s(-ttt + tt)P2 + (2ttt - 3tt + 1)P2 + s(ttt - 2tt + t)P3 + (-2ttt + 3tt)P3 + s(ttt - tt)P4
             */
            float s = (1 - tension) / 2;

            float b1 = s * ((-t3 + (2 * t2)) - t); // s(-t3 + 2 t2 - t)P1
            float b2 = s * (-t3 + t2) + (2 * t3 - 3 * t2 + 1); // s(-t3 + t2)P2 + (2 t3 - 3 t2 + 1)P2
            float b3 = s * (t3 - 2 * t2 + t) + (-2 * t3 + 3 * t2); // s(t3 - 2 t2 + t)P3 + (-2 t3 + 3 t2)P3
            float b4 = s * (t3 - t2); // s(t3 - t2)P4

            float x = (p0.X * b1 + p1.X * b2 + p2.X * b3 + p3.X * b4);
            float y = (p0.Y * b1 + p1.Y * b2 + p2.Y * b3 + p3.Y * b4);

            return new CCPoint(x, y);
        }

        // Bezier cubic formula:
        //	((1 - t) + t)3 = 1 
        // Expands to 
        //   (1 - t)3 + 3t(1-t)2 + 3t2(1 - t) + t3 = 1 
        public static float CubicBezier(float a, float b, float c, float d, float t)
        {
            float t1 = 1f - t;
            return ((t1 * t1 * t1) * a + 3f * t * (t1 * t1) * b + 3f * (t * t) * (t1) * c + (t * t * t) * d);
        }

        public static float QuadBezier(float a, float b, float c, float t)
        {
            float t1 = 1f - t;
            return (t1 * t1) * a + 2.0f * (t1) * t * b + (t * t) * c;
            
        }
    }
}