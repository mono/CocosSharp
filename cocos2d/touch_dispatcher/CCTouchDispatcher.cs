
using System.Collections.Generic;
using System.Linq;

namespace CocosSharp
{
    public class CCTouchDispatcher : ICCEGLTouchDelegate
    {
        private static List<CCTouch> pMutableTouches;
        private bool m_bDispatchEvents;
        private bool m_bLocked;
        private bool m_bToAdd;
        private bool m_bToQuit;
        private bool m_bToRemove;
        private List<CCTouchHandler> m_pHandlersToAdd;
        private List<object> m_pHandlersToRemove;
        protected List<CCTouchHandler> m_pStandardHandlers;
        protected List<CCTouchHandler> m_pTargetedHandlers;

        /// <summary>
        /// Whether or not the events are going to be dispatched. Default: true
        /// </summary>
        public bool IsDispatchEvents
        {
            get { return m_bDispatchEvents; }
            set { m_bDispatchEvents = value; }
        }

        #region IEGLTouchDelegate Members

        public virtual void TouchesBegan(List<CCTouch> touches)
        {
            if (m_bDispatchEvents)
            {
                Touches(touches, (int) CCTouchType.Began);
            }
        }

        public virtual void TouchesMoved(List<CCTouch> touches)
        {
            if (m_bDispatchEvents)
            {
                Touches(touches, (int) CCTouchType.Moved);
            }
        }

        public virtual void TouchesEnded(List<CCTouch> touches)
        {
            if (m_bDispatchEvents)
            {
                Touches(touches, (int) CCTouchType.Ended);
            }
        }

        public virtual void TouchesCancelled(List<CCTouch> touches)
        {
            if (m_bDispatchEvents)
            {
                Touches(touches, (int) CCTouchType.Cancelled);
            }
        }

        #endregion

        public bool Init()
        {
            m_bDispatchEvents = true;
            m_pTargetedHandlers = new List<CCTouchHandler>();
            m_pStandardHandlers = new List<CCTouchHandler>();

            m_pHandlersToAdd = new List<CCTouchHandler>();
            m_pHandlersToRemove = new List<object>();

            m_bToRemove = false;
            m_bToAdd = false;
            m_bToQuit = false;
            m_bLocked = false;

            return true;
        }

        /// <summary>
        /// Adds a standard touch delegate to the dispatcher's list.
        /// See StandardTouchDelegate description.
        /// IMPORTANT: The delegate will be retained.
        /// </summary>
        public void AddStandardDelegate(ICCStandardTouchDelegate pDelegate, int nPriority)
        {
            CCTouchHandler pHandler = CCStandardTouchHandler.HandlerWithDelegate(pDelegate, nPriority);
            if (!m_bLocked)
            {
                ForceAddHandler(pHandler, m_pStandardHandlers);
            }
            else
            {
                m_pHandlersToAdd.Add(pHandler);
                m_bToAdd = true;
            }
        }

        /// <summary>
        /// Adds a targeted touch delegate to the dispatcher's list.
        /// See TargetedTouchDelegate description.
        /// IMPORTANT: The delegate will be retained.
        /// </summary>
        public void AddTargetedDelegate(ICCTargetedTouchDelegate pDelegate, int nPriority, bool bSwallowsTouches)
        {
            CCTouchHandler pHandler = CCTargetedTouchHandler.HandlerWithDelegate(pDelegate, nPriority, bSwallowsTouches);
            if (!m_bLocked)
            {
                ForceAddHandler(pHandler, m_pTargetedHandlers);
            }
            else
            {
                m_pHandlersToAdd.Add(pHandler);
                m_bToAdd = true;
            }
        }

        /// <summary>
        /// Removes a touch delegate.
        /// The delegate will be released
        /// </summary>
        public void RemoveDelegate(ICCTouchDelegate pDelegate)
        {
            if (pDelegate == null)
            {
                return;
            }

            if (!m_bLocked)
            {
                ForceRemoveDelegate(pDelegate);
            }
            else
            {
                m_pHandlersToRemove.Add(pDelegate);
                m_bToRemove = true;
            }
        }

        /// <summary>
        /// Removes all touch delegates, releasing all the delegates
        /// </summary>
        public void RemoveAllDelegates()
        {
            if (!m_bLocked)
            {
                ForceRemoveAllDelegates();
            }
            else
            {
                m_bToQuit = true;
            }
        }

        /// <summary>
        /// Changes the priority of a previously added delegate. 
        /// The lower the number, the higher the priority
        /// </summary>
        public void SetPriority(int nPriority, ICCTouchDelegate pDelegate)
        {
            CCTouchHandler handler = FindHandler(pDelegate);
            handler.Priority = nPriority;

            RearrangeHandlers(m_pTargetedHandlers);
            RearrangeHandlers(m_pStandardHandlers);
        }

