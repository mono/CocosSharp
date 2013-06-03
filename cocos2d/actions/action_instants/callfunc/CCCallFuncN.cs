using System;

namespace Cocos2D
{
    public class CCCallFuncN : CCCallFunc
    {
        private Action<CCNode> m_pCallFuncN;

        public CCCallFuncN() : base()
        {
            m_pCallFuncN = null;
        }


        public CCCallFuncN (Action<CCNode> selector)
        {
            InitWithTarget(selector);
        }

		public CCCallFuncN (CCCallFuncN callFuncN) : base (callFuncN)
		{
			InitWithTarget(callFuncN.m_pCallFuncN);
		}

		public bool InitWithTarget(Action<CCNode> selector)
        {
            m_pCallFuncN = selector;
            return false;
        }

        public override object Copy(ICCCopyable zone)
        {
            if (zone != null)
            {
                //in case of being called at sub class
                var pRet = (CCCallFuncN) (zone);
				base.Copy(zone);
				
				pRet.InitWithTarget(m_pCallFuncN);
				
				return pRet;
			}
            else
            {
                return new CCCallFuncN(this);
			}

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