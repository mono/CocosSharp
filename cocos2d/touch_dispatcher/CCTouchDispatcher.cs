
using System.Collections.Generic;
using System.Linq;

namespace CocosSharp
{
    public class CCTouchDispatcher : ICCEGLTouchDelegate
    {
		private static List<CCTouch> mutableTouches;

		/// <summary>
		/// Whether or not the events are going to be dispatched. Default: true
		/// </summary>
		public bool IsDispatchEvents { get; set; }

		private bool locked;
		private bool toAdd;
		private bool toQuit;
		private bool toRemove;
		private List<CCTouchHandler> handlersToAdd;
		private List<object> handlersToRemove;
		internal List<CCTouchHandler> StandardHandlers { get; set; }
		internal List<CCTouchHandler> TargetedHandlers { get; set; }

		#region Constructors
		public CCTouchDispatcher()
		{
			IsDispatchEvents = true;
			TargetedHandlers = new List<CCTouchHandler>();
			StandardHandlers = new List<CCTouchHandler>();

			handlersToAdd = new List<CCTouchHandler>();
			handlersToRemove = new List<object>();

			toRemove = false;
			toAdd = false;
			toQuit = false;
			locked = false;

		}
		#endregion

        #region IEGLTouchDelegate Members

        public virtual void TouchesBegan(List<CCTouch> touches)
        {
            if (IsDispatchEvents)
            {
                Touches(touches, (int) CCTouchType.Began);
            }
        }

        public virtual void TouchesMoved(List<CCTouch> touches)
        {
            if (IsDispatchEvents)
            {
                Touches(touches, (int) CCTouchType.Moved);
            }
        }

        public virtual void TouchesEnded(List<CCTouch> touches)
        {
            if (IsDispatchEvents)
            {
                Touches(touches, (int) CCTouchType.Ended);
            }
        }

        public virtual void TouchesCancelled(List<CCTouch> touches)
        {
            if (IsDispatchEvents)
            {
                Touches(touches, (int) CCTouchType.Cancelled);
            }
        }

        #endregion

        /// <summary>
        /// Adds a standard touch delegate to the dispatcher's list.
        /// See StandardTouchDelegate description.
        /// IMPORTANT: The delegate will be retained.
        /// </summary>
		public void AddStandardDelegate(ICCStandardTouchDelegate touchDelegate, int touchPriority)
        {
			CCTouchHandler touchHandler = new CCStandardTouchHandler (touchDelegate, touchPriority);
            if (!locked)
            {
                ForceAddHandler(touchHandler, StandardHandlers);
            }
            else
            {
                handlersToAdd.Add(touchHandler);
                toAdd = true;
            }
        }

        /// <summary>
        /// Adds a targeted touch delegate to the dispatcher's list.
        /// See TargetedTouchDelegate description.
        /// IMPORTANT: The delegate will be retained.
        /// </summary>
        public void AddTargetedDelegate(ICCTargetedTouchDelegate touchDelegate, int touchPriority, bool isSwallowsTouches)
        {
			CCTouchHandler pHandler = new CCTargetedTouchHandler (touchDelegate, touchPriority, isSwallowsTouches);
            if (!locked)
            {
                ForceAddHandler(pHandler, TargetedHandlers);
            }
            else
            {
                handlersToAdd.Add(pHandler);
                toAdd = true;
            }
        }

        /// <summary>
        /// Removes a touch delegate.
        /// The delegate will be released
        /// </summary>
        public void RemoveDelegate(ICCTouchDelegate touchDelegate)
        {
            if (touchDelegate == null)
            {
                return;
            }

            if (!locked)
            {
                ForceRemoveDelegate(touchDelegate);
            }
            else
            {
                handlersToRemove.Add(touchDelegate);
                toRemove = true;
            }
        }

        /// <summary>
        /// Removes all touch delegates, releasing all the delegates
        /// </summary>
        public void RemoveAllDelegates()
        {
            if (!locked)
            {
                ForceRemoveAllDelegates();
            }
            else
            {
                toQuit = true;
            }
        }

        /// <summary>
        /// Changes the priority of a previously added delegate. 
        /// The lower the number, the higher the priority
        /// </summary>
        public void SetPriority(int touchPriority, ICCTouchDelegate touchDelegate)
        {
            CCTouchHandler handler = FindHandler(touchDelegate);
            handler.Priority = touchPriority;

            RearrangeHandlers(TargetedHandlers);
            RearrangeHandlers(StandardHandlers);
        }

