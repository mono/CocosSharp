using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Wpf.Interop.DirectX;

namespace CocosSharp
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CocosSharp.WPF"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:CocosSharp.WPF;assembly=CocosSharp.WPF"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>

    public class GameDesktop : ContentControl
    {
        protected virtual void Dispose(bool disposing)
        {
        }
    }
    public partial class CCGameView : GameDesktop
    {
        static CCGameView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CCGameView), new FrameworkPropertyMetadata(typeof(CCGameView)));
        }

        D3D11Image d3dImage;
        Image image;

        #region Constructors

        public CCGameView()
        {
            BeginInitialise();
        }

        #endregion Constructors


        #region Initialisation

        void BeginInitialise()
        {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewSize = new CCSizeI((int)ActualWidth, (int)ActualHeight);

            HwndSource source = (HwndSource)HwndSource.FromVisual(this);
            IntPtr hWnd = source.Handle;

            d3dImage = new D3D11Image();
            d3dImage.OnRender = PlatformPresent;
            d3dImage.WindowOwner = hWnd;

            image = new Image { Source = d3dImage, Stretch = Stretch.None };
            AddChild(image);

            Initialise();
        }

        void PlatformInitialise()
        {
            SizeChanged += ViewSizeChanged;
        }

        void CreateRenderTarget()
        {
            if (graphicsDevice != null)
            {                
                //renderTarget = new RenderTarget2D(graphicsDevice, (int)ActualWidth, (int)ActualHeight,
                //	false, SurfaceFormat.Bgra32, DepthFormat.Depth24Stencil8, 1,
                //	RenderTargetUsage.PlatformContents, true);

                //var handle = renderTarget.GetSharedHandle();
                //if (handle == IntPtr.Zero)
                //	throw new ArgumentException("Handle could not be retrieved");

                //renderTargetD3D9 = new SharpTexture(DeviceService.D3DDevice,
                //	renderTarget.Width, renderTarget.Height,
                //	1, SharpDX.Direct3D9.Usage.RenderTarget, SharpDX.Direct3D9.Format.A8R8G8B8,
                //	SharpDX.Direct3D9.Pool.Default, ref handle);

                //using (SharpDX.Direct3D9.Surface surface = renderTargetD3D9.GetSurfaceLevel(0))
                //{
                //	d3dImage.Lock();
                //	d3dImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, surface.NativePointer);
                //	d3dImage.Unlock();
                //}
            }
        }

        void PlatformInitialiseGraphicsDevice(ref PresentationParameters presParams)
        {
            HwndSource source = (HwndSource)HwndSource.FromVisual(this);
            IntPtr hWnd = source.Handle;
            presParams.DeviceWindowHandle = hWnd;
        }

        void PlatformStartGame()
        {
            CreateRenderTarget();

            CompositionTarget.Rendering += OnUpdateFrame;

            viewportDirty = true;
        }

        void InitialiseInputHandling()
        {

        }

        #endregion Initialisation


        #region Cleaning up

        void PlatformDispose(bool disposing)
        {

        }

        bool PlatformCanDisposeGraphicsDevice()
        {
            return true;
        }
        void OnUnloaded(object sender, RoutedEventArgs e)
        {
        }

        #endregion Cleaning up


        #region Rendering

        void ViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewSize = new CCSizeI((int)e.NewSize.Width, (int)e.NewSize.Height);

            d3dImage.SetPixelSize(ViewSize.Width, ViewSize.Height);

            // We need to ask the graphics device to resize the underlying swapchain/buffers
            if (graphicsDevice != null)
            {
                graphicsDevice.PresentationParameters.BackBufferWidth = ViewSize.Width;
                graphicsDevice.PresentationParameters.BackBufferHeight = ViewSize.Height;
                graphicsDevice.CreateSizeDependentResources();
                graphicsDevice.ApplyRenderTargets(null);
            }

            UpdateViewport();

            platformInitialised = true;

            LoadGame();
        }

        #endregion Rendering


        #region Run loop

        void PlatformUpdatePaused()
        {

            //MobilePlatformUpdatePaused();
        }

        void OnUpdateFrame(object sender, EventArgs e)
        {
            Tick();

            Draw();

            d3dImage.RequestRender();
        }

        void PlatformPresent(IntPtr resourcePointer, bool isNewSurface)
        {
            if (graphicsDevice != null)
                graphicsDevice.Present();
        }

        void ProcessInput()
        {

        }

        #endregion Run loop


        #region Touch handling

        //void PlatformUpdateTouchEnabled()
        //{
        //    if (TouchEnabled)
        //    {
        //    }
        //    else
        //    {
        //    }
        //}
        #endregion Touch handling

        #region Mouse Handling
        void PlatformUpdateMouseEnabled()
        {

        }

        void PlatformUpdateMouseVisible()
        {

        }
        #endregion Mouse Handling

    }
}