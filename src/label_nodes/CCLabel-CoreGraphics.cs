using System;
using System.Collections.Generic;

using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

#if MACOS
using MonoMac.CoreGraphics;
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.CoreText;
using MonoMac.ImageIO;

#else
using CoreGraphics;
using UIKit;
using Foundation;
using CoreText;
using ImageIO;
#endif

namespace CocosSharp
{
	public partial class CCLabel
	{

//		private static CTFont _font;
//		private static CGBitmapContext _bitmap;
//		private static IntPtr _bitmapData;
//		private static CCColor4B _brush;
//		private static Dictionary<char, KerningInfo> _abcValues = new Dictionary<char, KerningInfo>();
//
//		private void CreateFont(string fontName, float fontSize, CCRawList<char> charset)
//		{
//
//			_font = CCLabelUtilities.CreateFont (fontName, fontSize);
//
//			var value = new CCLabelUtilities.ABCFloat[1];
//
//			_abcValues.Clear();;
//
//			for (int i = 0; i < charset.Count; i++)
//			{
//				var ch = charset[i];
//				CCLabelUtilities.GetCharABCWidthsFloat(ch, _font, out value);
//				_abcValues.Add(ch, new KerningInfo() { A = value[0].abcfA, B = value[0].abcfB, C = value[0].abcfC });
//			}
//
//		}
//
//		private float GetFontHeight()
//		{
//			return _font.GetHeight();
//		}
//
//		private CCSize GetMeasureString(string text)
//		{
//			return CCLabelUtilities.MeasureString(text, _font);
//		}
//
//		private KerningInfo GetKerningInfo(char ch)
//		{
//			return _abcValues[ch];
//		}
//
//		private void CreateBitmap(int width, int height)
//		{
////			if (_bitmap == null || (_bitmap.Width < width || _bitmap.Height < height))
////			{
//
//            _bitmap = CCLabelUtilities.CreateBitmap (width, height);
//			//}
//
//			//if (_brush == null)
//			//{
//            _brush = CCColor4B.White;
//			//}
//		}
//
//		private unsafe byte* GetBitmapData(string s, out int stride)
//		{
//
//			var size = GetMeasureString(s);
//
//			var w = (int)(Math.Ceiling(size.Width += 2));
//			var h = (int)(Math.Ceiling(size.Height += 2));
//
//			CreateBitmap(w, h);
//
//			CCLabelUtilities.NativeDrawString(_bitmap, s, _font, _brush, new RectangleF(0,0,w,h));
//			_bitmapData = _bitmap.Data;
//
//			stride = (int)_bitmap.BytesPerRow;
//
//			return (byte*)_bitmapData;
//		}

//        private static void SaveFileStream(String path, Stream stream)
//        {
//            var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
//            stream.CopyTo(fileStream);
//            fileStream.Dispose();
//        }
        new Dictionary<string, CTFontDescriptor> nativeFontDescriptors;

        string LoadFontFile (string fileName)
        {
            CTFont nativeFont;
            var dpiSize = 0;
            var ext = Path.GetExtension(fileName);

            if (!String.IsNullOrEmpty(ext))
            {

                if (nativeFontDescriptors == null)
                    nativeFontDescriptors = new Dictionary<string, CTFontDescriptor> ();

                //Try loading from Bundle first
                var fontName = fileName.Substring (0, fileName.Length - ext.Length);
                var path = CCContentManager.SharedContentManager.RootDirectory + Path.DirectorySeparatorChar + fontName;
                var pathForResource = NSBundle.MainBundle.PathForResource (path, ext.Substring(1));

                NSUrl url;

                if (!string.IsNullOrEmpty(pathForResource))
                    url = NSUrl.FromFilename (pathForResource);
                else
                    url = NSUrl.FromFilename (fileName);

                // We will not use CTFontManager.RegisterFontsForUrl (url, CTFontManagerScope.Process);
                // here.  The reason is that there is no way we can be sure that the font can be created to
                // to identify the family name afterwards.  So instead we will create a CGFont from a data provider.
                // create CTFont to obtain the CTFontDescriptor, store family name and font descriptor to be accessed
                // later.
                try {
                    var dataProvider = new CGDataProvider (url.Path);
                    var cgFont = CGFont.CreateFromProvider (dataProvider);

                    try 
                    {
                        nativeFont = new CTFont(cgFont, dpiSize, null);
                        if (!nativeFontDescriptors.ContainsKey(nativeFont.FamilyName))
                        {
                            nativeFontDescriptors.Add(nativeFont.FamilyName, nativeFont.GetFontDescriptor());
                            NSError error;

                            var registered = CTFontManager.RegisterGraphicsFont(cgFont, out error);
                            if (!registered)
                            {
                                // If the error code is 105 then the font we are trying to register is already registered
                                // We will not report this as an error.
                                if (error.Code != 105)
                                    throw new ArgumentException("Error registering: " + Path.GetFileName(fileName));
                            }
                        }

                        return nativeFont.PostScriptName;
                    }
                    catch
                    {
                        // note: MS throw the same exception FileNotFoundException if the file exists but isn't a valid font file
                        throw new System.IO.FileNotFoundException (fileName);
                    }
                }
                catch (Exception)
                {
                    // note: MS throw the same exception FileNotFoundException if the file exists but isn't a valid font file
                    throw new System.IO.FileNotFoundException (fileName);

                }

            }
            return fileName;
        }

