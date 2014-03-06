
namespace CocosSharp
{
    /// <summary>
    ///  Object than contains the delegate and priority of the event handler.
    /// </summary>
    public class CCTouchHandler
    {
		/// <summary>
		/// enabled selectors 
		/// </summary>
		public int EnabledSelectors { get; set; }

        /// <summary>
        /// delegate
        /// </summary>
		public ICCTouchDelegate Delegate { get; set; }

		/// <summary>
        /// priority
        /// </summary>
		public int Priority { get; set; }

		/// <summary>
		/// initializes a TouchHandler with a delegate and a priority 
		/// </summary>
		public CCTouchHandler ( ICCTouchDelegate touchDelegate, int touchPriority )
		{
            Delegate = touchDelegate;
            Priority = touchPriority;
            EnabledSelectors = 0;
        }

    }
}