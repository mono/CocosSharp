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

        #if !NETFX_CORE
        public CCAccelerometer Accelerometer { get; set; }
        #endif

        public CCDirector Director { get; private set; }
        public CCRenderer Renderer { get { return DrawManager != null ? DrawManager.Renderer : null; } }
        public CCActionManager ActionManager { get; private set; }

        public bool DepthTesting
        {
            get { return Renderer.UsingDepthTest; }
            set { Renderer.UsingDepthTest = value; }
        }

        public bool DisplayStats 
        {
            get { return Stats.IsEnabled; }
            set { Stats.IsEnabled = value; }
        }

        public int StatsScale
        {
            get { return Stats.Scale; }
            set { Stats.Scale = value; }
        }

        internal CCEventDispatcher EventDispatcher { get; private set; }
        internal CCDrawManager DrawManager { get; private set; }

        CCStats Stats { get; set; }


        #endregion Properties

        public void StartGame()
        {
            if(!gameStarted)
            {
                PlatformStartGame();
                gameStarted = true;
            }
        }

        public void RunWithScene(CCScene scene)
        {
            Director.RunWithScene(scene);
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
            Director = new CCDirector();

            Stats.Initialize();

            ViewCreated(this, null);
        }

        internal void Present()
        {
            PlatformPresent();
        }

        void DrawScene()
        {
            DrawManager.BeginDraw();

            CCScene runningScene = Director.RunningScene;

            if (runningScene != null) 
            {
                runningScene.Visit();

                Renderer.VisitRenderQueue();
            }

            DrawManager.EndDraw();
        }
    
    }
}

