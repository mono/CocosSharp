using System;
using System.Collections.Generic;
using System.Diagnostics;
using CocosSharp;

namespace CocosSharp
{

	/// <summary>
	/// Light weight timer
	/// </summary>
	internal class CCTimer : ICCUpdatable
    {
		private CCScheduler Scheduler { get; set; }
		private readonly ICCUpdatable target;

		private readonly bool runForever;
		private readonly float delay;
		private readonly uint repeat; //0 = once, 1 is 2 x executed
		private float elapsed;
		private bool useDelay;

        //private int m_nScriptHandler;
		private uint timesExecuted;

		public float OriginalInterval { get; internal set; }
		public float Interval { get; set; }
		public Action<float> Selector { get; set; }


        #region Constructors

        /** Initializes a timer with a target and a selector. 
         */

        public CCTimer(CCScheduler scheduler, ICCUpdatable target, Action<float> selector)
            : this(scheduler, target, selector, 0, 0, 0)
        {
        }

        /** Initializes a timer with a target, a selector and an interval in seconds. 
         *  Target is not needed in c#, it is just for compatibility.
         */

        public CCTimer(CCScheduler scheduler, ICCUpdatable target, Action<float> selector, float seconds)
            : this(scheduler, target, selector, seconds, 0, 0)
        {
        }

        public CCTimer(CCScheduler scheduler, ICCUpdatable target, Action<float> selector, float seconds,
                       uint repeat, float delay)
        {
			this.Scheduler = scheduler;
			this.target = target;
			this.Selector = selector;
			this.elapsed = -1;
			this.OriginalInterval = seconds;
			this.Interval = seconds;
			this.delay = delay;
			this.useDelay = delay > 0f;
			this.repeat = repeat;
			this.runForever = repeat == uint.MaxValue;
        }

        /*
        public CCTimer(int scriptHandler, float seconds)
        {
            m_nScriptHandler = scriptHandler;
            Elapsed = -1;
            Interval = seconds;
        }
        */

        #endregion Constructors


        #region SelectorProtocol Members

        public void Update(float dt)
        {
            if (elapsed == -1)
            {
                elapsed = 0;
                timesExecuted = 0;
            }
            else
            {
                if (runForever && !useDelay)
                {
                    //standard timer usage
                    elapsed += dt;
                    if (elapsed >= Interval)
                    {
                        if (Selector != null)
                        {
                            Selector(elapsed);
                        }

                        /*
                        if (m_nScriptHandler != 0)
                        {
                            CCScriptEngineManager::sharedManager()->getScriptEngine()->executeSchedule(this, m_fElapsed);
                        }
                        */
						//Interval = OriginalInterval - (elapsed - Interval);
						elapsed = 0;
                    }
                }
                else
                {
                    //advanced usage
                    elapsed += dt;

                    if (useDelay)
                    {
                        if (elapsed >= delay)
                        {
                            if (Selector != null)
                            {
                                Selector(elapsed);
                            }

                            /*
                            if (m_nScriptHandler != 0)
                            {
                                CCScriptEngineManager::sharedManager()->getScriptEngine()->executeSchedule(this, m_fElapsed);
                            }
                            */

                            elapsed = elapsed - delay;
                            timesExecuted += 1;
                            useDelay = false;
                        }
                    }
                    else
                    {
                        if (elapsed >= Interval)
                        {
                            if (Selector != null)
                            {
                                Selector(elapsed);
                            }

                            /*
                            if (m_nScriptHandler)
                            {
                                CCScriptEngineManager::sharedManager()->getScriptEngine()->executeSchedule(m_nScriptHandler, m_fElapsed);
                            }
                            */

							//Interval = OriginalInterval - (elapsed - Interval);
                            elapsed = 0;
                            timesExecuted += 1;
                        }
                    }

                    if (!runForever && timesExecuted > repeat)
                    {
                        //unschedule timer
                        Scheduler.Unschedule(Selector, target);
                    }
                }
            }
        }

        #endregion
    }
}

