using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public enum CCKeypadMSGType
    {
        // the back key clicked msg
        BackClicked = 1,
        MenuClicked,
    }

    public class CCKeypadDispatcher 
    {
        protected List<CCKeypadHandler> delegates = new List<CCKeypadHandler>();
        protected bool isLocked;
        protected bool isToAdd;
        protected bool isToRemove;

        protected List<ICCKeypadDelegate> handlersToAdd = new List<ICCKeypadDelegate>();
        protected List<ICCKeypadDelegate> handlersToRemove = new List<ICCKeypadDelegate>();

        /**
        @brief add delegate to concern keypad msg
        */

        public void AddDelegate(ICCKeypadDelegate keyPadDelegate)
        {
            if (keyPadDelegate == null)
            {
                return;
            }

            if (!isLocked)
            {
                ForceAddDelegate(keyPadDelegate);
            }
            else
            {
                handlersToAdd.Add(keyPadDelegate);
                isToAdd = true;
            }
        }

        /**
        @brief remove the delegate from the delegates who concern keypad msg
        */

        public void RemoveDelegate(ICCKeypadDelegate keypadDelegate)
        {
            if (keypadDelegate == null)
            {
                return;
            }

            if (!isLocked)
            {
                ForceRemoveDelegate(keypadDelegate);
            }
            else
            {
                handlersToRemove.Add(keypadDelegate);
                isToRemove = true;
            }
        }

        /**
        @brief force add the delegate
        */

        public void ForceAddDelegate(ICCKeypadDelegate keypadDelegate)
        {
            CCKeypadHandler pHandler = CCKeypadHandler.HandlerWithDelegate(keypadDelegate);
            delegates.Add(pHandler);
        }

        /**
        @brief force remove the delegate
        */

        public void ForceRemoveDelegate(ICCKeypadDelegate pDelegate)
        {
            for (int i = 0; i < delegates.Count; i++)
            {
                if (delegates[i].Delegate == pDelegate)
                {
                    delegates.RemoveAt(i);
                    break;
                }
            }
        }

        /**
        @brief dispatch the key pad msg
        */

        public bool DispatchKeypadMsg(CCKeypadMSGType keypadMsgType)
        {
            isLocked = true;

            if (delegates.Count > 0)
            {
                for (int i = 0; i < delegates.Count; i++)
                {
                    CCKeypadHandler pHandler = delegates[i];
                    ICCKeypadDelegate pDelegate = pHandler.Delegate;

                    switch (keypadMsgType)
                    {
                        case CCKeypadMSGType.BackClicked:
                            pDelegate.KeyBackClicked();
                            break;

                        case CCKeypadMSGType.MenuClicked:
                            pDelegate.KeyMenuClicked();
                            break;
                    }
                }
            }

            isLocked = false;

            if (isToRemove)
            {
                isToRemove = false;
                for (int i = 0; i < handlersToRemove.Count; ++i)
                {
                    ForceRemoveDelegate(handlersToRemove[i]);
                }
                handlersToRemove.Clear();
            }

            if (isToAdd)
            {
                isToAdd = false;
                for (int i = 0; i < handlersToAdd.Count; ++i)
                {
                    ForceAddDelegate(handlersToAdd[i]);
                }
                handlersToAdd.Clear();
            }

            return true;
        }
    }
}
