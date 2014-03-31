/*
 * CCControl.h
 *
 * Copyright 2011 Yannick Loriot.
 * http://yannickloriot.com
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 *
 * Converted to c++ / cocos2d-x by Angus C
 */


/**
 * @addtogroup GUI
 * @{
 * @addtogroup control_extension
 * @{
 */

using System;
using System.Collections.Generic;

namespace CocosSharp
{
    /** Kinds of possible events for the control objects. */

    [Flags]
    public enum CCControlEvent
    {
        TouchDown = 1 << 0, // A touch-down event in the control.
        TouchDragInside = 1 << 1, // An event where a finger is dragged inside the bounds of the control.
        TouchDragOutside = 1 << 2, // An event where a finger is dragged just outside the bounds of the control. 
        TouchDragEnter = 1 << 3, // An event where a finger is dragged into the bounds of the control.
        TouchDragExit = 1 << 4, // An event where a finger is dragged from within a control to outside its bounds.
        TouchUpInside = 1 << 5, // A touch-up event in the control where the finger is inside the bounds of the control. 
        TouchUpOutside = 1 << 6, // A touch-up event in the control where the finger is outside the bounds of the control.
        TouchCancel = 1 << 7, // A system event canceling the current touches for the control.
        ValueChanged = 1 << 8 // A touch dragging or otherwise manipulating a control, causing it to emit a series of different values.
    }

    /** The possible state for a control.  */

    [Flags]
    public enum CCControlState
    {
        Normal = 1 << 0, // The normal, or default state of a control�that is, enabled but neither selected nor highlighted.
        Highlighted = 1 << 1,
        // Highlighted state of a control. A control enters this state when a touch down, drag inside or drag enter is performed. You can retrieve and set this value through the highlighted property.
        Disabled = 1 << 2,
        // Disabled state of a control. This state indicates that the control is currently disabled. You can retrieve and set this value through the enabled property.
        Selected = 1 << 3
        // Selected state of a control. This state indicates that the control is currently selected. You can retrieve and set this value through the selected property.
    }

    /*
     * @class
     * CCControl is inspired by the UIControl API class from the UIKit library of 
     * CocoaTouch. It provides a base class for control CCSprites such as CCButton 
     * or CCSlider that convey user intent to the application.
     *
     * The goal of CCControl is to define an interface and base implementation for 
     * preparing action messages and initially dispatching them to their targets when
     * certain events occur.
     *
     * To use the CCControl you have to subclass it.
     */

    public class CCControl : CCLayerRGBA
    {
        /** Number of kinds of control event. */
        private const int ControlEventTotalNumber = 9;
        protected bool _enabled;
        protected bool _highlighted;
        protected bool _selected;
        protected CCControlState _state;
        protected bool _hasVisibleParents;

        private bool _isOpacityModifyRGB;

        /** Changes the priority of the button. The lower the number, the higher the priority. */
        private int _defaultTouchPriority;
        protected Dictionary<CCControlEvent, CCRawList<CCInvocation>> _dispatchTable;

        public int DefaultTouchPriority
        {
            get { return _defaultTouchPriority; }
            set { _defaultTouchPriority = value; }
        }

        /** The current control state constant. */

        public CCControlState State
        {
            get { return _state; }
            set { _state = value; }
        }

        #region RGBA Protocol

        public override bool IsColorModifiedByOpacity
        {
            get { return _isOpacityModifyRGB; }
            set
            {
                _isOpacityModifyRGB = value;
                
						if (Children != null && Children.count > 0)
                {
							for (int i = 0, count = Children.count; i < count; i++)
                    {
								var item = Children.Elements[i] as ICCColor;
                        if (item != null)
                        {
                            item.IsColorModifiedByOpacity = value;
                        }
                    }
                }
            }
        }

        #endregion

        /** Tells whether the control is enabled. */

