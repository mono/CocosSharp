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



using System;
using System.Collections.Generic;

namespace CocosSharp
{
    /** Kinds of possible events for the control objects. */

    [Flags]
    public enum CCControlEvent
    {
		TouchDown = 1 << 0, 		// A touch-down event in the control.
		TouchDragInside = 1 << 1, 	// An event where a finger is dragged inside the bounds of the control.
		TouchDragOutside = 1 << 2, 	// An event where a finger is dragged just outside the bounds of the control. 
		TouchDragEnter = 1 << 3, 	// An event where a finger is dragged into the bounds of the control.
		TouchDragExit = 1 << 4, 	// An event where a finger is dragged from within a control to outside its bounds.
		TouchUpInside = 1 << 5, 	// A touch-up event in the control where the finger is inside the bounds of the control. 
		TouchUpOutside = 1 << 6, 	// A touch-up event in the control where the finger is outside the bounds of the control.
		TouchCancel = 1 << 7, 		// A system event canceling the current touches for the control.
		ValueChanged = 1 << 8 		// A touch dragging or otherwise manipulating a control, causing it to emit a series of different values.
    }

    /** The possible state for a control.  */

    [Flags]
    public enum CCControlState
    {
		Normal = 1 << 0, 			// The normal, or default state of a control that is, enabled but neither selected nor highlighted.
		Highlighted = 1 << 1,		// Highlighted state of a control. A control enters this state when a touch down, drag inside or drag enter is performed. You can retrieve and set this value through the highlighted property.
		Disabled = 1 << 2,        	// Disabled state of a control. This state indicates that the control is currently disabled. You can retrieve and set this value through the enabled property.
		Selected = 1 << 3        	// Selected state of a control. This state indicates that the control is currently selected. You can retrieve and set this value through the selected property.
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

    public class CCControl : CCNode
    {

        #region Events and Handlers

        public class CCControlEventArgs : EventArgs
        {
            public CCControlEventArgs(CCControlEvent controlEvent)
            {
                ControlEvent = controlEvent;
            }

            public CCControlEvent ControlEvent { get; protected internal set; }
        }

        public event EventHandler<CCControlEventArgs> ValueChanged;
        public event EventHandler<CCControlEventArgs> TouchDown;
        public event EventHandler<CCControlEventArgs> TouchDragInside;
        public event EventHandler<CCControlEventArgs> TouchDragOutside;
        public event EventHandler<CCControlEventArgs> TouchDragEnter;
        public event EventHandler<CCControlEventArgs> TouchDragExit;
        public event EventHandler<CCControlEventArgs> TouchUpInside;
        public event EventHandler<CCControlEventArgs> TouchUpOutside;
        public event EventHandler<CCControlEventArgs> TouchCancel;

        #endregion

        bool enabled;
        bool highlighted;
        bool selected;
        bool isColorModifiedByOpacity;

        #region Properties

        public int DefaultTouchPriority { get; set; }   // Changes the priority of the button. The lower the number, the higher the priority.
		public CCControlState State { get; set; }
 
        public override bool IsColorModifiedByOpacity
        {
            get { return isColorModifiedByOpacity; }
            set
            {
                isColorModifiedByOpacity = value;
                
                if (Children != null) 
                {
                    foreach (CCNode child in Children.Elements) 
                    {
                        var item = child;
                        if (item != null) {
                            item.IsColorModifiedByOpacity = value;
                        }
                    }
                }
            }
        }

        public virtual bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                if (enabled)
                {
                    State = CCControlState.Normal;
                }
                else
                {
                    State = CCControlState.Disabled;
                }
                NeedsLayout();
            }
        }

        public virtual bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                NeedsLayout();
            }
        }

        public virtual bool Highlighted
        {
            get { return highlighted; }
            set
            {
                highlighted = value;
                NeedsLayout();
            }
        }

        public bool HasVisibleParents
        {
            get
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

        #endregion Properties

        #region EventHandlers

        protected virtual void OnValueChanged()
        {
            EventHandler<CCControlEventArgs> handler = ValueChanged;
            if (handler != null)
            {
                handler(this, new CCControlEventArgs(CCControlEvent.ValueChanged));
            }
        }

        protected virtual void OnTouchDown()
        {
            EventHandler<CCControlEventArgs> handler = TouchDown;
            if (handler != null)
            {
                handler(this, new CCControlEventArgs(CCControlEvent.TouchDown));
            }
        }

        protected virtual void OnTouchDragInside()
        {
            EventHandler<CCControlEventArgs> handler = TouchDragInside;
            if (handler != null)
            {
                handler(this, new CCControlEventArgs(CCControlEvent.TouchDragInside));
            }
        }

        protected virtual void OnTouchDragOutside()
        {
            EventHandler<CCControlEventArgs> handler = TouchDragOutside;
            if (handler != null)
            {
                handler(this, new CCControlEventArgs(CCControlEvent.TouchDragOutside));
            }
        }

        protected virtual void OnTouchDragEnter()
        {
            EventHandler<CCControlEventArgs> handler = TouchDragEnter;
            if (handler != null)
            {
                handler(this, new CCControlEventArgs(CCControlEvent.TouchDragEnter));
            }
        }

        protected virtual void OnTouchDragExit()
        {
            EventHandler<CCControlEventArgs> handler = TouchDragExit;
            if (handler != null)
            {
                handler(this, new CCControlEventArgs(CCControlEvent.TouchDragExit));
            }
        }

        protected virtual void OnTouchUpInside()
        {
            EventHandler<CCControlEventArgs> handler = TouchUpInside;
            if (handler != null)
            {
                handler(this, new CCControlEventArgs(CCControlEvent.TouchUpInside));
            }
        }

        protected virtual void OnTouchUpOutside()
        {
            EventHandler<CCControlEventArgs> handler = TouchUpOutside;
            if (handler != null)
            {
                handler(this, new CCControlEventArgs(CCControlEvent.TouchUpOutside));
            }
        }

        protected virtual void OnTouchCancel()
        {
            EventHandler<CCControlEventArgs> handler = TouchCancel;
            if (handler != null)
            {
                handler(this, new CCControlEventArgs(CCControlEvent.TouchCancel));
            }
        }

        #endregion

        #region Constructors

        public CCControl()
        {
            State = CCControlState.Normal;
            Enabled = true;
            Selected = false;
            Highlighted = false;
            DefaultTouchPriority = 1;
        }

        #endregion Constructors

        public virtual void NeedsLayout()
        {
        }


        /**
        * Returns a point corresponding to the touh location converted into the 
        * control space coordinates.
        * @param touch A CCTouch object that represents a touch.
        */

        public virtual CCPoint GetTouchLocation(CCTouch touch)
        {
            CCPoint touchLocation = touch.LocationOnScreen; // Get the touch position
            touchLocation = WorldToParentspace(Layer.ScreenToWorldspace(touchLocation)); // Convert to the node space of this class

            return touchLocation;
        }

        /**
        * Returns a boolean value that indicates whether a touch is inside the bounds
        * of the receiver. The given touch must be relative to the world.
        *
        * @param touch A CCTouch object that represents a touch.
        *
        * @return YES whether a touch is inside the receivers rect.
        */

        public virtual bool IsTouchInside(CCTouch touch)
        {
            CCPoint touchLocation = touch.LocationOnScreen;
            touchLocation = Parent.Layer.ScreenToWorldspace(touchLocation);
            CCRect bBox = BoundingBoxTransformedToWorld;
            return bBox.ContainsPoint(touchLocation);
        }

    }
}