        #if MACOS
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

            // font
            NSFont font = null;

            var ext = System.IO.Path.GetExtension(textDef.FontName);
            if (!String.IsNullOrEmpty(ext) && ext.ToLower() == ".ttf")
            {
                try 
                {
                    textDef.FontName = LoadFontFile(textDef.FontName);
                    font = NSFont.FromFontName(textDef.FontName, textDef.FontSize);
                }
                catch (Exception exc)
                {
                    CCLog.Log(".ttf {0} file not found or can not be loaded.", textDef.FontName);
                }
            }
            else
            {
                // font
                font = NSFontManager.SharedFontManager.FontWithFamily(textDef.FontName, NSFontTraitMask.Unbold | NSFontTraitMask.Unitalic, 0, textDef.FontSize);
            }

            if (font == null) 
            {
                font = NSFontManager.SharedFontManager.FontWithFamily("Arial", NSFontTraitMask.Unbold | NSFontTraitMask.Unitalic, 0, textDef.FontSize);
                CCLog.Log("{0} not found.  Defaulting to Arial.", textDef.FontName);
            }

            // color
            var fontColor = textDef.FontFillColor;
            var fontAlpha = textDef.FontAlpha;
            var foregroundColor = NSColor.FromDeviceRgba(fontColor.R / 255.0f,
                fontColor.G / 255.0f,
                fontColor.B / 255.0f,
                fontAlpha / 255.0f);

            // alignment
            var horizontalAlignment = textDef.Alignment;
            var verticleAlignement = textDef.LineAlignment;

            var textAlign = (CCTextAlignment.Right == horizontalAlignment) ? NSTextAlignment.Right
                : (CCTextAlignment.Center == horizontalAlignment) ? NSTextAlignment.Center
                : NSTextAlignment.Left;

            // LineBreak
            var lineBreak = (CCLabelLineBreak.Character == textDef.LineBreak) ? NSLineBreakMode.CharWrapping 
                : (CCLabelLineBreak.Word == textDef.LineBreak) ? NSLineBreakMode.ByWordWrapping
                : NSLineBreakMode.Clipping;

            var nsparagraphStyle = new NSMutableParagraphStyle();
            nsparagraphStyle.SetParagraphStyle(NSMutableParagraphStyle.DefaultParagraphStyle);
            nsparagraphStyle.LineBreakMode = lineBreak;
            nsparagraphStyle.Alignment = textAlign;

            // Create a new attributed string definition
            var nsAttributes = new NSStringAttributes ();

            // Font attribute
            nsAttributes.Font = font;
            nsAttributes.ForegroundColor = foregroundColor;
            nsAttributes.ParagraphStyle = nsparagraphStyle;

            var stringWithAttributes = new NSAttributedString(text, nsAttributes);

            var realDimensions = stringWithAttributes.Size;

            // Mac crashes if the width or height is 0
            if (realDimensions == SizeF.Empty)
                throw new ArgumentOutOfRangeException("Native string:", "Dimensions of native NSAttributedString can not be 0,0");

