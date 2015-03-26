using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CocosSharp
{
    public partial class CCLabel
    {
        private struct KerningInfo
        {
            /// <summary>Specifies the A spacing of the character. The A spacing is the distance to add to the current
            /// position before drawing the character glyph.</summary>
            public float A;
            /// <summary>Specifies the B spacing of the character. The B spacing is the width of the drawn portion of
            /// the character glyph.</summary>
            public float B;
            /// <summary>Specifies the C spacing of the character. The C spacing is the distance to add to the current
            /// position to provide white space to the right of the character glyph.</summary>
            public float C;
        }

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
        private static PrivateFontCollection loadedFontsCollection = new PrivateFontCollection();

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
                            loadedFontsCollection.AddFontFile(fontPath);

                            fontFamily = loadedFontsCollection.Families[loadedFontsCollection.Families.Length - 1];

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

                _graphics.SmoothingMode = SmoothingMode.HighQuality;
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

        private Font CreateFont(string fontName, float fontSize)
        {

            Font currentFont;

            if (_defaultFont == null)
            {
                _defaultFont = new Font(FontFamily.GenericSansSerif, 12);
            }

            FontFamily fontFamily;

            if (!_fontFamilyCache.TryGetValue(fontName, out fontFamily))
            {
                var ext = Path.GetExtension(fontName);

                currentFont = _defaultFont;

                if (!String.IsNullOrEmpty(ext) && ext.ToLower() == ".ttf")
                {
                    var appPath = AppDomain.CurrentDomain.BaseDirectory;
                    var contentPath = Path.Combine(appPath, CCContentManager.SharedContentManager.RootDirectory);
                    var fontPath = Path.Combine(contentPath, fontName);

                    if (File.Exists(fontPath))
                    {
                        try
                        {

                            // Read the font file bytes
                            var fontBytes = File.ReadAllBytes(fontPath);

                            // Pin the font data for the length of the read file
                            var fontData = Marshal.AllocCoTaskMem(fontBytes.Length);

                            // Copy the font data to our memory
                            Marshal.Copy(fontBytes, 0, fontData, fontBytes.Length);

                            // Add the memory data to our private font collection as a Font in Memory
                            loadedFontsCollection.AddMemoryFont(fontData, fontBytes.Length);

                            // Release the pinned data
                            Marshal.FreeCoTaskMem(fontData);

                            // Try to get the family name of the 
                            var ttfFontFamily = CCLabelUtilities.GetFontFamily(fontBytes, 0);

                            fontFamily = new FontFamily(ttfFontFamily, loadedFontsCollection);

                            currentFont = new Font(fontFamily, fontSize);
                        }
                        catch
                        {
                            currentFont = _defaultFont;
                        }
                    }
                }
                else
                {
                    currentFont = new Font(fontName, fontSize);
                }

                _fontFamilyCache.Add(fontName, currentFont.FontFamily);
            }
            else
            {
                currentFont = new Font(fontFamily, fontSize);
            }

            // Create a small bitmap to be used for our graphics context
            CreateBitmap(1, 1);

            return currentFont;
        }

        static float dpiScale = 96f / 72f;  // default but will be recalculated below
#if WINDOWSGL
        internal CCTexture2D CreateTextSprite(string text, CCFontDefinition textDefinition)
        {

            if (string.IsNullOrEmpty(text))
                return new CCTexture2D();

            int imageWidth;
            int imageHeight;
            var textDef = textDefinition;
            var contentScaleFactorWidth = CCLabel.DefaultTexelToContentSizeRatios.Width;
            var contentScaleFactorHeight = CCLabel.DefaultTexelToContentSizeRatios.Height;
            textDef.FontSize *= (int)contentScaleFactorWidth;
            textDef.Dimensions.Width *= contentScaleFactorWidth;
            textDef.Dimensions.Height *= contentScaleFactorHeight;

            bool hasPremultipliedAlpha;

            var font = CreateFont(textDef.FontName, textDef.FontSize / dpiScale);

            var fontColor = textDef.FontFillColor;
            var fontAlpha = textDef.FontAlpha;
            var foregroundColor = System.Drawing.Color.FromArgb(fontAlpha,
                fontColor.R,
                fontColor.G,
                fontColor.B);

            // alignment
            var horizontalAlignment = textDef.Alignment;
            var verticleAlignement = textDef.LineAlignment;

            var textAlign = (CCTextAlignment.Right == horizontalAlignment) ? StringAlignment.Far
                : (CCTextAlignment.Center == horizontalAlignment) ? StringAlignment.Center
                : StringAlignment.Near;

            var paragraphAlign = (CCVerticalTextAlignment.Bottom == verticleAlignement) ? StringAlignment.Far
                : (CCVerticalTextAlignment.Center == verticleAlignement) ? StringAlignment.Center
                : StringAlignment.Near;

            // LineBreak
            var lineBreak = (CCLabelLineBreak.Character == textDef.LineBreak) ? StringTrimming.Character
                : (CCLabelLineBreak.Word == textDef.LineBreak) ? StringTrimming.Word
                : StringTrimming.None;

            var dimensions = new SizeF(textDef.Dimensions.Width, textDef.Dimensions.Height);

            var layoutAvailable = true;
            if (dimensions.Width <= 0)
            {
                dimensions.Width = 8388608;
                layoutAvailable = false;
            }

            if (dimensions.Height <= 0)
            {
                dimensions.Height = 8388608;
                layoutAvailable = false;
            }

            var stringFormat = StringFormat.GenericDefault;

            // We will set the Alignment to Near to begin with because of a calculation error of MeasureString
            // with a line of text with embedded newline '\n' characters after a number and Alignment = Center.
            // Example:  "Alignment 1\nnew line"
            stringFormat.Alignment = StringAlignment.Near;
            stringFormat.LineAlignment = paragraphAlign;
            stringFormat.Trimming = lineBreak;

            int charactersFitted = 0;
            int lineCount = 0;

            var boundingRect = RectangleF.Empty;

            var textMetrics = _graphics.MeasureString(text, font, dimensions, stringFormat,
                out charactersFitted, out lineCount);

            // early out if something went wrong somewhere and nothing is to be drawn
            if (lineCount == 0)
                return new CCTexture2D();

            // We will set the real Alignement here before drawing the text - See comment above about calculation error
            // with Alignment.
            stringFormat.Alignment = textAlign;

            // Fill out the bounding rect width and height so we can calculate the yOffset later if needed
            boundingRect.X = 0;
            boundingRect.Y = 0;
            boundingRect.Width = textMetrics.Width;
            boundingRect.Height = textMetrics.Height;

            if (!layoutAvailable)
            {
                if (dimensions.Width == 8388608)
                {
                    dimensions.Width = boundingRect.Width;
                }
                if (dimensions.Height == 8388608)
                {
                    dimensions.Height = boundingRect.Height;
                }
            }

            imageWidth = (int)dimensions.Width;
            imageHeight = (int)dimensions.Height;

            CreateBitmap(imageWidth, imageHeight);

            if (textDefinition.isShouldAntialias)
                _graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            var _brush = new SolidBrush(foregroundColor);
            _graphics.Clear(System.Drawing.Color.Transparent);
            _graphics.DrawString(text, font, _brush, new RectangleF(PointF.Empty, dimensions), stringFormat);
            _graphics.Flush();

            try
            {
                _bitmap = (Bitmap)_bitmap.RGBToBGR();
                var data = new byte[_bitmap.Width * _bitmap.Height * 4];

                BitmapData bitmapData = _bitmap.LockBits(new System.Drawing.Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                if (bitmapData.Stride != _bitmap.Width * 4)
                    throw new NotImplementedException();
                Marshal.Copy(bitmapData.Scan0, data, 0, data.Length);
                _bitmap.UnlockBits(bitmapData);

                Texture2D texture = null;
                texture = new Texture2D(CCDrawManager.SharedDrawManager.XnaGraphicsDevice, _bitmap.Width, _bitmap.Height);
                texture.SetData(data);

                return new CCTexture2D(texture);
            }
            catch (Exception ie)
            {
                CCLog.Log("CCLabel: internal error creating texture sprite: {0}\n{1}", ie.Message, ie.StackTrace);
            }
            finally
            {
                if (_bitmap != null)
                {
                    _bitmap.Dispose();
                    _bitmap = null;
                }
                if (_graphics != null)
                {
                    _graphics.Dispose();
                    _graphics = null;
                }
                if (_brush != null)
                    _brush.Dispose();
            }

            return new CCTexture2D();
        }

#endif

#if XNA 
        internal CCTexture2D CreateTextSprite(string text, CCFontDefinition textDefinition)
        {
            return new CCTexture2D();
        }
#endif
    }
}
