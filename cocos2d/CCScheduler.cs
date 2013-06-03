using System.Collections.Generic;
using System.Diagnostics;
using Cocos2D;

namespace Cocos2D
{
    //
    // CCTimer
    //
    /** @brief Light weight timer */

    public class CCTimer : CCSelectorProtocol
    {
        private CCScheduler _scheduler;
        private readonly CCSelectorProtocol Target;

        private readonly bool m_bRunForever;
        private readonly float m_fDelay;
        private readonly uint m_nRepeat; //0 = once, 1 is 2 x executed
        private float Elapsed;
        public float OriginalInterval;
        public float Interval;
        public SEL_SCHEDULE Selector;
        private bool m_bUseDelay;

        //private int m_nScriptHandler;
        private uint m_nTimesExecuted;

        public CCTimer()
        {
        }

        /** Initializes a timer with a target and a selector. 
         */

        public CCTimer(CCScheduler scheduler, CCSelectorProtocol target, SEL_SCHEDULE selector)
            : this(scheduler, target, selector, 0, 0, 0)
        {
        }

        /** Initializes a timer with a target, a selector and an interval in seconds. 
         *  Target is not needed in c#, it is just for compatibility.
         */

        public CCTimer(CCScheduler scheduler, CCSelectorProtocol target, SEL_SCHEDULE selector, float seconds)
            : this(scheduler, target, selector, seconds, 0, 0)
        {
        }

        public CCTimer(CCScheduler scheduler, CCSelectorProtocol target, SEL_SCHEDULE selector, float seconds, uint repeat, float delay)
        {
            _scheduler = scheduler;
            Target = target;
            Selector = selector;
            Elapsed = -1;
            OriginalInterval = seconds;
            Interval = seconds;
            m_fDelay = delay;
            m_bUseDelay = delay > 0f;
            m_nRepeat = repeat;
            m_bRunForever = m_nRepeat == uint.MaxValue;
        }

        /*
        public CCTimer(int scriptHandler, float seconds)
        {
            m_nScriptHandler = scriptHandler;
            Elapsed = -1;
            Interval = seconds;
        }
        */

        #region SelectorProtocol Members

        public void Update(float dt)
        {
            if (Elapsed == -1)
            {
                Elapsed = 0;
                m_nTimesExecuted = 0;
            }
            else
            {
                if (m_bRunForever && !m_bUseDelay)
                {
                    //standard timer usage
                    Elapsed += dt;
                    if (Elapsed >= Interval)
                    {
                        if (Selector != null)
                        {
                            Selector(Elapsed);
                        }

                        /*
                        if (m_nScriptHandler != 0)
                        {
                            CCScriptEngineManager::sharedManager()->getScriptEngine()->executeSchedule(this, m_fElapsed);
                        }
                        */
                        Interval = OriginalInterval - (Elapsed - Interval);
                    }
                }
                else
                {
                    //advanced usage
                    Elapsed += dt;
                    
                    if (m_bUseDelay)
                    {
                        if (Elapsed >= m_fDelay)
                        {
                            if (Selector != null)
                            {
                                Selector(Elapsed);
                            }

                            /*
                            if (m_nScriptHandler != 0)
                            {
                                CCScriptEngineManager::sharedManager()->getScriptEngine()->executeSchedule(this, m_fElapsed);
                            }
                            */

                            Elapsed = Elapsed - m_fDelay;
                            m_nTimesExecuted += 1;
                            m_bUseDelay = false;
                        }
                    }
                    else
                    {
                        if (Elapsed >= Interval)
                        {
                            if (Selector != null)
                            {
                                Selector(Elapsed);
                            }

                            /*
                            if (m_nScriptHandler)
                            {
                                CCScriptEngineManager::sharedManager()->getScriptEngine()->executeSchedule(m_nScriptHandler, m_fElapsed);
                            }
                            */

                            Interval = OriginalInterval - (Elapsed - Interval); 
                            Elapsed = 0;
                            m_nTimesExecuted += 1;
                        }
                    }

                    if (m_nTimesExecuted > m_nRepeat)
                    {
                        //unschedule timer
                        _scheduler.UnscheduleSelector(Selector, Target);
                    }
                }
            }
        }

