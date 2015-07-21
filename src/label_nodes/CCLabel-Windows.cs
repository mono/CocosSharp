using System;
using System.Collections.Generic;
using System.IO;



#if !WINDOWS_PHONE8
using System.Runtime.InteropServices;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX;
using SharpDX.WIC;

using Factory = SharpDX.Direct2D1.Factory;
using FactoryWrite = SharpDX.DirectWrite.Factory;
using FactoryImaging = SharpDX.WIC.ImagingFactory;
using Microsoft.Xna.Framework.Graphics;
#endif

namespace CocosSharp
{

    public partial class CCLabel
    {
#if WINDOWS_PHONE8
        internal CCTexture2D CreateTextSprite(string text, CCFontDefinition textDefinition)
        {
            return new CCTexture2D();
        }
#else
        private static Factory Factory2D { get; set; }
        private static FactoryWrite FactoryDWrite { get; set; }
        private static FactoryImaging FactoryImaging { get; set; }

        private static Font _defaultFont;
        private static Size2F dpi;
        private static Dictionary<string, FontFamily> _fontFamilyCache = new Dictionary<string, FontFamily>();
        private static Dictionary<string, FontCollection> _fontCollectionCache = new Dictionary<string, FontCollection>();
        static float dpiScale = 96f / 72f;  // default but will be recalculated below

        private static FontCollection _currentFontCollection;

        private static FontFamily GetFontFamily(string familyName)
        {
            int fontIndex = 0;
            _currentFontCollection.FindFamilyName(familyName, out fontIndex);
            if (fontIndex < 0)
                return null;

            var fontFamily = _currentFontCollection.GetFontFamily(fontIndex);
            return fontFamily;
        }

        private static Font GenericSanSerif()
        {
            var sanserifs = new string[] { "Microsoft San Serif", "Arial", "Tahoma" };
            FontFamily _fontFamily = null;

            foreach (var family in sanserifs)
            {
                _fontFamily = GetFontFamily(family);
                if (_fontFamily != null)
                    break;
            }

            if (_fontFamily == null)
            {
                int fontIndex = 0;
                _fontFamily = _currentFontCollection.GetFontFamily(fontIndex);
            }
            var _font = _fontFamily.GetFirstMatchingFont(FontWeight.Regular, FontStretch.Normal, FontStyle.Normal);

            return _font;
        }

        private static Font GetFont(string fontName, float fontSize)
        {
            var fontFamily = GetFontFamily(fontName);

            if (fontFamily == null)
            {
                fontFamily = GetFontFamily(fontName);
            }

            if (fontFamily == null)
            {
                _currentFontCollection = FactoryDWrite.GetSystemFontCollection(true);

                CCLog.Log("{0} not found.  Defaulting to {1}.", fontName, GenericSanSerif().FontFamily.FamilyNames.GetString(0));
                return GenericSanSerif();
            }

            // This is generic right now.  We should be able to handle different styles in the future
            var font = fontFamily.GetFirstMatchingFont(FontWeight.Regular, FontStretch.Normal, FontStyle.Normal);

            return font;
        }

