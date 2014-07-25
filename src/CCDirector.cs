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

    // Class that creates that manages how and when to execute the Scenes.

    public class CCDirector
    {
        #if !NETFX_CORE
        const string storageDirName = "CocosSharpDirector";
        const string saveFileName = "SceneList.dat";
        const string sceneSaveFileName = "Scene{0}.dat";
        #endif

        readonly List<CCScene> scenesStack = new List<CCScene>();


        #region Properties

        public bool IsSendCleanupToScene { get; private set; }
        public CCScene RunningScene { get; private set; }

        internal bool IsPurgeDirectorInNextLoop { get; set; }
        internal CCScene NextScene { get; private set; }

        public bool CanPopScene
        {
            get
            {
                int c = scenesStack.Count;
                return (c > 1);
            }
        }

        public int SceneCount
        {
            get { return scenesStack.Count; }
        }

        #endregion Properties


        #region Constructors

        public CCDirector()
        {
        }

        #endregion Constructors


        #region Cleaning up

        internal void End()
        {
            IsPurgeDirectorInNextLoop = true;
        }

        internal void PurgeDirector()
        {
            if (RunningScene != null)
            {
                RunningScene.OnExitTransitionDidStart();
                RunningScene.OnExit();
                RunningScene.Cleanup();
            }

            RunningScene = null;
            NextScene = null;

            // remove all objects, but don't release it.
            // runWithScene might be executed after 'end'.
            scenesStack.Clear();
        }

        #endregion Cleaning up


        #region Scene Management

        public void ResetSceneStack()
        {
            CCLog.Log("CCDirector(): ResetSceneStack, clearing out {0} scenes.", scenesStack.Count);

            RunningScene = null;
            scenesStack.Clear();
            NextScene = null;
        }

        public void RunWithScene(CCScene scene)
        {
            Debug.Assert(scene != null, "the scene should not be null");
            Debug.Assert(RunningScene == null, "Use runWithScene: instead to start the director");

            PushScene(scene);
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
            NextScene = scene;
        }

        // Push the given scene to the top of the scene stack.
        public void PushScene(CCScene pScene)
        {
            Debug.Assert(pScene != null, "the scene should not null");

            IsSendCleanupToScene = false;

            scenesStack.Add(pScene);
            NextScene = pScene;
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
                NextScene = scenesStack[c - 1];
                if (s != null)
                {
                    NextScene.Visible = true;
                    s.Reset(t, NextScene);
                    scenesStack.Add(s);
                    NextScene = s;
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

            NextScene = scenesStack[scenesStack.Count - 1];
            IsSendCleanupToScene = false;
        }

        internal void SetNextScene()
        {
            bool runningIsTransition = RunningScene != null && RunningScene.IsTransition;// is CCTransitionScene;

            // If it is not a transition, call onExit/cleanup
            if (!NextScene.IsTransition)
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

            if (NextScene.Director != this) 
            {
                NextScene.Director = this;
            }

			if (NextScene.Camera == null)
				NextScene.Camera = RunningScene.Camera;

            RunningScene = NextScene;
            NextScene = null;

            if (!runningIsTransition && RunningScene != null)
            {
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