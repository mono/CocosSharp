using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Box2D;
using Box2D.Common;

namespace Cocos2D
{
    
    /**
     * There are two Elliptical Arc routines included here that are controlled by the
     * compiler settings MAISONABE.
     * 
     * The normal and default functionality is a conversion from the libgdip library make_arcs.
     * From everything I can tell from the code is that it is based more or less on Riskus
     * http://itc.ktu.lt/itc354/Riskus354.pdf where you handle each quadrant of 90 degrees 
     * separately.  
     * 
     * I also added to the make_arcs routine a parameter to control whether it should
     * draw the arcs as pie slices or not.  This will draw a line from the center to the starting arc
     * and then close the path.
     * 
     * The other is another I have used before from L. Maisonobe and just wanted to include it.
     * More info can be read about it at the links included in the comments.  It has it's benefits
     * so may be usable.  Read more in the comments.  It is messy though with a lot of constants
     * used for error estimation.
     * 
     * I like the implementation because you can select the degree as well error threshold for 
     * different graphics implementations.
     * 1st degree - Uses Line Segments
     * 2nd degree - Uses Quadratic Bezier
     * 3rd degree - Uses Cubic Bezier
     * 
     **/
    public partial class CCDrawingPrimitives 
    {

#if MAISONABE
        const int SEGMENTS = 1;
#else
        const int SEGMENTS = 50;
#endif

#if MAISONABE
        /**
         * Based on the information at:
         * http://www.spaceroots.org/documents/ellipse/node2.html 
         * or http://www.spaceroots.org/documents/ellipse/elliptical-arc.pdf
         * 
         * and code is based off of the class http://www.spaceroots.org/documents/ellipse/EllipticalArc.java
        * <p>This class differs from the code used in libgdip
        * in the fact it can handle parts of ellipse in addition to full
        * ellipses and it can handle ellipses which are not aligned with the
        * x and y reference axes of the plane. <p>

        * <p>Another improvement is that this class can handle degenerated
        * cases like for example very flat ellipses (semi-minor axis much
        * smaller than semi-major axis) and drawing of very small parts of
        * such ellipses at very high magnification scales. This imply
        * monitoring the drawing approximation error for extremely small
        * values. Such cases occur for example while drawing orbits of comets
        * near the perihelion.</p>

        * <p>When the arc does not cover the complete ellipse, the lines
        * joining the center of the ellipse to the endpoints can optionally
        * be included or not in the outline, hence allowing to use it for
        * pie-charts rendering. If these lines are not included, the curve is
        * not naturally closed.</p>
            
        * @author L. Maisonobe
        **/
        
        // A little pre computation here
        private const double twoPi = 2 * Math.PI;
        
        // coefficients for error estimation
        // while using quadratic Bézier curves for approximation
        // 0 < b/a < 1/4
        private static readonly double[][][] coeffs2Low = new double[][][] {
            new double[][] {
                new double[] {  3.92478,   -13.5822,     -0.233377,    0.0128206   },
                new double[] { -1.08814,     0.859987,    0.000362265, 0.000229036 },
                new double[] { -0.942512,    0.390456,    0.0080909,   0.00723895  },
                new double[] { -0.736228,    0.20998,     0.0129867,   0.0103456   }
            }, new double[][] {
                new double[] { -0.395018,    6.82464,     0.0995293,   0.0122198   },
                new double[] { -0.545608,    0.0774863,   0.0267327,   0.0132482   },
                new double[] {  0.0534754,  -0.0884167,   0.012595,    0.0343396   },
                new double[] {  0.209052,   -0.0599987,  -0.00723897,  0.00789976  }
            }
        };
        
        // coefficients for error estimation
        // while using quadratic Bézier curves for approximation
        // 1/4 <= b/a <= 1
        private static readonly double[][][] coeffs2High = new double[][][] {
            new double[][] {
                new double[]{  0.0863805, -11.5595,     -2.68765,     0.181224    },
                new double[]{  0.242856,   -1.81073,     1.56876,     1.68544     },
                new double[]{  0.233337,   -0.455621,    0.222856,    0.403469    },
                new double[]{  0.0612978,  -0.104879,    0.0446799,   0.00867312  }
            }, new double[][] {
                new double[]{  0.028973,    6.68407,     0.171472,    0.0211706   },
                new double[]{  0.0307674,  -0.0517815,   0.0216803,  -0.0749348   },
                new double[]{ -0.0471179,   0.1288,     -0.0781702,   2.0         },
                new double[]{ -0.0309683,   0.0531557,  -0.0227191,   0.0434511   }
            }
        };
        
