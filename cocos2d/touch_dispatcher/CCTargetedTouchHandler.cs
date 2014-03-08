
using System.Collections.Generic;

namespace CocosSharp
{
    public class CCTargetedTouchHandler : CCTouchHandler
    {
        /// <summary>
        /// whether or not the touches are swallowed
        /// </summary>
		public bool IsSwallowsTouches { get; set; }

        /// <summary>
        /// MutableSet that contains the claimed touches 
        /// </summary>
		public List<CCTouch> ClaimedTouches { get; protected set; }

		/// <summary>
		///  initializes a TargetedTouchHandler with a delegate, a priority and whether or not it swallows touches or not
		/// </summary>
		public CCTargetedTouchHandler (ICCTargetedTouchDelegate touchDelegate, int touchPriority, bool isSwallowTouches) 
			: base (touchDelegate, touchPriority)
		{ 
            ClaimedTouches = new List<CCTouch>();
            IsSwallowsTouches = isSwallowTouches;

        }
    }
}