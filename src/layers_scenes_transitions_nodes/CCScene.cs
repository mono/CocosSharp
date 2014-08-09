using System;
using System.IO;
using System.Diagnostics;

namespace CocosSharp
{

    public enum CCSceneResolutionPolicy
    {
        // The Viewport is not automatically calculated and it is up to the developer to take care of setting
        // the values correctly.
        Custom,

        // The entire application is visible in the specified area without trying to preserve the original aspect ratio. 
        // Distortion can occur, and the application may appear stretched or compressed.
        ExactFit,
        // The entire application fills the specified area, without distortion but possibly with some cropping, 
        // while maintaining the original aspect ratio of the application.
        NoBorder,
        // The entire application is visible in the specified area without distortion while maintaining the original 
        // aspect ratio of the application. Borders can appear on two sides of the application.
        ShowAll,
        // The application takes the height of the design resolution size and modifies the width of the internal
        // canvas so that it fits the aspect ratio of the device
        // no distortion will occur however you must make sure your application works on different
        // aspect ratios
        FixedHeight,
        // The application takes the width of the design resolution size and modifies the height of the internal
        // canvas so that it fits the aspect ratio of the device
        // no distortion will occur however you must make sure your application works on different
        // aspect ratios
        FixedWidth
    }



    /// <summary>
    /// brief CCScene is a subclass of CCNode that is used only as an abstract concept.
    /// CCScene and CCNode are almost identical with the difference that CCScene has it's
    /// anchor point (by default) at the center of the screen. Scenes have state management
    /// where they can serialize their state and have it reconstructed upon resurrection.
    ///  It is a good practice to use and CCScene as the parent of all your nodes.
    /// </summary>
    public class CCScene : CCNode
    {
        static readonly CCRect exactFitRatio = new CCRect(0,0,1,1);

        CCViewport viewport;
        CCWindow window;

        internal event EventHandler SceneViewportChanged = delegate {};
        CCSceneResolutionPolicy resolutionPolicy = CCSceneResolutionPolicy.ExactFit;

        #region Properties

        public CCSceneResolutionPolicy SceneResolutionPolicy 
        { 
            get { return resolutionPolicy; }
            set 
            { 
                if (value != resolutionPolicy)
                {
                    resolutionPolicy = value;
                    UpdateResolutionRatios();
                    Viewport.UpdateViewport();
                }
            }
        }

        public virtual bool IsTransition
        {
            get { return false; }
        }

#if USE_PHYSICS
		private CCPhysicsWorld _physicsWorld;

		public CCPhysicsWorld PhysicsWorld
		{
			get { return _physicsWorld; }
			set { _physicsWorld = value; }
		}
#endif

        public CCRect VisibleBoundsScreenspace
        {
            get { return Viewport.ViewportInPixels; }
        }

        public override CCScene Scene
        {
            get { return this; }
        }

        public override CCWindow Window 
        { 
            get { return window; }
            set 
            {
                if(window != value) 
                {
                    window = value;
                    viewport.LandscapeScreenSizeInPixels = Window.LandscapeWindowSizeInPixels;
                }
                if (window != null)
                {
                    InitializeLazySceneGraph(Children);
                }
            }
        }

        void InitializeLazySceneGraph(CCRawList<CCNode> children)
        {
            if (children == null)
                return;

            foreach (var child in children)
            {
                if (child != null)
                {
                    child.AttachEvents();
                    child.AttachActions();
                    child.AttachSchedules ();
                    InitializeLazySceneGraph(child.Children);
                }
            }
        }

        public override CCDirector Director { get; set; }

        public override CCCamera Camera 
        { 
            get { return null; }
            set 
            {
            }
        }

        public override CCViewport Viewport 
        {
            get { return viewport; }
            set 
            {
                if (viewport != value) 
                {
                    // Stop listening to previous viewport's event
                    if(viewport != null)
                        viewport.OnViewportChanged -= OnViewportChanged;

                    viewport = value;

                    viewport.OnViewportChanged += OnViewportChanged;

                    OnViewportChanged(this, null);
                }
            }
        }

        internal override CCEventDispatcher EventDispatcher 
        { 
            get { return Window != null ? Window.EventDispatcher : null; }
        }

