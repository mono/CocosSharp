using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace CocosSharp
{
	public class CCActionManager : ICCUpdatable, IDisposable
    {
		protected class HashElement
		{
			public int ActionIndex;
			public List<CCAction> Actions;
			public List<CCActionState> ActionStates;
			public CCAction CurrentAction;
			public CCActionState CurrentActionState;
			public bool CurrentActionSalvaged;
			public bool Paused;
			public object Target;
		}

		static CCNode[] tmpKeysArray = new CCNode[128];

		readonly Dictionary<object, HashElement> targets = new Dictionary<object, HashElement>();

		bool currentTargetSalvaged;
		HashElement currentTarget;


		#region Cleaning up

		~CCActionManager()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);

			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing) 
			{
				// Dispose of managed resources
			}

			this.RemoveAllActions();
		}

		#endregion Cleaning up


		public CCAction GetActionByTag(int tag, CCNode target)
		{
			Debug.Assert(tag != (int) CCActionTag.Invalid);

			// Early out if we do not have any targets to search
			if (targets.Count == 0)
				return null;

			HashElement element;
			if (targets.TryGetValue(target, out element))
			{
				if (element.Actions != null)
				{
					int limit = element.Actions.Count;
					for (int i = 0; i < limit; i++)
					{
						CCAction action = element.Actions[i];

						if (action.Tag == tag)
						{
							return action;
						}
					}
					CCLog.Log("cocos2d : getActionByTag: Tag " + tag + " not found");
				}
			}
			else
			{
				CCLog.Log("cocos2d : getActionByTag: Target not found");
			}
			return null;
		}

		public int NumberOfRunningActionsInTarget(CCNode target)
		{
			HashElement element;
			if (targets.TryGetValue(target, out element))
			{
				return (element.Actions != null) ? element.Actions.Count : 0;
			}
			return 0;
		}

        public void Update(float dt)
        {
            int count = targets.Keys.Count;
            while (tmpKeysArray.Length < count)
            {
                tmpKeysArray = new CCNode[tmpKeysArray.Length * 2];
            }

            targets.Keys.CopyTo(tmpKeysArray, 0);

            for (int i = 0; i < count; i++)
            {
                HashElement elt;
                if (!targets.TryGetValue(tmpKeysArray[i], out elt))
                {
                    continue;
                }

                currentTarget = elt;
                currentTargetSalvaged = false;

                if (!currentTarget.Paused)
                {
                    // The 'actions' may change while inside this loop.
                    for (currentTarget.ActionIndex = 0;
                         currentTarget.ActionIndex < currentTarget.Actions.Count;
                         currentTarget.ActionIndex++)
                    {
                        currentTarget.CurrentAction = currentTarget.Actions[currentTarget.ActionIndex];
						currentTarget.CurrentActionState = currentTarget.ActionStates [currentTarget.ActionIndex];
                        if (currentTarget.CurrentAction == null)
                        {
                            continue;
                        }

                        currentTarget.CurrentActionSalvaged = false;

						currentTarget.CurrentActionState.Step (dt);

						if (currentTarget.CurrentActionSalvaged) {
							// The currentAction told the node to remove it. To prevent the action from
							// accidentally deallocating itself before finishing its step, we retained
							// it. Now that step is done, it's safe to release it.

							//currentTarget->currentAction->release();
						} else if (currentTarget.CurrentActionState.IsDone) {
							currentTarget.CurrentActionState.Stop ();

							CCActionState actionState = currentTarget.CurrentActionState;
							// Make currentAction nil to prevent removeAction from salvaging it.
							currentTarget.CurrentAction = null;
							RemoveAction (actionState);
						}
                        currentTarget.CurrentAction = null;
                    }
                }

                // only delete currentTarget if no actions were scheduled during the cycle (issue #481)
                if (currentTargetSalvaged && currentTarget.Actions.Count == 0)
                {
                    DeleteHashElement(currentTarget);
                }
            }

            // issue #635
            currentTarget = null;
        }

        protected void DeleteHashElement(HashElement element)
        {
            element.Actions.Clear();
			element.ActionStates.Clear ();
            targets.Remove(element.Target);
            element.Target = null;
        }

        protected void ActionAllocWithHashElement(HashElement element)
        {
            if (element.Actions == null)
            {
                element.Actions = new List<CCAction>();
				element.ActionStates = new List<CCActionState> ();
            }
        }


		#region Action running

        public void PauseTarget(object target)
        {
            HashElement element;
            if (targets.TryGetValue(target, out element))
            {
                element.Paused = true;
            }
        }

        public void ResumeTarget(object target)
        {
            HashElement element;
            if (targets.TryGetValue(target, out element))
            {
                element.Paused = false;
            }
        }

        public List<object> PauseAllRunningActions()
        {
            var idsWithActions = new List<object>();

            foreach (var element in targets.Values)
            {
                if (!element.Paused)
                {
                    element.Paused = true;
                    idsWithActions.Add(element.Target);
                }
            }

            return idsWithActions;
        }

        public void ResumeTargets(List<object> targetsToResume)
        {
            for (int i = 0; i < targetsToResume.Count; i++)
            {
                ResumeTarget(targetsToResume[i]);
            }
        }

		#endregion Action running


		#region Adding/removing actions

        public CCActionState AddAction(CCAction action, CCNode target, bool paused = false)
        {
            Debug.Assert(action != null);
            Debug.Assert(target != null);

            HashElement element;
            if (!targets.TryGetValue(target, out element))
            {
                element = new HashElement();
                element.Paused = paused;
                element.Target = target;
                targets.Add(target, element);
            }

            ActionAllocWithHashElement(element);

            Debug.Assert(!element.Actions.Contains(action));
            element.Actions.Add(action);

            CCActionState state = null;

			state = action.StartAction (target);
			element.ActionStates.Add (state);

            return state;
        }

        public void RemoveAllActions()
        {
            int count = targets.Keys.Count;
            if (tmpKeysArray.Length < count)
            {
                tmpKeysArray = new CCNode[tmpKeysArray.Length * 2];
            }

            targets.Keys.CopyTo(tmpKeysArray, 0);

            for (int i = 0; i < count; i++)
            {
                RemoveAllActionsFromTarget(tmpKeysArray[i]);
            }
        }

        public void RemoveAllActionsFromTarget(CCNode target)
        {
            if (target == null)
            {
                return;
            }

            HashElement element;
            if (targets.TryGetValue(target, out element))
            {
                if (element.Actions.Contains(element.CurrentAction) && (!element.CurrentActionSalvaged))
                {
                    element.CurrentActionSalvaged = true;
                }

                element.Actions.Clear();

                if (currentTarget == element)
                {
                    currentTargetSalvaged = true;
                }
                else
                {
                    DeleteHashElement(element);
                }
            }
        }

        public void RemoveAction(CCAction action)
        {
            if (action == null || action.OriginalTarget == null)
            {
                return;
            }

            object target = action.OriginalTarget;
            HashElement element;
            if (targets.TryGetValue(target, out element))
            {
                int i = element.Actions.IndexOf(action);

                if (i != -1)
                {
                    RemoveActionAtIndex(i, element);
                }
                else
                {
                    CCLog.Log("CocosSharp: removeAction: Action not found");
                }
            }
            else
            {
                CCLog.Log("CocosSharp: removeAction: Target not found");
            }
        }

		public void RemoveAction(CCActionState actionState)
		{
			if (actionState == null || actionState.OriginalTarget == null)
			{
				return;
			}

			object target = actionState.OriginalTarget;
			HashElement element;
			if (targets.TryGetValue(target, out element))
			{
				int i = element.ActionStates.IndexOf(actionState);

				if (i != -1)
				{
					RemoveActionAtIndex(i, element);
				}
				else
				{
					CCLog.Log("CocosSharp: removeAction: Action not found");
				}
			}
			else
			{
				CCLog.Log("CocosSharp: removeAction: Target not found");
			}
		}

		protected void RemoveActionAtIndex(int index, HashElement element)
		{
			CCAction action = element.Actions[index];

			if (action == element.CurrentAction && (!element.CurrentActionSalvaged))
			{
				element.CurrentActionSalvaged = true;
			}

			element.Actions.RemoveAt(index);
			element.ActionStates.RemoveAt (index);

			// update actionIndex in case we are in tick. looping over the actions
			if (element.ActionIndex >= index)
			{
				element.ActionIndex--;
			}

			if (element.Actions.Count == 0)
			{
				if (currentTarget == element)
				{
					currentTargetSalvaged = true;
				}
				else
				{
					DeleteHashElement(element);
				}
			}
		}

        public void RemoveActionByTag(int tag, CCNode target)
        {
            Debug.Assert((tag != (int) CCActionTag.Invalid));
            Debug.Assert(target != null);

			// Early out if we do not have any targets to search
			if (targets.Count == 0)
				return;

            HashElement element;
            if (targets.TryGetValue(target, out element))
            {
                int limit = element.Actions.Count;
				bool tagFound = false;

                for (int i = 0; i < limit; i++)
                {
                    CCAction action = element.Actions[i];

					if (action.Tag == tag && element.ActionStates[i].OriginalTarget == target) {
						RemoveActionAtIndex (i, element);
						tagFound = true;
						break;
					}
                }

				if (!tagFound)
                	CCLog.Log("cocos2d : removeActionByTag: Tag " + tag + " not found");
            }
            else
            {
                CCLog.Log("cocos2d : removeActionByTag: Target not found");
            }
        }

		#endregion Adding/removing actions
    }
}