        #endregion
    }
}

/** @brief Scheduler is responsible for triggering the scheduled callbacks.
    You should not use NSTimer. Instead use this class.

    There are 2 different types of callbacks (selectors):

    - update selector: the 'update' selector will be called every frame. You can customize the priority.
    - custom selector: A custom selector will be called every frame, or with a custom interval of time

    The 'custom selectors' should be avoided when possible. It is faster, and consumes less memory to use the 'update selector'.
    */

public class CCScheduler
{
    public const uint kCCRepeatForever = uint.MaxValue - 1;
    public const int kCCPrioritySystem = int.MinValue;
    public const int kCCPriorityNonSystemMin = kCCPrioritySystem + 1;

    private readonly Dictionary<CCSelectorProtocol, HashSelectorEntry> m_pHashForSelectors =
        new Dictionary<CCSelectorProtocol, HashSelectorEntry>();

    private readonly Dictionary<CCSelectorProtocol, HashUpdateEntry> m_pHashForUpdates =
        new Dictionary<CCSelectorProtocol, HashUpdateEntry>();

    // hash used to fetch quickly the list entries for pause,delete,etc
    private readonly LinkedList<ListEntry> m_pUpdates0List = new LinkedList<ListEntry>(); // list priority == 0
    private readonly LinkedList<ListEntry> m_pUpdatesNegList = new LinkedList<ListEntry>(); // list of priority < 0
    private readonly LinkedList<ListEntry> m_pUpdatesPosList = new LinkedList<ListEntry>(); // list priority > 0

    private HashSelectorEntry m_pCurrentTarget;
    private bool m_bCurrentTargetSalvaged;
    private bool m_bUpdateHashLocked;

    public float TimeScale = 1.0f;

    private static HashSelectorEntry[] s_pTmpHashSelectorArray = new HashSelectorEntry[128];
    private static CCSelectorProtocol[] s_pTmpSelectorArray = new CCSelectorProtocol[128];

