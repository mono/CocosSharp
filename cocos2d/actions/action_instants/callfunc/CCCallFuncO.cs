using System;

namespace CocosSharp
{
    public class CCCallFuncO : CCCallFunc
    {
        private Action<object> m_pCallFuncO;
        private object m_pObject;

        public object Object
        {
            get { return m_pObject; }
            set { m_pObject = value; }
        }


        #region Constructors

        public CCCallFuncO()
        {
            m_pObject = null;
            m_pCallFuncO = null;
        }

        public CCCallFuncO(Action<object> selector, object pObject) : this()
        {
            InitCCCallFuncO(selector, pObject);
        }

        protected CCCallFuncO(CCCallFuncO callFuncO) : base(callFuncO)
        {
            InitCCCallFuncO(callFuncO.m_pCallFuncO, callFuncO.m_pObject);
        }

        private void InitCCCallFuncO(Action<object> selector, object pObject)
        {
            m_pObject = pObject;
            m_pCallFuncO = selector;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCCallFuncO(this);
        }

        public override void Execute()
        {
            if (null != m_pCallFuncO)
            {
                m_pCallFuncO(m_pObject);
            }

            //if (CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()) {
            //    CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()->executeCallFunc0(
            //            m_scriptFuncName.c_str(), m_pObject);
            //}
        }
    }
}