        // safety factor to convert the "best" error approximation
        // into a "max bound" error
        private static readonly double[] safety2 = new double[] { 0.02, 2.83, 0.125, 0.01 };
        
        // coefficients for error estimation
        // while using cubic Bézier curves for approximation
        // 0 < b/a < 1/4
        private static readonly double[][][] coeffs3Low = new double[][][] {
            new double[][] {
                new double[] {  3.85268,   -21.229,      -0.330434,    0.0127842  },
                new double[] { -1.61486,     0.706564,    0.225945,    0.263682   },
                new double[] { -0.910164,    0.388383,    0.00551445,  0.00671814 },
                new double[] { -0.630184,    0.192402,    0.0098871,   0.0102527  }
            }, new double[][] {
                new double[] { -0.162211,    9.94329,     0.13723,     0.0124084  },
                new double[] { -0.253135,    0.00187735,  0.0230286,   0.01264    },
                new double[] { -0.0695069,  -0.0437594,   0.0120636,   0.0163087  },
                new double[] { -0.0328856,  -0.00926032, -0.00173573,  0.00527385 }
            }
        };
        
        // coefficients for error estimation
        // while using cubic Bézier curves for approximation
        // 1/4 <= b/a <= 1
        private static readonly double[][][] coeffs3High = new double[][][] {
            new double[][]{
                new double[]{  0.0899116, -19.2349,     -4.11711,     0.183362   },
                new double[]{  0.138148,   -1.45804,     1.32044,     1.38474    },
                new double[]{  0.230903,   -0.450262,    0.219963,    0.414038   },
                new double[]{  0.0590565,  -0.101062,    0.0430592,   0.0204699  }
            }, new double[][]{
                new double[]{  0.0164649,   9.89394,     0.0919496,   0.00760802 },
                new double[]{  0.0191603,  -0.0322058,   0.0134667,  -0.0825018  },
                new double[]{  0.0156192,  -0.017535,    0.00326508, -0.228157   },
                new double[]{ -0.0236752,   0.0405821,  -0.0173086,   0.176187   }
            }
        };
        
        // safety factor to convert the "best" error approximation
        // into a "max bound" error
        private static double[] safety3 = new double[] { 0.001, 4.98, 0.207, 0.0067 };
        
        /** Abscissa of the center of the ellipse. */
        private static double cx;
        
        /** Ordinate of the center of the ellipse. */
        private static double cy;
        
        /** Semi-major axis. */
        private static double a;
        
        /** Semi-minor axis. */
        private static double b;
        
        /** Orientation of the major axis with respect to the x axis. */
        private static double theta;
        private static double cosTheta;
        private static double sinTheta;
        
        /** Start angle of the arc. */
        protected static double eta1;
        
        /** End angle of the arc. */
        protected static double eta2;
        
        /** Abscissa of the first focus. */
        protected static double xF1;
        
        /** Ordinate of the first focus. */
        protected static double yF1;
        
        /** Abscissa of the second focus. */
        protected static double xF2;
        
        /** Ordinate of the second focus. */
        protected static double yF2;
        
        /** Indicator for center to endpoints line inclusion. */
        protected static bool isPieSlice;
        
        /** Build an elliptical arc from its canonical geometrical elements.
        * @param center center of the ellipse
        * @param a semi-major axis
        * @param b semi-minor axis
        * @param theta orientation of the major axis with respect to the x axis
        * @param lambda1 start angle of the arc
        * @param lambda2 end angle of the arc
        * @param isPieSlice if true, the lines between the center of the ellipse
        * and the endpoints are part of the shape (it is pie slice like)
        */
        internal static void DrawEllipticalArc(CCPoint center, double a, double b,
                                      double theta, double lambda1, double lambda2,
                                               bool isPieSlice, CCColor4B color)
        {
            DrawEllipticalArc(center.X, center.Y, a, b, theta, lambda1, lambda2, isPieSlice, color);
            
        }
#endif      

