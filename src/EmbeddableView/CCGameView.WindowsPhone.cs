using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace CocosSharp
{
    public partial class CCGameView : SwapChainPanel
    {        
        #region Constructors

        public CCGameView()
        {
            Initialise();
        }

        #endregion Constructors


        #region Initialisation

        void PlatformInitialise()
        {
            SizeChanged += ViewSizeChanged;
        }

        void PlatformInitialiseGraphicsDevice(ref PresentationParameters presParams)
        {
            presParams.SwapChainBackgroundPanel = this;
        }

        void PlatformStartGame()
        {
            CompositionTarget.Rendering += OnUpdateFrame;
            viewportDirty = true;
        }

        #endregion Initialisation


        #region Rendering

        void ViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewSize = new CCSizeI((int)e.NewSize.Width, (int)e.NewSize.Height);
            
            // We need to ask the graphics device to resize the underlying swapchain/buffers
            graphicsDevice.PresentationParameters.BackBufferWidth = ViewSize.Width;
            graphicsDevice.PresentationParameters.BackBufferHeight = ViewSize.Height;
            graphicsDevice.CreateSizeDependentResources();
            graphicsDevice.ApplyRenderTargets(null);
            
            viewportDirty = true;
        }

        #endregion Rendering


        #region Run loop

        void OnUpdateFrame(object sender, object e)
        {
            Tick();

            graphicsDevice.ResetRenderTargets();

            Draw();

            PlatformPresent();
        }

        void PlatformPresent()
        {
            if (graphicsDevice != null)
                graphicsDevice.Present();
        }

        #endregion Run loop


        #region Touch handling

        void PlatformUpdateTouchHandling()
        {

        }

        void PlatformUpdateTouchEnabled()
        {
        }

        #endregion Touch handling
    }
}