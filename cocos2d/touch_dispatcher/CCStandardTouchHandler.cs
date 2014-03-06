
namespace CocosSharp
{
    /// <summary>
    /// It forwardes each event to the delegate.
    /// </summary>
    public class CCStandardTouchHandler : CCTouchHandler
    {

		/// <summary>
		///  initializes a TouchHandler with a delegate and a priority
		/// </summary>
		public CCStandardTouchHandler ( ICCStandardTouchDelegate touchDelegate, int touchPriority )
			: base (touchDelegate, touchPriority)
		{	}
    }
}