        public void Touches(List<CCTouch> touches, int index)
        {
            locked = true;

            // optimization to prevent a mutable copy when it is not necessary
            int uTargetedHandlersCount = TargetedHandlers.Count;
            int uStandardHandlersCount = StandardHandlers.Count;
            bool bNeedsMutableSet = (uTargetedHandlersCount > 0 && uStandardHandlersCount > 0);

            if (bNeedsMutableSet)
            {
                CCTouch[] tempArray = touches.ToArray();
                mutableTouches = tempArray.ToList();
            }
            else
            {
                mutableTouches = touches;
            }

            var sHelper = (CCTouchType) index;

            // process the target handlers 1st
            if (uTargetedHandlersCount > 0)
            {
                #region CCTargetedTouchHandler

                foreach (CCTouch pTouch in touches)
                {
                    foreach (CCTargetedTouchHandler pHandler in TargetedHandlers)
                    {
                        var pDelegate = (ICCTargetedTouchDelegate) (pHandler.Delegate);

                        bool bClaimed = false;
                        if (sHelper == CCTouchType.Began)
                        {
                            bClaimed = pDelegate.TouchBegan(pTouch);

                            if (bClaimed)
                            {
                                pHandler.ClaimedTouches.Add(pTouch);
                            }
                        }
                        else
                        {
                            if (pHandler.ClaimedTouches.Contains(pTouch))
                            {
                                // moved ended cancelled
                                bClaimed = true;

                                switch (sHelper)
                                {
                                    case CCTouchType.Moved:
                                        pDelegate.TouchMoved(pTouch);
                                        break;
                                    case CCTouchType.Ended:
                                        pDelegate.TouchEnded(pTouch);
                                        pHandler.ClaimedTouches.Remove(pTouch);
                                        break;
                                    case CCTouchType.Cancelled:
                                        pDelegate.TouchCancelled(pTouch);
                                        pHandler.ClaimedTouches.Remove(pTouch);
                                        break;
                                }
                            }
                        }

                        if (bClaimed && pHandler.IsSwallowsTouches)
                        {
                            if (bNeedsMutableSet)
                            {
                                mutableTouches.Remove(pTouch);
                            }

                            break;
                        }
                    }
                }

                #endregion
            }

            // process standard handlers 2nd
            if (uStandardHandlersCount > 0 && mutableTouches.Count > 0)
            {
                #region CCStandardTouchHandler

                foreach (CCStandardTouchHandler pHandler in StandardHandlers)
                {
                    var pDelegate = (ICCStandardTouchDelegate) pHandler.Delegate;
                    switch (sHelper)
                    {
                        case CCTouchType.Began:
                            pDelegate.TouchesBegan(mutableTouches);
                            break;
                        case CCTouchType.Moved:
                            pDelegate.TouchesMoved(mutableTouches);
                            break;
                        case CCTouchType.Ended:
                            pDelegate.TouchesEnded(mutableTouches);
                            break;
                        case CCTouchType.Cancelled:
                            pDelegate.TouchesCancelled(mutableTouches);
                            break;
                    }
                }

                #endregion
            }

            if (bNeedsMutableSet)
            {
                mutableTouches = null;
            }

            //
            // Optimization. To prevent a [handlers copy] which is expensive
            // the add/removes/quit is done after the iterations
            //
            locked = false;
            if (toRemove)
            {
                toRemove = false;
                for (int i = 0; i < handlersToRemove.Count; ++i)
                {
                    ForceRemoveDelegate((ICCTouchDelegate) handlersToRemove[i]);
                }
                handlersToRemove.Clear();
            }

            if (toAdd)
            {
                toAdd = false;
                foreach (CCTouchHandler pHandler in handlersToAdd)
                {
                    if (pHandler is CCTargetedTouchHandler && pHandler.Delegate is ICCTargetedTouchDelegate)
                    {
                        ForceAddHandler(pHandler, TargetedHandlers);
                    }
                    else if (pHandler is CCStandardTouchHandler && pHandler.Delegate is ICCStandardTouchDelegate)
                    {
                        ForceAddHandler(pHandler, StandardHandlers);
                    }
                    else
                    {
                        CCLog.Log("ERROR: inconsistent touch handler and delegate found in m_pHandlersToAdd of CCTouchDispatcher");
                    }
                }

                handlersToAdd.Clear();
            }

            if (toQuit)
            {
                toQuit = false;
                ForceRemoveAllDelegates();
            }
        }

        public CCTouchHandler FindHandler(ICCTouchDelegate touchDelegate)
        {
            foreach (CCTouchHandler handler in TargetedHandlers)
            {
                if (handler.Delegate == touchDelegate)
                {
                    return handler;
                }
            }

            foreach (CCTouchHandler handler in StandardHandlers)
            {
                if (handler.Delegate == touchDelegate)
                {
                    return handler;
                }
            }

            return null;
        }

        protected void ForceRemoveDelegate(ICCTouchDelegate touchDelegate)
        {
            // remove handler from m_pStandardHandlers
            foreach (CCTouchHandler pHandler in StandardHandlers)
            {
                if (pHandler != null && pHandler.Delegate == touchDelegate)
                {
                    StandardHandlers.Remove(pHandler);
                    break;
                }
            }

            // remove handler from m_pTargetedHandlers
            foreach (CCTouchHandler pHandler in TargetedHandlers)
            {
                if (pHandler != null && pHandler.Delegate == touchDelegate)
                {
                    TargetedHandlers.Remove(pHandler);
                    break;
                }
            }
        }

        protected void ForceAddHandler(CCTouchHandler touchHandler, List<CCTouchHandler> touchHandlers)
        {
            int u = 0;
            for (int i = 0; i < touchHandlers.Count; i++)
            {
                CCTouchHandler h = touchHandlers[i];

                if (h != null)
                {
                    if (h.Priority < touchHandler.Priority)
                    {
                        ++u;
                    }

                    if (h.Delegate == touchHandler.Delegate)
                    {
                        return;
                    }
                }
            }

            touchHandlers.Insert(u, touchHandler);
        }

        protected void ForceRemoveAllDelegates()
        {
            StandardHandlers.Clear();
            TargetedHandlers.Clear();
        }

        protected void RearrangeHandlers(List<CCTouchHandler> touchHandlers)
        {
            touchHandlers.Sort(Less);
        }

        /// <summary>
        /// Used for sort
        /// </summary>
		private int Less(CCTouchHandler handler1, CCTouchHandler handler2)
        {
			return handler1.Priority - handler2.Priority;
        }
    }

    public enum CCTouchType
    {
        Began = 0,
        Moved = 1,
        Ended = 2,
        Cancelled = 3,
        TouchMax = 4
    }
}