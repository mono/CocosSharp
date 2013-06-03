using System;

namespace Cocos2D
{
    public class CCCallFuncND : CCCallFuncN
    {
        protected Action<CCNode,object> m_pCallFuncND;
        protected object m_pData;

		public CCCallFuncND (Action<CCNode,object> selector, object d) : base()
        {
            InitWithTarget(selector, d);
        }

		public CCCallFuncND (CCCallFuncND callFuncND) : base (callFuncND)
		{
			InitWithTarget(callFuncND.m_pCallFuncND, callFuncND.m_pData);
		}

		public bool InitWithTarget(Action<CCNode,object> selector, object d)
        {
            m_pData = d;
            m_pCallFuncND = selector;
            return true;
        }

        public override object Copy(ICCCopyable zone)
        {

            if (zone != null)
            {
                //in case of being called at sub class
                var pRet = (CCCallFuncND) (zone);
				base.Copy(zone);
				pRet.InitWithTarget(m_pCallFuncND, m_pData);
				return pRet;
			}
            else
            {
                return new CCCallFuncND(this);
            }

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