        public virtual bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (_enabled)
                {
                    _state = CCControlState.Normal;
                }
                else
                {
                    _state = CCControlState.Disabled;
                }
                NeedsLayout();
            }
        }

        /** A Boolean value that determines the control selected state. */

        public virtual bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                NeedsLayout();
            }
        }

        /** A Boolean value that determines whether the control is highlighted. */

        public virtual bool Highlighted
        {
            get { return _highlighted; }
            set
            {
                _highlighted = value;
                NeedsLayout();
            }
        }

        
        #region Constructors

        public CCControl()
        {
            InitCCControl();
        }

        private void InitCCControl()
        {
            //this->setTouchEnabled(true);
            //m_bIsTouchEnabled=true;
            // Initialise instance variables
            _state = CCControlState.Normal;
            Enabled = true;
            Selected = false;
            Highlighted = false;

            // Set the touch dispatcher priority by default to 1
            DefaultTouchPriority = 1;
            // Initialise the tables
            _dispatchTable = new Dictionary<CCControlEvent, CCRawList<CCInvocation>>();
        }

        #endregion Constructors


        /**
         * Sends action messages for the given control events.
         *
         * @param controlEvents A bitmask whose set flags specify the control events for
         * which action messages are sent. See "CCControlEvent" for bitmask constants.
         */

        public virtual void SendActionsForControlEvents(CCControlEvent controlEvents)
        {
            // For each control events
            for (int i = 0; i < ControlEventTotalNumber; i++)
            {
                // If the given controlEvents bitmask contains the curent event
                if ((controlEvents & (CCControlEvent) (1 << i)) != 0)
                {
                    // Call invocations
                    // <CCInvocation*>

                    CCRawList<CCInvocation> invocationList = DispatchListforControlEvent((CCControlEvent) (1 << i));
                    foreach (CCInvocation invocation in invocationList)
                    {
                        invocation.Invoke(this);
                    }
                }
            }
        }

        /**
        * Adds a target and action for a particular event (or events) to an internal
        * dispatch table.
        * The action message may optionnaly include the sender and the event as 
        * parameters, in that order.
        * When you call this method, target is not retained.
        *
        * @param target The target object�that is, the object to which the action 
        * message is sent. It cannot be nil. The target is not retained.
        * @param action A selector identifying an action message. It cannot be NULL.
        * @param controlEvents A bitmask specifying the control events for which the 
        * action message is sent. See "CCControlEvent" for bitmask constants.
        */

		public virtual void AddTargetWithActionForControlEvents(object target, Action<object, CCControlEvent> action, CCControlEvent controlEvents)
        {
            // For each control events
            for (int i = 0; i < ControlEventTotalNumber; i++)
            {
                // If the given controlEvents bitmask contains the curent event
                if (((int) controlEvents & (1 << i)) != 0)
                {
                    AddTargetWithActionForControlEvent(target, action, (CCControlEvent) (1 << i));
                }
            }
        }

        /**
        * Removes a target and action for a particular event (or events) from an 
        * internal dispatch table.
        *
        * @param target The target object�that is, the object to which the action 
        * message is sent. Pass nil to remove all targets paired with action and the
        * specified control events.
        * @param action A selector identifying an action message. Pass NULL to remove
        * all action messages paired with target.
        * @param controlEvents A bitmask specifying the control events associated with
        * target and action. See "CCControlEvent" for bitmask constants.
        */

		public virtual void RemoveTargetWithActionForControlEvents(object target, Action<object, CCControlEvent> action, CCControlEvent controlEvents)
        {
            // For each control events
            for (int i = 0; i < ControlEventTotalNumber; i++)
            {
                // If the given controlEvents bitmask contains the curent event
                if ((controlEvents & (CCControlEvent) (1 << i)) != 0)
                {
                    RemoveTargetWithActionForControlEvent(target, action, (CCControlEvent) (1 << i));
                }
            }
        }

        /**
        * Returns a point corresponding to the touh location converted into the 
        * control space coordinates.
        * @param touch A CCTouch object that represents a touch.
        */

        public virtual CCPoint GetTouchLocation(CCTouch touch)
        {
            CCPoint touchLocation = touch.Location; // Get the touch position
            touchLocation = ConvertToNodeSpace(touchLocation); // Convert to the node space of this class

            return touchLocation;
        }

        /**
        * Returns a boolean value that indicates whether a touch is inside the bounds
        * of the receiver. The given touch must be relative to the world.
        *
        * @param touch A CCTouch object that represents a touch.
        *
        * @return YES whether a touch is inside the receiver�s rect.
        */

        public virtual bool IsTouchInside(CCTouch touch)
        {
            CCPoint touchLocation = touch.Location;
            touchLocation = Parent.ConvertToNodeSpace(touchLocation);
            CCRect bBox = BoundingBox;
            return bBox.ContainsPoint(touchLocation);
        }

        /**
         * Returns an CCInvocation object able to construct messages using a given 
         * target-action pair. (The invocation may optionnaly include the sender and
         * the event as parameters, in that order)
         *
         * @param target The target object.
         * @param action A selector identifying an action message.
         * @param controlEvent A control events for which the action message is sent.
         * See "CCControlEvent" for constants.
         *
         * @return an CCInvocation object able to construct messages using a given 
         * target-action pair.
         */
        //protected CCInvocation invocationWithTargetAndActionForControlEvent(object target, SEL_CCControlHandler action, CCControlEvent controlEvent)
        //{
        //}


        /**
        * Returns the CCInvocation list for the given control event. If the list does
        * not exist, it'll create an empty array before returning it.
        *
        * @param controlEvent A control events for which the action message is sent.
        * See "CCControlEvent" for constants.
        *
        * @return the CCInvocation list for the given control event.
        */
        //<CCInvocation*>
        protected CCRawList<CCInvocation> DispatchListforControlEvent(CCControlEvent controlEvent)
        {
            CCRawList<CCInvocation> invocationList;
            if (!_dispatchTable.TryGetValue(controlEvent, out invocationList))
            {
                invocationList = new CCRawList<CCInvocation>(1);

                _dispatchTable.Add(controlEvent, invocationList);
            }
            return invocationList;
        }


		public void AddTargetWithActionForControlEvent(object target, Action<object, CCControlEvent> action, CCControlEvent controlEvent)
        {
            // Create the invocation object
            var invocation = new CCInvocation(target, action, controlEvent);

            // Add the invocation into the dispatch list for the given control event
            CCRawList<CCInvocation> eventInvocationList = DispatchListforControlEvent(controlEvent);
            eventInvocationList.Add(invocation);
        }

		public void RemoveTargetWithActionForControlEvent(object target, Action<object, CCControlEvent> action, CCControlEvent controlEvent)
        {
            // Retrieve all invocations for the given control event
            //<CCInvocation*>
            CCRawList<CCInvocation> eventInvocationList = DispatchListforControlEvent(controlEvent);

            //remove all invocations if the target and action are null
            if (target == null && action == null)
            {
                //remove objects
                eventInvocationList.Clear();
            }
            else
            {
                //normally we would use a predicate, but this won't work here. Have to do it manually
                foreach (CCInvocation invocation in eventInvocationList)
                {
                    bool shouldBeRemoved = true;
                    if (target != null)
                    {
                        shouldBeRemoved = (target == invocation.Target);
                    }
                    if (action != null)
                    {
                        shouldBeRemoved = (shouldBeRemoved && (action == invocation.Action));
                    }
                    // Remove the corresponding invocation object
                    if (shouldBeRemoved)
                    {
                        eventInvocationList.Remove(invocation);
                    }
                }
            }
        }

        public virtual void NeedsLayout()
        {
        }

        public bool HasVisibleParents()
        {
            CCNode parent = Parent;
            for (CCNode c = parent; c != null; c = c.Parent)
            {
                if (!c.Visible)
                {
                    return false;
                }
            }
            return true;
        }
    }
}