        //private string CreateFont(string fontName, float fontSize, CCRawList<char> charset)
        private Font CreateFont(string fontName, float fontSize)
        {
            Font _currentFont;

            if (Factory2D == null)
            {
                Factory2D = new SharpDX.Direct2D1.Factory();
                FactoryDWrite = new SharpDX.DirectWrite.Factory();
                FactoryImaging = new SharpDX.WIC.ImagingFactory();

                dpi = Factory2D.DesktopDpi;
                dpiScale = dpi.Height / 72f;

            }

            _currentFontCollection = FactoryDWrite.GetSystemFontCollection(true);

            if (_defaultFont == null)
            {
                _defaultFont = GenericSanSerif();
            }

            FontFamily fontFamily = GetFontFamily(fontName);


            FontCollection fontCollection;

            if (!_fontCollectionCache.TryGetValue(fontName, out fontCollection))
            {
                var ext = Path.GetExtension(fontName);

                if (!String.IsNullOrEmpty(ext) && ext.ToLower() == ".ttf")
                {
                    try
                    {
                        var fileFontLoader = new FileFontLoader(FactoryDWrite, fontName);
                        var fileFontCollection = new FontCollection(FactoryDWrite, fileFontLoader, fileFontLoader.Key);
                        _currentFontCollection = fileFontCollection;
                        fontFamily = _currentFontCollection.GetFontFamily(0);
                        var ffn = fontFamily.FamilyNames;
                        _currentFont = fontFamily.GetFirstMatchingFont(FontWeight.Regular, FontStretch.Normal, FontStyle.Normal);

                        _fontCollectionCache.Add(fontName, fileFontCollection);
                        _currentFontCollection = fileFontCollection;
                    }
                    catch
                    {
                        _currentFont = GetFont(fontName, fontSize);
                        CCLog.Log("{0} not found.  Defaulting to {1}.", fontName, _currentFont.FontFamily.FamilyNames.GetString(0));
                    }
                }
                else
                {
                    _currentFont = GetFont(fontName, fontSize);
                }

            }
            else
            {
                fontFamily = fontCollection.GetFontFamily(0);

                _currentFont = fontFamily.GetFirstMatchingFont(FontWeight.Regular, FontStretch.Normal, FontStyle.Normal);
                _currentFontCollection = fontCollection;
            }

            return _currentFont;
        }


        // Device Independant Pixels
        private static float ConvertPointSizeToDIP(float points)
        {
            return points * dpiScale;
        }

        // Used for debugging purposes
        private void SaveToFile(string fileName, SharpDX.WIC.Bitmap _bitmap, RenderTarget _renderTarget)
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

