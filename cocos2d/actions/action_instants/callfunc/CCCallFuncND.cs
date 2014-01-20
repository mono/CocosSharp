using System;

namespace CocosSharp
{
    public class CCCallFuncND : CCCallFuncN
    {
        protected Action<CCNode, object> m_pCallFuncND;
        protected object m_pData;


        #region Constructors

        public CCCallFuncND(Action<CCNode, object> selector, object d) : base()
        {
            InitCCCallFuncND(selector, d);
        }

        public CCCallFuncND(CCCallFuncND callFuncND) : base(callFuncND)
        {
            InitCCCallFuncND(callFuncND.m_pCallFuncND, callFuncND.m_pData);
        }

        private void InitCCCallFuncND(Action<CCNode, object> selector, object d)
        {
            m_pData = d;
            m_pCallFuncND = selector;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCCallFuncND(this);
        }

        public override void Execute()
        {
            if (null != m_pCallFuncND)
            {
                m_pCallFuncND(m_pTarget, m_pData);
            }

            //if (CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()) {
            //    CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine()->executeCallFuncND(
            //            m_scriptFuncName.c_str(), m_pTarget, m_pData);
            //}
        }
    }
}