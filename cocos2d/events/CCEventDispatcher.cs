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
    enum DirtyFlag
    {
        NONE = 0,
        FIXED_PRIORITY = 0x01,
        SCENE_GRAPH_PRIORITY = 0x02,
        ALL = FIXED_PRIORITY | SCENE_GRAPH_PRIORITY
    }

    public class CCEventDispatcher
    {

        CCEventListenerVector eventListeners;

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
            eventListeners = new CCEventListenerVector();
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
                //case CCEventType.ACCELERATION:
                //    ret = CCEventListenerAcceleration.LISTENER_ID;
                //    break;
                //case CCEventType.CUSTOM:
                //    {
                //        auto customEvent = static_cast<EventCustom*>(event);
                //        ret = customEvent->getEventName();
                //    }
                //    break;
                //case CCEventType.KEYBOARD:
                //    ret = CCEventListenerKeyboard.LISTENER_ID;
                //    break;
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

        }

        /// <summary>
        /// Remove a listener
        /// </summary>
        /// <param name="listener">The specified event listener which needs to be removed.</param>
        public void RemoveEventListener(CCEventListener listener)
        {

        }

        /// <summary>
        /// Removes all listeners with the same event listener type
        /// </summary>
        /// <param name="listenerType"></param>
        public void RemoveEventListener(CCEventListenerType listenerType)
        {
        }

        /// <summary>
        /// Removes all listeners which are associated with the specified target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="recursive"></param>
        public void RemoveEventListener(CCNode target, bool recursive)// = false)
        {
            var listeners = nodeListenersMap[target];
            if (listeners != null)
            {
                foreach (var listener in listeners)
                {
                    RemoveEventListener(listener);
                }
            }

            if (recursive)
            {
                var children = target.Children;
                foreach (var child in children)
                {
                    RemoveEventListenersForTarget(child, true);
                }
            }
        }

        /// <summary>
        /// Removes all listeners
        /// </summary>
        public void RemoveAll()
        {

        }

        /// <summary>
        /// Pauses all listeners which are associated the specified target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="recursive"></param>
        public void PauseEventListenersForTarget(CCNode target, bool recursive) // = false)
        {
            var listeners = nodeListenersMap[target];
            if (listeners != null)
            {
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
                    PauseEventListenersForTarget(child, true);
                }
            }
        }

        /// <summary>
        /// Resumes all listeners which are associated the specified target.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="recursive"></param>
        public void ResumeEventListenersForTarget(CCNode target, bool recursive) // = false)
        {
            var listeners = nodeListenersMap[target];
            if (listeners != null)
            {
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
                    ResumeEventListenersForTarget(child, true);
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

        }

        /// <summary>
        /// Checks or sets whether dispatching events is enabled
        /// </summary>
        public bool IsEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Dispatches the event
        /// Also removes all EventListeners marked for deletion from the event dispatcher list.
        /// </summary>
        /// <param name="dispatchEvent"></param>
  
        public void DispatchEvent(CCEvent dispatchEvent)
        {

        }

        /// <summary>
        /// Sets the dirty flag for a node.
        /// </summary>
        /// <param name="node"></param>
        protected CCNode MarkDirty
        {
            set { }
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

        }
    
        /// <summary>
        /// Removes all listeners with the same event listener ID
        /// </summary>
        /// <param name="?"></param>
        void RemoveEventListeners (string listenerID)
        {

        }
    
        /// <summary>
        /// Sort event listener
        /// </summary>
        /// <param name="?"></param>
        void SortEventListeners(string listenerID)
        {

        }
    
        /// <summary>
        /// Sorts the listeners of specified type by scene graph priority
        /// </summary>
        /// <param name="?"></param>
        void SortEventListenersOfSceneGraphPriority(string listenerID)
        {

        }
    
        /// <summary>
        /// Sorts the listeners of specified type by fixed priority
        /// </summary>
        /// <param name="?"></param>
        void SortEventListenersOfFixedPriority(string listenerID)
        {

        }
    
        ///** Updates all listeners
        // *  1) Removes all listener items that have been marked as 'removed' when dispatching event.
        // *  2) Adds all listener items that have been marked as 'added' when dispatching event.
        // */
        void UpdateListeners(CCEvent forEvent)
        {

        }

        /// <summary>
        /// Touch event needs to be processed different with other events since it needs support ALL_AT_ONCE and ONE_BY_NONE mode.
        /// </summary>
        /// <param name="touchEvent"></param>
        //void DispatchTouchEvent(CCEventTouch touchEvent);
    
        /// <summary>
        /// Associates node with event listener
        /// </summary>
        /// <param name="node"></param>
        /// <param name="listener"></param>
        void AssociateNodeAndEventListener(CCNode node, CCEventListener listener)
        {

        }
    
        /// <summary>
        /// Dissociates node with event listener
        /// </summary>
        /// <param name="node"></param>
        /// <param name="listener"></param>
        void DissociateNodeAndEventListener(CCNode node, CCEventListener listener)
        {

        }
    
        /// <summary>
        /// Dispatches event to listeners with a specified listener type
        /// </summary>
        /// <param name="listeners"></param>
        /// <param name="onEvent"></param>
        //void DispatchEventToListeners(EventListenerVector listeners,  Func<Action<CCEventListener>, bool> onEvent);

    }
}