    internal void update(float dt)
    {
        m_bUpdateHashLocked = true;

        try
        {
        if (TimeScale != 1.0f)
        {
            dt *= TimeScale;
        }

        LinkedListNode<ListEntry> next;

        // updates with priority < 0
        //foreach (ListEntry entry in _updatesNegList)
        for (LinkedListNode<ListEntry> node = m_pUpdatesNegList.First; node != null; node = next)
        {
            next = node.Next;
            if (!node.Value.Paused && !node.Value.MarkedForDeletion)
            {
                node.Value.Target.Update(dt);
            }
        }

        // updates with priority == 0
        //foreach (ListEntry entry in _updates0List)
        for (LinkedListNode<ListEntry> node = m_pUpdates0List.First; node != null; node = next)
        {
            next = node.Next;
            if (!node.Value.Paused && !node.Value.MarkedForDeletion)
            {
                node.Value.Target.Update(dt);
            }
        }

        // updates with priority > 0
        for (LinkedListNode<ListEntry> node = m_pUpdatesPosList.First; node != null; node = next)
        {
            next = node.Next;
            if (!node.Value.Paused && !node.Value.MarkedForDeletion)
            {
                node.Value.Target.Update(dt);
            }
        }

        // Iterate over all the custom selectors
        var count = m_pHashForSelectors.Keys.Count;
        if (s_pTmpSelectorArray.Length < count)
        {
            s_pTmpSelectorArray = new CCSelectorProtocol[s_pTmpSelectorArray.Length * 2];
        }
            m_pHashForSelectors.Keys.CopyTo(s_pTmpSelectorArray, 0);

            for (int i = 0; i < count; i++)
            {
                CCSelectorProtocol key = s_pTmpSelectorArray[i];
                if (!m_pHashForSelectors.ContainsKey(key))
                {
                    continue;
                }
                HashSelectorEntry elt = m_pHashForSelectors[key];

                m_pCurrentTarget = elt;
                m_bCurrentTargetSalvaged = false;

                if (!m_pCurrentTarget.Paused)
                {
                    // The 'timers' array may change while inside this loop
                    for (elt.TimerIndex = 0; elt.TimerIndex < elt.Timers.Count; ++elt.TimerIndex)
                    {
                        elt.CurrentTimer = elt.Timers[elt.TimerIndex];
                        elt.CurrentTimerSalvaged = false;

                        elt.CurrentTimer.Update(dt);

                        elt.CurrentTimer = null;
                    }
                }

                // only delete currentTarget if no actions were scheduled during the cycle (issue #481)
                if (m_bCurrentTargetSalvaged && m_pCurrentTarget.Timers.Count == 0)
                {
                    RemoveHashElement(m_pCurrentTarget);
                }
            }
        /*
            // Iterate over all the script callbacks
            if (m_pScriptHandlerEntries)
            {
                for (int i = m_pScriptHandlerEntries->count() - 1; i >= 0; i--)
                {
                    CCSchedulerScriptHandlerEntry* pEntry = static_cast<CCSchedulerScriptHandlerEntry*>(m_pScriptHandlerEntries->objectAtIndex(i));
                    if (pEntry->isMarkedForDeletion())
                    {
                        m_pScriptHandlerEntries->removeObjectAtIndex(i);
                    }
                    else if (!pEntry->isPaused())
                    {
                        pEntry->getTimer()->update(dt);
                    }
                }
            }             
            */

        // delete all updates that are marked for deletion
        // updates with priority < 0
        for (LinkedListNode<ListEntry> node = m_pUpdatesNegList.First; node != null; node = next)
        {
            next = node.Next;
            if (node.Value.MarkedForDeletion)
            {
                m_pUpdatesNegList.Remove(node);
                RemoveUpdateFromHash(node.Value);
            }
        }

        // updates with priority == 0
        for (LinkedListNode<ListEntry> node = m_pUpdates0List.First; node != null; node = next)
        {
            next = node.Next;
            if (node.Value.MarkedForDeletion)
            {
                m_pUpdates0List.Remove(node);
                RemoveUpdateFromHash(node.Value);
            }
        }

        // updates with priority > 0
        for (LinkedListNode<ListEntry> node = m_pUpdatesPosList.First; node != null; node = next)
        {
            next = node.Next;
            if (node.Value.MarkedForDeletion)
            {
                m_pUpdatesPosList.Remove(node);
                RemoveUpdateFromHash(node.Value);
            }
        }
        }
        finally
        {
            // Always do this just in case there is a problem

        m_bUpdateHashLocked = false;
        m_pCurrentTarget = null;
        }
    }

    /** The scheduled method will be called every 'interval' seconds.
     If paused is YES, then it won't be called until it is resumed.
     If 'interval' is 0, it will be called every frame, but if so, it's recommended to use 'scheduleUpdateForTarget:' instead.
     If the selector is already scheduled, then only the interval parameter will be updated without re-scheduling it again.
     repeat let the action be repeated repeat + 1 times, use kCCRepeatForever to let the action run continuously
     delay is the amount of time the action will wait before it'll start

     @since v0.99.3, repeat and delay added in v1.1
     */
    public void ScheduleSelector(SEL_SCHEDULE selector, CCSelectorProtocol target, float interval, bool paused,
                                        uint repeat,
                                        float delay)
    {
        Debug.Assert(selector != null);
        Debug.Assert(target != null);

        HashSelectorEntry element;

        if (!m_pHashForSelectors.TryGetValue(target, out element))
        {
            element = new HashSelectorEntry { Target = target };
            m_pHashForSelectors[target] = element;

            // Is this the 1st element ? Then set the pause level to all the selectors of this target
            element.Paused = paused;
        }
        else
        {
            Debug.Assert(element.Paused == paused);
        }

        if (element.Timers == null)
        {
            element.Timers = new List<CCTimer>();
        }
        else
        {
            foreach (var timer in element.Timers)
            {
                if (selector == timer.Selector)
                {
                    Debug.WriteLine("CCSheduler#scheduleSelector. Selector already scheduled. Updating interval from: {0} to {1}", timer.Interval, interval);
                    timer.Interval = interval;
                    return;
                }
            }
        }

        element.Timers.Add(new CCTimer(this, target, selector, interval, repeat, delay));
    }

