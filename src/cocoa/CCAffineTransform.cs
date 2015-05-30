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
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct CCAffineTransform
    {
        public static readonly CCAffineTransform Identity = new CCAffineTransform(1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f);

        Matrix xnaMatrix;

        #region Properties

        internal Matrix XnaMatrix { get { return xnaMatrix; } }

        public float A
        {
            get { return xnaMatrix.M11; }
            set { xnaMatrix.M11 = value; }
        }

        public float B
        {
            get { return xnaMatrix.M12; }
            set { xnaMatrix.M12 = value; }
        }

        public float C
        {
            get { return xnaMatrix.M21; }
            set { xnaMatrix.M21 = value; }
        }

        public float D
        {
            get { return xnaMatrix.M22; }
            set { xnaMatrix.M22 = value; }
        }

        public float Tx
        {
            get { return xnaMatrix.M41; }
            set { xnaMatrix.M41 = value; }
        }

        public float Ty
        {
            get { return xnaMatrix.M42; }
            set { xnaMatrix.M42 = value; }
        }

        public float Tz
        {
            get { return xnaMatrix.M43; }
            set { xnaMatrix.M43 = value; }
        }

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

        public CCAffineTransform Inverse
        {
            get { return new CCAffineTransform(Matrix.Invert(XnaMatrix)); }
        }

        #endregion Properties


        #region Constructors

        public CCAffineTransform(float a, float b, float c, float d, float tx, float ty, float tz = 0.0f)
        {
            xnaMatrix = Matrix.Identity;
            this.A = a;
            this.B = b;
            this.C = c;
            this.D = d;
            this.Tx = tx;
            this.Ty = ty;
            this.Tz = tz;
        }

        internal CCAffineTransform(Matrix xnaMatrixIn) : this()
        {
            xnaMatrix = xnaMatrixIn;
        }

        #endregion Constructors


        public void Lerp(CCAffineTransform m1, CCAffineTransform m2, float t)
        {
            Matrix.Lerp(ref m1.xnaMatrix, ref m2.xnaMatrix, t, out xnaMatrix);
        }

        public void Concat(ref CCAffineTransform m)
        {
            CCAffineTransform.Concat(ref this, ref m, out this);
        }

        #region Transforming types

        public void Transform(ref float x, ref float y, ref float z)
        {
            Vector3 vector = new Vector3(x, y, z);
            Vector3.Transform(ref vector, ref xnaMatrix, out vector);
            x = vector.X;
            y = vector.Y;
            z = vector.Z;
        }

        public void Transform(ref float x, ref float y)
        {
            float z = 0.0f;
            Transform(ref x, ref y, ref z);
        }

        public void Transform(ref int x, ref int y)
        {
            var tmpX = A * x + C * y + Tx;
            y = (int)(B * x + D * y + Ty);
            x = (int)tmpX;
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

        public CCPoint3 Transform(CCPoint3 point)
        {
            Transform(ref point.X, ref point.Y, ref point.Z);
            return point;
        }

        public void Transform(ref CCPoint3 point)
        {
            Transform(ref point.X, ref point.Y, ref point.Z);
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
            var x = quadPoint.Vertices.X;
            var y = quadPoint.Vertices.Y;
            var z = quadPoint.Vertices.Z;
            Transform(ref x, ref y, ref z);
            quadPoint.Vertices.X = x;
            quadPoint.Vertices.Y = y;
            quadPoint.Vertices.Z = z;
        }

        public void Transform(ref CCV3F_C4B quadPoint)
        {
            var x = quadPoint.Vertices.X;
            var y = quadPoint.Vertices.Y;
            var z = quadPoint.Vertices.Z;
            Transform(ref x, ref y, ref z);
            quadPoint.Vertices.X = x;
            quadPoint.Vertices.Y = y;
            quadPoint.Vertices.Z = z;
        }

        public void Transform(ref CCV3F_C4B_T2F_Quad quad)
        {
            Transform(ref quad.TopLeft);
            Transform(ref quad.TopRight);
            Transform(ref quad.BottomLeft);
            Transform(ref quad.BottomRight);
        }

        public CCV3F_C4B_T2F_Quad Transform(CCV3F_C4B_T2F_Quad quad)
        {
            Transform(ref quad);

            return quad;
        }

        #endregion Transforming types

        internal string DebugDisplayString
        {
            get
            {
                if (this == Identity)
                {
                    return "Identity";
                }

                return string.Concat(
                    "( ", xnaMatrix.M11.ToString(), "  ", xnaMatrix.M12.ToString(), "  ", xnaMatrix.M13.ToString(), "  ", xnaMatrix.M14.ToString(), " )  \r\n",
                    "( ", xnaMatrix.M21.ToString(), "  ", xnaMatrix.M22.ToString(), "  ", xnaMatrix.M23.ToString(), "  ", xnaMatrix.M24.ToString(), " )  \r\n",
                    "( ", xnaMatrix.M31.ToString(), "  ", xnaMatrix.M32.ToString(), "  ", xnaMatrix.M33.ToString(), "  ", xnaMatrix.M34.ToString(), " )  \r\n",
                    "( ", xnaMatrix.M41.ToString(), "  ", xnaMatrix.M42.ToString(), "  ", xnaMatrix.M43.ToString(), "  ", xnaMatrix.M44.ToString(), " )");
            }
        }

        public override string ToString()
        {
            return xnaMatrix.ToString();
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

        public bool Equals(ref CCAffineTransform t)
        {
            return xnaMatrix == t.xnaMatrix;
        }

        public override int GetHashCode()
        {
            return xnaMatrix.GetHashCode();

        }

        #endregion Equality


        #region Static methods

        public static CCPoint Transform(CCPoint point, ref CCAffineTransform t)
        {
            Vector3 vec = point.XnaVector;
            Vector3.Transform(ref vec, ref t.xnaMatrix, out vec);
            return new CCPoint(vec.X, vec.Y);
        }

        public static CCPoint3 Transform(CCPoint3 point, ref CCAffineTransform t)
        {
            Vector3 vec = point.XnaVector;
            Vector3.Transform(ref vec, ref t.xnaMatrix, out vec);
            return new CCPoint3(vec);
        }

        public static CCSize Transform(CCSize size, ref CCAffineTransform t)
        {
            Vector3 vec = new Vector3(size.Width, size.Height, 0.0f);
            Vector3.Transform(ref vec, ref t.xnaMatrix, out vec);
            return new CCSize(vec.X, vec.Y);
        }

        public static CCAffineTransform Translate(CCAffineTransform t, float tx, float ty, float tz = 0.0f)
        {
            Matrix translate = Matrix.CreateTranslation(tx, ty, tz);
            return new CCAffineTransform(Matrix.Multiply(translate, t.xnaMatrix));
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

        public static CCAffineTransform ScaleCopy(CCAffineTransform t, float sx, float sy, float sz = 1.0f)
        {
            Matrix scale = Matrix.CreateScale(sx, sy, sz);
            return new CCAffineTransform(Matrix.Multiply(t.xnaMatrix, scale));
        }

        public static void Concat(ref CCAffineTransform t1, ref CCAffineTransform t2, out CCAffineTransform tOut)
        {
            Matrix concatMatrix;
            Matrix.Multiply(ref t1.xnaMatrix, ref t2.xnaMatrix, out concatMatrix);
            tOut = new CCAffineTransform(concatMatrix);
        }

        public static CCAffineTransform Concat(ref CCAffineTransform t1, ref CCAffineTransform t2)
        {
            CCAffineTransform tOut;
            Concat(ref t1, ref t2, out tOut);
            return tOut;
        }

        public static bool Equal(CCAffineTransform t1, CCAffineTransform t2)
        {
            return t1.xnaMatrix == t2.xnaMatrix;
        }

        public static CCAffineTransform Invert(CCAffineTransform t)
        {
            return new CCAffineTransform(Matrix.Invert(t.xnaMatrix));
        }

        public static void LerpCopy(ref CCAffineTransform m1, ref CCAffineTransform m2, float t, out CCAffineTransform res)
        {
            res = new CCAffineTransform(Matrix.Lerp(m1.xnaMatrix, m2.xnaMatrix, t));
        }

        public static CCRect Transform(CCRect rect, CCAffineTransform anAffineTransform)
        {
            return anAffineTransform.Transform(rect);
        }

        #endregion Static methods


        #region Operators

        public static CCAffineTransform operator +(CCAffineTransform affineTransform1, CCAffineTransform affineTransform2)
        {
            return new CCAffineTransform(affineTransform1.xnaMatrix + affineTransform2.xnaMatrix);
        }

        public static CCAffineTransform operator /(CCAffineTransform affineTransform1, CCAffineTransform affineTransform2)
        {
            return new CCAffineTransform(affineTransform1.xnaMatrix/affineTransform2.xnaMatrix);
        }

        public static CCAffineTransform operator /(CCAffineTransform affineTransform, float divider)
        {
            return new CCAffineTransform(affineTransform.xnaMatrix/divider);
        }

        public static bool operator ==(CCAffineTransform affineTransform1, CCAffineTransform affineTransform2)
        {
            return affineTransform1.xnaMatrix == affineTransform2.xnaMatrix;
        }

        public static bool operator !=(CCAffineTransform affineTransform1, CCAffineTransform affineTransform2)
        {
            return affineTransform1.xnaMatrix != affineTransform2.xnaMatrix;
        }

        public static CCAffineTransform operator -(CCAffineTransform affineTransform1, CCAffineTransform affineTransform2)
        {
            return new CCAffineTransform(affineTransform1.xnaMatrix - affineTransform2.xnaMatrix);
        }

        public static CCAffineTransform operator *(CCAffineTransform affinematrix1, CCAffineTransform affinematrix2)
        {
            return new CCAffineTransform(affinematrix1.xnaMatrix * affinematrix2.xnaMatrix);
        }

        public static CCAffineTransform operator -(CCAffineTransform affineTransform1)
        {
            Matrix transformedMat = -(affineTransform1.xnaMatrix);
            return new CCAffineTransform(transformedMat);
        }

        #endregion Operators

    }
}