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

namespace Cocos2D
{
    public class CCLayer : CCNode, ICCTargetedTouchDelegate, ICCStandardTouchDelegate, ICCAccelerometerDelegate, CCKeypadDelegate
    {
        private bool m_bIsAccelerometerEnabled;
        private bool m_bKeypadEnabled;
        private bool m_bGamePadEnabled;
        private bool m_bIsMultiTouchEnabled;
        private bool m_bIsSingleTouchEnabled;

        private CCRenderTexture m_pRenderTexture;
        private bool m_bRestoreScissor;
        private CCRect m_tSaveScissorRect;
        private bool m_bNoDrawChildren;

        //private bool m_bMouseEnabled;
        //private bool m_bGamePadEnabled;
        /// <summary>
        /// Set to true if the child drawing should be isolated in their own render target
        /// </summary>
        protected CCClipMode m_childClippingMode = CCClipMode.ClipNone;

        public CCLayer(CCClipMode clipMode)
        {
            m_childClippingMode = clipMode;
            AnchorPoint = new CCPoint(0.5f, 0.5f);
            m_bIgnoreAnchorPointForPosition = true;
            CCDirector director = CCDirector.SharedDirector;
            if (director != null)
            {
                ContentSize = director.WinSize;
            }
            m_OnGamePadButtonUpdateDelegate = new CCGamePadButtonDelegate(OnGamePadButtonUpdate);
            m_OnGamePadConnectionUpdateDelegate = new CCGamePadConnectionDelegate(OnGamePadConnectionUpdate);
            m_OnGamePadDPadUpdateDelegate = new CCGamePadDPadDelegate(OnGamePadDPadUpdate);
            m_OnGamePadStickUpdateDelegate = new CCGamePadStickUpdateDelegate(OnGamePadStickUpdate);
            m_OnGamePadTriggerUpdateDelegate = new CCGamePadTriggerDelegate(OnGamePadTriggerUpdate);
            Init();
        }

        /// <summary>
        /// Default layer constructor that does not use clipping.
        /// </summary>
        public CCLayer() : this(CCClipMode.ClipNone)
        {
        }