        internal static void DrawEllipticalArc(CCRect arcRect, double lambda1, double lambda2,
                                               bool isPieSlice, CCColor4B color)
        {
#if MAISONABE
            DrawEllipticalArc(arcRect.Origin.X + arcRect.Size.Width / 2,
                              arcRect.Origin.Y + arcRect.Size.Height / 2,
                              arcRect.Size.Width / 2,
                              arcRect.Size.Height / 2, 0, lambda1, lambda2, isPieSlice, color
                              );
#else
            make_arcs( 
                      arcRect.Origin.X, arcRect.Origin.Y, arcRect.Size.Width, arcRect.Size.Height, 
                      (float)lambda1, (float)lambda2, 
                      false, true, isPieSlice, color);
#endif
        }
        

        internal static void DrawEllipticalArc(float x, float y, float width, float height, double lambda1, double lambda2,
                                               bool isPieSlice, CCColor4B color)
        {
#if MAISONABE
            DrawEllipticalArc(x + width / 2,
                              y + height / 2,
                              width / 2,
                              height / 2, 0, lambda1, lambda2, isPieSlice, color
                              );
#else       
            make_arcs( 
                      x, y, width, height, 
                      (float)lambda1, (float)lambda2, 
                      false, true, isPieSlice, color);
#endif
        }       
        
#if MAISONABE
        
        /** Build an elliptical arc from its canonical geometrical elements.
        * @param cx abscissa of the center of the ellipse
        * @param cy ordinate of the center of the ellipse
        * @param a semi-major axis
        * @param b semi-minor axis
        * @param theta orientation of the major axis with respect to the x axis
        * @param lambda1 start angle of the arc
        * @param lambda2 end angle of the arc
        * @param isPieSlice if true, the lines between the center of the ellipse
        * and the endpoints are part of the shape (it is pie slice like)
        */
        internal static void DrawEllipticalArc(double centerX, double centerY, double axisA, double axisB,
                                        double thetaOrientation, double lambda1, double lambda2,
                                               bool isPie, CCColor4B color)
        {
            cx = centerX;
            cy = centerY;
            a = axisA;
            b = axisB;
            theta = thetaOrientation;
            isPieSlice = isPie;
            
            // Angles in radians
            lambda1 = lambda1 * Math.PI / 180;
            lambda2 = lambda2 * Math.PI / 180;
            
            // Handle negative sweep angles
            if (lambda2 < 0)
            {
                var temp = lambda1;
                lambda1 += lambda2;
                lambda2 = temp;
                
            }
            else
                lambda2 += lambda1;
            
            eta1 = Math.Atan2(Math.Sin(lambda1) / b,
                              Math.Cos(lambda1) / a);
            eta2 = Math.Atan2(Math.Sin(lambda2) / b,
                              Math.Cos(lambda2) / a);
            cosTheta = Math.Cos(theta);
            sinTheta = Math.Sin(theta);
            
            // make sure we have eta1 <= eta2 <= eta1 + 2 PI
            eta2 -= twoPi * Math.Floor((eta2 - eta1) / twoPi);
            
            // the preceding correction fails if we have exactly et2 - eta1 = 2 PI
            // it reduces the interval to zero length
            if ((lambda2 - lambda1 > Math.PI) && (eta2 - eta1 < Math.PI))
            {
                eta2 += 2 * Math.PI;
            }
            
            computeFocii();
            
            // NOTE: Max degrees handled by the routine is 3 
            drawEllipticalArcToContext(3, 0, color);
        }
        
        /** Compute the locations of the focii. */
        private static void computeFocii()
        {
            
            double d = Math.Sqrt(a * a - b * b);
            double dx = d * cosTheta;
            double dy = d * sinTheta;
            
            xF1 = cx - dx;
            yF1 = cy - dy;
            xF2 = cx + dx;
            yF2 = cy + dy;
            
        }
        
        /** Compute the value of a rational function.
        * This method handles rational functions where the numerator is
        * quadratic and the denominator is linear
        * @param x absissa for which the value should be computed
        * @param c coefficients array of the rational function
        */
        private static double rationalFunction(double x, double[] c)
        {
            return (x * (x * c[0] + c[1]) + c[2]) / (x + c[3]);
        }
        
