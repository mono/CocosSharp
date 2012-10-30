using System.Collections.Generic;
using System.Linq;

namespace cocos2d
{
    public class CCTouchDelegate : ICCTouchDelegate
    {
        protected Dictionary<int, string> m_pEventTypeFuncMap;

        /// <summary>
        /// ! call the release() in child(layer or menu)
        /// </summary>
        public virtual void Destroy()
        {
        }

        /// <summary>
        /// ! call the retain() in child (layer or menu)
        /// </summary>
        public virtual void Keep()
        {
        }

        /// <summary>
        /// functions for script call back
        /// </summary>
        public void RegisterScriptTouchHandler(int eventType, string pszScriptFunctionName)
        {
            if (m_pEventTypeFuncMap == null)
            {
                m_pEventTypeFuncMap = new Dictionary<int, string>();
            }

            (m_pEventTypeFuncMap)[eventType] = pszScriptFunctionName;
        }

        public bool IsScriptHandlerExist(int eventType)
        {
            if (m_pEventTypeFuncMap != null)
            {
                return (m_pEventTypeFuncMap)[eventType].Count() != 0;
            }

            return false;
        }

        public void ExcuteScriptTouchHandler(int eventType, CCTouch pTouch)
        {
            if (m_pEventTypeFuncMap != null && CCScriptEngineManager.SharedScriptEngineManager.ScriptEngine != null)
            {
                CCScriptEngineManager.SharedScriptEngineManager.ScriptEngine.ExecuteTouchEvent((m_pEventTypeFuncMap)[eventType],
                                                                                                 pTouch);
            }
        }

        public void ExcuteScriptTouchesHandler(int eventType, List<CCTouch> pTouches)
        {
            if (m_pEventTypeFuncMap != null && CCScriptEngineManager.SharedScriptEngineManager.ScriptEngine != null)
            {
                CCScriptEngineManager.SharedScriptEngineManager.ScriptEngine.ExecuteTouchesEvent((m_pEventTypeFuncMap)[eventType],
                                                                                                   pTouches);
            }
        }
    }
}