        public CCClipMode ChildClippingMode
        {
            get { return m_childClippingMode; }
            set
            {
                if (m_childClippingMode != value)
                {
                    m_childClippingMode = value;
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

        private bool m_bDidInit = false;

        public override void Visit()
        {
            // quick return if not visible
            if (!Visible)
            {
                return;
            }
            if (m_childClippingMode == CCClipMode.ClipNone)
            {
                base.Visit();
                return;
            }

            CCDrawManager.PushMatrix();

            if (m_pGrid != null && m_pGrid.Active)
            {
                m_pGrid.BeforeDraw();
                TransformAncestors();
            }

            Transform();

            BeforeDraw();

            if (!m_bNoDrawChildren && m_pChildren != null)
            {
                SortAllChildren();

                CCNode[] arrayData = m_pChildren.Elements;
                int count = m_pChildren.count;
                int i = 0;

                // draw children zOrder < 0
                for (; i < count; i++)
                {
                    CCNode child = arrayData[i];
                    if (child.m_nZOrder < 0)
                    {
                        child.Visit();
                    }
                    else
                    {
                        break;
                    }
                }

                // this draw
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

            if (m_pGrid != null && m_pGrid.Active)
            {
                m_pGrid.AfterDraw(this);
            }

            CCDrawManager.PopMatrix();
        }

        private void InitClipping()
        {
            if (m_childClippingMode == CCClipMode.ClipBoundsWithRenderTarget)
            {
                if (m_pRenderTexture == null || m_pRenderTexture.ContentSize.Width < ContentSize.Width || m_pRenderTexture.ContentSize.Height < ContentSize.Height)
                {
                    m_pRenderTexture = new CCRenderTexture((int)ContentSize.Width, (int)ContentSize.Height);
                    m_pRenderTexture.Sprite.AnchorPoint = new CCPoint(0, 0);
                }
                m_pRenderTexture.Sprite.TextureRect = new CCRect(0, 0, ContentSize.Width, ContentSize.Height);
            }
            else
            {
                m_pRenderTexture = null;
            }
        }

        private void BeforeDraw()
        {
            m_bNoDrawChildren = false;

            if (m_childClippingMode == CCClipMode.ClipBounds)
            {
                // We always clip to the bounding box
                var rect = new CCRect(0, 0, m_tContentSize.Width, m_tContentSize.Height);
                var bounds = CCAffineTransform.CCRectApplyAffineTransform(rect, NodeToWorldTransform());

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
                    m_bNoDrawChildren = true;
                    return;
                }

                float minX = Math.Max(bounds.MinX, prevScissorRect.MinX);
                float minY = Math.Max(bounds.MinY, prevScissorRect.MinY);
                float maxX = Math.Min(bounds.MaxX, prevScissorRect.MaxX);
                float maxY = Math.Min(bounds.MaxY, prevScissorRect.MaxY);
              
                if (CCDrawManager.ScissorRectEnabled)
                {
                    m_bRestoreScissor = true;
                }
                else
                {
                    CCDrawManager.ScissorRectEnabled = true;
                }

                m_tSaveScissorRect = prevScissorRect;

                CCDrawManager.SetScissorInPoints(minX, minY, maxX - minX, maxY - minY);
            }
            else if (m_childClippingMode == CCClipMode.ClipBoundsWithRenderTarget)
            {
                m_tSaveScissorRect = CCDrawManager.ScissorRect;
                m_bRestoreScissor = CCDrawManager.ScissorRectEnabled;

                CCDrawManager.ScissorRectEnabled = false;

                CCDrawManager.PushMatrix();
                CCDrawManager.SetIdentityMatrix();

                m_pRenderTexture.BeginWithClear(0, 0, 0, 0);
            }
        }

        /**
     * retract what's done in beforeDraw so that there's no side effect to
     * other nodes.
     */
        private void AfterDraw()
        {
            if (m_childClippingMode != CCClipMode.ClipNone)
            {
                if (m_childClippingMode == CCClipMode.ClipBoundsWithRenderTarget)
                {
                    m_pRenderTexture.End();

                    CCDrawManager.PopMatrix();
                }

                if (m_bRestoreScissor)
                {
                    CCDrawManager.SetScissorInPoints(
                        m_tSaveScissorRect.Origin.X, m_tSaveScissorRect.Origin.Y,
                        m_tSaveScissorRect.Size.Width, m_tSaveScissorRect.Size.Height);

                    CCDrawManager.ScissorRectEnabled = true;

                    m_bRestoreScissor = false;
                }
                else
                {
                    CCDrawManager.ScissorRectEnabled = false;
                }

                if (m_childClippingMode == CCClipMode.ClipBoundsWithRenderTarget)
                {
                    m_pRenderTexture.Sprite.Visit();
                }
            }
        }

        public virtual bool Init()
        {
            if (m_bDidInit)
            {
                return (true);
            }
            bool bRet = false;
            CCDirector director = CCDirector.SharedDirector;
            if (director != null)
            {
                //                ContentSize = director.WinSize;
                m_bIsMultiTouchEnabled = false;
                m_bIsSingleTouchEnabled = false;
                m_bIsAccelerometerEnabled = false;
                m_bKeypadEnabled = false;
                m_bGamePadEnabled = false;
                bRet = true;
                m_bDidInit = true;
            }
            return bRet;
        }

        public override void OnEnter()
        {
            if(!m_bDidInit) {
                Init();
            }
            // register 'parent' nodes first
            // since events are propagated in reverse order
            if (m_bIsMultiTouchEnabled || m_bIsSingleTouchEnabled)
            {
                RegisterWithTouchDispatcher();
            }

            // then iterate over all the children
            base.OnEnter();

            CCDirector director = CCDirector.SharedDirector;
            CCApplication application = CCApplication.SharedApplication;

            // add this layer to concern the Accelerometer Sensor
            if (m_bIsAccelerometerEnabled)
            {
#if !PSM &&!NETFX_CORE
                director.Accelerometer.SetDelegate(this);
#endif
			}
            // add this layer to concern the kaypad msg
            if (m_bKeypadEnabled)
            {
                director.KeypadDispatcher.AddDelegate(this);
            }

            if (GamePadEnabled && director.GamePadEnabled)
            {
                application.GamePadButtonUpdate += m_OnGamePadButtonUpdateDelegate;
                application.GamePadConnectionUpdate += m_OnGamePadConnectionUpdateDelegate;
                application.GamePadDPadUpdate += m_OnGamePadDPadUpdateDelegate;
                application.GamePadStickUpdate += m_OnGamePadStickUpdateDelegate;
                application.GamePadTriggerUpdate += m_OnGamePadTriggerUpdateDelegate;
            }
        }

        public override void OnExit()
        {
            CCDirector director = CCDirector.SharedDirector;
            CCApplication application = CCApplication.SharedApplication;

            if (m_bIsMultiTouchEnabled || m_bIsSingleTouchEnabled)
            {
                director.TouchDispatcher.RemoveDelegate(this);
                //unregisterScriptTouchHandler();
            }

            // remove this layer from the delegates who concern Accelerometer Sensor
            if (m_bIsAccelerometerEnabled)
            {
                //director.Accelerometer.setDelegate(null);
            }

            // remove this layer from the delegates who concern the kaypad msg
            if (m_bKeypadEnabled)
            {
                director.KeypadDispatcher.RemoveDelegate(this);
            }

            if (GamePadEnabled && director.GamePadEnabled)
            {
                application.GamePadButtonUpdate -= m_OnGamePadButtonUpdateDelegate;
                application.GamePadConnectionUpdate -= m_OnGamePadConnectionUpdateDelegate;
                application.GamePadDPadUpdate -= m_OnGamePadDPadUpdateDelegate;
                application.GamePadStickUpdate -= m_OnGamePadStickUpdateDelegate;
                application.GamePadTriggerUpdate -= m_OnGamePadTriggerUpdateDelegate;
            }
            base.OnExit();
        }

        public override void OnEnterTransitionDidFinish()
        {
            if (m_bIsAccelerometerEnabled)
            {
                //CCDirector.sharedDirector().Accelerometer.setDelegate(this);
            }

            base.OnEnterTransitionDidFinish();
        }

        public virtual void RegisterWithTouchDispatcher()
        {
            CCTouchDispatcher pDispatcher = CCDirector.SharedDirector.TouchDispatcher;

            /*
            if (m_pScriptHandlerEntry)
            {
                if (m_pScriptHandlerEntry->isMultiTouches())
                {
                    pDispatcher->addStandardDelegate(this, 0);
                    LUALOG("[LUA] Add multi-touches event handler: %d", m_pScriptHandlerEntry->getHandler());
                }
                else
                {
                    pDispatcher->addTargetedDelegate(this,
								         m_pScriptHandlerEntry->getPriority(),
								         m_pScriptHandlerEntry->getSwallowsTouches());
                    LUALOG("[LUA] Add touch event handler: %d", m_pScriptHandlerEntry->getHandler());
                }
                return;
            }
            */
            if (m_bIsSingleTouchEnabled)
            {
                pDispatcher.AddTargetedDelegate(this, 0, true);
            }
            if (m_bIsMultiTouchEnabled)
            {
            pDispatcher.AddStandardDelegate(this, 0);
            }
        }

        public virtual bool SingleTouchEnabled
        {
            get { return m_bIsSingleTouchEnabled; }
            set
            {
                if (m_bIsSingleTouchEnabled != value)
                {
                    m_bIsSingleTouchEnabled = value;

                    if (m_bIsRunning)
                    {
                        if (value)
                        {
                            RegisterWithTouchDispatcher();
                        }
                        else
                        {
                            CCDirector.SharedDirector.TouchDispatcher.RemoveDelegate(this);
                        }
                    }
                }
            }
        }
        public virtual bool TouchEnabled
        {
            get { return m_bIsMultiTouchEnabled; }
            set
            {
                if (m_bIsMultiTouchEnabled != value)
                {
                    m_bIsMultiTouchEnabled = value;

                    if (m_bIsRunning)
                    {
                        if (value)
                        {
                            RegisterWithTouchDispatcher();
                        }
                        else
                        {
                            CCDirector.SharedDirector.TouchDispatcher.RemoveDelegate(this);
                        }
                    }
                }
            }
        }

        public bool AccelerometerEnabled
        {
            get { 
#if !PSM
				return m_bIsAccelerometerEnabled; 
#else
				return(false);
#endif
			}
            set {
#if !PSM &&!NETFX_CORE
                if (value != m_bIsAccelerometerEnabled)
                {
                    m_bIsAccelerometerEnabled = value;

                    if (m_bIsRunning)
                    {
                        CCDirector pDirector = CCDirector.SharedDirector;
                        pDirector.Accelerometer.SetDelegate(value ? this : null);
                    }
                }
#else
                m_bIsAccelerometerEnabled = false;
#endif
			}
        }

        public bool KeypadEnabled
        {
            get { return m_bKeypadEnabled; }
            set
            {
                if (value != m_bKeypadEnabled)
                {
                    m_bKeypadEnabled = value;
                    
                    if (m_bIsRunning)
                    {
                        /*
                        if (value)
                        {
                            throw new NotImplementedException();
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                         */
                    }
                }
            }
        }
        public bool GamePadEnabled
        {
            get { return (m_bGamePadEnabled); }
            set
            {
                if (value != m_bGamePadEnabled)
                {
                    m_bGamePadEnabled = value;
                }
                if (value && !CCDirector.SharedDirector.GamePadEnabled)
                {
                    CCDirector.SharedDirector.GamePadEnabled = true;
                }
            }
        }

        #region touches

        #region ICCStandardTouchDelegate Members

        public virtual void TouchesBegan(List<CCTouch> touches, CCEvent event_)
        {
        }

        public virtual void TouchesMoved(List<CCTouch> touches, CCEvent event_)
        {
        }

        public virtual void TouchesEnded(List<CCTouch> touches, CCEvent event_)
        {
        }

        public virtual void TouchesCancelled(List<CCTouch> touches, CCEvent event_)
        {
        }

        #endregion

        #region ICCTargetedTouchDelegate Members

        public virtual bool TouchBegan(CCTouch touch, CCEvent event_)
        {
            return true;
        }

        public virtual void TouchMoved(CCTouch touch, CCEvent event_)
        {
        }

        public virtual void TouchEnded(CCTouch touch, CCEvent event_)
        {
        }

        public virtual void TouchCancelled(CCTouch touch, CCEvent event_)
        {
        }

        #endregion

        #endregion

        public virtual void DidAccelerate(CCAcceleration pAccelerationValue)
        {
        }

        public virtual void KeyBackClicked()
        {
        }

        public virtual void KeyMenuClicked()
        {
        }

        #region GamePad Support
        private CCGamePadButtonDelegate m_OnGamePadButtonUpdateDelegate;
        private CCGamePadConnectionDelegate m_OnGamePadConnectionUpdateDelegate;
        private CCGamePadDPadDelegate m_OnGamePadDPadUpdateDelegate;
        private CCGamePadStickUpdateDelegate m_OnGamePadStickUpdateDelegate;
        private CCGamePadTriggerDelegate m_OnGamePadTriggerUpdateDelegate;

        protected virtual void OnGamePadTriggerUpdate(float leftTriggerStrength, float rightTriggerStrength, Microsoft.Xna.Framework.PlayerIndex player)
        {
        }

        protected virtual void OnGamePadStickUpdate(CCGameStickStatus leftStick, CCGameStickStatus rightStick, Microsoft.Xna.Framework.PlayerIndex player)
        {
        }

        protected virtual void OnGamePadDPadUpdate(CCGamePadButtonStatus leftButton, CCGamePadButtonStatus upButton, CCGamePadButtonStatus rightButton, CCGamePadButtonStatus downButton, Microsoft.Xna.Framework.PlayerIndex player)
        {
            if (!HasFocus)
            {
                return;
            }
        }

        protected virtual void OnGamePadConnectionUpdate(Microsoft.Xna.Framework.PlayerIndex player, bool IsConnected)
        {
        }

        protected virtual void OnGamePadButtonUpdate(CCGamePadButtonStatus backButton, CCGamePadButtonStatus startButton, CCGamePadButtonStatus systemButton, CCGamePadButtonStatus aButton, CCGamePadButtonStatus bButton, CCGamePadButtonStatus xButton, CCGamePadButtonStatus yButton, CCGamePadButtonStatus leftShoulder, CCGamePadButtonStatus rightShoulder, Microsoft.Xna.Framework.PlayerIndex player)
        {
        }
        #endregion
    }
}