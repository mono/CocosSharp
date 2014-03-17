/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2011-2012 openxlive.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public class CCLayer : CCNode
    {
        bool isAccelerometerEnabled;
        bool isTouchEnabled;
        bool restoreScissor;
        bool noDrawChildren;

        CCTouchMode touchMode;
        CCRenderTexture renderTexture;
        CCRect saveScissorRect;
        CCClipMode childClippingMode;


        #region Properties

        CCEventListener TouchListener { get; set; }

        /// <summary>
        /// Set to true if the child drawing should be isolated in their own render target
        /// </summary>
        public CCClipMode ChildClippingMode
        {
            get { return childClippingMode; }
            set
            {
                if (childClippingMode != value)
                {
                    childClippingMode = value;
                    InitClipping();
                }
            }
        }

        public override CCSize ContentSize
        {
            get { return base.ContentSize; }
            set
            {
                base.ContentSize = value;
                InitClipping();
            }
        }

        public bool AccelerometerEnabled
        {
            get 
            { 
                #if !PSM
                return isAccelerometerEnabled; 
                #else
                return false;
                #endif
            }
            set 
            {
                #if !PSM &&!NETFX_CORE
                if (value != isAccelerometerEnabled)
                {
                    isAccelerometerEnabled = value;

                    if (IsRunning)
                    {
                        CCDirector pDirector = CCDirector.SharedDirector;
                        pDirector.Accelerometer.IsEnabled = value;
                    }
                }
                #else
                isAccelerometerEnabled = false;
                #endif
            }
        }

        public CCTouchMode TouchMode
        {
            get { return touchMode; }
            set
            {
                if (touchMode != value)
                {
                    touchMode = value;

                    if (IsTouchEnabled)
                    {
                        IsTouchEnabled = false;
                        IsTouchEnabled = true;
                    }
                }
            }
        }

        public bool IsTouchEnabled
        {
            get { return isTouchEnabled; }

            set 
            {
                if (value != isTouchEnabled) 
                {

                    isTouchEnabled = value;

                    if (isTouchEnabled) 
                    {
                        if (TouchMode == CCTouchMode.AllAtOnce) 
                        {
                            // Register Touch Event
                            var touchListener = new CCEventListenerTouchAllAtOnce();

                            touchListener.OnTouchesBegan = TouchesBegan;
                            touchListener.OnTouchesMoved = TouchesMoved;
                            touchListener.OnTouchesEnded = TouchesEnded;
                            touchListener.OnTouchesCancelled = TouchesCancelled;

                            EventDispatcher.AddEventListener(touchListener, this);

                            TouchListener = touchListener;
                        } 
                        else 
                        {
                            // Register Touch Event
                            var touchListener = new CCEventListenerTouchOneByOne();
                            touchListener.IsSwallowTouches = true;

                            touchListener.OnTouchBegan = TouchBegan;
                            touchListener.OnTouchMoved = TouchMoved;
                            touchListener.OnTouchEnded = TouchEnded;
                            touchListener.OnTouchCancelled = TouchCancelled;

                            EventDispatcher.AddEventListener(touchListener, this);

                            TouchListener = touchListener;
                        }
                    }
                }
                else
                {
                    EventDispatcher.RemoveEventListener(TouchListener);
                    TouchListener = null;
                }
            }
        }

        #endregion Properties


        #region Constructors

        public CCLayer(CCClipMode clipMode)
        {
            ChildClippingMode = clipMode;

            AnchorPoint = new CCPoint(0.5f, 0.5f);
            IgnoreAnchorPointForPosition = true;

            CCDirector director = CCDirector.SharedDirector;
            if (director != null)
            {
                ContentSize = director.WinSize;
            }
        }

        public CCLayer() : this(CCClipMode.None)
        {
        }

        void InitClipping()
        {
            if (ChildClippingMode == CCClipMode.BoundsWithRenderTarget)
            {
                if (renderTexture == null || renderTexture.ContentSize.Width < ContentSize.Width 
                    || renderTexture.ContentSize.Height < ContentSize.Height)
                {
                    renderTexture = new CCRenderTexture((int)ContentSize.Width, (int)ContentSize.Height);
                    renderTexture.Sprite.AnchorPoint = new CCPoint(0, 0);
                }
                renderTexture.Sprite.TextureRect = new CCRect(0, 0, ContentSize.Width, ContentSize.Height);
            }
            else
            {
                renderTexture = null;
            }
        }

        #endregion Constructors


        public override void OnEnter()
        {
            base.OnEnter();

            CCDirector director = CCDirector.SharedDirector;
            CCApplication application = CCApplication.SharedApplication;

            if (isAccelerometerEnabled)
            {
                #if !PSM &&!NETFX_CORE
                director.Accelerometer.IsEnabled = true;
                #endif
            }
        }

        public override void OnExit()
        {
            if (isAccelerometerEnabled)
            {
                #if !PSM &&!NETFX_CORE
                CCDirector.SharedDirector.Accelerometer.IsEnabled = false;
                #endif
            }

            base.OnExit();
        }


        #region Event handling

        protected virtual void TouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            #if CC_ENABLE_SCRIPT_BINDING
//          if (kScriptTypeLua == _scriptType)
//          {
//          executeScriptTouchesHandler(EventTouch::EventCode::BEGAN, touches, event);
//          return;
//          }
            #endif
        }

        protected virtual void TouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            #if CC_ENABLE_SCRIPT_BINDING