        /** Estimate the approximation error for a sub-arc of the instance.
        * @param degree degree of the Bézier curve to use (1, 2 or 3)
        * @param tA start angle of the sub-arc
        * @param tB end angle of the sub-arc
        * @return upper bound of the approximation error between the Bézier
        * curve and the real ellipse
        */
        private static double estimateError(int degree, double etaA, double etaB)
        {
            
            double eta = 0.5 * (etaA + etaB);
            
            if (degree < 2)
            {
                
                // start point
                double aCosEtaA = a * Math.Cos(etaA);
                double bSinEtaA = b * Math.Sin(etaA);
                double xA = cx + aCosEtaA * cosTheta - bSinEtaA * sinTheta;
                double yA = cy + aCosEtaA * sinTheta + bSinEtaA * cosTheta;
                
                // end point
                double aCosEtaB = a * Math.Cos(etaB);
                double bSinEtaB = b * Math.Sin(etaB);
                double xB = cx + aCosEtaB * cosTheta - bSinEtaB * sinTheta;
                double yB = cy + aCosEtaB * sinTheta + bSinEtaB * cosTheta;
                
                // maximal error point
                double aCosEta = a * Math.Cos(eta);
                double bSinEta = b * Math.Sin(eta);
                double x = cx + aCosEta * cosTheta - bSinEta * sinTheta;
                double y = cy + aCosEta * sinTheta + bSinEta * cosTheta;
                
                double dx = xB - xA;
                double dy = yB - yA;
                
                return Math.Abs(x * dy - y * dx + xB * yA - xA * yB)
                    / Math.Sqrt(dx * dx + dy * dy);
                
            }
            else
            {
                
                double x = b / a;
                double dEta = etaB - etaA;
                double cos2 = Math.Cos(2 * eta);
                double cos4 = Math.Cos(4 * eta);
                double cos6 = Math.Cos(6 * eta);
                
                // select the right coeficients set according to degree and b/a
                double[][][] coeffs;
                double[] safety;
                if (degree == 2)
                {
                    coeffs = (x < 0.25) ? coeffs2Low : coeffs2High;
                    safety = safety2;
                }
                else
                {
                    coeffs = (x < 0.25) ? coeffs3Low : coeffs3High;
                    safety = safety3;
                }
                
                double c0 = rationalFunction(x, coeffs[0][0])
                    + cos2 * rationalFunction(x, coeffs[0][1])
                        + cos4 * rationalFunction(x, coeffs[0][2])
                        + cos6 * rationalFunction(x, coeffs[0][3]);
                
                double c1 = rationalFunction(x, coeffs[1][0])
                    + cos2 * rationalFunction(x, coeffs[1][1])
                        + cos4 * rationalFunction(x, coeffs[1][2])
                        + cos6 * rationalFunction(x, coeffs[1][3]);
                
                return rationalFunction(x, safety) * a * Math.Exp(c0 + c1 * dEta);
                
            }
            
        }
        