            var dimensions = new SizeF(textDef.Dimensions.Width, textDef.Dimensions.Height);

            var layoutAvailable = true;

            // 
            // * Note * This seems to only effect Mac because iOS works fine without this work around.
            // Right Alignment BoundingRectWithSize does not seem to be working correctly when the following conditions are set:
            //      1) Alignment Right
            //      2) No dimensions
            //      3) There are new line characters embedded in the string.
            //
            // So we set alignment to Left, calculate our bounds and then restore alignement afterwards before drawing.
            //
            if (dimensions.Width <= 0)
            {
                dimensions.Width = 8388608;
                layoutAvailable = false;

                // Set our alignment variables to left - see notes above.
                nsparagraphStyle.Alignment = NSTextAlignment.Left;
            }

            if (dimensions.Height <= 0)
            {
                dimensions.Height = 8388608;
                layoutAvailable = false;
            }

            // Calculate our bounding rectangle
            var boundingRect = stringWithAttributes.BoundingRectWithSize(new SizeF((int)dimensions.Width, (int)dimensions.Height), 
                NSStringDrawingOptions.UsesLineFragmentOrigin);

            if (!layoutAvailable)
            {
                if (dimensions.Width == 8388608)
                {
                    dimensions.Width = boundingRect.Width;

                    // Restore our alignment before drawing - see notes above.
                    nsparagraphStyle.Alignment = textAlign;
                }
                if (dimensions.Height == 8388608)
                {
                    dimensions.Height = boundingRect.Height;
                }
            }

            imageWidth = (int)dimensions.Width;
            imageHeight = (int)dimensions.Height;

            // Alignment
            var xOffset = 0.0f;
            switch (textAlign) {
            case NSTextAlignment.Left:
                xOffset = 0; 
                break;
            case NSTextAlignment.Center: 
                xOffset = (dimensions.Width-boundingRect.Width)/2.0f; 
                break;
            case NSTextAlignment.Right: xOffset = dimensions.Width-boundingRect.Width; break;
            default: break;
            }

            // Line alignment
            var yOffset = (CCVerticalTextAlignment.Top == verticleAlignement 
                || boundingRect.Height >= dimensions.Height) ? (dimensions.Height - boundingRect.Height)  // align to top
                : (CCVerticalTextAlignment.Bottom == verticleAlignement) ? 0                    // align to bottom
                : (imageHeight - boundingRect.Height) / 2.0f;                                   // align to center

            //Find the rect that the string will draw into inside the dimensions 
            var drawRect = new RectangleF(xOffset
                , yOffset
                , boundingRect.Width 
                , boundingRect.Height);

            //Disable antialias
            NSGraphicsContext.CurrentContext.ShouldAntialias = false;

            NSImage image = new NSImage(new SizeF(imageWidth, imageHeight));

            image.LockFocus();

            // set a default transform
            var transform = new NSAffineTransform();
            transform.Set();

            stringWithAttributes.DrawInRect(drawRect);

            image.UnlockFocus();

            // We will use Texture2D from stream here instead of CCTexture2D stream.
            var tex = Texture2D.FromStream(CCDrawManager.SharedDrawManager.XnaGraphicsDevice, image);

            // Debugging purposes
//            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
//            var fileName = Path.Combine(path, "Label3.png");
//            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
//            {
//                tex.SaveAsPng(stream, imageWidth, imageHeight);
//            }

            // Create our texture of the label string.
            var texture = new CCTexture2D(tex);

            return texture;

        }
        #else
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

            // font
            UIFont font = null;

            var ext = System.IO.Path.GetExtension(textDef.FontName);
            if (!String.IsNullOrEmpty(ext) && ext.ToLower() == ".ttf")
            {
                try 
                {
                    textDef.FontName = LoadFontFile(textDef.FontName);
                    font = UIFont.FromName(textDef.FontName, textDef.FontSize);
                }
                catch (Exception exc)
                {
                    CCLog.Log(".ttf {0} file not found or can not be loaded.", textDef.FontName);
                }
            }
            else
            {
                // font
                font = UIFont.FromName (textDef.FontName, textDef.FontSize);
                    //NSFontManager.SharedFontManager.FontWithFamily(textDef.FontName, NSFontTraitMask.Unbold | NSFontTraitMask.Unitalic, 0, textDef.FontSize);
            }

