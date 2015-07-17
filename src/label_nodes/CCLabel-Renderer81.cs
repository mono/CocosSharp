using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.DirectWrite;

using SharpDX.WIC;

using FactoryImaging = SharpDX.WIC.ImagingFactory;

namespace CocosSharp
{
    internal class CCLabel_Renderer81
    {

        private Factory2 Factory2D { get; set; }
        private FactoryImaging FactoryImaging { get; set; }

        private SharpDX.Direct3D11.Device device;
        private SharpDX.DXGI.Device1 dxgiDevice;
        private SharpDX.Direct2D1.Device1 d2dDevice;
        private SharpDX.Direct2D1.DeviceContext1 d2dResourceCreationDeviceContext;

        const float DEFAULT_DPI = 72.0f;

        public CCLabel_Renderer81()
        {
            var d2dFactory = CreateD2DFactory();

            var newDevice = CreateDevice();

            CreateRenderingResources(newDevice, d2dFactory);

            FactoryImaging = new SharpDX.WIC.ImagingFactory();
        }

        SharpDX.Direct3D11.Device CreateDevice(bool useSoftwareRenderer = false)
        {
            
            // We try to get the existing MonoGame 3D11 Device first
            var device = CCDrawManager.SharedDrawManager.XnaGraphicsDevice.Handle as SharpDX.Direct3D11.Device;

            if (device != null)
                return device;

            // If we get here then we probably should error out but let's keep going anyway!!


            // Windows requires BGRA support out of DX.
            var deviceFlags = SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport;
#if DEBUG
            deviceFlags |= SharpDX.Direct3D11.DeviceCreationFlags.Debug;
#endif

            var driverType =  useSoftwareRenderer ? DriverType.Software : DriverType.Hardware;

#if DEBUG
            try
            {
#endif
                device = new SharpDX.Direct3D11.Device(
                    driverType,
                    deviceFlags);

#if DEBUG
            }
            catch (SharpDXException)
            {
                // Try again without the debug flag.  This allows debug builds to run
                // on machines that don't have the debug runtime installed.
                deviceFlags &= ~SharpDX.Direct3D11.DeviceCreationFlags.Debug;
                device = new SharpDX.Direct3D11.Device(
                        driverType,
                        deviceFlags);

#endif
            }

            return device;
        }

        void CreateRenderingResources (SharpDX.Direct3D11.Device device3d, SharpDX.Direct2D1.Factory2 d2dFactory)
        {
            device = device3d;
            Factory2D = d2dFactory;

            dxgiDevice = device.QueryInterface<SharpDX.DXGI.Device1>();

            d2dDevice = new SharpDX.Direct2D1.Device1(Factory2D, dxgiDevice);

            d2dResourceCreationDeviceContext = new SharpDX.Direct2D1.DeviceContext1(d2dDevice, DeviceContextOptions.None);

        }

        public 

        SharpDX.Direct2D1.Factory2 CreateD2DFactory(DebugLevel debugLevel = DebugLevel.None)
        {
            return Factory2D = new SharpDX.Direct2D1.Factory2(
                    SharpDX.Direct2D1.FactoryType.MultiThreaded, debugLevel);
        }

        public SharpDX.Direct2D1.DeviceContext1 CreateDrawingContext(
            Bitmap1 renderTarget, 
            DeviceContextOptions options = DeviceContextOptions.None)
        {

            var drawingContext = new SharpDX.Direct2D1.DeviceContext1(d2dDevice, options);
            drawingContext.Target = renderTarget;
            return drawingContext;

        }

        public SharpDX.Direct2D1.Bitmap1 CreateRenderTarget(
            float width,
            float height,
            float dpi = DEFAULT_DPI,
            SharpDX.DXGI.Format format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
            SharpDX.Direct2D1.AlphaMode alpha = AlphaMode.Premultiplied)
        {

            var bitmapProperties = new SharpDX.Direct2D1.BitmapProperties1()
            {
                BitmapOptions = BitmapOptions.Target,
                DpiX = dpi,
                DpiY = dpi,
                PixelFormat = new SharpDX.Direct2D1.PixelFormat(format, alpha)
            };

            var pixelWidth = SizeDipsToPixels(width, dpi);
            var pixelHeight = SizeDipsToPixels(height, dpi);
            
            var bitmap = new SharpDX.Direct2D1.Bitmap1(d2dResourceCreationDeviceContext,
                new Size2(pixelWidth, pixelHeight), bitmapProperties);

            return bitmap;
        }

