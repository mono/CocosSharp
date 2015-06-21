using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    using XnaSurfaceFormat = Microsoft.Xna.Framework.Graphics.SurfaceFormat;

    public partial class CCGameView
    {
        class CCGraphicsDeviceService : IGraphicsDeviceService
        {
            public GraphicsDevice GraphicsDevice { get; private set; }

            public CCGraphicsDeviceService(GraphicsDevice graphicsDevice)
            {
                GraphicsDevice = graphicsDevice;
            }

            public event EventHandler<EventArgs> DeviceCreated;
            public event EventHandler<EventArgs> DeviceDisposing;
            public event EventHandler<EventArgs> DeviceReset;
            public event EventHandler<EventArgs> DeviceResetting;
        }

        public event EventHandler<EventArgs> ViewCreated;

        bool gameStarted;
        GraphicsDevice graphicsDevice;
        CCGraphicsDeviceService graphicsDeviceService;
        GameServiceContainer servicesContainer;
       

        #region Properties

        public CCScene RootScene { get; set; }
        public CCRenderer Renderer { get { return DrawManager != null ? DrawManager.Renderer : null; } }
        public CCActionManager ActionManager { get; private set; }

        internal CCDrawManager DrawManager { get; private set; }

        #endregion Properties


        public void StartGame()
        {
            if(!gameStarted)
            {
                PlatformStartGame();
                gameStarted = true;
            }
        }
            
        void Initialise()
        {
            PlatformInitialise();

            var graphicsProfile = GraphicsProfile.HiDef;

            var presParams = new PresentationParameters();
            presParams.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            presParams.DepthStencilFormat = DepthFormat.Depth24Stencil8;
            presParams.BackBufferFormat = XnaSurfaceFormat.Color;
            presParams.RenderTargetUsage = RenderTargetUsage.PreserveContents;

            graphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, graphicsProfile, presParams);
            DrawManager = new CCDrawManager(graphicsDevice);
            CCDrawManager.SharedDrawManager = DrawManager;

            graphicsDeviceService = new CCGraphicsDeviceService(graphicsDevice);

            var serviceProvider = CCContentManager.SharedContentManager.ServiceProvider as GameServiceContainer;
            serviceProvider.AddService(typeof(IGraphicsDeviceService), graphicsDeviceService);

            ActionManager = new CCActionManager();

            ViewCreated(this, null);
        }

        internal void Present()
        {
            PlatformPresent();
        }

        void DrawScene()
        {
            DrawManager.BeginDraw();

            if(RootScene != null)
            {
                RootScene.Visit();
                Renderer.VisitRenderQueue();
            }

            DrawManager.EndDraw();
        }
    
    }
}

