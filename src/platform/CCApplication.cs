using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace CocosSharp
{

	[Flags]
	public enum CCDisplayOrientation
	{
		Default = 0,
		LandscapeLeft = 1,
		LandscapeRight = 2,
		Portrait = 4,
		PortraitDown = 8,
		Unknown = 16
	}

	public class CCGameTime
	{
		public TimeSpan TotalGameTime { get; set; }

		public TimeSpan ElapsedGameTime { get; set; }

		public bool IsRunningSlowly { get; set; }

		public CCGameTime()
		{
			TotalGameTime = TimeSpan.Zero;
			ElapsedGameTime = TimeSpan.Zero;
			IsRunningSlowly = false;
		}

		public CCGameTime(TimeSpan totalGameTime, TimeSpan elapsedGameTime)
		{
			TotalGameTime = totalGameTime;
			ElapsedGameTime = elapsedGameTime;
			IsRunningSlowly = false;
		}

		public CCGameTime (TimeSpan totalRealTime, TimeSpan elapsedRealTime, bool isRunningSlowly)
		{
			TotalGameTime = totalRealTime;
			ElapsedGameTime = elapsedRealTime;
			IsRunningSlowly = isRunningSlowly;
		}
	}

    public abstract class CCApplication : DrawableGameComponent
    {
        private readonly List<CCTouch> endedTouches = new List<CCTouch>();
		private readonly Dictionary<int, LinkedListNode<CCTouch>> touchMap = new Dictionary<int, LinkedListNode<CCTouch>>();
		private readonly LinkedList<CCTouch> touches = new LinkedList<CCTouch>();
        private readonly List<CCTouch> movedTouches = new List<CCTouch>();
        private readonly List<CCTouch> newTouches = new List<CCTouch>();
#if WINDOWS || WINDOWSGL || MACOS || WINDOWSGL
        private int _lastMouseId;
        private MouseState lastMouseState;
        private MouseState prevMouseState;
#endif
        protected bool m_bCaptured;

		internal GameTime XNAGameTime;

		public CCGameTime GameTime { get; set; }

        private bool _initialized;
		public CCDisplayOrientation CurrentOrientation { get; private set; }

        public CCApplication(Game game, IGraphicsDeviceService service = null)
            : base(game)
        {

			GameTime = new CCGameTime ();

			SharedApplication = this;
 
            if (Game.Services.GetService(typeof(IGraphicsDeviceService)) == null)
            {
                if (service == null)
                    service = new GraphicsDeviceManager (game);

                // if we still do not have a service after creating the GraphicsDeviceManager
                // we need to stop somewhere and issue a warning.
                if (Game.Services.GetService (typeof(IGraphicsDeviceService)) == null) 
                {
                    Game.Services.AddService(typeof(IGraphicsDeviceService), service);
                }
            }

			CCDrawManager.GraphicsDeviceService = service;

            Content = game.Content;
            HandleMediaStateAutomatically = true;

            game.IsFixedTimeStep = true;

            TouchPanel.EnabledGestures = GestureType.Tap;

            game.Activated += GameActivated;
            game.Deactivated += GameDeactivated;
            game.Exiting += GameExiting;
			game.Window.OrientationChanged += OrientationChanged;

			// We will call this here as the last step
			CCDrawManager.InitializeDisplay (game, (GraphicsDeviceManager)service);
        }

        void OrientationChanged (object sender, EventArgs e)
        {
			CurrentOrientation = (CCDisplayOrientation)Game.Window.CurrentOrientation;
        }

        protected bool HandleMediaStateAutomatically { get; set; }


        private void GameActivated(object sender, EventArgs e)
        {
            // Clear out the prior gamepad state because we don't want it anymore.
            PriorGamePadState.Clear();
#if !IOS
            if (HandleMediaStateAutomatically)
            {
                CocosDenshion.CCSimpleAudioEngine.SharedEngine.SaveMediaState();
            }
#endif
            ApplicationWillEnterForeground();
        }

        private void GameDeactivated(object sender, EventArgs e)
        {
            ApplicationDidEnterBackground();
#if !IOS
            if (HandleMediaStateAutomatically)
            {
                CocosDenshion.CCSimpleAudioEngine.SharedEngine.RestoreMediaState();
            }
#endif
        }

        void GameExiting(object sender, EventArgs e)
        {
            CCDirector.SharedDirector.End();
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

        /// <summary>
        /// This returns the shared CCContentManager.
        /// </summary>
		public ContentManager Content { get { return(CCContentManager.SharedContentManager); } private set { } }

        public void ClearTouches()
        {
            touches.Clear();
            touchMap.Clear();
        }

        /// <summary>
        /// Loads the content for the game and then calls ApplicationDidFinishLaunching.
        /// </summary>
        protected override void LoadContent()
        {
            if (!_initialized)
            {
                CCContentManager.Initialize(Game.Content.ServiceProvider, Game.Content.RootDirectory);

                base.LoadContent();

                ApplicationDidFinishLaunching();

                _initialized = true;
            }
            else
            {
                base.LoadContent();
            }
        }

        public override void Initialize()
        {
            SharedApplication = this;

            InitInstance();

			// Initialize our Director
			// We are moving the initialization from the overriding class to here so the 
			// user does not have to deal with doing this thiemselves and cluttering up their codebase.
			CCDirector.SharedDirector.SetOpenGlView();

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            XNAGameTime = gameTime;

			GameTime.ElapsedGameTime = gameTime.ElapsedGameTime;
			GameTime.IsRunningSlowly = gameTime.IsRunningSlowly;
			GameTime.TotalGameTime = gameTime.TotalGameTime;

#if !NETFX_CORE
			if (CCDirector.SharedDirector.Accelerometer != null 
				&& CCDirector.SharedDirector.Accelerometer.Enabled
				&& CCDirector.SharedDirector.EventDispatcher.IsEventListenersFor(CCEventListenerAccelerometer.LISTENER_ID))
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

            ProcessKeyboard();

			ProcessMouse ();

			CCDirector.SharedDirector.Update(GameTime);

            base.Update(gameTime);
        }

		public override void Draw(GameTime gameTime)
        {
            XNAGameTime = gameTime;

			GameTime.ElapsedGameTime = gameTime.ElapsedGameTime;
			GameTime.IsRunningSlowly = gameTime.IsRunningSlowly;
			GameTime.TotalGameTime = gameTime.TotalGameTime;

            CCDrawManager.BeginDraw();

			CCDirector.SharedDirector.MainLoop(GameTime);

            base.Draw(gameTime);

            CCDrawManager.EndDraw();
        }

		internal virtual void HandleGesture(GestureSample gesture)
        {
            //TODO: Create CCGesture and convert the coordinates into the local coordinates.
        }

		public List<string> ContentSearchPaths
		{
			get
			{
				return CCContentManager.SharedContentManager.SearchPaths;
			}

			set 
			{
				CCContentManager.SharedContentManager.SearchPaths = value;
			}
		}

		public List<string> ContentSearchResolutionOrder
		{
			get
			{
				return CCContentManager.SharedContentManager.SearchResolutionsOrder;
			}

			set 
			{
				CCContentManager.SharedContentManager.SearchResolutionsOrder = value;
			}
		}

		public string ContentRootDirectory
		{
			get { return CCContentManager.SharedContentManager.RootDirectory; }

			set 
			{
				CCContentManager.SharedContentManager.RootDirectory = value;
			}
		}

		public bool AllowUserResizing
		{
			get { return Game.Window.AllowUserResizing; }
			set { Game.Window.AllowUserResizing = value; }
		}

		public CCDisplayOrientation SupportedOrientations
        {
            get { return CCDrawManager.SupportedOrientations; }
            set 
            {
                CCDrawManager.SupportedOrientations = value;
            }
        }

		public int PreferredBackBufferWidth 
		{ 
			get 
			{
                var service = Game.Services.GetService (typeof(IGraphicsDeviceService));
				var manager = service as GraphicsDeviceManager;

				Debug.Assert (manager != null, "CCApplication: GraphicsManager is not setup");
				if (manager != null) 
					return manager.PreferredBackBufferWidth;

				return 0;
			}
			set
			{
                var service = Game.Services.GetService (typeof(IGraphicsDeviceService));
				var manager = service as GraphicsDeviceManager;

				Debug.Assert (manager != null, "CCApplication: GraphicsManager is not setup");
                if (manager != null)
                {
                    manager.PreferredBackBufferWidth = value;
                    CCDrawManager.UpdatePresentationParameters();
                }

			}
		}

		public int PreferredBackBufferHeight 
		{ 
			get 
			{
                var service = Game.Services.GetService (typeof(IGraphicsDeviceService));
				var manager = service as GraphicsDeviceManager;

				Debug.Assert (manager != null, "CCApplication: GraphicsManager is not setup");
				if (manager != null) 
					return manager.PreferredBackBufferHeight;
                    
				return 0;
			}
			set
			{
                var service = Game.Services.GetService (typeof(IGraphicsDeviceService));
				var manager = service as GraphicsDeviceManager;

				Debug.Assert (manager != null, "CCApplication: GraphicsManager is not setup");
                if (manager != null)
                {
                    manager.PreferredBackBufferHeight = value;
                    CCDrawManager.UpdatePresentationParameters();
                }

			}
		}

		public bool PreferMultiSampling 
		{ 
			get 
			{
				var service = Game.Services.GetService (typeof(IGraphicsDeviceService));
				var manager = service as GraphicsDeviceManager;

				Debug.Assert (manager != null, "CCApplication: GraphicsManager is not setup");
				if (manager != null) 
					return manager.PreferMultiSampling;

				return false;
			}
			set
			{
				var service = Game.Services.GetService (typeof(IGraphicsDeviceService));
				var manager = service as GraphicsDeviceManager;

				Debug.Assert (manager != null, "CCApplication: GraphicsManager is not setup");
				if (manager != null)
				{
					manager.PreferMultiSampling = value;
				}

			}
		}


		public bool IsFullScreen 
		{ 
			get 
			{
				var service = Game.Services.GetService (typeof(IGraphicsDeviceService));
				var manager = service as GraphicsDeviceManager;

				Debug.Assert (manager != null, "CCApplication: GraphicsManager is not setup");
				if (manager != null)
					return manager.IsFullScreen;

				return false;
			}
			set
			{
				var service = Game.Services.GetService (typeof(IGraphicsDeviceService));
				var manager = service as GraphicsDeviceManager;

				Debug.Assert (manager != null, "CCApplication: GraphicsManager is not setup");
				if (manager != null)
				{
					manager.IsFullScreen = value;
				}

			}
		}

		public void ToggleFullScreen()
		{
			var service = Game.Services.GetService (typeof(IGraphicsDeviceService));
			var manager = service as GraphicsDeviceManager;

			Debug.Assert (manager != null, "CCApplication: GraphicsManager is not setup");
			if (manager != null)
			{
				manager.ToggleFullScreen ();
			}
		}


		internal GraphicsDeviceManager GraphicsDeviceManager
        {
            get 
            {
                return Game.Services.GetService (typeof(IGraphicsDeviceService)) as GraphicsDeviceManager;
            }
        }

        #region GamePad Support

		private Dictionary<PlayerIndex, GamePadState> PriorGamePadState = new Dictionary<PlayerIndex, GamePadState>();

		private CCEventGamePadConnection gamePadConnection = new CCEventGamePadConnection ();
		private CCEventGamePadButton gamePadButton = new CCEventGamePadButton ();
		private CCEventGamePadDPad gamePadDPad = new CCEventGamePadDPad ();
		private CCEventGamePadStick gamePadStick = new CCEventGamePadStick();
		private CCEventGamePadTrigger gamePadTrigger = new CCEventGamePadTrigger();

		private void ProcessGamePad (GamePadState gps, PlayerIndex player)
		{

			var dispatcher = CCDirector.SharedDirector.EventDispatcher;

			var lastState = new GamePadState ();

			if (!PriorGamePadState.ContainsKey (player) && gps.IsConnected) 
			{
				gamePadConnection.IsConnected = true;
				gamePadConnection.Player = (CCPlayerIndex)player;
				dispatcher.DispatchEvent (gamePadConnection);

			}

			if (PriorGamePadState.ContainsKey (player)) 
			{
				lastState = PriorGamePadState [player];
				// Notify listeners when the gamepad is connected/disconnected.
				if ((lastState.IsConnected != gps.IsConnected)) 
				{
					gamePadConnection.IsConnected = false;
					gamePadConnection.Player = (CCPlayerIndex)player;
					dispatcher.DispatchEvent (gamePadConnection);

				}
				// TODO: Check button pressed/released status for button tap events.
			}

			if (gps.IsConnected) 
			{
				var caps = GamePad.GetCapabilities (player);

				if (caps.HasBackButton || 
					caps.HasStartButton ||
					caps.HasBigButton ||
					caps.HasAButton ||
					caps.HasBButton ||
					caps.HasXButton ||
					caps.HasYButton ||
					caps.HasLeftShoulderButton ||
					caps.HasRightShoulderButton) 
				{
					var back = CCGamePadButtonStatus.NotApplicable;
					var start = CCGamePadButtonStatus.NotApplicable;
					var system = CCGamePadButtonStatus.NotApplicable;
					var a = CCGamePadButtonStatus.NotApplicable;
					var b = CCGamePadButtonStatus.NotApplicable;
					var x = CCGamePadButtonStatus.NotApplicable;
					var y = CCGamePadButtonStatus.NotApplicable;
					var leftShoulder = CCGamePadButtonStatus.NotApplicable;
					var rightShoulder = CCGamePadButtonStatus.NotApplicable;

					if (caps.HasBackButton) {
						back = (gps.Buttons.Back == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasStartButton) {
						start = (gps.Buttons.Start == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasBigButton) {
						system = (gps.Buttons.BigButton == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasAButton) {
						a = (gps.Buttons.A == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasBButton) {
						b = (gps.Buttons.B == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasXButton) {
						x = (gps.Buttons.X == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasYButton) {
						y = (gps.Buttons.Y == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasLeftShoulderButton) {
						leftShoulder = (gps.Buttons.LeftShoulder == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasRightShoulderButton) {
						rightShoulder = (gps.Buttons.RightShoulder == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}

					gamePadButton.Back = back;
					gamePadButton.Start = start;
					gamePadButton.System = system;
					gamePadButton.A = a;
					gamePadButton.B = b;
					gamePadButton.X = x;
					gamePadButton.Y = y;
					gamePadButton.LeftShoulder = leftShoulder;
					gamePadButton.RightShoulder = rightShoulder;
					gamePadButton.Player = (CCPlayerIndex)player;

					dispatcher.DispatchEvent (gamePadButton);
				}


				// Process the game sticks
				if ((caps.HasLeftXThumbStick || 
						caps.HasLeftYThumbStick || 
						caps.HasRightXThumbStick || 
						caps.HasRightYThumbStick || 
						caps.HasLeftStickButton || 
						caps.HasRightStickButton)) 
				{
					CCPoint vecLeft;
					if (caps.HasLeftXThumbStick || caps.HasLeftYThumbStick) {
						vecLeft = new CCPoint (gps.ThumbSticks.Left);
						vecLeft.Normalize ();
					} else {
						vecLeft = CCPoint.Zero;
					}
					CCPoint vecRight;
					if (caps.HasRightXThumbStick || caps.HasRightYThumbStick) {
						vecRight = new CCPoint (gps.ThumbSticks.Right);
						vecRight.Normalize ();
					} else {
						vecRight = CCPoint.Zero;
					}
					var left = new CCGameStickStatus ();
					left.Direction = vecLeft;
					left.Magnitude = ((caps.HasLeftXThumbStick || caps.HasLeftYThumbStick) ? gps.ThumbSticks.Left.Length () : 0f);
					left.IsDown = ((caps.HasLeftStickButton) ? gps.IsButtonDown (Buttons.LeftStick) : false);
					var right = new CCGameStickStatus ();
					right.Direction = vecRight;
					right.Magnitude = ((caps.HasRightXThumbStick || caps.HasRightYThumbStick) ? gps.ThumbSticks.Right.Length () : 0f);
					right.IsDown = ((caps.HasLeftStickButton) ? gps.IsButtonDown (Buttons.RightStick) : false);

					gamePadStick.Left = left;
					gamePadStick.Right = right;
					gamePadStick.Player = (CCPlayerIndex)player;

					dispatcher.DispatchEvent (gamePadStick);

				}
				// Process the game triggers
				if (caps.HasLeftTrigger || caps.HasRightTrigger) 
				{
					//GamePadTriggerUpdate (caps.HasLeftTrigger ? gps.Triggers.Left : 0f, caps.HasRightTrigger ? gps.Triggers.Right : 0f, player);
					gamePadTrigger.Left = caps.HasLeftTrigger ? gps.Triggers.Left : 0f;
					gamePadTrigger.Right = caps.HasRightTrigger ? gps.Triggers.Right : 0f;
					gamePadTrigger.Player = (CCPlayerIndex)player;

					dispatcher.DispatchEvent (gamePadTrigger);
				}

				// Process the D-Pad
				if (caps.HasDPadDownButton ||
					caps.HasDPadUpButton ||
					caps.HasDPadLeftButton ||
					caps.HasDPadRightButton) 
				{

					var leftButton = CCGamePadButtonStatus.NotApplicable;
					var rightButton = CCGamePadButtonStatus.NotApplicable;
					var upButton = CCGamePadButtonStatus.NotApplicable;
					var downButton = CCGamePadButtonStatus.NotApplicable;

					if (caps.HasDPadDownButton) {
						downButton = (gps.DPad.Down == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasDPadUpButton) {
						upButton = (gps.DPad.Up == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasDPadLeftButton) {
						leftButton = (gps.DPad.Left == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasDPadRightButton) {
						rightButton = (gps.DPad.Right == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}

					gamePadDPad.Down = downButton;
					gamePadDPad.Up = upButton;
					gamePadDPad.Left = leftButton;
					gamePadDPad.Right = rightButton;

					gamePadDPad.Player = (CCPlayerIndex)player;

					dispatcher.DispatchEvent (gamePadDPad);
				}
			}
			PriorGamePadState [player] = gps;
		}

        private void ProcessGamePad()
        {

			if (CCDirector.SharedDirector.GamePadEnabled &&
				CCDirector.SharedDirector.EventDispatcher.IsEventListenersFor (CCEventListenerGamePad.LISTENER_ID)) 
			{

				// On Android, the gamepad is always connected.
				GamePadState gps1 = GamePad.GetState (PlayerIndex.One);
				GamePadState gps2 = GamePad.GetState (PlayerIndex.Two);
				GamePadState gps3 = GamePad.GetState (PlayerIndex.Three);
				GamePadState gps4 = GamePad.GetState (PlayerIndex.Four);
				ProcessGamePad (gps1, PlayerIndex.One);
				ProcessGamePad (gps2, PlayerIndex.Two);
				ProcessGamePad (gps3, PlayerIndex.Three);
				ProcessGamePad (gps4, PlayerIndex.Four);
			}
        }
        #endregion

        #region Keyboard support
		private KeyboardState priorKeyboardState;

        private void ProcessKeyboard()
        {
			// Read the current keyboard state
			KeyboardState currentKeyboardState = Keyboard.GetState();

			var dispatcher = CCDirector.SharedDirector.EventDispatcher;

			if (currentKeyboardState == priorKeyboardState || !dispatcher.IsEventListenersFor(CCEventListenerKeyboard.LISTENER_ID) )
			{
				priorKeyboardState = currentKeyboardState;
				return;
			}


			// Check for Keypad interaction
			if(currentKeyboardState.IsKeyUp(Keys.Back) && priorKeyboardState.IsKeyDown(Keys.Back)) 
            {
                CCDirector.SharedDirector.KeypadDispatcher.DispatchKeypadMsg(CCKeypadMSGType.BackClicked);
            }
			else if(currentKeyboardState.IsKeyUp(Keys.Home) && priorKeyboardState.IsKeyDown(Keys.Home)) 
            {
                CCDirector.SharedDirector.KeypadDispatcher.DispatchKeypadMsg(CCKeypadMSGType.MenuClicked);
            }

			var keyboardEvent = new CCEventKeyboard (CCKeyboardEventType.KEYBOARD_PRESS);
			var keyboardState = new CCKeyboardState () { KeyboardState = currentKeyboardState };

			keyboardEvent.KeyboardState = keyboardState;

			// Check for pressed/released keys.
			// Loop for each possible pressed key (those that are pressed this update)
			Keys[] keys = currentKeyboardState.GetPressedKeys();

			for (int k = 0; k < keys.Length; k++) {
				// Was this key up during the last update?
				if (priorKeyboardState.IsKeyUp (keys [k])) {

					// Yes, so this key has been pressed
					//CCLog.Log("Pressed: " + keys[i].ToString());
					keyboardEvent.Keys = (CCKeys)keys [k];
					dispatcher.DispatchEvent (keyboardEvent);
				}
			}

			// Loop for each possible released key (those that were pressed last update)
			keys = priorKeyboardState.GetPressedKeys ();
			keyboardEvent.KeyboardEventType = CCKeyboardEventType.KEYBOARD_RELEASE;
			for (int k = 0; k < keys.Length; k++) {
				// Is this key now up?
				if (currentKeyboardState.IsKeyUp (keys [k])) {
					// Yes, so this key has been released
					//CCLog.Log("Released: " + keys[i].ToString());
					keyboardEvent.Keys = (CCKeys)keys [k];
					dispatcher.DispatchEvent (keyboardEvent);

				}
			}

			// Store the state for the next loop
			priorKeyboardState = currentKeyboardState;

        }
        #endregion

		#region Mouse support
		private MouseState priorMouseState;

		private void ProcessMouse()
		{
			// Read the current Mouse state
			MouseState currentMouseState = Mouse.GetState();

			var dispatcher = CCDirector.SharedDirector.EventDispatcher;

			if (currentMouseState == priorMouseState || !dispatcher.IsEventListenersFor(CCEventListenerMouse.LISTENER_ID) )
			{
				priorMouseState = currentMouseState;
				return;
			}


			CCPoint pos;
			int posX = 0;
			int posY = 0;

#if NETFX_CORE
			pos = TransformPoint(priorMouseState.X, priorMouseState.Y);
			pos = CCDrawManager.ScreenToWorld(pos.X, pos.Y);
#else
			pos = CCDrawManager.ScreenToWorld(priorMouseState.X, priorMouseState.Y);
#endif

			// We will only do the cast once.
			posX = (int)pos.X;
			posY = (int)pos.Y;

			var mouseEvent = new CCEventMouse (CCMouseEventType.MOUSE_MOVE);
			mouseEvent.CursorX = posX;
			mouseEvent.CursorY = posY;

			dispatcher.DispatchEvent (mouseEvent);

			CCMouseButton mouseButton = CCMouseButton.None;
			if (priorMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed) 
			{
				mouseButton |= CCMouseButton.LeftButton;
			}
			if (priorMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed) 
			{
				mouseButton |= CCMouseButton.RightButton;
			}
			if (priorMouseState.MiddleButton == ButtonState.Released && currentMouseState.MiddleButton == ButtonState.Pressed) 
			{
				mouseButton |= CCMouseButton.MiddleButton;
			}
			if (priorMouseState.XButton1 == ButtonState.Released && currentMouseState.XButton1 == ButtonState.Pressed) 
			{
				mouseButton |= CCMouseButton.ExtraButton1;
			}
			if (priorMouseState.XButton2 == ButtonState.Released && currentMouseState.XButton2 == ButtonState.Pressed) 
			{
				mouseButton |= CCMouseButton.ExtraButton1;
			}

			if (mouseButton > 0) 
			{
				mouseEvent.MouseEventType = CCMouseEventType.MOUSE_DOWN;
				mouseEvent.MouseButton = mouseButton;
				dispatcher.DispatchEvent (mouseEvent);
			}

			mouseButton = CCMouseButton.None;
			if (priorMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released) 
			{
				mouseButton |= CCMouseButton.LeftButton;
			}
			if (priorMouseState.RightButton == ButtonState.Pressed && currentMouseState.RightButton == ButtonState.Released) 
			{
				mouseButton |= CCMouseButton.RightButton;
			}
			if (priorMouseState.MiddleButton == ButtonState.Pressed && currentMouseState.MiddleButton == ButtonState.Released) 
			{
				mouseButton |= CCMouseButton.MiddleButton;
			}
			if (priorMouseState.XButton1 == ButtonState.Pressed && currentMouseState.XButton1 == ButtonState.Released) 
			{
				mouseButton |= CCMouseButton.ExtraButton1;
			}
			if (priorMouseState.XButton2 == ButtonState.Pressed && currentMouseState.XButton2 == ButtonState.Released) 
			{
				mouseButton |= CCMouseButton.ExtraButton1;
			}
			if (mouseButton > 0) 
			{
				mouseEvent.MouseEventType = CCMouseEventType.MOUSE_UP;
				mouseEvent.MouseButton = mouseButton;
				dispatcher.DispatchEvent (mouseEvent);
			}

			if (priorMouseState.ScrollWheelValue != currentMouseState.ScrollWheelValue) {
				var delta = priorMouseState.ScrollWheelValue - currentMouseState.ScrollWheelValue;
				if (delta != 0) {
					mouseEvent.MouseEventType = CCMouseEventType.MOUSE_SCROLL;
					mouseEvent.ScrollX = 0;
					mouseEvent.ScrollY = delta;
					dispatcher.DispatchEvent (mouseEvent);
					//Console.WriteLine ("mouse scroll: " + mouseEvent.ScrollY);
				}
			}
			// Store the state for the next loop
			priorMouseState = currentMouseState;

		}
		#endregion

        private CCPoint TransformPoint(float x, float y) {
            CCPoint newPoint;
            newPoint.X = x * TouchPanel.DisplayWidth / Game.Window.ClientBounds.Width;
            newPoint.Y = y * TouchPanel.DisplayHeight / Game.Window.ClientBounds.Height;
            return newPoint;
        }

        private void ProcessTouch()
        {
			//if (m_pDelegate != null)
			if (CCDirector.SharedDirector.EventDispatcher.IsEventListenersFor(CCEventListenerTouchOneByOne.LISTENER_ID)
                || CCDirector.SharedDirector.EventDispatcher.IsEventListenersFor(CCEventListenerTouchAllAtOnce.LISTENER_ID))
			{
                newTouches.Clear();
                movedTouches.Clear();
                endedTouches.Clear();

                CCRect viewPort = CCDrawManager.ViewPortRect;
                CCPoint pos;

                // TODO: allow configuration to treat the game pad as a touch device.

#if WINDOWS || WINDOWSGL || MACOS
				prevMouseState = lastMouseState;
                lastMouseState = Mouse.GetState();

                if (prevMouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
                {
#if NETFX_CORE
                    pos = TransformPoint(lastMouseState.X, lastMouseState.Y);
                    pos = CCDrawManager.ScreenToWorld(pos.X, pos.Y);
#else
                    pos = CCDrawManager.ScreenToWorld(lastMouseState.X, lastMouseState.Y);
#endif
                    _lastMouseId++;
                    touches.AddLast(new CCTouch(_lastMouseId, pos.X, pos.Y));
                    touchMap.Add(_lastMouseId, touches.Last);
                    newTouches.Add(touches.Last.Value);

                    m_bCaptured = true;
                }
                else if (prevMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (touchMap.ContainsKey(_lastMouseId))
                    {
                        if (prevMouseState.X != lastMouseState.X || prevMouseState.Y != lastMouseState.Y)
                        {
#if NETFX_CORE
                            pos = TransformPoint(lastMouseState.X, lastMouseState.Y);
                            pos = CCDrawManager.ScreenToWorld(pos.X, pos.Y);
#else
                            pos = CCDrawManager.ScreenToWorld(lastMouseState.X, lastMouseState.Y);
#endif
                            movedTouches.Add(touchMap[_lastMouseId].Value);
                            touchMap[_lastMouseId].Value.SetTouchInfo(_lastMouseId, pos.X, pos.Y);
                        }
                    }
                }
                else if (prevMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    if (touchMap.ContainsKey(_lastMouseId))
                    {
                        endedTouches.Add(touchMap[_lastMouseId].Value);
                        touches.Remove(touchMap[_lastMouseId]);
                        touchMap.Remove(_lastMouseId);
                    }
                }
#endif

                TouchCollection touchCollection = TouchPanel.GetState();

                foreach (TouchLocation touch in touchCollection)
                {
                    switch (touch.State)
                    {
                        case TouchLocationState.Pressed:
                            if (touchMap.ContainsKey(touch.Id))
                            {
                                break;
                            }

                            if (viewPort.ContainsPoint(touch.Position.X, touch.Position.Y))
                            {
                                pos = CCDrawManager.ScreenToWorld(touch.Position.X, touch.Position.Y);

                                touches.AddLast(new CCTouch(touch.Id, pos.X, pos.Y));
                                touchMap.Add(touch.Id, touches.Last);
                                newTouches.Add(touches.Last.Value);
                            }
                            break;

                        case TouchLocationState.Moved:
                            LinkedListNode<CCTouch> existingTouch;
                            if (touchMap.TryGetValue(touch.Id, out existingTouch))
                            {
                                pos = CCDrawManager.ScreenToWorld(touch.Position.X, touch.Position.Y);
                                var delta = existingTouch.Value.LocationInView - pos;
                                if (delta.LengthSQ > 1.0f)
                                {
                                    movedTouches.Add(existingTouch.Value);
                                    existingTouch.Value.SetTouchInfo(touch.Id, pos.X, pos.Y);
                                }
                            }
                            break;

                        case TouchLocationState.Released:
                            if (touchMap.TryGetValue(touch.Id, out existingTouch))
                            {
                                endedTouches.Add(existingTouch.Value);
                                touches.Remove(existingTouch);
                                touchMap.Remove(touch.Id);
                            }
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
					var touchEvent = new CCEventTouch(CCEventCode.BEGAN);

                if (newTouches.Count > 0)
                {
						touchEvent.Touches = newTouches;
						//m_pDelegate.TouchesBegan(newTouches);
						CCDirector.SharedDirector.EventDispatcher.DispatchEvent(touchEvent);
                }

                if (movedTouches.Count > 0)
                {
						touchEvent.EventCode = CCEventCode.MOVED;
						touchEvent.Touches = movedTouches;
						CCDirector.SharedDirector.EventDispatcher.DispatchEvent(touchEvent);

						//m_pDelegate.TouchesMoved(movedTouches);
                }

                if (endedTouches.Count > 0)
                {
						//m_pDelegate.TouchesEnded(endedTouches);
						touchEvent.EventCode = CCEventCode.ENDED;
						touchEvent.Touches = endedTouches;
						CCDirector.SharedDirector.EventDispatcher.DispatchEvent(touchEvent);
                }
            }
        }

        private CCTouch GetTouchBasedOnId(int nID)
        {
            if (touchMap.ContainsKey(nID))
            {
                LinkedListNode<CCTouch> curTouch = touchMap[nID];
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
		public static CCApplication SharedApplication { get ; protected set; }

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