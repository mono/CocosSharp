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
        static CCDirector s_sharedDirector;

        readonly float kDefaultFPS = 60f;
        readonly List<CCScene> m_pobScenesStack = new List<CCScene>();
        bool m_bNextDeltaTimeZero;
        bool m_bPaused;
        protected bool m_bPurgeDirecotorInNextLoop; // this flag will be set to true in end()
        bool m_bSendCleanupToScene;
        protected double m_dAnimationInterval;
        protected double m_dOldAnimationInterval;
        CCDirectorProjection m_eProjection;
        float m_fContentScaleFactor = 1.0f;
        float m_fDeltaTime;
        bool m_NeedsInit = true;
        internal CCSize m_obWinSizeInPoints;
		
#if !PSM &&!NETFX_CORE
        CCAccelerometer m_pAccelerometer;
        string m_sStorageDirName = "CocosSharpDirector";
        string m_sSaveFileName = "SceneList.dat";
        string m_sSceneSaveFileName = "Scene{0}.dat";
#endif
        CCActionManager m_pActionManager;
        CCKeypadDispatcher m_pKeypadDispatcher;
        CCKeyboardDispatcher m_pKeyboardDispatcher;
        CCScene m_pNextScene;
        CCNode m_pNotificationNode;
        ICCDirectorDelegate m_pProjectionDelegate;
        CCScene m_pRunningScene;
        CCScheduler m_pScheduler;
        CCTouchDispatcher m_pTouchDispatcher;
        CCStats m_pStats;

        bool m_GamePadEnabled = false;

        /// <summary>
        /// returns a shared instance of the director
        /// </summary>
        /// <value> </value>
        public static CCDirector SharedDirector
        {
            get
            {
                if (s_sharedDirector == null)
                {
                    s_sharedDirector = new CCDisplayLinkDirector();
                }
                return s_sharedDirector;
            }
        }

        public float ZEye
        {
            get { return (m_obWinSizeInPoints.Height / 1.1566f); }
        }

        public CCSize VisibleSize
        {
            get { return CCDrawManager.VisibleSize; }
        }

        public CCPoint VisibleOrigin
        {
            get { return CCDrawManager.VisibleOrigin; }
        }

        public CCScheduler Scheduler
        {
            get { return m_pScheduler; }
            set { m_pScheduler = value; }
        }

        public CCActionManager ActionManager
        {
            get { return m_pActionManager; }
            set { m_pActionManager = value; }
        }

        public CCTouchDispatcher TouchDispatcher
        {
            get { return m_pTouchDispatcher; }
            set { m_pTouchDispatcher = value; }
        }

        public CCKeypadDispatcher KeypadDispatcher
        {
            get { return m_pKeypadDispatcher; }
            set { m_pKeypadDispatcher = value; }
        }

        public CCKeyboardDispatcher KeyboardDispatcher
        {
            get { return m_pKeyboardDispatcher; }
            set { m_pKeyboardDispatcher = value; }
        }

        #if !PSM &&!NETFX_CORE
        public CCAccelerometer Accelerometer
        {
            get { return m_pAccelerometer; }
            set { m_pAccelerometer = value; }
        }
        #endif

        public CCScene RunningScene
        {
            get { return m_pRunningScene; }
        }

        public virtual double AnimationInterval
        {
            get { return m_dAnimationInterval; }
            set { m_dAnimationInterval = value; }
        }

        public bool IsPaused
        {
            get { return m_bPaused; }
        }

        public CCNode NotificationNode
        {
            get { return m_pNotificationNode; }
            set { m_pNotificationNode = value; }
        }

        public ICCDirectorDelegate Delegate
        {
            get { return m_pProjectionDelegate; }
            set { m_pProjectionDelegate = value; }
        }

        public CCDirectorProjection Projection
        {
            get { return m_eProjection; }
            set
            {
                SetViewport();

                CCSize size = m_obWinSizeInPoints;

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
                    if (m_pProjectionDelegate != null)
                    {
                        m_pProjectionDelegate.UpdateProjection();
                    }
                    break;

                default:
                    Debug.Assert(true, "cocos2d: Director: unrecognized projection");
                    break;
                }

                m_eProjection = value;
            }
        }

        public CCSize WinSize
        {
            get { return m_obWinSizeInPoints; }
        }

        public CCSize WinSizeInPixels
        {
            get { return m_obWinSizeInPoints * m_fContentScaleFactor; }
        }

        /// <summary>
        /// Set to true if this platform has a game pad connected.
        /// </summary>
        public bool GamePadEnabled
        {
            get { return (m_GamePadEnabled); }
            set { m_GamePadEnabled = value; }
        }

        public float ContentScaleFactor
        {
            get { return m_fContentScaleFactor; }
            set
            {
                if (value != m_fContentScaleFactor)
                {
                    m_fContentScaleFactor = value;
                }
            }
        }

        /** Give the number of scenes present in the scene stack.
         *  Note: this count also includes the root scene node.
         */
        public int SceneCount
        {
            get { return m_pobScenesStack.Count; }
        }

        /// <summary>
        /// Returns true if there is more than 1 scene on the stack.
        /// </summary>
        /// <returns></returns>
        public bool CanPopScene
        {
            get
            {
                int c = m_pobScenesStack.Count;
                return (c > 1);
            }
        }

        /// <summary>
        /// Control stats display status.
        /// </summary>
        /// <value><c>true</c> if stats is displayed; otherwise, <c>false</c>.</value>
        public bool DisplayStats {
            get { return m_pStats.IsEnabled; }
            set { m_pStats.IsEnabled = value; }
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
            m_pRunningScene = null;
            m_pNextScene = null;

            m_pNotificationNode = null;

            m_dOldAnimationInterval = m_dAnimationInterval = 1.0 / kDefaultFPS;

            // Set default projection (3D)
            m_eProjection = CCDirectorProjection.Default;

            // projection delegate if "Custom" projection is used
            m_pProjectionDelegate = null;
            
            // stats
            m_pStats = new CCStats ();

            // paused ?
            m_bPaused = false;

            // purge ?
            m_bPurgeDirecotorInNextLoop = false;

            m_obWinSizeInPoints = CCSize.Zero;

            // m_pobOpenGLView = null;

            m_fContentScaleFactor = 1.0f;

            // scheduler
            m_pScheduler = new CCScheduler();

            // action manager
            m_pActionManager = new CCActionManager();
            m_pScheduler.ScheduleUpdateForTarget(m_pActionManager, CCScheduler.kCCPrioritySystem, false);

            // touchDispatcher
            m_pTouchDispatcher = new CCTouchDispatcher();
            m_pTouchDispatcher.Init();

            // KeypadDispatcher
            m_pKeypadDispatcher = new CCKeypadDispatcher();

            // KeyboardDispatcher
            m_pKeyboardDispatcher = new CCKeyboardDispatcher();

            // Accelerometer
            #if !PSM &&!NETFX_CORE
            m_pAccelerometer = new CCAccelerometer();
            #endif

            // create autorelease pool
            // CCPoolManager::sharedPoolManager()->push();

            m_NeedsInit = false;
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
                if (storage.DirectoryExists(m_sStorageDirName))
                {
                    DeleteState(storage);
                }

                // otherwise just create the directory
                else
                {
                    storage.CreateDirectory(m_sStorageDirName);
                }

                // create a file we'll use to store the list of screens in the stack

                CCLog.Log("Saving CCDirector state to file: " + Path.Combine(m_sStorageDirName, m_sSaveFileName));

                try
                {
                    using (IsolatedStorageFileStream stream = storage.OpenFile(Path.Combine(m_sStorageDirName, m_sSaveFileName), FileMode.OpenOrCreate))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                    {
                        // write out the full name of all the types in our stack so we can
                        // recreate them if needed.
                        foreach (CCScene scene in m_pobScenesStack)
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
                        if (m_pRunningScene != null && m_pRunningScene.IsSerializable)
                        {
                                writer.WriteLine("m_pRunningScene");
                                writer.WriteLine(m_pRunningScene.GetType().AssemblyQualifiedName);
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
                foreach (CCScene scene in m_pobScenesStack)
                {
                    if (scene.IsSerializable)
                    {
                        fileName = string.Format(Path.Combine(m_sStorageDirName, m_sSceneSaveFileName), screenIndex);

                        // open up the stream and let the screen serialize whatever state it wants
                        using (IsolatedStorageFileStream stream = storage.CreateFile(fileName))
                        {
                            scene.Serialize(stream);
                        }

                        screenIndex++;
                    }
                }
                // Write the current running scene
                if (m_pRunningScene != null && m_pRunningScene.IsSerializable)
                {
                    fileName = string.Format(Path.Combine(m_sStorageDirName, m_sSceneSaveFileName), "XX");
                    // open up the stream and let the screen serialize whatever state it wants
                    using (IsolatedStorageFileStream stream = storage.CreateFile(fileName))
                    {
                        m_pRunningScene.Serialize(stream);
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
                if (storage.DirectoryExists(m_sStorageDirName))
                {
                    string saveFile = System.IO.Path.Combine(m_sStorageDirName, m_sSaveFileName);
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
                        for (int i = 0; i < m_pobScenesStack.Count; i++)
                        {
                                string filename = System.IO.Path.Combine(m_sStorageDirName, string.Format(m_sSceneSaveFileName, i));
                                if (storage.FileExists(filename))
                                {
                            using (IsolatedStorageFileStream stream = storage.OpenFile(filename, FileMode.Open, FileAccess.Read))
                            {
                                        CCLog.Log("Restoring state for scene {0}", filename);
                                m_pobScenesStack[i].Deserialize(stream);
                            }
                        }
                            }
                        if (m_pobScenesStack.Count > 0)
                        {
                                CCLog.Log("Director is running with scene..");

                            RunWithScene(m_pobScenesStack[m_pobScenesStack.Count - 1]); // always at the top of the stack
                        }
                        return (m_pobScenesStack.Count > 0 && m_pRunningScene != null);
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
            string[] files = storage.GetFileNames(System.IO.Path.Combine(m_sStorageDirName, "*"));
            foreach (string file in files)
            {
                storage.DeleteFile(Path.Combine(m_sStorageDirName, file));
            }
        }
#endif
        #endregion

        public void SetViewport()
        {
            CCDrawManager.SetViewPortInPoints(0, 0, (int)m_obWinSizeInPoints.Width, (int)m_obWinSizeInPoints.Height);
        }

        internal void SetGlDefaultValues()
        {
            SetAlphaBlending(true);
            SetDepthTest(false);

            Projection = m_eProjection;

            // set other opengl default values
            //ClearColor = new Color(0, 0, 0, 255);
        }

        protected void SetDefaultValues()
        {
        }

        public void Update(GameTime gameTime)
        {
            // Start stats measuring
            m_pStats.UpdateStart ();
            
            if (!m_bPaused)
            {
                if (m_bNextDeltaTimeZero)
                {
                    m_fDeltaTime = 0;
                    m_bNextDeltaTimeZero = false;
                }
                else
                {
                    m_fDeltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
                }

                // In Seconds
                m_pScheduler.update(m_fDeltaTime);
            }

            /* to avoid flickr, nextScene MUST be here: after tick and before draw.
             XXX: Which bug is this one. It seems that it can't be reproduced with v0.9 */
            if (m_pNextScene != null)
            {
                SetNextScene();
            }

            // End stats measuring
            m_pStats.UpdateEnd (m_fDeltaTime);
        }

        /// <summary>
        /// Draw the scene.
        /// This method is called every frame. Don't call it manually.
        /// </summary>
        protected void DrawScene(GameTime gameTime)
        {
            if (m_NeedsInit)
            {
                return;
            }

            // Start stats measuring
            m_pStats.UpdateStart ();

            CCDrawManager.PushMatrix();

            // draw the scene
            if (m_pRunningScene != null)
            {
                m_pRunningScene.Visit();
            }

            // draw the notifications node
            if (m_pNotificationNode != null)
            {
                NotificationNode.Visit();
            }

            CCDrawManager.PopMatrix();

            // Draw stats
            m_pStats.Draw ();
        }


        public abstract void MainLoop(GameTime gameTime);

        public void SetOpenGlView()
        {
            // set size
            m_obWinSizeInPoints = CCDrawManager.DesignResolutionSize;

            // Prepare stats
            m_pStats.Initialize ();

            SetGlDefaultValues();

            CCApplication.SharedApplication.TouchDelegate = m_pTouchDispatcher;
            m_pTouchDispatcher.IsDispatchEvents = true;
        }

        public void SetNextDeltaTimeZero(bool bNextDeltaTimeZero)
        {
            m_bNextDeltaTimeZero = bNextDeltaTimeZero;
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
        public void SetAlphaBlending(bool bOn)
        {
            if (bOn)
            {
                CCDrawManager.BlendFunc(CCBlendFunc.AlphaBlend);
            }
            else
            {
                CCDrawManager.BlendFunc(new CCBlendFunc(CCOGLES.GL_ONE, CCOGLES.GL_ZERO));
            }
        }

        /// <summary>
        /// enables/disables OpenGL depth test
        /// </summary>
        /// <param name="bOn"></param>
        public void SetDepthTest(bool bOn)
        {
            CCDrawManager.DepthTest = bOn;
        }

        public CCPoint ConvertToGl(CCPoint uiPoint)
        {
            return new CCPoint(uiPoint.X, m_obWinSizeInPoints.Height - uiPoint.Y);
        }

        public CCPoint ConvertToUi(CCPoint glPoint)
        {
            return new CCPoint(glPoint.X, m_obWinSizeInPoints.Height - glPoint.Y);
        }

        public void End()
        {
            m_bPurgeDirecotorInNextLoop = true;
        }

        protected void PurgeDirector()
        {
            // cleanup scheduler
            Scheduler.UnscheduleAll();

            // don't release the event handlers
            // They are needed in case the director is run again
            m_pTouchDispatcher.RemoveAllDelegates();

            if (m_pRunningScene != null)
            {
                m_pRunningScene.OnExitTransitionDidStart();
                m_pRunningScene.OnExit();
                m_pRunningScene.Cleanup();
            }

            m_pRunningScene = null;
            m_pNextScene = null;

            // remove all objects, but don't release it.
            // runWithScene might be executed after 'end'.
            m_pobScenesStack.Clear();

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

            m_NeedsInit = true;
        }

        public void Pause()
        {
            if (m_bPaused)
            {
                return;
            }

            m_dOldAnimationInterval = m_dAnimationInterval;

            // when paused, don't consume CPU
            AnimationInterval = 1 / 4.0;
            m_bPaused = true;
        }

        public void ResumeFromBackground()
        {
            Resume();
            
            if (m_pRunningScene != null)
            {
                bool runningIsTransition = m_pRunningScene is CCTransitionScene;
                if (!runningIsTransition)
                {
                    m_pRunningScene.OnEnter();
                    m_pRunningScene.OnEnterTransitionDidFinish();
                }
            }
        }

        public void Resume()
        {
            if (m_NeedsInit)
            {
                CCLog.Log("CCDirector(): Resume needs Init(). The director will re-initialize.");
                InitCCDirector();
            }
            if (!m_bPaused)
            {
                return;
            }

            CCLog.Log("CCDirector(): Resume called with {0} scenes", m_pobScenesStack.Count);

            AnimationInterval = m_dOldAnimationInterval;

            m_bPaused = false;
            m_fDeltaTime = 0;
        }

        public bool IsSendCleanupToScene()
        {
            return m_bSendCleanupToScene;
        }

        public abstract void StopAnimation();

        public abstract void StartAnimation();


        #region Scene Management

        public void ResetSceneStack()
        {
            CCLog.Log("CCDirector(): ResetSceneStack, clearing out {0} scenes.", m_pobScenesStack.Count);

            m_pRunningScene = null;
            m_pobScenesStack.Clear();
            m_pNextScene = null;
        }

        public void RunWithScene(CCScene pScene)
        {
            Debug.Assert(pScene != null, "the scene should not be null");
            Debug.Assert(m_pRunningScene == null, "Use runWithScene: instead to start the director");

            PushScene(pScene);
            StartAnimation();
        }

        /// <summary>
        /// Replaces the current scene at the top of the stack with the given scene.
        /// </summary>
        /// <param name="pScene"></param>
        public void ReplaceScene(CCScene pScene)
        {
            Debug.Assert(m_pRunningScene != null, "Use runWithScene: instead to start the director");
            Debug.Assert(pScene != null, "the scene should not be null");

            int index = m_pobScenesStack.Count;

            m_bSendCleanupToScene = true;
            if (index == 0)
            {
                m_pobScenesStack.Add(pScene);
            }
            else
            {
            m_pobScenesStack[index - 1] = pScene;
            }
            m_pNextScene = pScene;
        }

        /// <summary>
        /// Push the given scene to the top of the scene stack.
        /// </summary>
        /// <param name="pScene"></param>
        public void PushScene(CCScene pScene)
        {
            Debug.Assert(pScene != null, "the scene should not null");

            m_bSendCleanupToScene = false;

            m_pobScenesStack.Add(pScene);
            m_pNextScene = pScene;
        }

        public void PopScene(float t, CCTransitionScene s)
        {
            Debug.Assert(m_pRunningScene != null, "m_pRunningScene cannot be null");

            if (m_pobScenesStack.Count > 0)
            {
                // CCScene s = m_pobScenesStack[m_pobScenesStack.Count - 1];
                m_pobScenesStack.RemoveAt(m_pobScenesStack.Count - 1);
            }
            int c = m_pobScenesStack.Count;

            if (c == 0)
            {
                End(); // This should not happen here b/c we need to capture the current state and just deactivate the game (for Android).
            }
            else
            {
                m_bSendCleanupToScene = true;
                m_pNextScene = m_pobScenesStack[c - 1];
                if (s != null)
                {
                    m_pNextScene.Visible = true;
                    s.Reset(t, m_pNextScene);
                    m_pobScenesStack.Add(s);
                    m_pNextScene = s;
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
            Debug.Assert(m_pRunningScene != null, "A running Scene is needed");
            int c = m_pobScenesStack.Count;

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
                var current = m_pobScenesStack[m_pobScenesStack.Count - 1];
                
                if (current.IsRunning)
                {
                    current.OnExitTransitionDidStart();
                    current.OnExit();
                }
                
                current.Cleanup();
                m_pobScenesStack.RemoveAt(m_pobScenesStack.Count - 1);
                c--;
            }
            
            m_pNextScene = m_pobScenesStack[m_pobScenesStack.Count - 1];
            m_bSendCleanupToScene = false;
        }

        protected void SetNextScene()
        {
            bool runningIsTransition = m_pRunningScene != null && m_pRunningScene.IsTransition;// is CCTransitionScene;

            // If it is not a transition, call onExit/cleanup
            if (!m_pNextScene.IsTransition)
            {
                if (m_pRunningScene != null)
                {
                    m_pRunningScene.OnExitTransitionDidStart(); 
                    m_pRunningScene.OnExit();

                    // issue #709. the root node (scene) should receive the cleanup message too
                    // otherwise it might be leaked.
                    if (m_bSendCleanupToScene)
                    {
                        m_pRunningScene.Cleanup();

                        GC.Collect();
                    }
                }
            }

            m_pRunningScene = m_pNextScene;
            m_pNextScene = null;

            if (!runningIsTransition && m_pRunningScene != null)
            {
                m_pRunningScene.OnEnter();
                m_pRunningScene.OnEnterTransitionDidFinish();
            }
        }

		#endregion
    }
}