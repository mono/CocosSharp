using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

#if WINDOWS || WINDOWSGL
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
#endif

namespace Cocos2D
{
    public class CCLabelTTF : CCLabelBMFont
    {
        public static CCTexture2D m_pTexture;
        protected static bool m_bTextureDirty = true;

        protected string m_FontName;
        protected float m_FontSize;
        private CCSize m_tDimensions;

        public string FontName
        {
            get { return m_FontName; }
            set
            {
                if (m_FontName != value)
                {
                    m_FontName = value;
                    UpdateLabel();
                }
            }
        }

        public float FontSize
        {
            get { return m_FontSize; }
            set
            {
                if (m_FontSize != value)
                {
                    m_FontSize = value;
                    UpdateLabel();
                }
            }
        }

        public CCTextAlignment HorizontalAlignment
        {
            get { return m_pHAlignment; }
            set
            {
                if (m_pHAlignment != value)
                {
                    m_pHAlignment = value;
                    UpdateLabel();
                }
            }
        }

        public CCVerticalTextAlignment VerticalAlignment
        {
            get { return m_pVAlignment; }
            set
            {
                if (m_pVAlignment != value)
                {
                    m_pVAlignment = value;
                    UpdateLabel();
                }
            }
        }

        public CCSize Dimensions
        {
            get { return m_tDimensions; }
            set
            {
                if (m_tDimensions != value)
                {
                    m_tDimensions = value;
                    UpdateLabel();
                }
            }
        }

        public static void InitializeTTFAtlas(int width, int height)
        {
            m_nWidth = width;
            m_nHeight = height;
            m_nDepth = 4;

            m_pTexture = new CCTexture2D();
            m_pData = new int[width * height];

            m_pNodes.Clear();
            m_pNodes.Add(new ivec3() { x = 1, y = 1, z = m_nWidth - 2 });
        }
        
        public CCLabelTTF()
        {
        }

        public CCLabelTTF (string text, string fontName, float fontSize) :
            this(text, fontName, fontSize, CCTextAlignment.Center, CCVerticalTextAlignment.Top)
        { }

        public CCLabelTTF(string text, string fontName, float fontSize, CCTextAlignment hAlignment) :
            this (text, fontName, fontSize, hAlignment, CCVerticalTextAlignment.Top)
        { }

        public CCLabelTTF(string text, string fontName, float fontSize, CCSize dimensions, CCTextAlignment hAlignment)
        {
            InitWithString(text, fontName, fontSize, dimensions, hAlignment, CCPoint.Zero);
        }

        public CCLabelTTF(string text, string fontName, float fontSize, CCSize dimensions, CCTextAlignment hAlignment, CCVerticalTextAlignment vAlignment)
        {
            InitWithString(text, fontName, fontSize, dimensions, hAlignment, CCPoint.Zero);
        }

        public CCLabelTTF(string text, string fontName, float fontSize, CCTextAlignment hAlignment, CCVerticalTextAlignment vAlignment)
        {
            InitWithString(text, fontName, fontSize, CCSize.Zero, hAlignment, CCPoint.Zero);
        }

        public bool InitWithString(string text, string fontName, float fontSize)
        {
            return InitWithString(text, fontName, fontSize, CCSize.Zero, CCTextAlignment.Left, CCPoint.Zero);
        }

        public bool InitWithString(string text, string fontName, float fontSize, CCSize dimensions, CCTextAlignment alignment)
        {
            return InitWithString(text, fontName, fontSize, dimensions, alignment, CCPoint.Zero);
        }

        public bool InitWithString(string text, string fontName, float fontSize, CCSize dimensions, CCTextAlignment alignment, CCPoint imageOffset)
        {
            InitializeFont(fontName, (int)fontSize, text);

            if (m_bTextureDirty)
            {
                m_pTexture.InitWithRawData(m_pData, SurfaceFormat.Color, m_nWidth, m_nHeight, true);
                m_bTextureDirty = false;
            }

            m_pConfiguration = s_pConfigurations[GetFontKey(fontName, fontSize)];

            if (base.InitWithTexture(m_pTexture, text.Length))
            {
                m_fWidth = -1;
                m_pHAlignment = alignment;

                m_cDisplayedOpacity = m_cRealOpacity = 255;
                m_tDisplayedColor = m_tRealColor = CCTypes.CCWhite;
                m_bCascadeOpacityEnabled = true;
                m_bCascadeColorEnabled = true;

                m_obContentSize = CCSize.Zero;

                m_bIsOpacityModifyRGB = m_pobTextureAtlas.Texture.HasPremultipliedAlpha;
                AnchorPoint = new CCPoint(0.5f, 0.5f);

                m_tImageOffset = imageOffset;

                m_pReusedChar = new CCSprite();
                m_pReusedChar.InitWithTexture(m_pobTextureAtlas.Texture, CCRect.Zero, false);
                m_pReusedChar.BatchNode = this;

                SetString(text, true);

                return true;
            }
            return false;
        }

#if WINDOWS || WINDOWSGL
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


