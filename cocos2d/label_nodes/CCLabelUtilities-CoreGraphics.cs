using System;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;

#if MACOS
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

		static CCVerticalTextAlignment vertical;
		static CCTextAlignment horizontal;

		// Used for debuggin purposes
		internal static void SaveToFile (string fileName, CGBitmapContext bitmap)
		{

			if (bitmap == null)
				throw new ObjectDisposedException ("cgimage");

			// With MonoTouch we can use UTType from CoreMobileServices but since
			// MonoMac does not have that yet (or at least can not find it) I will 
			// use the string version of those for now.  I did not want to add another
			// #if #else in here.

			// for now we will just default this to png
			var typeIdentifier = "public.png";

			// * NOTE * we only support one image for right now.
			//NSMutableData imgData = new NSMutableData();
			NSUrl url = NSUrl.FromFilename (fileName);

			// Create an image destination that saves into the imgData 
			CGImageDestination dest = CGImageDestination.FromUrl (url, typeIdentifier, 1);

			// Add an image to the destination
			dest.AddImage(bitmap.GetImage(), null);

			// Finish the export
			bool success = dest.Close ();
			//                        if (success == false)
			//                                Console.WriteLine("did not work");
			//                        else
			//                                Console.WriteLine("did work: " + path);

			dest.Dispose();
			dest = null;

		}
		internal static IntPtr bitmapBlock;
		internal static CCSize imageSize = CCSize.Zero;

		internal static CGBitmapContext CreateBitmap (int width, int height)  //, PixelFormat format)
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
			bitmapInfo = CGBitmapFlags.PremultipliedLast;

			bytesPerRow = width * bitsPerPixel/bitsPerComponent;
			int size = bytesPerRow * height;

			bitmapBlock = Marshal.AllocHGlobal (size);
			var bitmapContext = new CGBitmapContext (bitmapBlock, 
			                                     width, height, 
			                                     bitsPerComponent, 
			                                     bytesPerRow,
			                                     colorSpace,
			                                     bitmapInfo);

			// This works for now but we need to look into initializing the memory area itself
			bitmapContext.ClearRect (new RectangleF (0,0,width,height));

			return bitmapContext;

		}

		internal static CGImage GetImage (this CGBitmapContext bitmapContext)
		{
			var provider = new CGDataProvider (bitmapContext.Data, bitmapContext.BytesPerRow * bitmapContext.Height, true);

			var NativeCGImage = new CGImage (bitmapContext.Width, 
			                                 bitmapContext.Height, 
			                                 bitmapContext.BitsPerComponent, 
			                                 bitmapContext.BitsPerPixel, 
			                                 bitmapContext.BytesPerRow, 
			                                 bitmapContext.ColorSpace,  
			                                 (CGBitmapFlags)bitmapContext.BitmapInfo,
			                                 provider, 
			                                 null, 
			                                 false, 
			                                 CGColorRenderingIntent.Default);
			return NativeCGImage;
		}

		internal static void NativeDrawString (CGBitmapContext bitmapContext, string s, CTFont font, CCColor4B brush, RectangleF layoutRectangle)
		{
			if (font == null)
				throw new ArgumentNullException ("font");

			if (s == null || s.Length == 0)
				return;

			bitmapContext.ConcatCTM (bitmapContext.GetCTM().Invert());

            // This is not needed here since the color is set in the attributed string.
			//bitmapContext.SetFillColor(brush.R/255f, brush.G/255f, brush.B/255f, brush.A/255f);

            // I think we only Fill the text with no Stroke surrounding
			//bitmapContext.SetTextDrawingMode(CGTextDrawingMode.Fill);

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

		[StructLayout(LayoutKind.Sequential)]
		internal struct ABCFloat
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

		// This only handles one character for right now
		internal static void GetCharABCWidthsFloat (char characters, CTFont font, out ABCFloat[] abc)
		{

			var atts = buildAttributedString(characters.ToString(), font);

			// for now just a line not sure if this is going to work
			CTLine line = new CTLine(atts);

			float ascent;
			float descent;
			float leading;
			abc = new ABCFloat[1];
			abc[0].abcfB = (float)line.GetTypographicBounds(out ascent, out descent, out leading);
			abc [0].abcfB += leading;
		}

		internal static CCSize MeasureString (string textg, CTFont font, CCRect rect)
		{

			var atts = buildAttributedString(textg, font);

			// for now just a line not sure if this is going to work
			CTLine line = new CTLine(atts);

			// Create and initialize some values from the bounds.
			float ascent;
			float descent;
			float leading;
			double lineWidth = line.GetTypographicBounds(out ascent, out descent, out leading);

			var measure = new CCSize((float)lineWidth + leading, ascent + descent);

			return measure;
		}

		internal static CCSize MeasureString (string textg, CTFont font, CCSize layoutArea)
		{
			return MeasureString (textg, font, new CCRect (0, 0, layoutArea.Width, layoutArea.Height));
		}

		internal static CCSize MeasureString (string textg, CTFont font)
		{
			return MeasureString (textg, font, CCSize.Zero);
		}

		private static NSMutableAttributedString buildAttributedString(string text, CTFont font, 
		                                                               CCColor4B? fontColor=null) 
		{

			// Create a new attributed string definition
			var ctAttributes = new CTStringAttributes ();

			// Font attribute
			ctAttributes.Font = font;
			// -- end font 

			if (fontColor.HasValue) {

				// Font color
				var fc = fontColor.Value;
                var cgColor = new CGColor(fc.R / 255f, 
                                             fc.G / 255f,
                                             fc.B / 255f,
                                             fc.A / 255f);

				ctAttributes.ForegroundColor = cgColor;
				ctAttributes.ForegroundColorFromContext = false;
				// -- end font Color
			}

			if (underLine) {
				// Underline
				#if MACOS
				int single = (int)MonoMac.AppKit.NSUnderlineStyle.Single;
				int solid = (int)MonoMac.AppKit.NSUnderlinePattern.Solid;
				var attss = single | solid;
				ctAttributes.UnderlineStyleValue = attss;
				#else
				ctAttributes.UnderlineStyleValue = 1;
				#endif
				// --- end underline
			}


			if (strikeThrough) {
				// StrikeThrough
				//				NSColor bcolor = NSColor.Blue;
				//				NSObject bcolorObject = new NSObject(bcolor.Handle);
				//				attsDic.Add(NSAttributedString.StrikethroughColorAttributeName, bcolorObject);
//				#if MACOS
//				int stsingle = (int)MonoMac.AppKit.NSUnderlineStyle.Single;
//				int stsolid = (int)MonoMac.AppKit.NSUnderlinePattern.Solid;
//				var stattss = stsingle | stsolid;
//				var stunderlineObject = NSNumber.FromInt32(stattss);
//				#else
//				var stunderlineObject = NSNumber.FromInt32 (1);
//				#endif
//
//				attsDic.Add(StrikethroughStyleAttributeName, stunderlineObject);
				// --- end underline
			}


			// Text alignment
			var alignment = CTTextAlignment.Left;
			var alignmentSettings = new CTParagraphStyleSettings();
			alignmentSettings.Alignment = alignment;
			var paragraphStyle = new CTParagraphStyle(alignmentSettings);

			ctAttributes.ParagraphStyle = paragraphStyle;
			// end text alignment

            NSMutableAttributedString atts = 
                new NSMutableAttributedString(text,ctAttributes.Dictionary);

			return atts;

		}

		const byte DefaultCharSet = 1;
		//static CTFont nativeFont;
		static bool underLine = false;
		static bool strikeThrough = false;

		static float dpiScale = 96f / 72f;


		static internal CTFont CreateFont (string familyName, float emSize)
		{
			return CreateFont (familyName, emSize, FontStyle.Regular, DefaultCharSet, false);
		}

		static internal CTFont CreateFont (string familyName, float emSize, FontStyle style)
		{
			return CreateFont (familyName, emSize, style, DefaultCharSet, false);
		}

		static internal CTFont CreateFont (string familyName, float emSize, FontStyle style, byte gdiCharSet)
		{
			return CreateFont (familyName, emSize, style, gdiCharSet, false);
		}

		static internal CTFont CreateFont (string familyName, float emSize, FontStyle style,
		                        byte gdiCharSet, bool  gdiVerticalFont )
		{
			if (emSize <= 0)
				throw new ArgumentException("emSize is less than or equal to 0, evaluates to infinity, or is not a valid number.","emSize");

			CTFont nativeFont;
			// convert to 96 Dpi to be consistent with Windows
			var dpiSize = emSize * dpiScale;

			var ext = System.IO.Path.GetExtension(familyName);
			if (!String.IsNullOrEmpty(ext) && ext.ToLower() == ".ttf")
			{
				var fontName = familyName.Substring (0, familyName.Length - ext.Length);
				var path = CCApplication.SharedApplication.Game.Content.RootDirectory + Path.DirectorySeparatorChar + fontName;
				var pathForResource = NSBundle.MainBundle.PathForResource (path, ext.Substring(1));

				try {
					var dataProvider = new CGDataProvider (pathForResource);
					var cgFont = CGFont.CreateFromProvider (dataProvider);

					try {
						nativeFont = new CTFont(cgFont, dpiSize, null);
					}
					catch
					{
						nativeFont = new CTFont("Helvetica",dpiSize);
					}
				}
				catch (Exception)
				{
					try {
						nativeFont = new CTFont(Path.GetFileNameWithoutExtension(familyName),dpiSize);
					}
					catch
					{
						nativeFont = new CTFont("Helvetica",dpiSize);
					}	
					CCLog.Log (string.Format ("Could not load font: {0} so will use default {1}.", familyName, nativeFont.DisplayName));

				}
			}
			else
			{
				try {
					nativeFont = new CTFont(familyName,dpiSize);
				}
				catch
				{
					nativeFont = new CTFont("Helvetica",dpiSize);
				}
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

			return nativeFont;
		}

		internal static float GetHeight(this CTFont font)
		{
			float lineHeight = 0;

			if (font == null)
				return 0;

			// Get the ascent from the font, already scaled for the font's size
			lineHeight += font.AscentMetric;

			// Get the descent from the font, already scaled for the font's size
			lineHeight += font.DescentMetric;

			// Get the leading from the font, already scaled for the font's size
			//lineHeight += font.LeadingMetric;

			return lineHeight;
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


