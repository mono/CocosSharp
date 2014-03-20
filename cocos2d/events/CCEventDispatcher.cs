
// used for testing
//#define DUMP_LISTENER_ITEM_PRIORITY_INFO


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CocosSharp
{

    /// <summary>
    /// Priority dirty flag
    /// </summary>
	[Flags]
	enum DirtyFlag
    {
        NONE = 0,
        FIXED_PRIORITY = 0x01,
        SCENE_GRAPH_PRIORITY = 0x02,
        ALL = FIXED_PRIORITY | SCENE_GRAPH_PRIORITY
    }

    public class CCEventDispatcher
    {

        /// <summary>
        /// The listeners to be added after dispatching event
        /// </summary>
        List<CCEventListener> toBeAddedListeners;

        /// <summary>
        /// Listeners map
        /// </summary>
        Dictionary<string, CCEventListenerVector> listenerMap;

        /// <summary>
        /// The map of dirty flag
        /// </summary>
        Dictionary<string, DirtyFlag> priorityDirtyFlagMap;

        /// <summary>
        /// The map of node and event listeners
        /// </summary>
        Dictionary<CCNode, List<CCEventListener>> nodeListenersMap;

        /// <summary>
        /// The map of node and its event priority
        /// </summary>
        Dictionary<CCNode, int> nodePriorityMap;

        /// <summary>
        /// key: Global Z Order, value: Sorted Nodes
        /// </summary>
        Dictionary<float, List<CCNode>> globalZOrderNodeMap;

        /// <summary>
        /// The nodes were associated with scene graph based priority listeners
        /// </summary>
        SortedSet<CCNode> dirtyNodes;

        SortedSet<string> internalCustomListenerIDs;

        /// <summary>
        /// Whether the dispatcher is dispatching event
        /// </summary>
        int inDispatch;

        int nodePriorityIndex;

        /// <summary>
        /// 
        /// </summary>
        public CCEventDispatcher()
        {
            toBeAddedListeners = new List<CCEventListener>(50);

            listenerMap = new Dictionary<string, CCEventListenerVector>();
            priorityDirtyFlagMap = new Dictionary<string, DirtyFlag>();
            nodeListenersMap = new Dictionary<CCNode, List<CCEventListener>>();
            nodePriorityMap = new Dictionary<CCNode, int>();
            globalZOrderNodeMap = new Dictionary<float, List<CCNode>>();
            dirtyNodes = new SortedSet<CCNode>();
            internalCustomListenerIDs = new SortedSet<string>();
            IsEnabled = true;
            inDispatch = 0;
            nodePriorityIndex = 0;

            //internalCustomListenerIDs.Add(EVENT_COME_TO_FOREGROUND);
            //internalCustomListenerIDs.Add(EVENT_COME_TO_BACKGROUND);
        }

        static string GetListenerID (CCEvent listenerEvent)
        {
            string ret = string.Empty;
            switch (listenerEvent.Type)
            {
                case CCEventType.ACCELERATION:
                    ret = CCEventListenerAccelerometer.LISTENER_ID;
                    break;
                //case CCEventType.CUSTOM:
                //    {
                //        auto customEvent = static_cast<EventCustom*>(event);
                //        ret = customEvent->getEventName();
                //    }
                //    break;
                case CCEventType.KEYBOARD:
                    ret = CCEventListenerKeyboard.LISTENER_ID;
                    break;
                case CCEventType.MOUSE:
                    ret = CCEventListenerMouse.LISTENER_ID;
                    break;
                case CCEventType.TOUCH:
                    // Touch listener is very special, it contains two kinds of listeners, EventListenerTouchOneByOne and EventListenerTouchAllAtOnce.
                    // return UNKNOWN instead.
                    Debug.Assert(false, "Don't call this method if the event is for touch.");
                    break;
                default:
                    Debug.Assert(false, "Invalid type!");
                    break;
            }
    
            return ret;
        }

        /// <summary>
        /// Adds a event listener for a specified event with the priority of scene graph.
        /// The priority of scene graph will be fixed value 0. So the order of listener item
        /// in the vector will be ' <0, scene graph (0 priority), >0'.
        /// </summary>
        /// <param name="listener">The listener of a specified event.</param>
        /// <param name="node">The priority of the listener is based on the draw order of this node.</param>
        public void AddEventListener(CCEventListener listener, CCNode node)
        {
			Debug.Assert((listener != null && node != null), "Invalid parameters.");
			Debug.Assert(!listener.IsRegistered, "The listener has been registered.");

			if (!listener.IsAvailable)
				return;

			listener.SceneGraphPriority = node;
			listener.FixedPriority = 0;
			listener.IsRegistered = true;

			AddEventListener(listener);

        }

        /// <summary>
        /// Adds a event listener for a specified event with the fixed priority.
        /// A lower priority will be called before the ones that have a higher value.
        /// 0 priority is not allowed for fixed priority since it's used for scene graph based priority.
        /// </summary>
        /// <param name="listener">The listener of a specified event.</param>
        /// <param name="fixedPriority">The fixed priority of the listener.</param>

        public void AddEventListener(CCEventListener listener, int fixedPriority)
        {
			Debug.Assert((listener != null), "Invalid parameters.");
			Debug.Assert(!listener.IsRegistered, "The listener has been registered.");
			Debug.Assert(fixedPriority != 0, "0 priority is forbidden for fixed priority since it's used for scene graph based priority.");

			if (!listener.IsAvailable)
				return;

			listener.SceneGraphPriority = null;
			listener.FixedPriority = fixedPriority;
			listener.IsRegistered = true;
			listener.IsPaused = false;

			AddEventListener(listener);
        }

		bool RemoveListenerInVector (List<CCEventListener> listeners, CCEventListener listener)
		{
			if (listeners == null)
				return false;

			for (int x = 0; x < listeners.Count; x++)
			{
				var l = listeners [x];
				if (l == listener) {
					l.IsRegistered = false;
				
					if (l.SceneGraphPriority != null) {
						DissociateNodeAndEventListener (l.SceneGraphPriority, l);
					}

					if (inDispatch == 0) {
						listeners.Remove (l);
					}

					return true;
				}
			}
			return false;
		}

        /// <summary>
        /// Remove a listener
        /// </summary>
        /// <param name="listener">The specified event listener which needs to be removed.</param>
        public void RemoveEventListener(CCEventListener listener)
        {
			if (listener == null)
				return;

			bool isFound = false;

#if DUMP_LISTENER_ITEM_PRIORITY_INFO
			CCLog.Log("-----  Remove > " + listener + " -----------------------");
#endif

			var listenerIds = listenerMap.Keys;

			foreach (var listenerId in listenerIds)
			{
				var listeners = listenerMap [listenerId];
				//var listeners = iter.Value;
				var fixedPriorityListeners = listeners.FixedPriorityListeners;
				var sceneGraphPriorityListeners = listeners.SceneGraphPriorityListeners;

				isFound = RemoveListenerInVector(sceneGraphPriorityListeners, listener);
				if (isFound)
				{
					// fixed #4160: Dirty flag need to be updated after listeners were removed.
					SetDirty(listener.ListenerID, DirtyFlag.SCENE_GRAPH_PRIORITY);
				}
				else
				{
					isFound = RemoveListenerInVector(fixedPriorityListeners, listener);
					if (isFound)
					{
						SetDirty(listener.ListenerID, DirtyFlag.FIXED_PRIORITY);
					}
				}

				if (listeners.IsEmpty)
				{
					priorityDirtyFlagMap.Remove (listenerId);
					listenerMap.Remove (listenerId);
				}

				if (isFound)
					break;
			}

			if (isFound)
			{
				listener.Dispose ();
			}
			else
			{
				for (int iter = 0; iter < toBeAddedListeners.Count; iter++)
				{
					var l = toBeAddedListeners [iter];

					if (l == listener)
					{
						listener.Dispose ();
						toBeAddedListeners.Remove(l);
						break;
					}
				}
			}
        }

        /// <summary>
        /// Removes all listeners with the same event listener type
        /// </summary>
        /// <param name="listenerType"></param>
        public void RemoveEventListeners(CCEventListenerType listenerType)
        {
			switch (listenerType) 
			{
			case CCEventListenerType.TOUCH_ONE_BY_ONE:
				RemoveEventListeners (CCEventListenerTouchOneByOne.LISTENER_ID);
				break;
			case CCEventListenerType.TOUCH_ALL_AT_ONCE:
				RemoveEventListeners (CCEventListenerTouchAllAtOnce.LISTENER_ID);
				break;
			case CCEventListenerType.MOUSE:
				RemoveEventListeners (CCEventListenerMouse.LISTENER_ID);
				break;
			case CCEventListenerType.ACCELEROMETER:
				RemoveEventListeners (CCEventListenerAccelerometer.LISTENER_ID);
				break;
			case CCEventListenerType.KEYBOARD:
				RemoveEventListeners (CCEventListenerKeyboard.LISTENER_ID);
				break;

			default:
				Debug.Assert (false, "Invalid listener type!");
				break;
			}

        }

        /// <summary>
        /// Removes all listeners which are associated with the specified target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="recursive"></param>
        public void RemoveEventListeners(CCNode target, bool recursive = false)
        {

			if (nodeListenersMap.ContainsKey(target))
            {
				var nodeListeners = nodeListenersMap[target];
				var listenersCopy2 = new CCEventListener[nodeListeners.Count];
				nodeListeners.CopyTo(listenersCopy2);

				for (int x = 0; x < listenersCopy2.Length; x++)
                {
					var listener = listenersCopy2 [x];
                    RemoveEventListener(listener);
                }
            }

            if (recursive)
            {
                var children = target.Children;
				if (children != null) 
				{
					foreach (var child in children) {
						RemoveEventListeners (child, true);
					}
				}
            }
        }

        /// <summary>
        /// Removes all listeners
        /// </summary>
        public void RemoveAll()
        {
			bool cleanMap = true;

//			std::vector<EventListener::ListenerID> types(_listenerMap.size());
//
//			for (const auto& e : _listenerMap)
//			{
//				if (_internalCustomListenerIDs.find(e.first) != _internalCustomListenerIDs.end())
//				{
//					cleanMap = false;
//				}
//				else
//				{
//					types.push_back(e.first);
//				}
//			}
//
//			for (const auto& type : types)
//			{
//				removeEventListenersForListenerID(type);
//			}
//
//			if (!_inDispatch && cleanMap)
//			{
//				_listenerMap.clear();
//			}

        }

        /// <summary>
        /// Pauses all listeners which are associated the specified target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="recursive"></param>
		public void Pause(CCNode target, bool recursive = false)
        {
			if (nodeListenersMap.ContainsKey(target))
			{
				var listeners = nodeListenersMap[target];
                foreach (var listener in listeners)
                {
                    listener.IsPaused = true;
                }
            }

    
            if (recursive)
            {
                var children = target.Children;
                foreach (var child in children)
                {
                    Pause(child, true);
                }
            }
        }

        /// <summary>
        /// Resumes all listeners which are associated the specified target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="recursive"></param>
		public void Resume(CCNode target, bool recursive = false)
        {
			if (nodeListenersMap.ContainsKey(target))
            {
				var listeners = nodeListenersMap[target];
                foreach (var listener in listeners)
                {
                    listener.IsPaused = false;
                }
            }

            MarkDirty = target;
    
            if (recursive)
            {
                var children = target.Children;
                foreach (var child in children)
                {
                    Resume(child, true);
                }
            }
        }

        /// <summary>
        /// Sets listener's priority with fixed value.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="fixedPriority"></param>
        public void SetPriority(CCEventListener listener, int fixedPriority)
        {
			if (listener == null)
				return;

        }

        /// <summary>
        /// Checks or sets whether dispatching events is enabled
        /// </summary>
		public bool IsEnabled { get; set; }

		/// <summary>
		/// Touch event needs to be processed different with other events since it needs support ALL_AT_ONCE and ONE_BY_NONE mode.
		/// </summary>
		/// <param name="touchEvent"></param>
		void DispatchTouchEvent(CCEventTouch touchEvent)
		{
			SortEventListeners(CCEventListenerTouchOneByOne.LISTENER_ID);
			SortEventListeners(CCEventListenerTouchAllAtOnce.LISTENER_ID);

			var oneByOneListeners = GetListeners(CCEventListenerTouchOneByOne.LISTENER_ID);
			var allAtOnceListeners = GetListeners(CCEventListenerTouchAllAtOnce.LISTENER_ID);

			// If there aren't any touch listeners, return directly.
			if (oneByOneListeners == null && allAtOnceListeners == null)
				return;

			bool isNeedsMutableSet = (oneByOneListeners != null && allAtOnceListeners != null);

			var originalTouches = touchEvent.Touches;
			var mutableTouchesArray = new CCTouch[originalTouches.Count];
			originalTouches.CopyTo (mutableTouchesArray);
			var mutableTouchesIter = 0;
			var mutableTouches = mutableTouchesArray.ToList ();

			//
			// process the target handlers 1st
			//
			if (oneByOneListeners != null)
			{

				foreach (var touchesIter in originalTouches)
				{
					bool isSwallowed = false;
					Func<CCEventListener, bool> onTouchEvent = delegate(CCEventListener l)
					{ 
						var listener = (CCEventListenerTouchOneByOne)l;

						// Skip if the listener was removed.
						if (!listener.IsRegistered)
							return false;

						touchEvent.CurrentTarget = listener.SceneGraphPriority;

						bool isClaimed = false;
						var removed = new List<CCTouch>();

						var eventCode = touchEvent.EventCode;

						if (eventCode == CCEventCode.BEGAN)
						{
							if (listener.OnTouchBegan != null)
							{
								isClaimed = listener.OnTouchBegan(touchesIter, touchEvent);
								if (isClaimed && listener.IsRegistered)
								{
									listener.ClaimedTouches.Add(touchesIter);
								}
							}
						}
						else if (listener.ClaimedTouches.Count > 0
							&& (removed = listener.ClaimedTouches.FindAll(t => t == touchesIter)).Count > 0)
						{

							isClaimed = true;

							switch (eventCode)
							{
							case CCEventCode.MOVED:
								if (listener.OnTouchMoved != null)
								{
									listener.OnTouchMoved(touchesIter, touchEvent);
								}
								break;
							case CCEventCode.ENDED:
								if (listener.OnTouchEnded != null)
								{
									listener.OnTouchEnded(touchesIter, touchEvent);
								}
								if (listener.IsRegistered)
								{

									listener.ClaimedTouches.RemoveAll(t => removed.Contains(t));
								}
								break;
							case CCEventCode.CANCELLED:
								if (listener.OnTouchCancelled != null)
								{
									listener.OnTouchCancelled(touchesIter, touchEvent);
								}
								if (listener.IsRegistered)
								{
									listener.ClaimedTouches.RemoveAll(t => removed.Contains(t));
								}
								break;
							default:
								Debug.Assert(false, "The eventcode is invalid.");
								break;
							}
						}

						// If the event was stopped, return directly.
						if (touchEvent.IsStopped)
						{
							UpdateListeners(touchEvent);
							return true;
						}

						Debug.Assert(touchesIter.Id == mutableTouches[mutableTouchesIter].Id, "");

						if (isClaimed && listener.IsRegistered && listener.IsSwallowTouches)
						{
							if (isNeedsMutableSet)
							{
								mutableTouches.RemoveAt(mutableTouchesIter);
								//++mutableTouchesIter;
								isSwallowed = true;
							}
							return true;
						}

						return false;
					};

					//
					DispatchEventToListeners(oneByOneListeners, onTouchEvent);
					if (touchEvent.IsStopped)
					{
						return;
					}

					if (!isSwallowed)
						++mutableTouchesIter;
				}
			}

			//
			// process standard handlers 2nd
			//
			if (allAtOnceListeners != null && mutableTouches.Count > 0)
			{

				Func<CCEventListener, bool> onTouchesEvent = delegate(CCEventListener l)
				{ 

					var listener = (CCEventListenerTouchAllAtOnce)l;

					// Skip if the listener was removed.
					if (!listener.IsRegistered)
						return false;

					touchEvent.CurrentTarget = listener.SceneGraphPriority;

					switch (touchEvent.EventCode)
					{
					case CCEventCode.BEGAN:
						if (listener.OnTouchesBegan != null)
						{
							listener.OnTouchesBegan(mutableTouches, touchEvent);
						}
						break;
					case CCEventCode.MOVED:
						if (listener.OnTouchesMoved != null)
						{
							listener.OnTouchesMoved(mutableTouches, touchEvent);
						}
						break;

					case CCEventCode.ENDED:
						if (listener.OnTouchesEnded != null)
						{
							listener.OnTouchesEnded(mutableTouches, touchEvent);
						}
						break;
					case CCEventCode.CANCELLED:
						if (listener.OnTouchesCancelled != null)
						{
							listener.OnTouchesCancelled(mutableTouches, touchEvent);
						}
						break;
					default:
						Debug.Assert(false, "The eventcode is invalid.");
						break;
					}

					// If the event was stopped, return directly.
					if (touchEvent.IsStopped)
					{
						UpdateListeners(touchEvent);
						return true;
					}

					return false;
				};

				DispatchEventToListeners(allAtOnceListeners, onTouchesEvent);
				if (touchEvent.IsStopped)
				{
					return;
				}
			}

			UpdateListeners(touchEvent);

		}


        /// <summary>
        /// Dispatches the event
        /// Also removes all EventListeners marked for deletion from the event dispatcher list.
        /// </summary>
		/// <param name="eventToDispatch"></param>
        public void DispatchEvent(CCEvent eventToDispatch)
        {
			if (!IsEnabled)
				return;

			UpdateDirtyFlagForSceneGraph();

			inDispatch++;

			if (eventToDispatch.Type == CCEventType.TOUCH)
			{
				DispatchTouchEvent((CCEventTouch)eventToDispatch);
				inDispatch--;
				return;
			}


			var listenerID = CCEventDispatcher.GetListenerID(eventToDispatch);

			SortEventListeners(listenerID);

			if (listenerMap.ContainsKey(listenerID))
			{
				var listeners = listenerMap [listenerID];

				Func<CCEventListener, bool> onEvent = delegate(CCEventListener listener)
				{ 
					eventToDispatch.CurrentTarget = listener.SceneGraphPriority;
					listener.OnEvent(eventToDispatch);
					return eventToDispatch.IsStopped;
				}; 

				DispatchEventToListeners(listeners, onEvent);
			}

			UpdateListeners(eventToDispatch);
			inDispatch--;

        }

        /// <summary>
        /// Sets the dirty flag for a node.
        /// </summary>
        /// <param name="node"></param>
        protected CCNode MarkDirty
        {
            set 
			{ 
				// Mark the node dirty only when there is an eventlistener associated with it. 
				if (nodeListenersMap.ContainsKey(value))
				{
					dirtyNodes.Add(value);
				}
			}
        }

        /// <summary>
        /// Adds an event listener with item
        /// ** Note ** if it is dispatching event, the added operation will be delayed to the end of current dispatch
        /// <see cref=">ForceAddEventListener"/>
        /// </summary>
        /// 
        /// <param name="listener"></param>
        internal void AddEventListener(CCEventListener listener)
        {
            if (inDispatch == 0)
            {
				#if DUMP_LISTENER_ITEM_PRIORITY_INFO
				CCLog.Log("-----  Add > --  Available > " + listener.IsAvailable + " ------{0}------{1}---------", listener, listener.SceneGraphPriority);
				#endif
                ForceAddEventListener(listener);
            }
            else
            {
                toBeAddedListeners.Add(listener);
            }

        }
    
        /// <summary>
        /// Force adding an event listener
        /// ** Note ** force add an event listener which will ignore whether it's in dispatching.
        /// <see cref=">AddEventListener"/>
        /// </summary>
        /// <param name="listener"></param>
        internal void ForceAddEventListener(CCEventListener listener)
        {
			CCEventListenerVector listeners = new CCEventListenerVector();
			var listenerID = listener.ListenerID;
			if (!listenerMap.ContainsKey(listenerID))
			{

				listeners = new CCEventListenerVector();
				listenerMap.Add(listenerID, listeners);
			}
			else
			{
				listeners = listenerMap[listenerID];;
			}

			listeners.PushBack(listener);

			if (listener.FixedPriority == 0)
			{
				SetDirty(listenerID, DirtyFlag.SCENE_GRAPH_PRIORITY);

				var node = listener.SceneGraphPriority;
				Debug.Assert(node != null, "Invalid scene graph priority!");

				AssociateNodeAndEventListener(node, listener);

				if (node.IsRunning)
				{
					Resume(node);
				}
			}
			else
			{
				SetDirty(listenerID, DirtyFlag.FIXED_PRIORITY);
			}
        }

		/// <summary>
		/// Sets the dirty flag for a specified listener ID
		/// </summary>
		/// <param name="listenerID">Listener I.</param>
		/// <param name="flag">Flag.</param>
		void SetDirty(string listenerID, DirtyFlag flag)
		{
			if (!priorityDirtyFlagMap.ContainsKey(listenerID))
			{
				priorityDirtyFlagMap.Add(listenerID, flag);
			}
			else
			{
				DirtyFlag ret = flag | priorityDirtyFlagMap[listenerID];
				priorityDirtyFlagMap[listenerID] = ret;
			}

		}


        /// <summary>
        /// Gets event the listener list for the event listener type.
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        //EventListenerVector* getListeners(const EventListener::ListenerID& listenerID);
    
        /// <summary>
        /// Update dirty flag
        /// </summary>
        void UpdateDirtyFlagForSceneGraph()
        {
			if (dirtyNodes.Count > 0)
			{
				foreach (var node in dirtyNodes)
				{
					if (nodeListenersMap.ContainsKey(node))
					{
						var listeners = nodeListenersMap[node];
						foreach (var l in listeners)
						{
							SetDirty(l.ListenerID, DirtyFlag.SCENE_GRAPH_PRIORITY);
						}
					}
				}

				dirtyNodes.Clear();
			}
		}
        

//		void RemoveAllListenersInVector (List<CCEventListener> listenerVector, string listenerId)
//		{
//			if (listenerVector == null)
//				return;
//
//			for (auto iter = listenerVector->begin(); iter != listenerVector->end();)
//			{
//				auto l = *iter;
//				l->setRegistered(false);
//				if (l->getSceneGraphPriority() != nullptr)
//				{
//					dissociateNodeAndEventListener(l->getSceneGraphPriority(), l);
//				}
//
//				if (_inDispatch == 0)
//				{
//					iter = listenerVector->erase(iter);
//					CC_SAFE_RELEASE(l);
//				}
//				else
//				{
//					++iter;
//				}
//			}
//
//		}

    
        /// <summary>
        /// Removes all listeners with the same event listener ID
        /// </summary>
        /// <param name="?"></param>
        void RemoveEventListeners (string listenerID)
        {
			if (listenerMap.ContainsKey(listenerID))
			{
				var listeners = listenerMap[listenerID];
				var fixedPriorityListeners = listeners.FixedPriorityListeners;
				var sceneGraphPriorityListeners = listeners.SceneGraphPriorityListeners;

				Action<List<CCEventListener>> RemoveAllListenersInVector = (listenerVector) => 
				{
					if (listenerVector == null)
						return;

					for (int x = 0; x < listenerVector.Count; x++)
					{
						var l = listenerVector[x];
						l.IsRegistered = false;
						if (l.SceneGraphPriority != null)
						{
							DissociateNodeAndEventListener(l.SceneGraphPriority, l);
						}

						if (inDispatch == 0)
						{
							listenerVector.Remove(l);
						}
					}
				};

				RemoveAllListenersInVector(sceneGraphPriorityListeners);
				RemoveAllListenersInVector(fixedPriorityListeners);

				// Remove the dirty flag according the 'listenerID'.
				// No need to check whether the dispatcher is dispatching event.
				priorityDirtyFlagMap.Remove(listenerID);

				if (inDispatch == 0)
				{
					listeners.Clear ();
					listenerMap.Remove(listenerID);
				}
			}

			for (int iter = 0; iter < toBeAddedListeners.Count; iter++)
			{
				var l = toBeAddedListeners [iter];

				if (l.ListenerID == listenerID)
				{
					l.Dispose ();
					toBeAddedListeners.Remove(l);
					break;
				}
			}

        }
    
        /// <summary>
        /// Sort event listener
        /// </summary>
        /// <param name="?"></param>
        void SortEventListeners(string listenerID)
        {
			DirtyFlag dirtyFlag = DirtyFlag.NONE;
			bool exists = false;

			if (priorityDirtyFlagMap.ContainsKey(listenerID))
			{
				dirtyFlag = priorityDirtyFlagMap[listenerID];
				exists = true;
			}

			if (dirtyFlag != DirtyFlag.NONE)
			{
				if (dirtyFlag.HasFlag(DirtyFlag.FIXED_PRIORITY))
				{
					SortEventListenersOfFixedPriority(listenerID);
				}

				if (dirtyFlag.HasFlag(DirtyFlag.SCENE_GRAPH_PRIORITY))
				{
					SortEventListenersOfSceneGraphPriority(listenerID);
				}

				if (exists)
					priorityDirtyFlagMap [listenerID] = DirtyFlag.NONE;

			}
        }
    
        /// <summary>
        /// Sorts the listeners of specified type by scene graph priority
        /// </summary>
        /// <param name="?"></param>
        void SortEventListenersOfSceneGraphPriority(string listenerID)
        {
			var listeners = GetListeners(listenerID);

			if (listeners == null)
				return;

			var sceneGraphListeners = listeners.SceneGraphPriorityListeners;

			if (sceneGraphListeners == null)
				return;

			var rootNode = (CCNode)CCDirector.SharedDirector.RunningScene;
			// Reset priority index
			nodePriorityIndex = 0;
			nodePriorityMap.Clear();

			VisitTarget(rootNode, true);

			// After sort: priority < 0, > 0

//			std::sort(sceneGraphListeners->begin(), sceneGraphListeners->end(), [this](const EventListener* l1, const EventListener* l2) {
//				return _nodePriorityMap[l1->getSceneGraphPriority()] > _nodePriorityMap[l2->getSceneGraphPriority()];
//			});
//
				sceneGraphListeners.Sort((a,b) => 
					{
						if (!nodePriorityMap.ContainsKey(a.SceneGraphPriority) && !nodePriorityMap.ContainsKey(b.SceneGraphPriority))
							return 0;
						if (!nodePriorityMap.ContainsKey(a.SceneGraphPriority))
							return 1;
						if (!nodePriorityMap.ContainsKey(b.SceneGraphPriority))
							return -1;

						return nodePriorityMap[a.SceneGraphPriority].CompareTo(nodePriorityMap[b.SceneGraphPriority]) * -1;
					});


#if DUMP_LISTENER_ITEM_PRIORITY_INFO
			CCLog.Log("----------------------- " + nodePriorityMap.Count + " -----------------------");
			foreach (var l in sceneGraphListeners)
			{
				if (nodePriorityMap.ContainsKey(l.SceneGraphPriority))
                    CCLog.Log("listener priority: node ({0}[{1}]), priority {2}, localZ {3}, globalZ {4}", l.SceneGraphPriority, l.SceneGraphPriority.Name, nodePriorityMap[l.SceneGraphPriority], l.SceneGraphPriority.LocalZOrder, l.SceneGraphPriority.GlobalZOrder);
//				else
//                    CCLog.Log("listener priority: node ({0}[{1}]), priority {2}, localZ {3}, globalZ {4}", l.SceneGraphPriority, l.SceneGraphPriority.Name, -1, l.SceneGraphPriority.LocalZOrder, l.SceneGraphPriority.GlobalZOrder);
			}
#endif
        }
    
        /// <summary>
        /// Sorts the listeners of specified type by fixed priority
        /// </summary>
        /// <param name="?"></param>
        void SortEventListenersOfFixedPriority(string listenerID)
        {

        }

		/// <summary>
		/// Gets event the listener list for the event listener type.
		/// </summary>
		/// <returns>The listeners.</returns>
		/// <param name="listenerID">Listener I.</param>
		CCEventListenerVector GetListeners(string listenerID)
		{
			if (listenerMap.ContainsKey(listenerID))
			{
				return listenerMap [listenerID];
			}

			return null;
		}

		/// <summary>
		/// Walks though scene graph to get the draw order for each node, it's called before sorting event listener with scene graph priority
		/// </summary>
		/// <param name="node">Node.</param>
		/// <param name="isRootNode">If set to <c>true</c> is root node.</param>
		void VisitTarget(CCNode node, bool isRootNode)
		{
			if (node == null)
				return;

			int i = 0;
			var children = node.Children;

			var childrenCount = (children == null) ? 0 : children.Count;

			if(childrenCount > 0)
			{
				CCNode child = null;
				// visit children zOrder < 0
				for( ; i < childrenCount; i++ )
				{
					child = children[i];

					if ( child != null && child.LocalZOrder < 0 )
						VisitTarget(child, false);
					else
						break;
				}

				if (nodeListenersMap.ContainsKey(node))
				{
					if (!globalZOrderNodeMap.ContainsKey (node.GlobalZOrder)) 
					{
						globalZOrderNodeMap.Add (node.GlobalZOrder, new List<CCNode> ());
					}
					globalZOrderNodeMap[node.GlobalZOrder].Add(node);
				}

				for( ; i < childrenCount; i++ )
				{
					child = children[i];
					if (child != null)
						VisitTarget(child, false);
				}
			}
			else
			{
				if (nodeListenersMap.ContainsKey(node))
				{
					if (!globalZOrderNodeMap.ContainsKey (node.GlobalZOrder)) 
					{
						globalZOrderNodeMap.Add (node.GlobalZOrder, new List<CCNode> ());
					}
					globalZOrderNodeMap[node.GlobalZOrder].Add(node);

				}
			}

			if (isRootNode)
			{
				List<float> globalZOrders = new List<float> (globalZOrderNodeMap.Count);

				foreach (var e in globalZOrderNodeMap.Keys)
				{
					globalZOrders.Add(e);
				}

				globalZOrders.Sort ();
//				std::sort(globalZOrders.begin(), globalZOrders.end(), [](const float a, const float b){
//					return a < b;
//				});

				foreach (var globalZ in globalZOrders)
				{
					foreach (var n in globalZOrderNodeMap[globalZ])
					{
						if (!nodePriorityMap.ContainsKey(n))
						{
							nodePriorityMap.Add (n, 0);
						}
						nodePriorityMap[n] = ++nodePriorityIndex;
					}
				}

				globalZOrderNodeMap.Clear();
			}


		}


		void UpdateListeners (string listenerID)
		{

			if (!listenerMap.ContainsKey(listenerID))
				return;

			var listeners = listenerMap [listenerID];

			var fixedPriorityListeners = listeners.FixedPriorityListeners;
			var sceneGraphPriorityListeners = listeners.SceneGraphPriorityListeners;

			if (sceneGraphPriorityListeners != null)
			{
				for (int x = 0; x < sceneGraphPriorityListeners.Count; x++)
				{
					var l = sceneGraphPriorityListeners[x];
					if (!l.IsRegistered)
					{
						sceneGraphPriorityListeners.Remove(l);
					}
				}
			}

			if (fixedPriorityListeners != null)
			{
				for (int x = 0; x < fixedPriorityListeners.Count; x++)
				{
					var l = fixedPriorityListeners[x];
					if (!l.IsRegistered)
					{
						fixedPriorityListeners.Remove(l);
					}
				}
			}

			if (sceneGraphPriorityListeners != null && sceneGraphPriorityListeners.Count == 0)
			{
				listeners.ClearSceneGraphListeners();
			}

			if (fixedPriorityListeners != null && fixedPriorityListeners.Count == 0)
			{
				listeners.ClearFixedListeners();
			}
		}

        ///** Updates all listeners
        // *  1) Removes all listener items that have been marked as 'removed' when dispatching event.
        // *  2) Adds all listener items that have been marked as 'added' when dispatching event.
        // */
        void UpdateListeners(CCEvent forEvent)
        {
			Debug.Assert(inDispatch > 0, "If program goes here, there should be events to dispatch.");

			if (forEvent.Type == CCEventType.TOUCH) 
			{
				UpdateListeners (CCEventListenerTouchOneByOne.LISTENER_ID);
				UpdateListeners (CCEventListenerTouchAllAtOnce.LISTENER_ID);
			} 
			else 
			{
				UpdateListeners (CCEventDispatcher.GetListenerID(forEvent));
			}

			if (inDispatch > 1)
				return;

			Debug.Assert (inDispatch == 1, "_inDispatch should be 1 here.");

			for (int lm = 0; lm < listenerMap.Keys.Count; lm++)
			{
				var lv = listenerMap.Keys.ElementAt(lm);
				if (listenerMap[lv].IsEmpty) 
				{
					priorityDirtyFlagMap.Remove (lv);
					listenerMap [lv] = null;
					listenerMap.Remove (lv);
				}
			}

			if (toBeAddedListeners.Count > 0)
			{
				foreach (var listener in toBeAddedListeners)
				{
					ForceAddEventListener(listener);
				}
				toBeAddedListeners.Clear();
			}

        }
    
        /// <summary>
        /// Associates node with event listener
        /// </summary>
        /// <param name="node"></param>
        /// <param name="listener"></param>
        void AssociateNodeAndEventListener(CCNode node, CCEventListener listener)
        {
			List<CCEventListener> listeners = null;
			if (nodeListenersMap.ContainsKey(node)) 
			{
				listeners = nodeListenersMap [node];
			} 
			else 
			{
				listeners = new List<CCEventListener> ();
				nodeListenersMap.Add (node, listeners);
			}
			listeners.Add (listener);
        }
    
        /// <summary>
        /// Dissociates node with event listener
        /// </summary>
        /// <param name="node"></param>
        /// <param name="listener"></param>
        void DissociateNodeAndEventListener(CCNode node, CCEventListener listener)
        {
			List<CCEventListener> listeners = null;
			if (nodeListenersMap.ContainsKey(node)) 
			{
				listeners = nodeListenersMap [node];
				if (listeners.Contains(listener))
					listeners.Remove (listener);

				if (listeners.Count == 0) 
				{
					nodeListenersMap.Remove (node);
				}
			} 
        }
    
        /// <summary>
        /// Dispatches event to listeners with a specified listener type
        /// </summary>
        /// <param name="listeners"></param>
        /// <param name="onEvent"></param>
		void DispatchEventToListeners(CCEventListenerVector listeners,  Func<CCEventListener, bool> onEvent)
		{
			bool shouldStopPropagation = false;
			var fixedPriorityListeners = listeners.FixedPriorityListeners;
			var sceneGraphPriorityListeners = listeners.SceneGraphPriorityListeners;

			int i = 0;
			// priority < 0
			if (fixedPriorityListeners != null)
			{
				Debug.Assert (listeners.Gt0Index <= fixedPriorityListeners.Count, "Out of range exception!");

				if (fixedPriorityListeners.Count == 0)
				{
					for (; i < listeners.Gt0Index; ++i)
					{
						var l = fixedPriorityListeners[i];
						if (!l.IsPaused && l.IsRegistered && onEvent(l))
						{
							shouldStopPropagation = true;
							break;
						}
					}
				}
			}

			if (sceneGraphPriorityListeners != null)
			{
				if (!shouldStopPropagation)
				{
					// priority == 0, scene graph priority
					foreach (var l in sceneGraphPriorityListeners)
					{
						if (!l.IsPaused && l.IsRegistered && onEvent(l))
						{
							shouldStopPropagation = true;
							break;
						}
					}
				}
			}

			if (fixedPriorityListeners != null)
			{
				if (!shouldStopPropagation)
				{
					// priority > 0
					var size = fixedPriorityListeners.Count;
					for (; i < size; ++i)
					{
						var l = fixedPriorityListeners[i];

						if (!l.IsPaused && l.IsRegistered && onEvent(l))
						{
							shouldStopPropagation = true;
							break;
						}
					}
				}
			}

		}

		internal bool IsEventListenersFor ( string listenerId)
		{
			return listenerMap.ContainsKey (listenerId);
		}

		#region CCEventListenerVector class definition
		class CCEventListenerVector
		{

			List<CCEventListener> sceneGraphListeners;
			List<CCEventListener> fixedListeners;
			public int Gt0Index { get; set; }

			public int Size
			{
				get
				{
					int size = 0;
					if (sceneGraphListeners != null)
						size += sceneGraphListeners.Count;
					if (fixedListeners != null)
						size += fixedListeners.Count;

					return size;
				}
			}

			public bool IsEmpty
			{
				get
				{
					return (sceneGraphListeners == null || sceneGraphListeners.Count == 0) 
						&& (fixedListeners == null || fixedListeners.Count == 0);
				}
			}

			public CCEventListenerVector()
			{
				Gt0Index = 0;
			}

			public void PushBack(CCEventListener listener)
			{
				if (listener.FixedPriority == 0)
				{
					if (sceneGraphListeners == null) 
					{
						sceneGraphListeners = new List<CCEventListener> (100);
					}

					sceneGraphListeners.Add(listener);
				}
				else
				{
					if (fixedListeners == null) 
					{
						fixedListeners = new List<CCEventListener> (100);
					}


					fixedListeners.Add(listener);
				}
			}

			public void ClearSceneGraphListeners()
			{
				if (sceneGraphListeners != null) 
				{
					sceneGraphListeners.Clear ();
					sceneGraphListeners = null;
				}
			}

			public void ClearFixedListeners()
			{
				if (fixedListeners != null) 
				{
					fixedListeners.Clear ();
					fixedListeners = null;
				}
			}

			public void Clear()
			{
				ClearSceneGraphListeners();
				ClearFixedListeners();
			}

			public List<CCEventListener> FixedPriorityListeners
			{
				get { return fixedListeners; }
			}

			public List<CCEventListener> SceneGraphPriorityListeners
			{
				get { return sceneGraphListeners; }
			}

		}
		#endregion
    }

}