namespace CocosSharp
{
	/// <summary>
	/// Scheduler is responsible for triggering the scheduled callbacks.
	/// You should not use NSTimer. Instead use this class.
	///
	/// There are 2 different types of callbacks (selectors):
	/// 
	/// - update selector: the 'update' selector will be called every frame. You can customize the priority.
	/// - custom selector: A custom selector will be called every frame, or with a custom interval of time
	///
	/// The 'custom selectors' should be avoided when possible. It is faster, and consumes less memory to use the 'update selector'.
	/// </summary>
    public class CCScheduler
    {
        public const uint RepeatForever = uint.MaxValue - 1;
        public const int PrioritySystem = int.MinValue;
        public const int PriorityNonSystemMin = PrioritySystem + 1;

		private readonly Dictionary<ICCUpdatable, HashTimeEntry> hashForTimers =
            new Dictionary<ICCUpdatable, HashTimeEntry>();

		private readonly Dictionary<ICCUpdatable, HashUpdateEntry> hashForUpdates =
            new Dictionary<ICCUpdatable, HashUpdateEntry>();

        // hash used to fetch quickly the list entries for pause,delete,etc
		private readonly LinkedList<ListEntry> updates0List = new LinkedList<ListEntry>(); // list priority == 0
		private readonly LinkedList<ListEntry> updatesNegList = new LinkedList<ListEntry>(); // list of priority < 0
		private readonly LinkedList<ListEntry> updatesPosList = new LinkedList<ListEntry>(); // list priority > 0

		private HashTimeEntry currentTarget;
		private bool isCurrentTargetSalvaged;
		private bool isUpdateHashLocked;

		public float TimeScale { get; set; }

		private static HashTimeEntry[] tmpHashSelectorArray = new HashTimeEntry[128];
		private static ICCUpdatable[] tmpSelectorArray = new ICCUpdatable[128];

		internal CCScheduler ()
		{
			TimeScale = 1.0f;
		}