    /** Schedules the 'update' selector for a given target with a given priority.
	     The 'update' selector will be called every frame.
	     The lower the priority, the earlier it is called.
	     @since v0.99.3
	     */

    public void ScheduleUpdateForTarget(CCSelectorProtocol targt, int priority, bool paused)
    {
        HashUpdateEntry element;

        if (m_pHashForUpdates.TryGetValue(targt, out element))
        {
            Debug.Assert(element.Entry.MarkedForDeletion);

            // TODO: check if priority has changed!
            element.Entry.MarkedForDeletion = false;

            return;
        }

        // most of the updates are going to be 0, that's way there
        // is an special list for updates with priority 0
        if (priority == 0)
        {
            AppendIn(m_pUpdates0List, targt, paused);
        }
        else if (priority < 0)
        {
            PriorityIn(m_pUpdatesNegList, targt, priority, paused);
        }
        else
        {
            PriorityIn(m_pUpdatesPosList, targt, priority, paused);
        }
    }

    /** Unschedule a selector for a given target.
	     If you want to unschedule the "update", use unscheudleUpdateForTarget.
	     @since v0.99.3
	     */

    public void UnscheduleSelector(SEL_SCHEDULE selector, CCSelectorProtocol target)
    {
        // explicity handle nil arguments when removing an object
        if (selector == null || target == null)
        {
            return;
        }

        HashSelectorEntry element;
        if (m_pHashForSelectors.TryGetValue(target, out element))
        {
            for (int i = 0; i < element.Timers.Count; i++)
            {
                var timer = element.Timers[i];

                if (selector == timer.Selector)
                {
                    if (timer == element.CurrentTimer && (!element.CurrentTimerSalvaged))
                    {
                        element.CurrentTimerSalvaged = true;
                    }

                    element.Timers.RemoveAt(i);

                    // update timerIndex in case we are in tick:, looping over the actions
                    if (element.TimerIndex >= i)
                    {
                        element.TimerIndex--;
                    }

                    if (element.Timers.Count == 0)
                    {
                        if (m_pCurrentTarget == element)
                        {
                            m_bCurrentTargetSalvaged = true;
                        }
                        else
                        {
                            RemoveHashElement(element);
                        }
                    }

                    return;
                }
            }
        }
    }

    /** Unschedules all selectors for a given target.
	     This also includes the "update" selector.
	     @since v0.99.3
	     */

    public void UnscheduleAllSelectorsForTarget(CCSelectorProtocol target)
    {
        // explicit NULL handling
        if (target == null)
        {
            return;
        }

        // custom selectors           
        HashSelectorEntry element;

        if (m_pHashForSelectors.TryGetValue(target, out element))
        {
            if (element.Timers.Contains(element.CurrentTimer))
            {
                element.CurrentTimerSalvaged = true;
            }
            element.Timers.Clear();

            if (m_pCurrentTarget == element)
            {
                m_bCurrentTargetSalvaged = true;
            }
            else
            {
                RemoveHashElement(element);
            }
        }

        // update selector
        UnscheduleUpdateForTarget(target);
    }

    /*
    unsigned int CCScheduler::scheduleScriptFunc(unsigned int nHandler, float fInterval, bool bPaused)
    {
        CCSchedulerScriptHandlerEntry* pEntry = CCSchedulerScriptHandlerEntry::create(nHandler, fInterval, bPaused);
        if (!m_pScriptHandlerEntries)
        {
            m_pScriptHandlerEntries = CCArray::create(20);
            m_pScriptHandlerEntries->retain();
        }
        m_pScriptHandlerEntries->addObject(pEntry);
        return pEntry->getEntryId();
    }

    void CCScheduler::unscheduleScriptEntry(unsigned int uScheduleScriptEntryID)
    {
        for (int i = m_pScriptHandlerEntries->count() - 1; i >= 0; i--)
        {
            CCSchedulerScriptHandlerEntry* pEntry = static_cast<CCSchedulerScriptHandlerEntry*>(m_pScriptHandlerEntries->objectAtIndex(i));
            if (pEntry->getEntryId() == uScheduleScriptEntryID)
            {
                pEntry->markedForDeletion();
                break;
            }
        }
    }
    */