        public static void InitializeFont(string fontName, float fontSize, string charset)
        {
            if (m_pData == null)
            {
                InitializeTTFAtlas(512, 512);
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
                return;
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
        }
#endif

        private static string GetFontKey(string fontName, float fontSize)
        {
            return String.Format("ttf-{0}-{1}", fontName, fontSize);
        }

        #region Skyline Bottom Left

        private struct ivec3
        {
            public int x;
            public int y;
            public int z;
        }

        public struct ivec4
        {
            public int x;
            public int y;
            public int width;
            public int height;
        }

        private static CCRawList<ivec3> m_pNodes = new CCRawList<ivec3>();
        private static int m_nUsed;
        private static int m_nWidth;
        private static int m_nHeight;
        private static int m_nDepth;
        private static int[] m_pData;

        private static int Fit(int index, int width, int height)
        {
            var node = m_pNodes[index];

            var x = node.x;
            var y = node.y;
            var widthLeft = width;
            var i = index;

            if ((x + width) > (m_nWidth - 1))
            {
                return -1;
            }

            while (widthLeft > 0)
            {
                node = m_pNodes[i];

                if (node.y > y)
                {
                    y = node.y;
                }

                if ((y + height) > (m_nHeight - 1))
                {
                    return -1;
                }

                widthLeft -= node.z;

                ++i;
            }
            return y;
        }

        private static void Merge()
        {
            var nodes = m_pNodes.Elements;
            for (int i = 0, count = m_pNodes.Count; i < count - 1; ++i)
            {
                if (nodes[i].y == nodes[i + 1].y)
                {
                    nodes[i].z += nodes[i + 1].z;
                    m_pNodes.RemoveAt(i + 1);
                    --count;
                    --i;
                }
            }
        }

        public static ivec4 AllocateRegion(int width, int height)
        {
            ivec3 node, prev;
            ivec4 region = new ivec4() { x = 0, y = 0, width = width, height = height };
            int i;

            int bestHeight = int.MaxValue;
            int bestIndex = -1;
            int bestWidth = int.MaxValue;

            for (i = 0; i < m_pNodes.Count; ++i)
            {
                int y = Fit(i, width, height);

                if (y >= 0)
                {
                    node = m_pNodes[i];
                    if (((y + height) < bestHeight) || (((y + height) == bestHeight) && (node.z < bestWidth)))
                    {
                        bestHeight = y + height;
                        bestIndex = i;
                        bestWidth = node.z;
                        region.x = node.x;
                        region.y = y;
                    }
                }
            }

            if (bestIndex == -1)
            {
                region.x = -1;
                region.y = -1;
                region.width = 0;
                region.height = 0;
                return region;
            }

            //New node
            node.x = region.x;
            node.y = region.y + height;
            node.z = width;
            m_pNodes.Insert(bestIndex, node);

            for (i = bestIndex + 1; i < m_pNodes.Count; ++i)
            {
                node = m_pNodes[i];
                prev = m_pNodes[i - 1];

                if (node.x < (prev.x + prev.z))
                {
                    int shrink = prev.x + prev.z - node.x;
                    node.x += shrink;
                    node.z -= shrink;
                    if (node.z <= 0)
                    {
                        m_pNodes.RemoveAt(i);
                        --i;
                    }
                    else
                    {
                        m_pNodes[i] = node;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            Merge();

            m_nUsed += width * height;

            return region;
        }

        public static void SetRegionData(ivec4 region, int[] data, int stride)
        {
            var x = region.x;
            var y = region.y;
            var width = region.width;
            var height = region.height;

            Debug.Assert(x > 0);
            Debug.Assert(y > 0);
            Debug.Assert(x < (m_nWidth - 1));
            Debug.Assert((x + width) <= (m_nWidth - 1));
            Debug.Assert(y < (m_nHeight - 1));
            Debug.Assert((y + height) <= (m_nHeight - 1));

            var depth = m_nDepth;
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; j++)
                {
                    var b = (byte) ((data[i * stride + j] & 0xFF0000) >> 16);
                    m_pData[((y + i) * m_nWidth + x) + j] = b << 24 | b << 16 | b << 8 | b;
                }
                //    Array.Copy(data, (i * stride), m_pData, ((y + i) * m_nWidth + x), width);
//                Buffer.BlockCopy(data, (i * stride), m_pData, ((y + i) * m_nWidth + x) * depth, width * depth);
            }
        }

        #endregion
    }
    
}
