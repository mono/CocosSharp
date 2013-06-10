using System.Collections.Generic;
using System.Diagnostics;

namespace Cocos2D
{
    public class CCActionManager : ICCSelectorProtocol
    {
        private static CCNode[] m_pTmpKeysArray = new CCNode[128];
        private bool m_bCurrentTargetSalvaged;
        private HashElement m_pCurrentTarget;
        private readonly Dictionary<object, HashElement> m_pTargets = new Dictionary<object, HashElement>();

        #region SelectorProtocol Members

        public void Update(float dt)
        {
            int count = m_pTargets.Keys.Count;
            while (m_pTmpKeysArray.Length < count)
            {
                m_pTmpKeysArray = new CCNode[m_pTmpKeysArray.Length * 2];
            }

            m_pTargets.Keys.CopyTo(m_pTmpKeysArray, 0);

            for (int i = 0; i < count; i++)
            {
                HashElement elt;
                if (!m_pTargets.TryGetValue(m_pTmpKeysArray[i], out elt))
                {
                    continue;
                }

                m_pCurrentTarget = elt;
                m_bCurrentTargetSalvaged = false;

                if (!m_pCurrentTarget.Paused)
                {
                    // The 'actions' may change while inside this loop.
                    for (m_pCurrentTarget.ActionIndex = 0;
                         m_pCurrentTarget.ActionIndex < m_pCurrentTarget.Actions.Count;
                         m_pCurrentTarget.ActionIndex++)
                    {
                        m_pCurrentTarget.CurrentAction = m_pCurrentTarget.Actions[m_pCurrentTarget.ActionIndex];
                        if (m_pCurrentTarget.CurrentAction == null)
                        {
                            continue;
                        }

                        m_pCurrentTarget.CurrentActionSalvaged = false;

                        m_pCurrentTarget.CurrentAction.Step(dt);

                        if (m_pCurrentTarget.CurrentActionSalvaged)
                        {
                            // The currentAction told the node to remove it. To prevent the action from
                            // accidentally deallocating itself before finishing its step, we retained
                            // it. Now that step is done, it's safe to release it.

                            //m_pCurrentTarget->currentAction->release();
                        }
                        else if (m_pCurrentTarget.CurrentAction.IsDone)
                        {
                            m_pCurrentTarget.CurrentAction.Stop();

                            CCAction action = m_pCurrentTarget.CurrentAction;
                            // Make currentAction nil to prevent removeAction from salvaging it.
                            m_pCurrentTarget.CurrentAction = null;
                            RemoveAction(action);
                        }

                        m_pCurrentTarget.CurrentAction = null;
                    }
                }

                // only delete currentTarget if no actions were scheduled during the cycle (issue #481)
                if (m_bCurrentTargetSalvaged && m_pCurrentTarget.Actions.Count == 0)
                {
                    DeleteHashElement(m_pCurrentTarget);
                }
            }

            // issue #635
            m_pCurrentTarget = null;
        }

        #endregion

        ~CCActionManager()
        {
            RemoveAllActions();
        }

        protected void DeleteHashElement(HashElement element)
        {
            element.Actions.Clear();
            m_pTargets.Remove(element.Target);
            element.Target = null;
        }

        protected void ActionAllocWithHashElement(HashElement element)
        {
            if (element.Actions == null)
            {
                element.Actions = new List<CCAction>();
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

            // update actionIndex in case we are in tick. looping over the actions
            if (element.ActionIndex >= index)
            {
                element.ActionIndex--;
            }

            if (element.Actions.Count == 0)
            {
                if (m_pCurrentTarget == element)
                {
                    m_bCurrentTargetSalvaged = true;
                }
                else
                {
                    DeleteHashElement(element);
                }
            }
        }

        public void PauseTarget(object target)
        {
            HashElement element;
            if (m_pTargets.TryGetValue(target, out element))
            {
                element.Paused = true;
            }
        }

        public void ResumeTarget(object target)
        {
            HashElement element;
            if (m_pTargets.TryGetValue(target, out element))
            {
                element.Paused = false;
            }
        }

        public List<object> PauseAllRunningActions()
        {
            var idsWithActions = new List<object>();

            foreach (var element in m_pTargets.Values)
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

        public void AddAction(CCAction action, CCNode target, bool paused)
        {
            Debug.Assert(action != null);
            Debug.Assert(target != null);

            HashElement element;
            if (!m_pTargets.TryGetValue(target, out element))
            {
                element = new HashElement();
                element.Paused = paused;
                element.Target = target;
                m_pTargets.Add(target, element);
            }

            ActionAllocWithHashElement(element);

            Debug.Assert(!element.Actions.Contains(action));
            element.Actions.Add(action);

            action.StartWithTarget(target);
        }

        public void RemoveAllActions()
        {
            int count = m_pTargets.Keys.Count;
            if (m_pTmpKeysArray.Length < count)
            {
                m_pTmpKeysArray = new CCNode[m_pTmpKeysArray.Length * 2];
            }

            m_pTargets.Keys.CopyTo(m_pTmpKeysArray, 0);

            for (int i = 0; i < count; i++)
            {
                RemoveAllActionsFromTarget(m_pTmpKeysArray[i]);
            }
        }

        public void RemoveAllActionsFromTarget(CCNode target)
        {
            if (target == null)
            {
                return;
            }

            HashElement element;
            if (m_pTargets.TryGetValue(target, out element))
            {
                if (element.Actions.Contains(element.CurrentAction) && (!element.CurrentActionSalvaged))
                {
                    element.CurrentActionSalvaged = true;
                }

                element.Actions.Clear();

                if (m_pCurrentTarget == element)
                {
                    m_bCurrentTargetSalvaged = true;
                }
                else
                {
                    DeleteHashElement(element);
                }
            }
        }

        public void RemoveAction(CCAction action)
        {
            if (action == null)
            {
                return;
            }

            object target = action.OriginalTarget;
            HashElement element;
            if (m_pTargets.TryGetValue(target, out element))
            {
                int i = element.Actions.IndexOf(action);

                if (i != -1)
                {
                    RemoveActionAtIndex(i, element);
                }
                else
                {
                    CCLog.Log("cocos2d: removeAction: Action not found");
                }
            }
            else
            {
                CCLog.Log("cocos2d: removeAction: Target not found");
            }
        }

        public void RemoveActionByTag(int tag, CCNode target)
        {
            Debug.Assert((tag != (int) CCActionTag.Invalid));
            Debug.Assert(target != null);

            HashElement element;
            if (m_pTargets.TryGetValue(target, out element))
            {
                int limit = element.Actions.Count;
                for (int i = 0; i < limit; i++)
                {
                    CCAction action = element.Actions[i];

                    if (action.Tag == tag && action.OriginalTarget == target)
                    {
                        RemoveActionAtIndex(i, element);
                        break;
                    }
                }
                CCLog.Log("cocos2d : removeActionByTag: Tag " + tag + " not found");
            }
            else
            {
                CCLog.Log("cocos2d : removeActionByTag: Target not found");
            }
        }

        public CCAction GetActionByTag(int tag, CCNode target)
        {
            Debug.Assert(tag != (int) CCActionTag.Invalid);

            HashElement element;
            if (m_pTargets.TryGetValue(target, out element))
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
            if (m_pTargets.TryGetValue(target, out element))
            {
                return (element.Actions != null) ? element.Actions.Count : 0;
            }
            return 0;
        }

        protected class HashElement
        {
            public int ActionIndex;
            public List<CCAction> Actions;
            public CCAction CurrentAction;
            public bool CurrentActionSalvaged;
            public bool Paused;
            public object Target;
        }
    }
}