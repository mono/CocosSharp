using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Cocos2D
{
    public partial class CCLabel
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct ABCFloat
        {
            /// <summary>Specifies the A spacing of the character. The A spacing is the distance to add to the current
            /// position before drawing the character glyph.</summary>
            public float abcfA;
            /// <summary>Specifies the B spacing of the character. The B spacing is the width of the drawn portion of
            /// the character glyph.</summary>
            public float abcfB;
            /// <summary>Specifies the C spacing of the character. The C spacing is the distance to add to the current
            /// position to provide white space to the right of the character glyph.</summary>
            public float abcfC;
        }

        private static Font _font;
        private static Graphics _graphics;
        private static Bitmap _bitmap;
        private static BitmapData _bitmapData;
        private static Brush _brush;
        private static Dictionary<char, KerningInfo> _abcValues = new Dictionary<char, KerningInfo>();

        [DllImport("gdi32.dll", SetLastError = true)]
        static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("user32.dll")]
        static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        private static extern bool GetCharABCWidthsFloat(IntPtr hdc, uint iFirstChar, uint iLastChar, [Out] ABCFloat[] lpABCF);

        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        static extern bool DeleteObject(IntPtr hObject);

        private void CreateFont(string fontName, float fontSize, CCRawList<char> charset)
        {
            _font = new Font(fontName, fontSize);

            var hDC = CreateCompatibleDC(IntPtr.Zero);

            var hFont = _font.ToHfont();
            SelectObject(hDC, hFont);
            
            var value = new ABCFloat[1];
            
            _abcValues.Clear();;

            for (int i = 0; i < charset.Count; i++)
            {
                var ch = charset[i];
                GetCharABCWidthsFloat(hDC, ch, ch, value);
                _abcValues.Add(ch, new KerningInfo() { A = value[0].abcfA, B = value[0].abcfB, C = value[0].abcfC });
            }
            
            DeleteObject(hFont);
            ReleaseDC(IntPtr.Zero, hDC);

            CreateBitmap(1, 1);
        }

        private float GetFontHeight()
        {
            return _font.GetHeight();
        }

        private CCSize GetMeasureString(string text)
        {
            var size = _graphics.MeasureString(text, _font);
            return new CCSize(size.Width, size.Height);
        }

        private void CreateBitmap(int width, int height)
        {
            if (_bitmap == null || (_bitmap.Width < width || _bitmap.Height < height))
            {
                _bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

                _graphics = Graphics.FromImage(_bitmap);

                _graphics.SmoothingMode = SmoothingMode.AntiAlias;
                _graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                _graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                //graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            }

            if (_brush == null)
            {
                _brush = new SolidBrush(System.Drawing.Color.White);
            }
        }

        private void FreeBitmapData()
        {
            if (_bitmapData != null)
            {
                _bitmap.UnlockBits(_bitmapData);
                _bitmapData = null;
            }
        }

        private KerningInfo GetKerningInfo(char ch)
        {
            return _abcValues[ch];
        }

        private unsafe byte* GetBitmapData(string s, out int stride)
        {
            FreeBitmapData();

            var size = GetMeasureString(s);

            var w = (int)(Math.Ceiling(size.Width += 2));
            var h = (int)(Math.Ceiling(size.Height += 2));

            CreateBitmap(w, h);

            _graphics.Clear(System.Drawing.Color.Transparent);
            _graphics.DrawString(s, _font, _brush, 0, 0);
            _graphics.Flush();

            _bitmapData = _bitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            stride = _bitmapData.Stride;

            return (byte*)_bitmapData.Scan0.ToPointer();
        }
    }
}
