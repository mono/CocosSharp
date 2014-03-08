using System.Collections.Generic;
using System.Linq;

namespace CocosSharp
{
    public class CCTouchDelegate : ICCTouchDelegate
    {
		protected Dictionary<int, string> EventTypeFuncMap { get; set; }

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
        public void RegisterScriptTouchHandler(int eventType, string scriptFunctionName)
        {
            if (EventTypeFuncMap == null)
            {
                EventTypeFuncMap = new Dictionary<int, string>();
            }

            (EventTypeFuncMap)[eventType] = scriptFunctionName;
        }

        public bool IsScriptHandlerExist(int eventType)
        {
            if (EventTypeFuncMap != null)
            {
#if NETFX_CORE
                return this.m_pEventTypeFuncMap != null && this.m_pEventTypeFuncMap[eventType].Length != 0;
#else
                return (EventTypeFuncMap)[eventType].Count() != 0;
#endif
            }

            return false;
        }

        public void ExcuteScriptTouchHandler(int eventType, CCTouch touch)
        {
            if (EventTypeFuncMap != null && CCScriptEngineManager.SharedScriptEngineManager.ScriptEngine != null)
            {
                CCScriptEngineManager.SharedScriptEngineManager.ScriptEngine.ExecuteTouchEvent((EventTypeFuncMap)[eventType],
                                                                                                 touch);
            }
        }

        public void ExcuteScriptTouchesHandler(int eventType, List<CCTouch> touches)
        {
            if (EventTypeFuncMap != null && CCScriptEngineManager.SharedScriptEngineManager.ScriptEngine != null)
            {
                CCScriptEngineManager.SharedScriptEngineManager.ScriptEngine.ExecuteTouchesEvent((EventTypeFuncMap)[eventType],
                                                                                                   touches);
            }
        }
    }
}