using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Android.App;
using Android.Graphics;
using Android.Util;

namespace CocosSharp
{
    public partial class CCLabel
    {
        private static Paint _paint;
        private static Bitmap _bitmap;
        private static Canvas _canvas;
        private static int[] _data;
        private static GCHandle _dataHandle;
        private static Paint.FontMetrics _fontMetrix;
        private static float _fontScaleFactor;

        private void CreateFont(string fontName, float fontSize, CCRawList<char> charset)
        {
            if (_paint == null)
            {
                var display = Game.Activity.WindowManager.DefaultDisplay;
                var metrics = new DisplayMetrics();
                display.GetMetrics(metrics);

                _fontScaleFactor = metrics.ScaledDensity;

                _paint = new Paint(PaintFlags.AntiAlias);
                _paint.Color = Android.Graphics.Color.White;
                _paint.TextAlign = Paint.Align.Left;
                // _paint.LinearText = true;
            }

            _paint.TextSize = fontSize;

            var ext = System.IO.Path.GetExtension(fontName);
            if (!String.IsNullOrEmpty(ext) && ext.ToLower() == ".ttf")
            {

				var path = System.IO.Path.Combine(CCContentManager.SharedContentManager.RootDirectory, fontName);
                var activity = Game.Activity;

                try
                {
                    var typeface = Typeface.CreateFromAsset(activity.Assets, path);
                    _paint.SetTypeface(typeface);
                }
                catch (Exception)
                {
                    _paint.SetTypeface(Typeface.Create(fontName, TypefaceStyle.Normal));
                }
            }
            else
            {
                _paint.SetTypeface(Typeface.Create(fontName, TypefaceStyle.Normal));
            }

            _fontMetrix = _paint.GetFontMetrics();
        }

        private void CreateBitmap(int width, int height)
        {
            //if (_bitmap == null || _bitmap.Width < width || _bitmap.Height < height)
            //{
            _bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            _canvas = new Canvas(_bitmap);
            _data = new int[width * height];
            //}
        }

        private float GetFontHeight()
        {
            return (_fontMetrix.Bottom - _fontMetrix.Top) + _paint.Descent(); // / _fontScaleFactor + 1f;
        }

        private CCSize GetMeasureString(string text)
        {
            //var bounds = new Rect();
            //_paint.GetTextBounds(text, 0, text.Length, bounds);
            //return new CCSize(bounds.Width(), bounds.Height());
            return new CCSize(_paint.MeasureText(text), GetFontHeight());
        }

        private KerningInfo GetKerningInfo(char ch)
        {
            float[] widths = new float[1];

            _paint.GetTextWidths(new char[] { ch }, 0, 1, widths);
            //var bounds = new Rect();
            //var s = ch.ToString();
            //_paint.GetTextBounds(s, 0, 1, bounds);

            var result = new KerningInfo();
            //result.A = -bounds.Left;
            //result.B = bounds.Width() + bounds.Left;

            result.B = widths[0];
            return result;
        }

        private unsafe byte* GetBitmapData(string text, out int stride)
        {
            if (_dataHandle.IsAllocated)
            {
                _dataHandle.Free();
            }

            var size = GetMeasureString(text);

            var w = (int)(Math.Ceiling(size.Width += 2));
            var h = (int)(Math.Ceiling(size.Height += 2));

            CreateBitmap(w, h);

            _canvas.DrawColor(Android.Graphics.Color.Black);

            _paint.TextAlign = Paint.Align.Left;

            // Get bounding rectangle - we need its attribute and method values
            Rect r = new Rect();
            _paint.GetTextBounds(text, 0, text.Length, r); // Note: r.top will be negative

            float textX = 0;
            float textY = 0;

            //Calculate base line
            textY = (_fontMetrix.Descent - _fontMetrix.Ascent + _fontMetrix.Leading); // / _fontScaleFactor; //GetFontHeight();// -(r.Height() + r.Top);

            _canvas.DrawText(text, textX, textY, _paint);


            //float x = 0;
            //float y = GetFontHeight() - _fontMetrix.Bottom;

            //var bounds = new Rect();
            //_paint.GetTextBounds(text, 0, text.Length, bounds);

            //if ((int) Android.OS.Build.VERSION.SdkInt <= 15)
            {
                //draw normally
                //_paint.TextAlign = Paint.Align.Left;
                //_canvas.DrawText(text, 0, 0, _paint);
                //_canvas.DrawText(text, -bounds.Left, (-_fontMetrix.Ascent + _fontMetrix.Descent) / 2f, _paint);
            }
            /*
            else
            {

                //workaround
                float originalTextSize = _paint.TextSize;

                float magnifier = 100f;

                _canvas.Save();

                _canvas.Scale(1f / magnifier, 1f / magnifier);
                _paint.TextSize = originalTextSize * magnifier;
                _canvas.DrawText(s, x * magnifier, y * magnifier, _paint);

                _canvas.Restore();

                _paint.TextSize = originalTextSize;
            }
            */

            /*
            var _buffer = ByteBuffer.Wrap(_data);
            _buffer.Order(ByteOrder.NativeOrder());

            //_buffer.Rewind();
            _bitmap.CopyPixelsToBuffer(_buffer);
            
            _buffer.Dispose();

            stride = _bitmap.Width;

            _dataHandle = GCHandle.Alloc(_data, GCHandleType.Pinned);

            return (byte*)_dataHandle.AddrOfPinnedObject().ToPointer();
            */

            _bitmap.GetPixels(_data, 0, _bitmap.Width, 0, 0, _bitmap.Width, _bitmap.Height);

            stride = _bitmap.Width * 4;

            _dataHandle = GCHandle.Alloc(_data, GCHandleType.Pinned);
            return (byte*)_dataHandle.AddrOfPinnedObject().ToPointer();
        }
    }
}