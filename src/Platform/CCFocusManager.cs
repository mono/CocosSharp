using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public delegate void CCFocusChangeDelegate(ICCFocusable prev, ICCFocusable current);

    public class CCFocusManager
    {
		// Scrolling focus delay used to slow down automatic focus changes when the dpad is held.
		public static float MenuScrollDelay = 50f;

		static CCFocusManager instance = new CCFocusManager();

		public event CCFocusChangeDelegate OnFocusChanged;

		bool scrollingPrevious = false;
		bool scrollingNext = false;
		long timeOfLastFocus = 0L;

		LinkedList<ICCFocusable> focusList = new LinkedList<ICCFocusable>();
		LinkedListNode<ICCFocusable> current = null;


		#region Properties

		public static CCFocusManager Instance
		{
			get { return instance; }
		}

		// When false, the focus will not traverse on the keyboard or dpad events.
		public bool Enabled { get; set; }

		// Returns the item with the current focus. This test will create a copy 
		// of the master item list.
		public ICCFocusable ItemWithFocus
		{
			get { return (current != null ? current.Value : null); }
		}

		#endregion Properties


		#region Constructors

		private CCFocusManager()
		{
		}

		#endregion Constructors


        // Removes the given focusable node
        public void Remove(params ICCFocusable[] focusItems)
        {
            foreach (ICCFocusable f in focusItems) 
            {
                focusList.Remove(f);
            }
        }

        // Adds the given node to the list of focus nodes. If the node has the focus, then it is
        // given the current focused item status. If there is already a focused item and the
        // given node has focus, the focus is disabled.
        public void Add(params ICCFocusable[] focusItems)
        {
            foreach (ICCFocusable f in focusItems) 
            {
                LinkedListNode<ICCFocusable> i = focusList.AddLast(f);
                if (f.HasFocus) 
                {
                    if (current == null) 
                    {
                        current = i;
                    } 
                    else 
                    {
                        f.HasFocus = false;
                    }
                }
            }
        }

        // Scroll to the next item in the focus list.
        public void FocusNextItem()
        {
            if (current == null && focusList.Count > 0)
            {
                current = focusList.First;
                current.Value.HasFocus = true;
                if (OnFocusChanged != null)
                {
                    OnFocusChanged(null, current.Value);
                }
            }
            else if (current != null)
            {
                ICCFocusable lostItem = current.Value;
                // Search for the next node.
                LinkedListNode<ICCFocusable> nextItem = null;
                for (LinkedListNode<ICCFocusable> p = current.Next; p != null; p = p.Next)
                {
                    if (p.Value.CanReceiveFocus)
                    {
                        nextItem = p;
                    }
                }
                if (nextItem != null)
                {
                    current = nextItem;
                }
                else
                {
                    current = focusList.First;
                }
                lostItem.HasFocus = false;
                current.Value.HasFocus = true;
                if (OnFocusChanged != null)
                {
                    OnFocusChanged(lostItem, current.Value);
                }
            }
            else
            {
                current = null;
            }
        }
			
        // Scroll to the previous item in the focus list.
        public void FocusPreviousItem()
        {
            if (ItemWithFocus == null && focusList.Count > 0)
            {
                current = focusList.Last;
                current.Value.HasFocus = true;
                if (OnFocusChanged != null)
                {
                    OnFocusChanged(null, current.Value);
                }
            }
            else if (current != null)
            {
                ICCFocusable lostItem = current.Value;
                // TODO: take a look at the commented out code it does not seem to do anything.
//                LinkedListNode<ICCFocusable> nextItem = null;
//                for (LinkedListNode<ICCFocusable> p = current.Previous; p != null; p = p.Previous)
//                {
//                    if (p.Value.CanReceiveFocus)
//                    {
//                        nextItem = p;
//                    }
//                }
                if (current.Previous != null)
                {
                    current = current.Previous;
                }
                else
                {
                    current = focusList.Last;
                }
                lostItem.HasFocus = false;
                current.Value.HasFocus = true;
                if (OnFocusChanged != null)
                {
                    OnFocusChanged(lostItem, current.Value);
                }
            }
            else
            {
                current = null;
            }
        }

        void SharedApplication_GamePadDPadUpdate(CCGamePadButtonStatus leftButton, CCGamePadButtonStatus upButton, 
			CCGamePadButtonStatus rightButton, CCGamePadButtonStatus downButton, Microsoft.Xna.Framework.PlayerIndex player)
        {
            if (!Enabled)
            {
                return;
            }
            if (leftButton == CCGamePadButtonStatus.Released || upButton == CCGamePadButtonStatus.Released || rightButton == CCGamePadButtonStatus.Released || downButton == CCGamePadButtonStatus.Released)
            {
                scrollingPrevious = false;
                timeOfLastFocus = 0L;
            }
            // Left and right d-pad shuffle through the menus
            else if (leftButton == CCGamePadButtonStatus.Pressed || upButton == CCGamePadButtonStatus.Pressed)
            {
                if (scrollingPrevious)
                {
                    TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - timeOfLastFocus);
                    if (ts.TotalMilliseconds > MenuScrollDelay)
                    {
                        FocusPreviousItem();
                    }
                }
                else
                {
                    scrollingPrevious = true;
                    timeOfLastFocus = DateTime.UtcNow.Ticks;
                }
            }
            else if (rightButton == CCGamePadButtonStatus.Pressed || downButton == CCGamePadButtonStatus.Pressed)
            {
                if (scrollingNext)
                {
                    TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - timeOfLastFocus);
                    if (ts.TotalMilliseconds > MenuScrollDelay)
                    {
                        FocusNextItem();
                    }
                }
                else
                {
                    scrollingNext = true;
                    timeOfLastFocus = DateTime.UtcNow.Ticks;
                }
            }
        }

    }
}