        public void Touches(List<CCTouch> pTouches, int uIndex)
        {
            m_bLocked = true;

            // optimization to prevent a mutable copy when it is not necessary
            int uTargetedHandlersCount = m_pTargetedHandlers.Count;
            int uStandardHandlersCount = m_pStandardHandlers.Count;
            bool bNeedsMutableSet = (uTargetedHandlersCount > 0 && uStandardHandlersCount > 0);

            if (bNeedsMutableSet)
            {
                CCTouch[] tempArray = pTouches.ToArray();
                pMutableTouches = tempArray.ToList();
            }
            else
            {
                pMutableTouches = pTouches;
            }

            var sHelper = (CCTouchType) uIndex;

            // process the target handlers 1st
            if (uTargetedHandlersCount > 0)
            {
                #region CCTargetedTouchHandler

                foreach (CCTouch pTouch in pTouches)
                {
                    foreach (CCTargetedTouchHandler pHandler in m_pTargetedHandlers)
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
                                pMutableTouches.Remove(pTouch);
                            }

                            break;
                        }
                    }
                }

                #endregion
            }

            // process standard handlers 2nd
            if (uStandardHandlersCount > 0 && pMutableTouches.Count > 0)
            {
                #region CCStandardTouchHandler

                foreach (CCStandardTouchHandler pHandler in m_pStandardHandlers)
                {
                    var pDelegate = (ICCStandardTouchDelegate) pHandler.Delegate;
                    switch (sHelper)
                    {
                        case CCTouchType.Began:
                            pDelegate.TouchesBegan(pMutableTouches);
                            break;
                        case CCTouchType.Moved:
                            pDelegate.TouchesMoved(pMutableTouches);
                            break;
                        case CCTouchType.Ended:
                            pDelegate.TouchesEnded(pMutableTouches);
                            break;
                        case CCTouchType.Cancelled:
                            pDelegate.TouchesCancelled(pMutableTouches);
                            break;
                    }
                }

                #endregion
            }

            if (bNeedsMutableSet)
            {
                pMutableTouches = null;
            }

            //
            // Optimization. To prevent a [handlers copy] which is expensive
            // the add/removes/quit is done after the iterations
            //
            m_bLocked = false;
            if (m_bToRemove)
            {
                m_bToRemove = false;
                for (int i = 0; i < m_pHandlersToRemove.Count; ++i)
                {
                    ForceRemoveDelegate((ICCTouchDelegate) m_pHandlersToRemove[i]);
                }
                m_pHandlersToRemove.Clear();
            }

            if (m_bToAdd)
            {
                m_bToAdd = false;
                foreach (CCTouchHandler pHandler in m_pHandlersToAdd)
                {
                    if (pHandler is CCTargetedTouchHandler && pHandler.Delegate is ICCTargetedTouchDelegate)
                    {
                        ForceAddHandler(pHandler, m_pTargetedHandlers);
                    }
                    else if (pHandler is CCStandardTouchHandler && pHandler.Delegate is ICCStandardTouchDelegate)
                    {
                        ForceAddHandler(pHandler, m_pStandardHandlers);
                    }
                    else
                    {
                        CCLog.Log("ERROR: inconsistent touch handler and delegate found in m_pHandlersToAdd of CCTouchDispatcher");
                    }
                }

                m_pHandlersToAdd.Clear();
            }

            if (m_bToQuit)
            {
                m_bToQuit = false;
                ForceRemoveAllDelegates();
            }
        }

        public CCTouchHandler FindHandler(ICCTouchDelegate pDelegate)
        {
            foreach (CCTouchHandler handler in m_pTargetedHandlers)
            {
                if (handler.Delegate == pDelegate)
                {
                    return handler;
                }
            }

            foreach (CCTouchHandler handler in m_pStandardHandlers)
            {
                if (handler.Delegate == pDelegate)
                {
                    return handler;
                }
            }

            return null;
        }

        protected void ForceRemoveDelegate(ICCTouchDelegate pDelegate)
        {
            // remove handler from m_pStandardHandlers
            foreach (CCTouchHandler pHandler in m_pStandardHandlers)
            {
                if (pHandler != null && pHandler.Delegate == pDelegate)
                {
                    m_pStandardHandlers.Remove(pHandler);
                    break;
                }
            }

            // remove handler from m_pTargetedHandlers
            foreach (CCTouchHandler pHandler in m_pTargetedHandlers)
            {
                if (pHandler != null && pHandler.Delegate == pDelegate)
                {
                    m_pTargetedHandlers.Remove(pHandler);
                    break;
                }
            }
        }

        protected void ForceAddHandler(CCTouchHandler pHandler, List<CCTouchHandler> pArray)
        {
            int u = 0;
            for (int i = 0; i < pArray.Count; i++)
            {
                CCTouchHandler h = pArray[i];

                if (h != null)
                {
                    if (h.Priority < pHandler.Priority)
                    {
                        ++u;
                    }

                    if (h.Delegate == pHandler.Delegate)
                    {
                        return;
                    }
                }
            }

            pArray.Insert(u, pHandler);
        }

        protected void ForceRemoveAllDelegates()
        {
            m_pStandardHandlers.Clear();
            m_pTargetedHandlers.Clear();
        }

        protected void RearrangeHandlers(List<CCTouchHandler> pArray)
        {
            pArray.Sort(Less);
        }

        /// <summary>
        /// Used for sort
        /// </summary>
        private int Less(CCTouchHandler p1, CCTouchHandler p2)
        {
            return p1.Priority - p2.Priority;
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