using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    public class CCGamePadButtonDispatcher : CCObject
    {
        protected List<CCGamePadButtonHandler> m_pDelegates = new List<CCGamePadButtonHandler>();
        protected bool m_bLocked;
        protected bool m_bToAdd;
        protected bool m_bToRemove;

        protected List<CCGamePadButtonDelegate> m_pHandlersToAdd = new List<CCGamePadButtonDelegate>();
        protected List<CCGamePadButtonDelegate> m_pHandlersToRemove = new List<CCGamePadButtonDelegate>();

        /**
        @brief add delegate to concern keypad msg
        */

        public void AddDelegate(CCGamePadButtonDelegate pDelegate)
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

        public void RemoveDelegate(CCGamePadButtonDelegate pDelegate)
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

        public void ForceAddDelegate(CCGamePadButtonDelegate pDelegate)
        {
            CCGamePadButtonHandler pHandler = CCGamePadButtonHandler.HandlerWithDelegate(pDelegate);
            m_pDelegates.Add(pHandler);
        }

        /**
        @brief force remove the delegate
        */

        public void ForceRemoveDelegate(CCGamePadButtonDelegate pDelegate)
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


        /// <summary>
        /// Dispatch the gamepad state.
        /// </summary>
        /// <param name="state">The current game pad state.</param>
        /// <param name="player">The player to which the game pad state pertains.</param>
        /// <returns>Always returns true.</returns>
        public bool DispatchGamePadState(GamePadState state, PlayerIndex player)
        {
            m_bLocked = true;
            try
            {
                if (m_pDelegates.Count > 0)
                {

                    for (int i = 0; i < m_pDelegates.Count; i++)
                    {
                        CCGamePadButtonHandler pHandler = m_pDelegates[i];
                        CCGamePadButtonDelegate pDelegate = pHandler.Delegate;
                        if (state.Buttons.Back == ButtonState.Pressed)
                        {
                            CCLog.Log("GamePad.Buttons.Back =  " + state.Buttons.Back);
                            pDelegate.BackButtonPressed(player);
                        }
                    }
                }
            }
            finally
            {
                m_bLocked = false;
            }

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
