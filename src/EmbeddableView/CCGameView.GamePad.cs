using System;
using System.Collections.Generic;

using CocosSharp;

namespace CocosSharp
{
    public partial class CCGameView
    {
        bool gamePadEnabled;

        Dictionary<int, CCEventGamePad> gamePadMap;
        List<CCEventGamePad> incomingConnectionStatus;
        List<CCEventGamePad> incomingNewPresses;
        List<CCEventGamePad> incomingChangedPresses;
        List<CCEventGamePad> incomingReleasePresses;

        object gamePadLock = new object();

        #region Initialisation

        void InitialiseGamePadInputHandling()
        {
            gamePadMap = new Dictionary<int, CCEventGamePad>();
            incomingConnectionStatus = new List<CCEventGamePad>();
            incomingNewPresses = new List<CCEventGamePad>();
            incomingChangedPresses = new List<CCEventGamePad>();
            incomingReleasePresses = new List<CCEventGamePad>();

            GamePadEnabled = true;

        }

        #endregion Initialisation

        #region Properties

        public bool GamePadEnabled
        {
            get { return gamePadEnabled; }
            set
            {
                gamePadEnabled = value;
                PlatformUpdateGamePadEnabled();
            }
        }

        #endregion Properties

        #region GamePad handling

        void AddIncomingConnection(int connectionId, bool connected, CCPlayerIndex player )
        {
            lock (gamePadLock) 
            {
                if (!gamePadMap.ContainsKey (connectionId)) 
                {
                    var press = new CCEventGamePadConnection (connectionId, gameTime.ElapsedGameTime);
                    press.IsConnected = connected;
                    press.Player = player;

                    gamePadMap.Add (connectionId, press);
                    incomingConnectionStatus.Add (press);
                }
            }
        }

        void AddIncomingNewPress(int pressId, CCEventGamePad gamePadEvent)
        {
            lock (gamePadLock) 
            {
                if (!gamePadMap.ContainsKey (pressId)) 
                {
                    gamePadMap.Add (pressId, gamePadEvent);
                    incomingNewPresses.Add (gamePadEvent);
                }
            }
        }

        void UpdateIncomingReleasePress(int pressId, CCEventGamePad existingPress)
        {
            lock (gamePadLock) 
            {
                incomingReleasePresses.Add (existingPress);
                gamePadMap.Remove (pressId);
            }
        }

        void ProcessGamePadInput()
        {
            lock (gamePadLock) 
            {
                if (EventDispatcher.IsEventListenersFor (CCEventListenerGamePad.LISTENER_ID))
                {
                    if (incomingConnectionStatus.Count > 0)
                    {
                        foreach(var connection in incomingConnectionStatus)
                        {
                            EventDispatcher.DispatchEvent (connection);
                        }
                    }

                    RemoveOldPresses ();

                    if (incomingNewPresses.Count > 0) 
                    {
                        foreach (var gpEvent in incomingNewPresses) 
                        {
                            EventDispatcher.DispatchEvent (gpEvent);
                        }
                    }

                    if (incomingReleasePresses.Count > 0) 
                    {
                        foreach (var gpEvent in incomingReleasePresses) 
                        {
                            EventDispatcher.DispatchEvent (gpEvent);
                        }
                    }

                    incomingConnectionStatus.Clear ();
                    incomingNewPresses.Clear ();
                    incomingReleasePresses.Clear ();
                }
            }

        }

        // Prevent memory leaks by removing stale presses
        // In particular, in the case of the game entering the background
        // a press event may not have been triggered within the view 
        void RemoveOldPresses()
        {
            lock (gamePadLock) 
            {
                var currentTime = gameTime.ElapsedGameTime;

                if (gamePadMap.Count > 0) 
                {
                    foreach (CCEventGamePad gpEvent in gamePadMap.Values) {
                        if (!incomingReleasePresses.Contains (gpEvent)
                        && (currentTime - gpEvent.TimeStamp) > TouchTimeLimit) {
                            incomingReleasePresses.Add (gpEvent);
                            gamePadMap.Remove (gpEvent.Id);
                        }
                    }
                }
            }
        }
        #endregion
    }
}

