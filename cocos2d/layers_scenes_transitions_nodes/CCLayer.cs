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

namespace cocos2d
{
    public class CCLayer : CCNode, ICCTargetedTouchDelegate, ICCStandardTouchDelegate, ICCAccelerometerDelegate
    {
        private bool m_bIsAccelerometerEnabled;
        private bool m_bKeypadEnabled;
        private bool m_bIsTouchEnabled;
        //private bool m_bMouseEnabled;
        //private bool m_bGamePadEnabled;

        public CCLayer()
        {
            AnchorPoint = CCPointExtension.ccp(0.5f, 0.5f);
            m_bIgnoreAnchorPointForPosition = true;
            CCDirector director = CCDirector.SharedDirector;
            if (director != null)
            {
                ContentSize = director.WinSize;
            }
        }

        public new static CCLayer Create()
        {
            var ret = new CCLayer();
            if (ret.Init())
            {
                return ret;
            }

            return null;
        }

        public virtual bool Init()
        {
            bool bRet = false;
            do
            {
                CCDirector director = CCDirector.SharedDirector;
                if (director == null)
                {
                    break;
                }

//                ContentSize = director.WinSize;
                m_bIsTouchEnabled = false;
                m_bIsAccelerometerEnabled = false;

                bRet = true;
            } while (false);

            return bRet;
        }

        public override void OnEnter()
        {
            // register 'parent' nodes first
            // since events are propagated in reverse order
            if (m_bIsTouchEnabled)
            {
                RegisterWithTouchDispatcher();
            }

            // then iterate over all the children
            base.OnEnter();

            CCDirector director = CCDirector.SharedDirector;

            // add this layer to concern the Accelerometer Sensor
            if (m_bIsAccelerometerEnabled)
            {
#if !PSM
                director.Accelerometer.SetDelegate(this);
#endif
			}

            // add this layer to concern the kaypad msg
            if (m_bKeypadEnabled)
            {
                //director.KeypadDispatcher.addDelegate(this);
            }
        }

        public override void OnExit()
        {
            CCDirector director = CCDirector.SharedDirector;

            if (m_bIsTouchEnabled)
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
                //director.KeypadDispatcher.removeDelegate(this);
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

            pDispatcher.AddStandardDelegate(this, 0);
        }

        public virtual bool TouchEnabled
        {
            get { return m_bIsTouchEnabled; }
            set
            {
                if (m_bIsTouchEnabled != value)
                {
                    m_bIsTouchEnabled = value;

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
            set
            {
#if !PSM
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
                        if (value)
                        {
                            throw new NotImplementedException();
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
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
    }
}