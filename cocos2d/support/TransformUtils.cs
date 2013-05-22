using Microsoft.Xna.Framework;

namespace Cocos2D
{
    internal class TransformUtils
    {
        internal static Matrix CGAffineToMatrix(float[] m)
        {
            return new Matrix()
            {
                M11 = m[0], M21 = m[4], M31 = m[8],  M41 = m[12],
                M12 = m[1], M22 = m[5], M32 = m[9],  M42 = m[13],
                M13 = m[2], M23 = m[6], M33 = m[10], M43 = m[14],
                M14 = m[3], M24 = m[7], M34 = m[11], M44 = m[15],
            };
        }

        internal static Matrix CGAffineToMatrix(CCAffineTransform t)
        {
            var m = new float[16];
            CGAffineToGL(t, ref m);
            return CGAffineToMatrix(m);
        }

        internal static void CGAffineToGL(CCAffineTransform t, ref float[] m)
        {
            // | m[0] m[4] m[8]  m[12] |     | m11 m21 m31 m41 |     | a c 0 tx |
            // | m[1] m[5] m[9]  m[13] |     | m12 m22 m32 m42 |     | b d 0 ty |
            // | m[2] m[6] m[10] m[14] | <=> | m13 m23 m33 m43 | <=> | 0 0 1  0 |
            // | m[3] m[7] m[11] m[15] |     | m14 m24 m34 m44 |     | 0 0 0  1 |

            m[2] = m[3] = m[6] = m[7] = m[8] = m[9] = m[11] = m[14] = 0.0f;
            m[10] = m[15] = 1.0f;
            m[0] = t.a; m[4] = t.c; m[12] = t.tx;
            m[1] = t.b; m[5] = t.d; m[13] = t.ty;
        }

        internal static void GLToCGAffine(float[] m, CCAffineTransform t)
        {
            t.a = m[0]; t.c = m[4]; t.tx = m[12];
            t.b = m[1]; t.d = m[5]; t.ty = m[13];
        }
    }
}
