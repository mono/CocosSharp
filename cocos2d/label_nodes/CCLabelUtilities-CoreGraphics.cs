using System;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Drawing;

#if MONOMAC
using MonoMac.CoreGraphics;
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.CoreText;
using MonoMac.ImageIO;

#else
using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreText;
using MonoTouch.ImageIO;
#endif

namespace Cocos2D
{
	internal static partial class CCLabelUtilities
	{

		static CGImage NativeCGImage;
		static CGBitmapContext bitmapContext;
		static CCTextAlignment horizontal;
		static CCVerticalTextAlignment vertical;

		internal static CCTexture2D CreateNativeLabel (string text, CCSize dimensions, CCTextAlignment hAlignment,
		                                   CCVerticalTextAlignment vAlignment, string fontName,
		                                   float fontSize, CCColor4B textColor)
		{

			if (string.IsNullOrEmpty (text))
				return new CCTexture2D ();

			CreateFont (fontName, fontSize);

			if (dimensions.Equals(CCSize.Zero))
			{
				dimensions = MeasureString(text, nativeFont);
			}

			horizontal = hAlignment;
			vertical = vAlignment;

			CreateBitmap ((int)dimensions.Width, (int)dimensions.Height);

			NativeDrawString (text, nativeFont, textColor, new RectangleF (0, 0, dimensions.Width, dimensions.Height));

			var texture = new CCTexture2D();
			texture.InitWithStream (SaveToStream());

			return texture;
		}

		static System.IO.Stream SaveToStream ()
		{

			if (NativeCGImage == null)
				throw new ObjectDisposedException ("cgimage");

			// With MonoTouch we can use UTType from CoreMobileServices but since
			// MonoMac does not have that yet (or at least can not find it) I will 
			// use the string version of those for now.  I did not want to add another
			// #if #else in here.

			// for now we will just default this to png
			var typeIdentifier = "public.png";

			// * NOTE * we only support one image for right now.
			NSMutableData imgData = new NSMutableData();

			// Create an image destination that saves into the imgData 
			CGImageDestination dest = CGImageDestination.FromData (imgData, typeIdentifier, 1);

			// Add an image to the destination
			dest.AddImage(NativeCGImage, null);

			// Finish the export
			bool success = dest.Close ();
			//                        if (success == false)
			//                                Console.WriteLine("did not work");
			//                        else
			//                                Console.WriteLine("did work: " + path);

			dest.Dispose();
			dest = null;
			return imgData.AsStream ();
		}

		internal static IntPtr bitmapBlock;
		internal static CCSize imageSize = CCSize.Zero;

		private static void CreateBitmap (int width, int height)  //, PixelFormat format)
		{
			int bitsPerComponent, bytesPerRow;
			CGColorSpace colorSpace;
			CGBitmapFlags bitmapInfo;
			//			bool premultiplied = false;
			int bitsPerPixel = 0;

			// Don't forget to set the Image width and height for size.
			imageSize.Width = width;
			imageSize.Height = height;

			colorSpace = CGColorSpace.CreateDeviceRGB ();
			bitsPerComponent = 8;
			bitsPerPixel = 32;
			bitmapInfo = CGBitmapFlags.PremultipliedFirst;

			bytesPerRow = width * bitsPerPixel/bitsPerComponent;
			int size = bytesPerRow * height;

			bitmapBlock = Marshal.AllocHGlobal (size);
			bitmapContext = new CGBitmapContext (bitmapBlock, 
			                                     width, height, 
			                                     bitsPerComponent, 
			                                     bytesPerRow,
			                                     colorSpace,
			                                     CGImageAlphaInfo.PremultipliedLast);

			// This works for now but we need to look into initializing the memory area itself
			bitmapContext.ClearRect (new RectangleF (0,0,width,height));

			var provider = new CGDataProvider (bitmapBlock, size, true);

			NativeCGImage = new CGImage (width, height, bitsPerComponent, bitsPerPixel, bytesPerRow, colorSpace, bitmapInfo, provider, null, false, CGColorRenderingIntent.Default);

		}

		static void NativeDrawString (string s, CTFont font, CCColor4B brush, PointF point)
		{
			NativeDrawString(s, font, brush, new RectangleF(point.X, point.Y, 0, 0));
		}

		static void NativeDrawString (string s, CTFont font, CCColor4B brush, float x, float y)
		{
			NativeDrawString (s, font, brush, new RectangleF(x, y, 0, 0));
		}

