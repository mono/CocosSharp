using System;
using System.Runtime.InteropServices;
using Android.App;
using Android.Graphics;

namespace Cocos2D
{
    public partial class CCLabel
    {
        private static Paint _paint;
		private static Bitmap _bitmap;
        private static Canvas _canvas;
        private static int[] _data;
        private static GCHandle _dataHandle;
        private static Paint.FontMetrics _fontMetrix;

        private void CreateFont(string fontName, float fontSize, CCRawList<char> charset)
        {
            if (_paint == null)
            {
                _paint = new Paint(PaintFlags.AntiAlias);
                _paint.Color = Android.Graphics.Color.White;
                _paint.TextAlign = Paint.Align.Left;
                _paint.LinearText = true;
            }

            _paint.TextSize = fontSize;

            var ext = System.IO.Path.GetExtension(fontName);
            if (!String.IsNullOrEmpty(ext) && ext.ToLower() == ".ttf")
            {
                var path = System.IO.Path.Combine(CCApplication.SharedApplication.Game.Content.RootDirectory, fontName);
                var activity = (Activity) CCApplication.SharedApplication.Game.Window.Context;

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
            if (_bitmap == null || _bitmap.Width < width || _bitmap.Height < height)
            {
                _bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
                _canvas = new Canvas(_bitmap);
                _data = new int[width * height];
            }
        }

        private float GetFontHeight()
        {
            return (_fontMetrix.Bottom - _fontMetrix.Top);
        }

        private CCSize GetMeasureString(string text)
        {
            var bounds = new Rect();
            _paint.GetTextBounds(text, 0, text.Length, bounds);
            return new CCSize(bounds.Width(), bounds.Height());
        }

        private KerningInfo GetKerningInfo(char ch)
        {
            var result = new KerningInfo();
            result.B = GetMeasureString(ch.ToString()).Width;
            return result;
        }

        private unsafe byte* GetBitmapData(string s, out int stride)
        {
            if (_dataHandle.IsAllocated)
            {
                _dataHandle.Free();
            }

            var size = GetMeasureString(s);

            CreateBitmap((int) size.Width + 2, (int) size.Height + 2);

            _canvas.DrawColor(Android.Graphics.Color.Black);
            
            float x = 0;
            float y = (_fontMetrix.Bottom - _fontMetrix.Top) / 2f;

            //if ((int) Android.OS.Build.VERSION.SdkInt <= 15)
            {
                //draw normally
                _canvas.DrawText(s, x, y, _paint);
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
            return (byte*) _dataHandle.AddrOfPinnedObject().ToPointer();
        }
    }
}