        /** Build an approximation of the instance outline.
        * @param degree degree of the Bézier curve to use
        * @param threshold acceptable error
        */
        public static void drawEllipticalArcToContext(int degree, double threshold, CCColor4B color)
        {
            
            // find the number of Bézier curves needed
            bool found = false;
            int n = 1;
            while ((!found) && (n < 1024))
            {
                double dEta2 = (eta2 - eta1) / n;
                if (dEta2 <= 0.5 * Math.PI)
                {
                    double etaB2 = eta1;
                    found = true;
                    for (int i = 0; found && (i < n); ++i)
                    {
                        double etaA = etaB2;
                        etaB2 += dEta2;
                        found = (estimateError(degree, etaA, etaB2) <= threshold);
                    }
                }
                n = n << 1;
            }
            
            double dEta = (eta2 - eta1) / n;
            double etaB = eta1;
            
            double cosEtaB = Math.Cos(etaB);
            double sinEtaB = Math.Sin(etaB);
            double aCosEtaB = a * cosEtaB;
            double bSinEtaB = b * sinEtaB;
            double aSinEtaB = a * sinEtaB;
            double bCosEtaB = b * cosEtaB;
            double xB = cx + aCosEtaB * cosTheta - bSinEtaB * sinTheta;
            double yB = cy + aCosEtaB * sinTheta + bSinEtaB * cosTheta;
            double xBDot = -aSinEtaB * cosTheta - bCosEtaB * sinTheta;
            double yBDot = -aSinEtaB * sinTheta + bCosEtaB * cosTheta;

            CCPoint startPoint = CCPoint.Zero;
            CCPoint piePoint = CCPoint.Zero;

            if (isPieSlice)
            {

                startPoint.X = (float)cx;
                startPoint.Y = (float)cy;
                piePoint.X = (float)cx;
                piePoint.Y = (float)cy;

            }
            else
            {
                startPoint.X = (float)xB;
                startPoint.Y = (float)yB;
            }
            
            double t = Math.Tan(0.5 * dEta);
            double alpha = Math.Sin(dEta) * (Math.Sqrt(4 + 3 * t * t) - 1) / 3;
 
            CCPoint destinationPoint = CCPoint.Zero;
            CCPoint controlPoint1 = CCPoint.Zero;
            CCPoint controlPoint2 = CCPoint.Zero;

            for (int i = 0; i < n; ++i)
            {
                
                //double etaA = etaB;
                double xA = xB;
                double yA = yB;
                double xADot = xBDot;
                double yADot = yBDot;
                
                etaB += dEta;
                cosEtaB = Math.Cos(etaB);
                sinEtaB = Math.Sin(etaB);
                aCosEtaB = a * cosEtaB;
                bSinEtaB = b * sinEtaB;
                aSinEtaB = a * sinEtaB;
                bCosEtaB = b * cosEtaB;
                xB = cx + aCosEtaB * cosTheta - bSinEtaB * sinTheta;
                yB = cy + aCosEtaB * sinTheta + bSinEtaB * cosTheta;
                xBDot = -aSinEtaB * cosTheta - bCosEtaB * sinTheta;
                yBDot = -aSinEtaB * sinTheta + bCosEtaB * cosTheta;
                
                destinationPoint.X = (float)xB;
                destinationPoint.Y = (float)yB;

                if (degree == 1)
                {

                    DrawLine(startPoint, destinationPoint, color);
                }
                else if (degree == 2)
                {
                    double k = (yBDot * (xB - xA) - xBDot * (yB - yA))
                        / (xADot * yBDot - yADot * xBDot);

                    controlPoint1.X = (float)(xA + k * xADot);
                    controlPoint1.Y = (float)(yA + k * yADot);

                    DrawQuadBezier(startPoint, controlPoint1, destinationPoint, SEGMENTS, color);
                }
                else
                {
                    controlPoint1.X = (float)(xA + alpha * xADot);
                    controlPoint1.Y = (float)(yA + alpha * yADot);

                    controlPoint2.X = (float)(xB - alpha * xBDot);
                    controlPoint2.Y = (float)(yB - alpha * yBDot);


                    DrawCubicBezier(startPoint, controlPoint1, controlPoint2, destinationPoint, SEGMENTS, color); 

                }

                startPoint.X = (float)xB;
                startPoint.Y = (float)yB;

            }
            
            if (isPieSlice)
            {

                DrawLine(piePoint, destinationPoint, color);
            }
            
        }
#endif


        static CCPoint startPoint = CCPoint.Zero;
        static CCPoint destinationPoint = CCPoint.Zero;
        static CCPoint controlPoint1 = CCPoint.Zero;
        static CCPoint controlPoint2 = CCPoint.Zero;


