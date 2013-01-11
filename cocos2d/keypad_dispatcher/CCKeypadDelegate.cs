namespace cocos2d
{
    public interface CCKeypadDelegate
    {
        // The back key clicked
        void KeyBackClicked();

        // The menu key clicked. only available on wophone & android
        void KeyMenuClicked();
    }

    public class CCKeypadHandler : CCObject
    {
        protected CCKeypadDelegate m_pDelegate;

        public CCKeypadDelegate Delegate
        {
            get { return m_pDelegate; }
            set { m_pDelegate = value; }
        }

        /** initializes a CCKeypadHandler with a delegate */

        public virtual bool InitWithDelegate(CCKeypadDelegate pDelegate)
        {
            m_pDelegate = pDelegate;
            return true;
        }

        /** allocates a CCKeypadHandler with a delegate */

        public static CCKeypadHandler HandlerWithDelegate(CCKeypadDelegate pDelegate)
        {
            var pHandler = new CCKeypadHandler();
            pHandler.InitWithDelegate(pDelegate);
            return pHandler;
        }
    }
}