        public override CCSize ContentSize
        {
            get { return CCSize.Zero; }
            set {}
        }

        public override CCAffineTransform AffineLocalTransform
        {
            get
            {
                return CCAffineTransform.Identity;
            }
        }

        #endregion Properties


        #region Constructors

#if USE_PHYSICS
		public CCScene(CCWindow window, CCViewport viewport, CCDirector director = null, bool physics = false)
#else
		public CCScene(CCWindow window, CCViewport viewport, CCDirector director = null)
#endif
        {
            IgnoreAnchorPointForPosition = true;
            AnchorPoint = new CCPoint(0.5f, 0.5f);
            Viewport = viewport;
            Window = window;
            Director = (director == null) ? window.DefaultDirector : director;

            if (window != null && director != null)
                window.AddSceneDirector(director);

#if USE_PHYSICS
			_physicsWorld = physics ? new CCPhysicsWorld(this) : null;
#endif

            SceneResolutionPolicy = window.DesignResolutionPolicy;
        }

#if USE_PHYSICS
		public CCScene(CCWindow window, CCDirector director, bool physics = false)
			: this(window, new CCViewport(new CCRect(0.0f, 0.0f, 1.0f, 1.0f)), director, physics)
#else
		public CCScene(CCWindow window, CCDirector director)
			: this(window, new CCViewport(new CCRect(0.0f, 0.0f, 1.0f, 1.0f)), director)
#endif
        {
        }

#if USE_PHYSICS
		public CCScene(CCWindow window, bool physics = false)
			: this(window, window.DefaultDirector, physics)
#else
		public CCScene(CCWindow window)
			: this(window, window.DefaultDirector)
#endif
        {
        }

#if USE_PHYSICS
		public CCScene(CCScene scene, bool physics = false)
			: this(scene.Window, scene.Viewport, scene.Director, physics)
#else
		public CCScene(CCScene scene)
			: this(scene.Window, scene.Viewport, scene.Director)
#endif
        {
        }

        #endregion Constructors


        #region Viewport handling

        void OnViewportChanged(object sender, EventArgs e)
        {
            CCViewport viewport = sender as CCViewport;

            if(viewport != null && viewport == Viewport) 
            {
                UpdateResolutionRatios();
                SceneViewportChanged(this, null);
            }
        }

        #endregion Viewport handling

        #region Resolution Policy

        void UpdateResolutionRatios ()
        {

            if (Children != null && SceneResolutionPolicy != CCSceneResolutionPolicy.Custom)
            {
                bool dirtyViewport = false;
                var unionedBounds = CCRect.Zero;

                foreach (var child in Children)
                {
                    if (child != null && child is CCLayer)
                    {
                        var layer = child as CCLayer;
                        unionedBounds.Origin.X = Math.Min (unionedBounds.Origin.X, layer.VisibleBoundsWorldspace.Origin.X);
                        unionedBounds.Origin.Y = Math.Min (unionedBounds.Origin.Y, layer.VisibleBoundsWorldspace.Origin.Y);
                        unionedBounds.Size.Width = Math.Max (unionedBounds.MaxX, layer.VisibleBoundsWorldspace.MaxX) - unionedBounds.Origin.X;
                        unionedBounds.Size.Height = Math.Max (unionedBounds.MaxY, layer.VisibleBoundsWorldspace.MaxY) - unionedBounds.Origin.Y;
                    }
                }

                // Calculate viewport ratios if not set to custom

                //var resolutionPolicy = Scene.SceneResolutionPolicy;

                if (unionedBounds != CCRect.Zero)
                {
                    // Calculate Landscape Ratio
                    var viewportRect = CalculateResolutionRatio(unionedBounds, resolutionPolicy);
                    dirtyViewport = Viewport.exactFitLandscapeRatio != viewportRect;
                    Viewport.exactFitLandscapeRatio = viewportRect;

                    // Calculate Portrait Ratio
                    var portraitBounds = unionedBounds.InvertedSize;
                    viewportRect = CalculateResolutionRatio(portraitBounds, resolutionPolicy);
                    dirtyViewport = Viewport.exactFitPortraitRatio != viewportRect;
                    Viewport.exactFitPortraitRatio = viewportRect;

                    // End Calculate viewport ratios

                }

                if (dirtyViewport)
                    Viewport.UpdateViewport(false);
            }
        }


