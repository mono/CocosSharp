
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace CocosSharp
{
    internal static partial class CCLabelUtilities
    {
        private static Bitmap _bitmap;
        private static Graphics _graphics;
        private static Brush _brush;

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

        // RGB to BGR convert Matrix
        private static float[][] rgbtobgr = new float[][]
	      {
		     new float[] {0, 0, 1, 0, 0},
		     new float[] {0, 1, 0, 0, 0},
		     new float[] {1, 0, 0, 0, 0},
		     new float[] {0, 0, 0, 1, 0},
		     new float[] {0, 0, 0, 0, 1}
	      };

        internal static Image RGBToBGR(this Image bmp)
        {
            Image newBmp;
            if ((bmp.PixelFormat & PixelFormat.Indexed) != 0)
            {
                newBmp = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb);
            }
            else
            {
                newBmp = bmp;
            }

            try
            {
                System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();
                System.Drawing.Imaging.ColorMatrix cm = new System.Drawing.Imaging.ColorMatrix(rgbtobgr);

                ia.SetColorMatrix(cm);
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(newBmp))
                {
                    g.DrawImage(bmp, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, System.Drawing.GraphicsUnit.Pixel, ia);
                }
            }
            finally
            {
                if (newBmp != bmp)
                {
                    bmp.Dispose();
                }
            }

            return newBmp;
        }
    }
}
