using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace cocos2d
{
    public enum Orientation
    {
        /// Device oriented vertically, home button on the bottom
        kOrientationPortrait = 0,

        /// Device oriented vertically, home button on the top
        kOrientationPortraitUpsideDown = 1,

        /// Device oriented horizontally, home button on the right
        kOrientationLandscapeLeft = 2,

        /// Device oriented horizontally, home button on the left
        kOrientationLandscapeRight = 3,
    };

    public abstract class CCApplication : DrawableGameComponent
    {
        protected static CCApplication s_pSharedApplication;
        private readonly List<CCTouch> endedTouches = new List<CCTouch>();
        private readonly Dictionary<int, LinkedListNode<CCTouch>> m_pTouchMap = new Dictionary<int, LinkedListNode<CCTouch>>();
        private readonly LinkedList<CCTouch> m_pTouches = new LinkedList<CCTouch>();
        private readonly List<CCTouch> movedTouches = new List<CCTouch>();
        private readonly List<CCTouch> newTouches = new List<CCTouch>();
#if WINDOWS
        private int _lastMouseId;
        private MouseState _lastMouseState;
        private MouseState _prevMouseState;
#endif
        protected bool m_bCaptured;
        protected IEGLTouchDelegate m_pDelegate;

        private IGraphicsDeviceService m_graphicsService;

        public GameTime GameTime;

        public CCApplication(Game game, IGraphicsDeviceService service)
            : base(game)
        {
            m_graphicsService = service;
            Content = game.Content;

            if (m_graphicsService.GraphicsDevice != null)
            {
                Game.Services.AddService(typeof(IGraphicsDeviceService), m_graphicsService);
                DrawManager.Init(m_graphicsService.GraphicsDevice);
            }
            else
            {
                service.DeviceCreated += ServiceDeviceCreated;
            }

            if (service is GraphicsDeviceManager)
            {
                ((GraphicsDeviceManager)service).PreparingDeviceSettings += GraphicsPreparingDeviceSettings;
            }

            game.IsFixedTimeStep = true;

            TouchPanel.EnabledGestures = GestureType.Tap;

            game.Activated += new EventHandler<EventArgs>(game_Activated);
            game.Deactivated += new EventHandler<EventArgs>(game_Deactivated);
        }

        private void game_Deactivated(object sender, EventArgs e)
        {
#if ANDROID
            CCTextureCache.PurgeSharedTextureCache();
#endif
            ApplicationDidEnterBackground();
            CocosDenshion.SimpleAudioEngine.SharedEngine.RestoreMediaState();
        }

        private void game_Activated(object sender, EventArgs e)
        {
            CocosDenshion.SimpleAudioEngine.SharedEngine.SaveMediaState();
#if ANDROID
            CCSpriteFontCache.SharedInstance.Clear();
#endif
            ApplicationWillEnterForeground();
        }

        protected virtual void ServiceDeviceCreated(object sender, EventArgs e)
        {
            DrawManager.Init(m_graphicsService.GraphicsDevice);
        }

        protected virtual void GraphicsPreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            e.GraphicsDeviceInformation.PresentationParameters.DepthStencilFormat = DepthFormat.Depth24Stencil8;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferFormat = SurfaceFormat.Color;
        }

        /// <summary>
        /// Callback by CCDirector for limit FPS
        /// </summary>
        /// <param name="interval">The time, which expressed in seconds, between current frame and next. </param>
        public virtual double AnimationInterval
        {
            get { return Game.TargetElapsedTime.Milliseconds / 10000000f; }
            set { Game.TargetElapsedTime = TimeSpan.FromTicks((int) (value * 10000000)); }
        }

        public IEGLTouchDelegate TouchDelegate
        {
            set { m_pDelegate = value; }
        }

        public ContentManager Content { get; private set; }

        public void ClearTouches()
        {
            m_pTouches.Clear();
            m_pTouchMap.Clear();
        }

        /// <summary>
        /// Loads the content for the game and then calls ApplicationDidFinishLaunching.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            
            ApplicationDidFinishLaunching();
#if ANDROID
            CCDirector.SharedDirector.DirtyLabels();
#endif
        }

        public override void Initialize()
        {
            //DebugSystem.Initialize(Game, "fonts/debugfont");
            //DebugSystem.Instance.FpsCounter.Visible = true;
            //DebugSystem.Instance.TimeRuler.Visible = true;
            //DebugSystem.Instance.TimeRuler.ShowLog = true;

            s_pSharedApplication = this;

            InitInstance();

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            GameTime = gameTime;

            // We must call StartFrame at the top of Update to indicate to the TimeRuler
            // that a new frame has started.
            //DebugSystem.Instance.TimeRuler.StartFrame();

            // We can now begin measuring our Update method
            //DebugSystem.Instance.TimeRuler.BeginMark("Update", Color.Blue);

#if !PSM
            if (CCDirector.SharedDirector.Accelerometer != null)
            {
                CCDirector.SharedDirector.Accelerometer.Update();
            }
#endif            
            // Process touch events 
            ProcessTouch();

            if (CCDirector.SharedDirector.GamePadEnabled)
            {
                // Process the game pad
                // This consumes game pad state.
                ProcessGamePad();
            }

            CCDirector.SharedDirector.Update(gameTime);

            base.Update(gameTime);

            // End measuring the Update method
            //DebugSystem.Instance.TimeRuler.EndMark("Update");
        }

        public override void Draw(GameTime gameTime)
        {
            GameTime = gameTime;

            // Begin measuring our Draw method
            //DebugSystem.Instance.TimeRuler.BeginMark("Draw", Color.Red);

            DrawManager.BeginDraw();

            CCDirector.SharedDirector.MainLoop(gameTime);

            base.Draw(gameTime);

            DrawManager.EndDraw();

            // End measuring our Draw method
            //DebugSystem.Instance.TimeRuler.EndMark("Draw");
        }

        protected virtual void HandleGesture(GestureSample gesture)
        {
            //TODO: Create CCGesture and convert the coordinates into the local coordinates.
        }

        private void ProcessGamePad()
        {
            CCGamePadButtonDispatcher dispatcher = CCDirector.SharedDirector.GamePadButtonDispatcher;

            // On Android, the gamepad is always connected.
            GamePadState gps = GamePad.GetState(PlayerIndex.One);
            if (gps.IsConnected)
            {
                if (dispatcher != null)
                {
                    dispatcher.DispatchGamePadState(gps, PlayerIndex.One);
                }
            }
            gps = GamePad.GetState(PlayerIndex.Two);
            if (gps.IsConnected)
            {
                if (dispatcher != null)
                {
                    dispatcher.DispatchGamePadState(gps, PlayerIndex.Two);
                }
            }
            gps = GamePad.GetState(PlayerIndex.Three); 
            if (gps.IsConnected)
            {
                if (dispatcher != null)
                {
                    dispatcher.DispatchGamePadState(gps, PlayerIndex.Three);
                }
            }
            gps = GamePad.GetState(PlayerIndex.Four); 
            if (gps.IsConnected)
            {
                if (dispatcher != null)
                {
                    dispatcher.DispatchGamePadState(gps, PlayerIndex.Four);
                }
            }
        }

        private void ProcessTouch()
        {
            if (m_pDelegate != null)
            {
                newTouches.Clear();
                movedTouches.Clear();
                endedTouches.Clear();

                CCRect viewPort = DrawManager.ViewPortRect;
                CCPoint pos;

                // TODO: allow configuration to treat the game pad as a touch device.

#if WINDOWS
                _prevMouseState = _lastMouseState;
                _lastMouseState = Mouse.GetState();

                if (_prevMouseState.LeftButton == ButtonState.Released && _lastMouseState.LeftButton == ButtonState.Pressed)
                {
                    pos = DrawManager.ScreenToWorld(_lastMouseState.X, _lastMouseState.Y);

                    _lastMouseId++;
                    m_pTouches.AddLast(new CCTouch(_lastMouseId, pos.x, pos.y));
                    m_pTouchMap.Add(_lastMouseId, m_pTouches.Last);
                    newTouches.Add(m_pTouches.Last.Value);

                    m_bCaptured = true;
                }
                else if (_prevMouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (m_pTouchMap.ContainsKey(_lastMouseId))
                    {
                        if (_prevMouseState.X != _lastMouseState.X || _prevMouseState.Y != _lastMouseState.Y)
                        {
                            pos = DrawManager.ScreenToWorld(_lastMouseState.X, _lastMouseState.Y);

                            movedTouches.Add(m_pTouchMap[_lastMouseId].Value);
                            m_pTouchMap[_lastMouseId].Value.SetTouchInfo(_lastMouseId, pos.x, pos.y);
                        }
                    }
                }
                else if (_prevMouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released)
                {
                    if (m_pTouchMap.ContainsKey(_lastMouseId))
                    {
                        endedTouches.Add(m_pTouchMap[_lastMouseId].Value);
                        m_pTouches.Remove(m_pTouchMap[_lastMouseId]);
                        m_pTouchMap.Remove(_lastMouseId);
                    }
                }
#endif

                TouchCollection touchCollection = TouchPanel.GetState();

                /*while (TouchPanel.IsGestureAvailable)
                {
                    HandleGesture(TouchPanel.ReadGesture());
                }*/


                foreach (TouchLocation touch in touchCollection)
                {
                    switch (touch.State)
                    {
                        case TouchLocationState.Pressed:
                            if (m_pTouchMap.ContainsKey(touch.Id))
                            {
                                break;
                            }

                            if (viewPort.containsPoint(touch.Position.X, touch.Position.Y))
                            {
                                pos = DrawManager.ScreenToWorld(touch.Position.X, touch.Position.Y);

                                m_pTouches.AddLast(new CCTouch(touch.Id, pos.x, pos.y));
                                m_pTouchMap.Add(touch.Id, m_pTouches.Last);
                                newTouches.Add(m_pTouches.Last.Value);
                            }
                            break;

                        case TouchLocationState.Moved:
                            LinkedListNode<CCTouch> existingTouch;
                            if (m_pTouchMap.TryGetValue(touch.Id, out existingTouch))
                            {
                                pos = DrawManager.ScreenToWorld(touch.Position.X, touch.Position.Y);
                                var delta = existingTouch.Value.LocationInView - pos;
                                if (delta.LengthSQ > 1.0f)
                                {
                                    movedTouches.Add(existingTouch.Value);
                                    existingTouch.Value.SetTouchInfo(touch.Id, pos.x, pos.y);
                                }
                            }
                            break;

                        case TouchLocationState.Released:
                            if (m_pTouchMap.TryGetValue(touch.Id, out existingTouch))
                            {
                                endedTouches.Add(existingTouch.Value);
                                m_pTouches.Remove(existingTouch);
                                m_pTouchMap.Remove(touch.Id);
                            }
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (newTouches.Count > 0)
                {
                    m_pDelegate.TouchesBegan(newTouches, null);
                }

                if (movedTouches.Count > 0)
                {
                    m_pDelegate.TouchesMoved(movedTouches, null);
                }

                if (endedTouches.Count > 0)
                {
                    m_pDelegate.TouchesEnded(endedTouches, null);
                }
            }
        }

        private CCTouch GetTouchBasedOnId(int nID)
        {
            if (m_pTouchMap.ContainsKey(nID))
            {
                LinkedListNode<CCTouch> curTouch = m_pTouchMap[nID];
                //If ID's match...
                if (curTouch.Value.Id == nID)
                {
                    //return the corresponding touch
                    return curTouch.Value;
                }
            }
            //If we reached here, we found no touches
            //matching the specified id.
            return null;
        }

        // sharedApplication pointer

        /// <summary>
        /// Get current applicaiton instance.
        /// </summary>
        /// <value> Current application instance pointer. </value>
        public static CCApplication SharedApplication
        {
            get { return s_pSharedApplication; }
        }

        /// <summary>
        /// Implement for initialize OpenGL instance, set source path, etc...
        /// </summary>
        public virtual bool InitInstance()
        {
            return true;
        }

        /// <summary>
        /// Implement CCDirector and CCScene init code here.
        /// </summary>
        /// <returns>
        ///     return true    Initialize success, app continue.
        ///     return false   Initialize failed, app terminate.
        /// </returns>
        public virtual bool ApplicationDidFinishLaunching()
        {
            return false;
        }

        /// <summary>
        /// Called when the game enters the background. This happens when the 'windows' button is pressed
        /// on a WP phone. On Android, it happens when the device is ide or the power button is pressed.
        /// </summary>
        public virtual void ApplicationDidEnterBackground()
        {
        }

        /// <summary>
        /// Called when the game returns to the foreground, such as when the game is launched after
        /// being paused.
        /// </summary>
        public virtual void ApplicationWillEnterForeground()
        {
        }
    }
}