        CCRect CalculateResolutionRatio(CCRect resolutionRect, CCSceneResolutionPolicy resolutionPolicy)
        {

            var width = resolutionRect.Size.Width;
            var height = resolutionRect.Size.Height;

            if (width == 0.0f || height == 0.0f)
            {
                return exactFitRatio;
            }

            var x = resolutionRect.Origin.X;
            var y = resolutionRect.Origin.Y;

            var designResolutionSize = resolutionRect.Size;
            var viewPortRect = CCRect.Zero;
            float resolutionScaleX, resolutionScaleY;

            // Not set anywhere right now.
            var frameZoomFactor = 1;

            var screenSize = Scene.Window.WindowSizeInPixels;

            resolutionScaleX = screenSize.Width / designResolutionSize.Width;
            resolutionScaleY = screenSize.Height / designResolutionSize.Height;

            if (resolutionPolicy == CCSceneResolutionPolicy.NoBorder)
            {
                resolutionScaleX = resolutionScaleY = Math.Max(resolutionScaleX, resolutionScaleY);
            }

            if (resolutionPolicy == CCSceneResolutionPolicy.ShowAll)
            {
                resolutionScaleX = resolutionScaleY = Math.Min(resolutionScaleX, resolutionScaleY);
            }


            if (resolutionPolicy == CCSceneResolutionPolicy.FixedHeight)
            {
                resolutionScaleX = resolutionScaleY;
                designResolutionSize.Width = (float)Math.Ceiling(screenSize.Width / resolutionScaleX);
            }

            if (resolutionPolicy == CCSceneResolutionPolicy.FixedWidth)
            {
                resolutionScaleY = resolutionScaleX;
                designResolutionSize.Height = (float)Math.Ceiling(screenSize.Height / resolutionScaleY);
            }

            // calculate the rect of viewport    
            float viewPortW = designResolutionSize.Width * resolutionScaleX;
            float viewPortH = designResolutionSize.Height * resolutionScaleY;

            viewPortRect = new CCRect((screenSize.Width - viewPortW) / 2, (screenSize.Height - viewPortH) / 2, viewPortW, viewPortH);

            var viewportRatio = new CCRect(
                ((x * resolutionScaleX * frameZoomFactor + viewPortRect.Origin.X * frameZoomFactor) / screenSize.Width),
                ((y * resolutionScaleY * frameZoomFactor + viewPortRect.Origin.Y * frameZoomFactor) / screenSize.Height),
                ((width * resolutionScaleX * frameZoomFactor) / screenSize.Width),
                ((height * resolutionScaleY * frameZoomFactor) / screenSize.Height)
            );

            return viewportRatio;

        }

        #endregion

		#region Physics


#if USE_PHYSICS

		public override void AddChild(CCNode child, int zOrder, int tag)
		{
			base.AddChild(child, zOrder, tag);
			AddChildToPhysicsWorld(child);
		}

		public override void Update(float dt)
		{
			base.Update(dt);

			if (_physicsWorld != null)
				_physicsWorld.Update(dt);
		}


		protected internal virtual void AddChildToPhysicsWorld(CCNode child)
		{
			if (_physicsWorld != null)
			{
				Action<CCNode> addToPhysicsWorldFunc = null;

				addToPhysicsWorldFunc = new Action<CCNode>(node =>
				{
					if (node.PhysicsBody != null)
					{
						_physicsWorld.AddBody(node.PhysicsBody);
					}

					var children = node.Children;
					if (children != null)
						foreach (var n in children)
						{
							addToPhysicsWorldFunc(n);
						}

				});

				addToPhysicsWorldFunc(child);

			}
		}

#endif
		
		#endregion

        public override void Visit()
        {
            CCDrawManager drawManager = Window.DrawManager;

            if(drawManager.CurrentRenderTarget == null)
                drawManager.Viewport = Viewport.XnaViewport; 

            base.Visit();
        }
    }
}