    public void UnscheduleUpdateForTarget(CCSelectorProtocol target)
    {
        if (target == null)
        {
            return;
        }

        HashUpdateEntry element;
        if (m_pHashForUpdates.TryGetValue(target, out element))
        {
            if (m_bUpdateHashLocked)
            {
                element.Entry.MarkedForDeletion = true;
            }
            else
            {
                RemoveUpdateFromHash(element.Entry);
            }
        }
    }

    public void UnscheduleAllSelectors()
    {
        UnscheduleAllSelectorsWithMinPriority(int.MinValue);
    }

    public void UnscheduleAllSelectorsWithMinPriority(int minPriority)
    {
        var count = m_pHashForSelectors.Values.Count;
        if (s_pTmpHashSelectorArray.Length < count)
        {
            s_pTmpHashSelectorArray = new HashSelectorEntry[s_pTmpHashSelectorArray.Length * 2];
        }

        m_pHashForSelectors.Values.CopyTo(s_pTmpHashSelectorArray, 0);

        for (int i = 0; i < count; i++)
        {
            // Element may be removed in unscheduleAllSelectorsForTarget
            UnscheduleAllSelectorsForTarget(s_pTmpHashSelectorArray[i].Target);
        }

        // Updates selectors
        if (minPriority < 0 && m_pUpdatesNegList.Count > 0)
        {
            LinkedList<ListEntry> copy = new LinkedList<ListEntry>(m_pUpdatesNegList);
            foreach (ListEntry entry in copy)
            {
                if (entry.Priority >= minPriority)
                {
                    UnscheduleAllSelectorsForTarget(entry.Target);
                }
            }
        }

        if (minPriority <= 0 && m_pUpdates0List.Count > 0)
        {
            LinkedList<ListEntry> copy = new LinkedList<ListEntry>(m_pUpdates0List);
            foreach (ListEntry entry in copy)
            {
                UnscheduleAllSelectorsForTarget(entry.Target);
            }
        }

        if (m_pUpdatesPosList.Count > 0)
        {
            LinkedList<ListEntry> copy = new LinkedList<ListEntry>(m_pUpdatesPosList);
            foreach (ListEntry entry in copy)
        {
            if (entry.Priority >= minPriority)
            {
                UnscheduleAllSelectorsForTarget(entry.Target);
                }
            }
        }

        /*
            if (m_pScriptHandlerEntries)
            {
                m_pScriptHandlerEntries->removeAllObjects();
            }
        */
    }


    public List<CCSelectorProtocol> PauseAllTargets()
    {
        return PauseAllTargetsWithMinPriority(int.MinValue);
    }

    public List<CCSelectorProtocol> PauseAllTargetsWithMinPriority(int minPriority)
    {
        var idsWithSelectors = new List<CCSelectorProtocol>();

        // Custom Selectors
        foreach (HashSelectorEntry element in m_pHashForSelectors.Values)
        {
            element.Paused = true;
            idsWithSelectors.Add(element.Target);
        }

        // Updates selectors
        if (minPriority < 0)
        {
            foreach (ListEntry element in m_pUpdatesNegList)
            {
                if (element.Priority >= minPriority)
                {
                    element.Paused = true;
                    idsWithSelectors.Add(element.Target);
                }
            }
        }

        if (minPriority <= 0)
        {
            foreach (ListEntry element in m_pUpdates0List)
            {
                element.Paused = true;
                idsWithSelectors.Add(element.Target);
            }
        }

        if (minPriority < 0)
        {
            foreach (ListEntry element in m_pUpdatesPosList)
            {
                if (element.Priority >= minPriority)
                {
                    element.Paused = true;
                    idsWithSelectors.Add(element.Target);
                }
            }
        }

        return idsWithSelectors;
    }

    public void ResumeTargets(List<CCSelectorProtocol> targetsToResume)
    {
        foreach (CCSelectorProtocol target in targetsToResume)
        {
            ResumeTarget(target);
        }
    }

