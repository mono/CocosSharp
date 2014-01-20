using System;

namespace CocosSharp
{
    public class CCCallFunc : CCActionInstant
    {
        private Action m_pCallFunc;
        protected string m_scriptFuncName;


        #region Constructors

        public CCCallFunc()
        {
            m_scriptFuncName = "";
            m_pCallFunc = null;
        }

        public CCCallFunc(Action selector) : base()
        {
            m_pCallFunc = selector;
        }

        protected CCCallFunc(CCCallFunc callFunc) : base(callFunc)
        {
            m_pCallFunc = callFunc.m_pCallFunc;
            m_scriptFuncName = callFunc.m_scriptFuncName;
        }

        #endregion Constructors


        public virtual void Execute()
        {
            if (null != m_pCallFunc)
            {
                m_pCallFunc();
            }
            //if (m_nScriptHandler) {
            //    CCScriptEngineManager::sharedManager()->getScriptEngine()->executeCallFuncActionEvent(this);
            //}
        }

        public override void Update(float time)
        {
            Execute();
        }

        public override object Copy(ICCCopyable pZone)
        {
            return new CCCallFunc(this);
        }
    }
}