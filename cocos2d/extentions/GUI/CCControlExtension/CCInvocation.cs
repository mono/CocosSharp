using System;
/*
 *
 * Helper class to store targets and selectors (and eventually, params?) in the same CCMutableArray. Basically a very crude form of a NSInvocation
 */

namespace Cocos2D
{

    public class CCInvocation 
    {
        private readonly Action<object, CCControlEvent> m_pAction;
        private readonly CCControlEvent m_pControlEvent;

        private readonly object m_target;

		public CCInvocation(object target, Action<object, CCControlEvent> action, CCControlEvent controlEvent)
        {
            m_target = target;
            m_pAction = action;
            m_pControlEvent = controlEvent;
        }

		public Action<object, CCControlEvent> Action
        {
            get { return m_pAction; }
        }

        public object Target
        {
            get { return m_target; }
        }

        public CCControlEvent ControlEvent
        {
            get { return m_pControlEvent; }
        }

        public void Invoke(object sender)
        {
            if (m_target != null && m_pAction != null)
            {
                Action(sender, m_pControlEvent);
            }
        }
    }
}
