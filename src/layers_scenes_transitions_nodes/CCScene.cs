using System;
using System.IO;

namespace CocosSharp
{
    /// <summary>
    /// brief CCScene is a subclass of CCNode that is used only as an abstract concept.
    /// CCScene and CCNode are almost identical with the difference that CCScene has it's
    /// anchor point (by default) at the center of the screen. Scenes have state management
    /// where they can serialize their state and have it reconstructed upon resurrection.
    ///  It is a good practice to use and CCScene as the parent of all your nodes.
    /// </summary>
    public class CCScene : CCNode
    {
        CCCamera camera;
        CCViewport viewport;
        CCWindow window;

        internal event EventHandler SceneVisibleBoundsChanged = delegate {};
        internal event EventHandler SceneViewportChanged = delegate {};


        #region Properties

        public virtual bool IsTransition
        {
            get { return false; }
        }

        public CCRect VisibleBoundsScreenspace
        {
            get { return Viewport.ViewportInPixels; }
        }

        public CCRect VisibleBoundsWorldspace
        {
            get { return Camera.VisibleBoundsWorldspace; }
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
            }
        }

        public override CCDirector Director { get; set; }

        public override CCCamera Camera
        {
            get { return camera; }
            set 
            {
                if (camera != value) 
                {
                    // Stop listening to previous camera's event
                    if(camera != null)
                        camera.OnCameraVisibleBoundsChanged -= OnCameraVisibleBoundsChanged;

                    camera = value;

                    camera.OnCameraVisibleBoundsChanged += OnCameraVisibleBoundsChanged;

                    OnCameraVisibleBoundsChanged(this, null);
                }
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
            get { return Camera.VisibleBoundsWorldspace.Size; }
            set {}
        }

        #endregion Properties


        #region Constructors

        public CCScene(CCWindow window, CCCamera camera, CCViewport viewport, CCDirector director)
        {
            IgnoreAnchorPointForPosition = true;
            AnchorPoint = new CCPoint(0.5f, 0.5f);
            Camera = camera;
            Viewport = viewport;
            Window = window;
            Director = director;
        }

        public CCScene(CCScene scene): this(scene.Window, scene.Camera, scene.Viewport, scene.Director)
        {
        }

        #endregion Constructors


        #region Viewport and camera handling

        void OnCameraVisibleBoundsChanged(object sender, EventArgs e)
        {
            CCCamera camera = sender as CCCamera;

            if(camera != null && camera == Camera) 
            {
                SceneVisibleBoundsChanged(this, null);
            }
        }

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
            drawManager.ViewMatrix = Camera.ViewMatrix;
            drawManager.ProjectionMatrix = Camera.ProjectionMatrix;
        }


        #region Unit conversion

        public CCPoint ScreenToWorldspace(CCPoint point)
        {
            CCRect viewportRectInPixels = Viewport.ViewportInPixels;
            CCRect worldBounds = Camera.VisibleBoundsWorldspace;

            point -= viewportRectInPixels.Origin;

            // Note: Screen coordinates have origin in top left corner
            // but world coords have origin in bottom left corner
            // Therefore, Y world ratio is 1 minus Y viewport ratio
            CCPoint worldPointRatio 
                = new CCPoint(point.X / viewportRectInPixels.Size.Width, 1 - (point.Y / viewportRectInPixels.Size.Height));

            return new CCPoint (
                worldBounds.Origin.X + (worldBounds.Size.Width * worldPointRatio.X),
                worldBounds.Origin.Y + (worldBounds.Size.Height * worldPointRatio.Y));
        }

        public CCSize ScreenToWorldspace(CCSize size)
        {
            CCRect viewportRectInPixels = Viewport.ViewportInPixels;
            CCRect worldBounds = Camera.VisibleBoundsWorldspace;

            CCPoint worldSizeRatio = new CCPoint(size.Width / viewportRectInPixels.Size.Width, size.Height / viewportRectInPixels.Size.Height);

            return new CCSize(worldSizeRatio.X * worldBounds.Size.Width, worldSizeRatio.Y * worldBounds.Size.Height);
        }

        public CCSize WorldToScreenspace(CCSize size)
        {
            CCRect visibleBounds = VisibleBoundsWorldspace;
            CCRect viewportInPixels = Viewport.ViewportInPixels;

            CCPoint worldSizeRatio = new CCPoint(size.Width / visibleBounds.Size.Width, size.Height / visibleBounds.Size.Height);

            return new CCSize(worldSizeRatio.X * viewportInPixels.Size.Width, worldSizeRatio.Y * viewportInPixels.Size.Height);
        }

        public CCPoint WorldToScreenspace(CCPoint point)
        {
            CCRect worldBounds = VisibleBoundsWorldspace;
            CCRect viewportRectInPixels = Viewport.ViewportInPixels;

            point -= worldBounds.Origin;

            CCPoint worldPointRatio 
                = new CCPoint(point.X / worldBounds.Size.Width, (point.Y / worldBounds.Size.Height));

            return new CCPoint(viewportRectInPixels.Origin.X + viewportRectInPixels.Size.Width * worldPointRatio.X,
                viewportRectInPixels.Origin.Y + viewportRectInPixels.Size.Height * (1 - worldPointRatio.Y));
        }

        #endregion Unit conversion
    }
}