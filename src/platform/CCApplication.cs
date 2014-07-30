using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

#if ANDROID
using Android.Views;
#endif

#if WINDOWS_PHONE
using MonoGame.Framework.WindowsPhone;
using Microsoft.Phone.Controls;
#endif

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

	public static class OrientationExtension
	{
		public static bool IsPortrait(this CCDisplayOrientation orientation)
		{
			orientation = orientation & CCDisplayOrientation.Portrait;
			orientation = orientation & CCDisplayOrientation.PortraitDown;

			return orientation != CCDisplayOrientation.Default;
		}
	}

	public class CCGameTime
	{
		#region Properties

		public bool IsRunningSlowly { get; set; }
		public TimeSpan TotalGameTime { get; set; }
		public TimeSpan ElapsedGameTime { get; set; }

		#endregion Properties


		#region Constructors

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

		public CCGameTime(TimeSpan totalRealTime, TimeSpan elapsedRealTime, bool isRunningSlowly)
		{
			TotalGameTime = totalRealTime;
			ElapsedGameTime = elapsedRealTime;
			IsRunningSlowly = isRunningSlowly;
		}

		#endregion Constructors
	}

	public class CCGame : Game
	{
		public CCGame()
		{
#if WINDOWS || WINDOWSGL || MACOS
            this.IsMouseVisible = true;
#endif
		}

#if OUYA
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

			// We will comment this out until Director and DrawManager are sorted out.
//            Director.DrawManager.SpriteBatch.Begin();
//            float y = 15;
//            for (int i = 0; i < 4; ++i)
//            {
//                GamePadState gs = GamePad.GetState((PlayerIndex)i, GamePadDeadZone.Circular);
//                string textToDraw = string.Format(
//                "Pad: {0} Connected: {1} LS: ({2:F2}, {3:F2}) RS: ({4:F2}, {5:F2}) LT: {6:F2} RT: {7:F2}",
//                i, gs.IsConnected,
//                gs.ThumbSticks.Left.X, gs.ThumbSticks.Left.Y,
//                gs.ThumbSticks.Right.X, gs.ThumbSticks.Right.Y,
//                gs.Triggers.Left, gs.Triggers.Right);
//
//                Director.DrawManager.SpriteBatch.DrawString(CCSpriteFontCache.SharedInstance["arial-20"], textToDraw, new Vector2(16, y), Color.White);
//                y += 25;
//            }
//            Director.DrawManager.SpriteBatch.End();
        }