        private static Color4 TransparentColor = new Color4(new Color3(1, 1.0f, 1.0f), 0.0f);

        public SharpDX.Direct2D1.Bitmap1 RenderLabel (
            float imageWidth,
            float imageHeight,
            Color4 foregroundColor,
            Vector2 origin,
            TextLayout textLayout,
            float dpi = DEFAULT_DPI,
            SharpDX.DXGI.Format format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
            SharpDX.Direct2D1.AlphaMode alpha = AlphaMode.Premultiplied)
        {
            var renderTarget = CreateRenderTarget(imageWidth, imageHeight, dpi, format, alpha);

            using (var drawingContext = CreateDrawingContext(renderTarget))
            {
                // Begin our drawing
                drawingContext.BeginDraw();

                // Clear to transparent
                drawingContext.Clear(TransparentColor);

                // Create our brush to actually draw with
                var solidBrush = new SolidColorBrush(drawingContext, foregroundColor);
                // Draw the text to the bitmap
                drawingContext.DrawTextLayout(origin, textLayout, solidBrush);

                // End our drawing
                drawingContext.EndDraw();
            }

            return renderTarget;
        }

        public System.IO.MemoryStream RenderLabelToStream(
            float imageWidth,
            float imageHeight,
            Color4 foregroundColor,
            Vector2 origin,
            TextLayout textLayout,
            float dpi = DEFAULT_DPI,
            SharpDX.DXGI.Format format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
            SharpDX.Direct2D1.AlphaMode alpha = AlphaMode.Premultiplied)
        {

            var renderTarget = RenderLabel(
                imageWidth,
                imageHeight,
                foregroundColor,
                origin,
                textLayout,
                dpi,
                format,
                alpha);

            // Get stream
            var pngStream = new MemoryStream();
            
            pngStream.Position = 0;

            // Create a WIC outputstream
            using (var wicStream = new WICStream(FactoryImaging, pngStream))
            {

                var size = renderTarget.PixelSize;

                // Initialize a Png encoder with this stream
                using (var wicBitmapEncoder = new PngBitmapEncoder(FactoryImaging, wicStream))
                {

                    // Create a Frame encoder
                    using (var wicFrameEncoder = new BitmapFrameEncode(wicBitmapEncoder))
                    {
                        wicFrameEncoder.Initialize();

                        // Create image encoder
                        ImageEncoder wicImageEncoder;
                        ImagingFactory2 factory2 = new ImagingFactory2();
                        wicImageEncoder = new ImageEncoder(factory2, D2DDevice);


                        var imgParams = new ImageParameters();
                        imgParams.PixelFormat =
                            new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, 
                                                                AlphaMode.Premultiplied);

                        imgParams.PixelHeight = (int)size.Height;
                        imgParams.PixelWidth = (int)size.Width;

                        wicImageEncoder.WriteFrame(renderTarget, wicFrameEncoder, imgParams);

                        //// Commit changes
                        wicFrameEncoder.Commit();
                        wicBitmapEncoder.Commit();

                        byte[] buffer = new byte[pngStream.Length];
                        pngStream.Position = 0;
                        pngStream.Read(buffer, 0, (int)pngStream.Length);
                    }
                }
            }

            return pngStream;
        }

        public SharpDX.Direct2D1.Device1 D2DDevice
        {
            get { return d2dDevice;  }
        }

        static float PixelsToDips(int pixels, float dpi)
        {
            return pixels * DEFAULT_DPI / dpi;
        }

        static int DipsToPixels(
            float dips,
            float dpi,
            DpiRounding dpiRounding)
        {
            float scaled = dips * dpi / DEFAULT_DPI;
            switch (dpiRounding)
            {
                case DpiRounding.Floor: scaled = (float)Math.Floor(scaled); break;
                case DpiRounding.Round:
                default:
                    scaled = (float)Math.Round(scaled); break;
                case DpiRounding.Ceiling: scaled = (float)Math.Ceiling(scaled); break;
            }

            return (int)(scaled);
        }

        static int SizeDipsToPixels(
            float dips,
            float dpi)
        {
            int result = DipsToPixels(dips, dpi, DpiRounding.Round);

            // Zero versus non-zero is pretty important for things like control sizes, so we want
            // to avoid ever rounding non-zero input sizes down to zero during conversion to pixels.
            // If the input value was small but positive, it's safer to round up to one instead.
            if (result == 0 && dips > 0)
            {
                return 1;
            }

            return result;
        }

        private enum DpiRounding
        {
            Ceiling,
            Floor,
            Round
        }
    }
}
