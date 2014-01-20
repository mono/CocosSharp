using System;

namespace CocosSharp
{
    public class CCCallFuncN : CCCallFunc
    {
        private Action<CCNode> m_pCallFuncN;


        #region Constructors

        public CCCallFuncN() : base()
        {
        }

        public CCCallFuncN(Action<CCNode> selector) : base()
        {
            InitCCCallFuncN(selector);
        }

        public CCCallFuncN(CCCallFuncN callFuncN) : base(callFuncN)
        {
            InitCCCallFuncN(callFuncN.m_pCallFuncN);
        }

        private void InitCCCallFuncN(Action<CCNode> selector)
        {
            m_pCallFuncN = selector;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCCallFuncN(this);
        }

        public override void Execute()
        {
            if (null != m_pCallFuncN)
            {
                m_pCallFuncN(m_pTarget);
            }
            //if (m_nScriptHandler) {
            //    CCScriptEngineManager::sharedManager()->getScriptEngine()->executeFunctionWithobject(m_nScriptHandler, m_pTarget, "CCNode");
            //}
        }
    }
}