		static void NativeDrawString (string s, CTFont font, CCColor4B brush, RectangleF layoutRectangle)
		{
			if (font == null)
				throw new ArgumentNullException ("font");

			if (s == null || s.Length == 0)
				return;

			bitmapContext.ConcatCTM (bitmapContext.GetCTM().Invert());

			bitmapContext.SetFillColor(brush.R/255f, brush.G/255f, brush.B/255f, brush.A/255f);

			// I think we only Fill the text with no Stroke surrounding
			//bitmap.SetTextDrawingMode(CGTextDrawingMode.Fill);

			var attributedString = buildAttributedString(s, font, brush);

			// Work out the geometry
			RectangleF insetBounds = layoutRectangle;

			PointF textPosition = new PointF(insetBounds.X,
			                                 insetBounds.Y);

			float boundsWidth = insetBounds.Width;

			// Calculate the lines
			int start = 0;
			int length = attributedString.Length;

			var typesetter = new CTTypesetter(attributedString);

			float baselineOffset = 0;

			// First we need to calculate the offset for Vertical Alignment if we 
			// are using anything but Top
			if (vertical != CCVerticalTextAlignment.Top) {
				while (start < length) {
					int count = typesetter.SuggestLineBreak (start, boundsWidth);
					var line = typesetter.GetLine (new NSRange(start, count));

					// Create and initialize some values from the bounds.
					float ascent;
					float descent;
					float leading;
					line.GetTypographicBounds (out ascent, out descent, out leading);
					baselineOffset += (float)Math.Ceiling (ascent + descent + leading + 1); // +1 matches best to CTFramesetter's behavior  
					line.Dispose ();
					start += count;
				}
			}

			start = 0;

			while (start < length && textPosition.Y < insetBounds.Bottom)
			{

				// Now we ask the typesetter to break off a line for us.
				// This also will take into account line feeds embedded in the text.
				//  Example: "This is text \n with a line feed embedded inside it"
				int count = typesetter.SuggestLineBreak(start, boundsWidth);
				var line = typesetter.GetLine(new NSRange(start, count));

				// Create and initialize some values from the bounds.
				float ascent;
				float descent;
				float leading;
				line.GetTypographicBounds(out ascent, out descent, out leading);

				// Calculate the string format if need be
				var penFlushness = 0.0f;

				if (horizontal == CCTextAlignment.Right)
					penFlushness = (float)line.GetPenOffsetForFlush(1.0f, boundsWidth);
				else if (horizontal == CCTextAlignment.Center)
					penFlushness = (float)line.GetPenOffsetForFlush(0.5f, boundsWidth);

				// initialize our Text Matrix or we could get trash in here
				var textMatrix = CGAffineTransform.MakeIdentity();

				if (vertical == CCVerticalTextAlignment.Top)
					textMatrix.Translate(penFlushness, insetBounds.Height - textPosition.Y -(float)Math.Floor(ascent - 1));
				if (vertical == CCVerticalTextAlignment.Center)
					textMatrix.Translate(penFlushness, ((insetBounds.Height / 2) + (baselineOffset / 2)) - textPosition.Y -(float)Math.Floor(ascent - 1));
				if (vertical == CCVerticalTextAlignment.Bottom)
					textMatrix.Translate(penFlushness, baselineOffset - textPosition.Y -(float)Math.Floor(ascent - 1));

				// Set our matrix
				bitmapContext.TextMatrix = textMatrix;

				// and draw the line
				line.Draw(bitmapContext);

				// Move the index beyond the line break.
				start += count;
				textPosition.Y += (float)Math.Ceiling(ascent + descent + leading + 1); // +1 matches best to CTFramesetter's behavior  
				line.Dispose();

			}

		}	

		static CCSize MeasureString (string textg, CTFont font, CCRect rect)
		{

			var atts = buildAttributedString(textg, font);

			// for now just a line not sure if this is going to work
			CTLine line = new CTLine(atts);

			// Create and initialize some values from the bounds.
			float ascent;
			float descent;
			float leading;
			double lineWidth = line.GetTypographicBounds(out ascent, out descent, out leading);

			var measure = new CCSize((float)lineWidth, ascent + descent);

			return measure;
		}

		static CCSize MeasureString (string textg, CTFont font, CCSize layoutArea)
		{
			return MeasureString (textg, font, new CCRect (0, 0, layoutArea.Width, layoutArea.Height));
		}

		static CCSize MeasureString (string textg, CTFont font)
		{
			return MeasureString (textg, font, CCSize.Zero);
		}

		static NSString FontAttributedName = (NSString)"NSFont";
		static NSString ForegroundColorAttributedName = (NSString)"NSColor";
		static NSString UnderlineStyleAttributeName = (NSString)"NSUnderline";
		static NSString ParagraphStyleAttributeName = (NSString)"NSParagraphStyle";
		//NSAttributedString.ParagraphStyleAttributeName
		static NSString StrikethroughStyleAttributeName = (NSString)"NSStrikethrough";