            if (font == null) 
            {
                font = UIFont.FromName (textDef.FontName, textDef.FontSize);
                CCLog.Log("{0} not found.  Defaulting to Arial.", textDef.FontName);
            }

            // color
            var fontColor = textDef.FontFillColor;
            var fontAlpha = textDef.FontAlpha;
            var foregroundColor = UIColor.FromRGBA (fontColor.R / 255.0f,
                fontColor.G / 255.0f,
                fontColor.B / 255.0f,
                fontAlpha / 255.0f);

            // alignment
            var horizontalAlignment = textDef.Alignment;
            var verticleAlignement = textDef.LineAlignment;

            var textAlign = (CCTextAlignment.Right == horizontalAlignment) ? UITextAlignment.Right
                : (CCTextAlignment.Center == horizontalAlignment) ? UITextAlignment.Center
                : UITextAlignment.Left;

            // LineBreak
            var lineBreak = (CCLabelLineBreak.Character == textDef.LineBreak) ? UILineBreakMode.CharacterWrap
                : (CCLabelLineBreak.Word == textDef.LineBreak) ? UILineBreakMode.WordWrap
                : UILineBreakMode.Clip;

            var nsparagraphStyle = (NSMutableParagraphStyle)NSParagraphStyle.Default.MutableCopy();
            nsparagraphStyle.LineBreakMode = lineBreak;
            nsparagraphStyle.Alignment = textAlign;

            // Create a new attributed string definition
            var nsAttributes = new UIStringAttributes ();

            // Font attribute
            nsAttributes.Font = font;
            nsAttributes.ForegroundColor = foregroundColor;
            nsAttributes.ParagraphStyle = nsparagraphStyle;

            var stringWithAttributes = new NSAttributedString(text, nsAttributes);

            var realDimensions = stringWithAttributes.Size;

            // Mac crashes if the width or height is 0
            if (realDimensions == SizeF.Empty)
                throw new ArgumentOutOfRangeException("Native string:", "Dimensions of native NSAttributedString can not be 0,0");

            var dimensions = new CGSize(textDef.Dimensions.Width, textDef.Dimensions.Height);

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


            var boundingRect = stringWithAttributes.GetBoundingRect(new CGSize((int)dimensions.Width, (int)dimensions.Height), 
                NSStringDrawingOptions.UsesLineFragmentOrigin, null);
            
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

            // Alignment
            var xOffset = (nfloat)0.0f;
            switch (textAlign) {
                case UITextAlignment.Left:
                    xOffset = 0; 
                    break;
                case UITextAlignment.Center: 
                    xOffset = (dimensions.Width-boundingRect.Width)/2.0f; 
                    break;
                case UITextAlignment.Right: xOffset = dimensions.Width-boundingRect.Width; break;
                default: break;
            }

            // Line alignment
            var yOffset = (CCVerticalTextAlignment.Bottom == verticleAlignement 
                    || boundingRect.Height >= dimensions.Height) ? (dimensions.Height - boundingRect.Height)  // align to bottom
                : (CCVerticalTextAlignment.Top == verticleAlignement) ? 0                    // align to top
                : (imageHeight - boundingRect.Height) / 2.0f;                                   // align to center

            //Find the rect that the string will draw into inside the dimensions 
            var drawRect = new CGRect(xOffset
                , yOffset
                , boundingRect.Width 
                , boundingRect.Height);


            UIGraphics.BeginImageContext (new CGSize(imageWidth,imageHeight));
            var context = UIGraphics.GetCurrentContext ();

            //Disable antialias
            context.SetShouldAntialias(false);

            stringWithAttributes.DrawString(drawRect);

            var image = UIGraphics.GetImageFromCurrentImageContext ();

            // We will use Texture2D from stream here instead of CCTexture2D stream.
            var tex = Texture2D.FromStream(CCDrawManager.SharedDrawManager.XnaGraphicsDevice, image);

            // Debugging purposes
//            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
//            var fileName = Path.Combine(path, "Label3.png");
//            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
//            {
//                tex.SaveAsPng(stream, imageWidth, imageHeight);
//            }

            // Create our texture of the label string.
            var texture = new CCTexture2D(tex);

            return texture;

        }
        #endif
	}
}
