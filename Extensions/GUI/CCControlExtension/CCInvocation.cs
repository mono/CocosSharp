using System;
/*
 *
 * Helper class to store targets and selectors (and eventually, params?) in the same CCMutableArray. Basically a very crude form of a NSInvocation
 */

namespace CocosSharp
{
    public class CCInvocation 
    {
		#region Properties

		public Action<object, CCControlEvent> Action { get; private set; }
		public object Target { get; private set; }
		public CCControlEvent ControlEvent { get; private set; }

		#endregion Properties


		#region Constructors

		public CCInvocation(object target, Action<object, CCControlEvent> action, CCControlEvent controlEvent)
        {
			Target = target;
			Action = action;
			ControlEvent = controlEvent;
        }

		#endregion Constructors

        public void Invoke(object sender)
        {
			if (Target != null && Action != null)
            {
				Action(sender, ControlEvent);
            }
        }
    }
}
