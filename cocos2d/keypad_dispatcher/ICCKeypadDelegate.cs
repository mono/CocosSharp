namespace Cocos2D
{
    public interface ICCKeypadDelegate
    {
        // The back key clicked
        void KeyBackClicked();

        // The menu key clicked. only available on wophone & android
        void KeyMenuClicked();
    }

    public class CCKeypadHandler 
    {
        protected ICCKeypadDelegate m_pDelegate;

        public ICCKeypadDelegate Delegate
        {
            get { return m_pDelegate; }
            set { m_pDelegate = value; }
        }

        /** initializes a CCKeypadHandler with a delegate */

        public virtual bool InitWithDelegate(ICCKeypadDelegate pDelegate)
        {
            m_pDelegate = pDelegate;
            return true;
        }

        /** allocates a CCKeypadHandler with a delegate */

        public static CCKeypadHandler HandlerWithDelegate(ICCKeypadDelegate pDelegate)
        {
            var pHandler = new CCKeypadHandler();
            pHandler.InitWithDelegate(pDelegate);
            return pHandler;
        }
    }
}