#endif

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// Allows the game to exit
#if (WINDOWS && !WINRT) || WINDOWSGL || WINDOWSDX || MACOS
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();
#endif
		}
	}

	public class CCApplicationDelegate
	{
		public virtual void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow) { }

		// Called when the game enters the background. This happens when the 'windows' button is pressed
		// on a WP phone. On Android, it happens when the device is ide or the power button is pressed.
		public virtual void ApplicationDidEnterBackground(CCApplication application) { }

		// Called when the game returns to the foreground, such as when the game is launched after
		// being paused.
		public virtual void ApplicationWillEnterForeground(CCApplication application) { }

	}

	public class CCApplication : DrawableGameComponent
	{
		readonly List<CCTouch> endedTouches = new List<CCTouch>();
		readonly Dictionary<int, LinkedListNode<CCTouch>> touchMap = new Dictionary<int, LinkedListNode<CCTouch>>();
		readonly LinkedList<CCTouch> touches = new LinkedList<CCTouch>();
		readonly List<CCTouch> movedTouches = new List<CCTouch>();
		readonly List<CCTouch> newTouches = new List<CCTouch>();

		internal GameTime XnaGameTime;

		bool paused;
		bool initialized;
		bool isNextDeltaTimeZero;
		float deltaTime;

#if WINDOWS || WINDOWSGL || MACOS || WINDOWSGL
        int lastMouseId;
        MouseState lastMouseState;
        MouseState prevMouseState;
#endif

		MouseState priorMouseState;
		KeyboardState priorKeyboardState;

		Dictionary<PlayerIndex, GamePadState> priorGamePadState;
		CCEventGamePadConnection gamePadConnection;
		CCEventGamePadButton gamePadButton;
		CCEventGamePadDPad gamePadDPad;
		CCEventGamePadStick gamePadStick;
		CCEventGamePadTrigger gamePadTrigger;

		GraphicsDeviceManager xnaDeviceManager;
		CCGame xnaGame;
		CCGameTime GameTime;

		CCWindow mainWindow;
		List<CCWindow> gameWindows;


		#region Properties

#if NETFX_CORE

		public static void Create(CCApplicationDelegate appDelegate)
		{
			Action<CCGame, Windows.ApplicationModel.Activation.IActivatedEventArgs> initAction =
			   delegate(CCGame game, Windows.ApplicationModel.Activation.IActivatedEventArgs args)
			   {

				   var instance = new CCApplication(game);
				   instance.ApplicationDelegate = appDelegate;
			   };
			var factory = new MonoGame.Framework.GameFrameworkViewSource<CCGame>(initAction);
			Windows.ApplicationModel.Core.CoreApplication.Run(factory);

		}

#endif

#if WINDOWS_PHONE

        public static CCApplication Create(string launchParameters, PhoneApplicationPage page)
        {
            //if (instance == null)
            //{
                var game = XamlGame<CCGame>.Create(launchParameters, page);
            //    instance = new CCApplication(game);
            //}

            //return instance;
                return null;
        }

#endif
		// Instance properties
		public bool HandleMediaStateAutomatically { get; set; }
		public CCDisplayOrientation CurrentOrientation { get; private set; }
		public CCApplicationDelegate ApplicationDelegate { get; set; }
		public CCActionManager ActionManager { get; private set; }
		public CCScheduler Scheduler { get; private set; }

		public bool Paused
		{
			get { return paused; }
		}

		public bool PreferMultiSampling
		{
			get
			{
				var service = Game.Services.GetService(typeof(IGraphicsDeviceService));
				var manager = service as GraphicsDeviceManager;

				Debug.Assert(manager != null, "CCApplication: GraphicsManager is not setup");
				if (manager != null)
					return manager.PreferMultiSampling;

				return false;
			}
			set
			{
				var service = Game.Services.GetService(typeof(IGraphicsDeviceService));
				var manager = service as GraphicsDeviceManager;

				Debug.Assert(manager != null, "CCApplication: GraphicsManager is not setup");
				if (manager != null)
				{
					manager.PreferMultiSampling = value;
				}
			}
		}

		// The time, which expressed in seconds, between current frame and next
		public virtual double AnimationInterval
		{
			get { return Game.TargetElapsedTime.Milliseconds / 10000000f; }
			set { Game.TargetElapsedTime = TimeSpan.FromTicks((int)(value * 10000000)); }
		}

		public string ContentRootDirectory
		{
			get { return CCContentManager.SharedContentManager.RootDirectory; }
			set { CCContentManager.SharedContentManager.RootDirectory = value; }
		}

		public List<string> ContentSearchPaths
		{
			get { return CCContentManager.SharedContentManager.SearchPaths; }
			set { CCContentManager.SharedContentManager.SearchPaths = value; }
		}

		public List<string> ContentSearchResolutionOrder
		{
			get { return CCContentManager.SharedContentManager.SearchResolutionsOrder; }
			set { CCContentManager.SharedContentManager.SearchResolutionsOrder = value; }
		}

		// Remove once multiple window support added
		public CCWindow MainWindow
		{
			get
			{
				if (mainWindow == null)
				{
					CCSize windowSize
						= new CCSize(xnaDeviceManager.PreferredBackBufferWidth, xnaDeviceManager.PreferredBackBufferHeight);
					mainWindow = AddWindow(windowSize);
				}

				return mainWindow;
			}
		}

		ContentManager Content
		{
			get { return (CCContentManager.SharedContentManager); }
			set { }
		}

#if ANDROID
		public View AndroidContentView
		{
			get 
			{ 
				View androidView = null;

				if (xnaGame != null)
					androidView = (View)xnaGame.Services.GetService(typeof(View));
				return androidView; 
			}
		}
#endif

		internal GraphicsDeviceManager GraphicsDeviceManager
		{
			get { return Game.Services.GetService(typeof(IGraphicsDeviceService)) as GraphicsDeviceManager; }
		}

		#endregion Properties


		#region Constructors

		public CCApplication(bool isFullScreen = true, CCSize? mainWindowSizeInPixels = null)
			: this(new CCGame(), isFullScreen, mainWindowSizeInPixels)
		{
		}

		CCApplication(CCGame game, bool isFullScreen = true, CCSize? mainWindowSizeInPixels = null,
			CCDisplayOrientation supportedOrientations = CCDisplayOrientation.Default)
			: base(game)
		{
			GameTime = new CCGameTime();
			xnaGame = game;

			Scheduler = new CCScheduler();
			ActionManager = new CCActionManager();
			Scheduler.Schedule(ActionManager, CCSchedulePriority.System, false);

			priorGamePadState = new Dictionary<PlayerIndex, GamePadState>();
			gamePadConnection = new CCEventGamePadConnection();
			gamePadButton = new CCEventGamePadButton();
			gamePadDPad = new CCEventGamePadDPad();
			gamePadStick = new CCEventGamePadStick();
			gamePadTrigger = new CCEventGamePadTrigger();

			IGraphicsDeviceService service = (IGraphicsDeviceService)Game.Services.GetService(typeof(IGraphicsDeviceService));

			if (service == null)
			{
				service = new GraphicsDeviceManager(game);

				// if we still do not have a service after creating the GraphicsDeviceManager
				// we need to stop somewhere and issue a warning.
				if (Game.Services.GetService(typeof(IGraphicsDeviceService)) == null)
				{
					Game.Services.AddService(typeof(IGraphicsDeviceService), service);
				}
			}

			xnaDeviceManager = (GraphicsDeviceManager)service;

			Content = game.Content;
			HandleMediaStateAutomatically = true;

			game.IsFixedTimeStep = true;

			TouchPanel.EnabledGestures = GestureType.Tap;

			game.Activated += GameActivated;
			game.Deactivated += GameDeactivated;
			game.Exiting += GameExiting;
			game.Window.OrientationChanged += OrientationChanged;

			game.Components.Add(this);

			gameWindows = new List<CCWindow>();

			InitializeMainWindow((mainWindowSizeInPixels.HasValue) ? mainWindowSizeInPixels.Value : CCSize.Zero, 
				isFullScreen, supportedOrientations);
		}

		#endregion Constructors


		#region Game window management

		void InitializeMainWindow(CCSize windowSizeInPixels, bool isFullscreen = false,
			CCDisplayOrientation supportedOrientations = CCDisplayOrientation.Default)
		{
			if (isFullscreen)
			{
				windowSizeInPixels.Width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
				windowSizeInPixels.Height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			}

			xnaDeviceManager.IsFullScreen = isFullscreen;

			if (CCDrawManager.SharedDrawManager == null)
			{
				CCDrawManager.SharedDrawManager = new CCDrawManager(xnaDeviceManager, windowSizeInPixels, supportedOrientations);
			}

		}

#if !(IOS || ANDROID || WINDOWS_PHONE)
#endif

		// Make public once multiple window support added
		CCWindow AddWindow(CCSize screenSizeInPixels)
		{
			CCWindow window = new CCWindow(this, screenSizeInPixels, xnaGame.Window, xnaDeviceManager);

			gameWindows.Add(window);

			return window;
		}

		void RemoveWindow(CCWindow window)
		{
			// TBA once multiple window support added
		}

		#endregion Game window management


		#region Cleaning up

		public void PurgeAllCachedData()
		{
			CCLabelBMFont.PurgeCachedData();
		}

		#endregion Cleaning up


		#region Game state

		public void StartGame()
		{
			if (xnaGame != null)
			{
#if !NETFX_CORE
                xnaGame.Run();
#endif
			}

		}

		public void ExitGame()
		{
			foreach (CCWindow window in gameWindows)
			{
				window.EndAllSceneDirectors();
			}
#if (WINDOWS && !WINRT) || WINDOWSGL || WINDOWSDX || MACOS
            xnaGame.Exit();
#endif
		}

		public void PauseGame()
		{
			paused = true;
		}

		public void ResumeGame()
		{
			if (!paused)
			{
				return;
			}

			paused = false;
			deltaTime = 0;
		}

		#endregion Game state



		// Implement for initialize OpenGL instance, set source path, etc...
		public virtual bool InitInstance()
		{
			return true;
		}

		void OrientationChanged(object sender, EventArgs e)
		{
			CurrentOrientation = (CCDisplayOrientation)Game.Window.CurrentOrientation;
		}

		void GameActivated(object sender, EventArgs e)
		{
			// Clear out the prior gamepad state because we don't want it anymore.
			priorGamePadState.Clear();
#if !IOS
			if (HandleMediaStateAutomatically)
			{
				CocosDenshion.CCSimpleAudioEngine.SharedEngine.SaveMediaState();
			}
#endif

			if (ApplicationDelegate != null)
				ApplicationDelegate.ApplicationWillEnterForeground(this);
		}

		void GameDeactivated(object sender, EventArgs e)
		{
			if (ApplicationDelegate != null)
				ApplicationDelegate.ApplicationDidEnterBackground(this);

#if !IOS
			if (HandleMediaStateAutomatically)
			{
				CocosDenshion.CCSimpleAudioEngine.SharedEngine.RestoreMediaState();
			}
#endif
		}

		void GameExiting(object sender, EventArgs e)
		{
			foreach (CCWindow window in gameWindows)
			{
				window.EndAllSceneDirectors();
			}
		}

		public void ClearTouches()
		{
			touches.Clear();
			touchMap.Clear();
		}

		protected override void LoadContent()
		{
			if (!initialized)
			{
				CCContentManager.Initialize(Game.Content.ServiceProvider, Game.Content.RootDirectory);

				base.LoadContent();

				if (ApplicationDelegate != null)
                    ApplicationDelegate.ApplicationDidFinishLaunching(this, this.MainWindow);

				initialized = true;
			}
			else
			{
				base.LoadContent();
			}
		}

		public override void Initialize()
		{
			InitInstance();

			base.Initialize();
		}

		public override void Update(GameTime gameTime)
		{
			XnaGameTime = gameTime;

			GameTime.ElapsedGameTime = gameTime.ElapsedGameTime;
			GameTime.IsRunningSlowly = gameTime.IsRunningSlowly;
			GameTime.TotalGameTime = gameTime.TotalGameTime;

#if !NETFX_CORE
            foreach(CCWindow window in gameWindows)
            {
                if (window.Accelerometer != null 
                    && window.Accelerometer.Enabled
                    && window.EventDispatcher.IsEventListenersFor(CCEventListenerAccelerometer.LISTENER_ID))
                {
                    window.Accelerometer.Update();
                }
            }

#endif


			foreach (CCWindow window in gameWindows)
			{
				ProcessTouch(window);

				if (window.GamePadEnabled)
				{
					ProcessGamePad(window);
				}

				ProcessKeyboard(window);
				ProcessMouse(window);

				if (!paused)
				{
					if (isNextDeltaTimeZero)
					{
						deltaTime = 0;
						isNextDeltaTimeZero = false;
					}
					else
					{
						deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
					}

					Scheduler.Update(deltaTime);

					window.Update(deltaTime);
				}
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			XnaGameTime = gameTime;

			GameTime.ElapsedGameTime = gameTime.ElapsedGameTime;
			GameTime.IsRunningSlowly = gameTime.IsRunningSlowly;
			GameTime.TotalGameTime = gameTime.TotalGameTime;

			foreach (CCWindow window in gameWindows)
			{
				window.DrawManager.BeginDraw();

				window.MainLoop(GameTime);

				window.DrawManager.EndDraw();
			}

			base.Draw(gameTime);
		}

		public void ToggleFullScreen()
		{
			var service = Game.Services.GetService(typeof(IGraphicsDeviceService));
			var manager = service as GraphicsDeviceManager;

			Debug.Assert(manager != null, "CCApplication: GraphicsManager is not setup");
			if (manager != null)
			{
				manager.ToggleFullScreen();
			}
		}

		internal virtual void HandleGesture(GestureSample gesture)
		{
			//TODO: Create CCGesture and convert the coordinates into the local coordinates.
		}


		#region GamePad Support

		void ProcessGamePad(CCWindow window, GamePadState gps, PlayerIndex player)
		{
			var dispatcher = window.EventDispatcher;

			var lastState = new GamePadState();

			if (!priorGamePadState.ContainsKey(player) && gps.IsConnected)
			{
				gamePadConnection.IsConnected = true;
				gamePadConnection.Player = (CCPlayerIndex)player;
				dispatcher.DispatchEvent(gamePadConnection);

			}

			if (priorGamePadState.ContainsKey(player))
			{
				lastState = priorGamePadState[player];
				// Notify listeners when the gamepad is connected/disconnected.
				if ((lastState.IsConnected != gps.IsConnected))
				{
					gamePadConnection.IsConnected = false;
					gamePadConnection.Player = (CCPlayerIndex)player;
					dispatcher.DispatchEvent(gamePadConnection);

				}
				// TODO: Check button pressed/released status for button tap events.
			}

			if (gps.IsConnected)
			{
				var caps = GamePad.GetCapabilities(player);

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

					if (caps.HasBackButton)
					{
						back = (gps.Buttons.Back == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasStartButton)
					{
						start = (gps.Buttons.Start == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasBigButton)
					{
						system = (gps.Buttons.BigButton == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasAButton)
					{
						a = (gps.Buttons.A == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasBButton)
					{
						b = (gps.Buttons.B == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasXButton)
					{
						x = (gps.Buttons.X == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasYButton)
					{
						y = (gps.Buttons.Y == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasLeftShoulderButton)
					{
						leftShoulder = (gps.Buttons.LeftShoulder == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasRightShoulderButton)
					{
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

					dispatcher.DispatchEvent(gamePadButton);
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
					if (caps.HasLeftXThumbStick || caps.HasLeftYThumbStick)
					{
						vecLeft = new CCPoint(gps.ThumbSticks.Left);
						vecLeft.Normalize();
					}
					else
					{
						vecLeft = CCPoint.Zero;
					}
					CCPoint vecRight;
					if (caps.HasRightXThumbStick || caps.HasRightYThumbStick)
					{
						vecRight = new CCPoint(gps.ThumbSticks.Right);
						vecRight.Normalize();
					}
					else
					{
						vecRight = CCPoint.Zero;
					}
					var left = new CCGameStickStatus();
					left.Direction = vecLeft;
					left.Magnitude = ((caps.HasLeftXThumbStick || caps.HasLeftYThumbStick) ? gps.ThumbSticks.Left.Length() : 0f);
					left.IsDown = ((caps.HasLeftStickButton) ? gps.IsButtonDown(Buttons.LeftStick) : false);
					var right = new CCGameStickStatus();
					right.Direction = vecRight;
					right.Magnitude = ((caps.HasRightXThumbStick || caps.HasRightYThumbStick) ? gps.ThumbSticks.Right.Length() : 0f);
					right.IsDown = ((caps.HasLeftStickButton) ? gps.IsButtonDown(Buttons.RightStick) : false);

					gamePadStick.Left = left;
					gamePadStick.Right = right;
					gamePadStick.Player = (CCPlayerIndex)player;

					dispatcher.DispatchEvent(gamePadStick);

				}
				// Process the game triggers
				if (caps.HasLeftTrigger || caps.HasRightTrigger)
				{
					//GamePadTriggerUpdate (caps.HasLeftTrigger ? gps.Triggers.Left : 0f, caps.HasRightTrigger ? gps.Triggers.Right : 0f, player);
					gamePadTrigger.Left = caps.HasLeftTrigger ? gps.Triggers.Left : 0f;
					gamePadTrigger.Right = caps.HasRightTrigger ? gps.Triggers.Right : 0f;
					gamePadTrigger.Player = (CCPlayerIndex)player;

					dispatcher.DispatchEvent(gamePadTrigger);
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

					if (caps.HasDPadDownButton)
					{
						downButton = (gps.DPad.Down == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasDPadUpButton)
					{
						upButton = (gps.DPad.Up == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasDPadLeftButton)
					{
						leftButton = (gps.DPad.Left == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}
					if (caps.HasDPadRightButton)
					{
						rightButton = (gps.DPad.Right == ButtonState.Pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released);
					}

					gamePadDPad.Down = downButton;
					gamePadDPad.Up = upButton;
					gamePadDPad.Left = leftButton;
					gamePadDPad.Right = rightButton;

					gamePadDPad.Player = (CCPlayerIndex)player;

					dispatcher.DispatchEvent(gamePadDPad);
				}
			}
			priorGamePadState[player] = gps;
		}

		void ProcessGamePad(CCWindow window)
		{

			if (window.GamePadEnabled &&
				window.EventDispatcher.IsEventListenersFor(CCEventListenerGamePad.LISTENER_ID))
			{

				// On Android, the gamepad is always connected.
				GamePadState gps1 = GamePad.GetState(PlayerIndex.One);
				GamePadState gps2 = GamePad.GetState(PlayerIndex.Two);
				GamePadState gps3 = GamePad.GetState(PlayerIndex.Three);
				GamePadState gps4 = GamePad.GetState(PlayerIndex.Four);
				ProcessGamePad(window, gps1, PlayerIndex.One);
				ProcessGamePad(window, gps2, PlayerIndex.Two);
				ProcessGamePad(window, gps3, PlayerIndex.Three);
				ProcessGamePad(window, gps4, PlayerIndex.Four);
			}
		}

		#endregion Gamepad support


		#region Keyboard support

		void ProcessKeyboard(CCWindow window)
		{
			// Read the current keyboard state
			KeyboardState currentKeyboardState = Keyboard.GetState();

			var dispatcher = window.EventDispatcher;

			if (currentKeyboardState == priorKeyboardState || !dispatcher.IsEventListenersFor(CCEventListenerKeyboard.LISTENER_ID))
			{
				priorKeyboardState = currentKeyboardState;
				return;
			}


			var keyboardEvent = new CCEventKeyboard(CCKeyboardEventType.KEYBOARD_PRESS);
			var keyboardState = new CCKeyboardState() { KeyboardState = currentKeyboardState };

			keyboardEvent.KeyboardState = keyboardState;

			// Check for pressed/released keys.
			// Loop for each possible pressed key (those that are pressed this update)
			Keys[] keys = currentKeyboardState.GetPressedKeys();

			for (int k = 0; k < keys.Length; k++)
			{
				// Was this key up during the last update?
				if (priorKeyboardState.IsKeyUp(keys[k]))
				{

					// Yes, so this key has been pressed
					//CCLog.Log("Pressed: " + keys[i].ToString());
					keyboardEvent.Keys = (CCKeys)keys[k];
					dispatcher.DispatchEvent(keyboardEvent);
				}
			}

			// Loop for each possible released key (those that were pressed last update)
			keys = priorKeyboardState.GetPressedKeys();
			keyboardEvent.KeyboardEventType = CCKeyboardEventType.KEYBOARD_RELEASE;
			for (int k = 0; k < keys.Length; k++)
			{
				// Is this key now up?
				if (currentKeyboardState.IsKeyUp(keys[k]))
				{
					// Yes, so this key has been released
					//CCLog.Log("Released: " + keys[i].ToString());
					keyboardEvent.Keys = (CCKeys)keys[k];
					dispatcher.DispatchEvent(keyboardEvent);

				}
			}

			// Store the state for the next loop
			priorKeyboardState = currentKeyboardState;

		}

		#endregion Keyboard support


		#region Mouse support

		void ProcessMouse(CCWindow window)
		{
			// Read the current Mouse state
			MouseState currentMouseState = Mouse.GetState();

			var dispatcher = window.EventDispatcher;

			if (currentMouseState == priorMouseState || !dispatcher.IsEventListenersFor(CCEventListenerMouse.LISTENER_ID))
			{
				priorMouseState = currentMouseState;
				return;
			}

			CCPoint pos;

#if NETFX_CORE
			//Because MonoGame and CocosSharp uses different Y axis, we need to convert the coordinate here
			pos = TransformPoint(priorMouseState.X, Game.Window.ClientBounds.Height - priorMouseState.Y);
#else
            //Because MonoGame and CocosSharp uses different Y axis, we need to convert the coordinate here
            pos = new CCPoint(priorMouseState.X, Game.Window.ClientBounds.Height - priorMouseState.Y);
#endif


			var mouseEvent = new CCEventMouse(CCMouseEventType.MOUSE_MOVE);
			mouseEvent.CursorX = pos.X;
			mouseEvent.CursorY = pos.Y;

			dispatcher.DispatchEvent(mouseEvent);

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
				dispatcher.DispatchEvent(mouseEvent);
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
				dispatcher.DispatchEvent(mouseEvent);
			}

			if (priorMouseState.ScrollWheelValue != currentMouseState.ScrollWheelValue)
			{
				var delta = priorMouseState.ScrollWheelValue - currentMouseState.ScrollWheelValue;
				if (delta != 0)
				{
					mouseEvent.MouseEventType = CCMouseEventType.MOUSE_SCROLL;
					mouseEvent.ScrollX = 0;
					mouseEvent.ScrollY = delta;
					dispatcher.DispatchEvent(mouseEvent);
					//Console.WriteLine ("mouse scroll: " + mouseEvent.ScrollY);
				}
			}
			// Store the state for the next loop
			priorMouseState = currentMouseState;
		}

		#endregion Mouse support


		CCPoint TransformPoint(float x, float y)
		{
			CCPoint newPoint;
			newPoint.X = x * TouchPanel.DisplayWidth / Game.Window.ClientBounds.Width;
			newPoint.Y = y * TouchPanel.DisplayHeight / Game.Window.ClientBounds.Height;
			return newPoint;
		}

		void ProcessTouch(CCWindow window)
		{
			if (window.EventDispatcher.IsEventListenersFor(CCEventListenerTouchOneByOne.LISTENER_ID)
				|| window.EventDispatcher.IsEventListenersFor(CCEventListenerTouchAllAtOnce.LISTENER_ID))
			{
				newTouches.Clear();
				movedTouches.Clear();
				endedTouches.Clear();

				CCPoint pos = CCPoint.Zero;

				// TODO: allow configuration to treat the game pad as a touch device.

#if WINDOWS || WINDOWSGL || MACOS
                pos = new CCPoint(lastMouseState.X, lastMouseState.Y);
                prevMouseState = lastMouseState;
                lastMouseState = Mouse.GetState();

                if (prevMouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
                {
                    lastMouseId++;
                    touches.AddLast(new CCTouch(lastMouseId, pos.X, pos.Y));
                    touchMap.Add(lastMouseId, touches.Last);
                    newTouches.Add(touches.Last.Value);
                }
                else if (prevMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (touchMap.ContainsKey(lastMouseId))
                    {
                        if (prevMouseState.X != lastMouseState.X || prevMouseState.Y != lastMouseState.Y)
                        {
                            movedTouches.Add(touchMap[lastMouseId].Value);
                            touchMap[lastMouseId].Value.SetTouchInfo(lastMouseId, pos.X, pos.Y);
                        }
                    }
                }
                else if (prevMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
                {
                    if (touchMap.ContainsKey(lastMouseId))
                    {
                        endedTouches.Add(touchMap[lastMouseId].Value);
                        touches.Remove(touchMap[lastMouseId]);
                        touchMap.Remove(lastMouseId);
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

							pos = new CCPoint(touch.Position.X, touch.Position.Y);

							touches.AddLast(new CCTouch(touch.Id, pos.X, pos.Y));
							touchMap.Add(touch.Id, touches.Last);
							newTouches.Add(touches.Last.Value);

							break;

						case TouchLocationState.Moved:
							LinkedListNode<CCTouch> existingTouch;
							if (touchMap.TryGetValue(touch.Id, out existingTouch))
							{
								pos = new CCPoint(touch.Position.X, touch.Position.Y);
								var delta = existingTouch.Value.LocationOnScreen - pos;
								if (delta.LengthSquared > 1.0f)
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
					window.EventDispatcher.DispatchEvent(touchEvent);
				}

				if (movedTouches.Count > 0)
				{
					touchEvent.EventCode = CCEventCode.MOVED;
					touchEvent.Touches = movedTouches;
					window.EventDispatcher.DispatchEvent(touchEvent);
				}

				if (endedTouches.Count > 0)
				{
					touchEvent.EventCode = CCEventCode.ENDED;
					touchEvent.Touches = endedTouches;
					window.EventDispatcher.DispatchEvent(touchEvent);
				}
			}
		}

		CCTouch GetTouchBasedOnId(int nID)
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
	}
}