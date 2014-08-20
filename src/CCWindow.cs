using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public class CCWindow
    {
        public static string EVENT_PROJECTION_CHANGED = "window_projection_changed";
        public static string EVENT_AFTER_UPDATE = "window_after_update";
        public static string EVENT_AFTER_VISIT = "window_after_visit";
        public static string EVENT_AFTER_DRAW = "window_after_draw";

        CCEventCustom eventAfterDraw;
        CCEventCustom eventAfterVisit;
        CCEventCustom eventAfterUpdate;

        List<CCDirector> sceneDirectors;

        #region Properties

        #if !NETFX_CORE
        public CCAccelerometer Accelerometer { get; set; }
        #endif

        public bool GamePadEnabled { get; set; }
        public virtual double AnimationInterval { get; set; }
        public CCNode NotificationNode { get; set; }
        internal CCEventDispatcher EventDispatcher { get; private set; }

        internal List<CCDirector> SceneDirectors { get { return sceneDirectors; } }
        internal CCDrawManager DrawManager { get { return CCDrawManager.SharedDrawManager; } }
        internal GameWindow XnaWindow { get; private set; }
        internal GraphicsDeviceManager DeviceManager { get; private set; }
        internal CCSize designResolutionSize = CCSize.Zero;
        private CCDirector defaultDirector;

        protected CCStats Stats { get; private set; }

        public bool IsUseAlphaBlending
        {
            set 
            {
                if (value)
                {
                    DrawManager.BlendFunc(CCBlendFunc.AlphaBlend);
                }
                else
                {
                    DrawManager.BlendFunc(new CCBlendFunc(CCOGLES.GL_ONE, CCOGLES.GL_ZERO));
                }
            }
        }

        public bool DisplayStats 
        {
            get { return Stats.IsEnabled; }
            set { Stats.IsEnabled = value; }
        }

        public bool AllowUserResizing
        {
            get { return XnaWindow.AllowUserResizing; }
            set { XnaWindow.AllowUserResizing = value; }
        }

        public bool FullScreen 
        { 
            get { return DeviceManager.IsFullScreen; }
            set { DeviceManager.IsFullScreen = value; }
        }

        public bool PreferMultiSampling 
        { 
            get { return DeviceManager.PreferMultiSampling; }
            set { DeviceManager.PreferMultiSampling = value; }
        }

        public CCSize WindowSizeInPixels
        {
            get 
            { 
                Rectangle winBounds = XnaWindow.ClientBounds;
                return new CCSize(winBounds.Width, winBounds.Height); 
            }
        }

        public CCDisplayOrientation SupportedDisplayOrientations
        {
            get { return DrawManager.SupportedDisplayOrientations; }
            set { DrawManager.SupportedDisplayOrientations = value; }
        }

        public CCDisplayOrientation CurrentDisplayOrientation
        {
            get { return (CCDisplayOrientation)XnaWindow.CurrentOrientation; }
            set { DrawManager.CurrentDisplayOrientation = value; }
        }

        public CCApplication Application { get; private set; }

        internal CCSize LandscapeWindowSizeInPixels
        {
            get { return CurrentDisplayOrientation.IsPortrait() ? WindowSizeInPixels.Inverted : WindowSizeInPixels; }
        }

        #endregion Properties


        #region Constructors

        internal CCWindow(CCApplication application, CCSize screenSizeInPixels, GameWindow xnaWindow, GraphicsDeviceManager deviceManager)
        {
            sceneDirectors = new List<CCDirector>();

            AddSceneDirector(new CCDirector());

            EventDispatcher = new CCEventDispatcher(this);
            //Stats = new CCStats();

            #if !NETFX_CORE
            Accelerometer = new CCAccelerometer(this);
            #endif

            eventAfterDraw = new CCEventCustom(EVENT_AFTER_DRAW);
            eventAfterDraw.UserData = this;
            eventAfterVisit = new CCEventCustom(EVENT_AFTER_VISIT);
            eventAfterVisit.UserData = this;
            eventAfterUpdate = new CCEventCustom(EVENT_AFTER_UPDATE);
            eventAfterUpdate.UserData = this;


            IsUseAlphaBlending = true;

            this.XnaWindow = xnaWindow;
            xnaWindow.OrientationChanged += OnOrientationChanged;
            xnaWindow.ClientSizeChanged += OnWindowSizeChanged;
            xnaWindow.AllowUserResizing = true;

            Application = application;

            designResolutionSize = WindowSizeInPixels;
            DesignResolutionPolicy = CCSceneResolutionPolicy.ExactFit;
            //Stats.Initialize();
        }

        #endregion Constructors


        #region Scene director management

        public CCDirector DefaultDirector 
        { 
            get { return defaultDirector; } 
            set 
            {
                if (value != null && value != defaultDirector)
                {
                    if (!sceneDirectors.Contains(value)) 
                    {
                        sceneDirectors.Add(value);
                    }

                    defaultDirector = value;
                }
            }
        }

        public void AddSceneDirector(CCDirector sceneDirector)
        {
            if (sceneDirector != null && !sceneDirectors.Contains(sceneDirector)) 
            {
                sceneDirectors.Add(sceneDirector);
            }

            if (sceneDirectors.Count == 1)
                DefaultDirector = sceneDirector;

        }

        public void RemoveSceneDirector(CCDirector sceneDirector)
        {
            sceneDirectors.Remove(sceneDirector);
            if (DefaultDirector == sceneDirector)
                DefaultDirector = null;

            // TODO: make this smarter
            if (sceneDirectors.Count > 0)
                DefaultDirector = sceneDirectors[0];

        }

        public void RemoveAllSceneDirectors()
        {
            sceneDirectors.Clear();
            DefaultDirector = null;
        }

        public void SetDefaultDirector(int index)
        {
            Debug.Assert(index < sceneDirectors.Count, "CococsSharp CCWindow: index out of range.");
            DefaultDirector = sceneDirectors[index];
        }

        public void RunWithScene(CCScene scene)
        {
            if (scene.Window == null)
                scene.Window = this;

            CCDirector sceneDirector = scene.Director;

            if (sceneDirector != null)
            {
                AddSceneDirector(sceneDirector);
            }
            else
            {
                sceneDirector = DefaultDirector;
            }

            sceneDirector.RunWithScene(scene);
        }


        #endregion Scene director management


        #region Cleaning up

        public void EndAllSceneDirectors()
        {
            foreach (CCDirector director in sceneDirectors) {
                director.End();
            }
        }

        internal void ReInitStats()
        {
            Stats = new CCStats();
        }

        #endregion Cleaning up


        #region Orientation handling

        void OnOrientationChanged(object sender, EventArgs e)
        {
            CurrentDisplayOrientation = (CCDisplayOrientation)XnaWindow.CurrentOrientation;

            foreach(CCDirector director in sceneDirectors) 
            {
                if(director.RunningScene != null) 
                {
                    director.RunningScene.Viewport.DisplayOrientation = CurrentDisplayOrientation;
                }
            }
        }

        #endregion Orientation handling


        #region Window resize handling

        void OnWindowSizeChanged(object sender, EventArgs e)
        {
            CCSize landscapeWindowSize = LandscapeWindowSizeInPixels;

            foreach(CCDirector director in sceneDirectors) 
            {
                if(director.RunningScene != null) 
                {
                    director.RunningScene.Viewport.LandscapeScreenSizeInPixels = landscapeWindowSize;
                }
            }
        }

        #endregion Window resize handling


        #region Drawing and updating

        public virtual void MainLoop(CCGameTime gameTime)
        {
            foreach (CCDirector director in sceneDirectors) 
            {
                if (director.IsPurgeDirectorInNextLoop) 
                {
                    director.PurgeDirector();
                    director.IsPurgeDirectorInNextLoop = false;
                } 
                else
                {
                    Draw(gameTime);
                }
            }
        }

        protected void Draw(CCGameTime gameTime)
        {
            //Stats.UpdateStart();

            DrawManager.PushMatrix();

            foreach (CCDirector director in sceneDirectors) 
            {
                CCScene runningScene = director.RunningScene;

                // draw the scene
                if (runningScene != null) 
                {
                    runningScene.Visit();
                    if (EventDispatcher.IsEventListenersFor(EVENT_AFTER_VISIT))
                        EventDispatcher.DispatchEvent (eventAfterVisit);
                }

                // draw the notifications node
                if (NotificationNode != null) 
                {
                    NotificationNode.Visit();
                }

                if (EventDispatcher.IsEventListenersFor (EVENT_AFTER_DRAW))
                    EventDispatcher.DispatchEvent (eventAfterDraw);
            }

            DrawManager.PopMatrix();

            //Stats.Draw(this);
        }

        internal void Update(float deltaTime)
        {
            //Stats.UpdateStart();

            if(EventDispatcher.IsEventListenersFor(EVENT_AFTER_UPDATE)) 
            {
                EventDispatcher.DispatchEvent(eventAfterUpdate);
            }

            CCDisplayOrientation orientation = CurrentDisplayOrientation;
            CCSize landscapeWindowSize = LandscapeWindowSizeInPixels;

            foreach(CCDirector director in sceneDirectors) 
            {
                if(director.NextScene != null)
                {
                    director.NextScene.Viewport.DisplayOrientation = orientation;
                    director.NextScene.Viewport.LandscapeScreenSizeInPixels = landscapeWindowSize;
                    if (director.NextScene.Window == null)
                        director.NextScene.Window = this;
                    director.SetNextScene();
                }
            }

            //Stats.UpdateEnd(deltaTime);
        }

        #endregion Drawing and updating

        public CCSceneResolutionPolicy DesignResolutionPolicy { get; private set; }


        public CCSize DesignResolutionSize 
        {
            get 
            {
                return designResolutionSize;
            }
            private set 
            {
                designResolutionSize = value;
            }
        }

        public void SetDesignResolutionSize(float width, float height, CCSceneResolutionPolicy resolutionPolicy)
        {
            designResolutionSize = new CCSize(width, height);
            DesignResolutionPolicy = resolutionPolicy;
        }

    }
}