        private static Color4 TransparentColor = new Color4(new Color3(1, 1.0f, 1.0f), 0.0f);
        internal CCTexture2D CreateTextSprite(string text, CCFontDefinition textDefinition)
        {
            if (string.IsNullOrEmpty(text))
                return new CCTexture2D();

            int imageWidth;
            int imageHeight;
            var textDef = textDefinition;
            var contentScaleFactorWidth = CCLabel.DefaultTexelToContentSizeRatios.Width;
            var contentScaleFactorHeight = CCLabel.DefaultTexelToContentSizeRatios.Height;
            textDef.FontSize *= contentScaleFactorWidth;
            textDef.Dimensions.Width *= contentScaleFactorWidth;
            textDef.Dimensions.Height *= contentScaleFactorHeight;

            bool hasPremultipliedAlpha;

            var font = CreateFont(textDef.FontName, textDef.FontSize);

            var _currentFontSizeEm = textDef.FontSize;
            var _currentDIP = ConvertPointSizeToDIP(_currentFontSizeEm);

            // color
            var foregroundColor = Color4.White;

            // alignment
            var horizontalAlignment = textDef.Alignment;
            var verticleAlignement = textDef.LineAlignment;

            var textAlign = (CCTextAlignment.Right == horizontalAlignment) ? TextAlignment.Trailing
                : (CCTextAlignment.Center == horizontalAlignment) ? TextAlignment.Center
                : TextAlignment.Leading;

            var paragraphAlign = (CCVerticalTextAlignment.Bottom == vertAlignment) ? ParagraphAlignment.Far
                : (CCVerticalTextAlignment.Center == vertAlignment) ? ParagraphAlignment.Center
                : ParagraphAlignment.Near;

            // LineBreak
            var lineBreak = (CCLabelLineBreak.Character == textDef.LineBreak) ? WordWrapping.Wrap
                : (CCLabelLineBreak.Word == textDef.LineBreak) ? WordWrapping.Wrap
                : WordWrapping.NoWrap;

            // LineBreak
            // TODO: Find a way to specify the type of line breaking if possible.

            var dimensions = new CCSize(textDef.Dimensions.Width, textDef.Dimensions.Height);

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

            var fontName = font.FontFamily.FamilyNames.GetString(0);
            var textFormat = new TextFormat(FactoryDWrite, fontName,
                _currentFontCollection, FontWeight.Regular, FontStyle.Normal, FontStretch.Normal, _currentDIP);

            textFormat.TextAlignment = textAlign;
            textFormat.ParagraphAlignment = paragraphAlign;

            var textLayout = new TextLayout(FactoryDWrite, text, textFormat, dimensions.Width, dimensions.Height);

            var boundingRect = new RectangleF();

            // Loop through all the lines so we can find our drawing offsets
            var textMetrics = textLayout.Metrics;
            var lineCount = textMetrics.LineCount;

            // early out if something went wrong somewhere and nothing is to be drawn
            if (lineCount == 0)
                return new CCTexture2D();

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

            // Recreate our layout based on calculated dimensions so that we can draw the text correctly
            // in our image when Alignment is not Left.
            if (textAlign != TextAlignment.Leading)
            {
                textLayout.MaxWidth = dimensions.Width;
                textLayout.MaxHeight = dimensions.Height;
            }

            // Line alignment
            var yOffset = (CCVerticalTextAlignment.Bottom == verticleAlignement
                || boundingRect.Bottom >= dimensions.Height) ? dimensions.Height - boundingRect.Bottom  // align to bottom
                : (CCVerticalTextAlignment.Top == verticleAlignement) ? 0                    // align to top
                : (imageHeight - boundingRect.Bottom) * 0.5f;                                   // align to center


            SharpDX.WIC.Bitmap sharpBitmap = null;
            WicRenderTarget sharpRenderTarget = null;
            SolidColorBrush solidBrush = null;

            try
            {
                // Select our pixel format
                var pixelFormat = SharpDX.WIC.PixelFormat.Format32bppPRGBA;

                // create our backing bitmap
                sharpBitmap = new SharpDX.WIC.Bitmap(FactoryImaging, imageWidth, imageHeight, pixelFormat, BitmapCreateCacheOption.CacheOnLoad);

                // Create the render target that we will draw to
                sharpRenderTarget = new WicRenderTarget(Factory2D, sharpBitmap, new RenderTargetProperties());
                // Create our brush to actually draw with
                solidBrush = new SolidColorBrush(sharpRenderTarget, foregroundColor);

                // Begin the drawing
                sharpRenderTarget.BeginDraw();

                if (textDefinition.isShouldAntialias)
                    sharpRenderTarget.AntialiasMode = AntialiasMode.Aliased;

                // Clear it
                sharpRenderTarget.Clear(TransparentColor);

                // Draw the text to the bitmap
                sharpRenderTarget.DrawTextLayout(new Vector2(boundingRect.X, yOffset), textLayout, solidBrush);

                // End our drawing which will commit the rendertarget to the bitmap
                sharpRenderTarget.EndDraw();

                // Debugging purposes
                //var s = "Label4";
                //SaveToFile(@"C:\Xamarin\" + s + ".png", _bitmap, _renderTarget);

                // The following code creates a .png stream in memory of our Bitmap and uses it to create our Textue2D
                Texture2D tex = null;

                using (var memStream = new MemoryStream())
                {
                    using (var encoder = new PngBitmapEncoder(FactoryImaging, memStream))
                    using (var frameEncoder = new BitmapFrameEncode(encoder))
                    {
                        frameEncoder.Initialize();
                        frameEncoder.WriteSource(sharpBitmap);
                        frameEncoder.Commit();
                        encoder.Commit();
                    }
                    // Create the Texture2D from the png stream
                    tex = Texture2D.FromStream(CCDrawManager.SharedDrawManager.XnaGraphicsDevice, memStream);
                }

                // Return our new CCTexture2D created from the Texture2D which will have our text drawn on it.
                return new CCTexture2D(tex);
            }
            catch (Exception exc)
            {
                CCLog.Log("CCLabel-Windows: Unable to create the backing image of our text.  Message: {0}", exc.StackTrace);
            }
            finally
            {
                if (sharpBitmap != null)
                {
                    sharpBitmap.Dispose();
                    sharpBitmap = null;
                }

                if (sharpRenderTarget != null)
                {
                    sharpRenderTarget.Dispose();
                    sharpRenderTarget = null;
                }

                if (solidBrush != null)
                {
                    solidBrush.Dispose();
                    solidBrush = null;
                }

                if (textFormat != null)
                {
                    textFormat.Dispose();
                    textFormat = null;
                }

                if (textLayout != null)
                {
                    textLayout.Dispose();
                    textLayout = null;
                }
            }

            // If we have reached here then something has gone wrong.
            return new CCTexture2D();
        }
#endif
    }
}
