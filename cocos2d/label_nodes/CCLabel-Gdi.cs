using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Cocos2D
{
    public partial class CCLabel
    {

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


        public static CCBMFontConfiguration InitializeFont(string fontName, float fontSize, string charset)
        {
            if (m_pData == null)
            {
                InitializeTTFAtlas(1024, 1024);
            }

            if (String.IsNullOrEmpty(charset))
            {
                charset = " ";
            }

            var chars = new CCRawList<char>();

            var fontKey = GetFontKey(fontName, fontSize);

            CCBMFontConfiguration fontConfig;

            if (!s_pConfigurations.TryGetValue(fontKey, out fontConfig))
            {
                fontConfig = new CCBMFontConfiguration();
                s_pConfigurations.Add(fontKey, fontConfig);
            }

            for (int i = 0; i < charset.Length; i++)
            {
                var ch = charset[i];
                if (!fontConfig.m_pFontDefDictionary.ContainsKey(ch) && chars.IndexOf(ch) == -1)
                {
                    chars.Add(ch);
                }
            }

            if (chars.Count == 0)
            {
                return fontConfig;
            }

            var font = new Font(fontName, fontSize);

            fontConfig.m_nCommonHeight = (int)Math.Ceiling(font.GetHeight());

            var bitmap = new Bitmap(fontConfig.m_nCommonHeight * 4, fontConfig.m_nCommonHeight * 2);
            var data = new int[bitmap.Width * bitmap.Height];

            var graphics = Graphics.FromImage(bitmap);

            var brush = new SolidBrush(System.Drawing.Color.White);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            var hDC = CreateCompatibleDC(IntPtr.Zero);
            var hFont = font.ToHfont();
            SelectObject(hDC, hFont);
            ABCFloat[] value = new ABCFloat[1];
            ABCFloat[] values = new ABCFloat[chars.Count];
            for (int i = 0; i < chars.Count; i++)
            {
                var ch = chars[i];
                GetCharABCWidthsFloat(hDC, ch, ch, value);
                values[i] = value[0];
            }
            DeleteObject(hFont);
            ReleaseDC(IntPtr.Zero, hDC);

            for (int i = 0; i < chars.Count; i++)
            {
                var s = chars[i].ToString();

                var charSize = graphics.MeasureString(s, font);

                int w = (int)Math.Ceiling(charSize.Width + 2);
                int h = (int)Math.Ceiling(charSize.Height + 2);

                graphics.Clear(System.Drawing.Color.Transparent);
                graphics.DrawString(s, font, brush, 0, 0);
                graphics.Flush();

                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                unsafe
                {
                    var pBase = (byte*)bitmapData.Scan0.ToPointer();
                    var stride = bitmapData.Stride;

                    int minX = w;
                    int maxX = 0;
                    int minY = h;
                    int maxY = 0;

                    for (int y = 0; y < h; y++)
                    {
                        var row = (int*)(pBase + y * stride);

                        for (int x = 0; x < w; x++)
                        {
                            if (row[x] != 0)
                            {
                                minX = Math.Min(minX, x);
                                maxX = Math.Max(maxX, x);
                                minY = Math.Min(minY, y);
                                maxY = Math.Max(maxY, y);
                            }
                        }
                    }

                    w = Math.Max(maxX - minX + 1, 1);
                    h = Math.Max(maxY - minY + 1, 1);

                    //maxX = minX + w;
                    //maxY = minY + h;

                    int index = 0;
                    for (int y = minY; y <= maxY; y++)
                    {
                        var row = (int*)(pBase + y * stride);
                        for (int x = minX; x <= maxX; x++)
                        {
                            data[index] = row[x];
                            index++;
                        }
                    }

                    var region = AllocateRegion(w, h);

                    if (region.x >= 0)
                    {
                        SetRegionData(region, data, w);

                        var fontDef = new CCBMFontConfiguration.CCBMFontDef()
                        {
                            charID = chars[i],
                            rect = new CCRect(region.x, region.y, region.width, region.height),
                            xOffset = minX, // + (int)Math.Ceiling(values[i].abcfA),
                            yOffset = minY,
                            xAdvance = (int)Math.Ceiling(values[i].abcfA + values[i].abcfB + values[i].abcfC)
                        };

                        fontConfig.CharacterSet.Add(chars[i]);
                        fontConfig.m_pFontDefDictionary.Add(chars[i], fontDef);
                    }
                    else
                    {
                        CCLog.Log("Texture atlas is full");
                    }
                }
                bitmap.UnlockBits(bitmapData);
            }

            m_bTextureDirty = true;

            return fontConfig;
        }

    }
}