    public void PauseTarget(CCSelectorProtocol target)
    {
        Debug.Assert(target != null);

        // custom selectors
        HashSelectorEntry entry;
        if (m_pHashForSelectors.TryGetValue(target, out entry))
        {
            entry.Paused = true;
        }

        // Update selector
        HashUpdateEntry updateEntry;
        if (m_pHashForUpdates.TryGetValue(target, out updateEntry))
        {
            updateEntry.Entry.Paused = true;
        }
    }

    public void ResumeTarget(CCSelectorProtocol target)
    {
        Debug.Assert(target != null);

        // custom selectors
        HashSelectorEntry element;
        if (m_pHashForSelectors.TryGetValue(target, out element))
        {
            element.Paused = false;
        }

        // Update selector
        HashUpdateEntry elementUpdate;
        if (m_pHashForUpdates.TryGetValue(target, out elementUpdate))
        {
            elementUpdate.Entry.Paused = false;
        }
    }

    public bool IsTargetPaused(CCSelectorProtocol target)
    {
        Debug.Assert(target != null, "target must be non nil");

        // Custom selectors
        HashSelectorEntry element;
        if (m_pHashForSelectors.TryGetValue(target, out element))
        {
            return element.Paused;
        }

        return false; // should never get here
    }

    private void RemoveHashElement(HashSelectorEntry element)
    {
        m_pHashForSelectors.Remove(element.Target);

        element.Timers.Clear();
        element.Target = null;
    }

    private void RemoveUpdateFromHash(ListEntry entry)
    {
        HashUpdateEntry element;
        if (m_pHashForUpdates.TryGetValue(entry.Target, out element))
        {
            // list entry
            element.List.Remove(entry);
            element.Entry = null;

            // hash entry
            m_pHashForUpdates.Remove(entry.Target);

            element.Target = null;
        }
    }

    private void PriorityIn(LinkedList<ListEntry> list, CCSelectorProtocol target, int priority, bool paused)
    {
        var listElement = new ListEntry
        {
            Target = target,
            Priority = priority,
            Paused = paused,
            MarkedForDeletion = false
        };

        if (list.First == null)
        {
            list.AddFirst(listElement);
        }
        else
        {
            bool added = false;
            for (LinkedListNode<ListEntry> node = list.First; node != null; node = node.Next)
            {
                if (priority < node.Value.Priority)
                {
                    list.AddBefore(node, listElement);
                    added = true;
                    break;
                }
            }

            if (!added)
            {
                list.AddLast(listElement);
            }
        }

        // update hash entry for quick access
        var hashElement = new HashUpdateEntry
        {
            Target = target,
            List = list,
            Entry = listElement
        };

        m_pHashForUpdates.Add(target, hashElement);
    }

    private void AppendIn(LinkedList<ListEntry> list, CCSelectorProtocol target, bool paused)
    {
        var listElement = new ListEntry
        {
            Target = target,
            Paused = paused,
            MarkedForDeletion = false
        };

        list.AddLast(listElement);

        // update hash entry for quicker access
        var hashElement = new HashUpdateEntry
        {
            Target = target,
            List = list,
            Entry = listElement
        };

        m_pHashForUpdates.Add(target, hashElement);
    }

    #region Nested type: HashSelectorEntry

    private class HashSelectorEntry
    {
        public CCTimer CurrentTimer;
        public bool CurrentTimerSalvaged;
        public bool Paused;
        public CCSelectorProtocol Target;
        public int TimerIndex;
        public List<CCTimer> Timers;
    }

    #endregion

    #region Nested type: HashUpdateEntry

    private class HashUpdateEntry
    {
        public ListEntry Entry; // entry in the list
        public LinkedList<ListEntry> List; // Which list does it belong to ?
        public CCSelectorProtocol Target; // hash key
    }

    #endregion

    #region Nested type: ListEntry

    private class ListEntry
    {
        public bool MarkedForDeletion;
        public bool Paused;
        public int Priority;
        public CCSelectorProtocol Target;
    }

    #endregion
}