using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    using XnaSurfaceFormat = Microsoft.Xna.Framework.Graphics.SurfaceFormat;

    public partial class CCGameView
    {
        bool gameStarted;
        GraphicsDevice graphicsDevice;


        #region Properties

        public CCScene RootScene { get; set; }
        public CCRenderer Renderer { get { return DrawManager != null ? DrawManager.Renderer : null; } }
        public CCScheduler Scheduler { get; private set; }
        public CCActionManager ActionManager { get; private set; }

        CCDrawManager DrawManager { get; set; }

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
            var graphicsProfile = GraphicsProfile.HiDef;

            var presParams = new PresentationParameters();
            presParams.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            presParams.DepthStencilFormat = DepthFormat.Depth24Stencil8;
            presParams.BackBufferFormat = XnaSurfaceFormat.Color;
            presParams.RenderTargetUsage = RenderTargetUsage.PreserveContents;

            graphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, graphicsProfile, presParams);
            DrawManager = new CCDrawManager(graphicsDevice);

            Scheduler = new CCScheduler();
            ActionManager = new CCActionManager();

            PlatformInitialise();
        }

        internal void Present()
        {
            PlatformPresent();
        }

        void DrawScene()
        {
            if(RootScene != null)
            {
                RootScene.Visit();
                Renderer.VisitRenderQueue();
            }
        }
    
    }
}