        /*
         * Based on the algorithm described in
         *      http://www.stillhq.com/ctpfaq/2002/03/c1088.html#AEN1212
         */
        static void
            make_arc(bool start, float x, float y, float width,
                     float height, float startAngle, float endAngle, bool antialiasing, bool isPieSlice, CCColor4B color)
        {
            float delta, bcp;
            double sin_alpha, sin_beta, cos_alpha, cos_beta;
            float PI = (float)Math.PI;
            
            float rx = width / 2;
            float ry = height / 2;
            
            /* center */
            float cx = x + rx;
            float cy = y + ry;
            
            /* angles in radians */
            float alpha = startAngle * PI / 180;
            float beta = endAngle * PI / 180;
            
            /* adjust angles for ellipses */
            alpha = (float)Math.Atan2(rx * Math.Sin(alpha), ry * Math.Cos(alpha));
            beta = (float)Math.Atan2(rx * Math.Sin(beta), ry * Math.Cos(beta));
            
            if (Math.Abs(beta - alpha) > PI)
            {
                if (beta > alpha)
                    beta -= 2 * PI;
                else
                    alpha -= 2 * PI;
            }
            
            delta = beta - alpha;
            bcp = (float)(4.0 / 3.0 * (1 - Math.Cos(delta / 2)) / Math.Sin(delta / 2));
            
            sin_alpha = Math.Sin(alpha);
            sin_beta = Math.Sin(beta);
            cos_alpha = Math.Cos(alpha);
            cos_beta = Math.Cos(beta);
            
            /* don't move to starting point if we're continuing an existing curve */
            if (start)
            {
                /* starting point */
                double sx = cx + rx * cos_alpha;
                double sy = cy + ry * sin_alpha;
                if (isPieSlice) 
                {
                    destinationPoint.X = (float)sx;
                    destinationPoint.Y = (float)sy;
                    
                    DrawLine(startPoint,destinationPoint,color);
                }

                startPoint.X = (float)sx;
                startPoint.Y = (float)sy;
            }

            destinationPoint.X = cx + rx * (float)cos_beta;
            destinationPoint.Y = cy + ry * (float)sin_beta;

            controlPoint1.X = cx + rx * (float)(cos_alpha - bcp * sin_alpha);
            controlPoint1.Y = cy + ry * (float)(sin_alpha + bcp * cos_alpha);
            
            controlPoint2.X = cx + rx * (float)(cos_beta + bcp * sin_beta);
            controlPoint2.Y = cy + ry * (float)(sin_beta - bcp * cos_beta);
            
            
            DrawCubicBezier(startPoint, controlPoint1, controlPoint2, destinationPoint, SEGMENTS, color); 

            startPoint.X = destinationPoint.X;
            startPoint.Y = destinationPoint.Y;
        }
        
        
        static void
            make_arcs(float x, float y, float width, float height, float startAngle, float sweepAngle,
                      bool convert_units, bool antialiasing, bool isPieSlice, CCColor4B color)
        {
            int i;
            float drawn = 0;
            float endAngle;
            bool enough = false;
            
            endAngle = startAngle + sweepAngle;
            /* if we end before the start then reverse positions (to keep increment positive) */
            if (endAngle < startAngle)
            {
                var temp = endAngle;
                endAngle = startAngle;
                startAngle = temp;
            }
            
            if (isPieSlice) {
                startPoint.X = x + (width / 2);
                startPoint.Y = y + (height / 2);
            }
            
            /* i is the number of sub-arcs drawn, each sub-arc can be at most 90 degrees.*/
            /* there can be no more then 4 subarcs, ie. 90 + 90 + 90 + (something less than 90) */
            for (i = 0; i < 4; i++)
            {
                float current = startAngle + drawn;
                float additional;
                
                if (enough) 
                {
                    if (isPieSlice) 
                    {
                        startPoint.X = x + (width / 2);
                        startPoint.Y = y + (height / 2);
                        DrawLine(destinationPoint, startPoint, color);
                    }
                    return;
                }
                
                additional = endAngle - current; /* otherwise, add the remainder */
                if (additional > 90)
                {
                    additional = 90.0f;
                }
                else
                {
                    /* a near zero value will introduce bad artefact in the drawing (#78999) */
                    if (( additional >= -0.0001f) && (additional <= 0.0001f))
                        return;
                    enough = true;
                }
                
                make_arc((i == 0),    /* only move to the starting pt in the 1st iteration */
                         x, y, width, height,   /* bounding rectangle */
                         current, current + additional, antialiasing, isPieSlice, color);
                
                drawn += additional;
                
            }
            
            if (isPieSlice) {
                startPoint.X = x + (width / 2);
                startPoint.Y = y + (height / 2);
                DrawLine(destinationPoint, startPoint, color);
            }
            
        }
        
    }
}