		private static NSMutableAttributedString buildAttributedString(string text, CTFont font, 
		                                                               CCColor4B? fontColor=null) 
		{


			// Create a new attributed string from text
			NSMutableAttributedString atts = 
				new NSMutableAttributedString(text);

			var attRange = new NSRange(0, atts.Length);
			var attsDic = new NSMutableDictionary();

			// Font attribute
			NSObject fontObject = new NSObject(font.Handle);
			attsDic.Add(FontAttributedName, fontObject);
			// -- end font 

			if (fontColor.HasValue) {

				// Font color
				var fc = fontColor.Value;
				#if MONOMAC
				NSColor color = NSColor.FromDeviceRgba(fc.R / 255f, 
				                                       fc.G / 255f,
				                                       fc.B / 255f,
				                                       fc.A / 255f);
				NSObject colorObject = new NSObject(color.Handle);
				attsDic.Add(ForegroundColorAttributedName, colorObject);
				#else
				UIColor color = UIColor.FromRGBA(fc.R / 255f, 
				                                 fc.G / 255f,
				                                 fc.B / 255f,
				                                 fc.A / 255f);
				NSObject colorObject = new NSObject(color.Handle);
				attsDic.Add(ForegroundColorAttributedName, colorObject);
				#endif
				// -- end font Color
			}

			if (underLine) {
				// Underline
				#if MONOMAC
				int single = (int)MonoMac.AppKit.NSUnderlineStyle.Single;
				int solid = (int)MonoMac.AppKit.NSUnderlinePattern.Solid;
				var attss = single | solid;
				var underlineObject = NSNumber.FromInt32(attss);
				//var under = NSAttributedString.UnderlineStyleAttributeName.ToString();
				#else
				var underlineObject = NSNumber.FromInt32 (1);
				#endif
				attsDic.Add(UnderlineStyleAttributeName, underlineObject);
				// --- end underline
			}


			if (strikeThrough) {
				// StrikeThrough
				//				NSColor bcolor = NSColor.Blue;
				//				NSObject bcolorObject = new NSObject(bcolor.Handle);
				//				attsDic.Add(NSAttributedString.StrikethroughColorAttributeName, bcolorObject);
				#if MONOMAC
				int stsingle = (int)MonoMac.AppKit.NSUnderlineStyle.Single;
				int stsolid = (int)MonoMac.AppKit.NSUnderlinePattern.Solid;
				var stattss = stsingle | stsolid;
				var stunderlineObject = NSNumber.FromInt32(stattss);
				#else
				var stunderlineObject = NSNumber.FromInt32 (1);
				#endif

				attsDic.Add(StrikethroughStyleAttributeName, stunderlineObject);
				// --- end underline
			}


			// Text alignment
			var alignment = CTTextAlignment.Left;
			var alignmentSettings = new CTParagraphStyleSettings();
			alignmentSettings.Alignment = alignment;
			var paragraphStyle = new CTParagraphStyle(alignmentSettings);
			NSObject psObject = new NSObject(paragraphStyle.Handle);

			// end text alignment

			attsDic.Add(ParagraphStyleAttributeName, psObject);

			atts.SetAttributes(attsDic, attRange);

			return atts;

		}

		const byte DefaultCharSet = 1;
		static CTFont nativeFont;
		static bool underLine = false;
		static bool strikeThrough = false;

		static float dpiScale = 96f / 72f;


		static void CreateFont (string familyName, float emSize)
		{
			CreateFont (familyName, emSize, FontStyle.Regular, DefaultCharSet, false);
		}

		static void CreateFont (string familyName, float emSize, FontStyle style)
		{
			CreateFont (familyName, emSize, style, DefaultCharSet, false);
		}

		static void CreateFont (string familyName, float emSize, FontStyle style, byte gdiCharSet)
		{
			CreateFont (familyName, emSize, style, gdiCharSet, false);
		}

		static void CreateFont (string familyName, float emSize, FontStyle style,
		                        byte gdiCharSet, bool  gdiVerticalFont )
		{
			if (emSize <= 0)
				throw new ArgumentException("emSize is less than or equal to 0, evaluates to infinity, or is not a valid number.","emSize");


			// convert to 96 Dpi to be consistent with Windows
			var dpiSize = emSize * dpiScale;

			try {
				nativeFont = new CTFont(familyName,dpiSize);
			}
			catch
			{
				nativeFont = new CTFont("Helvetica",dpiSize);
			}

			CTFontSymbolicTraits tMask = CTFontSymbolicTraits.None;

			if ((style & FontStyle.Bold) == FontStyle.Bold)
				tMask |= CTFontSymbolicTraits.Bold;
			if ((style & FontStyle.Italic) == FontStyle.Italic)
				tMask |= CTFontSymbolicTraits.Italic;
			strikeThrough = (style & FontStyle.Strikeout) == FontStyle.Strikeout;
			underLine = (style & FontStyle.Underline) == FontStyle.Underline;

			var nativeFont2 = nativeFont.WithSymbolicTraits(dpiSize,tMask,tMask);

			if (nativeFont2 != null)
				nativeFont = nativeFont2;

		}

	}


}

namespace Cocos2D
{
	[Flags]
	internal enum FontStyle {
		Regular   = 0,
		Bold      = 1,
		Italic    = 2,
		Underline = 4,
		Strikeout = 8
	}

}	


