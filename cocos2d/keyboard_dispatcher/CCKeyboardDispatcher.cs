using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Cocos2D
{
	public class CCKeyboardDispatcher
	{
		protected List<CCKeyboardHandler> m_pDelegates = new List<CCKeyboardHandler> ();
		protected bool m_bLocked;
		protected bool m_bToAdd;
		protected bool m_bToRemove;
		protected List<ICCKeyboardDelegate> m_pHandlersToAdd = new List<ICCKeyboardDelegate> ();
		protected List<ICCKeyboardDelegate> m_pHandlersToRemove = new List<ICCKeyboardDelegate> ();

		/**
        @brief add delegate to concern keypad msg
        */

		public void AddDelegate (ICCKeyboardDelegate pDelegate)
		{
			if (pDelegate == null) {
				return;
			}

			if (!m_bLocked) {
				ForceAddDelegate (pDelegate);
			} else {
				m_pHandlersToAdd.Add (pDelegate);
				m_bToAdd = true;
			}
		}

		/**
        @brief remove the delegate from the delegates who concern keypad msg
        */

		public void RemoveDelegate (ICCKeyboardDelegate pDelegate)
		{
			if (pDelegate == null) {
				return;
			}

			if (!m_bLocked) {
				ForceRemoveDelegate (pDelegate);
			} else {
				m_pHandlersToRemove.Add (pDelegate);
				m_bToRemove = true;
			}
		}

		/**
        @brief force add the delegate
        */

		public void ForceAddDelegate (ICCKeyboardDelegate pDelegate)
		{
			CCKeyboardHandler pHandler = new CCKeyboardHandler (pDelegate);
			m_pDelegates.Add (pHandler);
		}

		/**
        @brief force remove the delegate
        */

		public void ForceRemoveDelegate (ICCKeyboardDelegate pDelegate)
		{
			for (int i = 0; i < m_pDelegates.Count; i++) {
				if (m_pDelegates [i].Delegate == pDelegate) {
					m_pDelegates.RemoveAt (i);
					break;
				}
			}
		}


		private KeyboardState m_priorKeyboardState;

		public bool DispatchKeyboardState ()
		{

			if (m_pDelegates.Count == 0)
				return false;

			// Read the current keyboard state
			KeyboardState currentKeyState = Keyboard.GetState();

			// Check for pressed/released keys.
			// Loop for each possible pressed key (those that are pressed this update)
			Keys[] keys = currentKeyState.GetPressedKeys();

			m_bLocked = true;

			if (m_pDelegates.Count > 0) {
				for (int i = 0; i < m_pDelegates.Count; i++) {
					CCKeyboardHandler pHandler = m_pDelegates [i];
					ICCKeyboardDelegate pDelegate = pHandler.Delegate;

					if ((pDelegate.KeyboardMode & CCKeyboardMode.KeyPressed) == CCKeyboardMode.KeyPressed) {
						for (int k = 0; k < keys.Length; k++) {
							// Was this key up during the last update?
							if (m_priorKeyboardState.IsKeyUp (keys [k])) {
								// Yes, so this key has been pressed
								//CCLog.Log("Pressed: " + keys[i].ToString());
								pDelegate.KeyPressed (keys [k]);
							}
						}
					}


					if ((pDelegate.KeyboardMode & CCKeyboardMode.KeyboardState) == CCKeyboardMode.KeyboardState) {
						// Dispatch all pressed kyes if there are any
						if (keys.Length > 0) {

							//CCLog.Log("Pressed Keys: # of keys {0}" + keys.Length);
							pDelegate.KeyboardCurrentState (currentKeyState);
						}
					}

					if ((pDelegate.KeyboardMode & CCKeyboardMode.KeyReleased) == CCKeyboardMode.KeyReleased) {
						// Loop for each possible released key (those that were pressed last update)
						keys = m_priorKeyboardState.GetPressedKeys ();

						for (int k = 0; k < keys.Length; k++) {
							// Is this key now up?
							if (currentKeyState.IsKeyUp (keys [k])) {
								// Yes, so this key has been released
								//CCLog.Log("Released: " + keys[i].ToString());
								pDelegate.KeyReleased (keys [k]);
							}
						}
					}
				}
			}

			m_bLocked = false;

			if (m_bToRemove) {
				m_bToRemove = false;
				for (int i = 0; i < m_pHandlersToRemove.Count; ++i) {
					ForceRemoveDelegate (m_pHandlersToRemove [i]);
				}
				m_pHandlersToRemove.Clear ();
			}

			if (m_bToAdd) {
				m_bToAdd = false;
				for (int i = 0; i < m_pHandlersToAdd.Count; ++i) {
					ForceAddDelegate (m_pHandlersToAdd [i]);
				}
				m_pHandlersToAdd.Clear ();
			}

			// Store the state for the next loop
			m_priorKeyboardState = currentKeyState;

			return true;
		}
	
	}
}