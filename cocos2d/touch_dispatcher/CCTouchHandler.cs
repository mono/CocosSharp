
namespace cocos2d
{
    /// <summary>
    ///  Object than contains the delegate and priority of the event handler.
    /// </summary>
    public class CCTouchHandler
    {
        protected int m_nEnabledSelectors;
        protected int m_nPriority;
        protected ICCTouchDelegate m_pDelegate;

        /// <summary>
        /// delegate
        /// </summary>
        public ICCTouchDelegate Delegate
        {
            get { return m_pDelegate; }
            set { m_pDelegate = value; }
        }

        /// <summary>
        /// priority
        /// </summary>
        public int Priority
        {
            get { return m_nPriority; }
            set { m_nPriority = value; }
        }

        /// <summary>
        /// enabled selectors 
        /// </summary>
        public int EnabledSelectors
        {
            get { return m_nEnabledSelectors; }
            set { m_nEnabledSelectors = value; }
        }

        /// <summary>
        /// initializes a TouchHandler with a delegate and a priority 
        /// </summary>
        public virtual bool InitWithDelegate(ICCTouchDelegate pDelegate, int nPriority)
        {
            m_pDelegate = pDelegate;
            m_nPriority = nPriority;
            m_nEnabledSelectors = 0;

            return true;
        }

        /// <summary>
        /// allocates a TouchHandler with a delegate and a priority 
        /// </summary>
        public static CCTouchHandler Create(ICCTouchDelegate pDelegate, int nPriority)
        {
            var pHandler = new CCTouchHandler();
            pHandler.InitWithDelegate(pDelegate, nPriority);
            return pHandler;
        }
    }
}