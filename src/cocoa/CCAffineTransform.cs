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

		public float A, B, C, D;
		public float Tx, Ty;


		#region Properties

		public float Scale
		{
			set { ScaleX = value; ScaleY = value; }
		}

		public float ScaleX
		{
			get 
			{
				float a2 = (float)Math.Pow (A, 2);
				float b2 = (float)Math.Pow (B, 2);
				return (float)Math.Sqrt (a2 + b2);
			}

			set 
			{
				float rotX = RotationY;
				A = value * (float)Math.Cos(rotX);
				B = value * (float)Math.Sin(rotX);
			}
		}

		public float ScaleY
		{
			get 
			{
				float d2 = (float)Math.Pow(D, 2);
				float c2 = (float)Math.Pow(C, 2);
				return (float)Math.Sqrt(d2 + c2);
			}

			set 
			{
				double rotY = RotationY;

				C = -value * (float)Math.Sin(rotY);
				D = value * (float)Math.Cos(rotY);
			}
		}

		public float Rotation
		{
			set 
			{
				float rotX = RotationX;
				float rotY = RotationY;

				float scaleX = ScaleX;
				float scaleY = ScaleY;

				A = scaleX * (float)Math.Cos(value);
				B = scaleX * (float)Math.Sin(value);
				C = -scaleY * (float)Math.Sin(rotY - rotX + value);
				D = scaleY * (float)Math.Cos(rotY - rotX + value);
			}
		}

		public float RotationX
		{
			get { return (float)Math.Atan2(B, A); }
		}

		public float RotationY
		{
			get { return (float)Math.Atan2(-C, D); }
		}

		// Set the scale and angle all in one go
		// Simpler grouped operation than doing each individually
		public float this[float scaleX, float scaleY, float angle]
		{
			set 
			{
				var cosa = (float)Math.Cos(angle);
				var sina = (float)Math.Sin(angle);

				A = scaleX * cosa;
				C = scaleY * -sina;
				B = scaleX * sina;
				D = scaleY * cosa;
			}
		}


        internal Matrix XnaMatrix
        {
            get 
            {
                Matrix xnaMatrix = Matrix.Identity;
                xnaMatrix.M11 = this.A;
                xnaMatrix.M21 = this.C;
                xnaMatrix.M12 = this.B;
                xnaMatrix.M22 = this.D;
                xnaMatrix.M41 = this.Tx;
                xnaMatrix.M42 = this.Ty;
                xnaMatrix.M43 = 1;

                return xnaMatrix;
            }
        }

		#endregion Properties


		#region Constructors

		public CCAffineTransform(float a, float b, float c, float d, float tx, float ty) : this()
        {
			this.A = a;
			this.B = b;
			this.C = c;
			this.D = d;
			this.Tx = tx;
			this.Ty = ty;
        }

        internal CCAffineTransform(Matrix xnaMatrix) 
            : this(xnaMatrix.M11, xnaMatrix.M12, xnaMatrix.M21, xnaMatrix.M22, xnaMatrix.M41, xnaMatrix.M42)
        {
        }

		#endregion Constructors


		public void Lerp(CCAffineTransform m1, CCAffineTransform m2, float t)
		{
			A = MathHelper.Lerp(m1.A, m2.A, t);
			B = MathHelper.Lerp(m1.B, m2.B, t);
			C = MathHelper.Lerp(m1.C, m2.C, t);
			D = MathHelper.Lerp(m1.D, m2.D, t);
			Tx = MathHelper.Lerp(m1.Tx, m2.Tx, t);
			Ty = MathHelper.Lerp(m1.Ty, m2.Ty, t);
		}

		public void Concat(CCAffineTransform m)
		{
			Concat(ref m);
		}

		public void Concat(ref CCAffineTransform m)
		{
			float t_a = A;
			float t_b = B;
			float t_c = C;
			float t_d = D;
			float t_tx = Tx;
			float t_ty = Ty;

			float m_a = m.A;
			float m_b = m.B;
			float m_c = m.C;
			float m_d = m.D;

			A = m_a * t_a + m_c * t_b;
			B = m_b * t_a + m_d * t_b;
			C = m_a * t_c + m_c * t_d;
			D = m_b * t_c + m_d * t_d;

			Tx = m_a * t_tx + m_c * t_ty + m.Tx;
			Ty = m_b * t_tx + m_d * t_ty + m.Ty;
		}

		public void Transform(ref float x, ref float y)
		{
			var tmpX = A * x + C * y + Tx;
			y = B * x + D * y + Ty;
			x = tmpX;
		}

		public void Transform(ref int x, ref int y)
		{
			var tmpX = A * x + C * y + Tx;
			y = (int)(B * x + D * y + Ty);
			x = (int)tmpX;
		}

		public void Transform(float x, float y, out float xresult, out float yresult)
		{
			xresult = x;
			yresult = y;

			Transform(ref xresult, ref yresult);
		}


		public CCPoint Transform(CCPoint point)
		{
			Transform(ref point);
			return point;
		}

		public void Transform(ref CCPoint point)
		{
			Transform(ref point.X, ref point.Y);
		}

		public CCRect Transform(CCRect rect)
		{
			Transform(ref rect);

			return rect;
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

        public void Transform(ref CCV3F_C4B_T2F quadPoint)
        {
            CCPoint projectedPoint = Transform(new CCPoint(quadPoint.Vertices.X, quadPoint.Vertices.Y));

            quadPoint.Vertices = new CCVertex3F(projectedPoint.X, projectedPoint.Y, quadPoint.Vertices.Z);
        }

        public CCV3F_C4B_T2F_Quad Transform(CCV3F_C4B_T2F_Quad quad)
        {
            Transform(ref quad.TopLeft);
            Transform(ref quad.TopRight);
            Transform(ref quad.BottomLeft);
            Transform(ref quad.BottomRight);

            return quad;
        }

		#region Equality

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
			return (((((this.A.GetHashCode () + this.B.GetHashCode ()) + this.C.GetHashCode ()) + this.D.GetHashCode ()) + this.Tx.GetHashCode ()) + this.Ty.GetHashCode ());

		}

		#endregion Equality


		#region Static methods

        public static CCPoint Transform(CCPoint point, CCAffineTransform t)
        {
            return new CCPoint(
				t.A * point.X + t.C * point.Y + t.Tx,
				t.B * point.X + t.D * point.Y + t.Ty
                );
        }

        public static CCSize Transform(CCSize size, CCAffineTransform t)
        {
            var s = new CCSize();
			s.Width = (float) ((double) t.A * size.Width + (double) t.C * size.Height);
			s.Height = (float) ((double) t.B * size.Width + (double) t.D * size.Height);
            return s;
        }

        public static CCAffineTransform Translate(CCAffineTransform t, float tx, float ty)
        {
			return new CCAffineTransform(t.A, t.B, t.C, t.D, t.Tx + t.A * tx + t.C * ty, t.Ty + t.B * tx + t.D * ty);
        }

        public static CCAffineTransform Rotate(CCAffineTransform t, float anAngle)
        {
            var fSin = (float) Math.Sin(anAngle);
            var fCos = (float) Math.Cos(anAngle);

			return new CCAffineTransform(t.A * fCos + t.C * fSin,
				t.B * fCos + t.D * fSin,
				t.C * fCos - t.A * fSin,
				t.D * fCos - t.B * fSin,
				t.Tx,
				t.Ty);
        }

		public static CCAffineTransform ScaleCopy(CCAffineTransform t, float sx, float sy)
        {
			return new CCAffineTransform(t.A * sx, t.B * sx, t.C * sy, t.D * sy, t.Tx, t.Ty);
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
			return new CCAffineTransform(t1.A * t2.A + t1.B * t2.C, t1.A * t2.B + t1.B * t2.D, //a,b
				t1.C * t2.A + t1.D * t2.C, t1.C * t2.B + t1.D * t2.D, //c,d
				t1.Tx * t2.A + t1.Ty * t2.C + t2.Tx, //tx
				t1.Tx * t2.B + t1.Ty * t2.D + t2.Ty); //ty
        }

        public static bool Equal(CCAffineTransform t1, CCAffineTransform t2)
        {
			return (t1.A == t2.A && t1.B == t2.B && t1.C == t2.C && t1.D == t2.D && t1.Tx == t2.Tx && t1.Ty == t2.Ty);
        }

        public static CCAffineTransform Invert(CCAffineTransform t)
        {
			float determinant = 1 / (t.A * t.D - t.B * t.C);

			return new CCAffineTransform(determinant * t.D, -determinant * t.B, -determinant * t.C, determinant * t.A,
				determinant * (t.C * t.Ty - t.D * t.Tx), determinant * (t.B * t.Tx - t.A * t.Ty));
        }

		public static void LerpCopy(ref CCAffineTransform m1, ref CCAffineTransform m2, float t, out CCAffineTransform res)
        {
			res.A = MathHelper.Lerp(m1.A, m2.A, t);
			res.B = MathHelper.Lerp(m1.B, m2.B, t);
			res.C = MathHelper.Lerp(m1.C, m2.C, t);
			res.D = MathHelper.Lerp(m1.D, m2.D, t);
			res.Tx = MathHelper.Lerp(m1.Tx, m2.Tx, t);
			res.Ty = MathHelper.Lerp(m1.Ty, m2.Ty, t);
        }

        public static CCRect Transform(CCRect rect, CCAffineTransform anAffineTransform)
        {
            return anAffineTransform.Transform(rect);
        }

        public bool Equals(ref CCAffineTransform t)
        {
			return A == t.A && B == t.B && C == t.C && D == t.D && Tx == t.Tx && Ty == t.Ty;
        }

		#endregion Static methods


		#region Operators

		public static CCAffineTransform operator +(CCAffineTransform affineTransform1, CCAffineTransform affineTransform2)
		{
			affineTransform1.A = affineTransform1.A + affineTransform2.A;
			affineTransform1.B = affineTransform1.B + affineTransform2.B;
			affineTransform1.C = affineTransform1.C + affineTransform2.C;
			affineTransform1.D = affineTransform1.D + affineTransform2.D;
			affineTransform1.Tx = affineTransform1.Tx + affineTransform2.Tx;
			affineTransform1.Ty = affineTransform1.Ty + affineTransform2.Ty;
			return affineTransform1;
		}

		public static CCAffineTransform operator /(CCAffineTransform affineTransform1, CCAffineTransform affineTransform2)
		{
			affineTransform1.A = affineTransform1.A / affineTransform2.A;
			affineTransform1.B = affineTransform1.B / affineTransform2.B;
			affineTransform1.C = affineTransform1.C / affineTransform2.C;
			affineTransform1.D = affineTransform1.D / affineTransform2.D;
			affineTransform1.Tx = affineTransform1.Tx / affineTransform2.Tx;
			affineTransform1.Ty = affineTransform1.Ty / affineTransform2.Ty;
			return affineTransform1;
		}

		public static CCAffineTransform operator /(CCAffineTransform affineTransform, float divider)
		{
			float num = 1f / divider;
			affineTransform.A = affineTransform.A * num;
			affineTransform.B = affineTransform.B * num;
			affineTransform.C = affineTransform.C * num;
			affineTransform.D = affineTransform.D * num;
			affineTransform.Tx = affineTransform.Tx * num;
			affineTransform.Tx = affineTransform.Ty * num;
			return affineTransform;
		}

		public static bool operator ==(CCAffineTransform affineTransform1, CCAffineTransform affineTransform2)
		{
			return (affineTransform1.A == affineTransform2.A && 
				affineTransform1.B == affineTransform2.B && 
				affineTransform1.C == affineTransform2.C && 
				affineTransform1.D == affineTransform2.D && 
				affineTransform1.Tx == affineTransform2.Tx && 
				affineTransform1.Ty == affineTransform2.Ty);
		}

		public static bool operator !=(CCAffineTransform affineTransform1, CCAffineTransform affineTransform2)
		{
			return (affineTransform1.A != affineTransform2.A || 
				affineTransform1.B != affineTransform2.B || 
				affineTransform1.C != affineTransform2.C || 
				affineTransform1.D != affineTransform2.D || 
				affineTransform1.Tx != affineTransform2.Tx || 
				affineTransform1.Ty != affineTransform2.Ty);
		}

		public static CCAffineTransform operator -(CCAffineTransform affineTransform1, CCAffineTransform affineTransform2)
		{
			affineTransform1.A = affineTransform1.A - affineTransform2.A;
			affineTransform1.B = affineTransform1.B - affineTransform2.B;
			affineTransform1.C = affineTransform1.C - affineTransform2.C;
			affineTransform1.D = affineTransform1.D - affineTransform2.D;
			affineTransform1.Tx = affineTransform1.Tx - affineTransform2.Tx;
			affineTransform1.Ty = affineTransform1.Ty - affineTransform2.Ty;
			return affineTransform1;
		}

		public static CCAffineTransform operator *(CCAffineTransform affinematrix1, CCAffineTransform affinematrix2)
		{

			var a = (affinematrix1.A * affinematrix2.A) + (affinematrix1.B * affinematrix2.C);
			var b = (affinematrix1.A * affinematrix2.B) + (affinematrix1.B * affinematrix2.D);
			var c = (affinematrix1.C * affinematrix2.A) + (affinematrix1.D * affinematrix2.C);
			var d = (affinematrix1.C * affinematrix2.B) + (affinematrix1.D * affinematrix2.D);
			var tx = ((affinematrix1.Tx * affinematrix2.A) + (affinematrix1.Ty * affinematrix2.C)) + affinematrix2.Tx;
			var ty = ((affinematrix1.Tx * affinematrix2.B) + (affinematrix1.Ty * affinematrix2.D)) + affinematrix2.Ty;
			affinematrix1.A = a;
			affinematrix1.B = b;
			affinematrix1.C = c;
			affinematrix1.D = d;
			affinematrix1.Tx = tx;
			affinematrix1.Ty = ty;
		
			return affinematrix1;

		}

		public static CCAffineTransform operator -(CCAffineTransform affineTransform1)
		{
			affineTransform1.A = -affineTransform1.A;
			affineTransform1.B = -affineTransform1.B;
			affineTransform1.C = -affineTransform1.C;
			affineTransform1.D = -affineTransform1.D;
			affineTransform1.Tx = -affineTransform1.Tx;
			affineTransform1.Ty = -affineTransform1.Ty;
			return affineTransform1;
		}

		#endregion Operators

    }
}