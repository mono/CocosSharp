using System;
using System.Collections.Generic;
using System.IO;

namespace Cocos2D
{
    /// <summary>
    /// This is the input sensitive version of the scene. This class reacts to touches
    /// and gamepad input.
    /// </summary>
    public class CCInputScene : CCScene, ICCTargetedTouchDelegate, ICCStandardTouchDelegate, ICCAccelerometerDelegate, CCKeypadDelegate
    {
        private bool m_bIsAccelerometerEnabled;
        private bool m_bKeypadEnabled;
        private bool m_bGamePadEnabled;
        private bool m_bIsMultiTouchEnabled;
        private bool m_bIsSingleTouchEnabled;

        public CCInputScene()
        {
            m_OnGamePadButtonUpdateDelegate = new CCGamePadButtonDelegate(OnGamePadButtonUpdate);
            m_OnGamePadConnectionUpdateDelegate = new CCGamePadConnectionDelegate(OnGamePadConnectionUpdate);
            m_OnGamePadDPadUpdateDelegate = new CCGamePadDPadDelegate(OnGamePadDPadUpdate);
            m_OnGamePadStickUpdateDelegate = new CCGamePadStickUpdateDelegate(OnGamePadStickUpdate);
            m_OnGamePadTriggerUpdateDelegate = new CCGamePadTriggerDelegate(OnGamePadTriggerUpdate);
        }
        private bool m_bDidInit = false;

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
            if (!m_bDidInit)
            {
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
            get
            {
#if !PSM
                return m_bIsAccelerometerEnabled;
#else
				return(false);
#endif
            }
            set
            {
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
