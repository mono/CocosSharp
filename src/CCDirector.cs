using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
#if !NETFX_CORE
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

    public enum CCDirectorProjection
    {

        Projection2D,           /// Sets a 2D projection (orthogonal projection)
        Projection3D,           /// Sets a 3D projection with a fovy=60, znear=0.5f and zfar=1500.
        Custom,                 /// Calls "updateProjection" on the projection delegate.
        Default = Projection3D  /// Default projection is 3D projection
    }

    /// <summary>
    /// Class that creates and handle the a window and manages how and when to execute the Scenes.
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

        #if !NETFX_CORE
        const string storageDirName = "CocosSharpDirector";
        const string saveFileName = "SceneList.dat";
        const string sceneSaveFileName = "Scene{0}.dat";
        #endif

        readonly float defaultFPS = 60f;
        readonly List<CCScene> scenesStack = new List<CCScene>();

        bool isNeedsInit = true;
        double prevAnimationInterval;
        CCDirectorProjection directorProjection;

        CCScene nextScene;

        CCEventCustom eventAfterDraw;
        CCEventCustom eventAfterVisit;
        CCEventCustom eventAfterUpdate;
        CCEventCustom eventProjectionChanged;


        #region Properties

        #if !NETFX_CORE
        public CCAccelerometer Accelerometer { get; set; }
        #endif

        public bool GamePadEnabled { get; set; }                            // Set to true if this platform has a game pad connected.
        public bool IsSendCleanupToScene { get; private set; }
        public float ContentScaleFactor { get; set; }
        public virtual double AnimationInterval { get; set; }
        public CCSize WindowSizeInPoints { get; internal set; }
        public ICCDirectorDelegate ProjectionDelegate { get; set; }
        public CCScene RunningScene { get; private set; }
        public CCNode NotificationNode { get; set; }
        public CCEventDispatcher EventDispatcher { get; private set; }

        protected bool IsPurgeDirectorInNextLoop { get; set; }
        protected CCStats Stats { get; private set; }

        public bool CanPopScene
        {
            get
            {
                int c = scenesStack.Count;
                return (c > 1);
            }
        }

        // enables/disables OpenGL alpha blending 
        public bool IsUseAlphaBlending
        {
            set 
            {
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

        // enables/disables OpenGL depth test
        public bool IsUseDepthTesting
        {
            get { return CCDrawManager.DepthTest; }
            set { CCDrawManager.DepthTest = value; }
        }

        public bool DisplayStats 
        {
            get { return Stats.IsEnabled; }
            set { Stats.IsEnabled = value; }
        }

        public int SceneCount
        {
            get { return scenesStack.Count; }
        }

        public float ZEye
        {
            get { return (WindowSizeInPoints.Height / 1.1566f); }
        }

        public CCPoint VisibleOrigin
        {
            get { return CCDrawManager.VisibleOrigin; }
        }

        public CCSize VisibleSize
        {
            get { return CCDrawManager.VisibleSize; }
        }

        public CCSize WindowSizeInPixels
        {
            get { return WindowSizeInPoints * ContentScaleFactor; }
        }

        public CCDirectorProjection Projection
        {
            get { return directorProjection; }
            set
            {
                CCDrawManager.SetViewPortInPoints(0, 0, (int)WindowSizeInPoints.Width, (int)WindowSizeInPoints.Height);

                CCSize size = WindowSizeInPoints;

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

                if(EventDispatcher.IsEventListenersFor(EVENT_PROJECTION_CHANGED))
                    EventDispatcher.DispatchEvent(eventProjectionChanged);
            }
        }

        #endregion Properties


        #region Constructors

        protected CCDirector()
        {
            InitCCDirector();
        }

        // Also called after purging the director
        void InitCCDirector()
        {
            IsPurgeDirectorInNextLoop = false;
            ContentScaleFactor = 1.0f;
            WindowSizeInPoints = CCSize.Zero;

            RunningScene = null;
            NotificationNode = null;
            ProjectionDelegate = null;

            Stats = new CCStats();

            EventDispatcher = new CCEventDispatcher(this);

            #if !NETFX_CORE
            Accelerometer = new CCAccelerometer(this);
            #endif

            isNeedsInit = false;
            prevAnimationInterval = AnimationInterval = 1.0 / defaultFPS;
            directorProjection = CCDirectorProjection.Default;
            nextScene = null;

            eventAfterDraw = new CCEventCustom(EVENT_AFTER_DRAW);
            eventAfterDraw.UserData = this;
            eventAfterVisit = new CCEventCustom(EVENT_AFTER_VISIT);
            eventAfterVisit.UserData = this;
            eventAfterUpdate = new CCEventCustom(EVENT_AFTER_UPDATE);
            eventAfterUpdate.UserData = this;
            eventProjectionChanged = new CCEventCustom(EVENT_PROJECTION_CHANGED);
            eventProjectionChanged.UserData = this;
        }


        #endregion Constructors

        public abstract void MainLoop(CCGameTime gameTime);

        public abstract void StopAnimation();
        public abstract void StartAnimation();

        public CCPoint ConvertToGl(CCPoint uiPoint)
        {
            return new CCPoint(uiPoint.X, WindowSizeInPoints.Height - uiPoint.Y);
        }

        public CCPoint ConvertToUi(CCPoint glPoint)
        {
            return new CCPoint(glPoint.X, WindowSizeInPoints.Height - glPoint.Y);
        }


        #region Cleaning up

        internal void End()
        {
            IsPurgeDirectorInNextLoop = true;
        }

        // Re Initializes the statistics.  This needs to be called for example by coming back from tombstombing.
        internal void ReInitStats()
        {
            Stats = new CCStats();
        }

        protected void PurgeDirector()
        {
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

            if(EventDispatcher != null)
                EventDispatcher.RemoveAll();

            isNeedsInit = true;
        }

        #endregion Cleaning up


        #region Setting up and drawing/updating view

        internal void SetOpenGlView()
        {
            // set size
            WindowSizeInPoints = CCDrawManager.DesignResolutionSize;

            // Prepare stats
            Stats.Initialize();

            SetGlDefaultValues();
        }

        internal void SetGlDefaultValues()
        {
            IsUseAlphaBlending = true;
            IsUseDepthTesting = false;

            Projection = directorProjection;
        }

        // Draw the scene.
        // This method is called every frame. Don't call it manually.
        protected void DrawScene(CCGameTime gameTime)
        {
            if (isNeedsInit)
            {
                return;
            }

            // Start stats measuring
            Stats.UpdateStart();

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
            Stats.Draw();
        }

        internal void Update(float deltaTime)
        {
            // Start stats measuring
            Stats.UpdateStart();

            if (EventDispatcher.IsEventListenersFor(EVENT_AFTER_UPDATE)) 
            {
                EventDispatcher.DispatchEvent(eventAfterUpdate);
            }

            /* to avoid flickr, nextScene MUST be here: after tick and before draw.
             XXX: Which bug is this one. It seems that it can't be reproduced with v0.9 */
            if (nextScene != null)
            {
                SetNextScene();
            }

            // End stats measuring
            Stats.UpdateEnd(deltaTime);
        }

        #endregion Setting up and drawing view


        #region Scene Management

        public void ResetSceneStack()
        {
            CCLog.Log("CCDirector(): ResetSceneStack, clearing out {0} scenes.", scenesStack.Count);

            RunningScene = null;
            scenesStack.Clear();
            nextScene = null;
        }

        public void RunWithScene(CCScene scene)
        {
            Debug.Assert(scene != null, "the scene should not be null");
            Debug.Assert(RunningScene == null, "Use runWithScene: instead to start the director");

            PushScene(scene);
            StartAnimation();
        }

        // Replaces the current scene at the top of the stack with the given scene.
        public void ReplaceScene(CCScene scene)
        {
            Debug.Assert(RunningScene != null, "Use runWithScene: instead to start the director");
            Debug.Assert(scene != null, "the scene should not be null");

            int index = scenesStack.Count;

            IsSendCleanupToScene = true;
            if (index == 0)
            {
                scenesStack.Add(scene);
            }
            else
            {
                scenesStack[index - 1] = scene;
            }
            nextScene = scene;
        }

        // Push the given scene to the top of the scene stack.
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

            RunningScene = nextScene;
            nextScene = null;

            if (!runningIsTransition && RunningScene != null)
            {
                RunningScene.Director = this;
                RunningScene.OnEnter();
                RunningScene.OnEnterTransitionDidFinish();
            }
        }

        #endregion Scene management


        #region State Management

        #if !NETFX_CORE

        // Write out the current state of the director and all of its scenes.
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

        void DeserializeMyState(string name, string v)
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
            
        // Deletes the saved state files from isolated storage.
        void DeleteState(IsolatedStorageFile storage)
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
    }
}