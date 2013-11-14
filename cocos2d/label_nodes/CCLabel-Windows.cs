using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Runtime.InteropServices;

using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX;
using SharpDX.WIC;

using Factory = SharpDX.Direct2D1.Factory;
using FactoryWrite = SharpDX.DirectWrite.Factory;
using FactoryImaging = SharpDX.WIC.ImagingFactory;


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

        private static Factory Factory2D { get; set; }
        private static FactoryWrite FactoryDWrite { get; set; }
        private static FactoryImaging FactoryImaging { get; set; }
        private SolidColorBrush _brush;

        private SharpDX.WIC.Bitmap _bitmap;
        private WicRenderTarget _renderTarget;
        private static Font _defaultFont;
        private static Font _currentFont;
        private static float _currentDIP;
        private static float _currentFontSizeEm;
        private static Size2F dpi;
        private static Dictionary<string, FontFamily> _fontFamilyCache = new Dictionary<string, FontFamily>();
        private static TextFormat textFormat;
        static float dpiScale = 96f / 72f;  // default but will be recalculated below
        
        // This should be a FontCollection
        //private static PrivateFontCollection _loadedFonts = new PrivateFontCollection();

        private static FontFamily GetFontFamily (string familyName)
        {
            var fontList = FactoryDWrite.GetSystemFontCollection(true);
            int fontIndex = 0;
            fontList.FindFamilyName(familyName, out fontIndex);
            if (fontIndex < 0)
                return null;

            var fontFamily = fontList.GetFontFamily(fontIndex);
            return fontFamily;
        }

        private static Font GenericSanSerif()
        {
            var sanserifs = new string[] {"Microsoft San Serif", "Arial", "Tahoma"};
            FontFamily _fontFamily = null;

            foreach (var family in sanserifs)
            {
                _fontFamily = GetFontFamily(family);
                if (_fontFamily != null)
                    break;
            }

            if (_fontFamily == null)
            {
                var fontList = FactoryDWrite.GetSystemFontCollection(true);
                int fontIndex = 0;
                _fontFamily = fontList.GetFontFamily(fontIndex);
            }
            var _font = _fontFamily.GetFirstMatchingFont(FontWeight.Regular, FontStretch.Normal, FontStyle.Normal);

            return _font;
        }

        private static Font GetFont(string fontName, float fontSize)
        {
            var fontFamily = GetFontFamily(fontName);
            if (fontFamily == null)
                return GenericSanSerif();

            // This is generic right now.  We should be able to handle different styles in the future
            var font = fontFamily.GetFirstMatchingFont(FontWeight.Regular, FontStretch.Normal, FontStyle.Normal);
            
            return font;
        }

        private string CreateFont(string fontName, float fontSize, CCRawList<char> charset)
        {

            if (Factory2D == null)
            {
                Factory2D = new SharpDX.Direct2D1.Factory();
                FactoryDWrite = new SharpDX.DirectWrite.Factory();
                FactoryImaging = new SharpDX.WIC.ImagingFactory();
                
                dpi = Factory2D.DesktopDpi;
                dpiScale = dpi.Height / 72f;
            }

            if (_defaultFont == null)
            {
                _defaultFont = GenericSanSerif();
                //_defaultDIP = ConvertPointSizeToDIP(_defaultFontSizeEm);
            }

            FontFamily fontFamily = GetFontFamily(fontName);


            if (!_fontFamilyCache.TryGetValue(fontName, out fontFamily))
            {
                var ext = Path.GetExtension(fontName);

                _currentFont = _defaultFont;

                if (!String.IsNullOrEmpty(ext) && ext.ToLower() == ".ttf")
                {
                    //var appPath = AppDomain.CurrentDomain.BaseDirectory;
                    //var contentPath = Path.Combine(appPath, CCApplication.SharedApplication.Content.RootDirectory);
                    //var fontPath = Path.Combine(contentPath, fontName);

                    //if (File.Exists(fontPath))
                    //{
                       // try
                        //{
                            //var fontFileReference = new FontCollection(
                            //_loadedFonts.AddFontFile(fontPath);

                    //        //fontFamily = _loadedFonts.Families[_loadedFonts.Families.Length - 1];

                    //        //_currentFont = new Font(fontFamily, fontSize);
                    //    }
                    //    catch
                    //    {
                    //        _currentFont = _defaultFont;
                    //    }
                    //}
                    //else
                    //{
                        _currentFont = _defaultFont;
                        _currentFontSizeEm = fontSize;
                        _currentDIP = ConvertPointSizeToDIP(fontSize);
                    //}
                }
                else
                {
                    _currentFont = GetFont(fontName, fontSize);
                    _currentFontSizeEm = fontSize;
                    _currentDIP = ConvertPointSizeToDIP(fontSize);
                }

                _fontFamilyCache.Add(fontName, _currentFont.FontFamily);
            }
            else
            {
                _currentFont = fontFamily.GetFirstMatchingFont(FontWeight.Regular, FontStretch.Normal, FontStyle.Normal);
                _currentFontSizeEm = fontSize;
                _currentDIP = ConvertPointSizeToDIP(fontSize);
            }
            fontName = _currentFont.FontFamily.FamilyNames.GetString(0); 
            textFormat = new TextFormat(FactoryDWrite, fontName, _currentDIP);
            
            GetKerningInfo(charset);

            return _currentFont.ToString();
        }

        // Device Independant Pixels
        private static float ConvertPointSizeToDIP(float points)
        {
            return points * dpiScale;
        }

        private static Dictionary<char, KerningInfo> _abcValues = new Dictionary<char, KerningInfo>();

        private static void GetKerningInfo(CCRawList<char> charset)
        {
            _abcValues.Clear();

            var fontFace = new FontFace(_currentFont);
            
            var value = new ABCFloat[1];

            var glyphRun = new GlyphRun();
            glyphRun.FontFace = fontFace;
            glyphRun.FontSize = _currentDIP;

            var BrushColor = SharpDX.Color.White;
            /*
            SharpDX.DirectWrite.Matrix mtrx = new SharpDX.DirectWrite.Matrix();
            mtrx.M11 = 1F;
            mtrx.M12 = 0;
            mtrx.M21 = 0;
            mtrx.M22 = 1F;
            mtrx.Dx = 0;
            mtrx.Dy = 0;
            */
            //GlyphMetrics[] metrics = fontFace.GetGdiCompatibleGlyphMetrics(23, 1, mtrx, false, glyphIndices, false);

            //FontMetrics metr = fontFace.GetGdiCompatibleMetrics(23, 1, new SharpDX.DirectWrite.Matrix());
            //_pRenderTarget.DrawGlyphRun(new SharpDX.DrawingPointF(left, top), glyphRun, new SharpDX.Direct2D1.SolidColorBrush(_pRenderTarget, BrushColor), MeasuringMode.GdiClassic);
            int[] codePoints = new int[1];
            var unitsPerEm = fontFace.Metrics.DesignUnitsPerEm;
            var familyName = _currentFont.ToString();

            
            for (int i = 0; i < charset.Count; i++)
            {
                var ch = charset[i];
                if (!_abcValues.ContainsKey(ch))
                {
                    var textLayout = new TextLayout(FactoryDWrite, ch.ToString(), textFormat, unitsPerEm, unitsPerEm);

                    var tlMetrics = textLayout.Metrics;
                    var tlmWidth = tlMetrics.Width;
                    var tllWidth = tlMetrics.LayoutWidth;

                    codePoints[0] = (int)ch;
                    short[] glyphIndices = fontFace.GetGlyphIndices(codePoints);
                    glyphRun.Indices = glyphIndices;

                    var metrics = fontFace.GetDesignGlyphMetrics(glyphIndices, false);
                    
                    //var width = metrics[0].AdvanceWidth + metrics[0].LeftSideBearing + metrics[0].RightSideBearing;
                    //var glyphWidth = _currentFontSizeEm * (float)metrics[0].AdvanceWidth / unitsPerEm;
                    //var abcWidth = _currentDIP * (float)width / unitsPerEm;

                    //value[0].abcfA = _currentFontSizeEm * (float)metrics[0].LeftSideBearing / unitsPerEm;
                    //value[0].abcfB = _currentFontSizeEm * (float)metrics[0].AdvanceWidth / unitsPerEm;
                    //value[0].abcfC = _currentFontSizeEm * (float)metrics[0].RightSideBearing / unitsPerEm;

                    // The A and C values are throwing the spacing off
                    //value[0].abcfA = _currentDIP * (float)metrics[0].LeftSideBearing / unitsPerEm;
                    value[0].abcfB = _currentDIP * (float)metrics[0].AdvanceWidth / unitsPerEm;
                    //value[0].abcfC = _currentDIP * (float)metrics[0].RightSideBearing / unitsPerEm;

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

        }

        private float GetFontHeight()
        {
            return _currentDIP * (float)(_currentFont.Metrics.Ascent + _currentFont.Metrics.Descent + _currentFont.Metrics.LineGap) / _currentFont.Metrics.DesignUnitsPerEm;
        }

        private CCSize GetMeasureString(string text)
        {

            var textLayout = new TextLayout(FactoryDWrite, text, textFormat, 2048, 2048);
            var tlMetrics = textLayout.Metrics;

            return new CCSize(tlMetrics.Width, tlMetrics.Height);
        }

        private void CreateBitmap(int width, int height)
        {


            if (_bitmap == null || (_bitmap.Size.Width < width || _bitmap.Size.Height < height))
            {

                //RenderTargetProperties rtp = new RenderTargetProperties(RenderTargetType.Default,
                //    new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied),
                //    Factory2D.DesktopDpi.Width,
                //    Factory2D.DesktopDpi.Height,
                //    RenderTargetUsage.None,
                //    FeatureLevel.Level_DEFAULT);

                try
                {
                    var pixelFormat = SharpDX.WIC.PixelFormat.Format32bppPRGBA;
                    _bitmap = new SharpDX.WIC.Bitmap(FactoryImaging, width, height, pixelFormat, BitmapCreateCacheOption.CacheOnLoad);

                    _renderTarget = new WicRenderTarget(Factory2D, _bitmap, new RenderTargetProperties());
                    //_renderTarget = new WicRenderTarget(Factory2D, _bitmap, rtpGDI);

                    if (_brush == null)
                    {
                        _brush = new SolidColorBrush(_renderTarget, new Color4(new Color3(1, 1, 1), 1.0f));
                    }

                }
                catch (Exception exc)
                {
                    var ss = exc.Message;
                }

            }
 

        }


        private KerningInfo GetKerningInfo(char ch)
        {
            return _abcValues[ch];
        }

        protected void ReleaseResources()
        {
            _bitmap.Dispose();
            _bitmap = null;

            _renderTarget.Dispose();
            _renderTarget = null;

        }


        private void FreeBitmapData()
        {
            if (pinnedArray.IsAllocated)
                pinnedArray.Free();
        }

        GCHandle pinnedArray;
        private static Color4 TransparentColor = new Color4(new Color3(1, 1.0f, 1.0f), 0.0f);

        private unsafe byte* GetBitmapData(string s, out int stride)
        {

            FreeBitmapData();

            var size = GetMeasureString(s);

            var w = (int)(Math.Ceiling(size.Width += 2));
            var h = (int)(Math.Ceiling(size.Height += 2));

            CreateBitmap(w, h);

            _renderTarget.BeginDraw();

            _renderTarget.Clear(TransparentColor);
            _renderTarget.AntialiasMode = AntialiasMode.Aliased;
            textFormat.TextAlignment = TextAlignment.Center;
            _renderTarget.DrawText(s, textFormat, new RectangleF(0, 0, w, h), _brush);

            _renderTarget.EndDraw();

            //SaveToFile(@"C:\Xamarin\Cocos2D-XNAGraphics\" + s + ".png");

            // Calculate stride of source
            stride = _bitmap.Size.Width * 4;
            // Create data array to hold source pixel data
            //byte[] data = new byte[stride * _bitmap.Size.Height];
            byte[] data = new byte[stride * _bitmap.Size.Height];

            using (var datas = new DataStream(stride * _bitmap.Size.Height, true, true))
            {
                _bitmap.CopyPixels(stride, datas);

                data = datas.ReadRange<byte>(data.Length);
            }

            ReleaseResources();

            pinnedArray = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr pointer = pinnedArray.AddrOfPinnedObject();
            return (byte*)pointer; ;
        }

        // Used for debugging purposes
        private void SaveToFile(string fileName)
        {

            using (var pStream = new WICStream(FactoryImaging, fileName, SharpDX.IO.NativeFileAccess.Write))
            {

                //var format = SharpDX.WIC.PixelFormat.Format32bppPRGBA;
                var format = SharpDX.WIC.PixelFormat.FormatDontCare;
                //// Use InitializeFromFilename to write to a file. If there is need to write inside the memory, use InitializeFromMemory. 
                var encodingFormat = BitmapEncoderGuids.Png;
                var encoder = new PngBitmapEncoder(FactoryImaging, pStream);

                // Create a Frame encoder
                var pFrameEncode = new BitmapFrameEncode(encoder);
                pFrameEncode.Initialize();                

                pFrameEncode.SetSize((int)_renderTarget.Size.Width, (int)_renderTarget.Size.Height);

                pFrameEncode.SetPixelFormat(ref format);

                pFrameEncode.WriteSource(_bitmap);

                pFrameEncode.Commit();

                encoder.Commit();

            }
        }
    }
}
