using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
#if !PSM &&!NETFX_CORE
using System.IO.IsolatedStorage;
#endif
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace CocosSharp
{
    public interface ICCDirectorDelegate
    {
        void UpdateProjection();
    }

    /// <summary>
    ///  Possible OpenGL projections used by director
    /// </summary>
    public enum CCDirectorProjection
    {
        /// sets a 2D projection (orthogonal projection)
        Projection2D,

        /// sets a 3D projection with a fovy=60, znear=0.5f and zfar=1500.
        Projection3D,

        /// it calls "updateProjection" on the projection delegate.
        Custom,

        /// Default projection is 3D projection
        Default = Projection3D
    }

    /// <summary>
    /// Class that creates and handle the main Window and manages how and when to execute the Scenes.
    /// 
    /// The CCDirector is also responsible for:
    ///     - initializing the OpenGL context
    ///     - setting the OpenGL pixel format (default on is RGB565)
    ///     - setting the OpenGL buffer depth (default one is 0-bit)
    ///     - setting the projection (default one is 3D)
    ///     - setting the orientation (default one is Portrait)
    /// 
    /// Since the CCDirector is a singleton, the standard way to use it is by calling: _
    /// CCDirector::sharedDirector()->methodName();
    /// 
    /// The CCDirector also sets the default OpenGL context:
    ///     - GL_TEXTURE_2D is enabled
    ///     - GL_VERTEX_ARRAY is enabled
    ///     - GL_COLOR_ARRAY is enabled
    ///     - GL_TEXTURE_COORD_ARRAY is enabled.  
    /// </summary>

    public abstract class CCDirector
    {

		public static string EVENT_PROJECTION_CHANGED = "director_projection_changed";
		public static string EVENT_AFTER_UPDATE = "director_after_update";
		public static string EVENT_AFTER_VISIT = "director_after_visit";
		public static string EVENT_AFTER_DRAW = "director_after_draw";

		static CCDirector sharedDirector;

		readonly float defaultFPS = 60f;
		readonly List<CCScene> scenesStack = new List<CCScene>();
		
		CCDirectorProjection directorProjection;

		float deltaTime;
		bool isNeedsInit = true;
		CCScene nextScene;

#if !PSM && !NETFX_CORE
		public CCAccelerometer Accelerometer { get; set; }
		const string storageDirName = "CocosSharpDirector";
		const string saveFileName = "SceneList.dat";
		const string sceneSaveFileName = "Scene{0}.dat";
#endif
		public CCEventDispatcher EventDispatcher { get; set; }
		CCEventCustom eventAfterDraw;
		CCEventCustom eventAfterVisit;
		CCEventCustom eventAfterUpdate;
		CCEventCustom eventProjectionChanged;

		public CCActionManager ActionManager { get; set; }
		public virtual double AnimationInterval { get; set; }
		public float ContentScaleFactor { get; set; }
		/// <summary>
		/// Set to true if this platform has a game pad connected.
		/// </summary>
		public bool GamePadEnabled { get; set; }
		public bool IsNextDeltaTimeZero { get; set; }
		public bool IsPaused { get; private set; }
		protected bool IsPurgeDirectorInNextLoop { get; set; } // this flag will be set to true in end()
		public bool IsSendCleanupToScene { get; private set; }
		public CCNode NotificationNode { get; set; }
		protected double OldAnimationInterval { get; set; }
		public ICCDirectorDelegate ProjectionDelegate { get; set; }
		public CCScene RunningScene { get; private set; }
		public CCScheduler Scheduler { get; set; }
		internal CCSize WinSizeInPoints { get; set; }
		protected CCStats Stats;

		// Dispatchers
		public CCKeypadDispatcher KeypadDispatcher  { get; set; }

        /// <summary>
        /// returns a shared instance of the director
        /// </summary>
        /// <value> </value>
        public static CCDirector SharedDirector
        {
            get
            {
                if (sharedDirector == null)
                {
                    sharedDirector = new CCDisplayLinkDirector();
                }
                return sharedDirector;
            }
        }

        public float ZEye
        {
            get { return (WinSizeInPoints.Height / 1.1566f); }
        }

        public CCSize VisibleSize
        {
            get { return CCDrawManager.VisibleSize; }
        }

        public CCPoint VisibleOrigin
        {
            get { return CCDrawManager.VisibleOrigin; }
        }


        /// <summary>
        /// Control stats display status.
        /// </summary>
        /// <value><c>true</c> if stats is displayed; otherwise, <c>false</c>.</value>
        public bool DisplayStats {
            get { return Stats.IsEnabled; }
            set { Stats.IsEnabled = value; }
        }

        public CCDirectorProjection Projection
        {
            get { return directorProjection; }
            set
            {
                SetViewport();

                CCSize size = WinSizeInPoints;

                switch (value)
                {
                case CCDirectorProjection.Projection2D:

                    CCDrawManager.ProjectionMatrix = Matrix.CreateOrthographicOffCenter(
                        0, size.Width,
                        0, size.Height,
                        -1024.0f, 1024.0f
                    );

                    CCDrawManager.ViewMatrix = Matrix.Identity;

                    CCDrawManager.WorldMatrix = Matrix.Identity;
                    break;

                case CCDirectorProjection.Projection3D:

                    CCDrawManager.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.Pi / 3.0f,
                        size.Width / size.Height,
                        0.1f, 1500 //ZEye * 2f
                    );

                    CCDrawManager.ViewMatrix = Matrix.CreateLookAt(
                        new Vector3(size.Width / 2.0f, size.Height / 2.0f, ZEye),
                        new Vector3(size.Width / 2.0f, size.Height / 2.0f, 0f),
                        Vector3.Up
                    );

                    CCDrawManager.WorldMatrix = Matrix.Identity;
                    break;

                case CCDirectorProjection.Custom:
                    if (ProjectionDelegate != null)
                    {
                        ProjectionDelegate.UpdateProjection();
                    }
                    break;

                default:
                    Debug.Assert(true, "cocos2d: Director: unrecognized projection");
                    break;
                }

                directorProjection = value;
				if (EventDispatcher.IsEventListenersFor(EVENT_PROJECTION_CHANGED))
					EventDispatcher.DispatchEvent(eventProjectionChanged);
            }
        }

        public CCSize WinSize
        {
            get { return WinSizeInPoints; }
        }

        public CCSize WinSizeInPixels
        {
            get { return WinSizeInPoints * ContentScaleFactor; }
        }

        /** Give the number of scenes present in the scene stack.
         *  Note: this count also includes the root scene node.
         */
        public int SceneCount
        {
            get { return scenesStack.Count; }
        }

        /// <summary>
        /// Returns true if there is more than 1 scene on the stack.
        /// </summary>
        /// <returns></returns>
        public bool IsCanPopScene
        {
            get
            {
                int c = scenesStack.Count;
                return (c > 1);
            }
        }

        #region Constructors

        protected CCDirector()
        {
            InitCCDirector();
        }

        // Purging the director requires we re-initialize
        private void InitCCDirector()
        {
            SetDefaultValues();

            // scenes
            RunningScene = null;
            nextScene = null;

            NotificationNode = null;

            OldAnimationInterval = AnimationInterval = 1.0 / defaultFPS;

            // Set default projection (3D)
            directorProjection = CCDirectorProjection.Default;

            // projection delegate if "Custom" projection is used
            ProjectionDelegate = null;

            // stats
            Stats = new CCStats ();
            
            // paused ?
            IsPaused = false;

            // purge ?
            IsPurgeDirectorInNextLoop = false;

            WinSizeInPoints = CCSize.Zero;

            //m_pobOpenGLView = null;

            ContentScaleFactor = 1.0f;

            // scheduler
            Scheduler = new CCScheduler();
            
            // action manager
            ActionManager = new CCActionManager();
            Scheduler.Schedule (ActionManager, CCSchedulePriority.System, false);
            
			// EventDispatcher
			EventDispatcher = new CCEventDispatcher ();

			eventAfterDraw = new CCEventCustom(EVENT_AFTER_DRAW);
			eventAfterDraw.UserData = this;
			eventAfterVisit = new CCEventCustom(EVENT_AFTER_VISIT);
			eventAfterVisit.UserData = this;
			eventAfterUpdate = new CCEventCustom(EVENT_AFTER_UPDATE);
			eventAfterUpdate.UserData = this;
			eventProjectionChanged = new CCEventCustom(EVENT_PROJECTION_CHANGED);
			eventProjectionChanged.UserData = this;

            // KeypadDispatcher
            KeypadDispatcher = new CCKeypadDispatcher();

            // Accelerometer
            #if !PSM &&!NETFX_CORE
            Accelerometer = new CCAccelerometer();
            #endif

            // create autorelease pool
            //CCPoolManager::sharedPoolManager()->push();

            isNeedsInit = false;
        }

        #endregion Constructors


        #region State Management
		
#if !PSM &&!NETFX_CORE

        /// <summary>
        /// Write out the current state of the director and all of its scenes.
        /// </summary>
        public void SerializeState()
        {
            // open up isolated storage
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // if our screen manager directory already exists, delete the contents
                if (storage.DirectoryExists(storageDirName))
                {
                    DeleteState(storage);
                }

                // otherwise just create the directory
                else
                {
                    storage.CreateDirectory(storageDirName);
                }

                // create a file we'll use to store the list of screens in the stack

                CCLog.Log("Saving CCDirector state to file: " + Path.Combine(storageDirName, saveFileName));

                try
                {
                    using (IsolatedStorageFileStream stream = storage.OpenFile(Path.Combine(storageDirName, saveFileName), FileMode.OpenOrCreate))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                    {
                        // write out the full name of all the types in our stack so we can
                        // recreate them if needed.
                        foreach (CCScene scene in scenesStack)
                        {
                            if (scene.IsSerializable)
                            {
                                    writer.WriteLine(scene.GetType().AssemblyQualifiedName);
                                }
                                else
                                {
                                    CCLog.Log("Scene is not serializable: " + scene.GetType().FullName);
                            }
                        }
                        // Write out our local state
                        if (RunningScene != null && RunningScene.IsSerializable)
                        {
                                writer.WriteLine("m_pRunningScene");
                                writer.WriteLine(RunningScene.GetType().AssemblyQualifiedName);
                        }
                        // Add my own state 
                        // [*]name=value
                        //

                    }
                }

                // now we create a new file stream for each screen so it can save its state
                // if it needs to. we name each file "ScreenX.dat" where X is the index of
                // the screen in the stack, to ensure the files are uniquely named
                int screenIndex = 0;
                string fileName = null;
                foreach (CCScene scene in scenesStack)
                {
                    if (scene.IsSerializable)
                    {
                        fileName = string.Format(Path.Combine(storageDirName, sceneSaveFileName), screenIndex);

                        // open up the stream and let the screen serialize whatever state it wants
                        using (IsolatedStorageFileStream stream = storage.CreateFile(fileName))
                        {
                            scene.Serialize(stream);
                        }

                        screenIndex++;
                    }
                }
                // Write the current running scene
                if (RunningScene != null && RunningScene.IsSerializable)
                {
                    fileName = string.Format(Path.Combine(storageDirName, sceneSaveFileName), "XX");
                    // open up the stream and let the screen serialize whatever state it wants
                    using (IsolatedStorageFileStream stream = storage.CreateFile(fileName))
                    {
                        RunningScene.Serialize(stream);
                    }
                }
            }
                catch (Exception ex)
                {
                    CCLog.Log("Failed to serialize the CCDirector state. Erasing the save files.");
                    CCLog.Log(ex.ToString());
                    DeleteState(storage);
                }
        }
        }

        private void DeserializeMyState(string name, string v)
        {
            // TODO
        }

        public bool DeserializeState()
        {
            try
            {
            // open up isolated storage
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // see if our saved state directory exists
                if (storage.DirectoryExists(storageDirName))
                {
                    string saveFile = System.IO.Path.Combine(storageDirName, saveFileName);
                    try
                    {
                            CCLog.Log("Loading director data file: {0}", saveFile);
                        // see if we have a screen list
                        if (storage.FileExists(saveFile))
                        {
                            // load the list of screen types
                            using (IsolatedStorageFileStream stream = storage.OpenFile(saveFile, FileMode.Open, FileAccess.Read))
                            {
                                    using (StreamReader reader = new StreamReader(stream))
                                {
                                        CCLog.Log("Director save file contains {0} bytes.", reader.BaseStream.Length);
                                        try
                                        {
                                            while (true)
                                    {
                                        // read a line from our file
                                                string line = reader.ReadLine();
                                                if (line == null)
                                                {
                                                    break;
                                                }
                                                CCLog.Log("Restoring: {0}", line);

                                        // if it isn't blank, we can create a screen from it
                                        if (!string.IsNullOrEmpty(line))
                                        {
                                            if (line.StartsWith("[*]"))
                                            {
                                                // Reading my state
                                                string s = line.Substring(3);
                                                int idx = s.IndexOf('=');
                                                if (idx > -1)
                                                {
                                                    string name = s.Substring(0, idx);
                                                    string v = s.Substring(idx + 1);
                                                            CCLog.Log("Restoring: {0} = {1}", name, v);
                                                    DeserializeMyState(name, v);
                                                }
                                            }
                                            else
                                            {
                                                Type screenType = Type.GetType(line);
                                                CCScene scene = Activator.CreateInstance(screenType) as CCScene;
                                                        PushScene(scene);
                                                        //                                                    m_pobScenesStack.Add(scene);
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            // EndOfStreamException
                                            // this is OK here.
                                    }
                                }
                            }
                            // Now we deserialize our own state.
                        }
                            else
                            {
                                CCLog.Log("save file does not exist.");
                            }

                        // next we give each screen a chance to deserialize from the disk
                        for (int i = 0; i < scenesStack.Count; i++)
                        {
                                string filename = System.IO.Path.Combine(storageDirName, string.Format(sceneSaveFileName, i));
                                if (storage.FileExists(filename))
                                {
                            using (IsolatedStorageFileStream stream = storage.OpenFile(filename, FileMode.Open, FileAccess.Read))
                            {
                                        CCLog.Log("Restoring state for scene {0}", filename);
                                scenesStack[i].Deserialize(stream);
                            }
                        }
                            }
                        if (scenesStack.Count > 0)
                        {
                                CCLog.Log("Director is running with scene..");

                            RunWithScene(scenesStack[scenesStack.Count - 1]); // always at the top of the stack
                        }
                        return (scenesStack.Count > 0 && RunningScene != null);
                    }
                        catch (Exception ex)
                    {
                        // if an exception was thrown while reading, odds are we cannot recover
                        // from the saved state, so we will delete it so the game can correctly
                        // launch.
                        DeleteState(storage);
                            CCLog.Log("Failed to deserialize the director state, removing old storage file.");
                            CCLog.Log(ex.ToString());
                    }
                }
            }
            }
            catch (Exception ex)
            {
                CCLog.Log("Failed to deserialize director state.");
                CCLog.Log(ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// Deletes the saved state files from isolated storage.
        /// </summary>
        private void DeleteState(IsolatedStorageFile storage)
        {
            // glob on all of the files in the directory and delete them
            string[] files = storage.GetFileNames(System.IO.Path.Combine(storageDirName, "*"));
            foreach (string file in files)
            {
                storage.DeleteFile(Path.Combine(storageDirName, file));
            }
        }
#endif
        #endregion

        public void SetViewport()
        {
            CCDrawManager.SetViewPortInPoints(0, 0, (int)WinSizeInPoints.Width, (int)WinSizeInPoints.Height);
        }

        internal void SetGlDefaultValues()
        {
			IsUseAlphaBlending = true;
			IsUseDepthTesting = false;

            Projection = directorProjection;

            // set other opengl default values
            //ClearColor = new Color(0, 0, 0, 255);
        }

        protected void SetDefaultValues()
        {
        }

        public void Update(GameTime gameTime)
        {
            // Start stats measuring
            Stats.UpdateStart ();
            
            if (!IsPaused)
            {
                if (IsNextDeltaTimeZero)
                {
                    deltaTime = 0;
                    IsNextDeltaTimeZero = false;
                }
                else
                {
                    deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
                }

                // In Seconds
                Scheduler.Update(deltaTime);
				if (EventDispatcher.IsEventListenersFor(EVENT_AFTER_UPDATE));
					EventDispatcher.DispatchEvent(eventAfterUpdate);
            }

            /* to avoid flickr, nextScene MUST be here: after tick and before draw.
             XXX: Which bug is this one. It seems that it can't be reproduced with v0.9 */
            if (nextScene != null)
            {
                SetNextScene();
            }

            // End stats measuring
            Stats.UpdateEnd (deltaTime);
        }

        /// <summary>
        /// Draw the scene.
        /// This method is called every frame. Don't call it manually.
        /// </summary>
        protected void DrawScene(GameTime gameTime)
        {
            if (isNeedsInit)
            {
                return;
            }

            // Start stats measuring
            Stats.UpdateStart ();

            CCDrawManager.PushMatrix();

            // draw the scene
            if (RunningScene != null)
            {
                RunningScene.Visit();
				if (EventDispatcher.IsEventListenersFor(EVENT_AFTER_VISIT))
					EventDispatcher.DispatchEvent(eventAfterVisit);
            }

            // draw the notifications node
            if (NotificationNode != null)
            {
                NotificationNode.Visit();
            }

			if (EventDispatcher.IsEventListenersFor(EVENT_AFTER_DRAW))
				EventDispatcher.DispatchEvent(eventAfterDraw);

            CCDrawManager.PopMatrix();

            // Draw stats
            Stats.Draw ();
        }


        public abstract void MainLoop(GameTime gameTime);

        public void SetOpenGlView()
        {
            // set size
            WinSizeInPoints = CCDrawManager.DesignResolutionSize;

            // Prepare stats
            Stats.Initialize ();

            SetGlDefaultValues();

        }

        public void PurgeCachedData()
        {
            CCLabelBMFont.PurgeCachedData();
            CCTextureCache.SharedTextureCache.RemoveAllTextures();
            //CCFileUtils::sharedFileUtils()->purgeCachedEntries();
        }

        /// <summary>
        /// enables/disables OpenGL alpha blending 
        /// </summary>
        /// <param name="bOn"></param>
		public bool IsUseAlphaBlending
        {
			set {
				if (value)
            {
                CCDrawManager.BlendFunc(CCBlendFunc.AlphaBlend);
            }
            else
            {
                CCDrawManager.BlendFunc(new CCBlendFunc(CCOGLES.GL_ONE, CCOGLES.GL_ZERO));
            }
        }
        }

        /// <summary>
        /// enables/disables OpenGL depth test
        /// </summary>
        /// <param name="bOn"></param>
		public bool IsUseDepthTesting
        {
			get { return CCDrawManager.DepthTest; }
			set { CCDrawManager.DepthTest = value; }
        }

        public CCPoint ConvertToGl(CCPoint uiPoint)
        {
            return new CCPoint(uiPoint.X, WinSizeInPoints.Height - uiPoint.Y);
        }

        public CCPoint ConvertToUi(CCPoint glPoint)
        {
            return new CCPoint(glPoint.X, WinSizeInPoints.Height - glPoint.Y);
        }

        public void End()
        {
            IsPurgeDirectorInNextLoop = true;
        }

        protected void PurgeDirector()
        {
            // cleanup scheduler
            Scheduler.UnscheduleAll();

            if (RunningScene != null)
            {
                RunningScene.OnExitTransitionDidStart();
                RunningScene.OnExit();
                RunningScene.Cleanup();
            }

            RunningScene = null;
            nextScene = null;

            // remove all objects, but don't release it.
            // runWithScene might be executed after 'end'.
            scenesStack.Clear();

            StopAnimation();

            // purge bitmap cache
            CCLabelBMFont.PurgeCachedData();

            // purge all managed caches
            CCAnimationCache.PurgeSharedAnimationCache();
            CCSpriteFrameCache.PurgeSharedSpriteFrameCache();
            CCTextureCache.PurgeSharedTextureCache();
            //CCFileUtils.purgeFileUtils();
            //CCConfiguration.purgeConfiguration();

            // cocos2d-x specific data structures
            //CCUserDefault.purgeSharedUserDefault();
            //CCNotificationCenter.purgeNotificationCenter();

            CCDrawManager.PurgeDrawManager();

            isNeedsInit = true;
        }

        public void Pause()
        {
            if (IsPaused)
            {
                return;
            }

            OldAnimationInterval = AnimationInterval;

            // when paused, don't consume CPU
            AnimationInterval = 1 / 4.0;
            IsPaused = true;
        }

        public void ResumeFromBackground()
        {
            Resume();
            
            if (RunningScene != null)
            {
                bool runningIsTransition = RunningScene is CCTransitionScene;
                if (!runningIsTransition)
                {
                    RunningScene.OnEnter();
                    RunningScene.OnEnterTransitionDidFinish();
                }
            }
        }

        public void Resume()
        {
            if (isNeedsInit)
            {
                CCLog.Log("CCDirector(): Resume needs Init(). The director will re-initialize.");
                InitCCDirector();
            }
            if (!IsPaused)
            {
                return;
            }

            CCLog.Log("CCDirector(): Resume called with {0} scenes", scenesStack.Count);

            AnimationInterval = OldAnimationInterval;

            IsPaused = false;
            deltaTime = 0;
        }

        public abstract void StopAnimation();

        public abstract void StartAnimation();


        #region Scene Management

        public void ResetSceneStack()
        {
            CCLog.Log("CCDirector(): ResetSceneStack, clearing out {0} scenes.", scenesStack.Count);

            RunningScene = null;
            scenesStack.Clear();
            nextScene = null;
        }

        public void RunWithScene(CCScene pScene)
        {
            Debug.Assert(pScene != null, "the scene should not be null");
            Debug.Assert(RunningScene == null, "Use runWithScene: instead to start the director");

            PushScene(pScene);
            StartAnimation();
        }

        /// <summary>
        /// Replaces the current scene at the top of the stack with the given scene.
        /// </summary>
        /// <param name="pScene"></param>
        public void ReplaceScene(CCScene pScene)
        {
            Debug.Assert(RunningScene != null, "Use runWithScene: instead to start the director");
            Debug.Assert(pScene != null, "the scene should not be null");

            int index = scenesStack.Count;

            IsSendCleanupToScene = true;
            if (index == 0)
            {
                scenesStack.Add(pScene);
            }
            else
            {
            scenesStack[index - 1] = pScene;
            }
            nextScene = pScene;
        }

        /// <summary>
        /// Push the given scene to the top of the scene stack.
        /// </summary>
        /// <param name="pScene"></param>
        public void PushScene(CCScene pScene)
        {
            Debug.Assert(pScene != null, "the scene should not null");

            IsSendCleanupToScene = false;

            scenesStack.Add(pScene);
            nextScene = pScene;
        }

        public void PopScene(float t, CCTransitionScene s)
        {
            Debug.Assert(RunningScene != null, "m_pRunningScene cannot be null");

            if (scenesStack.Count > 0)
            {
                // CCScene s = m_pobScenesStack[m_pobScenesStack.Count - 1];
                scenesStack.RemoveAt(scenesStack.Count - 1);
            }
            int c = scenesStack.Count;

            if (c == 0)
            {
                End(); // This should not happen here b/c we need to capture the current state and just deactivate the game (for Android).
            }
            else
            {
                IsSendCleanupToScene = true;
                nextScene = scenesStack[c - 1];
                if (s != null)
                {
                    nextScene.Visible = true;
                    s.Reset(t, nextScene);
                    scenesStack.Add(s);
                    nextScene = s;
                }
            }
        }

        public void PopScene()
        {
            PopScene(0, null);
        }

        /** Pops out all scenes from the queue until the root scene in the queue.
        * This scene will replace the running one.
        * Internally it will call `PopToSceneStackLevel(1)`
        */
        public void PopToRootScene()
        {
            PopToSceneStackLevel(1);
        }

        /** Pops out all scenes from the queue until it reaches `level`.
         *   If level is 0, it will end the director.
         *   If level is 1, it will pop all scenes until it reaches to root scene.
         *   If level is <= than the current stack level, it won't do anything.
         */
        public void PopToSceneStackLevel(int level)
        {
            Debug.Assert(RunningScene != null, "A running Scene is needed");
            int c = scenesStack.Count;

            // level 0? -> end
            if (level == 0)
            {
                End();
                return;
            }
            
            // current level or lower -> nothing
            if (level >= c)
                return;
            
            // pop stack until reaching desired level
            while (c > level)
            {
                var current = scenesStack[scenesStack.Count - 1];
                
                if (current.IsRunning)
                {
                    current.OnExitTransitionDidStart();
                    current.OnExit();
                }
                
                current.Cleanup();
                scenesStack.RemoveAt(scenesStack.Count - 1);
                c--;
            }
            
            nextScene = scenesStack[scenesStack.Count - 1];
            IsSendCleanupToScene = false;
        }

        protected void SetNextScene()
        {
            bool runningIsTransition = RunningScene != null && RunningScene.IsTransition;// is CCTransitionScene;

            // If it is not a transition, call onExit/cleanup
            if (!nextScene.IsTransition)
            {
                if (RunningScene != null)
                {
                    RunningScene.OnExitTransitionDidStart(); 
                    RunningScene.OnExit();

                    // issue #709. the root node (scene) should receive the cleanup message too
                    // otherwise it might be leaked.
                    if (IsSendCleanupToScene)
                    {
                        RunningScene.Cleanup();
                    }
                }
            }

			// This is so that we get rid of the listeners in Cocos2d-x they use a release on the object
			// so that event listeners are detached.  We do not control when an object is released so
			// we call this directly.
			if (RunningScene != null) 
			{
				RunningScene.Dispose ();
			}

            RunningScene = nextScene;
            nextScene = null;

            if (!runningIsTransition && RunningScene != null)
            {
                RunningScene.OnEnter();
                RunningScene.OnEnterTransitionDidFinish();
            }
        }

		#endregion

    }
}