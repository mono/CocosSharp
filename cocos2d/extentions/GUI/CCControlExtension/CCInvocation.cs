using System;
/*
 *
 * Helper class to store targets and selectors (and eventually, params?) in the same CCMutableArray. Basically a very crude form of a NSInvocation
 */

namespace Cocos2D
{

    public class CCInvocation 
    {
        private readonly Action<object, CCControlEvent> _action;
        private readonly CCControlEvent _controlEvent;

        private readonly object _target;

		public CCInvocation(object target, Action<object, CCControlEvent> action, CCControlEvent controlEvent)
        {
            _target = target;
            _action = action;
            _controlEvent = controlEvent;
        }

		public Action<object, CCControlEvent> Action
        {
            get { return _action; }
        }

        public object Target
        {
            get { return _target; }
        }

        public CCControlEvent ControlEvent
        {
            get { return _controlEvent; }
        }

        public void Invoke(object sender)
        {
            if (_target != null && _action != null)
            {
                Action(sender, _controlEvent);
            }
        }
    }
}
