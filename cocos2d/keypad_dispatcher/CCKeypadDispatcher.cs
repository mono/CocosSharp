using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public enum KeypadMSGType
    {
        // the back key clicked msg
        BackClicked = 1,
        MenuClicked,
    }

    public class CCKeypadDispatcher : CCObject
    {
        protected List<CCKeypadHandler> m_pDelegates = new List<CCKeypadHandler>();
        protected bool m_bLocked;
        protected bool m_bToAdd;
        protected bool m_bToRemove;

        protected List<CCKeypadDelegate> m_pHandlersToAdd = new List<CCKeypadDelegate>();
        protected List<CCKeypadDelegate> m_pHandlersToRemove = new List<CCKeypadDelegate>();

        /**
        @brief add delegate to concern keypad msg
        */

        public void AddDelegate(CCKeypadDelegate pDelegate)
        {
            if (pDelegate == null)
            {
                return;
            }

            if (!m_bLocked)
            {
                ForceAddDelegate(pDelegate);
            }
            else
            {
                m_pHandlersToAdd.Add(pDelegate);
                m_bToAdd = true;
            }
        }

        /**
        @brief remove the delegate from the delegates who concern keypad msg
        */

        public void RemoveDelegate(CCKeypadDelegate pDelegate)
        {
            if (pDelegate == null)
            {
                return;
            }

            if (!m_bLocked)
            {
                ForceRemoveDelegate(pDelegate);
            }
            else
            {
                m_pHandlersToRemove.Add(pDelegate);
                m_bToRemove = true;
            }
        }

        /**
        @brief force add the delegate
        */

        public void ForceAddDelegate(CCKeypadDelegate pDelegate)
        {
            CCKeypadHandler pHandler = CCKeypadHandler.HandlerWithDelegate(pDelegate);
            m_pDelegates.Add(pHandler);
        }

        /**
        @brief force remove the delegate
        */

        public void ForceRemoveDelegate(CCKeypadDelegate pDelegate)
        {
            for (int i = 0; i < m_pDelegates.Count; i++)
            {
                if (m_pDelegates[i].Delegate == pDelegate)
                {
                    m_pDelegates.RemoveAt(i);
                    break;
                }
            }
        }

        /**
        @brief dispatch the key pad msg
        */

        public bool DispatchKeypadMsg(KeypadMSGType nMsgType)
        {
            m_bLocked = true;

            if (m_pDelegates.Count > 0)
            {
                for (int i = 0; i < m_pDelegates.Count; i++)
                {
                    CCKeypadHandler pHandler = m_pDelegates[i];
                    CCKeypadDelegate pDelegate = pHandler.Delegate;

                    switch (nMsgType)
                    {
                        case KeypadMSGType.BackClicked:
                            pDelegate.KeyBackClicked();
                            break;

                        case KeypadMSGType.MenuClicked:
                            pDelegate.KeyMenuClicked();
                            break;
                    }
                }
            }

            m_bLocked = false;

            if (m_bToRemove)
            {
                m_bToRemove = false;
                for (int i = 0; i < m_pHandlersToRemove.Count; ++i)
                {
                    ForceRemoveDelegate(m_pHandlersToRemove[i]);
                }
                m_pHandlersToRemove.Clear();
            }

            if (m_bToAdd)
            {
                m_bToAdd = false;
                for (int i = 0; i < m_pHandlersToAdd.Count; ++i)
                {
                    ForceAddDelegate(m_pHandlersToAdd[i]);
                }
                m_pHandlersToAdd.Clear();
            }

            return true;
        }
    }
}
