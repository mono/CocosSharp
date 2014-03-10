using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace CocosSharp
{
	public class CCKeyboardDispatcher
	{
		protected List<CCKeyboardHandler> delegates = new List<CCKeyboardHandler> ();
		protected bool locked;
		protected bool toAdd;
		protected bool toRemove;
		protected List<ICCKeyboardDelegate> handlersToAdd = new List<ICCKeyboardDelegate> ();
		protected List<ICCKeyboardDelegate> handlersToRemove = new List<ICCKeyboardDelegate> ();

		/**
        @brief add delegate to concern keypad msg
        */

		public void AddDelegate (ICCKeyboardDelegate keyboardDelegate)
		{
			if (keyboardDelegate == null) {
				return;
			}

			if (!locked) {
				ForceAddDelegate (keyboardDelegate);
			} else {
				handlersToAdd.Add (keyboardDelegate);
				toAdd = true;
			}
		}

		/**
        @brief remove the delegate from the delegates who concern keypad msg
        */

		public void RemoveDelegate (ICCKeyboardDelegate keyboardDelegate)
		{
			if (keyboardDelegate == null) {
				return;
			}

			if (!locked) {
				ForceRemoveDelegate (keyboardDelegate);
			} else {
				handlersToRemove.Add (keyboardDelegate);
				toRemove = true;
			}
		}

		/**
        @brief force add the delegate
        */

		public void ForceAddDelegate (ICCKeyboardDelegate keyboardDelegate)
		{
			CCKeyboardHandler pHandler = new CCKeyboardHandler (keyboardDelegate);
			delegates.Add (pHandler);
		}

		/**
        @brief force remove the delegate
        */

		public void ForceRemoveDelegate (ICCKeyboardDelegate keyboardDelegate)
		{
			for (int i = 0; i < delegates.Count; i++) {
				if (delegates [i].Delegate == keyboardDelegate) 
				{
					delegates.RemoveAt (i);
					break;
				}
			}
		}


		private KeyboardState priorKeyboardState;

		public bool DispatchKeyboardState ()
		{

			if (delegates.Count == 0)
				return false;

			// Read the current keyboard state
			KeyboardState currentKeyState = Keyboard.GetState();

			// Check for pressed/released keys.
			// Loop for each possible pressed key (those that are pressed this update)
			Keys[] keys = currentKeyState.GetPressedKeys();

			locked = true;

			if (delegates.Count > 0) {
				for (int i = 0; i < delegates.Count; i++) {
					CCKeyboardHandler pHandler = delegates [i];
					ICCKeyboardDelegate pDelegate = pHandler.Delegate;

					if ((pDelegate.KeyboardMode & CCKeyboardMode.KeyPressed) == CCKeyboardMode.KeyPressed) {
						for (int k = 0; k < keys.Length; k++) {
							// Was this key up during the last update?
							if (priorKeyboardState.IsKeyUp (keys [k])) {
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
						keys = priorKeyboardState.GetPressedKeys ();

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

			locked = false;

			if (toRemove) {
				toRemove = false;
				for (int i = 0; i < handlersToRemove.Count; ++i) {
					ForceRemoveDelegate (handlersToRemove [i]);
				}
				handlersToRemove.Clear ();
			}

			if (toAdd) {
				toAdd = false;
				for (int i = 0; i < handlersToAdd.Count; ++i) {
					ForceAddDelegate (handlersToAdd [i]);
				}
				handlersToAdd.Clear ();
			}

			// Store the state for the next loop
			priorKeyboardState = currentKeyState;

			return true;
		}
	
	}
}