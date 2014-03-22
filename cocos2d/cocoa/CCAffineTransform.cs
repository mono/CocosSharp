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
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public struct CCAffineTransform
    {
        public static readonly CCAffineTransform Identity = new CCAffineTransform(1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f);

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

        public static CCPoint Transform(CCPoint point, CCAffineTransform t)
        {
            return new CCPoint(
                t.a * point.X + t.c * point.Y + t.tx,
                t.b * point.X + t.d * point.Y + t.ty
                );
        }

        public static CCSize Transform(CCSize size, CCAffineTransform t)
        {
            var s = new CCSize();
            s.Width = (float) ((double) t.a * size.Width + (double) t.c * size.Height);
            s.Height = (float) ((double) t.b * size.Width + (double) t.d * size.Height);
            return s;
        }

        public static CCAffineTransform Translate(CCAffineTransform t, float tx, float ty)
        {
            return new CCAffineTransform(t.a, t.b, t.c, t.d, t.tx + t.a * tx + t.c * ty, t.ty + t.b * tx + t.d * ty);
        }

        public static CCAffineTransform Rotate(CCAffineTransform t, float anAngle)
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

        public static CCAffineTransform Scale(CCAffineTransform t, float sx, float sy)
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
        public static CCAffineTransform Concat(CCAffineTransform t1, CCAffineTransform t2)
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
        public static bool Equal(CCAffineTransform t1, CCAffineTransform t2)
        {
            return (t1.a == t2.a && t1.b == t2.b && t1.c == t2.c && t1.d == t2.d && t1.tx == t2.tx && t1.ty == t2.ty);
        }

        public static CCAffineTransform Invert(CCAffineTransform t)
        {
            float determinant = 1 / (t.a * t.d - t.b * t.c);

            return new CCAffineTransform(determinant * t.d, -determinant * t.b, -determinant * t.c, determinant * t.a,
                                         determinant * (t.c * t.ty - t.d * t.tx), determinant * (t.b * t.tx - t.a * t.ty));
        }

        public float TranslationX
        {
            get { return tx; }
            set { tx = value; }
        }

        public float TranslationY
        {
            get { return ty; }
            set { ty = value; }
        }

        public void SetTranslation(float x, float y)
        {
            tx = x;
            ty = y;
        }

        public void ConcatTranslation(float x, float y)
        {
            tx += x;
            ty += y;
        }

        public void SetScale(float scaleX, float scaleY)
        {
            SetScaleRotation(scaleX, scaleY, GetRotation());
        }

        public float GetScaleX()
        {
            float a2 = (float)Math.Pow(a, 2);
            float b2 = (float)Math.Pow(b, 2);
            return (float)Math.Sqrt(a2 + b2);
        }

        public void SetScaleX(float scaleX)
        {
            float rotX = GetRotationX();
            a = scaleX * (float)Math.Cos(rotX);
            b = scaleX * (float)Math.Sin(rotX);
        }

        public float GetScaleY()
        {
            float d2 = (float)Math.Pow(d, 2);
            float c2 = (float)Math.Pow(c, 2);
            return (float)Math.Sqrt(d2 + c2);
        }

        public void SetScaleY(float scaleY)
        {
            double rotY = GetRotationY();

            c = -scaleY * (float)Math.Sin(rotY);
            d = scaleY * (float)Math.Cos(rotY);
        }

        public void ConcatScale(float xscale, float yscale)
        {
            a *= xscale;
            c *= yscale;
            b *= xscale;
            d *= yscale;
        }

        public float GetRotation()
        {
            return GetRotationX();
        }

        public float GetRotationX()
        {
            return (float)Math.Atan2(b, a);
        }

        public float GetRotationY()
        {
            return (float)Math.Atan2(-c, d);
        }

        /// Set rotation in radians, scales component are unchanged.
        public void SetRotation(float rotation)
        {
            float rotX = GetRotationX();
            float rotY = GetRotationY();

            float scaleX = GetScaleX();
            float scaleY = GetScaleY();

            a = scaleX * (float)Math.Cos(rotation);
            b = scaleX * (float)Math.Sin(rotation);
            c = -scaleY * (float)Math.Sin(rotY - rotX + rotation);
            d = scaleY * (float)Math.Cos(rotY - rotX + rotation);
        }

        /// Set the scale & rotation part of the CCAffineTransform. angle in radians.
        public void SetScaleRotation(float scaleX, float scaleY, float angle)
        {
            var cosa = (float)Math.Cos(angle);
            var sina = (float)Math.Sin(angle);

            a = scaleX * cosa;
            c = scaleY * -sina;
            b = scaleX * sina;
            d = scaleY * cosa;
        }

        public void SetLerp(CCAffineTransform m1, CCAffineTransform m2, float t)
        {
            a = MathHelper.Lerp(m1.a, m2.a, t);
            b = MathHelper.Lerp(m1.b, m2.b, t);
            c = MathHelper.Lerp(m1.c, m2.c, t);
            d = MathHelper.Lerp(m1.d, m2.d, t);
            tx = MathHelper.Lerp(m1.tx, m2.tx, t);
            ty = MathHelper.Lerp(m1.ty, m2.ty, t);
        }

        public static void Lerp(ref CCAffineTransform m1, ref CCAffineTransform m2, float t, out CCAffineTransform res)
        {
            res.a = MathHelper.Lerp(m1.a, m2.a, t);
            res.b = MathHelper.Lerp(m1.b, m2.b, t);
            res.c = MathHelper.Lerp(m1.c, m2.c, t);
            res.d = MathHelper.Lerp(m1.d, m2.d, t);
            res.tx = MathHelper.Lerp(m1.tx, m2.tx, t);
            res.ty = MathHelper.Lerp(m1.ty, m2.ty, t);
        }

        public void Transform(ref float x, ref float y)
        {
            var tmpX = a * x + c * y + tx;
            y = b * x + d * y + ty;
            x = tmpX;
        }

        public void Transform(ref int x, ref int y)
        {
            var tmpX = a * x + c * y + tx;
            y = (int)(b * x + d * y + ty);
            x = (int)tmpX;
        }

        public void Transform(float x, float y, out float xresult, out float yresult)
        {
            xresult = a * x + c * y + tx;
            yresult = b * x + d * y + ty;
        }

		public CCPoint Transform(CCPoint point)
        {
            return new CCPoint(
                a * point.X + c * point.Y + tx,
                b * point.X + d * point.Y + ty
                );
        }

        public void Transform(ref CCPoint point)
        {
            var tmpX = a * point.X + c * point.Y + tx;
            point.Y = b * point.X + d * point.Y + ty;
            point.X = tmpX;
        }

        public CCRect Transform(CCRect rect)
        {
            float top = rect.MinY;
            float left = rect.MinX;
            float right = rect.MaxX;
            float bottom = rect.MaxY;

            CCPoint topLeft = new CCPoint(left, top);  
            CCPoint topRight = new CCPoint(right, top);
            CCPoint bottomLeft = new CCPoint(left, bottom);
            CCPoint bottomRight = new CCPoint(right, bottom);

            Transform(ref topLeft);
            Transform(ref topRight);
            Transform(ref bottomLeft);
            Transform(ref bottomRight);

            float minX = Math.Min(Math.Min(topLeft.X, topRight.X), Math.Min(bottomLeft.X, bottomRight.X));
            float maxX = Math.Max(Math.Max(topLeft.X, topRight.X), Math.Max(bottomLeft.X, bottomRight.X));
            float minY = Math.Min(Math.Min(topLeft.Y, topRight.Y), Math.Min(bottomLeft.Y, bottomRight.Y));
            float maxY = Math.Max(Math.Max(topLeft.Y, topRight.Y), Math.Max(bottomLeft.Y, bottomRight.Y));

            return new CCRect(minX, minY, (maxX - minX), (maxY - minY));
        }

        public void Transform(ref CCRect rect)
        {
            float top = rect.MinY;
            float left = rect.MinX;
            float right = rect.MaxX;
            float bottom = rect.MaxY;

            CCPoint topLeft = new CCPoint(left, top);
            CCPoint topRight = new CCPoint(right, top);
            CCPoint bottomLeft = new CCPoint(left, bottom);
            CCPoint bottomRight = new CCPoint(right, bottom);

            Transform(ref topLeft);
            Transform(ref topRight);
            Transform(ref bottomLeft);
            Transform(ref bottomRight);

            float minX = Math.Min(Math.Min(topLeft.X, topRight.X), Math.Min(bottomLeft.X, bottomRight.X));
            float maxX = Math.Max(Math.Max(topLeft.X, topRight.X), Math.Max(bottomLeft.X, bottomRight.X));
            float minY = Math.Min(Math.Min(topLeft.Y, topRight.Y), Math.Min(bottomLeft.Y, bottomRight.Y));
            float maxY = Math.Max(Math.Max(topLeft.Y, topRight.Y), Math.Max(bottomLeft.Y, bottomRight.Y));

            rect.Origin.X = minX;
            rect.Origin.Y = minY;
            rect.Size.Width = maxX - minX;
            rect.Size.Height = maxY - minY;
        }

        public static CCRect Transform(CCRect rect, CCAffineTransform anAffineTransform)
        {
            return anAffineTransform.Transform(rect);
        }

        public bool Equals(ref CCAffineTransform t)
        {
            return a == t.a && b == t.b && c == t.c && d == t.d && tx == t.tx && ty == t.ty;
        }


		public override bool Equals(object obj)
		{
			bool flag = false;
			if (obj is CCAffineTransform)
			{
				flag = this.Equals((CCAffineTransform) obj);
			}
			return flag;
		}


		public override int GetHashCode()
		{
			return (((((this.a.GetHashCode () + this.b.GetHashCode ()) + this.c.GetHashCode ()) + this.d.GetHashCode ()) + this.tx.GetHashCode ()) + this.ty.GetHashCode ());

		}

		public static CCAffineTransform operator +(CCAffineTransform affineTransform1, CCAffineTransform affineTransform2)
		{
			affineTransform1.a = affineTransform1.a + affineTransform2.a;
			affineTransform1.b = affineTransform1.b + affineTransform2.b;
			affineTransform1.c = affineTransform1.c + affineTransform2.c;
			affineTransform1.d = affineTransform1.d + affineTransform2.d;
			affineTransform1.tx = affineTransform1.tx + affineTransform2.tx;
			affineTransform1.ty = affineTransform1.ty + affineTransform2.ty;
			return affineTransform1;
		}

		public static CCAffineTransform operator /(CCAffineTransform affineTransform1, CCAffineTransform affineTransform2)
		{
			affineTransform1.a = affineTransform1.a / affineTransform2.a;
			affineTransform1.b = affineTransform1.b / affineTransform2.b;
			affineTransform1.c = affineTransform1.c / affineTransform2.c;
			affineTransform1.d = affineTransform1.d / affineTransform2.d;
			affineTransform1.tx = affineTransform1.tx / affineTransform2.tx;
			affineTransform1.ty = affineTransform1.ty / affineTransform2.ty;
			return affineTransform1;
		}

		public static CCAffineTransform operator /(CCAffineTransform affineTransform, float divider)
		{
			float num = 1f / divider;
			affineTransform.a = affineTransform.a * num;
			affineTransform.b = affineTransform.b * num;
			affineTransform.c = affineTransform.c * num;
			affineTransform.d = affineTransform.d * num;
			affineTransform.tx = affineTransform.tx * num;
			affineTransform.tx = affineTransform.ty * num;
			return affineTransform;
		}

		public static bool operator ==(CCAffineTransform affineTransform1, CCAffineTransform affineTransform2)
		{
			return (affineTransform1.a == affineTransform2.a && 
				affineTransform1.b == affineTransform2.b && 
				affineTransform1.c == affineTransform2.c && 
				affineTransform1.d == affineTransform2.d && 
				affineTransform1.tx == affineTransform2.tx && 
				affineTransform1.ty == affineTransform2.ty);
		}

		public static bool operator !=(CCAffineTransform affineTransform1, CCAffineTransform affineTransform2)
		{
			return (affineTransform1.a != affineTransform2.a || 
				affineTransform1.b != affineTransform2.b || 
				affineTransform1.c != affineTransform2.c || 
				affineTransform1.d != affineTransform2.d || 
				affineTransform1.tx != affineTransform2.tx || 
				affineTransform1.ty != affineTransform2.ty);
		}

		public static CCAffineTransform operator -(CCAffineTransform affineTransform1, CCAffineTransform affineTransform2)
		{
			affineTransform1.a = affineTransform1.a - affineTransform2.a;
			affineTransform1.b = affineTransform1.b - affineTransform2.b;
			affineTransform1.c = affineTransform1.c - affineTransform2.c;
			affineTransform1.d = affineTransform1.d - affineTransform2.d;
			affineTransform1.tx = affineTransform1.tx - affineTransform2.tx;
			affineTransform1.ty = affineTransform1.ty - affineTransform2.ty;
			return affineTransform1;
		}

		public static CCAffineTransform operator *(CCAffineTransform affinematrix1, CCAffineTransform affinematrix2)
		{

			var a = (affinematrix1.a * affinematrix2.a) + (affinematrix1.b * affinematrix2.c);
			var b = (affinematrix1.a * affinematrix2.b) + (affinematrix1.b * affinematrix2.d);
			var c = (affinematrix1.c * affinematrix2.a) + (affinematrix1.d * affinematrix2.c);
			var d = (affinematrix1.c * affinematrix2.b) + (affinematrix1.d * affinematrix2.d);
			var tx = ((affinematrix1.tx * affinematrix2.a) + (affinematrix1.ty * affinematrix2.c)) + affinematrix2.tx;
			var ty = ((affinematrix1.tx * affinematrix2.b) + (affinematrix1.ty * affinematrix2.d)) + affinematrix2.ty;
			affinematrix1.a = a;
			affinematrix1.b = b;
			affinematrix1.c = c;
			affinematrix1.d = d;
			affinematrix1.tx = tx;
			affinematrix1.ty = ty;
		
			return affinematrix1;

		}

		public static CCAffineTransform operator -(CCAffineTransform affineTransform1)
		{
			affineTransform1.a = -affineTransform1.a;
			affineTransform1.b = -affineTransform1.b;
			affineTransform1.c = -affineTransform1.c;
			affineTransform1.d = -affineTransform1.d;
			affineTransform1.tx = -affineTransform1.tx;
			affineTransform1.ty = -affineTransform1.ty;
			return affineTransform1;
		}

    }
}