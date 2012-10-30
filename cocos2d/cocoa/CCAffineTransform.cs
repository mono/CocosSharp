/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
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

namespace cocos2d
{
    public struct CCAffineTransform
    {
        public static readonly CCAffineTransform Identity = CCAffineTransformMake(1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f);

        public float a, b, c, d;
        public float tx, ty;

        public CCAffineTransform(float a, float b, float c, float d, float tx, float ty)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.tx = tx;
            this.ty = ty;
        }

        public static CCAffineTransform CCAffineTransformMakeIdentity()
        {
            return CCAffineTransformMake(1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f);
        }

        public static CCAffineTransform CCAffineTransformMake(float a, float b, float c, float d, float tx, float ty)
        {
            return new CCAffineTransform
                {
                    a = a,
                    b = b,
                    c = c,
                    d = d,
                    tx = tx,
                    ty = ty
                };
        }

        public static CCPoint CCPointApplyAffineTransform(CCPoint point, CCAffineTransform t)
        {
            return new CCPoint(
                t.a * point.x + t.c * point.y + t.tx,
                t.b * point.x + t.d * point.y + t.ty
                );
        }

        public static CCSize CCSizeApplyAffineTransform(CCSize size, CCAffineTransform t)
        {
            var s = new CCSize();
            s.width = (float) ((double) t.a * size.width + (double) t.c * size.height);
            s.height = (float) ((double) t.b * size.width + (double) t.d * size.height);
            return s;
        }

        public static CCRect CCRectApplyAffineTransform(CCRect rect, CCAffineTransform anAffineTransform)
        {
            float top = CCRect.CCRectGetMinY(rect);
            float left = CCRect.CCRectGetMinX(rect);
            float right = CCRect.CCRectGetMaxX(rect);
            float bottom = CCRect.CCRectGetMaxY(rect);

            CCPoint topLeft = CCPointApplyAffineTransform(new CCPoint(left, top), anAffineTransform);
            CCPoint topRight = CCPointApplyAffineTransform(new CCPoint(right, top), anAffineTransform);
            CCPoint bottomLeft = CCPointApplyAffineTransform(new CCPoint(left, bottom), anAffineTransform);
            CCPoint bottomRight = CCPointApplyAffineTransform(new CCPoint(right, bottom), anAffineTransform);

            float minX = Math.Min(Math.Min(topLeft.x, topRight.x), Math.Min(bottomLeft.x, bottomRight.x));
            float maxX = Math.Max(Math.Max(topLeft.x, topRight.x), Math.Max(bottomLeft.x, bottomRight.x));
            float minY = Math.Min(Math.Min(topLeft.y, topRight.y), Math.Min(bottomLeft.y, bottomRight.y));
            float maxY = Math.Max(Math.Max(topLeft.y, topRight.y), Math.Max(bottomLeft.y, bottomRight.y));

            return new CCRect(minX, minY, (maxX - minX), (maxY - minY));
        }

        public static CCAffineTransform CCAffineTransformTranslate(CCAffineTransform t, float tx, float ty)
        {
            return new CCAffineTransform(t.a, t.b, t.c, t.d, t.tx + t.a * tx + t.c * ty, t.ty + t.b * tx + t.d * ty);
        }

        public static CCAffineTransform CCAffineTransformRotate(CCAffineTransform t, float anAngle)
        {
            var fSin = (float) Math.Sin(anAngle);
            var fCos = (float) Math.Cos(anAngle);

            return new CCAffineTransform(t.a * fCos + t.c * fSin,
                                         t.b * fCos + t.d * fSin,
                                         t.c * fCos - t.a * fSin,
                                         t.d * fCos - t.b * fSin,
                                         t.tx,
                                         t.ty);
        }

        public static CCAffineTransform CCAffineTransformScale(CCAffineTransform t, float sx, float sy)
        {
            return new CCAffineTransform(t.a * sx, t.b * sx, t.c * sy, t.d * sy, t.tx, t.ty);
        }

        /// <summary>
        /// Concatenate `t2' to `t1' and return the result:
        /// t' = t1 * t2 */s
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static CCAffineTransform CCAffineTransformConcat(CCAffineTransform t1, CCAffineTransform t2)
        {
            return new CCAffineTransform(t1.a * t2.a + t1.b * t2.c, t1.a * t2.b + t1.b * t2.d, //a,b
                                         t1.c * t2.a + t1.d * t2.c, t1.c * t2.b + t1.d * t2.d, //c,d
                                         t1.tx * t2.a + t1.ty * t2.c + t2.tx, //tx
                                         t1.tx * t2.b + t1.ty * t2.d + t2.ty); //ty
        }

        public void Concat(CCAffineTransform m)
        {
            Concat(ref m);
        }

        public void Concat(ref CCAffineTransform m)
        {
            float t_a = a;
            float t_b = b;
            float t_c = c;
            float t_d = d;
            float t_tx = tx;
            float t_ty = ty;

            float m_a = m.a;
            float m_b = m.b;
            float m_c = m.c;
            float m_d = m.d;

            a = m_a * t_a + m_c * t_b;
            b = m_b * t_a + m_d * t_b;
            c = m_a * t_c + m_c * t_d;
            d = m_b * t_c + m_d * t_d;

            tx = m_a * t_tx + m_c * t_ty + m.tx;
            ty = m_b * t_tx + m_d * t_ty + m.ty;
        }

        /// <summary>
        ///  Return true if `t1' and `t2' are equal, false otherwise. 
        /// </summary>
        public static bool CCAffineTransformEqualToTransform(CCAffineTransform t1, CCAffineTransform t2)
        {
            return (t1.a == t2.a && t1.b == t2.b && t1.c == t2.c && t1.d == t2.d && t1.tx == t2.tx && t1.ty == t2.ty);
        }

        public static CCAffineTransform CCAffineTransformInvert(CCAffineTransform t)
        {
            float determinant = 1 / (t.a * t.d - t.b * t.c);

            return new CCAffineTransform(determinant * t.d, -determinant * t.b, -determinant * t.c, determinant * t.a,
                                         determinant * (t.c * t.ty - t.d * t.tx), determinant * (t.b * t.tx - t.a * t.ty));
        }
    }
}