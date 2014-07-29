using System;
using System.IO;
using System.Diagnostics;

namespace CocosSharp
{

    public enum CCResolutionPolicy
    {
        UnKnown,

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
        CCViewport viewport;
        CCWindow window;

        internal event EventHandler SceneViewportChanged = delegate {};
        CCResolutionPolicy resolutionPolicy = CCResolutionPolicy.UnKnown;

        #region Properties

        public CCResolutionPolicy ResolutionPolicy 
        { 
            get { return resolutionPolicy; }
            set 
            { 
                if (value != resolutionPolicy)
                {
                    resolutionPolicy = value;
                    Viewport.UpdateViewport();
                }
            }
        }

        public virtual bool IsTransition
        {
            get { return false; }
        }

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

        #endregion Properties


        #region Constructors

        public CCScene(CCWindow window, CCViewport viewport, CCDirector director)
        {
            IgnoreAnchorPointForPosition = true;
            AnchorPoint = new CCPoint(0.5f, 0.5f);
            Viewport = viewport;
            Window = window;
            Director = director;

			if (window != null && director != null)
            	window.AddSceneDirector(director);

            ResolutionPolicy = CCResolutionPolicy.ExactFit;
        }

        public CCScene(CCWindow window, CCDirector director) 
            : this(window, new CCViewport(new CCRect (0.0f, 0.0f, 1.0f, 1.0f)), director)
        {
        }

        public CCScene(CCWindow window) 
            : this(window, null)
        {
        }

        public CCScene(CCScene scene): this(scene.Window, scene.Viewport, scene.Director)
        {
        }

        #endregion Constructors


        #region Viewport handling

        void OnViewportChanged(object sender, EventArgs e)
        {
            CCViewport viewport = sender as CCViewport;

            if(viewport != null && viewport == Viewport) 
            {
                SceneViewportChanged(this, null);
            }
        }

        #endregion Viewport handling

        protected override void Draw()
        {
            CCDrawManager drawManager = Window.DrawManager;

            base.Draw();

            drawManager.Viewport = Viewport.XnaViewport; 
        }
    }
}