		internal void Update (float dt)
        {
            isUpdateHashLocked = true;

            try
            {
                if (TimeScale != 1.0f)
                {
                    dt *= TimeScale;
                }

                LinkedListNode<ListEntry> next;

                // updates with priority < 0
                //foreach (ListEntry entry in _updatesNegList)
                for (LinkedListNode<ListEntry> node = updatesNegList.First; node != null; node = next)
                {
                    next = node.Next;
                    if (!node.Value.Paused && !node.Value.MarkedForDeletion)
                    {
                        node.Value.Target.Update(dt);
                    }
                }

                // updates with priority == 0
                //foreach (ListEntry entry in _updates0List)
                for (LinkedListNode<ListEntry> node = updates0List.First; node != null; node = next)
                {
                    next = node.Next;
                    if (!node.Value.Paused && !node.Value.MarkedForDeletion)
                    {
                        node.Value.Target.Update(dt);
                    }
                }

                // updates with priority > 0
                for (LinkedListNode<ListEntry> node = updatesPosList.First; node != null; node = next)
                {
                    next = node.Next;
                    if (!node.Value.Paused && !node.Value.MarkedForDeletion)
                    {
                        node.Value.Target.Update(dt);
                    }
                }

                // Iterate over all the custom selectors
                var count = hashForTimers.Keys.Count;
                if (tmpSelectorArray.Length < count)
                {
                    tmpSelectorArray = new ICCUpdatable[tmpSelectorArray.Length * 2];
                }
                hashForTimers.Keys.CopyTo(tmpSelectorArray, 0);

                for (int i = 0; i < count; i++)
                {
                    ICCUpdatable key = tmpSelectorArray[i];
                    if (!hashForTimers.ContainsKey(key))
                    {
                        continue;
                    }
                    HashTimeEntry elt = hashForTimers[key];

                    currentTarget = elt;
                    isCurrentTargetSalvaged = false;

                    if (!currentTarget.Paused)
                    {
                        // The 'timers' array may change while inside this loop
                        for (elt.TimerIndex = 0; elt.TimerIndex < elt.Timers.Count; ++elt.TimerIndex)
                        {
                            elt.CurrentTimer = elt.Timers[elt.TimerIndex];
							if(elt.CurrentTimer != null) {
	                            elt.CurrentTimerSalvaged = false;

	                            elt.CurrentTimer.Update(dt);

	                            elt.CurrentTimer = null;
							}
                        }
                    }

                    // only delete currentTarget if no actions were scheduled during the cycle (issue #481)
                    if (isCurrentTargetSalvaged && currentTarget.Timers.Count == 0)
                    {
                        RemoveHashElement(currentTarget);
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
                for (LinkedListNode<ListEntry> node = updatesNegList.First; node != null; node = next)
                {
                    next = node.Next;
                    if (node.Value.MarkedForDeletion)
                    {
                        updatesNegList.Remove(node);
                        RemoveUpdateFromHash(node.Value);
                    }
                }

                // updates with priority == 0
                for (LinkedListNode<ListEntry> node = updates0List.First; node != null; node = next)
                {
                    next = node.Next;
                    if (node.Value.MarkedForDeletion)
                    {
                        updates0List.Remove(node);
                        RemoveUpdateFromHash(node.Value);
                    }
                }

                // updates with priority > 0
                for (LinkedListNode<ListEntry> node = updatesPosList.First; node != null; node = next)
                {
                    next = node.Next;
                    if (node.Value.MarkedForDeletion)
                    {
                        updatesPosList.Remove(node);
                        RemoveUpdateFromHash(node.Value);
                    }
                }
            }
            finally
            {
                // Always do this just in case there is a problem

                isUpdateHashLocked = false;
                currentTarget = null;
            }
        }

        /** The scheduled method will be called every 'interval' seconds.
         If paused is YES, then it won't be called until it is resumed.
         If 'interval' is 0, it will be called every frame, but if so, it's recommended to use 'scheduleUpdateForTarget:' instead.
         If the selector is already scheduled, then only the interval parameter will be updated without re-scheduling it again.
         repeat let the action be repeated repeat + 1 times, use RepeatForever to let the action run continuously
         delay is the amount of time the action will wait before it'll start

         @since v0.99.3, repeat and delay added in v1.1
         */

        public void Schedule (Action<float> selector, ICCUpdatable target, float interval, uint repeat,
                                     float delay, bool paused)
        {
            Debug.Assert(selector != null);
            Debug.Assert(target != null);

            HashTimeEntry element;

            lock (hashForTimers)
            {
                if (!hashForTimers.TryGetValue(target, out element))
                {
                    element = new HashTimeEntry { Target = target };
                    hashForTimers[target] = element;

                    // Is this the 1st element ? Then set the pause level to all the selectors of this target
                    element.Paused = paused;
                }
                else
                {
                    if (element != null)
                    {
						Debug.Assert(element.Paused == paused, "CCScheduler.Schedule: All are paused");
                    }
                }
                if (element != null)
                {
                    if (element.Timers == null)
                    {
                        element.Timers = new List<CCTimer>();
                    }
                    else
                    {
                        CCTimer[] timers = element.Timers.ToArray();
                        foreach (var timer in timers)
                        {
                            if (timer == null)
                            {
                                continue;
                            }
                            if (selector == timer.Selector)
                            {
                                CCLog.Log(
                                    "CCSheduler#scheduleSelector. Selector already scheduled. Updating interval from: {0} to {1}",
                                    timer.Interval, interval);
                                timer.Interval = interval;
                                return;
                            }
                        }
                    }

                    element.Timers.Add(new CCTimer(this, target, selector, interval, repeat, delay));
                }
            }
        }

        /** Schedules the 'update' selector for a given target with a given priority.
    	     The 'update' selector will be called every frame.
    	     The lower the priority, the earlier it is called.
    	     @since v0.99.3
    	     */

        public void Schedule (ICCUpdatable targt, int priority, bool paused)
        {
            HashUpdateEntry element;

            if (hashForUpdates.TryGetValue(targt, out element))
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
                AppendIn(updates0List, targt, paused);
            }
            else if (priority < 0)
            {
                PriorityIn(updatesNegList, targt, priority, paused);
            }
            else
            {
                PriorityIn(updatesPosList, targt, priority, paused);
            }
        }

        /** Unschedule a selector for a given target.
    	     If you want to unschedule the "update", use unscheudleUpdateForTarget.
    	     @since v0.99.3
    	     */

		public void Unschedule (Action<float> selector, ICCUpdatable target)
        {
            // explicity handle nil arguments when removing an object
            if (selector == null || target == null)
            {
                return;
            }

            HashTimeEntry element;
            if (hashForTimers.TryGetValue(target, out element))
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
                            if (currentTarget == element)
                            {
                                isCurrentTargetSalvaged = true;
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

		public void UnscheduleAll (ICCUpdatable target)
        {
            // explicit NULL handling
            if (target == null)
            {
                return;
            }

            // custom selectors           
            HashTimeEntry element;

            if (hashForTimers.TryGetValue(target, out element))
            {
                if (element.Timers.Contains(element.CurrentTimer))
                {
                    element.CurrentTimerSalvaged = true;
                }
                element.Timers.Clear();

                if (currentTarget == element)
                {
                    isCurrentTargetSalvaged = true;
                }
                else
                {
                    RemoveHashElement(element);
                }
            }

            // update selector
            Unschedule(target);
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

		public void Unschedule (ICCUpdatable target)
        {
            if (target == null)
            {
                return;
            }

            HashUpdateEntry element;
            if (hashForUpdates.TryGetValue(target, out element))
            {
                if (isUpdateHashLocked)
                {
                    element.Entry.MarkedForDeletion = true;
                }
                else
                {
                    RemoveUpdateFromHash(element.Entry);
                }
            }
        }

		/// <summary>
		/// Gets a value indicating whether the ActionManager is active.
		/// The ActionManager can be stopped from processing actions by calling UnscheduleAll() method.
		/// </summary>
		/// <value><c>true</c> if the ActionManager active and ready to process Actions; otherwise, <c>false</c>.</value>
		public bool IsActionManagerActive
		{
			get {

				var target = CCDirector.SharedDirector.ActionManager;

				LinkedListNode<ListEntry> next;

				for (LinkedListNode<ListEntry> node = updatesNegList.First; node != null; node = next)
				{
					next = node.Next;
					if (node.Value.Target == target && !node.Value.MarkedForDeletion)
					{
						return true;
					}
				}

				return false;
			}
		}

		/// <summary>
		/// Starts the action manager.  		
		/// This would be called after UnscheduleAll() method has been called to restart the ActionManager.
		/// </summary>
		public void StartActionManager()
		{
			if (!IsActionManagerActive)
				Schedule  (CCDirector.SharedDirector.ActionManager, CCScheduler.PrioritySystem, false);
		}

		public void UnscheduleAll ()
        {
			// This also stops ActionManger from updating which means all actions are stopped as well.
			UnscheduleAll (PrioritySystem);
        }

		public void UnscheduleAll (int minPriority)
        {
            var count = hashForTimers.Values.Count;
            if (tmpHashSelectorArray.Length < count)
            {
                tmpHashSelectorArray = new HashTimeEntry[tmpHashSelectorArray.Length * 2];
            }

            hashForTimers.Values.CopyTo(tmpHashSelectorArray, 0);

            for (int i = 0; i < count; i++)
            {
                // Element may be removed in unscheduleAllSelectorsForTarget
                UnscheduleAll(tmpHashSelectorArray[i].Target);
            }

            // Updates selectors
            if (minPriority < 0 && updatesNegList.Count > 0)
            {
                LinkedList<ListEntry> copy = new LinkedList<ListEntry>(updatesNegList);
                foreach (ListEntry entry in copy)
                {
                    if (entry.Priority >= minPriority)
                    {
                        UnscheduleAll(entry.Target);
                    }
                }
            }

            if (minPriority <= 0 && updates0List.Count > 0)
            {
                LinkedList<ListEntry> copy = new LinkedList<ListEntry>(updates0List);
                foreach (ListEntry entry in copy)
                {
                    UnscheduleAll(entry.Target);
                }
            }

            if (updatesPosList.Count > 0)
            {
                LinkedList<ListEntry> copy = new LinkedList<ListEntry>(updatesPosList);
                foreach (ListEntry entry in copy)
                {
                    if (entry.Priority >= minPriority)
                    {
                        UnscheduleAll(entry.Target);
                    }
                }
            }
        }

        public List<ICCUpdatable> PauseAllTargets()
        {
            return PauseAllTargets(int.MinValue);
        }

        public List<ICCUpdatable> PauseAllTargets(int minPriority)
        {
            var idsWithSelectors = new List<ICCUpdatable>();

            // Custom Selectors
            foreach (HashTimeEntry element in hashForTimers.Values)
            {
                element.Paused = true;
				if (!idsWithSelectors.Contains(element.Target))
                	idsWithSelectors.Add(element.Target);
            }

            // Updates selectors
            if (minPriority < 0)
            {
                foreach (ListEntry element in updatesNegList)
                {
                    if (element.Priority >= minPriority)
                    {
                        element.Paused = true;
						if (!idsWithSelectors.Contains(element.Target))
	                        idsWithSelectors.Add(element.Target);
                    }
                }
            }

            if (minPriority <= 0)
            {
                foreach (ListEntry element in updates0List)
                {
                    element.Paused = true;
					if (!idsWithSelectors.Contains(element.Target))
						idsWithSelectors.Add(element.Target);
                }
            }

            if (minPriority < 0)
            {
                foreach (ListEntry element in updatesPosList)
                {
                    if (element.Priority >= minPriority)
                    {
                        element.Paused = true;
						if (!idsWithSelectors.Contains(element.Target))
	                        idsWithSelectors.Add(element.Target);
                    }
                }
            }

            return idsWithSelectors;
        }

        public void PauseTarget(ICCUpdatable target)
        {
            Debug.Assert(target != null);

            // custom selectors
            HashTimeEntry entry;
            if (hashForTimers.TryGetValue(target, out entry))
            {
                entry.Paused = true;
            }

            // Update selector
            HashUpdateEntry updateEntry;
            if (hashForUpdates.TryGetValue(target, out updateEntry))
            {
                updateEntry.Entry.Paused = true;
            }
        }

		public void Resume (List<ICCUpdatable> targetsToResume)
		{
			foreach (ICCUpdatable target in targetsToResume)
			{
				Resume(target);
			}
		}

		public void Resume (ICCUpdatable target)
        {
            Debug.Assert(target != null);

            // custom selectors
            HashTimeEntry element;
            if (hashForTimers.TryGetValue(target, out element))
            {
                element.Paused = false;
            }

            // Update selector
            HashUpdateEntry elementUpdate;
            if (hashForUpdates.TryGetValue(target, out elementUpdate))
            {
                elementUpdate.Entry.Paused = false;
            }
        }

        public bool IsTargetPaused(ICCUpdatable target)
        {
            Debug.Assert(target != null, "target must be non nil");

            // Custom selectors
            HashTimeEntry element;
            if (hashForTimers.TryGetValue(target, out element))
            {
                return element.Paused;
            }

            // We should check update selectors if target does not have custom selectors
            HashUpdateEntry elementUpdate;
            if (hashForUpdates.TryGetValue(target, out elementUpdate))
            {
                return elementUpdate.Entry.Paused;
            }

            return false; // should never get here
        }

        private void RemoveHashElement(HashTimeEntry element)
        {
            hashForTimers.Remove(element.Target);

            element.Timers.Clear();
            element.Target = null;
        }

        private void RemoveUpdateFromHash(ListEntry entry)
        {
            HashUpdateEntry element;
            if (hashForUpdates.TryGetValue(entry.Target, out element))
            {
                // list entry
                element.List.Remove(entry);
                element.Entry = null;

                // hash entry
                hashForUpdates.Remove(entry.Target);

                element.Target = null;
            }
        }

        private void PriorityIn(LinkedList<ListEntry> list, ICCUpdatable target, int priority, bool paused)
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

            hashForUpdates.Add(target, hashElement);
        }

        private void AppendIn(LinkedList<ListEntry> list, ICCUpdatable target, bool paused)
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

            hashForUpdates.Add(target, hashElement);
        }

        #region Nested type: HashSelectorEntry

        private class HashTimeEntry
        {
            public CCTimer CurrentTimer;
            public bool CurrentTimerSalvaged;
            public bool Paused;
            public ICCUpdatable Target;
            public int TimerIndex;
            public List<CCTimer> Timers;
        }

        #endregion

        #region Nested type: HashUpdateEntry

        private class HashUpdateEntry
        {
            public ListEntry Entry; // entry in the list
            public LinkedList<ListEntry> List; // Which list does it belong to ?
            public ICCUpdatable Target; // hash key
        }

        #endregion

        #region Nested type: ListEntry

        private class ListEntry
        {
            public bool MarkedForDeletion;
            public bool Paused;
            public int Priority;
            public ICCUpdatable Target;
        }

        #endregion
    }
}