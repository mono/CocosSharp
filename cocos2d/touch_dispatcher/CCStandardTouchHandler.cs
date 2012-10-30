
namespace cocos2d
{
    /// <summary>
    /// It forwardes each event to the delegate.
    /// </summary>
    public class CCStandardTouchHandler : CCTouchHandler
    {
        /// <summary>
        ///  initializes a TouchHandler with a delegate and a priority
        /// </summary>
        public virtual bool InitWithDelegate(ICCStandardTouchDelegate pDelegate, int nPriority)
        {
            return base.InitWithDelegate(pDelegate, nPriority);
        }

        /// <summary>
        /// allocates a TouchHandler with a delegate and a priority
        /// </summary>
        public static CCStandardTouchHandler HandlerWithDelegate(ICCStandardTouchDelegate pDelegate, int nPriority)
        {
            var pHandler = new CCStandardTouchHandler();
            pHandler.InitWithDelegate(pDelegate, nPriority);
            return pHandler;
        }
    }
}