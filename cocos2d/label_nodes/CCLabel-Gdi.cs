using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
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

        private static Font _defaultFont;
        private static Font _currentFont;

        private static Graphics _graphics;
        private static Bitmap _bitmap;
        private static BitmapData _bitmapData;
        private static Brush _brush;
        private static Dictionary<char, KerningInfo> _abcValues = new Dictionary<char, KerningInfo>();
        private static Dictionary<string, FontFamily> _fontFamilyCache = new Dictionary<string, FontFamily>();
        private static PrivateFontCollection _loadedFonts = new PrivateFontCollection();

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

        private string CreateFont(string fontName, float fontSize, CCRawList<char> charset)
        {
            if (_defaultFont == null)
            {
                _defaultFont = new Font(FontFamily.GenericSansSerif, 12);
            }

            FontFamily fontFamily;

            if (!_fontFamilyCache.TryGetValue(fontName, out fontFamily))
            {
                var ext = Path.GetExtension(fontName);

                _currentFont = _defaultFont;

                if (!String.IsNullOrEmpty(ext) && ext.ToLower() == ".ttf")
                {
                    var appPath = AppDomain.CurrentDomain.BaseDirectory;
                    var contentPath = Path.Combine(appPath, CCContentManager.SharedContentManager.RootDirectory);
                    var fontPath = Path.Combine(contentPath, fontName);

                    if (File.Exists(fontPath))
                    {
                        try
                        {
                            _loadedFonts.AddFontFile(fontPath);

                            fontFamily = _loadedFonts.Families[_loadedFonts.Families.Length - 1];

                            _currentFont = new Font(fontFamily, fontSize);
                        }
                        catch
                        {
                            _currentFont = _defaultFont;
                        }
                    }
                }
                else
                {
                    _currentFont = new Font(fontName, fontSize);
                }

                _fontFamilyCache.Add(fontName, _currentFont.FontFamily);
            }
            else
            {
                _currentFont = new Font(fontFamily, fontSize);
            }

            GetKerningInfo(charset);

            CreateBitmap(1, 1);

            return _currentFont.Name;
        }

        private static void GetKerningInfo(CCRawList<char> charset)
        {
            _abcValues.Clear();

            var hDC = CreateCompatibleDC(IntPtr.Zero);

            var hFont = _currentFont.ToHfont();
            SelectObject(hDC, hFont);

            var value = new ABCFloat[1];
            
            for (int i = 0; i < charset.Count; i++)
            {
                var ch = charset[i];
                if (!_abcValues.ContainsKey(ch))
                {
                    GetCharABCWidthsFloat(hDC, ch, ch, value);
                    _abcValues.Add(
                        ch,
                        new KerningInfo()
                            {
                                A = value[0].abcfA,
                                B = value[0].abcfB,
                                C = value[0].abcfC
                            });
                }
            }

            DeleteObject(hFont);
            ReleaseDC(IntPtr.Zero, hDC);
        }

        private float GetFontHeight()
        {
            return _currentFont.GetHeight();
        }

        private CCSize GetMeasureString(string text)
        {
            var size = _graphics.MeasureString(text, _currentFont);
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
            _graphics.DrawString(s, _currentFont, _brush, 0, 0);
            _graphics.Flush();

            _bitmapData = _bitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            stride = _bitmapData.Stride;

            return (byte*)_bitmapData.Scan0.ToPointer();
        }
    }
}