//          if (kScriptTypeLua == _scriptType)
//          {
//          executeScriptTouchesHandler(EventTouch::EventCode::MOVED, touches, event);
//          return;
//          }
            #endif

        }

        protected virtual void TouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {
            #if CC_ENABLE_SCRIPT_BINDING
//          if (kScriptTypeLua == _scriptType)
//          {
//          executeScriptTouchesHandler(EventTouch::EventCode::ENDED, touches, event);
//          return;
//          }
            #endif
        }

        protected virtual void TouchesCancelled(List<CCTouch> touches, CCEvent touchEvent)
        {
            #if CC_ENABLE_SCRIPT_BINDING
//          if (kScriptTypeLua == _scriptType)
//          {
//          executeScriptTouchesHandler(EventTouch::EventCode::CANCELLED, touches, event);
//          return;
//          }
            #endif
        }

        // Layer touch callbacks

        protected virtual bool TouchBegan(CCTouch touch, CCEvent touchEvent)
        {
            #if CC_ENABLE_SCRIPT_BINDING
//          if (kScriptTypeLua == _scriptType)
//          {
//          return executeScriptTouchHandler(EventTouch::EventCode::BEGAN, touch, event) == 0 ? false : true;
//          }
            #endif
            Debug.Assert(false, "Layer#TouchBegan override me");
            return true;

        }

        protected virtual void TouchMoved(CCTouch touch, CCEvent touchEvent)
        {
            #if CC_ENABLE_SCRIPT_BINDING
//          if (kScriptTypeLua == _scriptType)
//          {
//          executeScriptTouchHandler(EventTouch::EventCode::MOVED, touch, event);
//          return;
//          }
            #endif

        }

        protected virtual void TouchEnded(CCTouch touch, CCEvent touchEvent)
        {
            #if CC_ENABLE_SCRIPT_BINDING
//          if (kScriptTypeLua == _scriptType)
//          {
//          executeScriptTouchHandler(EventTouch::EventCode::ENDED, touch, event);
//          return;
//          }
            #endif

        }

        protected virtual void TouchCancelled(CCTouch touch, CCEvent touchEvent)
        {
            #if CC_ENABLE_SCRIPT_BINDING
//          if (kScriptTypeLua == _scriptType)
//          {
//          executeScriptTouchHandler(EventTouch::EventCode::CANCELLED, touch, event);
//          return;
//          }
            #endif

        }

        #endregion Event handling


        #region Visiting and drawing

        public override void Visit()
        {
            // quick return if not visible
            if (!Visible)
            {
                return;
            }
            if (ChildClippingMode == CCClipMode.None)
            {
                base.Visit();
                return;
            }

            CCDrawManager.PushMatrix();

            if (Grid != null && Grid.Active)
            {
                Grid.BeforeDraw();
                TransformAncestors();
            }

            Transform();

            BeforeDraw();

            if (!noDrawChildren && Children != null)
            {
                SortAllChildren();

                CCNode[] arrayData = Children.Elements;
                int count = Children.Count;
                int i = 0;

                // draw children zOrder < 0
                for (; i < count; i++)
                {
                    CCNode child = arrayData[i];
                    if (child.ZOrder < 0)
                    {
                        child.Visit();
                    }
                    else
                    {
                        break;
                    }
                }

                Draw();

                // draw children zOrder >= 0
                for (; i < count; i++)
                {
                    arrayData[i].Visit();
                }
            }
            else
            {
                Draw();
            }

            AfterDraw();

            if (Grid != null && Grid.Active)
            {
                Grid.AfterDraw(this);
            }

            CCDrawManager.PopMatrix();
        }

        void BeforeDraw()
        {
            noDrawChildren = false;

            if (ChildClippingMode == CCClipMode.Bounds)
            {
                // We always clip to the bounding box
                CCSize contentSize = ContentSize;
                var rect = new CCRect(0, 0, contentSize.Width, contentSize.Height);
                var bounds = CCAffineTransform.Transform(rect, NodeToWorldTransform());

                var winSize = CCDirector.SharedDirector.WinSize;

                CCRect prevScissorRect;
                if (CCDrawManager.ScissorRectEnabled)
                {
                    prevScissorRect = CCDrawManager.ScissorRect;
                }
                else
                {
                    prevScissorRect = new CCRect(0, 0, winSize.Width, winSize.Height);
                }

                if (!bounds.IntersectsRect(prevScissorRect))
                {
                    noDrawChildren = true;
                    return;
                }

                float minX = Math.Max(bounds.MinX, prevScissorRect.MinX);
                float minY = Math.Max(bounds.MinY, prevScissorRect.MinY);
                float maxX = Math.Min(bounds.MaxX, prevScissorRect.MaxX);
                float maxY = Math.Min(bounds.MaxY, prevScissorRect.MaxY);

                if (CCDrawManager.ScissorRectEnabled)
                {
                    restoreScissor = true;
                }
                else
                {
                    CCDrawManager.ScissorRectEnabled = true;
                }

                saveScissorRect = prevScissorRect;

                CCDrawManager.SetScissorInPoints(minX, minY, maxX - minX, maxY - minY);
            }
            else if (ChildClippingMode == CCClipMode.BoundsWithRenderTarget)
            {
                saveScissorRect = CCDrawManager.ScissorRect;
                restoreScissor = CCDrawManager.ScissorRectEnabled;

                CCDrawManager.ScissorRectEnabled = false;

                CCDrawManager.PushMatrix();
                CCDrawManager.SetIdentityMatrix();

                renderTexture.BeginWithClear(0, 0, 0, 0);
            }
        }

        void AfterDraw()
        {
            if (ChildClippingMode != CCClipMode.None)
            {
                if (ChildClippingMode == CCClipMode.BoundsWithRenderTarget)
                {
                    renderTexture.End();

                    CCDrawManager.PopMatrix();
                }

                if (restoreScissor)
                {
                    CCDrawManager.SetScissorInPoints(
                        saveScissorRect.Origin.X, saveScissorRect.Origin.Y,
                        saveScissorRect.Size.Width, saveScissorRect.Size.Height);

                    CCDrawManager.ScissorRectEnabled = true;

                    restoreScissor = false;
                }
                else
                {
                    CCDrawManager.ScissorRectEnabled = false;
                }

                if (ChildClippingMode == CCClipMode.BoundsWithRenderTarget)
                {
                    renderTexture.Sprite.Visit();
                }
            }
        }

        #endregion Visiting and drawing
    }
}