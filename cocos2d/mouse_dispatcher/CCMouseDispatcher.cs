using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace CocosSharp
{
	public class CCMouseDispatcher
	{
		protected List<CCMouseHandler> delegates = new List<CCMouseHandler> ();
		protected bool locked;
		protected bool toAdd;
		protected bool toRemove;
		protected List<ICCMouseDelegate> handlersToAdd = new List<ICCMouseDelegate> ();
		protected List<ICCMouseDelegate> handlersToRemove = new List<ICCMouseDelegate> ();

		private int lastMouseId;

		/**
        @brief add delegate to concern keypad msg
        */

		public void AddDelegate (ICCMouseDelegate mouseDelegate)
		{
			if (mouseDelegate == null) {
				return;
			}

			if (!locked) {
				ForceAddDelegate (mouseDelegate);
			} else {
				handlersToAdd.Add (mouseDelegate);
				toAdd = true;
			}
		}

		/**
        @brief remove the delegate from the delegates who concern keypad msg
        */

		public void RemoveDelegate (ICCMouseDelegate mouseDelegate)
		{
			if (mouseDelegate == null) {
				return;
			}

			if (!locked) {
				ForceRemoveDelegate (mouseDelegate);
			} else {
				handlersToRemove.Add (mouseDelegate);
				toRemove = true;
			}
		}

		/**
        @brief force add the delegate
        */

		public void ForceAddDelegate (ICCMouseDelegate mouseDelegate)
		{
			CCMouseHandler pHandler = new CCMouseHandler (mouseDelegate);
			delegates.Add (pHandler);
		}

		/**
        @brief force remove the delegate
        */

		public void ForceRemoveDelegate (ICCMouseDelegate mouseDelegate)
		{
			for (int i = 0; i < delegates.Count; i++) {
				if (delegates [i].Delegate == mouseDelegate) 
				{
					delegates.RemoveAt (i);
					break;
				}
			}
		}


		private MouseState priorMouseState;

		public bool DispatchMouseState ()
		{

			if (delegates.Count == 0)
				return false;

			// Read the current keyboard state
			MouseState currentMouseState = Mouse.GetState();

			// Check for pressed/released keys.
			// Loop for each possible pressed key (those that are pressed this update)
			//Keys[] keys = currentKeyState.GetPressedKeys();

			locked = true;

			if (delegates.Count > 0) 
			{
				for (int i = 0; i < delegates.Count; i++) 
				{
					CCMouseHandler handler = delegates [i];
					ICCMouseDelegate mouseDelegate = handler.Delegate;
					CCPoint pos;
					int posX = 0;
					int posY = 0;

#if NETFX_CORE
					pos = TransformPoint(priorMouseState.X, priorMouseState.Y);
					pos = CCDrawManager.ScreenToWorld(pos.X, pos.Y);
#else
					pos = CCDrawManager.ScreenToWorld(priorMouseState.X, priorMouseState.Y);
#endif
					// We will only do the cast once.
					posX = (int)pos.X;
					posY = (int)pos.Y;

					if ((mouseDelegate.MouseMode & CCMouseMode.MouseMove) == CCMouseMode.MouseMove) 
					{
						lastMouseId++;
						mouseDelegate.MouseMove (posX, posY);
					}
				
					if ((mouseDelegate.MouseMode & CCMouseMode.MouseDown) == CCMouseMode.MouseDown)
					{
						CCMouseButton mouseButton = 0;
						if (priorMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed) 
						{
							mouseButton |= CCMouseButton.LeftButton;
						}
						if (priorMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed) 
						{
							mouseButton |= CCMouseButton.RightButton;
						}
						if (priorMouseState.MiddleButton == ButtonState.Released && currentMouseState.MiddleButton == ButtonState.Pressed) 
						{
							mouseButton |= CCMouseButton.MiddleButton;
						}
						if (priorMouseState.XButton1 == ButtonState.Released && currentMouseState.XButton1 == ButtonState.Pressed) 
						{
							mouseButton |= CCMouseButton.ExtraButton1;
						}
						if (priorMouseState.XButton2 == ButtonState.Released && currentMouseState.XButton2 == ButtonState.Pressed) 
						{
							mouseButton |= CCMouseButton.ExtraButton1;
						}

						if (mouseButton > 0)
							mouseDelegate.MouseDown (mouseButton, posX, posY);
					}

					if ((mouseDelegate.MouseMode & CCMouseMode.MouseUp) == CCMouseMode.MouseUp)
					{
						CCMouseButton mouseButton = 0;
						if (priorMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released) 
						{
							mouseButton |= CCMouseButton.LeftButton;
						}
						if (priorMouseState.RightButton == ButtonState.Pressed && currentMouseState.RightButton == ButtonState.Released) 
						{
							mouseButton |= CCMouseButton.RightButton;
						}
						if (priorMouseState.MiddleButton == ButtonState.Pressed && currentMouseState.MiddleButton == ButtonState.Released) 
						{
							mouseButton |= CCMouseButton.MiddleButton;
						}
						if (priorMouseState.XButton1 == ButtonState.Pressed && currentMouseState.XButton1 == ButtonState.Released) 
						{
							mouseButton |= CCMouseButton.ExtraButton1;
						}
						if (priorMouseState.XButton2 == ButtonState.Pressed && currentMouseState.XButton2 == ButtonState.Released) 
						{
							mouseButton |= CCMouseButton.ExtraButton1;
						}

						if (mouseButton > 0)
							mouseDelegate.MouseUp (mouseButton, posX, posY);
					}

					if ((mouseDelegate.MouseMode & CCMouseMode.ScrollWheel) == CCMouseMode.ScrollWheel) 
					{
						var delta = priorMouseState.ScrollWheelValue - currentMouseState.ScrollWheelValue;
						if (delta != 0)
							mouseDelegate.MouseScroll (delta);
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
			priorMouseState = currentMouseState;

			return true;
		}

	}
}