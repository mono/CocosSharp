
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Cocos2D
{
    internal static partial class CCLabelUtilities
    {
        private static Bitmap _bitmap;
        private static Graphics _graphics;
        private static Brush _brush;

        internal static CCTexture2D CreateNativeLabel(string text, CCSize dimensions, CCTextAlignment hAlignment,
		                                   CCVerticalTextAlignment vAlignment, string fontName,
		                                   float fontSize, CCColor4B textColor)
		{

		    if (string.IsNullOrEmpty(text))
		    {
		        return new CCTexture2D();
		    }

		    var font = CreateFont (fontName, fontSize);

            if (dimensions.Equals(CCSize.Zero))
            {
                CreateBitmap(1, 1);

                var ms = _graphics.MeasureString(text, font);
                
                dimensions.Width = ms.Width;
                dimensions.Height = ms.Height;
            }

            CreateBitmap((int)dimensions.Width, (int)dimensions.Height);

            var stringFormat = new StringFormat();

		    switch (hAlignment)
		    {
		        case CCTextAlignment.Left:
                    stringFormat.Alignment = StringAlignment.Near;
		            break;
		        case CCTextAlignment.Center:
                    stringFormat.Alignment = StringAlignment.Center;
		            break;
		        case CCTextAlignment.Right:
                    stringFormat.Alignment = StringAlignment.Far;
		            break;
		    }

		    switch (vAlignment)
		    {
		        case CCVerticalTextAlignment.Top:
        		    stringFormat.LineAlignment = StringAlignment.Near;
		            break;
		        case CCVerticalTextAlignment.Center:
        		    stringFormat.LineAlignment = StringAlignment.Center;
		            break;
		        case CCVerticalTextAlignment.Bottom:
        		    stringFormat.LineAlignment = StringAlignment.Far;
		            break;
		    }

            _graphics.DrawString(text, font, _brush, new RectangleF(0, 0, dimensions.Width, dimensions.Height), stringFormat);
            _graphics.Flush();

			var texture = new CCTexture2D();
			texture.InitWithStream (SaveToStream(), Microsoft.Xna.Framework.Graphics.SurfaceFormat.Bgra4444);

			return texture;
		}

        static void CreateBitmap(int width, int height)
        {
            if (_brush == null)
            {
                _brush = new SolidBrush(Color.White);
            }

            //if (_bitmap != null && _bitmap.Width <= width && _bitmap.Height <= height)
            //{
            //    return;
            //}

            width = Math.Max(width, 1);
            height = Math.Max(height, 1);

            _bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            _graphics = Graphics.FromImage(_bitmap);

            _graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }

        static Font CreateFont(string familyName, float emSize)
        {
            return CreateFont(familyName, emSize, FontStyle.Regular);
        }
        
        static Font CreateFont(string familyName, float emSize, FontStyle style)
		{
            return new Font(familyName, emSize, style);
        }

		static Stream SaveToStream()
		{
		    var stream = new MemoryStream();
            
            _bitmap.Save(stream, ImageFormat.Png);
		    stream.Position = 0;
		    
            return stream;
		}

    }
}
